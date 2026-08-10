using ApiSharp.Models;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesSocketClientModifyOrderTests
{
    [Fact]
    public void UsdModifyOrder_BuildsCurrentWebSocketContract()
    {
        var parameters = BinanceFuturesSocketClientUsd.CreateModifyOrderParameters(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            1.5m,
            50_000.25m,
            9_223_372_036_854_775_805L,
            "usd-client",
            null,
            9_007_199_254_740_993L,
            90_000L);

        Assert.Equal("ws-fapi/v1", BinanceFuturesSocketClientUsd.ModifyOrderPath);
        Assert.Equal("order.modify", BinanceFuturesSocketClientUsd.ModifyOrderMethod);
        Assert.Equal(0, BinanceFuturesSocketClientUsd.ModifyOrderIpWeight);
        Assert.Equal("BTCUSDT", parameters["symbol"]);
        Assert.Equal("BUY", parameters["side"]);
        Assert.Equal(1.5m, Assert.IsType<decimal>(parameters["quantity"]));
        Assert.Equal(50_000.25m, Assert.IsType<decimal>(parameters["price"]));
        Assert.Equal(9_223_372_036_854_775_805L, Assert.IsType<long>(parameters["orderId"]));
        Assert.Equal("usd-client", parameters["origClientOrderId"]);
        Assert.Equal(9_007_199_254_740_993L, Assert.IsType<long>(parameters["modifyId"]));
        Assert.Equal(90_000L, Assert.IsType<long>(parameters["recvWindow"]));
        Assert.False(parameters.ContainsKey("priceMatch"));
    }

    [Fact]
    public void CoinModifyOrder_BuildsCurrentWebSocketContract()
    {
        var parameters = BinanceFuturesSocketClientCoin.CreateModifyOrderParameters(
            "BTCUSD_PERP",
            BinanceOrderSide.Sell,
            3m,
            60_000.5m,
            null,
            "coin-client",
            null,
            9_007_199_254_740_995L,
            60_000L);

        Assert.Equal("ws-dapi/v1", BinanceFuturesSocketClientCoin.ModifyOrderPath);
        Assert.Equal("order.modify", BinanceFuturesSocketClientCoin.ModifyOrderMethod);
        Assert.Equal(1, BinanceFuturesSocketClientCoin.ModifyOrderIpWeight);
        Assert.Equal("BTCUSD_PERP", parameters["symbol"]);
        Assert.Equal("SELL", parameters["side"]);
        Assert.Equal(3m, Assert.IsType<decimal>(parameters["quantity"]));
        Assert.Equal(60_000.5m, Assert.IsType<decimal>(parameters["price"]));
        Assert.Equal("coin-client", parameters["origClientOrderId"]);
        Assert.Equal(9_007_199_254_740_995L, Assert.IsType<long>(parameters["modifyId"]));
        Assert.Equal(60_000L, Assert.IsType<long>(parameters["recvWindow"]));
        Assert.False(parameters.ContainsKey("orderId"));
        Assert.False(parameters.ContainsKey("priceMatch"));
    }

    [Fact]
    public async Task ModifyOrder_RejectsUnsafeOrUndocumentedRequestsBeforeTransport()
    {
        Assert.Throws<ArgumentException>(() => Usd(symbol: " "));
        Assert.Throws<ArgumentOutOfRangeException>(() => Usd(side: (BinanceOrderSide)0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Usd(quantity: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Usd(price: 0));
        Assert.Throws<ArgumentException>(() => Usd(orderId: null));
        Assert.Throws<ArgumentException>(() => Usd(orderId: null, origClientOrderId: " "));
        Assert.Throws<ArgumentException>(() => Usd(priceMatch: BinanceFuturesPriceMatch.Opponent));

        Assert.Throws<ArgumentException>(() => Coin(symbol: " "));
        Assert.Throws<ArgumentOutOfRangeException>(() => Coin(side: (BinanceOrderSide)0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Coin(quantity: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Coin(price: 0));
        Assert.Throws<ArgumentException>(() => Coin(orderId: null));
        Assert.Throws<ArgumentException>(() => Coin(orderId: null, origClientOrderId: " "));
        Assert.Throws<ArgumentException>(() => Coin(priceMatch: BinanceFuturesPriceMatch.Queue));
        Assert.Throws<ArgumentOutOfRangeException>(() => Coin(receiveWindow: 60_001));

        var options = new BinanceSocketApiClientOptions
        {
            ReceiveWindow = TimeSpan.FromMilliseconds(60_001)
        };
        var configuredClient = new BinanceSocketApiClient(options);
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.CoinFutures.ModifyOrderAsync(
                "BTCUSD_PERP",
                BinanceOrderSide.Sell,
                1,
                1,
                orderId: 1));

        Assert.Equal(60_001L, configuredClient.ReceiveWindow((long?)null));
    }

    [Fact]
    public void ModifyOrder_DeserializesCurrentWebSocketResponseEnvelopes()
    {
        var root = new BinanceSocketApiClient();
        var usdClient = Assert.IsType<BinanceFuturesSocketClientUsd>(root.UsdFutures);
        var usdResponse = usdClient.Deserializer<BinanceResultWithRateLimits<BinanceFuturesOrder>>(
            JToken.Parse("""
                {
                  "id":"usd-request",
                  "status":200,
                  "result":{
                    "orderId":9223372036854775805,
                    "symbol":"BTCUSDT",
                    "status":"NEW",
                    "clientOrderId":"usd-client",
                    "modifyId":9007199254740993,
                    "price":"50000.25",
                    "origQty":"1.5",
                    "executedQty":"0.25",
                    "cumQty":"0.25",
                    "timeInForce":"GTC",
                    "type":"LIMIT",
                    "reduceOnly":false,
                    "closePosition":false,
                    "side":"BUY",
                    "positionSide":"BOTH",
                    "stopPrice":"0",
                    "workingType":"CONTRACT_PRICE",
                    "priceProtect":false,
                    "origType":"LIMIT",
                    "priceMatch":"NONE",
                    "selfTradePreventionMode":"NONE",
                    "goodTillDate":1750492800000,
                    "updateTime":1750489200123
                  },
                  "rateLimits":[
                    {"rateLimitType":"REQUEST_WEIGHT","interval":"MINUTE","intervalNum":1,"limit":2400,"count":0},
                    {"rateLimitType":"ORDERS","interval":"SECOND","intervalNum":10,"limit":300,"count":1}
                  ]
                }
                """));

        Assert.True(usdResponse.Success);
        Assert.Equal("usd-request", usdResponse.Data.Id);
        Assert.Equal(200, usdResponse.Data.Status);
        Assert.Equal(2, usdResponse.Data.Ratelimits.Count);
        Assert.Equal(BinanceRateLimitType.RequestWeight, usdResponse.Data.Ratelimits[0].Type);
        Assert.Equal(0, usdResponse.Data.Ratelimits[0].Count);
        Assert.Equal(BinanceRateLimitType.Orders, usdResponse.Data.Ratelimits[1].Type);
        Assert.Equal(1, usdResponse.Data.Ratelimits[1].Count);
        Assert.Equal(9_223_372_036_854_775_805L, usdResponse.Data.Result.Id);
        Assert.Equal(9_007_199_254_740_993L, usdResponse.Data.Result.ModifyId);
        Assert.Equal(50_000.25m, usdResponse.Data.Result.Price);
        Assert.Equal(0.25m, usdResponse.Data.Result.CumulativeQuantity);
        Assert.Equal(BinanceOrderSide.Buy, usdResponse.Data.Result.Side);
        Assert.Equal(BinanceFuturesWorkingType.Contract, usdResponse.Data.Result.WorkingType);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_750_492_800_000).UtcDateTime, usdResponse.Data.Result.GoodTillDate);
        Assert.Equal(0m, usdResponse.Data.Result.AveragePrice);
        Assert.Null(usdResponse.Data.Result.QuoteQuantityFilled);
        Assert.Null(usdResponse.Data.Result.BaseQuantityFilled);

        var coinClient = Assert.IsType<BinanceFuturesSocketClientCoin>(root.CoinFutures);
        var coinResponse = coinClient.Deserializer<BinanceResultWithRateLimits<BinanceFuturesCoinSocketOrderAcknowledgement>>(
            JToken.Parse("""
                {
                  "id":"coin-request",
                  "status":200,
                  "result":{
                    "orderId":9223372036854775804,
                    "symbol":"BTCUSD_PERP",
                    "pair":"BTCUSD",
                    "status":"NEW",
                    "clientOrderId":"coin-client",
                    "price":"60000.5",
                    "origQty":"3",
                    "executedQty":"0",
                    "cumQty":"0",
                    "timeInForce":"GTC",
                    "type":"LIMIT",
                    "reduceOnly":true,
                    "closePosition":false,
                    "side":"SELL",
                    "positionSide":"SHORT",
                    "stopPrice":"0",
                    "workingType":"MARK_PRICE",
                    "priceProtect":true,
                    "origType":"LIMIT",
                    "updateTime":1750489200456
                  },
                  "rateLimits":[]
                }
                """));

        Assert.True(coinResponse.Success);
        Assert.Equal("coin-request", coinResponse.Data.Id);
        Assert.Equal(9_223_372_036_854_775_804L, coinResponse.Data.Result.Id);
        Assert.Equal("BTCUSD", coinResponse.Data.Result.Pair);
        Assert.Null(coinResponse.Data.Result.ModifyId);
        Assert.Equal(BinanceOrderSide.Sell, coinResponse.Data.Result.Side);
        Assert.Equal(BinancePositionSide.Short, coinResponse.Data.Result.PositionSide);
        Assert.Equal(BinanceFuturesWorkingType.Mark, coinResponse.Data.Result.WorkingType);
        Assert.True(coinResponse.Data.Result.ReduceOnly);
        Assert.True(coinResponse.Data.Result.PriceProtect);
        Assert.IsType<BinanceFuturesCoinSocketOrderAcknowledgement>(coinResponse.Data.Result);
    }

    private static ParameterCollection Usd(
        string symbol = "BTCUSDT",
        BinanceOrderSide side = BinanceOrderSide.Buy,
        decimal quantity = 1,
        decimal price = 1,
        long? orderId = 1,
        string? origClientOrderId = null,
        BinanceFuturesPriceMatch? priceMatch = null,
        long? receiveWindow = null)
        => BinanceFuturesSocketClientUsd.CreateModifyOrderParameters(
            symbol, side, quantity, price, orderId, origClientOrderId, priceMatch, null, receiveWindow);

    private static ParameterCollection Coin(
        string symbol = "BTCUSD_PERP",
        BinanceOrderSide side = BinanceOrderSide.Sell,
        decimal quantity = 1,
        decimal price = 1,
        long? orderId = 1,
        string? origClientOrderId = null,
        BinanceFuturesPriceMatch? priceMatch = null,
        long? receiveWindow = null)
        => BinanceFuturesSocketClientCoin.CreateModifyOrderParameters(
            symbol, side, quantity, price, orderId, origClientOrderId, priceMatch, null, receiveWindow);
}
