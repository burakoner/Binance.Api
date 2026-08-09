using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdMarketDataTests
{
    [Fact]
    public async Task GetRpiOrderBookAsync_UsesCurrentPublicContractAndDeserializesResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "lastUpdateId": 1027024,
              "E": 1589436922972,
              "T": 1589436922959,
              "bids": [["4.00000000", "431.00000000"]],
              "asks": [["4.00000200", "12.00000000"]]
            }
            """);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, limiter);

        var result = await client.UsdFutures.GetRpiOrderBookAsync("BTCUSDT", 1000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/rpiDepth", handler.RequestUri!.AbsolutePath);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("limit=1000", query);
        Assert.DoesNotContain("timestamp=", query);
        Assert.DoesNotContain("signature=", query);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/fapi/v1/rpiDepth" && item.Weight == 20 && !item.Signed);
        Assert.Equal(1027024, result.Data.LastUpdateId);
        Assert.Equal(1589436922972, result.Data.MessageOutputTime);
        Assert.Equal(1589436922959, result.Data.TransactionTime);
        Assert.Equal(4.00000000m, Assert.Single(result.Data.Bids).Price);
        Assert.Equal(431.00000000m, Assert.Single(result.Data.Bids).Quantity);
        Assert.Equal(4.00000200m, Assert.Single(result.Data.Asks).Price);
        Assert.Equal(12.00000000m, Assert.Single(result.Data.Asks).Quantity);
    }

    [Theory]
    [InlineData(999)]
    [InlineData(1001)]
    public async Task GetRpiOrderBookAsync_RejectsUnsupportedLimit(long limit)
    {
        using var client = CreateClient(new HttpClient(new RecordingHttpMessageHandler("{}")));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.UsdFutures.GetRpiOrderBookAsync("BTCUSDT", limit));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task SymbolSpecificQueries_RejectMissingSymbol(string? symbol)
    {
        using var client = CreateClient(new HttpClient(new RecordingHttpMessageHandler("{}")));

        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetRpiOrderBookAsync(symbol!));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetAdlRiskAsync(symbol!));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetPriceAsync(symbol!));
    }

    [Fact]
    public async Task GetAdlRiskAsync_UsesSymbolContractAndDeserializesObject()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"symbol":"BTCUSDT","adlRisk":"low","updateTime":1597370495002}""");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, limiter);

        var result = await client.UsdFutures.GetAdlRiskAsync("BTCUSDT");

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/symbolAdlRisk", handler.RequestUri!.AbsolutePath);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.DoesNotContain("timestamp=", query);
        Assert.DoesNotContain("signature=", query);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/fapi/v1/symbolAdlRisk" && item.Weight == 1 && !item.Signed);
        Assert.Equal("BTCUSDT", result.Data.Symbol);
        Assert.Equal("low", result.Data.AdlRisk);
        Assert.Equal(1597370495002, result.Data.UpdateTime);
    }

    [Fact]
    public async Task GetAdlRisksAsync_OmitsSymbolAndDeserializesArray()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """[{"symbol":"BTCUSDT","adlRisk":"low","updateTime":1597370495002},{"symbol":"ETHUSDT","adlRisk":"medium","updateTime":1597370495003}]""");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, limiter);

        var result = await client.UsdFutures.GetAdlRisksAsync();

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/symbolAdlRisk", handler.RequestUri!.AbsolutePath);
        Assert.Equal(string.Empty, handler.RequestUri.Query);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/fapi/v1/symbolAdlRisk" && item.Weight == 1 && !item.Signed);
        Assert.Collection(
            result.Data,
            item => Assert.Equal("BTCUSDT", item.Symbol),
            item => Assert.Equal("ETHUSDT", item.Symbol));
    }

    [Fact]
    public async Task GetPriceAsync_UsesCurrentV2PublicContractAndDeserializesObject()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"symbol":"BTCUSDT","price":"6000.01","time":1589437530011}""");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, limiter);

        var result = await client.UsdFutures.GetPriceAsync("BTCUSDT");

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v2/ticker/price", handler.RequestUri!.AbsolutePath);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.DoesNotContain("timestamp=", query);
        Assert.DoesNotContain("signature=", query);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/fapi/v2/ticker/price" && item.Weight == 1 && !item.Signed);
        Assert.Equal("BTCUSDT", result.Data.Symbol);
        Assert.Equal(6000.01m, result.Data.Price);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1589437530011).UtcDateTime, result.Data.Time);
    }

    [Fact]
    public async Task GetPricesAsync_UsesCurrentV2PublicContractAndDeserializesArray()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """[{"symbol":"BTCUSDT","price":"6000.01","time":1589437530011},{"symbol":"ETHUSDT","price":"3000.02","time":1589437530012}]""");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, limiter);

        var result = await client.UsdFutures.GetPricesAsync();

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v2/ticker/price", handler.RequestUri!.AbsolutePath);
        Assert.Equal(string.Empty, handler.RequestUri.Query);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/fapi/v2/ticker/price" && item.Weight == 2 && !item.Signed);
        Assert.Collection(
            result.Data,
            item => Assert.Equal("BTCUSDT", item.Symbol),
            item => Assert.Equal("ETHUSDT", item.Symbol));
    }

    [Fact]
    public async Task GetFundingRatesAsync_OmitsOptionalQueryAndDeserializesCurrentResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "symbol": "BTCUSDT",
                "fundingRate": "-0.03750000",
                "fundingTime": 1570608000000,
                "markPrice": "34287.54619963",
                "rateType": "Regular"
              },
              {
                "symbol": "STOCKUSDT",
                "fundingRate": "0.00010000",
                "fundingTime": 1570636800000,
                "markPrice": null,
                "rateType": "Special"
              }
            ]
            """);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, limiter);

        var result = await client.UsdFutures.GetFundingRatesAsync();

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/fundingRate", handler.RequestUri!.AbsolutePath);
        Assert.Equal(string.Empty, handler.RequestUri.Query);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/fapi/v1/fundingRate" && !item.Signed);
        Assert.Collection(
            result.Data,
            item =>
            {
                Assert.Equal("BTCUSDT", item.Symbol);
                Assert.Equal(-0.03750000m, item.FundingRate);
                Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1570608000000).UtcDateTime, item.FundingTime);
                Assert.Equal(34287.54619963m, item.MarkPrice);
                Assert.Equal("Regular", item.RateType);
            },
            item =>
            {
                Assert.Equal("STOCKUSDT", item.Symbol);
                Assert.Equal("Special", item.RateType);
                Assert.Null(item.MarkPrice);
            });
    }

    [Fact]
    public async Task GetFundingRatesAsync_SendsEveryProvidedFilterAsUnsignedQuery()
    {
        var startTime = DateTimeOffset.FromUnixTimeMilliseconds(1623319461670).UtcDateTime;
        var endTime = DateTimeOffset.FromUnixTimeMilliseconds(1641782889000).UtcDateTime;
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, limiter);

        var result = await client.UsdFutures.GetFundingRatesAsync("BTCUSDT", startTime, endTime, 1000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/fundingRate", handler.RequestUri!.AbsolutePath);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("startTime=1623319461670", query);
        Assert.Contains("endTime=1641782889000", query);
        Assert.Contains("limit=1000", query);
        Assert.DoesNotContain("timestamp=", query);
        Assert.DoesNotContain("signature=", query);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/fapi/v1/fundingRate" && !item.Signed);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1001)]
    public async Task GetFundingRatesAsync_RejectsLimitOutsideCurrentRange(int limit)
    {
        using var client = CreateClient(new HttpClient(new RecordingHttpMessageHandler("[]")));

        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.UsdFutures.GetFundingRatesAsync(limit: limit));
    }

    [Fact]
    public async Task GetTradingScheduleAsync_UsesCurrentPublicContractAndDeserializesAllMarkets()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "updateTime": 1761286643918,
              "marketSchedules": {
                "EQUITY": { "sessions": [{ "startTime": 1761177600000, "endTime": 1761206400000, "type": "OVERNIGHT" }] },
                "COMMODITY": { "sessions": [{ "startTime": 1761724800000, "endTime": 1761744600000, "type": "NO_TRADING" }] },
                "KR_EQUITY": { "sessions": [{ "startTime": 1779958800000, "endTime": 1780009200000, "type": "REGULAR" }] },
                "HK_EQUITY": { "sessions": [{ "startTime": 1779955200000, "endTime": 1780018200000, "type": "NO_TRADING" }] }
              }
            }
            """);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, limiter);

        var result = await client.UsdFutures.GetTradingScheduleAsync();

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/tradingSchedule", handler.RequestUri!.AbsolutePath);
        Assert.Equal(string.Empty, handler.RequestUri.Query);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/fapi/v1/tradingSchedule" && item.Weight == 5 && !item.Signed);
        Assert.Equal(1761286643918, result.Data.UpdateTime);

        var equity = Assert.Single(result.Data.MarketSchedules.Equity!.Sessions);
        Assert.Equal(1761177600000, equity.StartTime);
        Assert.Equal(1761206400000, equity.EndTime);
        Assert.Equal("OVERNIGHT", equity.Type);
        Assert.Equal("NO_TRADING", Assert.Single(result.Data.MarketSchedules.Commodity!.Sessions).Type);
        Assert.Equal("REGULAR", Assert.Single(result.Data.MarketSchedules.KoreanEquity!.Sessions).Type);
        Assert.Equal("NO_TRADING", Assert.Single(result.Data.MarketSchedules.HongKongEquity!.Sessions).Type);
    }

    [Theory]
    [InlineData("openInterestHist")]
    [InlineData("topLongShortPositionRatio")]
    [InlineData("topLongShortAccountRatio")]
    [InlineData("globalLongShortAccountRatio")]
    [InlineData("takerlongshortRatio")]
    [InlineData("basis")]
    public async Task FuturesDataMethods_SendRequestsToDocumentedRootPath(string endpoint)
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        switch (endpoint)
        {
            case "openInterestHist":
                await client.UsdFutures.GetOpenInterestHistoryAsync("BTCUSDT", BinancePeriodInterval.FiveMinutes);
                break;
            case "topLongShortPositionRatio":
                await client.UsdFutures.GetTopLongShortPositionRatioAsync("BTCUSDT", BinancePeriodInterval.FiveMinutes);
                break;
            case "topLongShortAccountRatio":
                await client.UsdFutures.GetTopLongShortAccountRatioAsync("BTCUSDT", BinancePeriodInterval.FiveMinutes);
                break;
            case "globalLongShortAccountRatio":
                await client.UsdFutures.GetGlobalLongShortAccountRatioAsync("BTCUSDT", BinancePeriodInterval.FiveMinutes);
                break;
            case "takerlongshortRatio":
                await client.UsdFutures.GetTakerBuySellVolumeRatioAsync("BTCUSDT", BinancePeriodInterval.FiveMinutes);
                break;
            case "basis":
                await client.UsdFutures.GetBasisAsync("BTCUSDT", BinanceFuturesContractType.Perpetual, BinancePeriodInterval.FiveMinutes);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(endpoint), endpoint, null);
        }

        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal($"/futures/data/{endpoint}", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
    }

    [Fact]
    public async Task GetOpenInterestHistoryAsync_DeserializesCirculatingSupply()
    {
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "symbol": "BTCUSDT",
                "sumOpenInterest": "20403.12345678",
                "sumOpenInterestValue": "176196512.12345678",
                "CMCCirculatingSupply": "165880.538",
                "timestamp": 1591261042378
              }
            ]
            """);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        var result = await client.UsdFutures.GetOpenInterestHistoryAsync(
            "BTCUSDT",
            BinancePeriodInterval.FiveMinutes);

        Assert.True(result.Success);
        var item = Assert.Single(result.Data);
        Assert.Equal(165880.538m, item.CmcCirculatingSupply);
    }

    [Theory]
    [InlineData(BinanceFuturesContractType.Unknown)]
    [InlineData(BinanceFuturesContractType.CurrentMonth)]
    public async Task GetBasisAsync_RejectsUnsupportedContractTypes(BinanceFuturesContractType contractType)
    {
        using var client = CreateClient(new HttpClient(new RecordingHttpMessageHandler("[]")));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.GetBasisAsync(
            "BTCUSDT",
            contractType,
            BinancePeriodInterval.FiveMinutes));
    }

    private static BinanceRestApiClient CreateClient(HttpClient httpClient, IRateLimiter? limiter = null)
    {
        var options = new BinanceRestApiClientOptions(new ApiCredentials("api-key", "api-secret"))
        {
            AutoTimestamp = false,
            HttpClient = httpClient,
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
