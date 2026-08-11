using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Options;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsMarketMakerTests
{
    private const string Underlying = "BTCUSDT";
    private const long Int64Value = 3_000_000_000L;

    [Fact]
    public async Task GetProtection_UsesCurrentSignedQueryAndCompleteInt64Response()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"underlyingId":3000000000,"underlying":"BTCUSDT","windowTimeInMilliseconds":5000,"frozenTimeInMilliseconds":3000000000,"qtyLimit":"2.25","deltaLimit":"-2.3","lastTriggerTime":3000000000}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.MarketMaker.GetProtectionAsync(Underlying, Int64Value);

        Assert.True(result.Success);
        Assert.Equal(Int64Value, result.Data.UnderlyingId);
        Assert.Equal(Underlying, result.Data.Underlying);
        Assert.Equal(5_000L, result.Data.WindowTimeInMilliseconds);
        Assert.Equal(Int64Value, result.Data.FrozenTimeInMilliseconds);
        Assert.Equal(2.25m, result.Data.QuantityLimit);
        Assert.Equal(-2.3m, result.Data.DeltaLimit);
        Assert.Equal(Int64Value, result.Data.LastTriggerTime);
        AssertSignedRequest(handler, limiter, HttpMethod.Get, "/eapi/v1/mmp", 1, hasBody: false);
        var query = ParseQuery(handler.RequestUri!);
        Assert.Equal(Underlying, query["underlying"]);
        Assert.Equal(Int64Value.ToString(), query["recvWindow"]);
    }

    [Fact]
    public async Task GetCountdown_OmitsOptionalUnderlyingAndPreservesResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"underlying":"ETHUSDT","countdownTime":3000000000}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.MarketMaker.GetCancelAllCountdownAsync(receiveWindow: Int64Value);

        Assert.True(result.Success);
        Assert.Equal("ETHUSDT", result.Data.Underlying);
        Assert.Equal(Int64Value, result.Data.CountdownTime);
        AssertSignedRequest(handler, limiter, HttpMethod.Get, "/eapi/v1/countdownCancelAll", 1, hasBody: false);
        var query = ParseQuery(handler.RequestUri!);
        Assert.False(query.ContainsKey("underlying"));
        Assert.Equal(Int64Value.ToString(), query["recvWindow"]);
    }

    [Fact]
    public async Task SetAndResetProtection_UseCurrentSignedBodies()
    {
        const string response =
            """{"underlyingId":2,"underlying":"BTCUSDT","windowTimeInMilliseconds":5000,"frozenTimeInMilliseconds":3000000000,"qtyLimit":"1.25","deltaLimit":"0.75","lastTriggerTime":0}""";

        var setLimiter = new RecordingRateLimiter();
        var setHandler = new RecordingHttpMessageHandler(response);
        using (var setClient = CreateClient(setHandler, setLimiter))
        {
            var result = await setClient.Options.MarketMaker.SetProtectionAsync(
                Underlying, 5_000L, Int64Value, 1.25m, 0.75m, Int64Value);

            Assert.True(result.Success);
            Assert.Equal(Int64Value, result.Data.FrozenTimeInMilliseconds);
            AssertSignedRequest(setHandler, setLimiter, HttpMethod.Post, "/eapi/v1/mmpSet", 1, hasBody: true);
            var body = ParseBody(setHandler);
            Assert.Equal(Underlying, body["underlying"]);
            Assert.Equal("5000", body["windowTimeInMilliseconds"]);
            Assert.Equal(Int64Value.ToString(), body["frozenTimeInMilliseconds"]);
            Assert.Equal("1.25", body["qtyLimit"]);
            Assert.Equal("0.75", body["deltaLimit"]);
            Assert.Equal(Int64Value.ToString(), body["recvWindow"]);
        }

        var resetLimiter = new RecordingRateLimiter();
        var resetHandler = new RecordingHttpMessageHandler(response);
        using var resetClient = CreateClient(resetHandler, resetLimiter);
        var resetResult = await resetClient.Options.MarketMaker.ResetProtectionAsync(Underlying, Int64Value);

        Assert.True(resetResult.Success);
        AssertSignedRequest(resetHandler, resetLimiter, HttpMethod.Post, "/eapi/v1/mmpReset", 1, hasBody: true);
        var resetBody = ParseBody(resetHandler);
        Assert.Equal(Underlying, resetBody["underlying"]);
        Assert.Equal(Int64Value.ToString(), resetBody["recvWindow"]);
    }

    [Fact]
    public async Task Heartbeat_UsesWeightTenAndEnumeratesUnderlyingsOnce()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""{"underlyings":["BTCUSDT","ETHUSDT"]}""");
        using var client = CreateClient(handler, limiter);
        var enumerationCount = 0;

        var result = await client.Options.MarketMaker.CancelAllCountdownHeartbeatAsync(
            EnumerateOnce(["BTCUSDT", "ETHUSDT"], () => enumerationCount++),
            Int64Value);

        Assert.True(result.Success);
        Assert.Equal(1, enumerationCount);
        Assert.Equal(["BTCUSDT", "ETHUSDT"], result.Data.Underlyings);
        AssertSignedRequest(handler, limiter, HttpMethod.Post, "/eapi/v1/countdownCancelAllHeartBeat", 10, hasBody: true);
        var body = ParseBody(handler);
        Assert.Equal("BTCUSDT,ETHUSDT", body["underlyings"]);
        Assert.Equal(Int64Value.ToString(), body["recvWindow"]);
    }

    [Fact]
    public async Task SetCountdown_UsesSingularUnderlyingAndCurrentRange()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"underlying":"BTCUSDT","countdownTime":3000000000}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.MarketMaker.SetCancelAllCountdownAsync(
            Underlying,
            Int64Value,
            Int64Value);

        Assert.True(result.Success);
        Assert.Equal(Underlying, result.Data.Underlying);
        Assert.Equal(Int64Value, result.Data.CountdownTime);
        AssertSignedRequest(handler, limiter, HttpMethod.Post, "/eapi/v1/countdownCancelAll", 1, hasBody: true);
        var body = ParseBody(handler);
        Assert.Equal(Underlying, body["underlying"]);
        Assert.False(body.ContainsKey("underlyings"));
        Assert.Equal(Int64Value.ToString(), body["countdownTime"]);
        Assert.Equal(Int64Value.ToString(), body["recvWindow"]);
    }

    [Fact]
    public async Task InvalidInputsFailBeforeTransportAndPublicTimesAreInt64()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.GetProtectionAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.GetCancelAllCountdownAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.MarketMaker.SetProtectionAsync(Underlying, -1, 0, 1, 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.MarketMaker.SetProtectionAsync(Underlying, 5_001, 0, 1, 1));
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.Options.MarketMaker.CancelAllCountdownHeartbeatAsync(null!));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.CancelAllCountdownHeartbeatAsync([]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.CancelAllCountdownHeartbeatAsync([Underlying, " "]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.ResetProtectionAsync(""));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.MarketMaker.SetCancelAllCountdownAsync(Underlying, -1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.MarketMaker.SetCancelAllCountdownAsync(Underlying, 4_999));
        Assert.Equal(0, handler.RequestCount);

        foreach (var method in typeof(IBinanceOptionsRestClientMarketMakerAccount).GetMethods())
        {
            Assert.Equal(typeof(long?), method.GetParameters().Single(parameter => parameter.Name == "receiveWindow").ParameterType);
        }

        var setProtection = typeof(IBinanceOptionsRestClientMarketMakerAccount)
            .GetMethod(nameof(IBinanceOptionsRestClientMarketMakerAccount.SetProtectionAsync))!;
        Assert.Equal(typeof(long), setProtection.GetParameters().Single(parameter => parameter.Name == "windowTimeInMilliseconds").ParameterType);
        Assert.Equal(typeof(long), setProtection.GetParameters().Single(parameter => parameter.Name == "frozenTimeInMilliseconds").ParameterType);
        var setCountdown = typeof(IBinanceOptionsRestClientMarketMakerAccount)
            .GetMethod(nameof(IBinanceOptionsRestClientMarketMakerAccount.SetCancelAllCountdownAsync))!;
        Assert.Equal(typeof(long), setCountdown.GetParameters().Single(parameter => parameter.Name == "countdownTime").ParameterType);
        Assert.Equal(typeof(long), typeof(BinanceOptionsMarketMakerProtection).GetProperty(nameof(BinanceOptionsMarketMakerProtection.WindowTimeInMilliseconds))!.PropertyType);
        Assert.Equal(typeof(long), typeof(BinanceOptionsMarketMakerProtection).GetProperty(nameof(BinanceOptionsMarketMakerProtection.FrozenTimeInMilliseconds))!.PropertyType);
    }

    private static IEnumerable<T> EnumerateOnce<T>(IEnumerable<T> values, Action onEnumeration)
    {
        onEnumeration();
        foreach (var value in values)
            yield return value;
    }

    private static Dictionary<string, string> ParseBody(RecordingHttpMessageHandler handler)
        => ParseParameters(handler.Body!);

    private static Dictionary<string, string> ParseQuery(Uri uri)
        => ParseParameters(uri.Query.TrimStart('?'));

    private static Dictionary<string, string> ParseParameters(string parameters)
        => parameters.Split('&')
            .Select(value => value.Split('=', 2))
            .ToDictionary(value => Uri.UnescapeDataString(value[0]), value => Uri.UnescapeDataString(value[1]));

    private static void AssertSignedRequest(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        HttpMethod method,
        string path,
        int weight,
        bool hasBody)
    {
        Assert.Equal(method, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Equal(hasBody, handler.Body != null);
        Assert.Equal(hasBody ? "application/x-www-form-urlencoded" : null, handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == path && item.Weight == weight && item.Signed);
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
