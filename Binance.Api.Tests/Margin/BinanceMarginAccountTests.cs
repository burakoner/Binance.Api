using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginAccountTests
{
    [Fact]
    public async Task AccountMutations_UseCurrentSignedBodyAndQueryContracts()
    {
        var leverageLimiter = new RecordingRateLimiter();
        var leverageHandler = new RecordingHttpMessageHandler("""{"success":true}""");
        using (var client = CreateClient(leverageHandler, leverageLimiter))
        {
            var result = await client.Margin.AdjustMaximumLeverageAsync(10);

            Assert.True(result.Success);
            Assert.True(result.Data.Success);
            Assert.Equal(HttpMethod.Post, leverageHandler.Method);
            Assert.Equal("/sapi/v1/margin/max-leverage", leverageHandler.RequestUri!.AbsolutePath);
            Assert.Contains("signature=", leverageHandler.RequestUri.Query);
            var body = Uri.UnescapeDataString(leverageHandler.Body!);
            Assert.Contains("maxLeverage=10", body);
            Assert.DoesNotContain("recvWindow", body);
            Assert.Contains("timestamp=", body);
            Assert.Contains(leverageLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/max-leverage" && item.Weight == 3_000);
        }

        var enableLimiter = new RecordingRateLimiter();
        var enableHandler = new RecordingHttpMessageHandler("""{"success":true,"symbol":"BTCUSDT"}""");
        using (var client = CreateClient(enableHandler, enableLimiter))
        {
            var result = await client.Margin.EnableIsolatedMarginAccountAsync("BTCUSDT", 5_000);

            Assert.True(result.Success);
            Assert.Equal("BTCUSDT", result.Data.Symbol);
            Assert.Equal(HttpMethod.Post, enableHandler.Method);
            Assert.Equal("/sapi/v1/margin/isolated/account", enableHandler.RequestUri!.AbsolutePath);
            Assert.Contains("signature=", enableHandler.RequestUri.Query);
            var body = Uri.UnescapeDataString(enableHandler.Body!);
            Assert.Contains("symbol=BTCUSDT", body);
            Assert.Contains("recvWindow=5000", body);
            Assert.Contains("timestamp=", body);
            Assert.Contains(enableLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/isolated/account" && item.Weight == 300);
        }

        var disableLimiter = new RecordingRateLimiter();
        var disableHandler = new RecordingHttpMessageHandler("""{"success":true,"symbol":"BTCUSDT"}""");
        using (var client = CreateClient(disableHandler, disableLimiter))
        {
            var result = await client.Margin.DisableIsolatedMarginAccountAsync("BTCUSDT", 5_000);

            Assert.True(result.Success);
            Assert.Equal("BTCUSDT", result.Data.Symbol);
            Assert.Equal(HttpMethod.Delete, disableHandler.Method);
            Assert.Equal("/sapi/v1/margin/isolated/account", disableHandler.RequestUri!.AbsolutePath);
            Assert.Null(disableHandler.Body);
            var query = AssertSignedQuery(disableHandler);
            Assert.Contains("symbol=BTCUSDT", query);
            Assert.Contains("recvWindow=5000", query);
            Assert.Contains(disableLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/isolated/account" && item.Weight == 300);
        }
    }

    [Fact]
    public async Task AccountQueries_DeserializeCurrentSummaryAndCrossAccountModels()
    {
        var burnLimiter = new RecordingRateLimiter();
        var burnHandler = new RecordingHttpMessageHandler("""{"spotBNBBurn":true,"interestBNBBurn":false}""");
        using (var client = CreateClient(burnHandler, burnLimiter))
        {
            var result = await client.Margin.GetBnbBurnStatusAsync(5_000);

            Assert.True(result.Data.SpotBnbBurn);
            Assert.False(result.Data.InterestBnbBurn);
            Assert.Contains("recvWindow=5000", AssertSignedQuery(burnHandler));
            Assert.Contains(burnLimiter.Requests, item => item.Endpoint == "/sapi/v1/bnbBurn" && item.Weight == 1);
        }

        var summaryLimiter = new RecordingRateLimiter();
        var summaryHandler = new RecordingHttpMessageHandler(
            """{"normalBar":"1.500000000000000001","marginCallBar":"1.3","forceLiquidationBar":"1.1"}""");
        using (var client = CreateClient(summaryHandler, summaryLimiter))
        {
            var result = await client.Margin.GetMarginLevelInformationAsync();

            Assert.Equal(1.500000000000000001m, result.Data.NormalLevel);
            Assert.Equal(1.3m, result.Data.MarginCallLevel);
            Assert.Equal(1.1m, result.Data.ForcedLiquidationLevel);
            AssertSignedQuery(summaryHandler);
            Assert.Contains(summaryLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/tradeCoeff" && item.Weight == 10);
        }

        var accountLimiter = new RecordingRateLimiter();
        var accountHandler = new RecordingHttpMessageHandler(
            """
            {
              "created":true,"borrowEnabled":true,"marginLevel":"11.64405625","collateralMarginLevel":"3.2",
              "totalAssetOfBtc":"6.82728457","totalLiabilityOfBtc":"0.58633215","totalNetAssetOfBtc":"6.24095242",
              "TotalCollateralValueInUSDT":"5.82728457","totalOpenOrderLossInUSDT":"582.728457",
              "tradeEnabled":true,"transferInEnabled":true,"transferOutEnabled":false,"accountType":"MARGIN_2",
              "userAssets":[{"asset":"BTC","borrowed":"0","free":"0.004995","interest":"0","locked":"0","netAsset":"0.004995"}]
            }
            """);
        using (var client = CreateClient(accountHandler, accountLimiter))
        {
            var result = await client.Margin.GetMarginAccountInfoAsync();

            Assert.True(result.Data.Created);
            Assert.Equal(3.2m, result.Data.CollateralMarginLevel);
            Assert.Equal(5.82728457m, result.Data.TotalCollateralValueInUsdt);
            Assert.Equal(582.728457m, result.Data.TotalOpenOrderLossInUsdt);
            Assert.True(result.Data.TransferInEnabled);
            Assert.False(result.Data.TransferOutEnabled);
            Assert.Equal("MARGIN_2", result.Data.AccountType);
            Assert.Equal("BTC", Assert.Single(result.Data.Balances).Asset);
            AssertSignedQuery(accountHandler);
            Assert.Contains(accountLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/account" && item.Weight == 10);
        }
    }

    [Fact]
    public async Task FeeAndLimitQueries_UseInt64ContractsAndDynamicWeights()
    {
        var crossLimiter = new RecordingRateLimiter();
        var crossHandler = new RecordingHttpMessageHandler(
            """[{"vipLevel":3000000000,"coin":"BTC","transferIn":true,"borrowable":true,"dailyInterest":"0.00026125","yearlyInterest":"0.0953","borrowLimit":"180","marginablePairs":["BNBBTC"]}]""");
        using (var client = CreateClient(crossHandler, crossLimiter))
        {
            var result = await client.Margin.GetCrossMarginFeeDataAsync("BTC", 3_000_000_000, 5_000);

            var fee = Assert.Single(result.Data);
            Assert.Equal(3_000_000_000, fee.VipLevel);
            Assert.Equal(0.00026125m, fee.DailyInterest);
            var query = AssertSignedQuery(crossHandler);
            Assert.Contains("coin=BTC", query);
            Assert.Contains("vipLevel=3000000000", query);
            Assert.Contains("recvWindow=5000", query);
            Assert.Contains(crossLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/crossMarginData" && item.Weight == 1);
        }

        var limitLimiter = new RecordingRateLimiter();
        var limitHandler = new RecordingHttpMessageHandler("""{"enabledAccount":3000000000,"maxAccount":4000000000}""");
        using (var client = CreateClient(limitHandler, limitLimiter))
        {
            var result = await client.Margin.GetEnabledIsolatedMarginAccountLimitAsync();

            Assert.Equal(3_000_000_000, result.Data.EnabledAccount);
            Assert.Equal(4_000_000_000, result.Data.MaxAccount);
            AssertSignedQuery(limitHandler);
            Assert.Contains(limitLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/isolated/accountLimit" && item.Weight == 1);
        }

        var isolatedLimiter = new RecordingRateLimiter();
        var isolatedHandler = new RecordingHttpMessageHandler(
            """[{"vipLevel":3000000000,"symbol":"BTCUSDT","leverage":"10","data":[{"coin":"BTC","dailyInterest":"0.00026125","borrowLimit":"270"}]}]""");
        using (var client = CreateClient(isolatedHandler, isolatedLimiter))
        {
            var result = await client.Margin.GetIsolatedMarginFeeDataAsync("BTCUSDT", 3_000_000_000);

            var fee = Assert.Single(result.Data);
            Assert.Equal(3_000_000_000, fee.VipLevel);
            Assert.Equal("10", fee.Leverage);
            Assert.Equal(270m, Assert.Single(fee.Data).BorrowLimit);
            var query = AssertSignedQuery(isolatedHandler);
            Assert.Contains("symbol=BTCUSDT", query);
            Assert.Contains("vipLevel=3000000000", query);
            Assert.Contains(isolatedLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/isolatedMarginData" && item.Weight == 1);
        }
    }

    [Fact]
    public async Task IsolatedAccountQuery_SendsAtMostFiveSymbolsAndPreservesModel()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "assets":[{
                "baseAsset":{"asset":"BTC","borrowEnabled":true,"borrowed":"1","free":"2","interest":"0.1","locked":"3","netAsset":"3.9","netAssetOfBtc":"3.9","repayEnabled":true,"totalAsset":"5"},
                "quoteAsset":{"asset":"USDT","borrowEnabled":true,"borrowed":"0","free":"100","interest":"0","locked":"0","netAsset":"100","netAssetOfBtc":"0.001","repayEnabled":true,"totalAsset":"100"},
                "symbol":"BTCUSDT","isolatedCreated":true,"enabled":true,"marginLevel":"2.5","marginLevelStatus":"NORMAL","marginRatio":"1.2","indexPrice":"60000","liquidatePrice":"30000","liquidateRate":"1.1","tradeEnabled":true
              }],
              "totalAssetOfBtc":"4","totalLiabilityOfBtc":"0.1","totalNetAssetOfBtc":"3.9"
            }
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.Margin.GetIsolatedMarginAccountAsync(["BTCUSDT", "ETHUSDT"], 5_000);

        var account = Assert.Single(result.Data.Assets);
        Assert.Equal("BTCUSDT", account.Symbol);
        Assert.Equal(Binance.Api.Margin.BinanceMarginLevelStatus.Normal, account.MarginLevelStatus);
        Assert.Equal(3.9m, account.BaseAsset.NetAsset);
        var query = AssertSignedQuery(handler);
        Assert.Contains("symbols=BTCUSDT,ETHUSDT", query);
        Assert.Contains("recvWindow=5000", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/isolated/account" && item.Weight == 10);
    }

    [Fact]
    public async Task AccountOperations_RejectDocumentedInvalidRequests()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.AdjustMaximumLeverageAsync(20));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.EnableIsolatedMarginAccountAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.EnableIsolatedMarginAccountAsync("BTCUSDT", 60_001));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.DisableIsolatedMarginAccountAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetBnbBurnStatusAsync(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginLevelInformationAsync(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginAccountInfoAsync(60_001));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetCrossMarginFeeDataAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetEnabledIsolatedMarginAccountLimitAsync(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetIsolatedMarginAccountAsync([]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetIsolatedMarginAccountAsync(["A", "B", "C", "D", "E", "F"]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetIsolatedMarginAccountAsync(["BTCUSDT,ETHUSDT"]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetIsolatedMarginFeeDataAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetIsolatedMarginFeeDataAsync(receiveWindow: 60_001));
    }

    private static string AssertSignedQuery(RecordingHttpMessageHandler handler)
    {
        Assert.Null(handler.Body);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        return query;
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
