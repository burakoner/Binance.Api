using Binance.Api.Futures;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamTickerTests
{
    [Fact]
    public void UsdTopics_UseCanonicalNamesAndValidateSymbols()
    {
        Assert.Equal(
            ["btcusdt@ticker", "ethusdt@ticker"],
            BinanceFuturesSocketClientUsd.TickerStreamTopics(["BTCUSDT", "ETHUSDT"]));
        Assert.Equal("!ticker@arr", BinanceFuturesSocketClientUsd.TickerAllMarketStreamTopic);

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientUsd.TickerStreamTopics(null!));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.TickerStreamTopics([]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.TickerStreamTopics([" "]));
    }

    [Fact]
    public void CoinTopics_UseCanonicalNamesAndPreserveDerivativeSymbols()
    {
        Assert.Equal(
            ["btcusd_perp@ticker", "btcusd_260925@ticker"],
            BinanceFuturesSocketClientCoin.TickerStreamTopics(["BTCUSD_PERP", "BTCUSD_260925"]));
        Assert.Equal("!ticker@arr", BinanceFuturesSocketClientCoin.TickerAllMarketStreamTopic);

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientCoin.TickerStreamTopics(null!));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.TickerStreamTopics([]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.TickerStreamTopics([""]));
    }

    [Fact]
    public void UsdIndividualPayload_MapsCompleteCurrentSchemaAndUsdVolumes()
    {
        const string payload = """
            {
              "e": "24hrTicker",
              "E": 1753344000002,
              "s": "BNBUSDT",
              "p": "10.25000001",
              "P": "1.60000002",
              "w": "645.12500003",
              "c": "650.25000004",
              "Q": "2.12500005",
              "o": "640.00000006",
              "h": "655.00000007",
              "l": "638.50000008",
              "v": "10000.12345678",
              "q": "6480000.87654321",
              "O": 1753257600001,
              "C": 1753344000001,
              "F": 9223372036854775700,
              "L": 9223372036854775806,
              "n": 9223372036854775805,
              "ps": "BNBUSDT",
              "st": 1
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamTick>(payload);

        Assert.NotNull(update);
        Assert.Equal("24hrTicker", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.EventTime);
        Assert.Equal("BNBUSDT", update.Symbol);
        Assert.Equal("BNBUSDT", update.Pair);
        Assert.Equal(10.25000001m, update.PriceChange);
        Assert.Equal(1.60000002m, update.PriceChangePercent);
        Assert.Equal(645.12500003m, update.WeightedAveragePrice);
        Assert.Equal(650.25000004m, update.LastPrice);
        Assert.Equal(2.12500005m, update.LastQuantity);
        Assert.Equal(640.00000006m, update.OpenPrice);
        Assert.Equal(655.00000007m, update.HighPrice);
        Assert.Equal(638.50000008m, update.LowPrice);
        Assert.Equal(10_000.12345678m, update.RawVolume);
        Assert.Equal(6_480_000.87654321m, update.RawQuoteOrBaseAssetVolume);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_257_600_001).UtcDateTime, update.OpenTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.CloseTime);
        Assert.Equal(9_223_372_036_854_775_700L, update.FirstTradeId);
        Assert.Equal(9_223_372_036_854_775_806L, update.LastTradeId);
        Assert.Equal(9_223_372_036_854_775_805L, update.TotalTrades);
        Assert.Equal(1, update.SymbolType);
        Assert.Equal(10_000.12345678m, update.BaseAssetVolume);
        Assert.Equal(6_480_000.87654321m, update.QuoteAssetVolume);
        Assert.Null(update.ContractVolume);

        var model = typeof(BinanceFuturesStreamTick);
        Assert.Null(model.GetProperty("PrevDayClosePrice"));
        Assert.Null(model.GetProperty("BestBidPrice"));
        Assert.Null(model.GetProperty("BestBidQuantity"));
        Assert.Null(model.GetProperty("BestAskPrice"));
        Assert.Null(model.GetProperty("BestAskQuantity"));
    }

    [Fact]
    public void CoinIndividualPayload_MapsCompleteCurrentSchemaAndCoinVolumes()
    {
        const string payload = """
            {
              "e": "24hrTicker",
              "E": 1753344000002,
              "s": "BTCUSD_260925",
              "p": "-43.40000001",
              "P": "-0.45200002",
              "w": "9549.12500003",
              "c": "9548.50000004",
              "Q": "2.12500005",
              "o": "9591.90000006",
              "h": "10000.00000007",
              "l": "7000.00000008",
              "v": "487850.12500000",
              "q": "52.34567891",
              "O": 1753257600001,
              "C": 1753344000001,
              "F": 9223372036854775700,
              "L": 9223372036854775806,
              "n": 9223372036854775805,
              "ps": "BTCUSD",
              "st": 2
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamTick>(payload);

        Assert.NotNull(update);
        Assert.Equal("24hrTicker", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSD_260925", update.Symbol);
        Assert.Equal("BTCUSD", update.Pair);
        Assert.Equal(-43.40000001m, update.PriceChange);
        Assert.Equal(-0.45200002m, update.PriceChangePercent);
        Assert.Equal(9_549.12500003m, update.WeightedAveragePrice);
        Assert.Equal(9_548.50000004m, update.LastPrice);
        Assert.Equal(2.12500005m, update.LastQuantity);
        Assert.Equal(9_591.90000006m, update.OpenPrice);
        Assert.Equal(10_000.00000007m, update.HighPrice);
        Assert.Equal(7_000.00000008m, update.LowPrice);
        Assert.Equal(487_850.125m, update.RawVolume);
        Assert.Equal(52.34567891m, update.RawQuoteOrBaseAssetVolume);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_257_600_001).UtcDateTime, update.OpenTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.CloseTime);
        Assert.Equal(9_223_372_036_854_775_700L, update.FirstTradeId);
        Assert.Equal(9_223_372_036_854_775_806L, update.LastTradeId);
        Assert.Equal(9_223_372_036_854_775_805L, update.TotalTrades);
        Assert.Equal(2, update.SymbolType);
        Assert.Equal(52.34567891m, update.BaseAssetVolume);
        Assert.Null(update.QuoteAssetVolume);
        Assert.Equal(487_850.125m, update.ContractVolume);
    }

    [Fact]
    public void MergedAllMarketPayload_SeparatesUsdAndCoinVolumeSemantics()
    {
        const string payload = """
            [
              {
                "e": "24hrTicker",
                "E": 1753344000002,
                "s": "BNBUSDT",
                "ps": "BNBUSDT",
                "v": "10000.125",
                "q": "6480000.875",
                "st": 1
              },
              {
                "e": "24hrTicker",
                "E": 1753344000002,
                "s": "BTCUSD_260925",
                "ps": "BTCUSD",
                "v": "487850.25",
                "q": "52.125",
                "st": 2
              }
            ]
            """;

        var updates = JsonConvert.DeserializeObject<List<BinanceFuturesStreamTick>>(payload);

        Assert.NotNull(updates);
        Assert.Equal(2, updates.Count);

        Assert.Equal(1, updates[0].SymbolType);
        Assert.Equal(10_000.125m, updates[0].BaseAssetVolume);
        Assert.Equal(6_480_000.875m, updates[0].QuoteAssetVolume);
        Assert.Null(updates[0].ContractVolume);

        Assert.Equal(2, updates[1].SymbolType);
        Assert.Equal(52.125m, updates[1].BaseAssetVolume);
        Assert.Null(updates[1].QuoteAssetVolume);
        Assert.Equal(487_850.25m, updates[1].ContractVolume);
    }
}
