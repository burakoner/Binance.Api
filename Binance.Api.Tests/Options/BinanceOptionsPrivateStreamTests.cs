using Binance.Api.Options;
using Binance.Api.Shared;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsPrivateStreamTests
{
    [Fact]
    public void UserDataStream_UsesCurrentDirectListenKeyRouteAndSixCallbacks()
    {
        Assert.Equal("wss://fstream.binance.com/private", BinanceAddress.Default.EuropeanOptionsPrivateSocketApiStreamAddress);
        Assert.Equal(
            "wss://fstream.binance.com/private/ws/Case-Sensitive-Listen-Key",
            BinanceOptionsSocketClient.UserDataStreamAddress("Case-Sensitive-Listen-Key"));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.UserDataStreamAddress(null!));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.UserDataStreamAddress(string.Empty));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.UserDataStreamAddress(" "));

        var method = typeof(IBinanceOptionsSocketClientStreamUserDataStream)
            .GetMethod(nameof(IBinanceOptionsSocketClientStreamUserDataStream.SubscribeToUserDataStreamAsync))!;
        var callbackTypes = method.GetParameters()
            .Where(parameter => parameter.Name!.StartsWith("on", StringComparison.Ordinal))
            .Select(parameter => parameter.ParameterType.GenericTypeArguments[0].GenericTypeArguments[0])
            .ToList();

        Assert.Equal(
            [
                typeof(BinanceOptionsStreamAccount),
                typeof(BinanceOptionsStreamOrder),
                typeof(BinanceOptionsStreamRiskLevel),
                typeof(BinanceOptionsStreamBalancePosition),
                typeof(BinanceOptionsStreamGreek),
                typeof(BinanceOptionsStreamListenKeyExpired)
            ],
            callbackTypes);
    }

    [Fact]
    public void AccountAndRiskEvents_UseCurrentFlatPayloads()
    {
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceOptionsSocketClient>(root.Options);

        var accountResult = client.DeserializeUserDataEvent(JToken.Parse(
            """{"e":"ACCOUNT_UPDATE","E":1762914568643,"T":1762914568619,"eq":"10000371.61462086","aeq":"10000475.51032086","b":"10000475.51032086","m":"-103.89570000","u":"16.10430000","i":"32354.38562539","M":"6089.28766956"}"""),
            "route-listen-key");
        Assert.Null(accountResult.Error);
        var account = Assert.IsType<BinanceOptionsStreamAccount>(accountResult.Data);
        Assert.Equal("ACCOUNT_UPDATE", account.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_762_914_568_643).UtcDateTime, account.EventTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_762_914_568_619).UtcDateTime, account.TransactionTime);
        Assert.Equal("route-listen-key", account.ListenKey);
        Assert.Equal(10_000_371.61462086m, account.Equity);
        Assert.Equal(10_000_475.51032086m, account.AdjustedEquity);
        Assert.Equal(10_000_475.51032086m, account.WalletBalance);
        Assert.Equal(-103.8957m, account.PositionValue);
        Assert.Equal(16.1043m, account.UnrealizedPnl);
        Assert.Equal(32_354.38562539m, account.InitialMargin);
        Assert.Equal(6_089.28766956m, account.MaintenanceMargin);

        var riskResult = client.DeserializeUserDataEvent(JToken.Parse(
            """{"e":"RISK_LEVEL_CHANGE","E":1587727187525,"s":"REDUCE_ONLY","mb":"1534.11708371","mm":"254789.11708371"}"""),
            "route-listen-key");
        Assert.Null(riskResult.Error);
        var risk = Assert.IsType<BinanceOptionsStreamRiskLevel>(riskResult.Data);
        Assert.Equal("route-listen-key", risk.ListenKey);
        Assert.Equal(BinanceOptionsRiskLevel.ReduceOnly, risk.RiskLevel);
        Assert.Equal(1_534.11708371m, risk.MarginBalance);
        Assert.Equal(254_789.11708371m, risk.MaintenanceMargin);
    }

    [Fact]
    public void BalancePositionAndGreekEvents_PreserveAllCurrentFields()
    {
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceOptionsSocketClient>(root.Options);

        var balanceResult = client.DeserializeUserDataEvent(JToken.Parse(
            """{"e":"BALANCE_POSITION_UPDATE","E":1762917544216,"T":1762917544206,"m":"ORDER","B":[{"a":"USDT","b":"100.25","bc":"-2.5"}],"P":[{"s":"BTC-260925-50000-C","c":"1.25","p":"125.75","a":"100.6"}]}"""),
            "route-listen-key");
        Assert.Null(balanceResult.Error);
        var balancePosition = Assert.IsType<BinanceOptionsStreamBalancePosition>(balanceResult.Data);
        Assert.Equal(BinanceOptionsBalancePositionUpdateReason.Order, balancePosition.Reason);
        Assert.Equal("route-listen-key", balancePosition.ListenKey);
        var balance = Assert.Single(balancePosition.Balances);
        Assert.Equal("USDT", balance.Asset);
        Assert.Equal(100.25m, balance.Balance);
        Assert.Equal(-2.5m, balance.BalanceChange);
        var position = Assert.Single(balancePosition.Positions);
        Assert.Equal("BTC-260925-50000-C", position.Symbol);
        Assert.Equal(1.25m, position.Quantity);
        Assert.Equal(125.75m, position.PositionValue);
        Assert.Equal(100.6m, position.AverageEntryPrice);

        var greekResult = client.DeserializeUserDataEvent(JToken.Parse(
            """{"e":"GREEK_UPDATE","E":1762917544216,"T":1762917544216,"G":[{"u":"BTCUSDT","d":"0.1","g":"0.2","t":"-0.3","v":"0.4"}]}"""),
            "route-listen-key");
        Assert.Null(greekResult.Error);
        var greekUpdate = Assert.IsType<BinanceOptionsStreamGreek>(greekResult.Data);
        Assert.Equal("route-listen-key", greekUpdate.ListenKey);
        var greek = Assert.Single(greekUpdate.Greeks);
        Assert.Equal("BTCUSDT", greek.Underlying);
        Assert.Equal(0.1m, greek.Delta);
        Assert.Equal(0.2m, greek.Gamma);
        Assert.Equal(-0.3m, greek.Theta);
        Assert.Equal(0.4m, greek.Vega);
    }

    [Fact]
    public void OrderEvent_UsesNestedCurrentPayloadAndCompleteFields()
    {
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceOptionsSocketClient>(root.Options);
        var result = client.DeserializeUserDataEvent(JToken.Parse(
            """
            {
              "e":"ORDER_TRADE_UPDATE","E":1762917544216,"T":1762917544206,
              "o":{
                "s":"BTC-260925-50000-C","c":"client-id","S":"SELL","o":"LIMIT","f":"GTC",
                "q":"2.5","p":"100.25","ap":"100.5","x":"TRADE","X":"PARTIALLY_FILLED",
                "i":9223372036854775807,"l":"1.25","z":"1.25","L":"100.5","N":"USDT","n":"-0.0125",
                "T":1762917544205,"t":9223372036854775806,"b":"3.5","a":"4.5","m":true,"R":false,
                "ot":"LIMIT","rp":"12.75","V":"EXPIRE_TAKER"
              }
            }
            """),
            "route-listen-key");

        Assert.Null(result.Error);
        var update = Assert.IsType<BinanceOptionsStreamOrder>(result.Data);
        Assert.Equal("route-listen-key", update.ListenKey);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_762_917_544_206).UtcDateTime, update.TransactionTime);
        var order = update.Order;
        Assert.Equal("BTC-260925-50000-C", order.Symbol);
        Assert.Equal("client-id", order.ClientOrderId);
        Assert.Equal(BinanceOrderSide.Sell, order.Side);
        Assert.Equal(BinanceOptionsOrderType.Limit, order.Type);
        Assert.Equal(BinanceTimeInForce.GoodTillCanceled, order.TimeInForce);
        Assert.Equal(2.5m, order.Quantity);
        Assert.Equal(100.25m, order.Price);
        Assert.Equal(100.5m, order.AveragePrice);
        Assert.Equal(BinanceOptionsExecutionType.Trade, order.ExecutionType);
        Assert.Equal(BinanceOrderStatus.PartiallyFilled, order.Status);
        Assert.Equal(long.MaxValue, order.Id);
        Assert.Equal(1.25m, order.LastFilledQuantity);
        Assert.Equal(1.25m, order.AccumulatedFilledQuantity);
        Assert.Equal(100.5m, order.LastFilledPrice);
        Assert.Equal("USDT", order.CommissionAsset);
        Assert.Equal(-0.0125m, order.Commission);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_762_917_544_205).UtcDateTime, order.TradeTime);
        Assert.Equal(long.MaxValue - 1, order.TradeId);
        Assert.Equal(3.5m, order.BidQuantity);
        Assert.Equal(4.5m, order.AskQuantity);
        Assert.True(order.IsMaker);
        Assert.False(order.ReduceOnly);
        Assert.Equal(BinanceOptionsOrderType.Limit, order.OriginalType);
        Assert.Equal(12.75m, order.RealizedProfit);
        Assert.Equal(BinanceSelfTradePreventionMode.ExpireTaker, order.SelfTradePreventionMode);
    }

    [Fact]
    public void ListenKeyExpiredAndEnvelopeParsing_UseCurrentPayload()
    {
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceOptionsSocketClient>(root.Options);
        const string payload = """{"e":"listenKeyExpired","E":"1736996475556","listenKey":"expired-listen-key"}""";

        var raw = BinanceOptionsSocketClient.ParseUserDataPayload(payload);
        var combined = BinanceOptionsSocketClient.ParseUserDataPayload($$"""{"stream":"route-listen-key","data":{{payload}}}""");
        Assert.Equal(raw, combined);

        var result = client.DeserializeUserDataEvent(combined, "route-listen-key");
        Assert.Null(result.Error);
        var expired = Assert.IsType<BinanceOptionsStreamListenKeyExpired>(result.Data);
        Assert.Equal("listenKeyExpired", expired.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_736_996_475_556).UtcDateTime, expired.EventTime);
        Assert.Equal("expired-listen-key", expired.ListenKey);

        var unknown = client.DeserializeUserDataEvent(JToken.Parse("""{"e":"UNKNOWN"}"""), "route-listen-key");
        Assert.Null(unknown.Data);
        Assert.Null(unknown.Error);
    }
}
