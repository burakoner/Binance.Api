using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientCoinAccountTests
{
    [Fact]
    public async Task PairBrackets_UseCurrentV1SignedContractAndDeserializeResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "pair": "BTCUSD",
                "brackets": [
                  {
                    "bracket": 9223372036854775807,
                    "initialLeverage": 9223372036854775806,
                    "qtyCap": 9223372036854775805,
                    "qtylFloor": 9223372036854775804,
                    "maintMarginRatio": 0.004,
                    "cum": 12.5
                  }
                ]
              }
            ]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetPairBracketsAsync("BTCUSD", 60_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/dapi/v1/leverageBracket", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));

        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("pair=BTCUSD", query);
        Assert.DoesNotContain("symbol=", query);
        Assert.Contains("recvWindow=60000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item =>
            item.Endpoint == "/dapi/v1/leverageBracket" && item.Weight == 1 && item.Signed);

        var pair = Assert.Single(result.Data!);
        Assert.Equal("BTCUSD", pair.Pair);
        var bracket = Assert.Single(pair.Brackets);
        Assert.Equal(long.MaxValue, bracket.Bracket);
        Assert.Equal(long.MaxValue - 1, bracket.InitialLeverage);
        Assert.Equal(long.MaxValue - 2, bracket.Cap);
        Assert.Equal(long.MaxValue - 3, bracket.Floor);
        Assert.Equal(0.004m, bracket.MaintenanceMarginRatio);
        Assert.Equal(12.5m, bracket.MaintAmount);
    }

    [Fact]
    public async Task SymbolBrackets_UseCurrentV2SymbolContractAndWeight()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "symbol": "BTCUSD_PERP",
                "notionalCoef": 1.5,
                "brackets": [
                  {
                    "bracket": 1,
                    "initialLeverage": 125,
                    "qtyCap": 50,
                    "qtylFloor": 7,
                    "maintMarginRatio": 0.004,
                    "cum": 0
                  }
                ]
              }
            ]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetBracketsAsync("BTCUSD_PERP", 45_000);

        Assert.True(result.Success);
        Assert.Equal("/dapi/v2/leverageBracket", handler.RequestUri!.AbsolutePath);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("symbol=BTCUSD_PERP", query);
        Assert.DoesNotContain("pair=", query);
        Assert.Contains("recvWindow=45000", query);
        Assert.Contains(limiter.Requests, item =>
            item.Endpoint == "/dapi/v2/leverageBracket" && item.Weight == 1 && item.Signed);

        var symbol = Assert.Single(result.Data!);
        Assert.Equal("BTCUSD_PERP", symbol.Symbol);
        Assert.Equal(1.5m, symbol.NotionalCoef);
        Assert.Equal(7, Assert.Single(symbol.Brackets).Floor);
    }

    [Fact]
    public async Task SymbolBrackets_WithoutSymbolUseWeightTwoAndConfiguredReceiveWindow()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(
            handler,
            limiter,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(5_000));

        var result = await client.CoinFutures.GetBracketsAsync();

        Assert.True(result.Success);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.DoesNotContain("symbol=", query);
        Assert.Contains("recvWindow=5000", query);
        Assert.Contains(limiter.Requests, item =>
            item.Endpoint == "/dapi/v2/leverageBracket" && item.Weight == 2 && item.Signed);
    }

    [Fact]
    public async Task BracketQueries_RejectReceiveWindowAboveCurrentMaximum()
    {
        var explicitHandler = new RecordingHttpMessageHandler("[]");
        using (var explicitClient = CreateClient(explicitHandler))
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.CoinFutures.GetPairBracketsAsync(receiveWindow: 60_001));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.CoinFutures.GetBracketsAsync(receiveWindow: 60_001));
        }
        Assert.Null(explicitHandler.RequestUri);

        var configuredHandler = new RecordingHttpMessageHandler("[]");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.CoinFutures.GetPairBracketsAsync());
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.CoinFutures.GetBracketsAsync());
        Assert.Null(configuredHandler.RequestUri);
    }

    private static BinanceRestApiClient CreateClient(
        RecordingHttpMessageHandler handler,
        IRateLimiter? limiter = null,
        TimeSpan? defaultReceiveWindow = null)
    {
        var options = new BinanceRestApiClientOptions(new ApiCredentials("api-key", "api-secret"))
        {
            AutoTimestamp = false,
            HttpClient = new HttpClient(handler),
            RateLimiterEnabled = limiter != null,
            ReceiveWindow = defaultReceiveWindow
        };
#pragma warning disable CS0612
        options.RateLimiters = limiter == null ? [] : [limiter];
#pragma warning restore CS0612
        return new BinanceRestApiClient(options);
    }

    private sealed class RecordingRateLimiter : IRateLimiter
    {
        internal List<(string Endpoint, int Weight, bool Signed)> Requests { get; } = [];

        public Task<CallResult<int>> LimitRequestAsync(
            ILogger logger,
            string endpoint,
            HttpMethod method,
            bool signed,
            SensitiveString? apiKey,
            RateLimitingBehavior limitBehaviour,
            int requestWeight,
            CancellationToken ct)
        {
            Requests.Add((endpoint, requestWeight, signed));
            return Task.FromResult(new CallResult<int>(0));
        }
    }
}
