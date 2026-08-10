using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientCoinMarketDataTests
{
    private static readonly Dictionary<BinanceKlineInterval, string> SupportedKlineIntervals = new()
    {
        [BinanceKlineInterval.OneMinute] = "1m",
        [BinanceKlineInterval.ThreeMinutes] = "3m",
        [BinanceKlineInterval.FiveMinutes] = "5m",
        [BinanceKlineInterval.FifteenMinutes] = "15m",
        [BinanceKlineInterval.ThirtyMinutes] = "30m",
        [BinanceKlineInterval.OneHour] = "1h",
        [BinanceKlineInterval.TwoHours] = "2h",
        [BinanceKlineInterval.FourHours] = "4h",
        [BinanceKlineInterval.SixHours] = "6h",
        [BinanceKlineInterval.EightHours] = "8h",
        [BinanceKlineInterval.TwelveHours] = "12h",
        [BinanceKlineInterval.OneDay] = "1d",
        [BinanceKlineInterval.ThreeDays] = "3d",
        [BinanceKlineInterval.OneWeek] = "1w",
        [BinanceKlineInterval.OneMonth] = "1M"
    };

    private static readonly Dictionary<BinanceFuturesContractType, string> StandardContractTypes = new()
    {
        [BinanceFuturesContractType.Perpetual] = "PERPETUAL",
        [BinanceFuturesContractType.CurrentQuarter] = "CURRENT_QUARTER",
        [BinanceFuturesContractType.NextQuarter] = "NEXT_QUARTER"
    };

    [Fact]
    public async Task KlineQueries_AllowEveryDocumentedInterval()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        foreach (var interval in SupportedKlineIntervals)
        {
            var result = await client.CoinFutures.GetKlinesAsync("BTCUSD_PERP", interval.Key);

            Assert.True(result.Success);
            Assert.Contains($"interval={interval.Value}", Uri.UnescapeDataString(handler.RequestUri!.Query));
        }

        Assert.Equal(SupportedKlineIntervals.Count, handler.RequestCount);
    }

    [Fact]
    public async Task KlineQueries_RejectUnsupportedIntervalsBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.GetKlinesAsync("BTCUSD_PERP", BinanceKlineInterval.OneSecond));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.GetContinuousContractKlinesAsync("BTCUSD", BinanceFuturesContractType.Perpetual, BinanceKlineInterval.OneSecond));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.GetIndexPriceKlinesAsync("BTCUSD", BinanceKlineInterval.OneSecond));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.GetMarkPriceKlinesAsync("BTCUSD_PERP", BinanceKlineInterval.OneSecond));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.GetPremiumIndexKlinesAsync("BTCUSD_PERP", BinanceKlineInterval.OneSecond));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.GetKlinesAsync("BTCUSD_PERP", (BinanceKlineInterval)12345));

        Assert.Equal(0, handler.RequestCount);
    }

    [Fact]
    public async Task KlineQueries_UseExactBoundaryWeights()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        var limiter = new RecordingRateLimiter();
        using var client = CreateClient(handler, limiter);
        (int? Limit, int Weight)[] cases =
        [
            (null, 5),
            (99, 1),
            (100, 2),
            (499, 2),
            (500, 5),
            (1000, 5),
            (1001, 10),
            (1500, 10)
        ];

        foreach (var item in cases)
        {
            var result = await client.CoinFutures.GetKlinesAsync(
                "BTCUSD_PERP",
                BinanceKlineInterval.OneMinute,
                limit: item.Limit);

            Assert.True(result.Success);
            Assert.Equal(item.Weight, limiter.Requests[^1].Weight);
        }
    }

    [Fact]
    public async Task KlineQueries_EnforceDocumentedDateRanges()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);
        var startTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var validEndTime = startTime.AddDays(200);
        var invalidEndTime = startTime.AddDays(200).AddMilliseconds(1);

        var boundary = await client.CoinFutures.GetKlinesAsync(
            "BTCUSD_PERP", BinanceKlineInterval.OneMinute, startTime, validEndTime);
        Assert.True(boundary.Success);

        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetKlinesAsync(
            "BTCUSD_PERP", BinanceKlineInterval.OneMinute, startTime, invalidEndTime));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetContinuousContractKlinesAsync(
            "BTCUSD", BinanceFuturesContractType.Perpetual, BinanceKlineInterval.OneMinute, startTime, invalidEndTime));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetIndexPriceKlinesAsync(
            "BTCUSD", BinanceKlineInterval.OneMinute, startTime, invalidEndTime));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetMarkPriceKlinesAsync(
            "BTCUSD_PERP", BinanceKlineInterval.OneMinute, startTime: startTime, endTime: invalidEndTime));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetKlinesAsync(
            "BTCUSD_PERP", BinanceKlineInterval.OneMinute, validEndTime, startTime));

        var premium = await client.CoinFutures.GetPremiumIndexKlinesAsync(
            "BTCUSD_PERP", BinanceKlineInterval.OneMinute, startTime, invalidEndTime);
        Assert.True(premium.Success);
        Assert.Equal(2, handler.RequestCount);
    }

    [Fact]
    public async Task ContinuousKlines_AllowOnlyDocumentedContractTypes()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        foreach (var contractType in StandardContractTypes)
        {
            var result = await client.CoinFutures.GetContinuousContractKlinesAsync(
                "BTCUSD",
                contractType.Key,
                BinanceKlineInterval.OneMinute);

            Assert.True(result.Success);
            Assert.Contains($"contractType={contractType.Value}", Uri.UnescapeDataString(handler.RequestUri!.Query));
        }

        Assert.Equal(StandardContractTypes.Count, handler.RequestCount);
    }

    [Fact]
    public async Task ContinuousKlines_RejectUndocumentedContractTypesBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);
        var unsupported = Enum.GetValues<BinanceFuturesContractType>()
            .Where(contractType => !StandardContractTypes.ContainsKey(contractType))
            .Append((BinanceFuturesContractType)255);

        foreach (var contractType in unsupported)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.CoinFutures.GetContinuousContractKlinesAsync(
                    "BTCUSD",
                    contractType,
                    BinanceKlineInterval.OneMinute));
        }

        Assert.Equal(0, handler.RequestCount);
    }

    [Fact]
    public async Task FuturesDataQueries_UseOperationSpecificContractTypes()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);
        var allContractTypes = StandardContractTypes
            .Append(new KeyValuePair<BinanceFuturesContractType, string>(BinanceFuturesContractType.All, "ALL"));

        foreach (var contractType in allContractTypes)
        {
            var openInterest = await client.CoinFutures.GetOpenInterestHistoryAsync(
                "BTCUSD",
                contractType.Key,
                BinancePeriodInterval.FiveMinutes);
            Assert.True(openInterest.Success);
            Assert.Equal("/futures/data/openInterestHist", handler.RequestUri!.AbsolutePath);
            Assert.Contains($"contractType={contractType.Value}", Uri.UnescapeDataString(handler.RequestUri.Query));

            var takerVolume = await client.CoinFutures.GetTakerBuySellVolumeRatioAsync(
                "BTCUSD",
                contractType.Key,
                BinancePeriodInterval.FiveMinutes);
            Assert.True(takerVolume.Success);
            Assert.Equal("/futures/data/takerBuySellVol", handler.RequestUri!.AbsolutePath);
            Assert.Contains($"contractType={contractType.Value}", Uri.UnescapeDataString(handler.RequestUri.Query));
        }

        foreach (var contractType in StandardContractTypes)
        {
            var basis = await client.CoinFutures.GetBasisAsync(
                "BTCUSD",
                contractType.Key,
                BinancePeriodInterval.FiveMinutes);
            Assert.True(basis.Success);
            Assert.Equal("/futures/data/basis", handler.RequestUri!.AbsolutePath);
            Assert.Contains($"contractType={contractType.Value}", Uri.UnescapeDataString(handler.RequestUri.Query));
        }

        Assert.Equal(11, handler.RequestCount);
    }

    [Fact]
    public async Task FuturesDataQueries_RejectUndocumentedContractTypesBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);
        var allContractTypes = StandardContractTypes.Keys.Append(BinanceFuturesContractType.All).ToHashSet();
        var allUnsupported = Enum.GetValues<BinanceFuturesContractType>()
            .Where(contractType => !allContractTypes.Contains(contractType))
            .Append((BinanceFuturesContractType)255);

        foreach (var contractType in allUnsupported)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.GetOpenInterestHistoryAsync(
                "BTCUSD", contractType, BinancePeriodInterval.FiveMinutes));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.GetTakerBuySellVolumeRatioAsync(
                "BTCUSD", contractType, BinancePeriodInterval.FiveMinutes));
        }

        var basisUnsupported = Enum.GetValues<BinanceFuturesContractType>()
            .Where(contractType => !StandardContractTypes.ContainsKey(contractType))
            .Append((BinanceFuturesContractType)255);
        foreach (var contractType in basisUnsupported)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.GetBasisAsync(
                "BTCUSD", contractType, BinancePeriodInterval.FiveMinutes));
        }

        Assert.Equal(0, handler.RequestCount);
    }

    [Fact]
    public async Task RequiredIdentifiersAndPeriods_RejectBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetKlinesAsync(
            " ", BinanceKlineInterval.OneMinute));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetContinuousContractKlinesAsync(
            " ", BinanceFuturesContractType.Perpetual, BinanceKlineInterval.OneMinute));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetIndexPriceKlinesAsync(
            " ", BinanceKlineInterval.OneMinute));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetMarkPriceKlinesAsync(
            " ", BinanceKlineInterval.OneMinute));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetPremiumIndexKlinesAsync(
            " ", BinanceKlineInterval.OneMinute));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetOpenInterestHistoryAsync(
            " ", BinanceFuturesContractType.All, BinancePeriodInterval.FiveMinutes));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetTakerBuySellVolumeRatioAsync(
            " ", BinanceFuturesContractType.All, BinancePeriodInterval.FiveMinutes));
        await Assert.ThrowsAsync<ArgumentException>(() => client.CoinFutures.GetBasisAsync(
            " ", BinanceFuturesContractType.Perpetual, BinancePeriodInterval.FiveMinutes));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.GetOpenInterestHistoryAsync(
            "BTCUSD", BinanceFuturesContractType.All, (BinancePeriodInterval)12345));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.GetTakerBuySellVolumeRatioAsync(
            "BTCUSD", BinanceFuturesContractType.All, (BinancePeriodInterval)12345));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.CoinFutures.GetBasisAsync(
            "BTCUSD", BinanceFuturesContractType.Perpetual, (BinancePeriodInterval)12345));

        Assert.Equal(0, handler.RequestCount);
    }

    [Fact]
    public async Task KlineResponses_RepresentCurrentTransportSpecificTuples()
    {
        var coinHandler = new RecordingHttpMessageHandler(
            """[[1591258320000,"9640.7","9642.4","9640.6","9642.0","206",1591258379999,"2.13660389",48,"119","1.23424865","0"]]""");
        using var coinClient = CreateClient(coinHandler);

        var coinResult = await coinClient.CoinFutures.GetKlinesAsync(
            "BTCUSD_PERP", BinanceKlineInterval.OneMinute);

        Assert.True(coinResult.Success);
        var coinKline = Assert.Single(coinResult.Data);
        Assert.Equal(206m, coinKline.Volume);
        Assert.Equal(2.13660389m, coinKline.BaseVolume);
        Assert.Equal(48, coinKline.TradeCount);
        Assert.Equal(119m, coinKline.TakerBuyVolume);
        Assert.Equal(1.23424865m, coinKline.TakerBuyBaseVolume);
        Assert.Null(typeof(BinanceFuturesCoinKline).GetProperty("TakerBuyQuoteVolume"));

        var priceHandler = new RecordingHttpMessageHandler(
            """[[1591256400000,"9653.6944","9653.6964","9651.3860","9651.5520","0",1591256459999,"0",60,"0","0","0"]]""");
        using var priceClient = CreateClient(priceHandler);

        var indexResult = await priceClient.CoinFutures.GetIndexPriceKlinesAsync(
            "BTCUSD", BinanceKlineInterval.OneMinute);

        Assert.True(indexResult.Success);
        var priceKline = Assert.Single(indexResult.Data);
        Assert.Equal(60, priceKline.DataCount);
        Assert.Equal(9653.6944m, priceKline.OpenPrice);
        Assert.Equal(9651.5520m, priceKline.ClosePrice);

        var premiumHandler = new RecordingHttpMessageHandler(
            """[[1691603820000,"-0.00042931","-0.00023641","-0.00059406","-0.00043659","0",1691603879999,"0",12,"0","0","0"]]""");
        using var premiumClient = CreateClient(premiumHandler);

        var premiumResult = await premiumClient.CoinFutures.GetPremiumIndexKlinesAsync(
            "BTCUSD_PERP", BinanceKlineInterval.OneMinute);

        Assert.True(premiumResult.Success);
        Assert.IsType<BinanceFuturesKline>(Assert.Single(premiumResult.Data));
        Assert.Null(typeof(BinanceFuturesKline).GetProperty("DataCount"));
    }

    [Fact]
    public async Task FuturesDataResponses_RepresentEveryCurrentField()
    {
        var openInterestHandler = new RecordingHttpMessageHandler(
            """[{"pair":"BTCUSD","contractType":"CURRENT_QUARTER","sumOpenInterest":"20403","sumOpenInterestValue":"176196512.23400000","timestamp":1591261042378}]""");
        using var openInterestClient = CreateClient(openInterestHandler);
        var openInterestResult = await openInterestClient.CoinFutures.GetOpenInterestHistoryAsync(
            "BTCUSD", BinanceFuturesContractType.All, BinancePeriodInterval.FiveMinutes);

        Assert.True(openInterestResult.Success);
        var openInterest = Assert.Single(openInterestResult.Data);
        Assert.Equal("BTCUSD", openInterest.Pair);
        Assert.Equal(BinanceFuturesContractType.CurrentQuarter, openInterest.ContractType);
        Assert.Equal(20403m, openInterest.SumOpenInterest);
        Assert.Equal(176196512.23400000m, openInterest.SumOpenInterestValue);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1591261042378).UtcDateTime, openInterest.Timestamp);

        var takerHandler = new RecordingHttpMessageHandler(
            """[{"pair":"BTCUSD","contractType":"CURRENT_QUARTER","takerBuyVol":"387","takerSellVol":"248","takerBuyVolValue":"2342.1220","takerSellVolValue":"4213.9800","timestamp":1591261042378}]""");
        using var takerClient = CreateClient(takerHandler);
        var takerResult = await takerClient.CoinFutures.GetTakerBuySellVolumeRatioAsync(
            "BTCUSD", BinanceFuturesContractType.All, BinancePeriodInterval.FiveMinutes);

        Assert.True(takerResult.Success);
        var taker = Assert.Single(takerResult.Data);
        Assert.Equal(387m, taker.TakerBuyVolume);
        Assert.Equal(248m, taker.TakerSellVolume);
        Assert.Equal(2342.1220m, taker.TakerBuyVolumeValue);
        Assert.Equal(4213.9800m, taker.TakerSellVolumeValue);

        var basisHandler = new RecordingHttpMessageHandler(
            """[{"indexPrice":"29269.93972727","contractType":"CURRENT_QUARTER","basisRate":"0.0024","futuresPrice":"29341.3","annualizedBasisRate":"0.0283","basis":"71.36027273","pair":"BTCUSD","timestamp":1653381600000}]""");
        using var basisClient = CreateClient(basisHandler);
        var basisResult = await basisClient.CoinFutures.GetBasisAsync(
            "BTCUSD", BinanceFuturesContractType.CurrentQuarter, BinancePeriodInterval.FiveMinutes);

        Assert.True(basisResult.Success);
        var basis = Assert.Single(basisResult.Data);
        Assert.Equal(29269.93972727m, basis.IndexPrice);
        Assert.Equal(BinanceFuturesContractType.CurrentQuarter, basis.ContractType);
        Assert.Equal(0.0024m, basis.BasisRate);
        Assert.Equal(29341.3m, basis.FuturesPrice);
        Assert.Equal(0.0283m, basis.AnnualizedBasisRate);
        Assert.Equal(71.36027273m, basis.Basis);
        Assert.Equal("BTCUSD", basis.Pair);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1653381600000).UtcDateTime, basis.Timestamp);
    }

    private static BinanceRestApiClient CreateClient(
        RecordingHttpMessageHandler handler,
        IRateLimiter? limiter = null)
    {
        var options = new BinanceRestApiClientOptions(new ApiCredentials("api-key", "api-secret"))
        {
            AutoTimestamp = false,
            HttpClient = new HttpClient(handler),
            RateLimiterEnabled = limiter != null
        };
#pragma warning disable CS0612
        options.RateLimiters = limiter == null ? [] : [limiter];
#pragma warning restore CS0612
        return new BinanceRestApiClient(options);
    }

    private sealed class RecordingRateLimiter : IRateLimiter
    {
        internal List<(string Endpoint, int Weight, bool Signed)> Requests { get; } = [];

        public Task<CallResult<int>> LimitRequestAsync(
            ILogger logger,
            string endpoint,
            HttpMethod method,
            bool signed,
            SensitiveString? apiKey,
            RateLimitingBehavior limitBehaviour,
            int requestWeight,
            CancellationToken ct)
        {
            Requests.Add((endpoint, requestWeight, signed));
            return Task.FromResult(new CallResult<int>(0));
        }
    }
}
