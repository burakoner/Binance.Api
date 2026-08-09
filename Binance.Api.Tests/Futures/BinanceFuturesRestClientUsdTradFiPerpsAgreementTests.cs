using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdTradFiPerpsAgreementTests
{
    [Fact]
    public async Task SignTradFiPerpsAgreementAsync_UsesCurrentSignedContractAndDeserializesResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"code":9223372036854775807,"msg":"success"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.SignTradFiPerpsAgreementAsync(60_000);

        Assert.True(result.Success);
        Assert.Equal(long.MaxValue, result.Data!.Code);
        Assert.Equal("success", result.Data.Message);
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal("/fapi/v1/stock/contract", handler.RequestUri!.AbsolutePath);
        Assert.Equal("application/x-www-form-urlencoded", handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));

        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("signature=", query);
        Assert.DoesNotContain("timestamp=", query);
        Assert.DoesNotContain("recvWindow=", query);

        var body = ParseBody(handler);
        Assert.Equal("60000", body["recvWindow"]);
        Assert.True(body.ContainsKey("timestamp"));
        Assert.Equal(2, body.Count);
        Assert.DoesNotContain("signature=", handler.Body);
        Assert.Contains(limiter.Requests, item =>
            item.Endpoint == "/fapi/v1/stock/contract" && item.Weight == 50 && item.Signed);
    }

    [Fact]
    public async Task SignTradFiPerpsAgreementAsync_UsesConfiguredReceiveWindow()
    {
        var handler = new RecordingHttpMessageHandler("""{"code":200,"msg":"success"}""");
        using var client = CreateClient(
            handler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(5_000));

        var result = await client.UsdFutures.SignTradFiPerpsAgreementAsync();

        Assert.True(result.Success);
        Assert.Equal("5000", ParseBody(handler)["recvWindow"]);
    }

    [Fact]
    public async Task SignTradFiPerpsAgreementAsync_RejectsInvalidReceiveWindowsBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using (var client = CreateClient(handler))
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.UsdFutures.SignTradFiPerpsAgreementAsync(60_001));
        }

        using var configuredClient = CreateClient(
            handler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.SignTradFiPerpsAgreementAsync());
        Assert.Null(handler.RequestUri);
    }

    private static Dictionary<string, string> ParseBody(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.Body!).Split('&').Select(value => value.Split('=', 2)).ToDictionary(value => value[0], value => value[1]);

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
