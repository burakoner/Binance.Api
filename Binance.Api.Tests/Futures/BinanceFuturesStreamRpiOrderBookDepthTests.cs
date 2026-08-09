using Binance.Api.Futures;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamRpiOrderBookDepthTests
{
    [Fact]
    public void Topics_UseCanonicalFixedSpeedAndRejectInvalidSymbols()
    {
        Assert.Equal(
            ["btcusdt@rpiDepth@500ms", "ethusdt@rpiDepth@500ms"],
            BinanceFuturesSocketClientUsd.RpiDepthStreamTopics(["BTCUSDT", "ETHUSDT"]));

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientUsd.RpiDepthStreamTopics(null!));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.RpiDepthStreamTopics([]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.RpiDepthStreamTopics([" "]));
    }

    [Fact]
    public void Payload_MapsCompleteCurrentSchemaIncludingRpiMetadata()
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
              "a": [["9911.87654321", "0"]],
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
        Assert.Equal(0m, update.Asks[0].Quantity);
    }
}
