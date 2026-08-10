using ApiSharp.Models;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesSocketClientCoinAccountQueryTests
{
    [Fact]
    public void MetadataAndPublicSurfaceMatchCurrentContracts()
    {
        Assert.Equal("ws-dapi/v1", BinanceFuturesSocketClientCoin.AccountQueryPath);
        Assert.Equal("account.balance", BinanceFuturesSocketClientCoin.GetBalancesMethod);
        Assert.Equal("account.status", BinanceFuturesSocketClientCoin.GetAccountMethod);
        Assert.Equal(5, BinanceFuturesSocketClientCoin.AccountQueryIpWeight);
        Assert.Equal("ws-dapi/v1", BinanceFuturesSocketClientCoin.CancelOrderPath);
        Assert.Equal("order.cancel", BinanceFuturesSocketClientCoin.CancelOrderMethod);
        Assert.Equal(1, BinanceFuturesSocketClientCoin.CancelOrderIpWeight);
        Assert.Equal("ws-dapi/v1", BinanceFuturesSocketClientCoin.GetOrderPath);
        Assert.Equal("order.status", BinanceFuturesSocketClientCoin.GetOrderMethod);
        Assert.Equal(1, BinanceFuturesSocketClientCoin.GetOrderIpWeight);
        Assert.Equal("ws-dapi/v1", BinanceFuturesSocketClientCoin.PositionQueryPath);
        Assert.Equal("account.position", BinanceFuturesSocketClientCoin.GetPositionsMethod);
        Assert.Equal(5, BinanceFuturesSocketClientCoin.PositionQueryIpWeight);

        var accountMethods = typeof(IBinanceFuturesSocketClientCoinQueryAccount).GetMethods();
        var tradeMethods = typeof(IBinanceFuturesSocketClientCoinQueryTrade).GetMethods();
        Assert.All(
            accountMethods.Concat(tradeMethods).Where(method => method.Name is
                nameof(IBinanceFuturesSocketClientCoinQueryAccount.GetBalancesAsync)
                or nameof(IBinanceFuturesSocketClientCoinQueryAccount.GetAccountInfoAsync)
                or nameof(IBinanceFuturesSocketClientCoinQueryTrade.CancelOrderAsync)
                or nameof(IBinanceFuturesSocketClientCoinQueryTrade.GetOrderAsync)
                or nameof(IBinanceFuturesSocketClientCoinQueryTrade.GetPositionsAsync)),
            method => Assert.Equal(
                typeof(long?),
                method.GetParameters().Single(parameter => parameter.Name == "receiveWindow").ParameterType));

        var positionParameters = tradeMethods
            .Single(method => method.Name == nameof(IBinanceFuturesSocketClientCoinQueryTrade.GetPositionsAsync))
            .GetParameters()
            .Select(parameter => parameter.Name)
            .ToArray();
        Assert.Equal(new[] { "receiveWindow", "marginAsset", "pair", "ct" }, positionParameters);

        var getOrder = tradeMethods.Single(method => method.Name == nameof(IBinanceFuturesSocketClientCoinQueryTrade.GetOrderAsync));
        Assert.Equal(typeof(Task<CallResult<BinanceFuturesCoinSocketOrder>>), getOrder.ReturnType);
    }

    [Fact]
    public void QueryParametersUseCurrentNamesAndInt64Values()
    {
        var account = BinanceFuturesSocketClientCoin.CreateAccountQueryParameters(60_000L);
        Assert.Equal(60_000L, Assert.IsType<long>(account["recvWindow"]));

        var order = BinanceFuturesSocketClientCoin.CreateOrderIdentityParameters(
            "BTCUSD_PERP",
            9_223_372_036_854_775_805L,
            "client-1",
            59_999L);
        Assert.Equal("BTCUSD_PERP", order["symbol"]);
        Assert.Equal(9_223_372_036_854_775_805L, Assert.IsType<long>(order["orderId"]));
        Assert.Equal("client-1", order["origClientOrderId"]);
        Assert.Equal(59_999L, Assert.IsType<long>(order["recvWindow"]));

        var positions = BinanceFuturesSocketClientCoin.CreatePositionQueryParameters(
            "BTC",
            "BTCUSD",
            58_000L);
        Assert.Equal("BTC", positions["marginAsset"]);
        Assert.Equal("BTCUSD", positions["pair"]);
        Assert.Equal(58_000L, Assert.IsType<long>(positions["recvWindow"]));
        Assert.False(positions.ContainsKey("symbol"));

        Assert.Throws<ArgumentException>(() => BinanceFuturesSocketClientCoin.CreateOrderIdentityParameters(" ", 1, null, null));
        Assert.Throws<ArgumentException>(() => BinanceFuturesSocketClientCoin.CreateOrderIdentityParameters("BTCUSD_PERP", null, null, null));
        Assert.Throws<ArgumentException>(() => BinanceFuturesSocketClientCoin.CreateOrderIdentityParameters("BTCUSD_PERP", null, " ", null));
        Assert.Throws<ArgumentException>(() => BinanceFuturesSocketClientCoin.CreatePositionQueryParameters(" ", null, null));
        Assert.Throws<ArgumentException>(() => BinanceFuturesSocketClientCoin.CreatePositionQueryParameters(null, " ", null));
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceFuturesSocketClientCoin.CreateAccountQueryParameters(60_001));
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceFuturesSocketClientCoin.CreateOrderIdentityParameters("BTCUSD_PERP", 1, null, 60_001));
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceFuturesSocketClientCoin.CreatePositionQueryParameters(null, null, 60_001));
    }

    [Fact]
    public async Task ConfiguredReceiveWindowIsRejectedBeforeTransportForAllFiveQueries()
    {
        var root = new BinanceSocketApiClient(new BinanceSocketApiClientOptions
        {
            ReceiveWindow = TimeSpan.FromMilliseconds(60_001)
        });

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => root.CoinFutures.GetBalancesAsync());
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => root.CoinFutures.GetAccountInfoAsync());
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => root.CoinFutures.CancelOrderAsync("BTCUSD_PERP", orderId: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => root.CoinFutures.GetOrderAsync("BTCUSD_PERP", orderId: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => root.CoinFutures.GetPositionsAsync());
    }

    [Fact]
    public void AccountAndBalanceDeserializeCompleteCurrentResponses()
    {
        Assert.Null(typeof(BinanceFuturesCoinAccountAsset).GetProperty(nameof(BinanceFuturesAccountAsset.MarginAvailable)));
        Assert.Equal(
            typeof(List<BinanceFuturesCoinAccountAsset>),
            typeof(BinanceFuturesCoinAccountInfo).GetProperty(nameof(BinanceFuturesCoinAccountInfo.Assets))!.PropertyType);

        var account = Client().Deserializer<BinanceResultWithRateLimits<BinanceFuturesCoinAccountInfo>>(
            JToken.Parse("""
                {
                  "id":"account",
                  "status":200,
                  "result":{
                    "feeTier":4294967296,
                    "canTrade":true,
                    "canDeposit":true,
                    "canWithdraw":false,
                    "updateTime":1726731195000,
                    "assets":[{
                      "asset":"BTC",
                      "walletBalance":"1.1",
                      "unrealizedProfit":"2.2",
                      "marginBalance":"3.3",
                      "maintMargin":"4.4",
                      "initialMargin":"5.5",
                      "positionInitialMargin":"6.6",
                      "openOrderInitialMargin":"7.7",
                      "maxWithdrawAmount":"8.8",
                      "crossWalletBalance":"9.9",
                      "crossUnPnl":"10.1",
                      "availableBalance":"11.2",
                      "updateTime":1726731195001
                    }],
                    "positions":[{
                      "symbol":"BTCUSD_PERP",
                      "initialMargin":"12.3",
                      "maintMargin":"13.4",
                      "unrealizedProfit":"14.5",
                      "positionInitialMargin":"15.6",
                      "openOrderInitialMargin":"16.7",
                      "leverage":"7",
                      "isolated":true,
                      "positionSide":"BOTH",
                      "entryPrice":"60000.5",
                      "maxQty":"100",
                      "notionalValue":"17.8",
                      "isolatedWallet":"18.9",
                      "updateTime":1726731195002,
                      "positionAmt":"2",
                      "breakEvenPrice":"59000.25"
                    }]
                  },
                  "rateLimits":[]
                }
                """));

        Assert.True(account.Success);
        Assert.Equal(4_294_967_296L, account.Data.Result.FeeTier);
        Assert.Equal(10.1m, Assert.Single(account.Data.Result.Assets).CrossUnrealizedPnl);
        var accountPosition = Assert.Single(account.Data.Result.Positions);
        Assert.Equal(18.9m, accountPosition.IsolatedWallet);
        Assert.Equal(59_000.25m, accountPosition.BreakEvenPrice);

        var balance = Client().Deserializer<BinanceResultWithRateLimits<List<BinanceFuturesCoinAccountBalance>>>(
            JToken.Parse("""
                {
                  "id":"balance",
                  "status":200,
                  "result":[{
                    "accountAlias":"alias",
                    "asset":"BTC",
                    "balance":"1.2",
                    "withdrawAvailable":"1.1",
                    "crossWalletBalance":"1.0",
                    "crossUnPnl":"0.2",
                    "availableBalance":"0.9",
                    "updateTime":1726731195003
                  }],
                  "rateLimits":[]
                }
                """));

        Assert.True(balance.Success);
        var item = Assert.Single(balance.Data.Result);
        Assert.Equal("alias", item.AccountAlias);
        Assert.Equal(1.1m, item.WithdrawAvailable);
        Assert.Equal(0.2m, item.CrossUnrealizedPnl);
    }

    [Fact]
    public void PositionQueryDeserializesCompleteCurrentResponse()
    {
        var response = Client().Deserializer<BinanceResultWithRateLimits<List<BinanceFuturesCoinPosition>>>(
            JToken.Parse("""
                {
                  "id":"positions",
                  "status":200,
                  "result":[{
                    "symbol":"BTCUSD_PERP",
                    "positionAmt":"2",
                    "entryPrice":"60000.5",
                    "markPrice":"62000.25",
                    "unRealizedProfit":"0.7",
                    "liquidationPrice":"40000",
                    "leverage":"7",
                    "maxQty":"100",
                    "marginType":"isolated",
                    "isolatedMargin":"0.8",
                    "isAutoAddMargin":"false",
                    "positionSide":"BOTH",
                    "notionalValue":"0.9",
                    "isolatedWallet":"1.1",
                    "updateTime":1726731195634,
                    "breakEvenPrice":"59000.25"
                  }],
                  "rateLimits":[]
                }
                """));

        Assert.True(response.Success);
        var position = Assert.Single(response.Data.Result);
        Assert.Equal(0.7m, position.UnrealizedProfit);
        Assert.Equal(1.1m, position.IsolatedWallet);
        Assert.Equal(0.9m, position.NotionalValue);
        Assert.Equal(59_000.25m, position.BreakEvenPrice);
        Assert.False(position.IsAutoAddMargin);
    }

    [Fact]
    public void QueryOrderUsesItsExactCurrentResponseModel()
    {
        var properties = typeof(BinanceFuturesCoinSocketOrder).GetProperties().Select(property => property.Name).ToHashSet();
        Assert.DoesNotContain(nameof(BinanceFuturesOrder.ModifyId), properties);
        Assert.DoesNotContain(nameof(BinanceFuturesOrder.QuoteQuantityFilled), properties);
        Assert.DoesNotContain(nameof(BinanceFuturesOrder.ActivatePrice), properties);
        Assert.DoesNotContain(nameof(BinanceFuturesOrder.CallbackRate), properties);
        Assert.DoesNotContain(nameof(BinanceFuturesOrder.GoodTillDate), properties);

        var response = Client().Deserializer<BinanceResultWithRateLimits<BinanceFuturesCoinSocketOrder>>(
            JToken.Parse("""
                {
                  "id":"order",
                  "status":200,
                  "result":{
                    "orderId":9223372036854775805,
                    "symbol":"BTCUSD_PERP",
                    "pair":"BTCUSD",
                    "status":"NEW",
                    "clientOrderId":"client-1",
                    "price":"58000",
                    "avgPrice":"0",
                    "origQty":"2",
                    "executedQty":"0.5",
                    "cumQty":"0.5",
                    "cumBase":"0.00862069",
                    "timeInForce":"GTC",
                    "type":"LIMIT",
                    "reduceOnly":false,
                    "closePosition":false,
                    "side":"BUY",
                    "positionSide":"LONG",
                    "stopPrice":"0",
                    "workingType":"CONTRACT_PRICE",
                    "priceProtect":false,
                    "origType":"LIMIT",
                    "selfTradePreventionMode":"EXPIRE_TAKER",
                    "time":1733740063619,
                    "updateTime":1733740063620,
                    "priceMatch":"NONE"
                  },
                  "rateLimits":[]
                }
                """));

        Assert.True(response.Success);
        Assert.Equal(9_223_372_036_854_775_805L, response.Data.Result.Id);
        Assert.Equal("BTCUSD", response.Data.Result.Pair);
        Assert.Equal(0.5m, response.Data.Result.CumulativeQuantity);
        Assert.Equal(0.00862069m, response.Data.Result.BaseQuantityFilled);
        Assert.Equal(BinanceSelfTradePreventionMode.ExpireTaker, response.Data.Result.SelfTradePreventionMode);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_733_740_063_619).UtcDateTime, response.Data.Result.CreateTime);
    }

    private static BinanceFuturesSocketClientCoin Client()
        => Assert.IsType<BinanceFuturesSocketClientCoin>(new BinanceSocketApiClient().CoinFutures);
}
