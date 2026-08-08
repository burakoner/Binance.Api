using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Margin;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginSpecialKeyTests
{
    [Fact]
    public async Task CreateSpecialKey_UsesSignedFormContractAndProtectsLoggedKeyMaterial()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""
            {"apiKey":"issued-api-key","secretKey":"issued-secret","type":"HMAC_SHA256"}
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.Margin.CreateMarginSpecialKeyAsync(
            "low-latency",
            "BTCUSDT",
            ["192.0.2.1", "198.51.100.2"],
            "PUBLIC KEY",
            BinanceMarginSpecialKeyPermissionMode.Read,
            5_000);

        Assert.True(result.Success);
        Assert.Equal("issued-api-key", result.Data.ApiKey);
        Assert.Equal("issued-secret", result.Data.SecretKey);
        Assert.Equal("HMAC_SHA256", result.Data.Type);
        Assert.DoesNotContain("issued-api-key", result.Data.ToString());
        Assert.DoesNotContain("issued-secret", result.Data.ToString());
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal("/sapi/v1/margin/apiKey", handler.RequestUri!.AbsolutePath);
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.Equal("application/x-www-form-urlencoded", handler.ContentType);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("apiName=low-latency", body);
        Assert.Contains("symbol=BTCUSDT", body);
        Assert.Contains("ip=192.0.2.1,198.51.100.2", body);
        Assert.Contains("publicKey=PUBLIC+KEY", handler.Body!);
        Assert.Contains("permissionMode=READ", body);
        Assert.Contains("recvWindow=5000", body);
        Assert.Contains("timestamp=", body);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/apiKey" && item.Weight == 1);
    }

    [Fact]
    public async Task QuerySpecialKeys_UsesDocumentedIdentifiersAndModels()
    {
        var limiter = new RecordingRateLimiter();
        var detailHandler = new RecordingHttpMessageHandler("""
            {"apiKey":"target-key","ip":"0.0.0.0,192.0.2.1","apiName":"low-latency","type":"RSA","permissionMode":"TRADE"}
            """);
        using (var client = CreateClient(detailHandler, limiter))
        {
            var result = await client.Margin.GetMarginSpecialKeyAsync("target-key", "BTCUSDT", 5_000);

            Assert.True(result.Success);
            Assert.Equal("low-latency", result.Data.ApiName);
            Assert.Equal("0.0.0.0,192.0.2.1", result.Data.IpAddresses);
            Assert.DoesNotContain("target-key", result.Data.ToString());
            Assert.Equal(HttpMethod.Get, detailHandler.Method);
            Assert.Equal("/sapi/v1/margin/apiKey", detailHandler.RequestUri!.AbsolutePath);
            Assert.Null(detailHandler.Body);
            var query = Uri.UnescapeDataString(detailHandler.RequestUri.Query);
            Assert.Contains("apiKey=target-key", query);
            Assert.Contains("symbol=BTCUSDT", query);
            Assert.Contains("recvWindow=5000", query);
            Assert.Contains("signature=", query);
        }

        var listHandler = new RecordingHttpMessageHandler("""
            [{"apiName":"first","apiKey":"key-one","ip":"192.0.2.1","type":"ED25519","permissionMode":"READ"}]
            """);
        using (var client = CreateClient(listHandler, limiter))
        {
            var result = await client.Margin.GetMarginSpecialKeysAsync("BTCUSDT", 5_000);

            Assert.True(result.Success);
            Assert.Equal("ED25519", Assert.Single(result.Data).Type);
            Assert.Equal("/sapi/v1/margin/api-key-list", listHandler.RequestUri!.AbsolutePath);
            var query = Uri.UnescapeDataString(listHandler.RequestUri.Query);
            Assert.Contains("symbol=BTCUSDT", query);
            Assert.DoesNotContain("apiKey=", query);
        }

        Assert.Equal(2, limiter.Requests.Count(item => item.Weight == 1));
    }

    [Fact]
    public async Task DeleteUpdateAndExitSpecialKey_UseCurrentParameterLocationsAndWeights()
    {
        var limiter = new RecordingRateLimiter();
        var deleteHandler = new RecordingHttpMessageHandler("{}");
        using (var client = CreateClient(deleteHandler, limiter))
        {
            var result = await client.Margin.DeleteMarginSpecialKeyAsync("target-key", "ignored-name", "BTCUSDT", 5_000);

            Assert.True(result.Success);
            Assert.True(result.Data);
            Assert.Equal(HttpMethod.Delete, deleteHandler.Method);
            Assert.Equal("/sapi/v1/margin/apiKey", deleteHandler.RequestUri!.AbsolutePath);
            Assert.Null(deleteHandler.Body);
            var query = Uri.UnescapeDataString(deleteHandler.RequestUri.Query);
            Assert.Contains("apiKey=target-key", query);
            Assert.Contains("apiName=ignored-name", query);
            Assert.Contains("symbol=BTCUSDT", query);
            Assert.Contains("signature=", query);
        }

        var updateHandler = new RecordingHttpMessageHandler("{}");
        using (var client = CreateClient(updateHandler, limiter))
        {
            var result = await client.Margin.UpdateMarginSpecialKeyIpAsync(
                "target-key",
                ["192.0.2.1", "198.51.100.2"],
                "BTCUSDT",
                5_000);

            Assert.True(result.Success);
            Assert.True(result.Data);
            Assert.Equal(HttpMethod.Put, updateHandler.Method);
            Assert.Equal("/sapi/v1/margin/apiKey/ip", updateHandler.RequestUri!.AbsolutePath);
            Assert.Contains("signature=", updateHandler.RequestUri.Query);
            var body = Uri.UnescapeDataString(updateHandler.Body!);
            Assert.Contains("apiKey=target-key", body);
            Assert.Contains("ip=192.0.2.1,198.51.100.2", body);
            Assert.Contains("symbol=BTCUSDT", body);
            Assert.Contains("recvWindow=5000", body);
            Assert.Contains("timestamp=", body);
        }

        var exitHandler = new RecordingHttpMessageHandler("{}");
        using (var client = CreateClient(exitHandler, limiter))
        {
            var result = await client.Margin.ExitMarginSpecialKeyModeAsync(5_000);

            Assert.True(result.Success);
            Assert.True(result.Data);
            Assert.Equal(HttpMethod.Post, exitHandler.Method);
            Assert.Equal("/sapi/v1/margin/exit-special-key-mode", exitHandler.RequestUri!.AbsolutePath);
            Assert.Contains("signature=", exitHandler.RequestUri.Query);
            var body = Uri.UnescapeDataString(exitHandler.Body!);
            Assert.Contains("recvWindow=5000", body);
            Assert.Contains("timestamp=", body);
        }

        Assert.Equal(2, limiter.Requests.Count(item => item.Weight == 1));
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/exit-special-key-mode" && item.Weight == 10);
    }

    [Fact]
    public async Task SpecialKeyMethods_RejectUnsafeOrUndocumentedRequests()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.CreateMarginSpecialKeyAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.CreateMarginSpecialKeyAsync("name", ipAddresses: []));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.CreateMarginSpecialKeyAsync("name", ipAddresses: Enumerable.Repeat("192.0.2.1", 31)));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.CreateMarginSpecialKeyAsync("name", ipAddresses: ["192.0.2.1,198.51.100.2"]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.DeleteMarginSpecialKeyAsync());
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.DeleteMarginSpecialKeyAsync(apiKey: " "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginSpecialKeyAsync(" "));
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.Margin.UpdateMarginSpecialKeyIpAsync("target-key", null!));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.ExitMarginSpecialKeyModeAsync(60_001));
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
