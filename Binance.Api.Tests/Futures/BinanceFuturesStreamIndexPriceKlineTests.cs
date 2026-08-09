using ApiSharp.WebSocket;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamIndexPriceKlineTests
{
    [Fact]
    public void Topics_UseCurrentNameIntervalsAndConnectionLimit()
    {
        Assert.Equal(
            ["btcusd@indexPriceKline_1m"],
            BinanceFuturesSocketClientCoin.IndexPriceKlineStreamTopics(
                ["BTCUSD"],
                BinanceKlineInterval.OneMinute));
        Assert.Equal(
            ["ethusd@indexPriceKline_1M"],
            BinanceFuturesSocketClientCoin.IndexPriceKlineStreamTopics(
                ["ETHUSD"],
                BinanceKlineInterval.OneMonth));

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientCoin.IndexPriceKlineStreamTopics(null!, BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.IndexPriceKlineStreamTopics([], BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.IndexPriceKlineStreamTopics([" "], BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientCoin.IndexPriceKlineStreamTopics(["BTCUSD"], BinanceKlineInterval.OneSecond));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientCoin.IndexPriceKlineStreamTopics(["BTCUSD"], (BinanceKlineInterval)12345));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.IndexPriceKlineStreamTopics(
                Enumerable.Repeat("BTCUSD", 1025),
                BinanceKlineInterval.OneMinute));
    }

    [Fact]
    public void Payload_PreservesOuterPairAndCompleteDocumentedKline()
    {
        const string payload = """
            {
              "stream": "btcusd@indexPriceKline_1m",
              "data": {
                "e": "indexPrice_kline",
                "E": 1786315604076,
                "ps": "BTCUSD",
                "k": {
                  "t": 1786315560000,
                  "T": 1786315619999,
                  "s": "0",
                  "i": "1m",
                  "f": 9223372036854775700,
                  "L": 9223372036854775806,
                  "o": "64955.40202891",
                  "c": "64968.13334200",
                  "h": "64971.09528952",
                  "l": "64955.21601531",
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

        var envelope = JsonConvert.DeserializeObject<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamIndexPriceKline>>(payload);

        Assert.NotNull(envelope);
        var update = envelope.Data;
        Assert.Equal("indexPrice_kline", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_315_604_076).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSD", update.Pair);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_315_560_000).UtcDateTime, update.Kline.OpenTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_315_619_999).UtcDateTime, update.Kline.TransactionTime);
        Assert.Equal("0", update.Kline.Symbol);
        Assert.Equal(BinanceKlineInterval.OneMinute, update.Kline.Interval);
        Assert.Equal(9_223_372_036_854_775_700L, update.Kline.IgnoredValueF);
        Assert.Equal(9_223_372_036_854_775_806L, update.Kline.IgnoredValueL);
        Assert.Equal(64_955.40202891m, update.Kline.OpenPrice);
        Assert.Equal(64_968.133342m, update.Kline.ClosePrice);
        Assert.Equal(64_971.09528952m, update.Kline.HighPrice);
        Assert.Equal(64_955.21601531m, update.Kline.LowPrice);
        Assert.Equal(1.12345678m, update.Kline.Volume);
        Assert.Equal(2_147_483_648L, update.Kline.TradeCount);
        Assert.False(update.Kline.Final);
        Assert.Equal(2.23456789m, update.Kline.QuoteVolume);
        Assert.Equal(3.34567891m, update.Kline.TakerBuyVolume);
        Assert.Equal(4.45678912m, update.Kline.LastTradeVolume);
        Assert.Equal(5.56789123m, update.Kline.BestBidQuantity);
    }

    [Fact]
    public void PublicSurface_UsesDedicatedOuterModelWithoutInventedCoinVolumeUnits()
    {
        var callbacks = typeof(IBinanceFuturesSocketClientCoinStreamMarketData)
            .GetMethods()
            .Where(method => method.Name == nameof(IBinanceFuturesSocketClientCoinStreamMarketData.SubscribeToIndexKlineUpdatesAsync))
            .SelectMany(method => method.GetParameters())
            .Where(parameter => parameter.ParameterType.IsGenericType
                && parameter.ParameterType.GetGenericTypeDefinition() == typeof(Action<>))
            .Select(parameter => parameter.ParameterType.GenericTypeArguments[0])
            .ToArray();

        Assert.Equal(2, callbacks.Length);
        Assert.All(callbacks, callback => Assert.Equal(
            typeof(WebSocketDataEvent<BinanceFuturesStreamIndexPriceKline>), callback));

        var model = typeof(BinanceFuturesStreamIndexPriceKlineData);
        Assert.Null(model.GetProperty("ContractVolume"));
        Assert.Null(model.GetProperty("BaseAssetVolume"));
        Assert.Null(model.GetProperty("TakerBuyContractVolume"));
        Assert.Null(model.GetProperty("TakerBuyBaseAssetVolume"));
        Assert.Null(model.Assembly.GetType("Binance.Api.Futures.BinanceFuturesStreamIndexKline"));
    }
}
