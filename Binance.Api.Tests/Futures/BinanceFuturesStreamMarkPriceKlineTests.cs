using ApiSharp.WebSocket;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamMarkPriceKlineTests
{
    [Fact]
    public void Topics_UseCurrentNameIntervalsAndConnectionLimit()
    {
        Assert.Equal(
            ["btcusd_perp@markPriceKline_1m", "btcusd_260925@markPriceKline_1m"],
            BinanceFuturesSocketClientCoin.MarkPriceKlineStreamTopics(
                ["BTCUSD_PERP", "BTCUSD_260925"],
                BinanceKlineInterval.OneMinute));

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientCoin.MarkPriceKlineStreamTopics(null!, BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.MarkPriceKlineStreamTopics([], BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.MarkPriceKlineStreamTopics([" "], BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientCoin.MarkPriceKlineStreamTopics(["BTCUSD_PERP"], BinanceKlineInterval.OneSecond));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientCoin.MarkPriceKlineStreamTopics(["BTCUSD_PERP"], (BinanceKlineInterval)12345));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.MarkPriceKlineStreamTopics(
                Enumerable.Repeat("BTCUSD_PERP", 1025),
                BinanceKlineInterval.OneMinute));
    }

    [Fact]
    public void Payload_PreservesOuterIdentityAndCompleteIgnoreAwareKline()
    {
        const string payload = """
            {
              "stream": "btcusd_perp@markPriceKline_1m",
              "data": {
                "e": "markPrice_kline",
                "E": 1786315119111,
                "ps": "BTCUSD",
                "k": {
                  "t": 1786315080000,
                  "T": 1786315139999,
                  "s": "BTCUSD_PERP",
                  "i": "1m",
                  "f": 9223372036854775700,
                  "L": 9223372036854775806,
                  "o": "64950.13313305",
                  "c": "64936.47411336",
                  "h": "64964.40000000",
                  "l": "64929.66671536",
                  "v": "1.12345678",
                  "n": 2147483648,
                  "x": false,
                  "q": "2.23456789",
                  "V": "3.34567891",
                  "Q": "4.45678912",
                  "B": "5.56789123"
                }
              }
            }
            """;

        var envelope = JsonConvert.DeserializeObject<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamMarkPriceKline>>(payload);

        Assert.NotNull(envelope);
        var update = envelope.Data;
        Assert.Equal("markPrice_kline", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_315_119_111).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSD", update.Pair);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_315_080_000).UtcDateTime, update.Kline.OpenTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_315_139_999).UtcDateTime, update.Kline.CloseTime);
        Assert.Equal("BTCUSD_PERP", update.Kline.Symbol);
        Assert.Equal(BinanceKlineInterval.OneMinute, update.Kline.Interval);
        Assert.Equal(9_223_372_036_854_775_700L, update.Kline.IgnoredValueF);
        Assert.Equal(9_223_372_036_854_775_806L, update.Kline.IgnoredValueL);
        Assert.Equal(64_950.13313305m, update.Kline.OpenPrice);
        Assert.Equal(64_936.47411336m, update.Kline.ClosePrice);
        Assert.Equal(64_964.4m, update.Kline.HighPrice);
        Assert.Equal(64_929.66671536m, update.Kline.LowPrice);
        Assert.Equal(1.12345678m, update.Kline.IgnoredValueV);
        Assert.Equal(2_147_483_648L, update.Kline.BasicDataCount);
        Assert.False(update.Kline.Final);
        Assert.Equal(2.23456789m, update.Kline.IgnoredValueQ);
        Assert.Equal(3.34567891m, update.Kline.IgnoredValueUpperV);
        Assert.Equal(4.45678912m, update.Kline.IgnoredValueUpperQ);
        Assert.Equal(5.56789123m, update.Kline.IgnoredValueB);
    }

    [Fact]
    public void PublicSurface_UsesDedicatedOuterAndIgnoreAwareInnerModels()
    {
        var callbacks = typeof(IBinanceFuturesSocketClientCoinStreamMarketData)
            .GetMethods()
            .Where(method => method.Name == nameof(IBinanceFuturesSocketClientCoinStreamMarketData.SubscribeToMarkPriceKlineUpdatesAsync))
            .SelectMany(method => method.GetParameters())
            .Where(parameter => parameter.ParameterType.IsGenericType
                && parameter.ParameterType.GetGenericTypeDefinition() == typeof(Action<>))
            .Select(parameter => parameter.ParameterType.GenericTypeArguments[0])
            .ToArray();

        Assert.Equal(2, callbacks.Length);
        Assert.All(callbacks, callback => Assert.Equal(
            typeof(WebSocketDataEvent<BinanceFuturesStreamMarkPriceKline>), callback));

        var model = typeof(BinanceFuturesStreamMarkPriceKlineData);
        Assert.Null(model.GetProperty("ContractVolume"));
        Assert.Null(model.GetProperty("BaseAssetVolume"));
        Assert.Null(model.GetProperty("QuoteAssetVolume"));
        Assert.Null(model.GetProperty("FirstTrade"));
        Assert.Null(model.GetProperty("LastTrade"));
    }
}
