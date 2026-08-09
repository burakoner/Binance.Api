using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamKlineTests
{
    [Fact]
    public void UsdTopics_UseCurrentNamesIntervalsAndConnectionLimit()
    {
        Assert.Equal(
            ["btcusdt@kline_1m", "btcusdt@kline_1M", "ethusdt@kline_1m", "ethusdt@kline_1M"],
            BinanceFuturesSocketClientUsd.KlineStreamTopics(
                ["BTCUSDT", "ETHUSDT"],
                [BinanceKlineInterval.OneMinute, BinanceKlineInterval.OneMonth]));

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientUsd.KlineStreamTopics(null!, [BinanceKlineInterval.OneMinute]));
        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientUsd.KlineStreamTopics(["BTCUSDT"], null!));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.KlineStreamTopics([], [BinanceKlineInterval.OneMinute]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.KlineStreamTopics([" "], [BinanceKlineInterval.OneMinute]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.KlineStreamTopics(["BTCUSDT"], []));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientUsd.KlineStreamTopics(["BTCUSDT"], [BinanceKlineInterval.OneSecond]));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientUsd.KlineStreamTopics(["BTCUSDT"], [(BinanceKlineInterval)12345]));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.KlineStreamTopics(
                Enumerable.Repeat("BTCUSDT", 1025),
                [BinanceKlineInterval.OneMinute]));
    }

    [Fact]
    public void CoinTopics_UseCurrentNamesAndPreserveDatedSymbols()
    {
        Assert.Equal(
            ["btcusd_perp@kline_1m", "btcusd_260925@kline_1m"],
            BinanceFuturesSocketClientCoin.KlineStreamTopics(
                ["BTCUSD_PERP", "BTCUSD_260925"],
                [BinanceKlineInterval.OneMinute]));

        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientCoin.KlineStreamTopics([""], [BinanceKlineInterval.OneMinute]));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientCoin.KlineStreamTopics(["BTCUSD_PERP"], [BinanceKlineInterval.OneSecond]));
    }

    [Fact]
    public void UsdPayload_PreservesOuterEventAndUsdVolumeSemantics()
    {
        const string payload = """
            {
              "stream": "btcusdt@kline_1m",
              "data": {
                "e": "kline",
                "E": 1753344000002,
                "s": "BTCUSDT",
                "k": {
                  "t": 1753343940001,
                  "T": 1753343999999,
                  "s": "BTCUSDT",
                  "i": "1m",
                  "f": 9223372036854775700,
                  "L": 9223372036854775806,
                  "o": "95000.12345678",
                  "c": "95100.23456789",
                  "h": "95200.34567891",
                  "l": "94900.45678912",
                  "v": "132.56789123",
                  "n": 2147483648,
                  "x": false,
                  "q": "12585000.67891234",
                  "V": "65.78912345",
                  "Q": "6250000.89123456",
                  "B": "0"
                }
              }
            }
            """;

        var envelope = JsonConvert.DeserializeObject<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamKlineWrapper>>(payload);

        Assert.NotNull(envelope);
        var update = BinanceFuturesSocketClientUsd.StandardKline(envelope.Data);
        Assert.Equal("kline", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.EventTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_343_940_001).UtcDateTime, update.OpenTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_343_999_999).UtcDateTime, update.CloseTime);
        Assert.Equal("BTCUSDT", update.Symbol);
        Assert.Equal(BinanceKlineInterval.OneMinute, update.Interval);
        Assert.Equal(9_223_372_036_854_775_700L, update.FirstTrade);
        Assert.Equal(9_223_372_036_854_775_806L, update.LastTrade);
        Assert.Equal(95_000.12345678m, update.OpenPrice);
        Assert.Equal(95_100.23456789m, update.ClosePrice);
        Assert.Equal(95_200.34567891m, update.HighPrice);
        Assert.Equal(94_900.45678912m, update.LowPrice);
        Assert.Equal(132.56789123m, update.BaseAssetVolume);
        Assert.Equal(12_585_000.67891234m, update.QuoteAssetVolume);
        Assert.Equal(65.78912345m, update.TakerBuyBaseAssetVolume);
        Assert.Equal(6_250_000.89123456m, update.TakerBuyQuoteAssetVolume);
        Assert.Equal(2_147_483_648L, update.TradeCount);
        Assert.False(update.Final);
        Assert.Equal(0m, update.Ignore);
    }

    [Fact]
    public void CoinPayload_PreservesOuterEventAndCoinVolumeSemantics()
    {
        const string payload = """
            {
              "stream": "btcusd_260925@kline_1m",
              "data": {
                "e": "kline",
                "E": 1753344000002,
                "s": "BTCUSD_260925",
                "k": {
                  "t": 1753343940001,
                  "T": 1753343999999,
                  "s": "BTCUSD_260925",
                  "i": "1m",
                  "f": 9223372036854775700,
                  "L": 9223372036854775806,
                  "o": "95000.12345678",
                  "c": "95100.23456789",
                  "h": "95200.34567891",
                  "l": "94900.45678912",
                  "v": "156.12500000",
                  "n": 2147483648,
                  "x": true,
                  "q": "1.23456789",
                  "V": "78.06250000",
                  "Q": "0.61728394",
                  "B": "0"
                }
              }
            }
            """;

        var envelope = JsonConvert.DeserializeObject<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamCoinKlineWrapper>>(payload);

        Assert.NotNull(envelope);
        var update = BinanceFuturesSocketClientCoin.StandardKline(envelope.Data);
        Assert.Equal("kline", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_753_344_000_002).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSD_260925", update.Symbol);
        Assert.Equal(BinanceKlineInterval.OneMinute, update.Interval);
        Assert.Equal(156.125m, update.ContractVolume);
        Assert.Equal(1.23456789m, update.BaseAssetVolume);
        Assert.Equal(78.0625m, update.TakerBuyContractVolume);
        Assert.Equal(0.61728394m, update.TakerBuyBaseAssetVolume);
        Assert.Equal(2_147_483_648L, update.TradeCount);
        Assert.True(update.Final);
        Assert.Equal(0m, update.Ignore);
    }

    [Fact]
    public void PublicSurface_RemovesUndocumentedPremiumSwitchAndMisleadingVolumeNames()
    {
        var methods = typeof(IBinanceFuturesSocketClientUsdStreamMarketData)
            .GetMethods()
            .Where(method => method.Name == nameof(IBinanceFuturesSocketClientUsdStreamMarketData.SubscribeToKlinesAsync))
            .ToArray();

        Assert.Equal(4, methods.Length);
        Assert.All(methods, method =>
            Assert.DoesNotContain(method.GetParameters(), parameter => parameter.ParameterType == typeof(bool)));

        var usdModel = typeof(BinanceFuturesStreamKline);
        Assert.Null(usdModel.GetProperty("Volume"));
        Assert.Null(usdModel.GetProperty("QuoteVolume"));
        Assert.Null(usdModel.GetProperty("TakerBuyBaseVolume"));
        Assert.Null(usdModel.GetProperty("TakerBuyQuoteVolume"));

        var coinModel = typeof(BinanceFuturesStreamCoinKline);
        Assert.Null(coinModel.GetProperty("Volume"));
        Assert.Null(coinModel.GetProperty("QuoteVolume"));
        Assert.Null(coinModel.GetProperty("TakerBuyBaseVolume"));
        Assert.Null(coinModel.GetProperty("TakerBuyQuoteVolume"));
    }
}
