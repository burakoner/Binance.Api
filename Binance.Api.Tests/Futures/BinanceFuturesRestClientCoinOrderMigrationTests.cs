using System.Collections;
using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientCoinOrderMigrationTests
{
    [Theory]
    [InlineData(BinanceFuturesOrderType.Stop)]
    [InlineData(BinanceFuturesOrderType.StopMarket)]
    [InlineData(BinanceFuturesOrderType.TakeProfit)]
    [InlineData(BinanceFuturesOrderType.TakeProfitMarket)]
    [InlineData(BinanceFuturesOrderType.TrailingStopMarket)]
    [InlineData(BinanceFuturesOrderType.Liquidation)]
    [InlineData((BinanceFuturesOrderType)byte.MaxValue)]
    public async Task PlaceOrderAsync_RejectsNonNormalTypesBeforeTransport(BinanceFuturesOrderType type)
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.PlaceOrderAsync(
            "BTCUSD_PERP",
            BinanceOrderSide.Buy,
            type,
            quantity: 1));

        Assert.Equal(0, handler.RequestCount);
    }

    [Theory]
    [InlineData(BinanceFuturesOrderType.Limit, "LIMIT")]
    [InlineData(BinanceFuturesOrderType.Market, "MARKET")]
    public async Task PlaceOrderAsync_UsesCurrentNormalContract(BinanceFuturesOrderType type, string wireType)
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("{\"orderId\":20072994037}");
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.PlaceOrderAsync(
            "BTCUSD_PERP",
            BinanceOrderSide.Buy,
            type,
            quantity: 1,
            price: type == BinanceFuturesOrderType.Limit ? 50_000 : null,
            timeInForce: type == BinanceFuturesOrderType.Limit ? BinanceTimeInForce.GoodTillCanceled : null,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.Equal(20_072_994_037L, result.Data.Id);
        AssertSignedPost(handler, limiter, "/dapi/v1/order", 0);
        var body = ParseBody(handler);
        Assert.Equal("BTCUSD_PERP", body["symbol"]);
        Assert.Equal(wireType, body["type"]);
        Assert.Equal("60000", body["recvWindow"]);
        Assert.False(body.ContainsKey("stopPrice"));
        Assert.False(body.ContainsKey("activationPrice"));
        Assert.False(body.ContainsKey("callbackRate"));
        Assert.False(body.ContainsKey("workingType"));
        Assert.False(body.ContainsKey("closePosition"));
        Assert.False(body.ContainsKey("priceProtect"));
    }

    [Fact]
    public void PlaceOrderAsync_PublicContractDoesNotExposeMigratedConditionalParameters()
    {
        var parameterNames = typeof(IBinanceFuturesRestClientCoinTrade)
            .GetMethod(nameof(IBinanceFuturesRestClientCoinTrade.PlaceOrderAsync))!
            .GetParameters()
            .Select(parameter => parameter.Name)
            .ToList();

        Assert.DoesNotContain("stopPrice", parameterNames);
        Assert.DoesNotContain("activationPrice", parameterNames);
        Assert.DoesNotContain("callbackRate", parameterNames);
        Assert.DoesNotContain("workingType", parameterNames);
        Assert.DoesNotContain("closePosition", parameterNames);
        Assert.DoesNotContain("priceProtect", parameterNames);
    }

    [Fact]
    public async Task PlaceOrdersAsync_MaterializesOnceAndUsesOnlyNormalFields()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);
        var orders = new SingleUseEnumerable<BinanceFuturesBatchOrderRequest>(
        [
            CreateBatchOrder(BinanceFuturesOrderType.Limit),
            CreateBatchOrder(BinanceFuturesOrderType.Market)
        ]);

        var result = await client.CoinFutures.PlaceOrdersAsync(orders, 60_000);

        Assert.True(result.Success);
        Assert.Equal(1, orders.EnumerationCount);
        AssertSignedPost(handler, limiter, "/dapi/v1/batchOrders", 5);
        var body = ParseBody(handler);
        Assert.Equal("60000", body["recvWindow"]);
        var batch = JArray.Parse(body["batchOrders"]);
        Assert.Equal("LIMIT", batch[0]!["type"]!.Value<string>());
        Assert.Equal("MARKET", batch[1]!["type"]!.Value<string>());
        Assert.Equal("RESULT", batch[0]!["newOrderRespType"]!.Value<string>());
        Assert.Null(batch[0]!["stopPrice"]);
        Assert.Null(batch[0]!["activationPrice"]);
        Assert.Null(batch[0]!["callbackRate"]);
        Assert.Null(batch[0]!["workingType"]);
        Assert.Null(batch[0]!["priceProtect"]);
    }

    [Fact]
    public async Task PlaceOrdersAsync_RejectsInvalidCollectionsAndTypesBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            client.CoinFutures.PlaceOrdersAsync(null!));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.PlaceOrdersAsync([]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.PlaceOrdersAsync(Enumerable.Range(0, 6).Select(_ => CreateBatchOrder(BinanceFuturesOrderType.Market))));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.CoinFutures.PlaceOrdersAsync([null!]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.PlaceOrdersAsync([CreateBatchOrder(BinanceFuturesOrderType.Stop)]));

        Assert.Equal(0, handler.RequestCount);
    }

    [Fact]
    public void BatchOrderRequest_PublicContractDoesNotExposeMigratedConditionalParameters()
    {
        var propertyNames = typeof(BinanceFuturesBatchOrderRequest)
            .GetProperties()
            .Select(property => property.Name)
            .ToList();

        Assert.DoesNotContain("StopPrice", propertyNames);
        Assert.DoesNotContain("ActivationPrice", propertyNames);
        Assert.DoesNotContain("CallbackRate", propertyNames);
        Assert.DoesNotContain("WorkingType", propertyNames);
        Assert.DoesNotContain("PriceProtect", propertyNames);
    }

    [Fact]
    public async Task PlaceAlgoOrderAsync_UsesSignedDapiBodyAndCompleteSharedResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "algoId": 2147676000001,
              "clientAlgoId": "client.algo-1",
              "algoType": "CONDITIONAL",
              "orderType": "STOP",
              "symbol": "BTCUSD_PERP",
              "side": "SELL",
              "positionSide": "BOTH",
              "timeInForce": "GTD",
              "quantity": "1.25",
              "algoStatus": "NEW",
              "triggerPrice": "50000.5",
              "price": "0",
              "icebergQuantity": "null",
              "selfTradePreventionMode": "EXPIRE_MAKER",
              "workingType": "MARK_PRICE",
              "priceMatch": "OPPONENT_5",
              "closePosition": false,
              "priceProtect": false,
              "reduceOnly": false,
              "activatePrice": "",
              "callbackRate": "",
              "createTime": 1750485492076,
              "updateTime": 1750485492077,
              "triggerTime": 0,
              "goodTillDate": 1750489092000
            }
            """);
        using var client = CreateClient(handler, limiter);
        var goodTillDate = DateTimeOffset.FromUnixTimeMilliseconds(
            DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds()).UtcDateTime;

        var result = await client.CoinFutures.PlaceAlgoOrderAsync(
            "BTCUSD_PERP",
            BinanceOrderSide.Sell,
            BinanceFuturesAlgoOrderType.Stop,
            positionSide: BinancePositionSide.Both,
            timeInForce: BinanceTimeInForce.GoodTillDate,
            quantity: 1.25m,
            triggerPrice: 50_000.5m,
            workingType: BinanceFuturesWorkingType.Mark,
            priceMatch: BinanceFuturesPriceMatch.Opponent5,
            reduceOnly: false,
            clientAlgoId: "client.algo-1",
            orderResponseType: BinanceOrderResponseType.Result,
            selfTradePreventionMode: BinanceSelfTradePreventionMode.ExpireMaker,
            goodTillDate: goodTillDate,
            receiveWindow: 60_001);

        Assert.True(result.Success);
        Assert.Equal(2_147_676_000_001L, result.Data.AlgoId);
        Assert.Equal("client.algo-1", result.Data.ClientAlgoId);
        Assert.Equal("CONDITIONAL", result.Data.AlgoType);
        Assert.Equal("STOP", result.Data.OrderType);
        Assert.Equal("BTCUSD_PERP", result.Data.Symbol);
        Assert.Equal("SELL", result.Data.Side);
        Assert.Equal("BOTH", result.Data.PositionSide);
        Assert.Equal("GTD", result.Data.TimeInForce);
        Assert.Equal(1.25m, result.Data.Quantity);
        Assert.Equal("NEW", result.Data.Status);
        Assert.Equal(50_000.5m, result.Data.TriggerPrice);
        Assert.Equal(0m, result.Data.Price);
        Assert.Equal("null", result.Data.IcebergQuantity);
        Assert.Equal("EXPIRE_MAKER", result.Data.SelfTradePreventionMode);
        Assert.Equal("MARK_PRICE", result.Data.WorkingType);
        Assert.Equal("OPPONENT_5", result.Data.PriceMatch);
        Assert.False(result.Data.ClosePosition);
        Assert.False(result.Data.PriceProtect);
        Assert.False(result.Data.ReduceOnly);
        Assert.Equal(string.Empty, result.Data.ActivatePrice);
        Assert.Equal(string.Empty, result.Data.CallbackRate);
        Assert.Equal(1_750_485_492_076L, result.Data.CreateTime);
        Assert.Equal(1_750_485_492_077L, result.Data.UpdateTime);
        Assert.Equal(0L, result.Data.TriggerTime);
        Assert.Equal(1_750_489_092_000L, result.Data.GoodTillDate);

        AssertSignedPost(handler, limiter, "/dapi/v1/algoOrder", 0);
        var body = ParseBody(handler);
        Assert.Equal("CONDITIONAL", body["algoType"]);
        Assert.Equal("BTCUSD_PERP", body["symbol"]);
        Assert.Equal("SELL", body["side"]);
        Assert.Equal("STOP", body["type"]);
        Assert.Equal("BOTH", body["positionSide"]);
        Assert.Equal("GTD", body["timeInForce"]);
        Assert.Equal("1.25", body["quantity"]);
        Assert.Equal("50000.5", body["triggerPrice"]);
        Assert.Equal("MARK_PRICE", body["workingType"]);
        Assert.Equal("OPPONENT_5", body["priceMatch"]);
        Assert.Equal("false", body["reduceOnly"]);
        Assert.Equal("client.algo-1", body["clientAlgoId"]);
        Assert.Equal("RESULT", body["newOrderRespType"]);
        Assert.Equal("EXPIRE_MAKER", body["selfTradePreventionMode"]);
        Assert.Equal(new DateTimeOffset(goodTillDate).ToUnixTimeMilliseconds().ToString(), body["goodTillDate"]);
        Assert.Equal("60001", body["recvWindow"]);
    }

    [Fact]
    public async Task PlaceAlgoOrderAsync_UsesSharedFailClosedValidationAndConfiguredReceiveWindow()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using (var client = CreateClient(handler))
        {
            await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.PlaceAlgoOrderAsync(
                "BTCUSD_PERP",
                BinanceOrderSide.Buy,
                BinanceFuturesAlgoOrderType.StopMarket,
                quantity: 1,
                closePosition: true));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.PlaceAlgoOrderAsync(
                "BTCUSD_PERP",
                BinanceOrderSide.Buy,
                BinanceFuturesAlgoOrderType.TrailingStopMarket,
                callbackRate: 10.01m));
        }
        Assert.Equal(0, handler.RequestCount);

        var configuredHandler = new RecordingHttpMessageHandler("{}");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(90_000));
        var result = await configuredClient.CoinFutures.PlaceAlgoOrderAsync(
            "BTCUSD_PERP",
            BinanceOrderSide.Sell,
            BinanceFuturesAlgoOrderType.TrailingStopMarket,
            quantity: 2,
            activatePrice: 50_000,
            callbackRate: 0.1m);

        Assert.True(result.Success);
        Assert.Equal("90000", ParseBody(configuredHandler)["recvWindow"]);
    }

    [Fact]
    public async Task CancelAlgoOrderAsync_UsesSignedDapiQueryAndValidatesInputs()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"algoId":2147676000001,"clientAlgoId":"client-only","code":"200","msg":"success"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.CancelAlgoOrderAsync(
            algoId: 2_147_676_000_001L,
            clientAlgoId: "client-only",
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.Equal(2_147_676_000_001L, result.Data.AlgoId);
        Assert.Equal("client-only", result.Data.ClientAlgoId);
        Assert.Equal("200", result.Data.Code);
        Assert.Equal("success", result.Data.Message);
        AssertSignedQuery(handler, limiter, HttpMethod.Delete, "/dapi/v1/algoOrder", 1);
        var query = DecodedQuery(handler);
        Assert.Contains("algoId=2147676000001", query);
        Assert.Contains("clientAlgoId=client-only", query);
        Assert.Contains("recvWindow=60000", query);

        var invalidHandler = new RecordingHttpMessageHandler("{}");
        using var invalidClient = CreateClient(invalidHandler);
        await Assert.ThrowsAsync<ArgumentException>(() => invalidClient.CoinFutures.CancelAlgoOrderAsync());
        await Assert.ThrowsAsync<ArgumentException>(() => invalidClient.CoinFutures.CancelAlgoOrderAsync(clientAlgoId: " "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => invalidClient.CoinFutures.CancelAlgoOrderAsync(algoId: 1, receiveWindow: 60_001));
        Assert.Equal(0, invalidHandler.RequestCount);
    }

    [Fact]
    public async Task CancelAlgoOrderAsync_AcceptsClientIdentifierAlternativeAndConfiguredReceiveWindow()
    {
        var handler = new RecordingHttpMessageHandler(
            """{"algoId":2146760,"clientAlgoId":"client-only","code":"200","msg":"success"}""");
        using var client = CreateClient(
            handler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(5_000));

        var result = await client.CoinFutures.CancelAlgoOrderAsync(clientAlgoId: "client-only");

        Assert.True(result.Success);
        var query = DecodedQuery(handler);
        Assert.Contains("clientAlgoId=client-only", query);
        Assert.Contains("recvWindow=5000", query);
        Assert.DoesNotContain("algoId=", query);
    }

    [Fact]
    public async Task GetOpenAlgoOrdersAsync_UsesCurrentFiltersWeightAndResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "algoId": 2148627,
                "clientAlgoId": "client-open",
                "algoType": "CONDITIONAL",
                "orderType": "TAKE_PROFIT",
                "symbol": "BTCUSD_PERP",
                "side": "SELL",
                "positionSide": "BOTH",
                "timeInForce": "GTC",
                "quantity": "1",
                "algoStatus": "NEW",
                "actualOrderId": "",
                "actualPrice": "0",
                "triggerPrice": "51000",
                "price": "51010",
                "icebergQuantity": "null",
                "tpTriggerPrice": "52000",
                "tpPrice": "52010",
                "slTriggerPrice": "49000",
                "slPrice": "48990",
                "tpOrderType": "",
                "selfTradePreventionMode": "EXPIRE_MAKER",
                "workingType": "CONTRACT_PRICE",
                "priceMatch": "NONE",
                "closePosition": false,
                "priceProtect": false,
                "reduceOnly": false,
                "createTime": 1750514941540,
                "updateTime": 1750514941541,
                "triggerTime": 0,
                "goodTillDate": 0
              }
            ]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetOpenAlgoOrdersAsync(
            "BTCUSD_PERP",
            "CONDITIONAL",
            2_148_627,
            60_000);

        Assert.True(result.Success);
        AssertSignedQuery(handler, limiter, HttpMethod.Get, "/dapi/v1/openAlgoOrders", 1);
        var query = DecodedQuery(handler);
        Assert.Contains("symbol=BTCUSD_PERP", query);
        Assert.Contains("algoType=CONDITIONAL", query);
        Assert.Contains("algoId=2148627", query);
        Assert.Contains("recvWindow=60000", query);
        var order = Assert.Single(result.Data);
        Assert.Equal(2_148_627, order.AlgoId);
        Assert.Equal("BTCUSD_PERP", order.Symbol);
        Assert.Equal("NEW", order.Status);
        Assert.Equal(51_000m, order.TriggerPrice);
        Assert.Equal(52_000m, order.TakeProfitTriggerPrice);
        Assert.Equal(49_000m, order.StopLossTriggerPrice);
        Assert.Equal(1_750_514_941_541L, order.UpdateTime);
    }

    [Fact]
    public async Task GetOpenAlgoOrdersAsync_UsesAllSymbolsWeightAndRejectsInvalidFilters()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetOpenAlgoOrdersAsync();

        Assert.True(result.Success);
        AssertSignedQuery(handler, limiter, HttpMethod.Get, "/dapi/v1/openAlgoOrders", 40);
        Assert.DoesNotContain("symbol=", DecodedQuery(handler));

        var invalidHandler = new RecordingHttpMessageHandler("[]");
        using var invalidClient = CreateClient(invalidHandler);
        await Assert.ThrowsAsync<ArgumentException>(() => invalidClient.CoinFutures.GetOpenAlgoOrdersAsync(symbol: " "));
        await Assert.ThrowsAsync<ArgumentException>(() => invalidClient.CoinFutures.GetOpenAlgoOrdersAsync(algoType: " "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => invalidClient.CoinFutures.GetOpenAlgoOrdersAsync(receiveWindow: 60_001));
        Assert.Equal(0, invalidHandler.RequestCount);

        var configuredHandler = new RecordingHttpMessageHandler("[]");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => configuredClient.CoinFutures.GetOpenAlgoOrdersAsync());
        Assert.Equal(0, configuredHandler.RequestCount);
    }

    private static BinanceFuturesBatchOrderRequest CreateBatchOrder(BinanceFuturesOrderType type)
        => new()
        {
            Symbol = "BTCUSD_PERP",
            Side = BinanceOrderSide.Buy,
            Type = type,
            Quantity = 1,
            Price = type == BinanceFuturesOrderType.Limit ? 50_000 : null,
            TimeInForce = type == BinanceFuturesOrderType.Limit ? BinanceTimeInForce.GoodTillCanceled : null
        };

    private static Dictionary<string, string> ParseBody(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.Body!).Split('&').Select(value => value.Split('=', 2)).ToDictionary(value => value[0], value => value[1]);

    private static string DecodedQuery(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.RequestUri!.Query);

    private static void AssertSignedPost(RecordingHttpMessageHandler handler, RecordingRateLimiter limiter, string path, int weight)
    {
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Equal("application/x-www-form-urlencoded", handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.Contains("timestamp=", handler.Body);
        Assert.Contains(limiter.Requests, item => item.Endpoint == path && item.Weight == weight && item.Signed);
    }

    private static void AssertSignedQuery(RecordingHttpMessageHandler handler, RecordingRateLimiter limiter, HttpMethod method, string path, int weight)
    {
        Assert.Equal(method, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        Assert.Contains("timestamp=", handler.RequestUri.Query);
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == path && item.Weight == weight && item.Signed);
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

    private sealed class SingleUseEnumerable<T>(IEnumerable<T> items) : IEnumerable<T>
    {
        internal int EnumerationCount { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            EnumerationCount++;
            if (EnumerationCount > 1)
                throw new InvalidOperationException("The source was enumerated more than once");

            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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
