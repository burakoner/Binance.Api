using ApiSharp.WebSocket;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamContinuousKlineTests
{
    [Fact]
    public void UsdTopics_UseCurrentContractTypesIntervalsAndConnectionLimit()
    {
        Assert.Equal(
            ["btcusdt_perpetual@continuousKline_1s", "ethusdt_perpetual@continuousKline_1s"],
            BinanceFuturesSocketClientUsd.ContinuousKlineStreamTopics(
                ["BTCUSDT", "ETHUSDT"],
                BinanceFuturesContractType.Perpetual,
                BinanceKlineInterval.OneSecond));
        Assert.Equal(
            ["aaplusdt_tradifi_perpetual@continuousKline_1m"],
            BinanceFuturesSocketClientUsd.ContinuousKlineStreamTopics(
                ["AAPLUSDT"],
                BinanceFuturesContractType.TradFiPerpetual,
                BinanceKlineInterval.OneMinute));

        Assert.Throws<ArgumentNullException>(() =>
            BinanceFuturesSocketClientUsd.ContinuousKlineStreamTopics(
                null!, BinanceFuturesContractType.Perpetual, BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.ContinuousKlineStreamTopics(
                [], BinanceFuturesContractType.Perpetual, BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.ContinuousKlineStreamTopics(
                [" "], BinanceFuturesContractType.Perpetual, BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientUsd.ContinuousKlineStreamTopics(
                ["BTCUSDT"], BinanceFuturesContractType.CurrentMonth, BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientUsd.ContinuousKlineStreamTopics(
                ["BTCUSDT"], (BinanceFuturesContractType)255, BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientUsd.ContinuousKlineStreamTopics(
                ["BTCUSDT"], BinanceFuturesContractType.Perpetual, (BinanceKlineInterval)12345));
        Assert.Throws<ArgumentException>(() =>
            BinanceFuturesSocketClientUsd.ContinuousKlineStreamTopics(
                Enumerable.Repeat("BTCUSDT", 1025),
                BinanceFuturesContractType.Perpetual,
                BinanceKlineInterval.OneMinute));
    }

    [Fact]
    public void CoinTopics_UseCoinContractTypesAndRejectOneSecond()
    {
        Assert.Equal(
            ["btcusd_current_quarter@continuousKline_1m"],
            BinanceFuturesSocketClientCoin.ContinuousKlineStreamTopics(
                ["BTCUSD"],
                BinanceFuturesContractType.CurrentQuarter,
                BinanceKlineInterval.OneMinute));

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientCoin.ContinuousKlineStreamTopics(
                ["BTCUSD"], BinanceFuturesContractType.TradFiPerpetual, BinanceKlineInterval.OneMinute));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFuturesSocketClientCoin.ContinuousKlineStreamTopics(
                ["BTCUSD"], BinanceFuturesContractType.Perpetual, BinanceKlineInterval.OneSecond));
    }

    [Fact]
    public void UsdPayload_PreservesOuterIdentityAndUsdVolumeSemantics()
    {
        const string payload = """
            {
              "stream": "aaplusdt_tradifi_perpetual@continuousKline_1s",
              "data": {
                "e": "continuous_kline",
                "E": 1786313973731,
                "ps": "AAPLUSDT",
                "ct": "TRADIFI_PERPETUAL",
                "k": {
                  "t": 1786313973000,
                  "T": 1786313973999,
                  "i": "1s",
                  "f": 9223372036854775700,
                  "L": 9223372036854775806,
                  "o": "65283.90123456",
                  "c": "65299.61234567",
                  "h": "65300.72345678",
                  "l": "65260.83456789",
                  "v": "73.59512345",
                  "n": 2147483648,
                  "x": false,
                  "q": "4804057.87920123",
                  "V": "38.66323456",
                  "Q": "2523885.45260345",
                  "B": "0"
                }
              }
            }
            """;

        var envelope = JsonConvert.DeserializeObject<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamContinuousKline>>(payload);

        Assert.NotNull(envelope);
        var update = envelope.Data;
        Assert.Equal("continuous_kline", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_313_973_731).UtcDateTime, update.EventTime);
        Assert.Equal("AAPLUSDT", update.Pair);
        Assert.Equal(BinanceFuturesContractType.TradFiPerpetual, update.ContractType);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_313_973_000).UtcDateTime, update.Kline.OpenTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_313_973_999).UtcDateTime, update.Kline.CloseTime);
        Assert.Equal(BinanceKlineInterval.OneSecond, update.Kline.Interval);
        Assert.Equal(9_223_372_036_854_775_700L, update.Kline.FirstUpdateId);
        Assert.Equal(9_223_372_036_854_775_806L, update.Kline.LastUpdateId);
        Assert.Equal(65_283.90123456m, update.Kline.OpenPrice);
        Assert.Equal(65_299.61234567m, update.Kline.ClosePrice);
        Assert.Equal(65_300.72345678m, update.Kline.HighPrice);
        Assert.Equal(65_260.83456789m, update.Kline.LowPrice);
        Assert.Equal(73.59512345m, update.Kline.BaseAssetVolume);
        Assert.Equal(4_804_057.87920123m, update.Kline.QuoteAssetVolume);
        Assert.Equal(38.66323456m, update.Kline.TakerBuyBaseAssetVolume);
        Assert.Equal(2_523_885.45260345m, update.Kline.TakerBuyQuoteAssetVolume);
        Assert.Equal(2_147_483_648L, update.Kline.TradeCount);
        Assert.False(update.Kline.Final);
        Assert.Equal(0m, update.Kline.Ignore);
    }

    [Fact]
    public void CoinPayload_PreservesOuterIdentityAndCoinVolumeSemantics()
    {
        const string payload = """
            {
              "stream": "btcusd_perpetual@continuousKline_1m",
              "data": {
                "e": "continuous_kline",
                "E": 1786313976464,
                "ps": "BTCUSD",
                "ct": "PERPETUAL",
                "k": {
                  "t": 1786313940000,
                  "T": 1786313999999,
                  "i": "1m",
                  "f": 9223372036854775700,
                  "L": 9223372036854775806,
                  "o": "65233.81234567",
                  "c": "65246.32345678",
                  "h": "65247.43456789",
                  "l": "65214.24567891",
                  "v": "3261.12500000",
                  "n": 2147483648,
                  "x": true,
                  "q": "4.99970797",
                  "V": "620.06250000",
                  "Q": "0.95049566",
                  "B": "0"
                }
              }
            }
            """;

        var envelope = JsonConvert.DeserializeObject<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamCoinContinuousKline>>(payload);

        Assert.NotNull(envelope);
        var update = envelope.Data;
        Assert.Equal("continuous_kline", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_313_976_464).UtcDateTime, update.EventTime);
        Assert.Equal("BTCUSD", update.Pair);
        Assert.Equal(BinanceFuturesContractType.Perpetual, update.ContractType);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_313_940_000).UtcDateTime, update.Kline.OpenTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_786_313_999_999).UtcDateTime, update.Kline.CloseTime);
        Assert.Equal(BinanceKlineInterval.OneMinute, update.Kline.Interval);
        Assert.Equal(9_223_372_036_854_775_700L, update.Kline.FirstUpdateId);
        Assert.Equal(9_223_372_036_854_775_806L, update.Kline.LastUpdateId);
        Assert.Equal(65_233.81234567m, update.Kline.OpenPrice);
        Assert.Equal(65_246.32345678m, update.Kline.ClosePrice);
        Assert.Equal(65_247.43456789m, update.Kline.HighPrice);
        Assert.Equal(65_214.24567891m, update.Kline.LowPrice);
        Assert.Equal(3_261.125m, update.Kline.ContractVolume);
        Assert.Equal(4.99970797m, update.Kline.BaseAssetVolume);
        Assert.Equal(620.0625m, update.Kline.TakerBuyContractVolume);
        Assert.Equal(0.95049566m, update.Kline.TakerBuyBaseAssetVolume);
        Assert.Equal(2_147_483_648L, update.Kline.TradeCount);
        Assert.True(update.Kline.Final);
        Assert.Equal(0m, update.Kline.Ignore);
    }

    [Fact]
    public void PublicSurface_UsesProductSpecificOuterAndInnerModels()
    {
        var usdCallbacks = typeof(IBinanceFuturesSocketClientUsdStreamMarketData)
            .GetMethods()
            .Where(method => method.Name == nameof(IBinanceFuturesSocketClientUsdStreamMarketData.SubscribeToContinuousContractKlinesAsync))
            .SelectMany(method => method.GetParameters())
            .Where(parameter => parameter.ParameterType.IsGenericType
                && parameter.ParameterType.GetGenericTypeDefinition() == typeof(Action<>))
            .Select(parameter => parameter.ParameterType.GenericTypeArguments[0])
            .ToArray();
        var coinCallbacks = typeof(IBinanceFuturesSocketClientCoinStreamMarketData)
            .GetMethods()
            .Where(method => method.Name == nameof(IBinanceFuturesSocketClientCoinStreamMarketData.SubscribeToContinuousContractKlineUpdatesAsync))
            .SelectMany(method => method.GetParameters())
            .Where(parameter => parameter.ParameterType.IsGenericType
                && parameter.ParameterType.GetGenericTypeDefinition() == typeof(Action<>))
            .Select(parameter => parameter.ParameterType.GenericTypeArguments[0])
            .ToArray();

        Assert.Equal(2, usdCallbacks.Length);
        Assert.All(usdCallbacks, callback => Assert.Equal(
            typeof(WebSocketDataEvent<BinanceFuturesStreamContinuousKline>), callback));
        Assert.Equal(2, coinCallbacks.Length);
        Assert.All(coinCallbacks, callback => Assert.Equal(
            typeof(WebSocketDataEvent<BinanceFuturesStreamCoinContinuousKline>), callback));

        Assert.Null(typeof(BinanceFuturesStreamContinuousKlineData).GetProperty("Symbol"));
        Assert.Null(typeof(BinanceFuturesStreamCoinContinuousKlineData).GetProperty("Symbol"));
        Assert.Null(typeof(BinanceFuturesStreamCoinContinuousKlineData).GetProperty("QuoteAssetVolume"));
    }
}
