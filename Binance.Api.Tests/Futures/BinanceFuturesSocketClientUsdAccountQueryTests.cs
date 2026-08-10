using ApiSharp.Models;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesSocketClientUsdAccountQueryTests
{
    [Fact]
    public void MetadataPublicSurfaceAndParametersMatchCurrentCoexistingContracts()
    {
        Assert.Equal("ws-fapi/v1", BinanceFuturesSocketClientUsd.AccountQueryPath);
        Assert.Equal("account.balance", BinanceFuturesSocketClientUsd.GetBalancesV1Method);
        Assert.Equal("v2/account.balance", BinanceFuturesSocketClientUsd.GetBalancesV2Method);
        Assert.Equal("account.status", BinanceFuturesSocketClientUsd.GetAccountV1Method);
        Assert.Equal("v2/account.status", BinanceFuturesSocketClientUsd.GetAccountV2Method);
        Assert.Equal(5, BinanceFuturesSocketClientUsd.AccountQueryIpWeight);
        Assert.Equal("ws-fapi/v1", BinanceFuturesSocketClientUsd.PositionQueryPath);
        Assert.Equal("account.position", BinanceFuturesSocketClientUsd.GetPositionsV1Method);
        Assert.Equal("v2/account.position", BinanceFuturesSocketClientUsd.GetPositionsV2Method);
        Assert.Equal(5, BinanceFuturesSocketClientUsd.PositionQueryIpWeight);

        var methods = typeof(IBinanceFuturesSocketClientUsdQueryAccount).GetMethods()
            .Concat(typeof(IBinanceFuturesSocketClientUsdQueryTrade).GetMethods())
            .ToArray();
        Assert.Contains(methods, method => method.Name == nameof(IBinanceFuturesSocketClientUsdQueryAccount.GetBalancesV1Async));
        Assert.Contains(methods, method => method.Name == nameof(IBinanceFuturesSocketClientUsdQueryAccount.GetAccountV1Async));
        Assert.Contains(methods, method => method.Name == nameof(IBinanceFuturesSocketClientUsdQueryTrade.GetPositionsV1Async));
        Assert.All(
            methods.Where(method => method.Name is nameof(IBinanceFuturesSocketClientUsdQueryAccount.GetBalancesV1Async)
                or nameof(IBinanceFuturesSocketClientUsdQueryAccount.GetBalancesAsync)
                or nameof(IBinanceFuturesSocketClientUsdQueryAccount.GetAccountV1Async)
                or nameof(IBinanceFuturesSocketClientUsdQueryAccount.GetAccountAsync)
                or nameof(IBinanceFuturesSocketClientUsdQueryTrade.GetPositionsV1Async)
                or nameof(IBinanceFuturesSocketClientUsdQueryTrade.GetPositionsAsync)),
            method => Assert.Equal(typeof(long?), method.GetParameters().Single(parameter => parameter.Name == "receiveWindow").ParameterType));

        var accountParameters = BinanceFuturesSocketClientUsd.CreateAccountQueryParameters(3_000_000_000L);
        Assert.Equal(3_000_000_000L, Assert.IsType<long>(accountParameters["recvWindow"]));

        var positionParameters = BinanceFuturesSocketClientUsd.CreatePositionQueryParameters("BTCUSDT", 3_000_000_001L);
        Assert.Equal("BTCUSDT", positionParameters["symbol"]);
        Assert.Equal(3_000_000_001L, Assert.IsType<long>(positionParameters["recvWindow"]));
    }

    [Fact]
    public void AccountV1DeserializesCompleteCurrentResponse()
    {
        var response = Client().Deserializer<BinanceResultWithRateLimits<BinanceFuturesAccountInfoV2>>(
            JToken.Parse("""
                {
                  "id":"account-v1",
                  "status":200,
                  "result":{
                    "feeTier":4294967296,
                    "canTrade":true,
                    "canDeposit":true,
                    "canWithdraw":false,
                    "updateTime":0,
                    "multiAssetsMargin":true,
                    "tradeGroupId":9007199254740993,
                    "totalInitialMargin":"1.1",
                    "totalMaintMargin":"2.2",
                    "totalWalletBalance":"3.3",
                    "totalUnrealizedProfit":"4.4",
                    "totalMarginBalance":"5.5",
                    "totalPositionInitialMargin":"6.6",
                    "totalOpenOrderInitialMargin":"7.7",
                    "totalCrossWalletBalance":"8.8",
                    "totalCrossUnPnl":"9.9",
                    "availableBalance":"10.1",
                    "maxWithdrawAmount":"11.2",
                    "assets":[{
                      "asset":"USDT",
                      "walletBalance":"12.3",
                      "unrealizedProfit":"13.4",
                      "marginBalance":"14.5",
                      "maintMargin":"15.6",
                      "initialMargin":"16.7",
                      "positionInitialMargin":"17.8",
                      "openOrderInitialMargin":"18.9",
                      "crossWalletBalance":"19.1",
                      "crossUnPnl":"20.2",
                      "availableBalance":"21.3",
                      "maxWithdrawAmount":"22.4",
                      "marginAvailable":true,
                      "updateTime":1625474304765
                    }],
                    "positions":[{
                      "symbol":"BTCUSDT",
                      "initialMargin":"23.5",
                      "maintMargin":"24.6",
                      "unrealizedProfit":"25.7",
                      "positionInitialMargin":"26.8",
                      "openOrderInitialMargin":"27.9",
                      "leverage":"100",
                      "isolated":true,
                      "entryPrice":"28.1",
                      "maxNotional":"29.2",
                      "bidNotional":"30.3",
                      "askNotional":"31.4",
                      "positionSide":"BOTH",
                      "positionAmt":"32.5",
                      "updateTime":1625474304766,
                      "breakEvenPrice":"33.6"
                    }]
                  },
                  "rateLimits":[{"rateLimitType":"REQUEST_WEIGHT","interval":"MINUTE","intervalNum":1,"limit":2400,"count":20}]
                }
                """));

        Assert.True(response.Success);
        Assert.Equal(4_294_967_296L, response.Data.Result.FeeTier);
        Assert.Equal(9_007_199_254_740_993L, response.Data.Result.TradeGroupId);
        Assert.Equal(11.2m, response.Data.Result.MaxWithdrawQuantity);
        var asset = Assert.Single(response.Data.Result.Assets);
        Assert.True(asset.MarginAvailable);
        Assert.Equal(20.2m, asset.CrossUnrealizedPnl);
        var position = Assert.Single(response.Data.Result.Positions);
        Assert.Equal(25.7m, position.UnrealizedProfit);
        Assert.Equal(30.3m, position.IgnoredBidNotional);
        Assert.Equal(31.4m, position.IgnoredAskNotional);
        Assert.Equal(33.6m, position.BreakEvenPrice);
        Assert.Equal(20, Assert.Single(response.Data.Ratelimits).Count);
    }

    [Fact]
    public void AccountV2DeserializesItsDistinctCurrentResponse()
    {
        var response = Client().Deserializer<BinanceResultWithRateLimits<BinanceFuturesAccountInfo>>(
            JToken.Parse("""
                {
                  "id":"account-v2",
                  "status":200,
                  "result":{
                    "totalInitialMargin":"1",
                    "totalMaintMargin":"2",
                    "totalWalletBalance":"3",
                    "totalUnrealizedProfit":"4",
                    "totalMarginBalance":"5",
                    "totalPositionInitialMargin":"6",
                    "totalOpenOrderInitialMargin":"7",
                    "totalCrossWalletBalance":"8",
                    "totalCrossUnPnl":"9",
                    "availableBalance":"10",
                    "maxWithdrawAmount":"11",
                    "assets":[{
                      "asset":"USDT",
                      "walletBalance":"12",
                      "unrealizedProfit":"13",
                      "marginBalance":"14",
                      "maintMargin":"15",
                      "initialMargin":"16",
                      "positionInitialMargin":"17",
                      "openOrderInitialMargin":"18",
                      "crossWalletBalance":"19",
                      "crossUnPnl":"20",
                      "availableBalance":"21",
                      "maxWithdrawAmount":"22",
                      "marginAvailable":true,
                      "updateTime":1625474304765
                    }],
                    "positions":[{
                      "symbol":"ETHUSDT",
                      "positionSide":"LONG",
                      "positionAmt":"23",
                      "unrealizedProfit":"24",
                      "isolatedMargin":"25",
                      "notional":"26",
                      "isolatedWallet":"27",
                      "initialMargin":"28",
                      "maintMargin":"29",
                      "updateTime":1625474304766
                    }]
                  },
                  "rateLimits":[]
                }
                """));

        Assert.True(response.Success);
        Assert.Equal(11m, response.Data.Result.MaxWithdrawQuantity);
        Assert.True(Assert.Single(response.Data.Result.Assets).MarginAvailable);
        var position = Assert.Single(response.Data.Result.Positions);
        Assert.Equal(BinancePositionSide.Long, position.PositionSide);
        Assert.Equal(26m, position.Notional);
        Assert.Equal(29m, position.MaintenanceMargin);
    }

    [Fact]
    public void BalanceV1AndV2ShareTheDocumentedResponseShape()
    {
        var response = Client().Deserializer<BinanceResultWithRateLimits<List<BinanceFuturesUsdAccountBalance>>>(
            JToken.Parse("""
                {
                  "id":"balance",
                  "status":200,
                  "result":[{
                    "accountAlias":"SgsR",
                    "asset":"USDT",
                    "balance":"122607.35137903",
                    "crossWalletBalance":"23.72469206",
                    "crossUnPnl":"1.2",
                    "availableBalance":"22.5",
                    "maxWithdrawAmount":"21.4",
                    "marginAvailable":true,
                    "updateTime":1617939110373
                  }],
                  "rateLimits":[]
                }
                """));

        Assert.True(response.Success);
        var balance = Assert.Single(response.Data.Result);
        Assert.Equal("SgsR", balance.AccountAlias);
        Assert.Equal(122_607.35137903m, balance.WalletBalance);
        Assert.Equal(1.2m, balance.CrossUnrealizedPnl);
        Assert.Equal(21.4m, balance.MaxWithdrawQuantity);
        Assert.True(balance.MarginAvailable);
    }

    [Fact]
    public void PositionV1AndV2DeserializeTheirDistinctCurrentShapes()
    {
        var client = Client();
        var v1 = client.Deserializer<BinanceResultWithRateLimits<List<BinanceFuturesUsdtPosition>>>(
            JToken.Parse("""
                {
                  "id":"position-v1",
                  "status":200,
                  "result":[{
                    "entryPrice":"1.1",
                    "breakEvenPrice":"2.2",
                    "marginType":"isolated",
                    "isAutoAddMargin":"false",
                    "isolatedMargin":"3.3",
                    "leverage":"10",
                    "liquidationPrice":"4.4",
                    "markPrice":"5.5",
                    "maxNotionalValue":"6.6",
                    "positionAmt":"7.7",
                    "notional":"8.8",
                    "isolatedWallet":"9.9",
                    "symbol":"BTCUSDT",
                    "unRealizedProfit":"10.1",
                    "positionSide":"BOTH",
                    "updateTime":1625474304765
                  }],
                  "rateLimits":[]
                }
                """));

        Assert.True(v1.Success);
        var v1Position = Assert.Single(v1.Data.Result);
        Assert.Equal(BinanceFuturesMarginType.Isolated, v1Position.MarginType);
        Assert.False(v1Position.IsAutoAddMargin);
        Assert.Equal(10.1m, v1Position.UnrealizedProfit);

        var v2 = client.Deserializer<BinanceResultWithRateLimits<List<BinanceFuturesPositionV3>>>(
            JToken.Parse("""
                {
                  "id":"position-v2",
                  "status":200,
                  "result":[{
                    "symbol":"ADAUSDT",
                    "positionSide":"SHORT",
                    "positionAmt":"30",
                    "entryPrice":"0.385",
                    "breakEvenPrice":"0.385077",
                    "markPrice":"0.41047590",
                    "unRealizedProfit":"0.76427700",
                    "liquidationPrice":"0",
                    "isolatedMargin":"0",
                    "notional":"12.31427700",
                    "marginAsset":"USDT",
                    "isolatedWallet":"0",
                    "initialMargin":"0.61571385",
                    "maintMargin":"0.08004280",
                    "positionInitialMargin":"0.61571385",
                    "openOrderInitialMargin":"0",
                    "adl":9223372036854775805,
                    "bidNotional":"0",
                    "askNotional":"0",
                    "updateTime":1720736417660
                  }],
                  "rateLimits":[]
                }
                """));

        Assert.True(v2.Success);
        var v2Position = Assert.Single(v2.Data.Result);
        Assert.Equal(BinancePositionSide.Short, v2Position.PositionSide);
        Assert.Equal(0.76427700m, v2Position.UnrealizedProfit);
        Assert.Equal("USDT", v2Position.MarginAsset);
        Assert.Equal(9_223_372_036_854_775_805L, v2Position.Adl);
    }

    private static BinanceFuturesSocketClientUsd Client()
        => Assert.IsType<BinanceFuturesSocketClientUsd>(new BinanceSocketApiClient().UsdFutures);
}
