using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Margin;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginMarketDataContractTests
{
    [Fact]
    public async Task PublicMarketData_UsesCurrentFiltersWeightsAndModels()
    {
        var collateralLimiter = new RecordingRateLimiter();
        var collateralHandler = new RecordingHttpMessageHandler(
            """[{"collaterals":[{"minUsdValue":"0","maxUsdValue":"13000000","discountRate":"1"}],"assetNames":["BNX"]}]""");
        using (var client = CreateClient(collateralHandler, collateralLimiter))
        {
            var result = await client.Margin.GetCrossMarginCollateralRatioAsync();

            Assert.True(result.Success);
            var ratio = Assert.Single(result.Data);
            Assert.Equal("BNX", Assert.Single(ratio.AssetNames));
            Assert.Equal(13_000_000m, Assert.Single(ratio.Collaterals).MaxUsdValue);
            AssertUnsignedMarketDataRequest(collateralHandler, "/sapi/v1/margin/crossMarginCollateralRatio");
            Assert.Empty(collateralHandler.RequestUri!.Query);
            Assert.Contains(collateralLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/crossMarginCollateralRatio" && item.Weight == 100);
        }

        var pairLimiter = new RecordingRateLimiter();
        var pairHandler = new RecordingHttpMessageHandler(
            """[{"base":"BNB","id":351637150141315840,"isBuyAllowed":true,"isMarginTrade":true,"isSellAllowed":true,"quote":"BTC","symbol":"BNBBTC","delistTime":1704973040}]""");
        using (var client = CreateClient(pairHandler, pairLimiter))
        {
            var result = await client.Margin.GetMarginSymbolsAsync("BNBBTC");

            Assert.True(result.Success);
            var pair = Assert.Single(result.Data);
            Assert.Equal(351_637_150_141_315_840, pair.Id);
            Assert.Equal("BNB", pair.BaseAsset);
            Assert.Equal("BTC", pair.QuoteAsset);
            Assert.Equal(1_704_973_040, pair.DelistTime);
            AssertUnsignedMarketDataRequest(pairHandler, "/sapi/v1/margin/allPairs");
            Assert.Contains("symbol=BNBBTC", Uri.UnescapeDataString(pairHandler.RequestUri!.Query));
            Assert.Contains(pairLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/allPairs" && item.Weight == 1);
        }

        var assetLimiter = new RecordingRateLimiter();
        var assetHandler = new RecordingHttpMessageHandler(
            """[{"assetFullName":"USD coin","assetName":"USDC","isBorrowable":true,"isMortgageable":true,"userMinBorrow":"0.01","userMinRepay":"0.02","delistTime":1704973040}]""");
        using (var client = CreateClient(assetHandler, assetLimiter))
        {
            var result = await client.Margin.GetMarginAssetsAsync("USDC");

            Assert.True(result.Success);
            var asset = Assert.Single(result.Data);
            Assert.Equal(0.01m, asset.MinimalBorrowQuantity);
            Assert.Equal(0.02m, asset.MinimalRepayQuantity);
            Assert.Equal(1_704_973_040, asset.DelistTime);
            AssertUnsignedMarketDataRequest(assetHandler, "/sapi/v1/margin/allAssets");
            Assert.Contains("asset=USDC", Uri.UnescapeDataString(assetHandler.RequestUri!.Query));
            Assert.Contains(assetLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/allAssets" && item.Weight == 1);
        }

        var priceLimiter = new RecordingRateLimiter();
        var priceHandler = new RecordingHttpMessageHandler(
            """{"calcTime":1562046418000,"price":"0.00333930","symbol":"BNBBTC"}""");
        using (var client = CreateClient(priceHandler, priceLimiter))
        {
            var result = await client.Margin.GetMarginPriceIndexAsync("BNBBTC");

            Assert.True(result.Success);
            Assert.Equal(0.00333930m, result.Data.Price);
            Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_562_046_418_000).UtcDateTime, result.Data.CalculationTime);
            AssertUnsignedMarketDataRequest(priceHandler, "/sapi/v1/margin/priceIndex");
            Assert.Contains(priceLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/priceIndex" && item.Weight == 10);
        }
    }

    [Fact]
    public async Task TimestampConflictingMarketData_FollowsSecurityClassAndConnector()
    {
        var isolatedLimiter = new RecordingRateLimiter();
        var isolatedHandler = new RecordingHttpMessageHandler(
            """[{"base":"BNB","isBuyAllowed":true,"isMarginTrade":true,"isSellAllowed":true,"quote":"BTC","symbol":"BNBBTC"}]""");
        using (var client = CreateClient(isolatedHandler, isolatedLimiter))
        {
            var result = await client.Margin.GetIsolatedMarginSymbolsAsync("BNBBTC", 5_000);

            Assert.True(result.Success);
            var pair = Assert.Single(result.Data);
            Assert.Equal("BNB", pair.BaseAsset);
            Assert.Equal("BTC", pair.QuoteAsset);
            AssertUnsignedMarketDataRequest(isolatedHandler, "/sapi/v1/margin/isolated/allPairs");
            var query = Uri.UnescapeDataString(isolatedHandler.RequestUri!.Query);
            Assert.Contains("symbol=BNBBTC", query);
            Assert.Contains("recvWindow=5000", query);
            Assert.Contains(isolatedLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/isolated/allPairs" && item.Weight == 10);
        }

        var delistLimiter = new RecordingRateLimiter();
        var delistHandler = new RecordingHttpMessageHandler(
            """[{"delistTime":1686161202000,"crossMarginAssets":["BTC"],"isolatedMarginSymbols":["ADAUSDT"]}]""");
        using (var client = CreateClient(delistHandler, delistLimiter))
        {
            var result = await client.Margin.GetMarginDelistScheduleAsync(5_000);

            Assert.True(result.Success);
            Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_686_161_202_000).UtcDateTime, Assert.Single(result.Data).DelistTime);
            AssertUnsignedMarketDataRequest(delistHandler, "/sapi/v1/margin/delist-schedule");
            Assert.Contains("recvWindow=5000", Uri.UnescapeDataString(delistHandler.RequestUri!.Query));
            Assert.Contains(delistLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/delist-schedule" && item.Weight == 100);
        }
    }

    [Fact]
    public async Task UserDataMarketQueries_AreSignedAndPreserveRawTimestamps()
    {
        var tierLimiter = new RecordingRateLimiter();
        var tierHandler = new RecordingHttpMessageHandler(
            """[{"symbol":"BTCUSDT","tier":3000000000,"effectiveMultiple":"10","initialRiskRatio":"1.111","liquidationRiskRatio":"1.05","baseAssetMaxBorrowable":"9","quoteAssetMaxBorrowable":"70000"}]""");
        using (var client = CreateClient(tierHandler, tierLimiter))
        {
            var result = await client.Margin.GetIsolatedMarginTierDataAsync("BTCUSDT", 3_000_000_000, 5_000);

            Assert.True(result.Success);
            var tier = Assert.Single(result.Data);
            Assert.Equal(3_000_000_000, tier.Tier);
            Assert.Equal(1.111m, tier.InitialRiskRatio);
            AssertSignedQueryRequest(tierHandler, "/sapi/v1/margin/isolatedMarginTier");
            var query = Uri.UnescapeDataString(tierHandler.RequestUri!.Query);
            Assert.Contains("symbol=BTCUSDT", query);
            Assert.Contains("tier=3000000000", query);
            Assert.Contains("recvWindow=5000", query);
            Assert.Contains(tierLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/isolatedMarginTier" && item.Weight == 1);
        }

        var inventoryLimiter = new RecordingRateLimiter();
        var inventoryHandler = new RecordingHttpMessageHandler(
            """{"assets":{"MATIC":"200","SHIB":"4000000"},"updateTime":1699272487}""");
        using (var client = CreateClient(inventoryHandler, inventoryLimiter))
        {
            var result = await client.Margin.GetMarginAvailableInventoryAsync(BinanceMarginInventoryType.Isolated);

            Assert.True(result.Success);
            Assert.Equal("4000000", result.Data.Assets["SHIB"]);
            Assert.Equal(1_699_272_487, result.Data.UpdateTime);
            AssertSignedQueryRequest(inventoryHandler, "/sapi/v1/margin/available-inventory");
            Assert.Contains("type=ISOLATED", Uri.UnescapeDataString(inventoryHandler.RequestUri!.Query));
            Assert.Contains(inventoryLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/available-inventory" && item.Weight == 50);
        }
    }

    [Fact]
    public async Task MarketData_RejectsKnownInvalidRequests()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("[]"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginSymbolsAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetIsolatedMarginSymbolsAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginAssetsAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetIsolatedMarginTierDataAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginPriceIndexAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetIsolatedMarginSymbolsAsync(receiveWindow: 60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginDelistScheduleAsync(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetIsolatedMarginTierDataAsync("BTCUSDT", receiveWindow: 60_001));
    }

    private static void AssertUnsignedMarketDataRequest(RecordingHttpMessageHandler handler, string path)
    {
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        Assert.DoesNotContain("timestamp=", handler.RequestUri.Query);
        Assert.DoesNotContain("signature=", handler.RequestUri.Query);
    }

    private static void AssertSignedQueryRequest(RecordingHttpMessageHandler handler, string path)
    {
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        Assert.Contains("timestamp=", handler.RequestUri.Query);
        Assert.Contains("signature=", handler.RequestUri.Query);
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
