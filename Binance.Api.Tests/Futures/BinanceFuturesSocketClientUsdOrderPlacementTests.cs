using ApiSharp.Models;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesSocketClientUsdOrderPlacementTests
{
    [Fact]
    public void NormalOrder_MetadataAndPublicSurfaceMatchCurrentContract()
    {
        Assert.Equal("ws-fapi/v1", BinanceFuturesSocketClientUsd.PlaceOrderPath);
        Assert.Equal("order.place", BinanceFuturesSocketClientUsd.PlaceOrderMethod);
        Assert.Equal(0, BinanceFuturesSocketClientUsd.PlaceOrderIpWeight);

        var parameterNames = typeof(IBinanceFuturesSocketClientUsdQueryTrade)
            .GetMethod(nameof(IBinanceFuturesSocketClientUsdQueryTrade.PlaceOrderAsync))!
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
    public void NormalLimitOrder_BuildsCurrentNumericWebSocketContract()
    {
        var goodTillDate = DateTimeOffset.FromUnixTimeMilliseconds(
            DateTimeOffset.UtcNow.AddMinutes(20).ToUnixTimeMilliseconds()).UtcDateTime;
        var parameters = BinanceFuturesSocketClientUsd.CreatePlaceOrderParameters(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            BinanceFuturesOrderType.Limit,
            1.25m,
            50_000.5m,
            "client-1",
            BinancePositionSide.Both,
            BinanceTimeInForce.GoodTillDate,
            BinanceOrderResponseType.Result,
            BinanceSelfTradePreventionMode.ExpireMaker,
            null,
            false,
            goodTillDate,
            5_000);

        Assert.Equal("BTCUSDT", parameters["symbol"]);
        Assert.Equal("BUY", parameters["side"]);
        Assert.Equal("LIMIT", parameters["type"]);
        Assert.Equal(1.25m, Assert.IsType<decimal>(parameters["quantity"]));
        Assert.Equal(50_000.5m, Assert.IsType<decimal>(parameters["price"]));
        Assert.Equal("client-1", parameters["newClientOrderId"]);
        Assert.Equal("BOTH", parameters["positionSide"]);
        Assert.Equal("GTD", parameters["timeInForce"]);
        Assert.Equal("RESULT", parameters["newOrderRespType"]);
        Assert.Equal("EXPIRE_MAKER", parameters["selfTradePreventionMode"]);
        Assert.Equal("false", parameters["reduceOnly"]);
        Assert.Equal(new DateTimeOffset(goodTillDate).ToUnixTimeMilliseconds(), Assert.IsType<long>(parameters["goodTillDate"]));
        Assert.Equal(5_000L, Assert.IsType<long>(parameters["recvWindow"]));
    }

    [Fact]
    public void NormalMarketOrder_BuildsOnlyCurrentMarketFields()
    {
        var parameters = BinanceFuturesSocketClientUsd.CreatePlaceOrderParameters(
            "ETHUSDT",
            BinanceOrderSide.Sell,
            BinanceFuturesOrderType.Market,
            2.5m,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        Assert.Equal("MARKET", parameters["type"]);
        Assert.Equal(2.5m, Assert.IsType<decimal>(parameters["quantity"]));
        Assert.False(parameters.ContainsKey("price"));
        Assert.False(parameters.ContainsKey("priceMatch"));
        Assert.False(parameters.ContainsKey("timeInForce"));
        Assert.False(parameters.ContainsKey("goodTillDate"));
    }

    [Theory]
    [InlineData(BinanceFuturesOrderType.Stop)]
    [InlineData(BinanceFuturesOrderType.StopMarket)]
    [InlineData(BinanceFuturesOrderType.TakeProfit)]
    [InlineData(BinanceFuturesOrderType.TakeProfitMarket)]
    [InlineData(BinanceFuturesOrderType.TrailingStopMarket)]
    [InlineData(BinanceFuturesOrderType.Liquidation)]
    [InlineData((BinanceFuturesOrderType)byte.MaxValue)]
    public void NormalOrder_RejectsNonNormalTypes(BinanceFuturesOrderType type)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(type: type));
    }

    [Fact]
    public void NormalOrder_RejectsInvalidCurrentCombinations()
    {
        Assert.Throws<ArgumentException>(() => ValidateNormal(symbol: " "));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(side: (BinanceOrderSide)0));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(quantity: null));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(quantity: 0));
        Assert.Throws<ArgumentException>(() => ValidateNormal(timeInForce: null));
        Assert.Throws<ArgumentException>(() => ValidateNormal(price: null));
        Assert.Throws<ArgumentException>(() => ValidateNormal(priceMatch: BinanceFuturesPriceMatch.Opponent));
        Assert.Throws<ArgumentException>(() => ValidateNormal(type: BinanceFuturesOrderType.Market));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(timeInForce: BinanceTimeInForce.GoodTillExpiredOrCanceled));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(orderResponseType: BinanceOrderResponseType.Full));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(price: null, priceMatch: BinanceFuturesPriceMatch.None));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(selfTradePreventionMode: BinanceSelfTradePreventionMode.Decrement));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(positionSide: BinancePositionSide.Hedge));
        Assert.Throws<ArgumentException>(() => ValidateNormal(positionSide: BinancePositionSide.Long, reduceOnly: false));
        Assert.Throws<ArgumentException>(() => ValidateNormal(timeInForce: BinanceTimeInForce.GoodTillDate));
        Assert.Throws<ArgumentException>(() => ValidateNormal(goodTillDate: DateTime.UtcNow.AddMinutes(20)));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(
            timeInForce: BinanceTimeInForce.GoodTillDate,
            goodTillDate: DateTime.UtcNow.AddMinutes(5)));

        ValidateNormal(price: null, priceMatch: BinanceFuturesPriceMatch.Queue);
        ValidateNormal(type: BinanceFuturesOrderType.Market, price: null, timeInForce: null);
    }

    [Fact]
    public void AlgoOrder_MetadataAndPublicSurfaceMatchCurrentContract()
    {
        Assert.Equal("ws-fapi/v1", BinanceFuturesSocketClientUsd.PlaceAlgoOrderPath);
        Assert.Equal("algoOrder.place", BinanceFuturesSocketClientUsd.PlaceAlgoOrderMethod);
        Assert.Equal(0, BinanceFuturesSocketClientUsd.PlaceAlgoOrderIpWeight);
        Assert.Equal("ws-fapi/v1", BinanceFuturesSocketClientUsd.CancelAlgoOrderPath);
        Assert.Equal("algoOrder.cancel", BinanceFuturesSocketClientUsd.CancelAlgoOrderMethod);
        Assert.Equal(1, BinanceFuturesSocketClientUsd.CancelAlgoOrderIpWeight);

        var methods = typeof(IBinanceFuturesSocketClientUsdQueryTrade).GetMethods();
        Assert.Contains(methods, method => method.Name == nameof(IBinanceFuturesSocketClientUsdQueryTrade.PlaceAlgoOrderAsync));
        Assert.Contains(methods, method => method.Name == nameof(IBinanceFuturesSocketClientUsdQueryTrade.CancelAlgoOrderAsync));
    }

    [Fact]
    public void PlaceAlgoOrder_BuildsCurrentNumericWebSocketContract()
    {
        var parameters = BinanceFuturesSocketClientUsd.CreatePlaceAlgoOrderParameters(
            "BTCUSDT",
            BinanceOrderSide.Sell,
            BinanceFuturesAlgoOrderType.Stop,
            BinancePositionSide.Both,
            BinanceTimeInForce.GoodTillCanceled,
            1.5m,
            160_000.25m,
            120_000.5m,
            BinanceFuturesWorkingType.Contract,
            null,
            null,
            null,
            false,
            null,
            null,
            "algo-client",
            BinanceOrderResponseType.Result,
            BinanceSelfTradePreventionMode.ExpireMaker,
            null,
            5_000);

        Assert.Equal("CONDITIONAL", parameters["algoType"]);
        Assert.Equal("STOP", parameters["type"]);
        Assert.Equal(1.5m, Assert.IsType<decimal>(parameters["quantity"]));
        Assert.Equal(160_000.25m, Assert.IsType<decimal>(parameters["price"]));
        Assert.Equal(120_000.5m, Assert.IsType<decimal>(parameters["triggerPrice"]));
        Assert.Equal("CONTRACT_PRICE", parameters["workingType"]);
        Assert.Equal("false", parameters["reduceOnly"]);
        Assert.Equal("RESULT", parameters["newOrderRespType"]);
        Assert.Equal("EXPIRE_MAKER", parameters["selfTradePreventionMode"]);
        Assert.Equal(5_000L, Assert.IsType<long>(parameters["recvWindow"]));
    }

    [Fact]
    public void PlaceAlgoOrder_BuildsCurrentCloseAllAndTrailingContracts()
    {
        var closeAll = BinanceFuturesSocketClientUsd.CreatePlaceAlgoOrderParameters(
            "BTCUSDT", BinanceOrderSide.Buy, BinanceFuturesAlgoOrderType.StopMarket,
            BinancePositionSide.Short, null, null, null, 90_000m, BinanceFuturesWorkingType.Mark,
            null, true, true, null, null, null, null, null, null, null, null);
        Assert.Equal("true", closeAll["closePosition"]);
        Assert.Equal("true", closeAll["priceProtect"]);
        Assert.Equal(90_000m, Assert.IsType<decimal>(closeAll["triggerPrice"]));
        Assert.False(closeAll.ContainsKey("quantity"));

        var trailing = BinanceFuturesSocketClientUsd.CreatePlaceAlgoOrderParameters(
            "ETHUSDT", BinanceOrderSide.Sell, BinanceFuturesAlgoOrderType.TrailingStopMarket,
            null, null, 2m, null, null, null, null, null, null, null, 4_000m, 0.5m,
            null, null, null, null, null);
        Assert.Equal(4_000m, Assert.IsType<decimal>(trailing["activatePrice"]));
        Assert.Equal(0.5m, Assert.IsType<decimal>(trailing["callbackRate"]));
    }

    [Fact]
    public void PlaceAlgoOrder_UsesSharedValidatedContract()
    {
        Assert.Throws<ArgumentException>(() => CreateAlgo(symbol: " "));
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateAlgo(type: (BinanceFuturesAlgoOrderType)byte.MaxValue));
        Assert.Throws<ArgumentException>(() => CreateAlgo(type: BinanceFuturesAlgoOrderType.Stop, callbackRate: 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateAlgo(type: BinanceFuturesAlgoOrderType.TrailingStopMarket, callbackRate: 10.1m));
        Assert.Throws<ArgumentException>(() => CreateAlgo(clientAlgoId: "invalid id"));
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateAlgo(timeInForce: BinanceTimeInForce.GoodTillCrossing));
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateAlgo(timeInForce: BinanceTimeInForce.RetailPriceImprovement));

        CreateAlgo(
            timeInForce: BinanceTimeInForce.GoodTillDate,
            goodTillDate: DateTime.UtcNow.AddMinutes(20));
    }

    [Fact]
    public void CancelAlgoOrder_BuildsCurrentInt64ContractAndValidatesIdentity()
    {
        var parameters = BinanceFuturesSocketClientUsd.CreateCancelAlgoOrderParameters(
            9_223_372_036_854_775_805L,
            "algo-client",
            5_000);

        Assert.Equal(9_223_372_036_854_775_805L, Assert.IsType<long>(parameters["algoId"]));
        Assert.Equal("algo-client", parameters["clientAlgoId"]);
        Assert.Equal(5_000L, Assert.IsType<long>(parameters["recvWindow"]));
        Assert.Throws<ArgumentException>(() => BinanceFuturesSocketClientUsd.CreateCancelAlgoOrderParameters(null, null, null));
        Assert.Throws<ArgumentException>(() => BinanceFuturesSocketClientUsd.CreateCancelAlgoOrderParameters(null, " ", null));
    }

    [Fact]
    public void AlgoOrders_DeserializeCurrentWebSocketResponseEnvelopes()
    {
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceFuturesSocketClientUsd>(root.UsdFutures);
        var placement = client.Deserializer<BinanceResultWithRateLimits<BinanceFuturesAlgoOrderPlacementResult>>(
            JToken.Parse("""
                {
                  "id":"place-request",
                  "status":200,
                  "result":{
                    "algoId":9223372036854775805,
                    "clientAlgoId":"algo-client",
                    "algoType":"CONDITIONAL",
                    "orderType":"TAKE_PROFIT",
                    "symbol":"BTCUSDT",
                    "side":"SELL",
                    "positionSide":"SHORT",
                    "timeInForce":"GTC",
                    "quantity":"1.000",
                    "algoStatus":"NEW",
                    "triggerPrice":"120000.00",
                    "price":"160000.00",
                    "icebergQuantity":"null",
                    "selfTradePreventionMode":"EXPIRE_MAKER",
                    "workingType":"CONTRACT_PRICE",
                    "priceMatch":"NONE",
                    "closePosition":false,
                    "priceProtect":false,
                    "reduceOnly":false,
                    "createTime":1762507264142,
                    "updateTime":1762507264143,
                    "triggerTime":0,
                    "goodTillDate":0
                  },
                  "rateLimits":[{"rateLimitType":"REQUEST_WEIGHT","interval":"MINUTE","intervalNum":1,"limit":2400,"count":1}]
                }
                """));

        Assert.True(placement.Success);
        Assert.Equal(9_223_372_036_854_775_805L, placement.Data.Result.AlgoId);
        Assert.Equal(120_000m, placement.Data.Result.TriggerPrice);
        Assert.Equal(160_000m, placement.Data.Result.Price);
        Assert.Equal("null", placement.Data.Result.IcebergQuantity);
        Assert.Equal(1, placement.Data.Ratelimits.Single().Count);

        var cancellation = client.Deserializer<BinanceResultWithRateLimits<BinanceFuturesAlgoOrderCancellationResult>>(
            JToken.Parse("""
                {
                  "id":"cancel-request",
                  "status":200,
                  "result":{"algoId":9223372036854775805,"clientAlgoId":"algo-client","code":"200","msg":"success"},
                  "rateLimits":[]
                }
                """));

        Assert.True(cancellation.Success);
        Assert.Equal(9_223_372_036_854_775_805L, cancellation.Data.Result.AlgoId);
        Assert.Equal("200", cancellation.Data.Result.Code);
        Assert.Equal("success", cancellation.Data.Result.Message);
    }

    private static void ValidateNormal(
        string symbol = "BTCUSDT",
        BinanceOrderSide side = BinanceOrderSide.Buy,
        BinanceFuturesOrderType type = BinanceFuturesOrderType.Limit,
        decimal? quantity = 1,
        decimal? price = 1,
        string? newClientOrderId = null,
        BinancePositionSide? positionSide = null,
        BinanceTimeInForce? timeInForce = BinanceTimeInForce.GoodTillCanceled,
        BinanceOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        BinanceFuturesPriceMatch? priceMatch = null,
        bool? reduceOnly = null,
        DateTime? goodTillDate = null)
        => BinanceFuturesSocketClientUsd.ValidatePlaceOrderParameters(
            symbol, side, type, quantity, price, newClientOrderId, positionSide, timeInForce,
            orderResponseType, selfTradePreventionMode, priceMatch, reduceOnly, goodTillDate);

    private static ParameterCollection CreateAlgo(
        string symbol = "BTCUSDT",
        BinanceFuturesAlgoOrderType type = BinanceFuturesAlgoOrderType.Stop,
        decimal? callbackRate = null,
        string? clientAlgoId = null,
        BinanceTimeInForce? timeInForce = null,
        DateTime? goodTillDate = null)
        => BinanceFuturesSocketClientUsd.CreatePlaceAlgoOrderParameters(
            symbol, BinanceOrderSide.Buy, type, null, timeInForce, 1, null, 1, null, null,
            null, null, null, null, callbackRate, clientAlgoId, null, null, goodTillDate, null);
}
