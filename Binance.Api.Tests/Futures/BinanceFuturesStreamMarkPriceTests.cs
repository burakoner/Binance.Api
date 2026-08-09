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

        var updates = JsonConvert.DeserializeObject<List<BinanceFuturesUsdtStreamMarkPrice>>(payload);

        var update = Assert.Single(Assert.IsType<List<BinanceFuturesUsdtStreamMarkPrice>>(updates));
        Assert.Equal("BTCUSD_PERP", update.Symbol);
        Assert.Equal(11_185.87786614m, update.MarkPriceMovingAverage);
        Assert.Equal(2, update.SymbolType);
    }
}
