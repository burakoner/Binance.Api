using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginAccountTests
{
    [Fact]
    public async Task DisableIsolatedMarginAccount_UsesSignedDeleteQueryContract()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""{"success":true,"symbol":"BTCUSDT"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Margin.DisableIsolatedMarginAccountAsync("BTCUSDT", 5_000);

        Assert.True(result.Success);
        Assert.True(result.Data.Success);
        Assert.Equal("BTCUSDT", result.Data.Symbol);
        Assert.Equal(HttpMethod.Delete, handler.Method);
        Assert.Equal("/sapi/v1/margin/isolated/account", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("recvWindow=5000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/isolated/account" && item.Weight == 300);
    }

    [Fact]
    public async Task DisableIsolatedMarginAccount_RejectsInvalidRequests()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.DisableIsolatedMarginAccountAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.DisableIsolatedMarginAccountAsync(
            "BTCUSDT", 60_001));
    }

    private static BinanceRestApiClient CreateClient(RecordingHttpMessageHandler handler, IRateLimiter? limiter = null)
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
        public List<(string Endpoint, int Weight)> Requests { get; } = [];

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
            Requests.Add((endpoint, requestWeight));
            return Task.FromResult(new CallResult<int>(0));
        }
    }
}
