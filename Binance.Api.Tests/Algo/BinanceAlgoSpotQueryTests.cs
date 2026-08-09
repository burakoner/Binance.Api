using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Algo;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Algo;

public class BinanceAlgoSpotQueryTests
{
    [Fact]
    public async Task GetOpenAlgoOrders_UsesCurrentSignedQueryWeightAndResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"total":2147483648,"orders":[{"algoId":9223372036854775806,"symbol":"ETHUSDT","side":"SELL","totalQty":"5.000","executedQty":"0.000","executedAmt":"0.00000000","avgPrice":"0.00","clientAlgoId":"d7096549481642f8a0bb69e9e2e31f2e","bookTime":1649756817004,"endTime":0,"algoStatus":"WORKING","algoType":"VP","urgency":"LOW"}]}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Algo.Spot.GetOpenAlgoOrdersAsync(60_000);

        Assert.True(result.Success);
        Assert.Equal(2_147_483_648L, result.Data.Total);
        var order = Assert.Single(result.Data.Orders);
        Assert.Equal(9_223_372_036_854_775_806L, order.AlgoId);
        Assert.Equal(BinanceOrderSide.Sell, order.Side);
        Assert.Null(order.PositionSide);
        Assert.Equal(BinanceAlgoType.VP, order.Type);
        Assert.Equal(BinanceUrgency.Low, order.Urgency);
        AssertSignedGet(handler, limiter, "/sapi/v1/algo/spot/openOrders");
        Assert.Contains("recvWindow=60000", DecodedQuery(handler));
    }

    [Fact]
    public async Task GetHistoricalAlgoOrders_UsesCurrentFiltersPaginationAndResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""{"total":2147483648,"orders":[]}""");
        using var client = CreateClient(handler, limiter);
        var startTime = DateTimeOffset.FromUnixTimeMilliseconds(1_623_319_461_670).UtcDateTime;
        var endTime = DateTimeOffset.FromUnixTimeMilliseconds(1_641_782_889_000).UtcDateTime;

        var result = await client.Algo.Spot.GetHistoricalAlgoOrdersAsync(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            startTime,
            endTime,
            page: 2_147_483_648L,
            pageSize: 100,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.Equal(2_147_483_648L, result.Data.Total);
        Assert.Empty(result.Data.Orders);
        AssertSignedGet(handler, limiter, "/sapi/v1/algo/spot/historicalOrders");
        var query = DecodedQuery(handler);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("side=BUY", query);
        Assert.Contains("startTime=1623319461670", query);
        Assert.Contains("endTime=1641782889000", query);
        Assert.Contains("page=2147483648", query);
        Assert.Contains("pageSize=100", query);
        Assert.Contains("recvWindow=60000", query);
    }

    [Fact]
    public async Task GetAlgoSubOrders_UsesCurrentPaginationAndResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"total":2147483648,"executedQty":"1.000","executedAmt":"3229.44000000","subOrders":[{"algoId":13723,"orderId":8389765519993909000,"orderStatus":"FILLED","executedQty":"1.000","executedAmt":"3229.44000000","feeAmt":"-1.61471999","feeAsset":"USDT","bookTime":1649319001964,"avgPrice":"3229.44","side":"SELL","symbol":"ETHUSDT","subId":2147483648,"timeInForce":"IMMEDIATE_OR_CANCEL","origQty":"1.000"}]}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Algo.Spot.GetAlgoSubOrdersAsync(
            9_223_372_036_854_775_806L,
            page: 2_147_483_648L,
            pageSize: 100,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.Equal(2_147_483_648L, result.Data.Total);
        Assert.Equal(1m, result.Data.ExecutedQuantity);
        var subOrder = Assert.Single(result.Data.SubOrders);
        Assert.Equal(8_389_765_519_993_909_000L, subOrder.OrderId);
        Assert.Equal(2_147_483_648L, subOrder.SubId);
        Assert.Equal("IMMEDIATE_OR_CANCEL", subOrder.TimeInForce);
        Assert.Equal(BinanceOrderStatus.Filled, subOrder.Status);
        AssertSignedGet(handler, limiter, "/sapi/v1/algo/spot/subOrders");
        var query = DecodedQuery(handler);
        Assert.Contains("algoId=9223372036854775806", query);
        Assert.Contains("page=2147483648", query);
        Assert.Contains("pageSize=100", query);
        Assert.Contains("recvWindow=60000", query);
    }

    [Fact]
    public async Task SpotQueries_RejectDocumentedInvalidValues()
    {
        using (var client = CreateClient(new RecordingHttpMessageHandler("{}")))
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                client.Algo.Spot.GetAlgoSubOrdersAsync(1, pageSize: 0));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                client.Algo.Spot.GetAlgoSubOrdersAsync(1, pageSize: 101));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                client.Algo.Spot.GetHistoricalAlgoOrdersAsync(symbol: " "));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.Algo.Spot.GetHistoricalAlgoOrdersAsync(side: (BinanceOrderSide)0));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                client.Algo.Spot.GetHistoricalAlgoOrdersAsync(pageSize: 0));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                client.Algo.Spot.GetHistoricalAlgoOrdersAsync(pageSize: 101));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.Algo.Spot.GetOpenAlgoOrdersAsync(60_001));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.Algo.Spot.GetHistoricalAlgoOrdersAsync(receiveWindow: 60_001));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                client.Algo.Spot.GetAlgoSubOrdersAsync(1, receiveWindow: 60_001));
        }

        using var configuredClient = CreateClient(
            new RecordingHttpMessageHandler("{}"),
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.Algo.Spot.GetOpenAlgoOrdersAsync());
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.Algo.Spot.GetHistoricalAlgoOrdersAsync());
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.Algo.Spot.GetAlgoSubOrdersAsync(1));
    }

    private static void AssertSignedGet(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        string path)
    {
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = DecodedQuery(handler);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == path && item.Weight == 1 && item.Signed);
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
