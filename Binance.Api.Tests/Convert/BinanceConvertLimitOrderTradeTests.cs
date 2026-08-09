using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Convert;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Convert;

public class BinanceConvertLimitOrderTradeTests
{
    [Fact]
    public async Task PlaceLimitOrder_UsesCurrentSignedBodyAndResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"orderId":1603680255057330400,"status":"PROCESS"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Convert.PlaceLimitOrderAsync(
            "BTC",
            "USDT",
            38_163.7m,
            BinanceOrderSide.Buy,
            BinanceConvertExpiredTime.OneDay,
            baseAmount: 0.1m,
            walletType: BinanceConvertWalletType.SpotEarn,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.Equal(1_603_680_255_057_330_400L, result.Data.OrderId);
        Assert.Equal("PROCESS", result.Data.Status);
        AssertSignedPost(handler, limiter, "/sapi/v1/convert/limit/placeOrder", 500);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("baseAsset=BTC", body);
        Assert.Contains("quoteAsset=USDT", body);
        Assert.Contains("limitPrice=38163.7", body);
        Assert.Contains("side=BUY", body);
        Assert.Contains("expiredType=1_D", body);
        Assert.Contains("baseAmount=0.1", body);
        Assert.DoesNotContain("quoteAmount=", body);
        Assert.Contains("walletType=SPOT_EARN", body);
        Assert.Contains("recvWindow=60000", body);
    }

    [Fact]
    public async Task PlaceLimitOrder_RequiresAssetsExactlyOneAmountAndValidReceiveWindow()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.PlaceLimitOrderAsync(" ", "USDT", 1, BinanceOrderSide.Buy, BinanceConvertExpiredTime.OneDay, baseAmount: 1));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.PlaceLimitOrderAsync("BTC", " ", 1, BinanceOrderSide.Buy, BinanceConvertExpiredTime.OneDay, baseAmount: 1));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.PlaceLimitOrderAsync("BTC", "USDT", 1, BinanceOrderSide.Buy, BinanceConvertExpiredTime.OneDay));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.PlaceLimitOrderAsync("BTC", "USDT", 1, BinanceOrderSide.Buy, BinanceConvertExpiredTime.OneDay, 1, 2));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Convert.PlaceLimitOrderAsync("BTC", "USDT", 1, BinanceOrderSide.Buy, BinanceConvertExpiredTime.OneDay, baseAmount: 1, receiveWindow: 60_001));

        var handler = new RecordingHttpMessageHandler("{}");
        using (var quoteAmountClient = CreateClient(handler))
            Assert.True((await quoteAmountClient.Convert.PlaceLimitOrderAsync("BTC", "USDT", 1, BinanceOrderSide.Sell, BinanceConvertExpiredTime.ThirtyDays, quoteAmount: 2)).Success);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("quoteAmount=2", body);
        Assert.DoesNotContain("baseAmount=", body);
        Assert.Contains("side=SELL", body);
        Assert.Contains("expiredType=30_D", body);

        using var configuredClient = CreateClient(
            new RecordingHttpMessageHandler("{}"),
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => configuredClient.Convert.PlaceLimitOrderAsync("BTC", "USDT", 1, BinanceOrderSide.Buy, BinanceConvertExpiredTime.OneDay, baseAmount: 1));
    }

    [Fact]
    public async Task CancelLimitOrder_UsesCurrentInt64BodyWeightAndResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"orderId":1603680255057330400,"status":"CANCELED"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Convert.CancelLimitOrderAsync(1_603_680_255_057_330_400L, 60_000);

        Assert.True(result.Success);
        Assert.Equal(1_603_680_255_057_330_400L, result.Data.OrderId);
        Assert.Equal("CANCELED", result.Data.Status);
        AssertSignedPost(handler, limiter, "/sapi/v1/convert/limit/cancelOrder", 200);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("orderId=1603680255057330400", body);
        Assert.Contains("recvWindow=60000", body);
    }

    [Fact]
    public async Task CancelLimitOrder_RejectsInvalidReceiveWindow()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Convert.CancelLimitOrderAsync(1, 60_001));

        using var configuredClient = CreateClient(
            new RecordingHttpMessageHandler("{}"),
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => configuredClient.Convert.CancelLimitOrderAsync(1));
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
        Assert.StartsWith("?signature=", query);
        Assert.DoesNotContain("&", query);
        Assert.DoesNotContain("timestamp=", query);
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
