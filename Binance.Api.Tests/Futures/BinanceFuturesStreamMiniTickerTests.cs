using Binance.Api.Futures;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamMiniTickerTests
{
    [Fact]
    public void UsdTopics_UseCanonicalNamesAndValidateSymbols()
    {
        Assert.Equal(
            ["btcusdt@miniTicker", "ethusdt@miniTicker"],
            BinanceFuturesSocketClientUsd.MiniTickerStreamTopics(["BTCUSDT", "ETHUSDT"]));
        Assert.Equal("!miniTicker@arr", BinanceFuturesSocketClientUsd.MiniTickerAllMarketStreamTopic);

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientUsd.MiniTickerStreamTopics(null!));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.MiniTickerStreamTopics([]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.MiniTickerStreamTopics([" "]));
    }

    [Fact]
    public void CoinTopics_UseCanonicalNamesAndPreserveDerivativeSymbols()
    {
        Assert.Equal(
            ["btcusd_perp@miniTicker", "btcusd_260925@miniTicker"],
            BinanceFuturesSocketClientCoin.MiniTickerStreamTopics(["BTCUSD_PERP", "BTCUSD_260925"]));
        Assert.Equal("!miniTicker@arr", BinanceFuturesSocketClientCoin.MiniTickerAllMarketStreamTopic);

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientCoin.MiniTickerStreamTopics(null!));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.MiniTickerStreamTopics([]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.MiniTickerStreamTopics([""]));
    }

    [Fact]
    public void UsdIndividualPayload_MapsCompleteCurrentSchemaAndUsdVolumes()
    {
        const string payload = """
            {
              "e": "24hrMiniTicker",
              "E": 1753344000002,
              "s": "BNBUSDT",
              "c": "0.00250001",
              "o": "0.00100002",
              "h": "0.00250003",
              "l": "0.00100004",
              "v": "10000.12345678",
              "q": "18.87654321",
              "ps": "BNBUSDT",
              "st": 1
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamMiniTick>(payload);

        Assert.NotNull(update);
        Assert.Equal("24hrMiniTicker", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.EventTime);
        Assert.Equal("BNBUSDT", update.Symbol);
        Assert.Equal("BNBUSDT", update.Pair);
        Assert.Equal(0.00250001m, update.LastPrice);
        Assert.Equal(0.00100002m, update.OpenPrice);
        Assert.Equal(0.00250003m, update.HighPrice);
        Assert.Equal(0.00100004m, update.LowPrice);
        Assert.Equal(10_000.12345678m, update.RawVolume);
        Assert.Equal(18.87654321m, update.RawQuoteOrBaseAssetVolume);
        Assert.Equal(1, update.SymbolType);
        Assert.Equal(10_000.12345678m, update.BaseAssetVolume);
        Assert.Equal(18.87654321m, update.QuoteAssetVolume);
        Assert.Null(update.ContractVolume);
    }

    [Fact]
    public void CoinIndividualPayload_MapsCompleteCurrentSchemaAndCoinVolumes()
    {
        const string payload = """
            {
              "e": "24hrMiniTicker",
              "E": 1753344000002,
              "s": "BTCUSD_260925",
              "ps": "BTCUSD",
              "c": "9561.70000001",
              "o": "9580.90000002",
              "h": "10000.00000003",
              "l": "7000.00000004",
              "v": "487476.12500000",
              "q": "52.34567891",
              "st": 2
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamMiniTick>(payload);

        Assert.NotNull(update);
        Assert.Equal("24hrMiniTicker", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSD_260925", update.Symbol);
        Assert.Equal("BTCUSD", update.Pair);
        Assert.Equal(9_561.70000001m, update.LastPrice);
        Assert.Equal(9_580.90000002m, update.OpenPrice);
        Assert.Equal(10_000.00000003m, update.HighPrice);
        Assert.Equal(7_000.00000004m, update.LowPrice);
        Assert.Equal(487_476.125m, update.RawVolume);
        Assert.Equal(52.34567891m, update.RawQuoteOrBaseAssetVolume);
        Assert.Equal(2, update.SymbolType);
        Assert.Equal(52.34567891m, update.BaseAssetVolume);
        Assert.Null(update.QuoteAssetVolume);
        Assert.Equal(487_476.125m, update.ContractVolume);
    }

    [Fact]
    public void MergedAllMarketPayload_SeparatesUsdAndCoinVolumeSemantics()
    {
        const string payload = """
            [
              {
                "e": "24hrMiniTicker",
                "E": 1753344000002,
                "s": "BNBUSDT",
                "ps": "BNBUSDT",
                "c": "650.25",
                "o": "640.00",
                "h": "655.00",
                "l": "638.50",
                "v": "10000.125",
                "q": "6480000.875",
                "st": 1
              },
              {
                "e": "24hrMiniTicker",
                "E": 1753344000002,
                "s": "BTCUSD_260925",
                "ps": "BTCUSD",
                "c": "9561.7",
                "o": "9580.9",
                "h": "10000.0",
                "l": "7000.0",
                "v": "487476.25",
                "q": "52.125",
                "st": 2
              }
            ]
            """;

        var updates = JsonConvert.DeserializeObject<List<BinanceFuturesStreamMiniTick>>(payload);

        Assert.NotNull(updates);
        Assert.Equal(2, updates.Count);

        Assert.Equal(1, updates[0].SymbolType);
        Assert.Equal(10_000.125m, updates[0].BaseAssetVolume);
        Assert.Equal(6_480_000.875m, updates[0].QuoteAssetVolume);
        Assert.Null(updates[0].ContractVolume);

        Assert.Equal(2, updates[1].SymbolType);
        Assert.Equal(52.125m, updates[1].BaseAssetVolume);
        Assert.Null(updates[1].QuoteAssetVolume);
        Assert.Equal(487_476.25m, updates[1].ContractVolume);
    }
}
