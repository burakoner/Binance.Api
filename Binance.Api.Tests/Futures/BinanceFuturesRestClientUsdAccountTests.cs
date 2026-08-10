using ApiSharp.Authentication;
using ApiSharp.Converters;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdAccountTests
{
    [Theory]
    [InlineData(BinanceFuturesIncomeType.StrategyUmFuturesTransfer, "STRATEGY_UMFUTURES_TRANSFER")]
    [InlineData(BinanceFuturesIncomeType.FeeReturn, "FEE_RETURN")]
    [InlineData(BinanceFuturesIncomeType.BfusdReward, "BFUSD_REWARD")]
    public void IncomeTypes_MapCurrentWireValuesBidirectionally(BinanceFuturesIncomeType incomeType, string expected)
    {
        Assert.Equal(expected, MapConverter.GetString(incomeType));

        var response = JsonConvert.DeserializeObject<BinanceFuturesIncomeHistory>(
            $$"""{"incomeType":"{{expected}}"}""");
        Assert.Equal(incomeType, response!.IncomeType);
    }

    [Fact]
    public async Task IncomeHistory_UsesTypedInt64QueryAndPreservesCurrentIdentifiers()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "symbol": "BTCUSDT",
                "incomeType": "BFUSD_REWARD",
                "income": "1.25",
                "asset": "BFUSD",
                "info": "reward",
                "time": 1770736694138,
                "tranId": 9223372036854775805,
                "tradeId": "trade-42"
              }
            ]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetIncomeHistoryAsync(
            "BTCUSDT",
            BinanceFuturesIncomeType.BfusdReward,
            page: 3_000_000_000L,
            limit: 1000,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/income", handler.RequestUri!.AbsolutePath);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("incomeType=BFUSD_REWARD", query);
        Assert.Contains("page=3000000000", query);
        Assert.Contains("limit=1000", query);
        Assert.Contains("recvWindow=60000", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/fapi/v1/income" && item.Weight == 30 && item.Signed);

        var income = Assert.Single(result.Data);
        Assert.Equal(BinanceFuturesIncomeType.BfusdReward, income.IncomeType);
        Assert.Equal(9_223_372_036_854_775_805L, income.TransactionId);
        Assert.Equal("trade-42", income.TradeId);
    }

    [Theory]
    [InlineData(false, "/fapi/v3/account")]
    [InlineData(true, "/fapi/v2/account")]
    public async Task AccountInfoQueries_UseCurrentWeightReceiveWindowAndModel(bool useV2, string expectedPath)
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("{\"feeBurn\":true}");
        using var client = CreateClient(handler, limiter);

        if (useV2)
        {
#pragma warning disable CS0618 // The obsolete v2 route remains public and needs regression coverage.
            var result = await client.UsdFutures.GetAccountInfoV2Async(60_000);
#pragma warning restore CS0618
            Assert.True(result.Success);
            Assert.True(result.Data.FeeBurn);
        }
        else
        {
            var result = await client.UsdFutures.GetAccountInfoAsync(60_000);
            Assert.True(result.Success);
        }

        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(expectedPath, handler.RequestUri!.AbsolutePath);
        Assert.Contains("recvWindow=60000", handler.RequestUri.Query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == expectedPath && item.Weight == 5 && item.Signed);
    }

    [Fact]
    public async Task AccountInfoQueries_RejectReceiveWindowAboveCurrentMaximum()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.UsdFutures.GetAccountInfoAsync(60_001));
#pragma warning disable CS0618 // The obsolete v2 route remains public and needs regression coverage.
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.UsdFutures.GetAccountInfoV2Async(60_001));
#pragma warning restore CS0618

        Assert.Null(handler.RequestUri);
    }

    [Theory]
    [InlineData(false, "/fapi/v3/balance")]
    [InlineData(true, "/fapi/v2/balance")]
    public async Task BalanceQueries_UseCurrentSignedContractAndDeserializeResponse(bool useV2, string expectedPath)
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "accountAlias": "SgsR",
                "asset": "USDT",
                "balance": "122607.35137903",
                "crossWalletBalance": "23.72469206",
                "crossUnPnl": "0.00000000",
                "availableBalance": "23.72469206",
                "maxWithdrawAmount": "23.72469206",
                "marginAvailable": true,
                "updateTime": 1720733057660
              }
            ]
            """);
        using var client = CreateClient(handler, limiter);

        var result = useV2
            ? await client.UsdFutures.GetBalancesV2Async(45_000)
            : await client.UsdFutures.GetBalancesAsync(45_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(expectedPath, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("recvWindow=45000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == expectedPath && item.Weight == 5 && item.Signed);

        var balance = Assert.Single(result.Data);
        Assert.Equal("SgsR", balance.AccountAlias);
        Assert.Equal("USDT", balance.Asset);
        Assert.Equal(122607.35137903m, balance.WalletBalance);
        Assert.Equal(23.72469206m, balance.CrossWalletBalance);
        Assert.Equal(0m, balance.CrossUnrealizedPnl);
        Assert.Equal(23.72469206m, balance.AvailableBalance);
        Assert.Equal(23.72469206m, balance.MaxWithdrawQuantity);
        Assert.True(balance.MarginAvailable);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1720733057660).UtcDateTime, balance.UpdateTime);
    }

    [Fact]
    public async Task BalanceQueries_RejectReceiveWindowAboveCurrentMaximum()
    {
        var explicitHandler = new RecordingHttpMessageHandler("[]");
        using (var explicitClient = CreateClient(explicitHandler))
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.UsdFutures.GetBalancesAsync(60_001));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.UsdFutures.GetBalancesV2Async(60_001));
        }
        Assert.Null(explicitHandler.RequestUri);

        var configuredHandler = new RecordingHttpMessageHandler("[]");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.GetBalancesAsync());
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.GetBalancesV2Async());
        Assert.Null(configuredHandler.RequestUri);
    }

    [Fact]
    public async Task PortfolioMarginAccountInfo_UsesCurrentSignedContractAndDeserializesResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "maxWithdrawAmountUSD": "1627523.32459208",
              "asset": "BTC",
              "maxWithdrawAmount": "27.43689636"
            }
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetPortfolioMarginAccountInfoAsync("BTC", 60_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/pmAccountInfo", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("asset=BTC", query);
        Assert.Contains("recvWindow=60000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item =>
            item.Endpoint == "/fapi/v1/pmAccountInfo" && item.Weight == 5 && item.Signed);

        Assert.Equal("BTC", result.Data.Asset);
        Assert.Equal(27.43689636m, result.Data.MaximumWithdrawAmount);
        Assert.Equal(1627523.32459208m, result.Data.MaximumWithdrawAmountUSD);
    }

    [Fact]
    public async Task PortfolioMarginAccountInfo_RejectsReceiveWindowAboveCurrentMaximum()
    {
        var explicitHandler = new RecordingHttpMessageHandler("{}");
        using (var explicitClient = CreateClient(explicitHandler))
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.UsdFutures.GetPortfolioMarginAccountInfoAsync("BTC", 60_001));
        }
        Assert.Null(explicitHandler.RequestUri);

        var configuredHandler = new RecordingHttpMessageHandler("{}");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.GetPortfolioMarginAccountInfoAsync("BTC"));
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
