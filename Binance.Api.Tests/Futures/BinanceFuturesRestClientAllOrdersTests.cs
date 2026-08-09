using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientAllOrdersTests
{
    [Fact]
    public async Task UsdAllOrders_UsesCurrentSignedContractAndCompleteResponse()
    {
        var startTime = DateTimeOffset.FromUnixTimeMilliseconds(1623319461670).UtcDateTime;
        var endTime = startTime.AddDays(6);
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(CurrentOrderResponse("BTCUSDT", "BTCUSDT"));
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetOrdersAsync(
            "BTCUSDT",
            orderId: 9223372036854775805,
            startTime: startTime,
            endTime: endTime,
            limit: 1000,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/fapi/v1/allOrders", 5);
        var query = DecodedQuery(handler);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("orderId=9223372036854775805", query);
        Assert.Contains($"startTime={new DateTimeOffset(startTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains($"endTime={new DateTimeOffset(endTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains("limit=1000", query);
        Assert.Contains("recvWindow=60000", query);
        Assert.DoesNotContain("pair=", query);
        AssertCurrentOrder(Assert.Single(result.Data), "BTCUSDT", "BTCUSDT");
    }

    [Fact]
    public async Task CoinPairAllOrders_UsesCurrentFlatWeightAndCompleteResponse()
    {
        var startTime = DateTimeOffset.FromUnixTimeMilliseconds(1623319461670).UtcDateTime;
        var endTime = startTime.AddDays(6);
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(CurrentOrderResponse("BTCUSD_PERP", "BTCUSD"));
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetOrdersAsync(
            pair: "BTCUSD",
            startTime: startTime,
            endTime: endTime,
            limit: 100,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/dapi/v1/allOrders", 5);
        var query = DecodedQuery(handler);
        Assert.Contains("pair=BTCUSD", query);
        Assert.DoesNotContain("symbol=", query);
        Assert.DoesNotContain("orderId=", query);
        Assert.Contains($"startTime={new DateTimeOffset(startTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains($"endTime={new DateTimeOffset(endTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains("limit=100", query);
        Assert.Contains("recvWindow=60000", query);
        AssertCurrentOrder(Assert.Single(result.Data), "BTCUSD_PERP", "BTCUSD");
    }

    [Fact]
    public async Task AllOrders_RejectsInvalidScopeAndPairIdentifierCombinations()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetOrdersAsync(null!));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetOrdersAsync(""));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetOrdersAsync(" "));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetOrdersAsync());
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetOrdersAsync(symbol: ""));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetOrdersAsync(pair: " "));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetOrdersAsync("BTCUSD_PERP", "BTCUSD"));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetOrdersAsync(pair: "BTCUSD", orderId: 1));
        Assert.Null(handler.RequestUri);
    }

    [Fact]
    public async Task AllOrders_RejectsInvalidRangesLimitsAndReceiveWindows()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);
        var startTime = DateTime.UtcNow.AddDays(-7);

        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetOrdersAsync("BTCUSDT", startTime: startTime, endTime: startTime));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetOrdersAsync("BTCUSDT", startTime: startTime, endTime: startTime.AddDays(7)));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetOrdersAsync(pair: "BTCUSD", startTime: startTime, endTime: startTime));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetOrdersAsync(pair: "BTCUSD", startTime: startTime, endTime: startTime.AddDays(7)));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetOrdersAsync("BTCUSDT", limit: 0));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetOrdersAsync("BTCUSDT", limit: 1001));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetOrdersAsync(pair: "BTCUSD", limit: 0));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetOrdersAsync(pair: "BTCUSD", limit: 101));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.UsdFutures.GetOrdersAsync("BTCUSDT", receiveWindow: 60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.GetOrdersAsync(pair: "BTCUSD", receiveWindow: 60_001));
        Assert.Null(handler.RequestUri);
    }

    [Fact]
    public async Task AllOrders_RejectsConfiguredReceiveWindowAboveCurrentMaximum()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.GetOrdersAsync("BTCUSDT"));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.GetOrdersAsync(pair: "BTCUSD"));
        Assert.Null(handler.RequestUri);
    }

    private const string CurrentOrderResponseTemplate =
        """
        [{
          "avgPrice": "0.00000",
          "clientOrderId": "current-client-id",
          "cumQuote": "12.345",
          "cumBase": "0.0015",
          "executedQty": "0.001",
          "orderId": 9223372036854775806,
          "origQty": "0.010",
          "origType": "TRAILING_STOP_MARKET",
          "price": "50000.50",
          "reduceOnly": true,
          "side": "BUY",
          "positionSide": "LONG",
          "status": "NEW",
          "stopPrice": "49000.25",
          "closePosition": false,
          "symbol": "__SYMBOL__",
          "pair": "__PAIR__",
          "time": 1579276756075,
          "timeInForce": "GTC",
          "type": "TRAILING_STOP_MARKET",
          "activatePrice": "48000.75",
          "priceRate": "0.3",
          "updateTime": 1579276756076,
          "workingType": "MARK_PRICE",
          "priceProtect": true,
          "priceMatch": "NONE",
          "selfTradePreventionMode": "EXPIRE_TAKER",
          "goodTillDate": 1579276856075
        }]
        """;

    private static string CurrentOrderResponse(string symbol, string pair)
        => CurrentOrderResponseTemplate
            .Replace("__SYMBOL__", symbol, StringComparison.Ordinal)
            .Replace("__PAIR__", pair, StringComparison.Ordinal);

    private static void AssertCurrentOrder(BinanceFuturesOrder order, string symbol, string pair)
    {
        Assert.Equal(0m, order.AveragePrice);
        Assert.Equal("current-client-id", order.ClientOrderId);
        Assert.Equal(12.345m, order.QuoteQuantityFilled);
        Assert.Equal(0.0015m, order.BaseQuantityFilled);
        Assert.Equal(0.001m, order.QuantityFilled);
        Assert.Equal(9223372036854775806, order.Id);
        Assert.Equal(0.010m, order.Quantity);
        Assert.Equal(BinanceFuturesOrderType.TrailingStopMarket, order.OriginalType);
        Assert.Equal(50000.50m, order.Price);
        Assert.True(order.ReduceOnly);
        Assert.Equal(BinanceOrderSide.Buy, order.Side);
        Assert.Equal(BinancePositionSide.Long, order.PositionSide);
        Assert.Equal(BinanceOrderStatus.New, order.Status);
        Assert.Equal(49000.25m, order.StopPrice);
        Assert.False(order.ClosePosition);
        Assert.Equal(symbol, order.Symbol);
        Assert.Equal(pair, order.Pair);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1579276756075).UtcDateTime, order.CreateTime);
        Assert.Equal(BinanceTimeInForce.GoodTillCanceled, order.TimeInForce);
        Assert.Equal(BinanceFuturesOrderType.TrailingStopMarket, order.Type);
        Assert.Equal(48000.75m, order.ActivatePrice);
        Assert.Equal(0.3m, order.CallbackRate);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1579276756076).UtcDateTime, order.UpdateTime);
        Assert.Equal(BinanceFuturesWorkingType.Mark, order.WorkingType);
        Assert.True(order.PriceProtect);
        Assert.Equal(BinanceFuturesPriceMatch.None, order.PriceMatch);
        Assert.Equal(BinanceSelfTradePreventionMode.ExpireTaker, order.SelfTradePreventionMode);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1579276856075).UtcDateTime, order.GoodTillDate);
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
