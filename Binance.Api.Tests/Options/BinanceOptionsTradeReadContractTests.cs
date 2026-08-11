using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Options;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsTradeReadContractTests
{
    private const string Symbol = "BTC-251226-90000-C";

    [Fact]
    public async Task GetOrder_UsesExactRequestAndSingleOrderResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"orderId":4611875134427365000,"symbol":"BTC-251226-90000-C","price":"100","quantity":"1","executedQty":"0.5","side":"BUY","type":"LIMIT","timeInForce":"GTC","reduceOnly":false,"postOnly":true,"createTime":1762779600000,"updateTime":1762780499999,"status":"PARTIALLY_FILLED","avgPrice":"99.5","clientOrderId":"client-1","priceScale":3000000000,"quantityScale":3000000001,"optionSide":"CALL","quoteAsset":"USDT","mmp":false,"selfTradePreventionMode":"EXPIRE_MAKER"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.GetOrderAsync(Symbol, orderId: 4_611_875_134_427_365_000, receiveWindow: 60_000);

        Assert.True(result.Success);
        var order = result.Data;
        Assert.Equal(4_611_875_134_427_365_000, order.Id);
        Assert.Equal(Symbol, order.Symbol);
        Assert.Equal(100m, order.Price);
        Assert.Equal(1m, order.Quantity);
        Assert.Equal(0.5m, order.ExecutedQuantity);
        Assert.Equal(BinanceOrderSide.Buy, order.Side);
        Assert.Equal(BinanceOptionsOrderType.Limit, order.Type);
        Assert.Equal(BinanceTimeInForce.GoodTillCanceled, order.TimeInForce);
        Assert.False(order.ReduceOnly);
        Assert.True(order.PostOnly);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1762779600000).UtcDateTime, order.CreateTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1762780499999).UtcDateTime, order.UpdateTime);
        Assert.Equal("PARTIALLY_FILLED", order.Status);
        Assert.Equal(99.5m, order.AveragePrice);
        Assert.Equal("client-1", order.ClientOrderId);
        Assert.Equal(3_000_000_000L, order.PriceScale);
        Assert.Equal(3_000_000_001L, order.QuantityScale);
        Assert.Equal(BinanceOptionsSide.Call, order.OptionSide);
        Assert.Equal("USDT", order.QuoteAsset);
        Assert.False(order.MMP);
        Assert.Equal(BinanceSelfTradePreventionMode.ExpireMaker, order.SelfTradePreventionMode);
        AssertSignedGet(handler, limiter, "/eapi/v1/order", 1);
        AssertQuery(handler, "symbol=BTC-251226-90000-C", "orderId=4611875134427365000", "recvWindow=60000");
        Assert.Null(typeof(BinanceOptionsOrderQuery).GetProperty("Fee"));
        Assert.Null(typeof(BinanceOptionsOrderQuery).GetProperty("Source"));
    }

    [Fact]
    public async Task GetOrdersHistory_UsesExactParametersAndHistoryResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """[{"orderId":4611875134427365001,"symbol":"BTC-251226-90000-C","price":"101","quantity":"2","executedQty":"2","side":"SELL","type":"LIMIT","timeInForce":"IOC","reduceOnly":true,"createTime":1762779600000,"updateTime":1762780499999,"status":"FILLED","avgPrice":"101","clientOrderId":"client-2","priceScale":3000000000,"quantityScale":3000000001,"optionSide":"CALL","quoteAsset":"USDT","mmp":true}]""");
        using var client = CreateClient(handler, limiter);
        var start = DateTimeOffset.FromUnixTimeMilliseconds(1_762_779_600_000).UtcDateTime;
        var end = DateTimeOffset.FromUnixTimeMilliseconds(1_762_780_499_999).UtcDateTime;

        var result = await client.Options.GetOrdersHistoryAsync(Symbol, 4_611_875_134_427_365_001, start, end, 1000, 60_000);

        Assert.True(result.Success);
        var order = Assert.Single(result.Data);
        Assert.Equal(4_611_875_134_427_365_001, order.Id);
        Assert.Equal(BinanceOrderSide.Sell, order.Side);
        Assert.Equal(BinanceTimeInForce.ImmediateOrCancel, order.TimeInForce);
        Assert.True(order.ReduceOnly);
        Assert.Equal("FILLED", order.Status);
        Assert.Equal(3_000_000_000L, order.PriceScale);
        Assert.Equal(3_000_000_001L, order.QuantityScale);
        Assert.True(order.MMP);
        AssertSignedGet(handler, limiter, "/eapi/v1/historyOrders", 3);
        AssertQuery(handler, "symbol=BTC-251226-90000-C", "orderId=4611875134427365001", "startTime=1762779600000", "endTime=1762780499999", "limit=1000", "recvWindow=60000");
        Assert.Null(typeof(BinanceOptionsOrderHistory).GetProperty("PostOnly"));
        Assert.Null(typeof(BinanceOptionsOrderHistory).GetProperty("SelfTradePreventionMode"));
        Assert.Null(typeof(BinanceOptionsOrderHistory).GetProperty("Fee"));
        Assert.Null(typeof(BinanceOptionsOrderHistory).GetProperty("Source"));
    }

    [Fact]
    public async Task GetOpenOrders_UsesSymbolDependentWeightAndNoLimitParameter()
    {
        var filteredLimiter = new RecordingRateLimiter();
        var filteredHandler = new RecordingHttpMessageHandler(
            """[{"orderId":4611875134427365002,"symbol":"BTC-251226-90000-C","price":"102","quantity":"3","executedQty":"1","side":"BUY","type":"LIMIT","timeInForce":"FOK","reduceOnly":false,"createTime":1762779600000,"updateTime":1762780499999,"status":"PARTIALLY_FILLED","avgPrice":"102","clientOrderId":"client-3","priceScale":3000000000,"quantityScale":3000000001,"optionSide":"CALL","quoteAsset":"USDT","mmp":false,"selfTradePreventionMode":"EXPIRE_BOTH"}]""");
        using (var filteredClient = CreateClient(filteredHandler, filteredLimiter))
        {
            var result = await filteredClient.Options.GetOpenOrdersAsync(Symbol, 4_611_875_134_427_365_002, receiveWindow: 60_000);

            Assert.True(result.Success);
            var order = Assert.Single(result.Data);
            Assert.Equal(4_611_875_134_427_365_002, order.Id);
            Assert.Equal(BinanceSelfTradePreventionMode.ExpireBoth, order.SelfTradePreventionMode);
            Assert.Equal(3_000_000_000L, order.PriceScale);
            AssertSignedGet(filteredHandler, filteredLimiter, "/eapi/v1/openOrders", 1);
            AssertQuery(filteredHandler, "symbol=BTC-251226-90000-C", "orderId=4611875134427365002", "recvWindow=60000");
            Assert.DoesNotContain("limit=", Uri.UnescapeDataString(filteredHandler.RequestUri!.Query));
        }

        var allLimiter = new RecordingRateLimiter();
        var allHandler = new RecordingHttpMessageHandler("[]");
        using var allClient = CreateClient(allHandler, allLimiter);

        var allResult = await allClient.Options.GetOpenOrdersAsync();

        Assert.True(allResult.Success);
        Assert.Empty(allResult.Data);
        AssertSignedGet(allHandler, allLimiter, "/eapi/v1/openOrders", 40);
        Assert.Null(typeof(BinanceOptionsOpenOrder).GetProperty("PostOnly"));
        Assert.Null(typeof(BinanceOptionsOpenOrder).GetProperty("Fee"));
        Assert.Null(typeof(BinanceOptionsOpenOrder).GetProperty("Source"));
    }

    [Fact]
    public async Task GetPositions_DeserializesCompleteCurrentResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """[{"entryPrice":"1000","symbol":"BTC-251226-90000-C","side":"SHORT","quantity":"-0.1","markValue":"105.00138","unrealizedPNL":"-5.00138","markPrice":"1050.0138","strikePrice":"90000","expiryDate":1766707200000,"priceScale":3000000000,"quantityScale":3000000001,"optionSide":"CALL","quoteAsset":"USDT","time":1762872654561,"bidQuantity":"0.25","askQuantity":"0.5"}]""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.GetPositionsAsync(Symbol, 60_000);

        Assert.True(result.Success);
        var position = Assert.Single(result.Data);
        Assert.Equal(1000m, position.AverageEntryPrice);
        Assert.Equal(BinancePositionSide.Short, position.Side);
        Assert.Equal(-0.1m, position.Quantity);
        Assert.Equal(105.00138m, position.MarkValue);
        Assert.Equal(-5.00138m, position.UnrealizedPNL);
        Assert.Equal(1050.0138m, position.MarkPrice);
        Assert.Equal(90_000m, position.StrikePrice);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1766707200000).UtcDateTime, position.ExpiryDate);
        Assert.Equal(3_000_000_000L, position.PriceScale);
        Assert.Equal(3_000_000_001L, position.QuantityScale);
        Assert.Equal(BinanceOptionsSide.Call, position.OptionsSide);
        Assert.Equal("USDT", position.QuoteAsset);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1762872654561).UtcDateTime, position.Time);
        Assert.Equal(0.25m, position.BidQuantity);
        Assert.Equal(0.5m, position.AskQuantity);
        AssertSignedGet(handler, limiter, "/eapi/v1/position", 5);
        AssertQuery(handler, "symbol=BTC-251226-90000-C", "recvWindow=60000");
        Assert.Null(typeof(BinanceOptionsPosition).GetProperty("ReducibleQuantity"));
        Assert.Null(typeof(BinanceOptionsPosition).GetProperty("RateOfReturn"));
        Assert.Null(typeof(BinanceOptionsPosition).GetProperty("PositionCost"));
    }

    [Fact]
    public async Task GetUserExerciseRecords_PreservesStringIdPositionSideAndInt64Scales()
    {
        var limiter = new RecordingRateLimiter();
        const string recordId = "11258999068426240420";
        var handler = new RecordingHttpMessageHandler(
            $$"""[{"id":"{{recordId}}","currency":"USDT","symbol":"{{Symbol}}","exercisePrice":"90000","quantity":"1","amount":"250","fee":"0.5","createDate":1766707200000,"priceScale":3000000000,"quantityScale":3000000001,"optionSide":"CALL","positionSide":"LONG","quoteAsset":"USDT"}]""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.GetUserExerciseRecordsAsync(Symbol, limit: 1000, receiveWindow: 60_000);

        Assert.True(result.Success);
        var exercise = Assert.Single(result.Data);
        Assert.Equal(recordId, exercise.Id);
        Assert.Equal("USDT", exercise.Currency);
        Assert.Equal(Symbol, exercise.Symbol);
        Assert.Equal(90_000m, exercise.ExercisePrice);
        Assert.Equal(1m, exercise.Quantity);
        Assert.Equal(250m, exercise.Amount);
        Assert.Equal(0.5m, exercise.Fee);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1766707200000).UtcDateTime, exercise.CreateDate);
        Assert.Equal(3_000_000_000L, exercise.PriceScale);
        Assert.Equal(3_000_000_001L, exercise.QuantityScale);
        Assert.Equal(BinanceOptionsSide.Call, exercise.OptionSide);
        Assert.Equal(BinancePositionSide.Long, exercise.PositionSide);
        Assert.Equal("USDT", exercise.QuoteAsset);
        AssertSignedGet(handler, limiter, "/eapi/v1/exerciseRecord", 5);
        AssertQuery(handler, "symbol=BTC-251226-90000-C", "limit=1000", "recvWindow=60000");
        Assert.Null(typeof(BinanceOptionsUserExercise).GetProperty("MarkPrice"));
        Assert.Null(typeof(BinanceOptionsUserExercise).GetProperty("Side"));
    }

    [Fact]
    public async Task GetUserTrades_RequiresSymbolAndDeserializesCompleteCurrentResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """[{"id":4611875134427365000,"tradeId":3000000000,"orderId":4611875134427365001,"symbol":"BTC-251226-90000-C","price":"100","quantity":"1","fee":"-1.04378629","realizedProfit":"2.5","side":"BUY","type":"LIMIT","liquidity":"TAKER","time":1762780499999,"priceScale":3000000000,"quantityScale":3000000001,"optionSide":"CALL","quoteAsset":"USDT"}]""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.GetUserTradesAsync(Symbol, 3_000_000_000, limit: 1000, receiveWindow: 60_000);

        Assert.True(result.Success);
        var trade = Assert.Single(result.Data);
        Assert.Equal(4_611_875_134_427_365_000, trade.Id);
        Assert.Equal(3_000_000_000L, trade.TradeId);
        Assert.Equal(4_611_875_134_427_365_001, trade.OrderId);
        Assert.Equal(Symbol, trade.Symbol);
        Assert.Equal(100m, trade.Price);
        Assert.Equal(1m, trade.Quantity);
        Assert.Equal(-1.04378629m, trade.Fee);
        Assert.Equal(2.5m, trade.RealizedProfit);
        Assert.Equal(BinanceOrderSide.Buy, trade.Side);
        Assert.Equal(BinanceOptionsOrderType.Limit, trade.Type);
        Assert.Equal(BinanceOptionsLiquidity.Taker, trade.Liquidity);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1762780499999).UtcDateTime, trade.Time);
        Assert.Equal(3_000_000_000L, trade.PriceScale);
        Assert.Equal(3_000_000_001L, trade.QuantityScale);
        Assert.Equal(BinanceOptionsSide.Call, trade.OptionSide);
        Assert.Equal("USDT", trade.QuoteAsset);
        AssertSignedGet(handler, limiter, "/eapi/v1/userTrades", 5);
        AssertQuery(handler, "symbol=BTC-251226-90000-C", "fromId=3000000000", "limit=1000", "recvWindow=60000");
        Assert.Null(typeof(BinanceOptionsUserTrade).GetProperty("Volatility"));
    }

    [Fact]
    public async Task ReadMethods_RejectInvalidIdentifiersLimitsAndReceiveWindowsBeforeTransport()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("[]"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetOrderAsync(" ", orderId: 1));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetOrderAsync(Symbol));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetOrderAsync(Symbol, clientOrderId: " "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetOrdersHistoryAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetOpenOrdersAsync(symbol: " "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetPositionsAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetUserExerciseRecordsAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetUserTradesAsync(" "));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetOrdersHistoryAsync(Symbol, limit: 0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetUserExerciseRecordsAsync(limit: 1001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetUserTradesAsync(Symbol, limit: 1001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetOrderAsync(Symbol, orderId: 1, receiveWindow: 60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetOrdersHistoryAsync(Symbol, receiveWindow: 60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetOpenOrdersAsync(receiveWindow: 60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetPositionsAsync(receiveWindow: 60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetUserExerciseRecordsAsync(receiveWindow: 60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetUserTradesAsync(Symbol, receiveWindow: 60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetUserCommissionAsync(60_001));
    }

    [Fact]
    public void ReadMethodSignatures_ExposeCurrentInt64AndEndpointSpecificContracts()
    {
        var type = typeof(IBinanceOptionsRestClientTrading);

        Assert.Equal(typeof(Task<RestCallResult<BinanceOptionsOrderQuery>>), type.GetMethod(nameof(IBinanceOptionsRestClientTrading.GetOrderAsync))!.ReturnType);
        Assert.Equal(typeof(Task<RestCallResult<List<BinanceOptionsOrderHistory>>>), type.GetMethod(nameof(IBinanceOptionsRestClientTrading.GetOrdersHistoryAsync))!.ReturnType);
        var openOrders = type.GetMethod(nameof(IBinanceOptionsRestClientTrading.GetOpenOrdersAsync))!;
        Assert.Equal(typeof(Task<RestCallResult<List<BinanceOptionsOpenOrder>>>), openOrders.ReturnType);
        Assert.DoesNotContain(openOrders.GetParameters(), parameter => parameter.Name == "limit");

        foreach (var methodName in new[]
                 {
                     nameof(IBinanceOptionsRestClientTrading.GetOrderAsync),
                     nameof(IBinanceOptionsRestClientTrading.GetOrdersHistoryAsync),
                     nameof(IBinanceOptionsRestClientTrading.GetOpenOrdersAsync),
                     nameof(IBinanceOptionsRestClientTrading.GetPositionsAsync),
                     nameof(IBinanceOptionsRestClientTrading.GetUserExerciseRecordsAsync),
                     nameof(IBinanceOptionsRestClientTrading.GetUserTradesAsync),
                     nameof(IBinanceOptionsRestClientTrading.GetUserCommissionAsync)
                 })
        {
            var method = type.GetMethod(methodName)!;
            Assert.Equal(typeof(long?), method.GetParameters().Single(parameter => parameter.Name == "receiveWindow").ParameterType);
        }

        foreach (var methodName in new[]
                 {
                     nameof(IBinanceOptionsRestClientTrading.GetOrdersHistoryAsync),
                     nameof(IBinanceOptionsRestClientTrading.GetUserExerciseRecordsAsync),
                     nameof(IBinanceOptionsRestClientTrading.GetUserTradesAsync)
                 })
        {
            var method = type.GetMethod(methodName)!;
            Assert.Equal(typeof(long?), method.GetParameters().Single(parameter => parameter.Name == "limit").ParameterType);
        }

        Assert.False(type.GetMethod(nameof(IBinanceOptionsRestClientTrading.GetUserTradesAsync))!
            .GetParameters().Single(parameter => parameter.Name == "symbol").HasDefaultValue);
    }

    private static void AssertSignedGet(RecordingHttpMessageHandler handler, RecordingRateLimiter limiter, string path, int weight)
    {
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == path && item.Weight == weight && item.Signed);
    }

    private static void AssertQuery(RecordingHttpMessageHandler handler, params string[] expected)
    {
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        foreach (var value in expected)
            Assert.Contains(value, query);
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
