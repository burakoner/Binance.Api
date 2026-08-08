using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginPreventedMatchTests
{
    [Fact]
    public async Task PreventedMatches_ByPreventedMatchId_UsesCurrentSignedContract()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [{
              "symbol":"BTCUSDT","preventedMatchId":3000000001,"takerOrderId":3000000002,
              "makerSymbol":"BTCUSDC","makerOrderId":3000000003,"tradeGroupId":3000000004,
              "selfTradePreventionMode":"EXPIRE_MAKER","price":"65000.12500000",
              "makerPreventedQuantity":"0.01250000","transactTime":1691116657000
            }]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.Margin.GetMarginPreventedMatchesAsync(
            "BTCUSDT",
            preventedMatchId: 3_000_000_001,
            isIsolated: true,
            receiveWindow: 5_000);

        Assert.True(result.Success);
        var match = Assert.Single(result.Data);
        Assert.Equal(3_000_000_001, match.PreventedMatchId);
        Assert.Equal(3_000_000_002, match.TakerOrderId);
        Assert.Equal("BTCUSDC", match.MakerSymbol);
        Assert.Equal(3_000_000_003, match.MakerOrderId);
        Assert.Equal(3_000_000_004, match.TradeGroupId);
        Assert.Equal(BinanceSelfTradePreventionMode.ExpireMaker, match.SelfTradePreventionMode);
        Assert.Equal(65_000.125m, match.Price);
        Assert.Equal(0.0125m, match.MakerPreventedQuantity);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_691_116_657_000).UtcDateTime, match.TransactionTime);

        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/sapi/v1/margin/myPreventedMatches", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("preventedMatchId=3000000001", query);
        Assert.Contains("isIsolated=TRUE", query);
        Assert.Contains("recvWindow=5000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.DoesNotContain("limit=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/myPreventedMatches" && item.Weight == 10);
    }

    [Fact]
    public async Task PreventedMatches_ByOrderId_UsesFromPreventedMatchIdAsCursor()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        var result = await client.Margin.GetMarginPreventedMatchesAsync(
            "ETHUSDT",
            orderId: 3_000_000_005,
            fromPreventedMatchId: 3_000_000_006);

        Assert.True(result.Success);
        Assert.Empty(result.Data);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("orderId=3000000005", query);
        Assert.Contains("fromPreventedMatchId=3000000006", query);
        Assert.DoesNotContain("preventedMatchId=", query);
        Assert.DoesNotContain("isIsolated=", query);
        Assert.DoesNotContain("limit=", query);
    }

    [Fact]
    public async Task PreventedMatches_RejectsUnsupportedParameterCombinations()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("[]"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginPreventedMatchesAsync("BTCUSDT"));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginPreventedMatchesAsync(
            "BTCUSDT", preventedMatchId: 1, orderId: 2));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginPreventedMatchesAsync(
            "BTCUSDT", fromPreventedMatchId: 1));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginPreventedMatchesAsync(
            " ", orderId: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginPreventedMatchesAsync(
            "BTCUSDT", orderId: 1, receiveWindow: 60_001));
    }

    private static BinanceRestApiClient CreateClient(RecordingHttpMessageHandler handler, IRateLimiter? limiter = null)
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
        public List<(string Endpoint, int Weight)> Requests { get; } = [];

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
            Requests.Add((endpoint, requestWeight));
            return Task.FromResult(new CallResult<int>(0));
        }
    }
}
