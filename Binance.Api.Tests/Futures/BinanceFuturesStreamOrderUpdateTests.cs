using Binance.Api.Futures;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamOrderUpdateTests
{
    [Fact]
    public void UsdAmendment_MapsCurrentConditionalAndProductFields()
    {
        const string payload = """
            {
              "e": "ORDER_TRADE_UPDATE",
              "E": 1753344000001,
              "T": 1753344000002,
              "o": {
                "s": "BTCUSDT",
                "c": "amended-order",
                "S": "BUY",
                "o": "LIMIT",
                "f": "GTC",
                "q": "0.01000000",
                "p": "120000.12345678",
                "ap": "0",
                "sp": "0",
                "x": "AMENDMENT",
                "X": "NEW",
                "i": 9223372036854775806,
                "M": "9007199254740993",
                "l": "0",
                "z": "0",
                "L": "0",
                "N": "USDT",
                "n": "0",
                "T": 1753344000003,
                "t": 0,
                "b": "1200.00123456",
                "a": "0",
                "m": false,
                "R": false,
                "wt": "CONTRACT_PRICE",
                "ot": "LIMIT",
                "ps": "BOTH",
                "cp": false,
                "AP": "0",
                "cr": "0",
                "pP": false,
                "si": 9223372036854775805,
                "ss": 9223372036854775804,
                "rp": "0",
                "V": "NONE",
                "pm": "NONE",
                "gtd": 1753430400000,
                "er": "0"
              }
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamOrderUpdate>(payload);

        Assert.NotNull(update);
        Assert.Equal("ORDER_TRADE_UPDATE", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.EventTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.TransactionTime);
        Assert.Equal(BinanceFuturesExecutionType.Amendment, update.UpdateData.ExecutionType);
        Assert.Equal(9_223_372_036_854_775_806L, update.UpdateData.OrderId);
        Assert.Equal("9007199254740993", update.UpdateData.ModifyId);
        Assert.Null(update.UpdateData.MarginAsset);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_003).UtcDateTime, update.UpdateData.UpdateTime);
        Assert.Equal(9_223_372_036_854_775_805L, update.UpdateData.StrategyId);
        Assert.Equal(9_223_372_036_854_775_804L, update.UpdateData.StrategyStatus);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_430_400_000).UtcDateTime, update.UpdateData.GoodTillDate);
        Assert.Equal("0", update.UpdateData.ExpiryReason);
    }

    [Fact]
    public void CoinAmendment_MapsMarginAssetAndPreservesConditionalAbsence()
    {
        const string payload = """
            {
              "e": "ORDER_TRADE_UPDATE",
              "E": 1753344000101,
              "T": 1753344000102,
              "i": "SfsR",
              "o": {
                "s": "BTCUSD_PERP",
                "x": "AMENDMENT",
                "i": 5363,
                "ma": "BTC",
                "b": "2",
                "a": "1",
                "T": 1753344000103,
                "er": "1"
              }
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamOrderUpdate>(payload);

        Assert.NotNull(update);
        Assert.Equal("SfsR", update.AccountAlias);
        Assert.Equal(BinanceFuturesExecutionType.Amendment, update.UpdateData.ExecutionType);
        Assert.Equal("BTC", update.UpdateData.MarginAsset);
        Assert.Equal(2m, update.UpdateData.BidNotional);
        Assert.Equal(1m, update.UpdateData.AskNotional);
        Assert.Null(update.UpdateData.ModifyId);
        Assert.Null(update.UpdateData.StrategyId);
        Assert.Null(update.UpdateData.StrategyStatus);
        Assert.Null(update.UpdateData.GoodTillDate);
        Assert.Equal("1", update.UpdateData.ExpiryReason);
    }
}
