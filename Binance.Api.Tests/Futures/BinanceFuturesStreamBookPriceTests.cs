using Binance.Api.Futures;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamBookPriceTests
{
    [Fact]
    public void CoinTopics_UseCanonicalNamesAndPreserveDerivativeSymbols()
    {
        Assert.Equal(
            ["btcusd_perp@bookTicker", "btcusd_260925@bookTicker"],
            BinanceFuturesSocketClientCoin.BookTickerStreamTopics(["BTCUSD_PERP", "BTCUSD_260925"]));
        Assert.Equal("!bookTicker", BinanceFuturesSocketClientCoin.BookTickerAllMarketStreamTopic);

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientCoin.BookTickerStreamTopics(null!));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.BookTickerStreamTopics([]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.BookTickerStreamTopics([" "]));
    }

    [Fact]
    public void CoinIndividualPayload_MapsCompleteCurrentOfficialSchema()
    {
        const string payload = """
            {
              "e": "bookTicker",
              "u": 9223372036854775806,
              "s": "BTCUSD_260925",
              "b": "9548.12345678",
              "B": "52.12500000",
              "a": "9548.87654321",
              "A": "11.37500000",
              "T": 1753344000001,
              "E": 1753344000002,
              "ps": "BTCUSD",
              "st": 2
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamBookPrice>(payload);

        Assert.NotNull(update);
        Assert.Equal("bookTicker", update.Event);
        Assert.Equal(9_223_372_036_854_775_806L, update.UpdateId);
        Assert.Equal("BTCUSD_260925", update.Symbol);
        Assert.Equal("BTCUSD", update.Pair);
        Assert.Equal(9_548.12345678m, update.BestBidPrice);
        Assert.Equal(52.125m, update.BestBidQuantity);
        Assert.Equal(9_548.87654321m, update.BestAskPrice);
        Assert.Equal(11.375m, update.BestAskQuantity);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.TransactionTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.EventTime);
        Assert.Equal(2, update.SymbolType);
    }

    [Fact]
    public void CoinAllMarketPayload_MapsMergedUsdMarginedSymbol()
    {
        const string payload = """
            {
              "e": "bookTicker",
              "u": 400900217,
              "s": "BNBUSDT",
              "b": "25.35190000",
              "B": "31.21000000",
              "a": "25.36520000",
              "A": "40.66000000",
              "T": 1753344000001,
              "E": 1753344000002,
              "ps": "BNBUSDT",
              "st": 1
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamBookPrice>(payload);

        Assert.NotNull(update);
        Assert.Equal("bookTicker", update.Event);
        Assert.Equal(400_900_217L, update.UpdateId);
        Assert.Equal("BNBUSDT", update.Symbol);
        Assert.Equal("BNBUSDT", update.Pair);
        Assert.Equal(25.3519m, update.BestBidPrice);
        Assert.Equal(31.21m, update.BestBidQuantity);
        Assert.Equal(25.3652m, update.BestAskPrice);
        Assert.Equal(40.66m, update.BestAskQuantity);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.TransactionTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.EventTime);
        Assert.Equal(1, update.SymbolType);
    }
}
