using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Convert;

public class BinanceConvertMarketDataTests
{
    [Fact]
    public async Task GetPairs_UsesCurrentPublicContractAndDeserializesLimits()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """[{"fromAsset":"BTC","toAsset":"USDT","fromAssetMinAmount":"0.0004","fromAssetMaxAmount":"50","toAssetMinAmount":"20","toAssetMaxAmount":"9E+24"}]""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Convert.GetPairsAsync("BTC");

        Assert.True(result.Success);
        var pair = Assert.Single(result.Data);
        Assert.Equal("BTC", pair.FromAsset);
        Assert.Equal("USDT", pair.ToAsset);
        Assert.Equal(0.0004m, pair.FromAssetMinimumAmount);
        Assert.Equal(9E+24m, pair.ToAssetMaximumAmount);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/sapi/v1/convert/exchangeInfo", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("fromAsset=BTC", query);
        Assert.DoesNotContain("toAsset=", query);
        Assert.DoesNotContain("timestamp=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/convert/exchangeInfo" && item.Weight == 3000 && !item.Signed);
    }

    [Fact]
    public async Task GetPairs_RequiresAtLeastOneNonEmptyAssetFilter()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("[]"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.GetPairsAsync());
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.GetPairsAsync(" ", "USDT"));
    }

    [Fact]
    public async Task GetAssets_UsesCurrentSignedContractAndInt64Fraction()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""[{"asset":"BTC","fraction":2147483648}]""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Convert.GetAssetsAsync(60_000);

        Assert.True(result.Success);
        var asset = Assert.Single(result.Data);
        Assert.Equal("BTC", asset.Asset);
        Assert.Equal(2_147_483_648L, asset.Fraction);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/sapi/v1/convert/assetInfo", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("recvWindow=60000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/convert/assetInfo" && item.Weight == 100 && item.Signed);
    }

    [Fact]
    public async Task GetAssets_RejectsExplicitOrConfiguredReceiveWindowAboveMaximum()
    {
        using (var client = CreateClient(new RecordingHttpMessageHandler("[]")))
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Convert.GetAssetsAsync(60_001));

        using var configuredClient = CreateClient(
            new RecordingHttpMessageHandler("[]"),
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => configuredClient.Convert.GetAssetsAsync());
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
