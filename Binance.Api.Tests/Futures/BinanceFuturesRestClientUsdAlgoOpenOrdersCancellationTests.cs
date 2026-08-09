using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdAlgoOpenOrdersCancellationTests
{
    [Fact]
    public async Task CancelAllOpenAlgoOrdersAsync_UsesCurrentSignedContractAndDeserializesResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"code":9223372036854775807,"msg":"The operation of cancel all open order is done."}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.CancelAllOpenAlgoOrdersAsync("BTCUSDT", 60_000);

        Assert.True(result.Success);
        Assert.Equal(long.MaxValue, result.Data!.Code);
        Assert.Equal("The operation of cancel all open order is done.", result.Data.Message);
        Assert.Equal(HttpMethod.Delete, handler.Method);
        Assert.Equal("/fapi/v1/algoOpenOrders", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("recvWindow=60000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item =>
            item.Endpoint == "/fapi/v1/algoOpenOrders" && item.Weight == 1 && item.Signed);
    }

    [Fact]
    public async Task CancelAllOpenAlgoOrdersAsync_UsesConfiguredReceiveWindow()
    {
        var handler = new RecordingHttpMessageHandler(
            """{"code":200,"msg":"The operation of cancel all open order is done."}""");
        using var client = CreateClient(
            handler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(5_000));

        var result = await client.UsdFutures.CancelAllOpenAlgoOrdersAsync("ETHUSDT");

        Assert.True(result.Success);
        Assert.Contains("recvWindow=5000", Uri.UnescapeDataString(handler.RequestUri!.Query));
    }

    [Fact]
    public async Task CancelAllOpenAlgoOrdersAsync_RejectsInvalidInputsBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using (var client = CreateClient(handler))
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                client.UsdFutures.CancelAllOpenAlgoOrdersAsync(" "));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.UsdFutures.CancelAllOpenAlgoOrdersAsync("BTCUSDT", 60_001));
        }

        using var configuredClient = CreateClient(
            handler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.CancelAllOpenAlgoOrdersAsync("BTCUSDT"));
        Assert.Null(handler.RequestUri);
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
