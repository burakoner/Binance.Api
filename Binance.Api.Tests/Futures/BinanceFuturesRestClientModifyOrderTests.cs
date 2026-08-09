using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientModifyOrderTests
{
    [Fact]
    public async Task UsdModifyOrder_UsesCurrentSignedBodyAndDeserializesCurrentResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "orderId": 9223372036854775805,
              "symbol": "BTCUSDT",
              "pair": "BTCUSDT",
              "status": "NEW",
              "clientOrderId": "usd-client",
              "modifyId": 9007199254740993,
              "price": "50000.25",
              "origQty": "1.5",
              "executedQty": "0.25",
              "cumQty": "0.25",
              "timeInForce": "GTC",
              "type": "LIMIT",
              "reduceOnly": false,
              "closePosition": false,
              "side": "BUY",
              "positionSide": "BOTH",
              "stopPrice": "0",
              "workingType": "CONTRACT_PRICE",
              "priceProtect": false,
              "origType": "LIMIT",
              "priceMatch": "NONE",
              "selfTradePreventionMode": "NONE",
              "goodTillDate": 1750492800000,
              "updateTime": 1750489200123
            }
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.ModifyOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            1.5m,
            50_000.25m,
            orderId: 9_223_372_036_854_775_805L,
            origClientOrderId: "usd-client",
            modifyId: 9_007_199_254_740_993L,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        AssertSignedPut(handler, limiter, "/fapi/v1/order", 0);
        var body = ParseBody(handler);
        Assert.Equal("BTCUSDT", body["symbol"]);
        Assert.Equal("BUY", body["side"]);
        Assert.Equal("1.5", body["quantity"]);
        Assert.Equal("50000.25", body["price"]);
        Assert.Equal("9223372036854775805", body["orderId"]);
        Assert.Equal("usd-client", body["origClientOrderId"]);
        Assert.Equal("9007199254740993", body["modifyId"]);
        Assert.Equal("60000", body["recvWindow"]);
        Assert.False(body.ContainsKey("priceMatch"));

        Assert.Equal(9_223_372_036_854_775_805L, result.Data.Id);
        Assert.Equal("BTCUSDT", result.Data.Symbol);
        Assert.Equal("BTCUSDT", result.Data.Pair);
        Assert.Equal("usd-client", result.Data.ClientOrderId);
        Assert.Equal(9_007_199_254_740_993L, result.Data.ModifyId);
        Assert.Equal(BinanceOrderStatus.New, result.Data.Status);
        Assert.Equal(50_000.25m, result.Data.Price);
        Assert.Equal(1.5m, result.Data.Quantity);
        Assert.Equal(0.25m, result.Data.QuantityFilled);
        Assert.Equal(0.25m, result.Data.CumulativeQuantity);
        Assert.Equal(BinanceTimeInForce.GoodTillCanceled, result.Data.TimeInForce);
        Assert.Equal(BinanceFuturesOrderType.Limit, result.Data.Type);
        Assert.Equal(BinanceFuturesOrderType.Limit, result.Data.OriginalType);
        Assert.Equal(BinanceOrderSide.Buy, result.Data.Side);
        Assert.Equal(BinancePositionSide.Both, result.Data.PositionSide);
        Assert.Equal(BinanceFuturesWorkingType.Contract, result.Data.WorkingType);
        Assert.Equal(BinanceFuturesPriceMatch.None, result.Data.PriceMatch);
        Assert.Equal(BinanceSelfTradePreventionMode.None, result.Data.SelfTradePreventionMode);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_750_492_800_000).UtcDateTime, result.Data.GoodTillDate);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_750_489_200_123).UtcDateTime, result.Data.UpdateTime);
        Assert.Equal(0m, result.Data.AveragePrice);
        Assert.Null(result.Data.QuoteQuantityFilled);
        Assert.Null(result.Data.BaseQuantityFilled);
    }

    [Fact]
    public async Task CoinModifyOrder_SerializesRequiredSideAndCorrectClientIdentifier()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "orderId": 9223372036854775804,
              "symbol": "BTCUSD_PERP",
              "pair": "BTCUSD",
              "status": "NEW",
              "clientOrderId": "coin-client",
              "modifyId": 9007199254740995,
              "price": "60000.5",
              "origQty": "3",
              "executedQty": "0",
              "cumQty": "0",
              "timeInForce": "GTC",
              "type": "LIMIT",
              "reduceOnly": true,
              "closePosition": false,
              "side": "SELL",
              "positionSide": "SHORT",
              "stopPrice": "0",
              "workingType": "MARK_PRICE",
              "priceProtect": true,
              "origType": "LIMIT",
              "priceMatch": "NONE",
              "selfTradePreventionMode": "EXPIRE_MAKER",
              "updateTime": 1750489200456
            }
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.ModifyOrderAsync(
            "BTCUSD_PERP",
            BinanceOrderSide.Sell,
            3m,
            60_000.5m,
            origClientOrderId: "coin-client",
            modifyId: 9_007_199_254_740_995L);

        Assert.True(result.Success);
        AssertSignedPut(handler, limiter, "/dapi/v1/order", 1);
        var body = ParseBody(handler);
        Assert.Equal("BTCUSD_PERP", body["symbol"]);
        Assert.Equal("SELL", body["side"]);
        Assert.Equal("3", body["quantity"]);
        Assert.Equal("60000.5", body["price"]);
        Assert.Equal("coin-client", body["origClientOrderId"]);
        Assert.Equal("9007199254740995", body["modifyId"]);
        Assert.False(body.ContainsKey("orderId"));
        Assert.False(body.ContainsKey("priceMatch"));

        Assert.Equal(9_223_372_036_854_775_804L, result.Data.Id);
        Assert.Equal("BTCUSD_PERP", result.Data.Symbol);
        Assert.Equal("BTCUSD", result.Data.Pair);
        Assert.Equal(9_007_199_254_740_995L, result.Data.ModifyId);
        Assert.Equal(BinanceOrderSide.Sell, result.Data.Side);
        Assert.Equal(BinancePositionSide.Short, result.Data.PositionSide);
        Assert.Equal(BinanceFuturesWorkingType.Mark, result.Data.WorkingType);
        Assert.Equal(BinanceSelfTradePreventionMode.ExpireMaker, result.Data.SelfTradePreventionMode);
        Assert.True(result.Data.ReduceOnly);
        Assert.True(result.Data.PriceProtect);
        Assert.Null(result.Data.GoodTillDate);
        Assert.Null(result.Data.QuoteQuantityFilled);
        Assert.Null(result.Data.BaseQuantityFilled);
    }

    [Fact]
    public async Task ModifyOrder_RejectsUnsafeOrUndocumentedRequestsBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.UsdFutures.ModifyOrderAsync(" ", BinanceOrderSide.Buy, 1, 1, orderId: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.UsdFutures.ModifyOrderAsync("BTCUSDT", (BinanceOrderSide)0, 1, 1, orderId: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.UsdFutures.ModifyOrderAsync("BTCUSDT", BinanceOrderSide.Buy, 0, 1, orderId: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.UsdFutures.ModifyOrderAsync("BTCUSDT", BinanceOrderSide.Buy, 1, 0, orderId: 1));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.UsdFutures.ModifyOrderAsync("BTCUSDT", BinanceOrderSide.Buy, 1, 1));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.UsdFutures.ModifyOrderAsync("BTCUSDT", BinanceOrderSide.Buy, 1, 1, origClientOrderId: " "));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.UsdFutures.ModifyOrderAsync("BTCUSDT", BinanceOrderSide.Buy, 1, 1, orderId: 1, priceMatch: BinanceFuturesPriceMatch.Opponent));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.UsdFutures.ModifyOrderAsync("BTCUSDT", BinanceOrderSide.Buy, 1, 1, orderId: 1, receiveWindow: 60_001));

        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.CoinFutures.ModifyOrderAsync(" ", BinanceOrderSide.Sell, 1, 1, orderId: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.ModifyOrderAsync("BTCUSD_PERP", (BinanceOrderSide)0, 1, 1, orderId: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.ModifyOrderAsync("BTCUSD_PERP", BinanceOrderSide.Sell, 0, 1, orderId: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.ModifyOrderAsync("BTCUSD_PERP", BinanceOrderSide.Sell, 1, 0, orderId: 1));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.CoinFutures.ModifyOrderAsync("BTCUSD_PERP", BinanceOrderSide.Sell, 1, 1));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.CoinFutures.ModifyOrderAsync("BTCUSD_PERP", BinanceOrderSide.Sell, 1, 1, orderId: 1, origClientOrderId: " "));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.CoinFutures.ModifyOrderAsync("BTCUSD_PERP", BinanceOrderSide.Sell, 1, 1, orderId: 1, priceMatch: BinanceFuturesPriceMatch.Queue));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.ModifyOrderAsync("BTCUSD_PERP", BinanceOrderSide.Sell, 1, 1, orderId: 1, receiveWindow: 60_001));
        Assert.Null(handler.RequestUri);

        var configuredHandler = new RecordingHttpMessageHandler("{}");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.ModifyOrderAsync("BTCUSDT", BinanceOrderSide.Buy, 1, 1, orderId: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.CoinFutures.ModifyOrderAsync("BTCUSD_PERP", BinanceOrderSide.Sell, 1, 1, orderId: 1));
        Assert.Null(configuredHandler.RequestUri);
    }

    private static Dictionary<string, string> ParseBody(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.Body!)
            .Split('&')
            .Select(value => value.Split('=', 2))
            .ToDictionary(value => value[0], value => value[1]);

    private static void AssertSignedPut(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        string path,
        int weight)
    {
        Assert.Equal(HttpMethod.Put, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Equal("application/x-www-form-urlencoded", handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values));
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.DoesNotContain("timestamp=", handler.RequestUri.Query);
        Assert.Contains("timestamp=", handler.Body);
        Assert.DoesNotContain("signature=", handler.Body);
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
