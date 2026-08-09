using Binance.Api.Futures;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamOrderBookDepthTests
{
    [Fact]
    public void UsdTopics_UseCanonicalLevelsAndUpdateSpeeds()
    {
        Assert.Equal(
            ["btcusdt@depth5", "ethusdt@depth5"],
            BinanceFuturesSocketClientUsd.PartialDepthStreamTopics(["BTCUSDT", "ETHUSDT"], 5, null));
        Assert.Equal(
            ["btcusdt@depth10@100ms"],
            BinanceFuturesSocketClientUsd.PartialDepthStreamTopics(["BTCUSDT"], 10, 100));
        Assert.Equal(
            ["btcusdt@depth20@500ms"],
            BinanceFuturesSocketClientUsd.PartialDepthStreamTopics(["BTCUSDT"], 20, 500));
        Assert.Equal(
            ["btcusdt@depth", "ethusdt@depth"],
            BinanceFuturesSocketClientUsd.DiffDepthStreamTopics(["BTCUSDT", "ETHUSDT"], null));
        Assert.Equal(
            ["btcusdt@depth@100ms"],
            BinanceFuturesSocketClientUsd.DiffDepthStreamTopics(["BTCUSDT"], 100));
        Assert.Equal(
            ["btcusdt@depth@500ms"],
            BinanceFuturesSocketClientUsd.DiffDepthStreamTopics(["BTCUSDT"], 500));

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientUsd.PartialDepthStreamTopics(null!, 5, null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.PartialDepthStreamTopics([], 5, null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.DiffDepthStreamTopics([" "], null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.PartialDepthStreamTopics(["BTCUSDT"], 15, null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.DiffDepthStreamTopics(["BTCUSDT"], 250));
    }

    [Fact]
    public void CoinTopics_UseCanonicalLevelsAndUpdateSpeeds()
    {
        Assert.Equal(
            ["btcusd_perp@depth5", "btcusd_260925@depth5"],
            BinanceFuturesSocketClientCoin.PartialDepthStreamTopics(["BTCUSD_PERP", "BTCUSD_260925"], 5, null));
        Assert.Equal(
            ["btcusd_perp@depth10@100ms"],
            BinanceFuturesSocketClientCoin.PartialDepthStreamTopics(["BTCUSD_PERP"], 10, 100));
        Assert.Equal(
            ["btcusd_260925@depth20@500ms"],
            BinanceFuturesSocketClientCoin.PartialDepthStreamTopics(["BTCUSD_260925"], 20, 500));
        Assert.Equal(
            ["btcusd_perp@depth", "btcusd_260925@depth"],
            BinanceFuturesSocketClientCoin.DiffDepthStreamTopics(["BTCUSD_PERP", "BTCUSD_260925"], null));
        Assert.Equal(
            ["btcusd_perp@depth@100ms"],
            BinanceFuturesSocketClientCoin.DiffDepthStreamTopics(["BTCUSD_PERP"], 100));
        Assert.Equal(
            ["btcusd_260925@depth@500ms"],
            BinanceFuturesSocketClientCoin.DiffDepthStreamTopics(["BTCUSD_260925"], 500));

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientCoin.DiffDepthStreamTopics(null!, null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.DiffDepthStreamTopics([], null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.PartialDepthStreamTopics([""], 5, null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.PartialDepthStreamTopics(["BTCUSD_PERP"], 50, null));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.PartialDepthStreamTopics(["BTCUSD_PERP"], 5, 250));
    }

    [Fact]
    public void UsdPayload_MapsCompleteCurrentSchema()
    {
        const string payload = """
            {
              "e": "depthUpdate",
              "E": 1753344000002,
              "T": 1753344000001,
              "s": "BTCUSDT",
              "U": 9223372036854775797,
              "u": 9223372036854775806,
              "pu": 9223372036854775796,
              "b": [["9910.12345678", "0.01400001"]],
              "a": [["9911.87654321", "2.50000000"]],
              "ps": "BTCUSDT",
              "st": 1
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamOrderBookDepth>(payload);

        Assert.NotNull(update);
        Assert.Equal("depthUpdate", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.EventTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.TransactionTime);
        Assert.Equal("BTCUSDT", update.Symbol);
        Assert.Equal("BTCUSDT", update.Pair);
        Assert.Equal(1, update.SymbolType);
        Assert.Equal(9_223_372_036_854_775_797L, update.FirstUpdateId);
        Assert.Equal(9_223_372_036_854_775_806L, update.LastUpdateId);
        Assert.Equal(9_223_372_036_854_775_796L, update.LastUpdateIdStream);
        Assert.Single(update.Bids);
        Assert.Equal(9_910.12345678m, update.Bids[0].Price);
        Assert.Equal(0.01400001m, update.Bids[0].Quantity);
        Assert.Single(update.Asks);
        Assert.Equal(9_911.87654321m, update.Asks[0].Price);
        Assert.Equal(2.5m, update.Asks[0].Quantity);
    }

    [Fact]
    public void CoinPayload_MapsCompleteCurrentSchema()
    {
        const string payload = """
            {
              "e": "depthUpdate",
              "E": 1753344000002,
              "T": 1753344000001,
              "s": "BTCUSD_260925",
              "ps": "BTCUSD",
              "U": 17285681,
              "u": 17285702,
              "pu": 17285675,
              "b": [["9517.60000001", "10"]],
              "a": [["9518.50000002", "45"]],
              "st": 2
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamOrderBookDepth>(payload);

        Assert.NotNull(update);
        Assert.Equal("depthUpdate", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.EventTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.TransactionTime);
        Assert.Equal("BTCUSD_260925", update.Symbol);
        Assert.Equal("BTCUSD", update.Pair);
        Assert.Equal(2, update.SymbolType);
        Assert.Equal(17_285_681L, update.FirstUpdateId);
        Assert.Equal(17_285_702L, update.LastUpdateId);
        Assert.Equal(17_285_675L, update.LastUpdateIdStream);
        Assert.Single(update.Bids);
        Assert.Equal(9_517.60000001m, update.Bids[0].Price);
        Assert.Equal(10m, update.Bids[0].Quantity);
        Assert.Single(update.Asks);
        Assert.Equal(9_518.50000002m, update.Asks[0].Price);
        Assert.Equal(45m, update.Asks[0].Quantity);
    }
}
