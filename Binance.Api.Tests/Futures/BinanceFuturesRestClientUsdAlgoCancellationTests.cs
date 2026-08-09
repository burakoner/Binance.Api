using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdAlgoCancellationTests
{
    [Fact]
    public async Task CancelAlgoOrderAsync_UsesCurrentSignedContractAndDeserializesResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "algoId": 2147676000001,
              "clientAlgoId": "6B2I9XVcJpCjqPAJ4YoFX7",
              "code": "200",
              "msg": "success"
            }
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.CancelAlgoOrderAsync(
            algoId: 2_147_676_000_001L,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.Equal(2_147_676_000_001L, result.Data!.AlgoId);
        Assert.Equal("6B2I9XVcJpCjqPAJ4YoFX7", result.Data.ClientAlgoId);
        Assert.Equal("200", result.Data.Code);
        Assert.Equal("success", result.Data.Message);
        Assert.Equal(HttpMethod.Delete, handler.Method);
        Assert.Equal("/fapi/v1/algoOrder", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = DecodedQuery(handler);
        Assert.Contains("algoId=2147676000001", query);
        Assert.DoesNotContain("clientAlgoId=", query);
        Assert.Contains("recvWindow=60000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item =>
            item.Endpoint == "/fapi/v1/algoOrder" && item.Weight == 1 && item.Signed);
    }

    [Fact]
    public async Task CancelAlgoOrderAsync_AcceptsClientIdentifierAlternativeAndConfiguredReceiveWindow()
    {
        var handler = new RecordingHttpMessageHandler(
            """{"algoId":2146760,"clientAlgoId":"client-only","code":"200","msg":"success"}""");
        using var client = CreateClient(
            handler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(5_000));

        var result = await client.UsdFutures.CancelAlgoOrderAsync(clientAlgoId: "client-only");

        Assert.True(result.Success);
        var query = DecodedQuery(handler);
        Assert.Contains("clientAlgoId=client-only", query);
        Assert.Contains("recvWindow=5000", query);
        Assert.DoesNotContain("algoId=", query);
    }

    [Fact]
    public async Task CancelAlgoOrderAsync_RejectsInvalidIdentifiersAndReceiveWindowsBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using (var client = CreateClient(handler))
        {
            await Assert.ThrowsAsync<ArgumentException>(() => client.UsdFutures.CancelAlgoOrderAsync());
            await Assert.ThrowsAsync<ArgumentException>(() =>
                client.UsdFutures.CancelAlgoOrderAsync(clientAlgoId: " "));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.UsdFutures.CancelAlgoOrderAsync(algoId: 1, receiveWindow: 60_001));
        }

        using var configuredClient = CreateClient(
            handler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.CancelAlgoOrderAsync(algoId: 1));
        Assert.Null(handler.RequestUri);
    }

    private static string DecodedQuery(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.RequestUri!.Query);

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
