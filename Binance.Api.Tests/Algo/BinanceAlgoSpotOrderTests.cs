using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Algo;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Algo;

public class BinanceAlgoSpotOrderTests
{
    private const string ClientAlgoId = "fedcba9876543210fedcba9876543210";

    [Fact]
    public async Task SpotTwapOrder_UsesCurrentSignedBodyAndOmitsReceiveWindow()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"clientAlgoId":"fedcba9876543210fedcba9876543210","success":true,"code":2147483648,"msg":"OK"}""");
        using var client = CreateClient(handler, limiter, TimeSpan.FromMilliseconds(5000));

        var result = await client.Algo.Spot.PlaceTimeWeightedAveragePriceOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            1.25m,
            300,
            clientAlgoId: ClientAlgoId,
            limitPrice: 50_000.5m);

        Assert.True(result.Success);
        Assert.True(result.Data.Success);
        Assert.Equal(2_147_483_648L, result.Data.Code);
        Assert.Equal("OK", result.Data.Message);
        Assert.Equal(ClientAlgoId, result.Data.ClientAlgoId);
        AssertSignedPost(handler, limiter, "/sapi/v1/algo/spot/newOrderTwap", 3000);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("symbol=BTCUSDT", body);
        Assert.Contains("side=BUY", body);
        Assert.Contains("quantity=1.25", body);
        Assert.Contains("duration=300", body);
        Assert.Contains($"clientAlgoId={ClientAlgoId}", body);
        Assert.Contains("limitPrice=50000.5", body);
        Assert.DoesNotContain("recvWindow=", body);
    }

    [Fact]
    public async Task SpotTwapOrder_GeneratesExactClientIdAtMaximumDuration()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""{"success":true,"code":0,"msg":"OK"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Algo.Spot.PlaceTimeWeightedAveragePriceOrderAsync(
            "ETHUSDT",
            BinanceOrderSide.Sell,
            2.5m,
            86_400);

        Assert.True(result.Success);
        Assert.True(result.Data.Success);
        AssertSignedPost(handler, limiter, "/sapi/v1/algo/spot/newOrderTwap", 3000);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("duration=86400", body);
        var clientAlgoId = body.Split('&').Single(parameter => parameter.StartsWith("clientAlgoId=", StringComparison.Ordinal)).Split('=', 2)[1];
        Assert.Equal(32, clientAlgoId.Length);
    }

    [Fact]
    public async Task SpotTwapOrder_RejectsUndocumentedValues()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Algo.Spot.PlaceTimeWeightedAveragePriceOrderAsync(
            " ", BinanceOrderSide.Buy, 1, 300));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Spot.PlaceTimeWeightedAveragePriceOrderAsync(
            "BTCUSDT", (BinanceOrderSide)0, 1, 300));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Spot.PlaceTimeWeightedAveragePriceOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 0, 300));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Spot.PlaceTimeWeightedAveragePriceOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, 299));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Spot.PlaceTimeWeightedAveragePriceOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, 86_401));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Algo.Spot.PlaceTimeWeightedAveragePriceOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, 300, clientAlgoId: string.Empty));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Algo.Spot.PlaceTimeWeightedAveragePriceOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, 300, clientAlgoId: "short"));
    }

    private static void AssertSignedPost(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        string path,
        int weight)
    {
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Equal("application/x-www-form-urlencoded", handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("signature=", query);
        Assert.DoesNotContain("timestamp=", query);
        Assert.DoesNotContain("recvWindow=", query);
        Assert.Contains("timestamp=", handler.Body);
        Assert.DoesNotContain("signature=", handler.Body);
        Assert.Contains(limiter.Requests, item => item.Endpoint == path && item.Weight == weight && item.Signed);
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
