using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Options;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsOrderMutationTests
{
    private const string Symbol = "BTC-260925-50000-C";

    [Fact]
    public async Task PlaceOrder_UsesCurrentContractWeightAndResponseFields()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"orderId":9223372036854775807,"symbol":"BTC-260925-50000-C","quantity":"2.5","createTime":1730170445600,"source":"API","selfTradePreventionMode":"EXPIRE_TAKER"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.PlaceOrderAsync(
            Symbol,
            BinanceOrderSide.Buy,
            BinanceOptionsOrderType.Limit,
            2.5m,
            100.25m,
            BinanceTimeInForce.GoodTillCanceled,
            "client-1",
            reduceOnly: true,
            postOnly: false,
            isMmp: true,
            receiveWindow: 60_000L,
            orderResponseType: BinanceOrderResponseType.Result,
            selfTradePreventionMode: BinanceSelfTradePreventionMode.ExpireTaker);

        Assert.True(result.Success);
        Assert.Equal(long.MaxValue, result.Data.Id);
        Assert.Equal("API", result.Data.Source);
        Assert.Equal(BinanceSelfTradePreventionMode.ExpireTaker, result.Data.SelfTradePreventionMode);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_730_170_445_600).UtcDateTime, result.Data.CreateTime);
        AssertSignedRequest(handler, limiter, HttpMethod.Post, "/eapi/v1/order", 0, hasBody: true);

        var body = ParseBody(handler);
        Assert.Equal(Symbol, body["symbol"]);
        Assert.Equal("BUY", body["side"]);
        Assert.Equal("LIMIT", body["type"]);
        Assert.Equal("2.5", body["quantity"]);
        Assert.Equal("100.25", body["price"]);
        Assert.Equal("GTC", body["timeInForce"]);
        Assert.Equal("client-1", body["clientOrderId"]);
        Assert.Equal("RESULT", body["newOrderRespType"]);
        Assert.Equal("EXPIRE_TAKER", body["selfTradePreventionMode"]);
        Assert.Equal("60000", body["recvWindow"]);
    }

    [Fact]
    public async Task PlaceOrder_OmitsOptionalResponseAndStpToPreserveOfficialDefaults()
    {
        var handler = new RecordingHttpMessageHandler("""{"orderId":1}""");
        using var client = CreateClient(handler);

        var result = await client.Options.PlaceOrderAsync(Symbol, BinanceOrderSide.Sell, BinanceOptionsOrderType.Limit, 1m);

        Assert.True(result.Success);
        var body = ParseBody(handler);
        Assert.False(body.ContainsKey("newOrderRespType"));
        Assert.False(body.ContainsKey("selfTradePreventionMode"));
    }

    [Fact]
    public async Task PlaceOrder_RejectsEnumsOutsideTheOptionsContractBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.PlaceOrderAsync(Symbol, (BinanceOrderSide)99, BinanceOptionsOrderType.Limit, 1m));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.PlaceOrderAsync(Symbol, BinanceOrderSide.Buy, (BinanceOptionsOrderType)99, 1m));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.PlaceOrderAsync(Symbol, BinanceOrderSide.Buy, BinanceOptionsOrderType.Limit, 1m, timeInForce: BinanceTimeInForce.GoodTillDate));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.PlaceOrderAsync(Symbol, BinanceOrderSide.Buy, BinanceOptionsOrderType.Limit, 1m, orderResponseType: BinanceOrderResponseType.Full));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.PlaceOrderAsync(Symbol, BinanceOrderSide.Buy, BinanceOptionsOrderType.Limit, 1m, selfTradePreventionMode: BinanceSelfTradePreventionMode.Decrement));
        Assert.Equal(0, handler.RequestCount);
    }

    [Fact]
    public async Task PlaceOrders_AcceptsTenAndEnumeratesInputOnce()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);
        var enumerationCount = 0;
        var orders = Enumerable.Range(1, 10)
            .Select(index => new BinanceOptionsBatchOrderRequest(Symbol, BinanceOrderSide.Buy, BinanceOptionsOrderType.Limit, index)
            {
                Price = 100m + index,
                OrderResponseType = index == 1 ? BinanceOrderResponseType.Acknowledge : null,
                SelfTradePreventionMode = index == 1 ? BinanceSelfTradePreventionMode.ExpireMaker : null
            })
            .ToList();

        var result = await client.Options.PlaceOrdersAsync(EnumerateOnce(orders, () => enumerationCount++), 60_000L);

        Assert.True(result.Success);
        Assert.Equal(1, enumerationCount);
        AssertSignedRequest(handler, limiter, HttpMethod.Post, "/eapi/v1/batchOrders", 5, hasBody: true);
        var serializedOrders = JArray.Parse(ParseBody(handler)["orders"]);
        Assert.Equal(10, serializedOrders.Count);
        Assert.Equal("1", serializedOrders[0]!["quantity"]!.Value<string>());
        Assert.Equal("ACK", serializedOrders[0]!["newOrderRespType"]!.Value<string>());
        Assert.Equal("EXPIRE_MAKER", serializedOrders[0]!["selfTradePreventionMode"]!.Value<string>());
        Assert.Null(serializedOrders[1]!["newOrderRespType"]);
        Assert.Null(serializedOrders[1]!["selfTradePreventionMode"]);
    }

    [Fact]
    public async Task PlaceOrders_RejectsInvalidCollectionsBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);
        var valid = new BinanceOptionsBatchOrderRequest(Symbol, BinanceOrderSide.Buy, BinanceOptionsOrderType.Limit, 1m);

        await Assert.ThrowsAsync<ArgumentNullException>(() => client.Options.PlaceOrdersAsync(null!));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.PlaceOrdersAsync([]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.PlaceOrdersAsync(Enumerable.Repeat(valid, 11)));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.PlaceOrdersAsync([valid, null!]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.PlaceOrdersAsync([
            new BinanceOptionsBatchOrderRequest(Symbol, BinanceOrderSide.Buy, BinanceOptionsOrderType.Limit, 1m)
            {
                SelfTradePreventionMode = BinanceSelfTradePreventionMode.None
            }
        ]));
        Assert.Equal(0, handler.RequestCount);
    }

    [Fact]
    public async Task CancelOrder_UsesQueryAndMapsCancelSpecificResponseFields()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"orderId":9223372036854775807,"symbol":"BTC-260925-50000-C","createDate":1730170445600,"source":"API","selfTradePreventionMode":"EXPIRE_BOTH"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.CancelOrderAsync(Symbol, orderId: long.MaxValue, receiveWindow: 60_000L);

        Assert.True(result.Success);
        Assert.Equal(long.MaxValue, result.Data.Id);
        Assert.Equal("API", result.Data.Source);
        Assert.Equal(BinanceSelfTradePreventionMode.ExpireBoth, result.Data.SelfTradePreventionMode);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_730_170_445_600).UtcDateTime, result.Data.CreateTime);
        AssertSignedRequest(handler, limiter, HttpMethod.Delete, "/eapi/v1/order", 1, hasBody: false);
        var query = ParseQuery(handler.RequestUri!);
        Assert.Equal(Symbol, query["symbol"]);
        Assert.Equal(long.MaxValue.ToString(), query["orderId"]);
        Assert.Equal("60000", query["recvWindow"]);
    }

    [Fact]
    public async Task CancelOrders_UsesEscapedQueryArraysWeightFiveAndSingleEnumeration()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);
        var orderIdEnumerationCount = 0;
        var clientIdEnumerationCount = 0;

        var result = await client.Options.CancelOrdersAsync(
            Symbol,
            EnumerateOnce([1L, long.MaxValue], () => orderIdEnumerationCount++),
            EnumerateOnce(["client-1", "quoted\"client"], () => clientIdEnumerationCount++),
            60_000L);

        Assert.True(result.Success);
        Assert.Equal(1, orderIdEnumerationCount);
        Assert.Equal(1, clientIdEnumerationCount);
        AssertSignedRequest(handler, limiter, HttpMethod.Delete, "/eapi/v1/batchOrders", 5, hasBody: false);
        var query = ParseQuery(handler.RequestUri!);
        Assert.Equal([1L, long.MaxValue], JsonConvert.DeserializeObject<List<long>>(query["orderIds"]));
        Assert.Equal(["client-1", "quoted\"client"], JsonConvert.DeserializeObject<List<string>>(query["clientOrderIds"]));
    }

    [Fact]
    public async Task CancelOrders_RejectsEmptyOversizedOrBlankIdentifiersBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.CancelOrdersAsync(Symbol));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.CancelOrdersAsync(Symbol, [], []));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.CancelOrdersAsync(Symbol, Enumerable.Range(1, 11).Select(value => (long)value)));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.CancelOrdersAsync(Symbol, origClientOrderIdList: ["client", " "]));
        Assert.Equal(0, handler.RequestCount);
    }

    [Fact]
    public async Task CancelAllMutations_ReturnExactAcknowledgementsAndCurrentWeights()
    {
        var underlyingLimiter = new RecordingRateLimiter();
        var underlyingHandler = new RecordingHttpMessageHandler("""{"code":9223372036854775807,"msg":"success"}""");
        using (var underlyingClient = CreateClient(underlyingHandler, underlyingLimiter))
        {
            var result = await underlyingClient.Options.CancelOrdersByUnderlyingAsync("BTCUSDT", 60_000L);
            Assert.True(result.Success);
            Assert.Equal(long.MaxValue, result.Data.Code);
            Assert.Equal("success", result.Data.Message);
            AssertSignedRequest(underlyingHandler, underlyingLimiter, HttpMethod.Delete, "/eapi/v1/allOpenOrdersByUnderlying", 5, hasBody: false);
            Assert.Equal("BTCUSDT", ParseQuery(underlyingHandler.RequestUri!)["underlying"]);
        }

        var symbolLimiter = new RecordingRateLimiter();
        var symbolHandler = new RecordingHttpMessageHandler("""{"code":"200","msg":"success"}""");
        using var symbolClient = CreateClient(symbolHandler, symbolLimiter);
        var symbolResult = await symbolClient.Options.CancelOrdersBySymbolAsync(Symbol, 60_000L);
        Assert.True(symbolResult.Success);
        Assert.Equal("200", symbolResult.Data.Code);
        Assert.Equal("success", symbolResult.Data.Message);
        AssertSignedRequest(symbolHandler, symbolLimiter, HttpMethod.Delete, "/eapi/v1/allOpenOrders", 1, hasBody: false);
        Assert.Equal(Symbol, ParseQuery(symbolHandler.RequestUri!)["symbol"]);
    }

    [Fact]
    public async Task AllTouchedMutations_RejectReceiveWindowAboveMaximumBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);
        var order = new BinanceOptionsBatchOrderRequest(Symbol, BinanceOrderSide.Buy, BinanceOptionsOrderType.Limit, 1m);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.PlaceOrderAsync(Symbol, BinanceOrderSide.Buy, BinanceOptionsOrderType.Limit, 1m, receiveWindow: 60_001L));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.PlaceOrdersAsync([order], 60_001L));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.CancelOrderAsync(Symbol, orderId: 1, receiveWindow: 60_001L));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.CancelOrdersAsync(Symbol, [1], receiveWindow: 60_001L));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.CancelOrdersByUnderlyingAsync("BTCUSDT", 60_001L));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.CancelOrdersBySymbolAsync(Symbol, 60_001L));
        Assert.Equal(0, handler.RequestCount);

        var configuredHandler = new RecordingHttpMessageHandler("{}");
        using var configuredClient = CreateClient(configuredHandler, defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => configuredClient.Options.CancelOrdersBySymbolAsync(Symbol));
        Assert.Equal(0, configuredHandler.RequestCount);
    }

    [Fact]
    public void PublicMutationContracts_ExposeRequiredQuantitiesAndInt64ReceiveWindows()
    {
        var placeOrder = typeof(IBinanceOptionsRestClientTrading).GetMethod(nameof(IBinanceOptionsRestClientTrading.PlaceOrderAsync))!;
        Assert.Equal(typeof(decimal), placeOrder.GetParameters().Single(parameter => parameter.Name == "quantity").ParameterType);
        Assert.Equal(typeof(long?), placeOrder.GetParameters().Single(parameter => parameter.Name == "receiveWindow").ParameterType);
        Assert.Equal(typeof(decimal), typeof(BinanceOptionsBatchOrderRequest).GetProperty(nameof(BinanceOptionsBatchOrderRequest.Quantity))!.PropertyType);
        Assert.DoesNotContain(typeof(BinanceOptionsBatchOrderRequest).GetConstructors(), constructor => constructor.GetParameters().Length == 0);

        foreach (var methodName in new[]
                 {
                     nameof(IBinanceOptionsRestClientTrading.PlaceOrdersAsync),
                     nameof(IBinanceOptionsRestClientTrading.CancelOrderAsync),
                     nameof(IBinanceOptionsRestClientTrading.CancelOrdersAsync),
                     nameof(IBinanceOptionsRestClientTrading.CancelOrdersByUnderlyingAsync),
                     nameof(IBinanceOptionsRestClientTrading.CancelOrdersBySymbolAsync)
                 })
        {
            var method = typeof(IBinanceOptionsRestClientTrading).GetMethod(methodName)!;
            Assert.Equal(typeof(long?), method.GetParameters().Single(parameter => parameter.Name == "receiveWindow").ParameterType);
        }
    }

    private static IEnumerable<T> EnumerateOnce<T>(IEnumerable<T> values, Action onEnumeration)
    {
        onEnumeration();
        foreach (var value in values)
            yield return value;
    }

    private static Dictionary<string, string> ParseBody(RecordingHttpMessageHandler handler)
        => ParseParameters(handler.Body!);

    private static Dictionary<string, string> ParseQuery(Uri uri)
        => ParseParameters(uri.Query.TrimStart('?'));

    private static Dictionary<string, string> ParseParameters(string parameters)
        => parameters.Split('&')
            .Select(value => value.Split('=', 2))
            .ToDictionary(value => Uri.UnescapeDataString(value[0]), value => Uri.UnescapeDataString(value[1]));

    private static void AssertSignedRequest(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        HttpMethod method,
        string path,
        int weight,
        bool hasBody)
    {
        Assert.Equal(method, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Equal(hasBody, handler.Body != null);
        Assert.Equal(hasBody ? "application/x-www-form-urlencoded" : null, handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
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
            ReceiveWindow = defaultReceiveWindow,
            AllowAppendingClientOrderId = false
        };
        options.EuropeanOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.None;
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
