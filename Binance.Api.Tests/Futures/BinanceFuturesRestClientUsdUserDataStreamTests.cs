using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdUserDataStreamTests
{
    [Fact]
    public async Task StartUserStreamAsync_UsesCurrentApiKeyOnlyParameterlessContract()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""{"listenKey":"active-listen-key"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.StartUserStreamAsync();

        Assert.True(result.Success);
        Assert.Equal("active-listen-key", result.Data);
        AssertParameterlessApiKeyOnlyRequest(handler, limiter, HttpMethod.Post);
    }

    [Fact]
    public async Task KeepAliveUserStreamAsync_ReturnsRefreshedKeyWithoutSendingOldKey()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""{"listenKey":"refreshed-listen-key"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.KeepAliveUserStreamAsync();

        Assert.True(result.Success);
        Assert.Equal("refreshed-listen-key", result.Data);
        AssertParameterlessApiKeyOnlyRequest(handler, limiter, HttpMethod.Put);
    }

    [Fact]
    public async Task StopUserStreamAsync_UsesCurrentNoContentContract()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(string.Empty);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.StopUserStreamAsync();

        Assert.True(result.Success);
        Assert.True(result.Data);
        AssertParameterlessApiKeyOnlyRequest(handler, limiter, HttpMethod.Delete);
    }

    private static void AssertParameterlessApiKeyOnlyRequest(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        HttpMethod method)
    {
        Assert.Equal(method, handler.Method);
        Assert.Equal("/fapi/v1/listenKey", handler.RequestUri!.AbsolutePath);
        Assert.Empty(handler.RequestUri.Query);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values));
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == "/fapi/v1/listenKey" && request.Weight == 1 && !request.Signed);
    }

    private static BinanceRestApiClient CreateClient(
        RecordingHttpMessageHandler handler,
        IRateLimiter limiter)
    {
        var options = new BinanceRestApiClientOptions(new ApiCredentials("api-key"))
        {
            HttpClient = new HttpClient(handler),
            RateLimiterEnabled = true
        };
#pragma warning disable CS0612
        options.RateLimiters = [limiter];
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
