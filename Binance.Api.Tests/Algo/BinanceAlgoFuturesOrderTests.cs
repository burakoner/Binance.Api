using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Algo;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Algo;

public class BinanceAlgoFuturesOrderTests
{
    private const string ClientAlgoId = "0123456789abcdef0123456789abcdef";

    [Fact]
    public async Task VolumeParticipationOrder_UsesCurrentSignedBodyWeightAndResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"clientAlgoId":"0123456789abcdef0123456789abcdef","success":true,"code":2147483648,"msg":"OK"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Algo.Futures.PlaceVolumeParticipationOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            1.25m,
            BinanceUrgency.High,
            clientAlgoId: ClientAlgoId,
            reduceOnly: false,
            limitPrice: 50_000.5m,
            positionSide: BinancePositionSide.Both,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.True(result.Data.Success);
        Assert.Equal(2_147_483_648L, result.Data.Code);
        Assert.Equal("OK", result.Data.Message);
        Assert.Equal(ClientAlgoId, result.Data.ClientAlgoId);
        AssertSignedPost(handler, limiter, "/sapi/v1/algo/futures/newOrderVp", 300);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("symbol=BTCUSDT", body);
        Assert.Contains("side=BUY", body);
        Assert.Contains("quantity=1.25", body);
        Assert.Contains("urgency=HIGH", body);
        Assert.Contains($"clientAlgoId={ClientAlgoId}", body);
        Assert.Contains("reduceonly=false", body.ToLowerInvariant());
        Assert.Contains("limitPrice=50000.5", body);
        Assert.Contains("positionSide=BOTH", body);
        Assert.Contains("recvWindow=60000", body);
    }

    [Fact]
    public async Task TimeWeightedOrder_UsesCurrentSignedBodyAndGeneratesExactClientId()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""{"success":true,"code":0,"msg":"OK"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Algo.Futures.PlaceTimeWeightedAveragePriceOrderAsync(
            "ETHUSDT",
            BinanceOrderSide.Sell,
            2.5m,
            86_400,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.True(result.Data.Success);
        AssertSignedPost(handler, limiter, "/sapi/v1/algo/futures/newOrderTwap", 3000);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("symbol=ETHUSDT", body);
        Assert.Contains("side=SELL", body);
        Assert.Contains("quantity=2.5", body);
        Assert.Contains("duration=86400", body);
        Assert.Contains("recvWindow=60000", body);
        var clientAlgoId = body.Split('&').Single(parameter => parameter.StartsWith("clientAlgoId=", StringComparison.Ordinal)).Split('=', 2)[1];
        Assert.Equal(32, clientAlgoId.Length);
    }

    [Fact]
    public async Task FuturesNewOrders_RejectUndocumentedValues()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Algo.Futures.PlaceVolumeParticipationOrderAsync(
            " ", BinanceOrderSide.Buy, 1, BinanceUrgency.Low));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Futures.PlaceVolumeParticipationOrderAsync(
            "BTCUSDT", (BinanceOrderSide)0, 1, BinanceUrgency.Low));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Futures.PlaceVolumeParticipationOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 0, BinanceUrgency.Low));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Futures.PlaceVolumeParticipationOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, (BinanceUrgency)0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Futures.PlaceVolumeParticipationOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, BinanceUrgency.Low, positionSide: BinancePositionSide.Hedge));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Algo.Futures.PlaceVolumeParticipationOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, BinanceUrgency.Low, reduceOnly: false, positionSide: BinancePositionSide.Long));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Algo.Futures.PlaceVolumeParticipationOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, BinanceUrgency.Low, clientAlgoId: "short"));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Futures.PlaceVolumeParticipationOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, BinanceUrgency.Low, receiveWindow: 60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Futures.PlaceTimeWeightedAveragePriceOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, 299));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Futures.PlaceTimeWeightedAveragePriceOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, 86_401));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Algo.Futures.PlaceTimeWeightedAveragePriceOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, 300, clientAlgoId: string.Empty));
    }

    [Fact]
    public async Task FuturesNewOrders_RejectInvalidConfiguredReceiveWindow()
    {
        using var client = CreateClient(
            new RecordingHttpMessageHandler("{}"),
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Futures.PlaceVolumeParticipationOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, BinanceUrgency.Low));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Algo.Futures.PlaceTimeWeightedAveragePriceOrderAsync(
            "BTCUSDT", BinanceOrderSide.Buy, 1, 300));
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
