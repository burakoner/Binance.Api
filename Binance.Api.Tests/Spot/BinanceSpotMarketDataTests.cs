using ApiSharp.Authentication;
using Binance.Api.Shared;
using Binance.Api.Spot;

namespace Binance.Api.Tests.Spot;

public class BinanceSpotMarketDataTests
{
    [Fact]
    public async Task OrderBook_SendsCurrentStatusAndUsesCurrentWeightTable()
    {
        var handler = new RecordingHttpMessageHandler("""{"lastUpdateId":42,"bids":[],"asks":[]}""");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        var result = await client.Spot.GetOrderBookAsync("BTCUSDT", status: BinanceSpotSymbolStatusFilter.Break);

        Assert.True(result.Success);
        Assert.Equal(42, result.Data.LastUpdateId);
        Assert.Contains("symbolStatus=BREAK", Uri.UnescapeDataString(handler.RequestUri!.Query));
        Assert.Equal(5, BinanceSpotMarketDataValidation.DepthWeight(null));
        Assert.Equal(5, BinanceSpotMarketDataValidation.DepthWeight(100));
        Assert.Equal(25, BinanceSpotMarketDataValidation.DepthWeight(500));
        Assert.Equal(50, BinanceSpotMarketDataValidation.DepthWeight(1000));
        Assert.Equal(250, BinanceSpotMarketDataValidation.DepthWeight(5000));
    }

    [Fact]
    public async Task HistoricalBlockTrades_UsesMarketDataRouteApiKeyAndCurrentModel()
    {
        const string response = """
            [{
              "id": 582,
              "price": "0.10000000",
              "qty": "10.00000000",
              "quoteQty": "1.00000000",
              "time": 1650000000123,
              "isBuyerMaker": true
            }]
            """;
        var handler = new RecordingHttpMessageHandler(response);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient, new ApiCredentials("api-key", "secret"));

        var result = await client.Spot.GetHistoricalBlockTradesAsync("BTCUSDT", 582, 1000);

        Assert.True(result.Success);
        Assert.Equal("/api/v3/historicalBlockTrades", handler.RequestUri!.AbsolutePath);
        Assert.Contains("fromId=582", handler.RequestUri.Query);
        Assert.Equal("api-key", Assert.Single(handler.Headers["X-MBX-APIKEY"]));
        Assert.DoesNotContain("signature=", handler.RequestUri.Query);
        var trade = Assert.Single(result.Data);
        Assert.Equal(582, trade.TradeId);
        Assert.Equal(1m, trade.QuoteQuantity);
        Assert.True(trade.IsBuyerMaker);
    }

    [Fact]
    public async Task Klines_SendTimeZoneAndEnforceCurrentLimitAndRange()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);
        var start = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = start.AddDays(1);

        var result = await client.Spot.GetKlinesAsync(
            "BTCUSDT",
            BinanceKlineInterval.OneHour,
            start,
            end,
            "05:45",
            1000);

        Assert.True(result.Success);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("timeZone=05:45", query);
        Assert.Contains("limit=1000", query);
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Spot.GetKlinesAsync("BTCUSDT", BinanceKlineInterval.OneHour, limit: 1001));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            client.Spot.GetKlinesAsync("BTCUSDT", BinanceKlineInterval.OneHour, end, start));
    }

    [Fact]
    public async Task ReferencePriceEndpoints_UseCurrentRoutesStatusAndModels()
    {
        var priceHandler = new RecordingHttpMessageHandler(
            """{"symbol":"BTCUSDT","referencePrice":"50000.12345678","timestamp":1650000000123}""");
        using var priceHttpClient = new HttpClient(priceHandler);
        using var priceClient = CreateClient(priceHttpClient);

        var priceResult = await priceClient.Spot.GetReferencePriceAsync("BTCUSDT");

        Assert.True(priceResult.Success);
        Assert.Equal("/api/v3/referencePrice", priceHandler.RequestUri!.AbsolutePath);
        Assert.Equal(50000.12345678m, priceResult.Data.ReferencePrice);

        var calculationHandler = new RecordingHttpMessageHandler(
            """{"symbol":"BTCUSDT","calculationType":"ARITHMETIC_MEAN","bucketCount":10,"bucketWidthMs":1000}""");
        using var calculationHttpClient = new HttpClient(calculationHandler);
        using var calculationClient = CreateClient(calculationHttpClient);

        var calculationResult = await calculationClient.Spot.GetReferencePriceCalculationAsync(
            "BTCUSDT",
            BinanceSpotSymbolStatusFilter.Trading);

        Assert.True(calculationResult.Success);
        Assert.Equal("/api/v3/referencePrice/calculation", calculationHandler.RequestUri!.AbsolutePath);
        Assert.Contains("symbolStatus=TRADING", calculationHandler.RequestUri.Query);
        Assert.Equal(BinanceSpotReferencePriceCalculationType.ArithmeticMean, calculationResult.Data.CalculationType);
        Assert.Equal(10, calculationResult.Data.BucketCount);
        Assert.Equal(1000, calculationResult.Data.BucketWidthMs);
    }

    [Fact]
    public async Task RollingWindowTicker_UsesCurrentModelParametersAndWeightTable()
    {
        const string response = """
            {
              "symbol":"BTCUSDT",
              "priceChange":"100.0",
              "priceChangePercent":"0.2",
              "weightedAvgPrice":"50000.0",
              "openPrice":"49900.0",
              "highPrice":"50200.0",
              "lowPrice":"49800.0",
              "lastPrice":"50000.0",
              "volume":"10.0",
              "quoteVolume":"500000.0",
              "openTime":1650000000000,
              "closeTime":1650014400000,
              "firstId":1,
              "lastId":2,
              "count":2
            }
            """;
        var handler = new RecordingHttpMessageHandler(response);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        var result = await client.Spot.GetRollingWindowTickerAsync(
            "BTCUSDT",
            TimeSpan.FromHours(4),
            BinanceSpotSymbolStatusFilter.Trading);

        Assert.True(result.Success);
        Assert.Equal(100m, result.Data.PriceChange);
        Assert.Equal(2, result.Data.TotalTrades);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("windowSize=4h", query);
        Assert.Contains("type=FULL", query);
        Assert.Contains("symbolStatus=TRADING", query);
        Assert.Equal(4, BinanceSpotMarketDataValidation.RollingTickerWeight(1));
        Assert.Equal(200, BinanceSpotMarketDataValidation.RollingTickerWeight(51));
        Assert.Equal(2, BinanceSpotMarketDataValidation.Ticker24HourWeight(20));
        Assert.Equal(40, BinanceSpotMarketDataValidation.Ticker24HourWeight(21));
        Assert.Equal(80, BinanceSpotMarketDataValidation.Ticker24HourWeight(null));
    }

    [Fact]
    public async Task MarketDataValidation_RejectsInvalidOfficialParameterCombinations()
    {
        Assert.Equal("-12:00", BinanceSpotMarketDataValidation.TimeZone("-12:00"));
        Assert.Equal("14:00", BinanceSpotMarketDataValidation.TimeZone("14:00"));
        Assert.Equal("59m", BinanceSpotMarketDataValidation.WindowSize(TimeSpan.FromMinutes(59)));
        Assert.Equal("23h", BinanceSpotMarketDataValidation.WindowSize(TimeSpan.FromHours(23)));
        Assert.Equal("7d", BinanceSpotMarketDataValidation.WindowSize(TimeSpan.FromDays(7)));

        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceSpotMarketDataValidation.TimeZone("14:01"));
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceSpotMarketDataValidation.TimeZone("-12:01"));
        Assert.Throws<ArgumentException>(() => BinanceSpotMarketDataValidation.WindowSize(TimeSpan.FromMinutes(61)));
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceSpotMarketDataValidation.WindowSize(TimeSpan.FromDays(8)));
        Assert.Throws<ArgumentException>(() => BinanceSpotMarketDataValidation.Symbols([]));

        using var client = CreateClient(new HttpClient(new RecordingHttpMessageHandler("[]")));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Spot.GetAggregatedTradesAsync(
            "BTCUSDT",
            fromId: 1,
            startTime: DateTime.UtcNow));
    }

    private static BinanceRestApiClient CreateClient(HttpClient httpClient, ApiCredentials? credentials = null)
    {
        var options = credentials == null
            ? new BinanceRestApiClientOptions()
            : new BinanceRestApiClientOptions(credentials);
        options.HttpClient = httpClient;
        options.RateLimiterEnabled = false;
        options.AutoTimestamp = false;
        return new BinanceRestApiClient(options);
    }
}
