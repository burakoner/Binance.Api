using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientForceOrdersTests
{
    [Fact]
    public async Task UsdForceOrders_UsesCurrentSymbolWeightAndCompleteResponse()
    {
        var startTime = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        var endTime = startTime.AddDays(30);
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(UsdResponse);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetForcedOrdersAsync(
            "BTCUSDT",
            BinanceFuturesAutoCloseType.Liquidation,
            startTime,
            endTime,
            limit: 100,
            receiveWindow: 60_001);

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/fapi/v1/forceOrders", 20);
        var query = DecodedQuery(handler);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("autoCloseType=LIQUIDATION", query);
        Assert.Contains($"startTime={new DateTimeOffset(startTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains($"endTime={new DateTimeOffset(endTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains("limit=100", query);
        Assert.Contains("recvWindow=60001", query);

        var order = Assert.Single(result.Data);
        AssertCommonOrder(order, "BTCUSDT", "BTCUSDT");
        Assert.False(order.PriceProtect);
        Assert.Null(order.GoodTillDate);
    }

    [Fact]
    public async Task CoinForceOrders_UsesCurrentUnscopedWeightAndCompleteResponse()
    {
        var startTime = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc);
        var endTime = startTime.AddDays(30);
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(CoinResponse);
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetForcedOrdersAsync(
            autoCloseType: BinanceFuturesAutoCloseType.ADL,
            startTime: startTime,
            endTime: endTime,
            limit: 100,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/dapi/v1/forceOrders", 50);
        var query = DecodedQuery(handler);
        Assert.DoesNotContain("symbol=", query);
        Assert.Contains("autoCloseType=ADL", query);
        Assert.Contains($"startTime={new DateTimeOffset(startTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains($"endTime={new DateTimeOffset(endTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains("limit=100", query);
        Assert.Contains("recvWindow=60000", query);

        var order = Assert.Single(result.Data);
        AssertCommonOrder(order, "BTCUSD_PERP", "BTCUSD");
        Assert.True(order.PriceProtect);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1596542105050).UtcDateTime, order.GoodTillDate);
    }

    [Fact]
    public async Task ForceOrders_UsesCurrentDynamicWeightsForTheOtherScopes()
    {
        var usdLimiter = new RecordingRateLimiter();
        var usdHandler = new RecordingHttpMessageHandler("[]");
        using (var usdClient = CreateClient(usdHandler, usdLimiter))
        {
            Assert.True((await usdClient.UsdFutures.GetForcedOrdersAsync()).Success);
            AssertSignedGet(usdHandler, usdLimiter, "/fapi/v1/forceOrders", 50);
            Assert.DoesNotContain("symbol=", DecodedQuery(usdHandler));
        }

        var coinLimiter = new RecordingRateLimiter();
        var coinHandler = new RecordingHttpMessageHandler("[]");
        using var coinClient = CreateClient(coinHandler, coinLimiter);

        Assert.True((await coinClient.CoinFutures.GetForcedOrdersAsync("BTCUSD_PERP")).Success);
        AssertSignedGet(coinHandler, coinLimiter, "/dapi/v1/forceOrders", 20);
        Assert.Contains("symbol=BTCUSD_PERP", DecodedQuery(coinHandler));
    }

    [Fact]
    public async Task ForceOrders_RejectsOnlyDocumentedLocalBoundsBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetForcedOrdersAsync(""));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetForcedOrdersAsync(" "));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetForcedOrdersAsync(""));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetForcedOrdersAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.GetForcedOrdersAsync(limit: 101));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.GetForcedOrdersAsync(limit: 101));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.GetForcedOrdersAsync(receiveWindow: 60_001));
        Assert.Null(handler.RequestUri);
    }

    [Fact]
    public async Task ForceOrders_PreservesProductSpecificConfiguredReceiveWindowContract()
    {
        var usdHandler = new RecordingHttpMessageHandler("[]");
        using (var usdClient = CreateClient(usdHandler, defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001)))
        {
            Assert.True((await usdClient.UsdFutures.GetForcedOrdersAsync()).Success);
            Assert.Contains("recvWindow=60001", DecodedQuery(usdHandler));
        }

        var coinHandler = new RecordingHttpMessageHandler("[]");
        using var coinClient = CreateClient(coinHandler, defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => coinClient.CoinFutures.GetForcedOrdersAsync());
        Assert.Null(coinHandler.RequestUri);
    }

    private const string UsdResponse =
        """
        [{
          "orderId": 9223372036854775806,
          "symbol": "BTCUSDT",
          "pair": "BTCUSDT",
          "status": "FILLED",
          "clientOrderId": "autoclose-usd-current",
          "price": "10871.09",
          "avgPrice": "10913.21000",
          "origQty": "0.001",
          "executedQty": "0.001",
          "cumQuote": "10.91321",
          "cumBase": "0.001",
          "timeInForce": "IOC",
          "type": "LIMIT",
          "reduceOnly": false,
          "closePosition": false,
          "side": "SELL",
          "positionSide": "BOTH",
          "stopPrice": "0",
          "workingType": "CONTRACT_PRICE",
          "origType": "LIMIT",
          "time": 1596107620044,
          "updateTime": 1596107620087
        }]
        """;

    private const string CoinResponse =
        """
        [{
          "orderId": 9223372036854775806,
          "symbol": "BTCUSD_PERP",
          "pair": "BTCUSD",
          "status": "FILLED",
          "clientOrderId": "autoclose-coin-current",
          "price": "10871.09",
          "avgPrice": "10913.21000",
          "origQty": "0.001",
          "executedQty": "0.001",
          "cumQuote": "10.91321",
          "cumBase": "0.001",
          "timeInForce": "IOC",
          "type": "LIMIT",
          "reduceOnly": false,
          "closePosition": false,
          "side": "SELL",
          "positionSide": "BOTH",
          "stopPrice": "0",
          "workingType": "CONTRACT_PRICE",
          "priceProtect": true,
          "origType": "LIMIT",
          "time": 1596107620044,
          "updateTime": 1596107620087,
          "goodTillDate": 1596542105050
        }]
        """;

    private static void AssertCommonOrder(BinanceFuturesOrder order, string symbol, string pair)
    {
        Assert.Equal(9223372036854775806, order.Id);
        Assert.Equal(symbol, order.Symbol);
        Assert.Equal(pair, order.Pair);
        Assert.Equal(BinanceOrderStatus.Filled, order.Status);
        Assert.StartsWith("autoclose-", order.ClientOrderId, StringComparison.Ordinal);
        Assert.Equal(10871.09m, order.Price);
        Assert.Equal(10913.21000m, order.AveragePrice);
        Assert.Equal(0.001m, order.Quantity);
        Assert.Equal(0.001m, order.QuantityFilled);
        Assert.Equal(10.91321m, order.QuoteQuantityFilled);
        Assert.Equal(0.001m, order.BaseQuantityFilled);
        Assert.Equal(BinanceTimeInForce.ImmediateOrCancel, order.TimeInForce);
        Assert.Equal(BinanceFuturesOrderType.Limit, order.Type);
        Assert.False(order.ReduceOnly);
        Assert.False(order.ClosePosition);
        Assert.Equal(BinanceOrderSide.Sell, order.Side);
        Assert.Equal(BinancePositionSide.Both, order.PositionSide);
        Assert.Equal(0m, order.StopPrice);
        Assert.Equal(BinanceFuturesWorkingType.Contract, order.WorkingType);
        Assert.Equal(BinanceFuturesOrderType.Limit, order.OriginalType);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1596107620044).UtcDateTime, order.CreateTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1596107620087).UtcDateTime, order.UpdateTime);
    }

    private static string DecodedQuery(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.RequestUri!.Query);

    private static void AssertSignedGet(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        string path,
        int weight)
    {
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values));
        Assert.Contains("timestamp=", handler.RequestUri.Query);
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == path && request.Weight == weight && request.Signed);
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
