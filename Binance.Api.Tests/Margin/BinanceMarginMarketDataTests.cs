using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginMarketDataTests
{
    [Fact]
    public async Task LimitPriceAndRiskQueries_UseCurrentMarketDataContracts()
    {
        var limitLimiter = new RecordingRateLimiter();
        var limitHandler = new RecordingHttpMessageHandler("""{"crossMarginSymbols":["BLURUSDC"]}""");
        using (var client = CreateClient(limitHandler, limitLimiter))
        {
            var result = await client.Margin.GetMarginLimitPricePairsAsync();

            Assert.True(result.Success);
            Assert.Equal("BLURUSDC", Assert.Single(result.Data.CrossMarginSymbols));
            AssertMarketDataRequest(limitHandler, "/sapi/v1/margin/limit-price-pairs");
            Assert.Contains(limitLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/limit-price-pairs" && item.Weight == 1);
        }

        var ratioLimiter = new RecordingRateLimiter();
        var ratioHandler = new RecordingHttpMessageHandler("""[{"asset":"USDC","riskBasedLiquidationRatio":"0.01"}]""");
        using (var client = CreateClient(ratioHandler, ratioLimiter))
        {
            var result = await client.Margin.GetMarginRiskBasedLiquidationRatiosAsync();

            Assert.True(result.Success);
            var ratio = Assert.Single(result.Data);
            Assert.Equal("USDC", ratio.Asset);
            Assert.Equal(0.01m, ratio.RiskBasedLiquidationRatio);
            AssertMarketDataRequest(ratioHandler, "/sapi/v1/margin/risk-based-liquidation-ratio");
            Assert.Contains(ratioLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/risk-based-liquidation-ratio" && item.Weight == 1);
        }

        var restrictedLimiter = new RecordingRateLimiter();
        var restrictedHandler = new RecordingHttpMessageHandler(
            """{"openLongRestrictedAsset":["ADA"],"maxCollateralExceededAsset":["ACH"]}""");
        using (var client = CreateClient(restrictedHandler, restrictedLimiter))
        {
            var result = await client.Margin.GetMarginRestrictedAssetsAsync();

            Assert.True(result.Success);
            Assert.Equal("ADA", Assert.Single(result.Data.OpenLongRestrictedAssets));
            Assert.Equal("ACH", Assert.Single(result.Data.MaxCollateralExceededAssets));
            AssertMarketDataRequest(restrictedHandler, "/sapi/v1/margin/restricted-asset");
            Assert.Contains(restrictedLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/restricted-asset" && item.Weight == 1);
        }
    }

    [Fact]
    public async Task ListSchedule_FollowsCurrentMarketDataConnectorContract()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """[{"listTime":1686161202000,"crossMarginAssets":["BTC"],"isolatedMarginSymbols":["ADAUSDT"]}]""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Margin.GetMarginListScheduleAsync(5_000);

        Assert.True(result.Success);
        var entry = Assert.Single(result.Data);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_686_161_202_000).UtcDateTime, entry.ListTime);
        Assert.Equal("BTC", Assert.Single(entry.CrossMarginAssets));
        Assert.Equal("ADAUSDT", Assert.Single(entry.IsolatedMarginSymbols));
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/sapi/v1/margin/list-schedule", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("recvWindow=5000", query);
        Assert.DoesNotContain("timestamp=", query);
        Assert.DoesNotContain("signature=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/list-schedule" && item.Weight == 100);
    }

    [Fact]
    public async Task ListSchedule_RejectsReceiveWindowAboveCurrentMaximum()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("[]"));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginListScheduleAsync(60_001));
    }

    private static void AssertMarketDataRequest(RecordingHttpMessageHandler handler, string path)
    {
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        Assert.DoesNotContain("timestamp=", handler.RequestUri.Query);
        Assert.DoesNotContain("signature=", handler.RequestUri.Query);
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
