using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientHistoricalTradesTests
{
    [Fact]
    public async Task UsdHistoricalTrades_UsesCurrentApiKeyOnlyContractAndCompleteResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [{
              "id": 9223372036854775806,
              "price": "4.00000100",
              "qty": "12.00000000",
              "quoteQty": "8000.00",
              "time": 1499865549590,
              "isBuyerMaker": true,
              "isRPITrade": true
            }]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetHistoricalTradesAsync(
            "BTCUSDT",
            500,
            9223372036854775806);

        Assert.True(result.Success);
        AssertApiKeyOnlyRequest(handler, limiter, "/fapi/v1/historicalTrades");
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("limit=500", query);
        Assert.Contains("fromId=9223372036854775806", query);

        var trade = Assert.Single(result.Data);
        Assert.Equal(9223372036854775806, trade.Id);
        Assert.Equal(4.00000100m, trade.Price);
        Assert.Equal(12.00000000m, trade.Quantity);
        Assert.Equal(8000.00m, trade.QuoteQuantity);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1499865549590).UtcDateTime, trade.Timestamp);
        Assert.True(trade.IsBuyerMaker);
        Assert.True(trade.IsRpiTrade);
    }

    [Fact]
    public async Task CoinHistoricalTrades_UsesCurrentApiKeyOnlyContractAndCompleteResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [{
              "id": 595103,
              "price": "9642.2",
              "qty": "1",
              "baseQty": "0.01037108",
              "time": 1499865549590,
              "isBuyerMaker": true
            }]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetHistoricalTradesAsync(
            "BTCUSD_PERP",
            500,
            595103);

        Assert.True(result.Success);
        AssertApiKeyOnlyRequest(handler, limiter, "/dapi/v1/historicalTrades");
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("symbol=BTCUSD_PERP", query);
        Assert.Contains("limit=500", query);
        Assert.Contains("fromId=595103", query);

        var trade = Assert.Single(result.Data);
        Assert.Equal(595103, trade.Id);
        Assert.Equal(9642.2m, trade.Price);
        Assert.Equal(1m, trade.Quantity);
        Assert.Equal(0.01037108m, trade.BaseQuantity);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1499865549590).UtcDateTime, trade.Timestamp);
        Assert.True(trade.IsBuyerMaker);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task HistoricalTrades_RejectsMissingSymbol(string? symbol)
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetHistoricalTradesAsync(symbol!));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetHistoricalTradesAsync(symbol!));
        Assert.Null(handler.RequestUri);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public async Task HistoricalTrades_RejectsLimitOutsideCurrentRange(int limit)
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetHistoricalTradesAsync("BTCUSDT", limit));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetHistoricalTradesAsync("BTCUSD_PERP", limit));
        Assert.Null(handler.RequestUri);
    }

    private static void AssertApiKeyOnlyRequest(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        string path)
    {
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.DoesNotContain("timestamp=", handler.RequestUri.Query);
        Assert.DoesNotContain("signature=", handler.RequestUri.Query);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values));
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == path && request.Weight == 200 && !request.Signed);
    }

    private static BinanceRestApiClient CreateClient(
        RecordingHttpMessageHandler handler,
        IRateLimiter? limiter = null)
    {
        var options = new BinanceRestApiClientOptions(new ApiCredentials("api-key", "api-secret"))
        {
            AutoTimestamp = false,
            HttpClient = new HttpClient(handler),
            RateLimiterEnabled = limiter != null
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
