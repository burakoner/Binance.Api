using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientBatchModifyTests
{
    [Fact]
    public async Task UsdBatchModify_UsesCurrentSignedBodyAndPreservesPerItemResults()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [
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
                "positionSide": "LONG",
                "stopPrice": "0",
                "workingType": "CONTRACT_PRICE",
                "priceProtect": false,
                "origType": "LIMIT",
                "priceMatch": "NONE",
                "selfTradePreventionMode": "NONE",
                "goodTillDate": 1750492800000,
                "updateTime": 1750489200123
              },
              {
                "code": -2022,
                "msg": "ReduceOnly Order is rejected."
              }
            ]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.ModifyOrdersAsync(
            [
                new BinanceFuturesBatchModifyRequest
                {
                    Symbol = "BTCUSDT",
                    Side = BinanceOrderSide.Buy,
                    Quantity = 1.5m,
                    Price = 50_000.25m,
                    OrderId = 9_223_372_036_854_775_805L,
                    OriginalClientOrderId = "usd-client",
                    ModifyId = 9_007_199_254_740_993L
                },
                new BinanceFuturesBatchModifyRequest
                {
                    Symbol = "ETHUSDT",
                    Side = BinanceOrderSide.Sell,
                    Quantity = 2m,
                    Price = 3_500.5m,
                    OriginalClientOrderId = "eth-client"
                }
            ],
            receiveWindow: 60_000);

        Assert.True(result.Success);
        AssertSignedPut(handler, limiter, "/fapi/v1/batchOrders", 5);
        var body = ParseBody(handler);
        Assert.Equal("60000", body["recvWindow"]);
        var batch = JArray.Parse(body["batchOrders"]);
        Assert.Equal(2, batch.Count);
        Assert.Equal("BTCUSDT", batch[0]!["symbol"]!.Value<string>());
        Assert.Equal("BUY", batch[0]!["side"]!.Value<string>());
        Assert.Equal(1.5m, batch[0]!["quantity"]!.Value<decimal>());
        Assert.Equal(50_000.25m, batch[0]!["price"]!.Value<decimal>());
        Assert.Equal(9_223_372_036_854_775_805L, batch[0]!["orderId"]!.Value<long>());
        Assert.Equal("usd-client", batch[0]!["origClientOrderId"]!.Value<string>());
        Assert.Equal(9_007_199_254_740_993L, batch[0]!["modifyId"]!.Value<long>());
        Assert.Equal("eth-client", batch[1]!["origClientOrderId"]!.Value<string>());
        Assert.Null(batch[1]!["orderId"]);
        Assert.Null(batch[1]!["modifyId"]);
        Assert.All(batch, item =>
        {
            Assert.Null(item!["priceMatch"]);
            Assert.Null(item["stopPrice"]);
            Assert.Null(item["timestamp"]);
            Assert.Null(item["recvWindow"]);
        });

        Assert.Equal(2, result.Data.Count);
        var success = result.Data[0];
        Assert.True(success.Success);
        Assert.Equal(9_223_372_036_854_775_805L, success.Data!.Id);
        Assert.Equal(9_007_199_254_740_993L, success.Data.ModifyId);
        Assert.Equal("BTCUSDT", success.Data.Pair);
        Assert.Equal(0.25m, success.Data.CumulativeQuantity);
        Assert.Equal(BinancePositionSide.Long, success.Data.PositionSide);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_750_492_800_000).UtcDateTime, success.Data.GoodTillDate);
        Assert.Null(success.Data.QuoteQuantityFilled);
        Assert.Null(success.Data.BaseQuantityFilled);

        var failure = result.Data[1];
        Assert.False(failure.Success);
        Assert.Equal(-2022, failure.Error!.Code);
        Assert.Equal("ReduceOnly Order is rejected.", failure.Error.Message);
    }

    [Fact]
    public async Task CoinBatchModify_UsesPostMigrationItemContractAndCurrentResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [{
              "orderId": 9223372036854775804,
              "symbol": "BTCUSD_PERP",
              "pair": "BTCUSD",
              "status": "NEW",
              "clientOrderId": "coin_client",
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
            }]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.ModifyOrdersAsync(
            [
                new BinanceFuturesBatchModifyRequest
                {
                    Symbol = "BTCUSD_PERP",
                    Side = BinanceOrderSide.Sell,
                    Quantity = 3m,
                    Price = 60_000.5m,
                    OriginalClientOrderId = "coin_client",
                    ModifyId = 9_007_199_254_740_995L
                }
            ]);

        Assert.True(result.Success);
        AssertSignedPut(handler, limiter, "/dapi/v1/batchOrders", 5);
        var body = ParseBody(handler);
        var item = Assert.Single(JArray.Parse(body["batchOrders"]));
        Assert.Equal("BTCUSD_PERP", item["symbol"]!.Value<string>());
        Assert.Equal("SELL", item["side"]!.Value<string>());
        Assert.Equal(3m, item["quantity"]!.Value<decimal>());
        Assert.Equal(60_000.5m, item["price"]!.Value<decimal>());
        Assert.Equal("coin_client", item["origClientOrderId"]!.Value<string>());
        Assert.Equal(9_007_199_254_740_995L, item["modifyId"]!.Value<long>());
        Assert.Null(item["priceMatch"]);
        Assert.Null(item["timestamp"]);
        Assert.Null(item["recvWindow"]);

        var order = Assert.Single(result.Data).Data!;
        Assert.Equal(9_223_372_036_854_775_804L, order.Id);
        Assert.Equal(9_007_199_254_740_995L, order.ModifyId);
        Assert.Equal("BTCUSD", order.Pair);
        Assert.Equal(BinanceOrderSide.Sell, order.Side);
        Assert.Equal(BinancePositionSide.Short, order.PositionSide);
        Assert.Equal(BinanceFuturesWorkingType.Mark, order.WorkingType);
        Assert.Equal(BinanceSelfTradePreventionMode.ExpireMaker, order.SelfTradePreventionMode);
        Assert.IsAssignableFrom<BinanceFuturesCoinRestOrderModificationAcknowledgement>(order);
    }

    [Fact]
    public async Task BatchModify_RejectsInvalidListsItemsAndReceiveWindowsBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentNullException>(() => client.UsdFutures.ModifyOrdersAsync(null!));
        await Assert.ThrowsAsync<ArgumentException>(() => client.UsdFutures.ModifyOrdersAsync([]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.UsdFutures.ModifyOrdersAsync(
            Enumerable.Range(0, 6).Select(_ => ValidRequest()).ToArray()));
        await Assert.ThrowsAsync<ArgumentException>(() => client.UsdFutures.ModifyOrdersAsync([null!]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.UsdFutures.ModifyOrdersAsync([ValidRequest(symbol: " ")]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.ModifyOrdersAsync([ValidRequest(side: (BinanceOrderSide)0)]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.ModifyOrdersAsync([ValidRequest(quantity: 0)]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.ModifyOrdersAsync([ValidRequest(price: 0)]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.UsdFutures.ModifyOrdersAsync([ValidRequest(orderId: null)]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.UsdFutures.ModifyOrdersAsync([ValidRequest(originalClientOrderId: " ")]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.UsdFutures.ModifyOrdersAsync([ValidRequest(priceMatch: BinanceFuturesPriceMatch.Opponent)]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.ModifyOrdersAsync([ValidRequest()], receiveWindow: 60_001));

        await Assert.ThrowsAsync<ArgumentNullException>(() => client.CoinFutures.ModifyOrdersAsync(null!));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.ModifyOrdersAsync([]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.ModifyOrdersAsync(
            Enumerable.Range(0, 6).Select(_ => ValidRequest()).ToArray()));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.ModifyOrdersAsync([null!]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.ModifyOrdersAsync([ValidRequest(symbol: " ")]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.ModifyOrdersAsync([ValidRequest(side: (BinanceOrderSide)0)]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.ModifyOrdersAsync([ValidRequest(quantity: 0)]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.ModifyOrdersAsync([ValidRequest(price: 0)]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.ModifyOrdersAsync([ValidRequest(orderId: null)]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.ModifyOrdersAsync([ValidRequest(originalClientOrderId: " ")]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.ModifyOrdersAsync([ValidRequest(priceMatch: BinanceFuturesPriceMatch.Queue)]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.ModifyOrdersAsync([ValidRequest()], receiveWindow: 60_001));
        Assert.Null(handler.RequestUri);

        var configuredHandler = new RecordingHttpMessageHandler("[]");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.ModifyOrdersAsync([ValidRequest()]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.CoinFutures.ModifyOrdersAsync([ValidRequest()]));
        Assert.Null(configuredHandler.RequestUri);
    }

    private static BinanceFuturesBatchModifyRequest ValidRequest(
        string symbol = "BTCUSDT",
        BinanceOrderSide side = BinanceOrderSide.Buy,
        decimal quantity = 1,
        decimal price = 1,
        long? orderId = 1,
        string? originalClientOrderId = null,
        BinanceFuturesPriceMatch? priceMatch = null)
        => new()
        {
            Symbol = symbol,
            Side = side,
            Quantity = quantity,
            Price = price,
            OrderId = orderId,
            OriginalClientOrderId = originalClientOrderId,
            PriceMatch = priceMatch
        };

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
