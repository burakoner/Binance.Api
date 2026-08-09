using Binance.Api.Futures;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamMarkPriceTests
{
    [Fact]
    public void Topics_UseCanonicalNamesAndUpdateSpeeds()
    {
        Assert.Equal(
            ["btcusdt@markPrice", "ethusdt@markPrice"],
            BinanceFuturesSocketClientUsd.MarkPriceStreamTopics(["BTCUSDT", "ETHUSDT"], null));
        Assert.Equal(
            ["btcusdt@markPrice"],
            BinanceFuturesSocketClientUsd.MarkPriceStreamTopics(["BTCUSDT"], 3000));
        Assert.Equal(
            ["btcusdt@markPrice@1s", "btcusd_perp@markPrice@1s"],
            BinanceFuturesSocketClientUsd.MarkPriceStreamTopics(["BTCUSDT", "BTCUSD_PERP"], 1000));
        Assert.Equal("!markPrice@arr", BinanceFuturesSocketClientUsd.MarkPriceAllMarketStreamTopic(null));
        Assert.Equal("!markPrice@arr", BinanceFuturesSocketClientUsd.MarkPriceAllMarketStreamTopic(3000));
        Assert.Equal("!markPrice@arr@1s", BinanceFuturesSocketClientUsd.MarkPriceAllMarketStreamTopic(1000));

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientUsd.MarkPriceStreamTopics(null!, null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.MarkPriceStreamTopics([], null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.MarkPriceStreamTopics([""], null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.MarkPriceStreamTopics(["BTCUSDT"], 2000));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.MarkPriceAllMarketStreamTopic(2000));
    }

    [Fact]
    public void CoinTopics_UseCanonicalNamesAndUpdateSpeeds()
    {
        Assert.Equal(
            ["btcusd_perp@markPrice", "btcusd_260925@markPrice"],
            BinanceFuturesSocketClientCoin.MarkPriceStreamTopics(["BTCUSD_PERP", "BTCUSD_260925"], null));
        Assert.Equal(
            ["btcusd_perp@markPrice"],
            BinanceFuturesSocketClientCoin.MarkPriceStreamTopics(["BTCUSD_PERP"], 3000));
        Assert.Equal(
            ["btcusd_perp@markPrice@1s"],
            BinanceFuturesSocketClientCoin.MarkPriceStreamTopics(["BTCUSD_PERP"], 1000));
        Assert.Equal("btcusd@markPrice", BinanceFuturesSocketClientCoin.MarkPricePairStreamTopic("BTCUSD", null));
        Assert.Equal("btcusd@markPrice", BinanceFuturesSocketClientCoin.MarkPricePairStreamTopic("BTCUSD", 3000));
        Assert.Equal("btcusd@markPrice@1s", BinanceFuturesSocketClientCoin.MarkPricePairStreamTopic("BTCUSD", 1000));
        Assert.Equal("!markPrice@arr", BinanceFuturesSocketClientCoin.MarkPriceAllMarketStreamTopic(null));
        Assert.Equal("!markPrice@arr", BinanceFuturesSocketClientCoin.MarkPriceAllMarketStreamTopic(3000));
        Assert.Equal("!markPrice@arr@1s", BinanceFuturesSocketClientCoin.MarkPriceAllMarketStreamTopic(1000));

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientCoin.MarkPriceStreamTopics(null!, null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.MarkPriceStreamTopics([], null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.MarkPriceStreamTopics([" "], null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.MarkPriceStreamTopics(["BTCUSD_PERP"], 2000));
        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientCoin.MarkPricePairStreamTopic(null!, null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.MarkPricePairStreamTopic(" ", null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.MarkPricePairStreamTopic("BTCUSD", 2000));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.MarkPriceAllMarketStreamTopic(2000));
    }

    [Fact]
    public void PerSymbolPayload_MapsCurrentOfficialFields()
    {
        const string payload = """
            {
              "e": "markPriceUpdate",
              "E": 1562305380000,
              "s": "BTCUSDT",
              "p": "11794.15000000",
              "i": "11784.62659091",
              "P": "11784.25641265",
              "r": "0.00038167",
              "ap": "11794.15000000",
              "T": 1562306400000,
              "st": 1
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesUsdtStreamMarkPrice>(payload);

        Assert.NotNull(update);
        Assert.Equal("markPriceUpdate", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_562_305_380_000).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSDT", update.Symbol);
        Assert.Equal(11_794.15m, update.MarkPrice);
        Assert.Equal(11_784.62659091m, update.IndexPrice);
        Assert.Equal(11_784.25641265m, update.EstimatedSettlePrice);
        Assert.Equal(0.00038167m, update.FundingRate);
        Assert.Equal(11_794.15m, update.MarkPriceMovingAverage);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_562_306_400_000).UtcDateTime, update.NextFundingTime);
        Assert.Equal(1, update.SymbolType);
    }

    [Fact]
    public void AllMarketPayload_MapsMergedCoinMarginedSymbol()
    {
        const string payload = """
            [{
              "e": "markPriceUpdate",
              "E": 1562305380000,
              "s": "BTCUSD_PERP",
              "p": "11185.87786614",
              "i": "11784.62659091",
              "P": "11784.25641265",
              "r": "0.00030000",
              "ap": "11185.87786614",
              "T": 1562306400000,
              "st": 2
            }]
            """;

        var updates = JsonConvert.DeserializeObject<List<BinanceFuturesStreamAllMarketMarkPrice>>(payload);

        var update = Assert.Single(Assert.IsType<List<BinanceFuturesStreamAllMarketMarkPrice>>(updates));
        Assert.Equal("BTCUSD_PERP", update.Symbol);
        Assert.Equal(11_185.87786614m, update.MarkPriceMovingAverage);
        Assert.Equal(2, update.SymbolType);
    }

    [Fact]
    public void CoinPerSymbolPayload_MapsCompleteCurrentOfficialSchema()
    {
        const string payload = """
            {
              "e": "markPriceUpdate",
              "E": 1753344000001,
              "s": "BTCUSD_PERP",
              "p": "11834.62615417",
              "P": "11862.17178236",
              "i": "11833.62615417",
              "r": "0.00010000",
              "T": 1753347600000,
              "st": 2
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesCoinStreamMarkPrice>(payload);

        Assert.NotNull(update);
        Assert.Equal("markPriceUpdate", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSD_PERP", update.Symbol);
        Assert.Equal(11_834.62615417m, update.MarkPrice);
        Assert.Equal(11_862.17178236m, update.EstimatedSettlePrice);
        Assert.Equal(11_833.62615417m, update.IndexPrice);
        Assert.Equal(0.0001m, update.FundingRate);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_347_600_000).UtcDateTime, update.NextFundingTime);
        Assert.Equal(2, update.SymbolType);

        BinanceFuturesStreamMarkPrice baseUpdate = update;
        Assert.Equal(11_862.17178236m, baseUpdate.EstimatedSettlePrice);
        Assert.Null(typeof(BinanceFuturesCoinStreamMarkPrice).GetProperty("MarkPriceMovingAverage"));
    }

    [Fact]
    public void CoinPairPayload_MapsDeliveryContractEmptyFundingFields()
    {
        const string payload = """
            [{
              "e": "markPriceUpdate",
              "E": 1753344000001,
              "s": "BTCUSD_260925",
              "p": "10934.62615417",
              "P": "10962.17178236",
              "i": "10933.62615417",
              "r": "",
              "T": 0,
              "st": 2
            }]
            """;

        var updates = JsonConvert.DeserializeObject<List<BinanceFuturesCoinStreamMarkPrice>>(payload);

        var update = Assert.Single(Assert.IsType<List<BinanceFuturesCoinStreamMarkPrice>>(updates));
        Assert.Equal("BTCUSD_260925", update.Symbol);
        Assert.Equal(10_934.62615417m, update.MarkPrice);
        Assert.Equal(10_962.17178236m, update.EstimatedSettlePrice);
        Assert.Equal(10_933.62615417m, update.IndexPrice);
        Assert.Null(update.FundingRate);
        Assert.Equal(default, update.NextFundingTime);
        Assert.Equal(2, update.SymbolType);
    }

    [Fact]
    public void CoinAllMarketPayload_MapsCompleteCrossHostOfficialSchema()
    {
        const string payload = """
            [{
              "e": "markPriceUpdate",
              "E": 1753344000001,
              "s": "BTCUSDT",
              "p": "11834.62615417",
              "P": "11862.17178236",
              "i": "11833.62615417",
              "r": "0.00010000",
              "ap": "11834.62615417",
              "T": 1753347600000,
              "st": 1
            }]
            """;

        var updates = JsonConvert.DeserializeObject<List<BinanceFuturesStreamAllMarketMarkPrice>>(payload);

        var update = Assert.Single(Assert.IsType<List<BinanceFuturesStreamAllMarketMarkPrice>>(updates));
        Assert.Equal("markPriceUpdate", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSDT", update.Symbol);
        Assert.Equal(11_834.62615417m, update.MarkPrice);
        Assert.Equal(11_862.17178236m, update.EstimatedSettlePrice);
        Assert.Equal(11_833.62615417m, update.IndexPrice);
        Assert.Equal(0.0001m, update.FundingRate);
        Assert.Equal(11_834.62615417m, update.MarkPriceMovingAverage);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_347_600_000).UtcDateTime, update.NextFundingTime);
        Assert.Equal(1, update.SymbolType);
    }
}
