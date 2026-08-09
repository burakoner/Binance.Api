using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Algo;

public class BinanceAlgoCancellationTests
{
    private const long AlgoId = 9_223_372_036_854_775_806L;

    [Fact]
    public async Task FuturesCancellation_UsesCurrentSignedQueryWeightAndResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"algoId":9223372036854775806,"success":true,"code":2147483648,"msg":"OK"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Algo.Futures.CancelAlgoOrderAsync(AlgoId, 60_000);

        Assert.True(result.Success);
        Assert.True(result.Data.Success);
        Assert.Equal(AlgoId, result.Data.AlgoId);
        Assert.Equal(2_147_483_648L, result.Data.Code);
        Assert.Equal("OK", result.Data.Message);
        AssertSignedDelete(handler, limiter, "/sapi/v1/algo/futures/order");
    }

    [Fact]
    public async Task SpotCancellation_UsesCurrentSignedQueryWeightAndResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"algoId":9223372036854775806,"success":true,"code":2147483648,"msg":"OK"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Algo.Spot.CancelAlgoOrderAsync(AlgoId, 60_000);

        Assert.True(result.Success);
        Assert.True(result.Data.Success);
        Assert.Equal(AlgoId, result.Data.AlgoId);
        Assert.Equal(2_147_483_648L, result.Data.Code);
        Assert.Equal("OK", result.Data.Message);
        AssertSignedDelete(handler, limiter, "/sapi/v1/algo/spot/order");
    }

    [Fact]
    public async Task Cancellations_RejectExplicitOrConfiguredReceiveWindowAboveMaximum()
    {
        using (var client = CreateClient(new RecordingHttpMessageHandler("{}")))
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.Algo.Futures.CancelAlgoOrderAsync(AlgoId, 60_001));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.Algo.Spot.CancelAlgoOrderAsync(AlgoId, 60_001));
        }

        using var configuredClient = CreateClient(
            new RecordingHttpMessageHandler("{}"),
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.Algo.Futures.CancelAlgoOrderAsync(AlgoId));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.Algo.Spot.CancelAlgoOrderAsync(AlgoId));
    }

    private static void AssertSignedDelete(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        string path)
    {
        Assert.Equal(HttpMethod.Delete, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains($"algoId={AlgoId}", query);
        Assert.Contains("recvWindow=60000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == path && item.Weight == 1 && item.Signed);
    }

    private static BinanceRestApiClient CreateClient(
        RecordingHttpMessageHandler handler,
        IRateLimiter? limiter = null,
        TimeSpan? defaultReceiveWindow = null)
    {
        var options = new BinanceRestApiClientOptions(new ApiCredentials("api-key", "api-secret"))
        {
            AutoTimestamp = false,
            HttpClient = new HttpClient(handler),
            RateLimiterEnabled = limiter != null,
            ReceiveWindow = defaultReceiveWindow
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
