using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamLiquidationTests
{
    [Fact]
    public void Topics_UseCanonicalNamesAndRejectInvalidSymbols()
    {
        Assert.Equal(
            ["btcusdt@forceOrder", "ethusdt@forceOrder"],
            BinanceFuturesSocketClientUsd.LiquidationStreamTopics(["BTCUSDT", "ETHUSDT"]));
        Assert.Equal(
            ["btcusd_perp@forceOrder", "btcusd_260925@forceOrder"],
            BinanceFuturesSocketClientCoin.LiquidationStreamTopics(["BTCUSD_PERP", "BTCUSD_260925"]));
        Assert.Equal("!forceOrder@arr", BinanceFuturesSocketClientUsd.LiquidationAllMarketStreamTopic);
        Assert.Equal("!forceOrder@arr", BinanceFuturesSocketClientCoin.LiquidationAllMarketStreamTopic);

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientUsd.LiquidationStreamTopics(null!));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.LiquidationStreamTopics([]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.LiquidationStreamTopics([" "]));
        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientCoin.LiquidationStreamTopics(null!));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.LiquidationStreamTopics([]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.LiquidationStreamTopics([" "]));
    }

    [Fact]
    public void UsdIndividualPayload_MapsCompleteCurrentSchema()
    {
        const string payload = """
            {
              "e": "forceOrder",
              "E": 1753344000002,
              "o": {
                "s": "BTCUSDT",
                "S": "SELL",
                "o": "LIMIT",
                "f": "IOC",
                "q": "0.01400001",
                "p": "9910.12345678",
                "ap": "9910.12345670",
                "X": "FILLED",
                "l": "0.01400000",
                "z": "0.01400001",
                "T": 1753344000001
              }
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamLiquidation>(payload);

        Assert.NotNull(update);
        Assert.Equal("forceOrder", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.EventTime);
        Assert.Null(update.Pair);
        Assert.Null(update.SymbolType);
        Assert.Equal("BTCUSDT", update.Order.Symbol);
        Assert.Null(update.Order.Pair);
        Assert.Equal(BinanceOrderSide.Sell, update.Order.Side);
        Assert.Equal(BinanceFuturesOrderType.Limit, update.Order.Type);
        Assert.Equal(BinanceTimeInForce.ImmediateOrCancel, update.Order.TimeInForce);
        Assert.Equal(0.01400001m, update.Order.Quantity);
        Assert.Equal(9_910.12345678m, update.Order.Price);
        Assert.Equal(9_910.12345670m, update.Order.AveragePrice);
        Assert.Equal(BinanceOrderStatus.Filled, update.Order.Status);
        Assert.Equal(0.014m, update.Order.LastQuantityFilled);
        Assert.Equal(0.01400001m, update.Order.QuantityFilled);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_001).UtcDateTime, update.Order.Timestamp);
    }

    [Fact]
    public void CoinIndividualPayload_MapsPairInsideOrder()
    {
        const string payload = """
            {
              "e": "forceOrder",
              "E": 1753344000002,
              "o": {
                "s": "BTCUSD_260925",
                "ps": "BTCUSD",
                "S": "SELL",
                "o": "LIMIT",
                "f": "IOC",
                "q": "7",
                "p": "9425.50000001",
                "ap": "9496.50000002",
                "X": "FILLED",
                "l": "3",
                "z": "7",
                "T": 1753344000001
              }
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamLiquidation>(payload);

        Assert.NotNull(update);
        Assert.Equal("forceOrder", update.Event);
        Assert.Null(update.Pair);
        Assert.Null(update.SymbolType);
        Assert.Equal("BTCUSD_260925", update.Order.Symbol);
        Assert.Equal("BTCUSD", update.Order.Pair);
        Assert.Equal(BinanceOrderSide.Sell, update.Order.Side);
        Assert.Equal(BinanceFuturesOrderType.Limit, update.Order.Type);
        Assert.Equal(BinanceTimeInForce.ImmediateOrCancel, update.Order.TimeInForce);
        Assert.Equal(7m, update.Order.Quantity);
        Assert.Equal(9_425.50000001m, update.Order.Price);
        Assert.Equal(9_496.50000002m, update.Order.AveragePrice);
        Assert.Equal(BinanceOrderStatus.Filled, update.Order.Status);
        Assert.Equal(3m, update.Order.LastQuantityFilled);
        Assert.Equal(7m, update.Order.QuantityFilled);
    }

    [Fact]
    public void UsdAllMarketPayload_MapsPairAndSymbolTypeOutsideOrder()
    {
        const string payload = """
            {
              "e": "forceOrder",
              "E": 1753344000002,
              "o": {
                "s": "ETHUSDT",
                "S": "SELL",
                "o": "LIMIT",
                "f": "IOC",
                "q": "1.25",
                "p": "3000.50",
                "ap": "3000.25",
                "X": "FILLED",
                "l": "1.25",
                "z": "1.25",
                "T": 1753344000001
              },
              "ps": "ETHUSDT",
              "st": 1
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamLiquidation>(payload);

        Assert.NotNull(update);
        Assert.Equal("ETHUSDT", update.Pair);
        Assert.Equal(1, update.SymbolType);
        Assert.Equal("ETHUSDT", update.Order.Symbol);
        Assert.Null(update.Order.Pair);
    }

    [Fact]
    public void CoinAllMarketPayload_MapsPairInsideOrderAndSymbolTypeOutside()
    {
        const string payload = """
            {
              "e": "forceOrder",
              "E": 1753344000002,
              "o": {
                "s": "ETHUSD_260925",
                "ps": "ETHUSD",
                "S": "SELL",
                "o": "LIMIT",
                "f": "IOC",
                "q": "5",
                "p": "3000.50",
                "ap": "3000.25",
                "X": "FILLED",
                "l": "5",
                "z": "5",
                "T": 1753344000001
              },
              "st": 2
            }
            """;

        var update = JsonConvert.DeserializeObject<BinanceFuturesStreamLiquidation>(payload);

        Assert.NotNull(update);
        Assert.Null(update.Pair);
        Assert.Equal(2, update.SymbolType);
        Assert.Equal("ETHUSD_260925", update.Order.Symbol);
        Assert.Equal("ETHUSD", update.Order.Pair);
    }
}
