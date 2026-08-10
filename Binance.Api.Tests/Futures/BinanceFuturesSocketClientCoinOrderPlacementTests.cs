using Binance.Api.Futures;
using Binance.Api.Shared;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesSocketClientCoinOrderPlacementTests
{
    [Fact]
    public void NormalOrder_MetadataAndPublicSurfaceMatchCurrentContract()
    {
        Assert.Equal("ws-dapi/v1", BinanceFuturesSocketClientCoin.PlaceOrderPath);
        Assert.Equal("order.place", BinanceFuturesSocketClientCoin.PlaceOrderMethod);
        Assert.Equal(0, BinanceFuturesSocketClientCoin.PlaceOrderIpWeight);

        var parameters = typeof(IBinanceFuturesSocketClientCoinQueryTrade)
            .GetMethod(nameof(IBinanceFuturesSocketClientCoinQueryTrade.PlaceOrderAsync))!
            .GetParameters();
        var parameterNames = parameters.Select(parameter => parameter.Name).ToList();

        Assert.DoesNotContain("stopPrice", parameterNames);
        Assert.DoesNotContain("activationPrice", parameterNames);
        Assert.DoesNotContain("callbackRate", parameterNames);
        Assert.DoesNotContain("workingType", parameterNames);
        Assert.DoesNotContain("closePosition", parameterNames);
        Assert.DoesNotContain("priceProtect", parameterNames);
        Assert.Equal(typeof(long?), parameters.Single(parameter => parameter.Name == "receiveWindow").ParameterType);
    }

    [Fact]
    public void NormalLimitOrder_BuildsCurrentNumericWebSocketContract()
    {
        var parameters = BinanceFuturesSocketClientCoin.CreatePlaceOrderParameters(
            "BTCUSD_PERP",
            BinanceOrderSide.Buy,
            BinanceFuturesOrderType.Limit,
            2.5m,
            60_000.25m,
            "client-1",
            BinancePositionSide.Both,
            BinanceTimeInForce.GoodTillCrossing,
            BinanceOrderResponseType.Result,
            BinanceSelfTradePreventionMode.ExpireMaker,
            null,
            false,
            5_000L);

        Assert.Equal("BTCUSD_PERP", parameters["symbol"]);
        Assert.Equal("BUY", parameters["side"]);
        Assert.Equal("LIMIT", parameters["type"]);
        Assert.Equal(2.5m, Assert.IsType<decimal>(parameters["quantity"]));
        Assert.Equal(60_000.25m, Assert.IsType<decimal>(parameters["price"]));
        Assert.Equal("client-1", parameters["newClientOrderId"]);
        Assert.Equal("BOTH", parameters["positionSide"]);
        Assert.Equal("GTX", parameters["timeInForce"]);
        Assert.Equal("RESULT", parameters["newOrderRespType"]);
        Assert.Equal("EXPIRE_MAKER", parameters["selfTradePreventionMode"]);
        Assert.Equal("false", parameters["reduceOnly"]);
        Assert.Equal(5_000L, Assert.IsType<long>(parameters["recvWindow"]));
    }

    [Fact]
    public void NormalMarketOrder_BuildsOnlyCurrentMarketFields()
    {
        var parameters = BinanceFuturesSocketClientCoin.CreatePlaceOrderParameters(
            "ETHUSD_PERP",
            BinanceOrderSide.Sell,
            BinanceFuturesOrderType.Market,
            3m,
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
        Assert.Equal(3m, Assert.IsType<decimal>(parameters["quantity"]));
        Assert.False(parameters.ContainsKey("price"));
        Assert.False(parameters.ContainsKey("priceMatch"));
        Assert.False(parameters.ContainsKey("timeInForce"));
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
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(price: 0));
        Assert.Throws<ArgumentException>(() => ValidateNormal(newClientOrderId: "invalid id"));
        Assert.Throws<ArgumentException>(() => ValidateNormal(timeInForce: null));
        Assert.Throws<ArgumentException>(() => ValidateNormal(price: null));
        Assert.Throws<ArgumentException>(() => ValidateNormal(priceMatch: BinanceFuturesPriceMatch.Opponent));
        Assert.Throws<ArgumentException>(() => ValidateNormal(type: BinanceFuturesOrderType.Market));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(timeInForce: BinanceTimeInForce.GoodTillDate));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(timeInForce: BinanceTimeInForce.RetailPriceImprovement));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(orderResponseType: BinanceOrderResponseType.Full));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(price: null, priceMatch: BinanceFuturesPriceMatch.None));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(selfTradePreventionMode: BinanceSelfTradePreventionMode.Decrement));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(positionSide: BinancePositionSide.Hedge));
        Assert.Throws<ArgumentException>(() => ValidateNormal(positionSide: BinancePositionSide.Long, reduceOnly: false));
        Assert.Throws<ArgumentOutOfRangeException>(() => ValidateNormal(receiveWindow: 60_001));

        ValidateNormal(price: null, priceMatch: BinanceFuturesPriceMatch.Queue);
        ValidateNormal(type: BinanceFuturesOrderType.Market, price: null, timeInForce: null);
    }

    [Fact]
    public async Task NormalOrder_RejectsConfiguredReceiveWindowBeforeTransport()
    {
        var root = new BinanceSocketApiClient(new BinanceSocketApiClientOptions
        {
            ReceiveWindow = TimeSpan.FromMilliseconds(60_001)
        });

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            root.CoinFutures.PlaceOrderAsync(
                "BTCUSD_PERP",
                BinanceOrderSide.Buy,
                BinanceFuturesOrderType.Limit,
                1,
                price: 1,
                timeInForce: BinanceTimeInForce.GoodTillCanceled));

        Assert.Equal(60_001L, root.ReceiveWindow((long?)null));
    }

    private static void ValidateNormal(
        string symbol = "BTCUSD_PERP",
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
        long? receiveWindow = null)
        => BinanceFuturesSocketClientCoin.ValidatePlaceOrderParameters(
            symbol, side, type, quantity, price, newClientOrderId, positionSide, timeInForce,
            orderResponseType, selfTradePreventionMode, priceMatch, reduceOnly, receiveWindow);
}
