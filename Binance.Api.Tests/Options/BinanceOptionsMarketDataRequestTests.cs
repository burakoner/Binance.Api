using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Options;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsMarketDataRequestTests
{
    private const string Symbol = "BTC-251226-90000-C";
    private const string Underlying = "BTCUSDT";

    [Fact]
    public async Task GeneralMarketDataRoutes_AccountForOfficialWeight()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("{\"serverTime\":1762843368098}");
        using var client = CreateClient(handler, limiter);

        var ping = await client.Options.PingAsync();
        Assert.True(ping.Success);
        AssertUnsignedRequest(handler, HttpMethod.Get, "/eapi/v1/ping");

        var time = await client.Options.GetTimeAsync();
        Assert.True(time.Success);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1762843368098).UtcDateTime, time.Data);
        AssertUnsignedRequest(handler, HttpMethod.Get, "/eapi/v1/time");

        var exchangeInfo = await client.Options.GetExchangeInfoAsync();
        Assert.True(exchangeInfo.Success);
        AssertUnsignedRequest(handler, HttpMethod.Get, "/eapi/v1/exchangeInfo");

        AssertRateLimit(limiter, "/eapi/v1/ping", 1);
        AssertRateLimit(limiter, "/eapi/v1/time", 1);
        AssertRateLimit(limiter, "/eapi/v1/exchangeInfo", 1);
    }

    [Fact]
    public async Task Tickers_UseCurrentFilteredAndUnfilteredWeights()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler($"[{{\"symbol\":\"{Symbol}\"}}]");
        using var client = CreateClient(handler, limiter);

        var all = await client.Options.GetTickersAsync();
        Assert.True(all.Success);
        AssertUnsignedRequest(handler, HttpMethod.Get, "/eapi/v1/ticker");
        Assert.Empty(ParseQuery(handler.RequestUri!));

        var single = await client.Options.GetTickersAsync(Symbol);
        Assert.True(single.Success);
        Assert.Equal(Symbol, single.Data.Symbol);
        AssertUnsignedRequest(handler, HttpMethod.Get, "/eapi/v1/ticker");
        Assert.Equal(Symbol, ParseQuery(handler.RequestUri!)["symbol"]);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/eapi/v1/ticker" && item.Weight == 40 && !item.Signed);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/eapi/v1/ticker" && item.Weight == 1 && !item.Signed);
    }

    [Fact]
    public async Task ExerciseHistory_UsesOptionalFiltersAndInt64Schema()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);
        var start = new DateTime(2025, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var end = start.AddHours(1);

        var result = await client.Options.GetPublicExerciseRecordsAsync(Underlying, start, end, 100L);

        Assert.True(result.Success);
        AssertUnsignedRequest(handler, HttpMethod.Get, "/eapi/v1/exerciseHistory");
        AssertRateLimit(limiter, "/eapi/v1/exerciseHistory", 3);
        var query = ParseQuery(handler.RequestUri!);
        Assert.Equal(Underlying, query["underlying"]);
        Assert.Equal(new DateTimeOffset(start).ToUnixTimeMilliseconds().ToString(), query["startTime"]);
        Assert.Equal(new DateTimeOffset(end).ToUnixTimeMilliseconds().ToString(), query["endTime"]);
        Assert.Equal("100", query["limit"]);
    }

    [Fact]
    public async Task OpenInterest_UsesOfficialYearMonthDayExpirationFormat()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.GetOpenInterestAsync(Underlying, new DateTime(2025, 12, 26));

        Assert.True(result.Success);
        AssertUnsignedRequest(handler, HttpMethod.Get, "/eapi/v1/openInterest");
        AssertRateLimit(limiter, "/eapi/v1/openInterest", 0);
        var query = ParseQuery(handler.RequestUri!);
        Assert.Equal(Underlying, query["underlyingAsset"]);
        Assert.Equal("251226", query["expiration"]);
    }

    [Theory]
    [InlineData(null, 5)]
    [InlineData(5L, 1)]
    [InlineData(10L, 1)]
    [InlineData(20L, 1)]
    [InlineData(50L, 1)]
    [InlineData(100L, 5)]
    [InlineData(500L, 10)]
    [InlineData(1000L, 20)]
    public async Task OrderBook_UsesCurrentLimitWeightTable(long? limit, int expectedWeight)
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.GetOrderBookAsync(Symbol, limit);

        Assert.True(result.Success);
        AssertUnsignedRequest(handler, HttpMethod.Get, "/eapi/v1/depth");
        AssertRateLimit(limiter, "/eapi/v1/depth", expectedWeight);
        var query = ParseQuery(handler.RequestUri!);
        Assert.Equal(Symbol, query["symbol"]);
        if (limit is null)
            Assert.False(query.ContainsKey("limit"));
        else
            Assert.Equal(limit.Value.ToString(), query["limit"]);
    }

    [Fact]
    public async Task RecentTrades_UsesRequiredOptionSymbolAndCurrentLimit()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.GetRecentTradesAsync(Symbol, 500L);

        Assert.True(result.Success);
        AssertUnsignedRequest(handler, HttpMethod.Get, "/eapi/v1/trades");
        AssertRateLimit(limiter, "/eapi/v1/trades", 5);
        var query = ParseQuery(handler.RequestUri!);
        Assert.Equal(Symbol, query["symbol"]);
        Assert.Equal("500", query["limit"]);
    }

    [Fact]
    public async Task RecentBlockTrades_AllowsTheDocumentedUnfilteredRequest()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);

        var all = await client.Options.GetRecentBlockTradesAsync(limit: 500L);

        Assert.True(all.Success);
        AssertUnsignedRequest(handler, HttpMethod.Get, "/eapi/v1/blockTrades");
        AssertRateLimit(limiter, "/eapi/v1/blockTrades", 5);
        var query = ParseQuery(handler.RequestUri!);
        Assert.False(query.ContainsKey("symbol"));
        Assert.Equal("500", query["limit"]);

        var filtered = await client.Options.GetRecentBlockTradesAsync(Symbol);

        Assert.True(filtered.Success);
        Assert.Equal(Symbol, ParseQuery(handler.RequestUri!)["symbol"]);
    }

    [Fact]
    public async Task IndexAndMarkPrice_UseCurrentRequiredAndOptionalFilters()
    {
        var indexLimiter = new RecordingRateLimiter();
        var indexHandler = new RecordingHttpMessageHandler("{}");
        using (var indexClient = CreateClient(indexHandler, indexLimiter))
        {
            var index = await indexClient.Options.GetIndexPriceAsync(Underlying);

            Assert.True(index.Success);
            AssertUnsignedRequest(indexHandler, HttpMethod.Get, "/eapi/v1/index");
            AssertRateLimit(indexLimiter, "/eapi/v1/index", 1);
            Assert.Equal(Underlying, ParseQuery(indexHandler.RequestUri!)["underlying"]);
        }

        var markLimiter = new RecordingRateLimiter();
        var markHandler = new RecordingHttpMessageHandler("[]");
        using var markClient = CreateClient(markHandler, markLimiter);
        var allMarks = await markClient.Options.GetMarkPriceAsync();

        Assert.True(allMarks.Success);
        AssertUnsignedRequest(markHandler, HttpMethod.Get, "/eapi/v1/mark");
        AssertRateLimit(markLimiter, "/eapi/v1/mark", 5);
        Assert.Empty(ParseQuery(markHandler.RequestUri!));

        var filteredMarks = await markClient.Options.GetMarkPriceAsync(Symbol);

        Assert.True(filteredMarks.Success);
        Assert.Equal(Symbol, ParseQuery(markHandler.RequestUri!)["symbol"]);
    }

    [Fact]
    public async Task Klines_UseCurrentIntervalTimeLimitAndWeightContract()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);
        var start = new DateTime(2025, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        var end = start.AddHours(1);

        var result = await client.Options.GetKlinesAsync(
            Symbol, BinanceKlineInterval.OneMonth, start, end, 1500L);

        Assert.True(result.Success);
        AssertUnsignedRequest(handler, HttpMethod.Get, "/eapi/v1/klines");
        AssertRateLimit(limiter, "/eapi/v1/klines", 1);
        var query = ParseQuery(handler.RequestUri!);
        Assert.Equal(Symbol, query["symbol"]);
        Assert.Equal("1M", query["interval"]);
        Assert.Equal(new DateTimeOffset(start).ToUnixTimeMilliseconds().ToString(), query["startTime"]);
        Assert.Equal(new DateTimeOffset(end).ToUnixTimeMilliseconds().ToString(), query["endTime"]);
        Assert.Equal("1500", query["limit"]);
    }

    [Fact]
    public async Task InvalidInputs_FailBeforeTransportAndPublicLimitsAreInt64()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetTickersAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetPublicExerciseRecordsAsync(""));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetPublicExerciseRecordsAsync(limit: 101L));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetOpenInterestAsync(" ", DateTime.UtcNow));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetOrderBookAsync("", 100L));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetOrderBookAsync(Symbol, 25L));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetRecentTradesAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetRecentTradesAsync(Symbol, 501L));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetRecentBlockTradesAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetRecentBlockTradesAsync(limit: 501L));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetIndexPriceAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetKlinesAsync(" ", BinanceKlineInterval.OneMinute));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetKlinesAsync(Symbol, BinanceKlineInterval.OneSecond));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetKlinesAsync(Symbol, (BinanceKlineInterval)12345));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.GetKlinesAsync(Symbol, BinanceKlineInterval.OneMinute, limit: 1501L));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.GetMarkPriceAsync(" "));
        Assert.Equal(0, handler.RequestCount);

        var int64LimitMethods = new[]
        {
            nameof(IBinanceOptionsRestClientMarketData.GetPublicExerciseRecordsAsync),
            nameof(IBinanceOptionsRestClientMarketData.GetOrderBookAsync),
            nameof(IBinanceOptionsRestClientMarketData.GetRecentTradesAsync),
            nameof(IBinanceOptionsRestClientMarketData.GetRecentBlockTradesAsync),
            nameof(IBinanceOptionsRestClientMarketData.GetKlinesAsync)
        };

        foreach (var methodName in int64LimitMethods)
        {
            var method = typeof(IBinanceOptionsRestClientMarketData).GetMethod(methodName)!;
            Assert.Equal(typeof(long?), method.GetParameters().Single(parameter => parameter.Name == "limit").ParameterType);
        }

        Assert.True(typeof(IBinanceOptionsRestClientMarketData)
            .GetMethod(nameof(IBinanceOptionsRestClientMarketData.GetRecentBlockTradesAsync))!
            .GetParameters().Single(parameter => parameter.Name == "symbol").HasDefaultValue);
        Assert.True(typeof(IBinanceOptionsRestClientMarketData)
            .GetMethod(nameof(IBinanceOptionsRestClientMarketData.GetMarkPriceAsync))!
            .GetParameters().Single(parameter => parameter.Name == "symbol").HasDefaultValue);
    }

    private static Dictionary<string, string> ParseQuery(Uri uri)
        => string.IsNullOrEmpty(uri.Query)
            ? []
            : uri.Query.TrimStart('?').Split('&')
                .Select(value => value.Split('=', 2))
                .ToDictionary(value => Uri.UnescapeDataString(value[0]), value => Uri.UnescapeDataString(value[1]));

    private static void AssertUnsignedRequest(
        RecordingHttpMessageHandler handler,
        HttpMethod method,
        string path)
    {
        Assert.Equal(method, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.DoesNotContain("signature=", handler.RequestUri.Query);
    }

    private static void AssertRateLimit(RecordingRateLimiter limiter, string endpoint, int weight)
        => Assert.Contains(limiter.Requests, item =>
            item.Endpoint == endpoint && item.Weight == weight && !item.Signed);

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
