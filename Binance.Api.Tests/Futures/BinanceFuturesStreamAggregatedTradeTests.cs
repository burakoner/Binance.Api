using Binance.Api.Futures;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamAggregatedTradeTests
{
    [Fact]
    public void Topics_UseCanonicalNameAndPreserveDerivativeSymbols()
    {
        Assert.Equal(
            ["btcusdt@aggTrade", "btcusd_perp@aggTrade"],
            BinanceFuturesSocketClientUsd.AggregateTradeStreamTopics(["BTCUSDT", "BTCUSD_PERP"]));

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientUsd.AggregateTradeStreamTopics(null!));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.AggregateTradeStreamTopics([]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.AggregateTradeStreamTopics([" "]));
    }

    [Fact]
    public void Payload_MapsCompleteCurrentOfficialSchema()
    {
        const string payload = """
            {
              "e": "aggTrade",
              "E": 1753344000001,
              "s": "BTCUSD_PERP",
              "a": 9223372036854775806,
              "p": "11794.15000000",
              "q": "100.12500000",
              "nq": "99.87500000",
              "f": 9223372036854775804,
              "l": 9223372036854775805,
              "T": 1753344000002,
              "m": true,
              "st": 2
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesUsdtStreamAggregatedTrade>(payload);

        Assert.NotNull(update);
        Assert.Equal("aggTrade", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSD_PERP", update.Symbol);
        Assert.Equal(9_223_372_036_854_775_806L, update.Id);
        Assert.Equal(11_794.15m, update.Price);
        Assert.Equal(100.125m, update.Quantity);
        Assert.Equal(99.875m, update.NormalQuantity);
        Assert.Equal(9_223_372_036_854_775_804L, update.FirstTradeId);
        Assert.Equal(9_223_372_036_854_775_805L, update.LastTradeId);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.TradeTime);
        Assert.True(update.BuyerIsMaker);
        Assert.Equal(2, update.SymbolType);
        Assert.Null(typeof(BinanceFuturesUsdtStreamAggregatedTrade).GetProperty("Ignore"));
    }

    [Fact]
    public void CoinTopics_UseCanonicalNameAndPreserveDerivativeSymbols()
    {
        Assert.Equal(
            ["btcusd_perp@aggTrade", "btcusd_260925@aggTrade"],
            BinanceFuturesSocketClientCoin.AggregateTradeStreamTopics(["BTCUSD_PERP", "BTCUSD_260925"]));

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientCoin.AggregateTradeStreamTopics(null!));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.AggregateTradeStreamTopics([]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.AggregateTradeStreamTopics([" "]));
    }

    [Fact]
    public void CoinPayload_MapsCompleteCurrentOfficialSchema()
    {
        const string payload = """
            {
              "e": "aggTrade",
              "E": 1753344000001,
              "a": 9223372036854775806,
              "s": "BTCUSD_PERP",
              "p": "9643.50000000",
              "q": "2.12500000",
              "f": 9223372036854775804,
              "l": 9223372036854775805,
              "T": 1753344000002,
              "m": false,
              "st": 2
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesCoinStreamAggregatedTrade>(payload);

        Assert.NotNull(update);
        Assert.Equal("aggTrade", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSD_PERP", update.Symbol);
        Assert.Equal(9_223_372_036_854_775_806L, update.Id);
        Assert.Equal(9_643.5m, update.Price);
        Assert.Equal(2.125m, update.Quantity);
        Assert.Equal(9_223_372_036_854_775_804L, update.FirstTradeId);
        Assert.Equal(9_223_372_036_854_775_805L, update.LastTradeId);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.TradeTime);
        Assert.False(update.BuyerIsMaker);
        Assert.Equal(2, update.SymbolType);
        Assert.Null(typeof(BinanceFuturesCoinStreamAggregatedTrade).GetProperty("Ignore"));
        Assert.Null(typeof(BinanceFuturesCoinStreamAggregatedTrade).GetProperty("NormalQuantity"));
    }

    [Fact]
    public void PublicSurface_ExcludesUnsupportedRawTradeStreams()
    {
        var usdMethods = typeof(IBinanceFuturesSocketClientUsdStreamMarketData).GetMethods();
        var coinMethods = typeof(IBinanceFuturesSocketClientCoinStreamMarketData).GetMethods();

        Assert.DoesNotContain(usdMethods, method => method.Name == "SubscribeToTradesAsync");
        Assert.DoesNotContain(coinMethods, method => method.Name == "SubscribeToTradeUpdatesAsync");
        Assert.Contains(usdMethods, method => method.Name == "SubscribeToAggregatedTradesAsync");
        Assert.Contains(coinMethods, method => method.Name == "SubscribeToAggregatedTradeUpdatesAsync");
        Assert.Null(typeof(BinanceFuturesUsdtStreamAggregatedTrade).Assembly.GetType(
            "Binance.Api.Futures.BinanceFuturesStreamTrade"));
    }
}
