using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientOrderModifyHistoryTests
{
    [Fact]
    public async Task UsdOrderModifyHistory_UsesCurrentSignedQueryAndCompleteResponse()
    {
        var startTime = DateTimeOffset.FromUnixTimeMilliseconds(1_623_319_461_670).UtcDateTime;
        var endTime = DateTimeOffset.FromUnixTimeMilliseconds(1_641_782_889_000).UtcDateTime;
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [{
              "amendmentId": 9223372036854775806,
              "symbol": "BTCUSDT",
              "pair": "BTCUSDT",
              "orderId": 9223372036854775805,
              "clientOrderId": "usd-history",
              "time": 1629184560899,
              "amendment": {
                "price": { "before": "30004", "after": "30003.2" },
                "origQty": { "before": "1", "after": "1.5" },
                "count": 3,
                "modifyId": 9007199254740993
              }
            }]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetOrderModifyHistoryAsync(
            "BTCUSDT",
            orderId: 9_223_372_036_854_775_805L,
            origClientOrderId: "usd-history",
            startTime: startTime,
            endTime: endTime,
            limit: 100,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/fapi/v1/orderAmendment");
        var query = DecodedQuery(handler);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("orderId=9223372036854775805", query);
        Assert.Contains("origClientOrderId=usd-history", query);
        Assert.Contains($"startTime={new DateTimeOffset(startTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains($"endTime={new DateTimeOffset(endTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains("limit=100", query);
        Assert.Contains("recvWindow=60000", query);

        var history = Assert.Single(result.Data);
        Assert.Equal(9_223_372_036_854_775_806L, history.AmendmentId);
        Assert.Equal("BTCUSDT", history.Symbol);
        Assert.Equal("BTCUSDT", history.Pair);
        Assert.Equal(9_223_372_036_854_775_805L, history.Id);
        Assert.Equal("usd-history", history.ClientOrderId);
        Assert.Equal("usd-history", history.RequestClientOrderId);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_629_184_560_899).UtcDateTime, history.Timestamp);
        Assert.Equal(30_004m, history.EditInfo.Price.Before);
        Assert.Equal(30_003.2m, history.EditInfo.Price.After);
        Assert.Equal(1m, history.EditInfo.Quantity.Before);
        Assert.Equal(1.5m, history.EditInfo.Quantity.After);
        Assert.Equal(3L, history.EditInfo.EditCount);
        Assert.Equal(9_007_199_254_740_993L, history.EditInfo.ModifyId);
    }

    [Fact]
    public async Task CoinOrderModifyHistory_SupportsOriginalClientIdAndAbsentModifyId()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [{
              "amendmentId": 5363,
              "symbol": "BTCUSD_PERP",
              "pair": "BTCUSD",
              "orderId": 20072994037,
              "clientOrderId": "coin_history",
              "time": 1629184560899,
              "amendment": {
                "price": { "before": "30004", "after": "30003.2" },
                "origQty": { "before": "1", "after": "1" },
                "count": 3
              }
            }]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetOrderModifyHistoryAsync(
            "BTCUSD_PERP",
            origClientOrderId: "coin_history");

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/dapi/v1/orderAmendment");
        var query = DecodedQuery(handler);
        Assert.Contains("symbol=BTCUSD_PERP", query);
        Assert.Contains("origClientOrderId=coin_history", query);
        Assert.DoesNotContain("orderId=", query);
        Assert.DoesNotContain("startTime=", query);
        Assert.DoesNotContain("endTime=", query);
        Assert.DoesNotContain("limit=", query);
        Assert.DoesNotContain("recvWindow=", query);

        var history = Assert.Single(result.Data);
        Assert.Equal(5_363L, history.AmendmentId);
        Assert.Equal("BTCUSD_PERP", history.Symbol);
        Assert.Equal("BTCUSD", history.Pair);
        Assert.Equal(20_072_994_037L, history.Id);
        Assert.Equal("coin_history", history.ClientOrderId);
        Assert.Equal(30_004m, history.EditInfo.Price.Before);
        Assert.Equal(30_003.2m, history.EditInfo.Price.After);
        Assert.Equal(1m, history.EditInfo.Quantity.Before);
        Assert.Equal(1m, history.EditInfo.Quantity.After);
        Assert.Equal(3L, history.EditInfo.EditCount);
        Assert.Null(history.EditInfo.ModifyId);
    }

    [Fact]
    public async Task OrderModifyHistory_RejectsInvalidRequiredParametersAndLimitBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetOrderModifyHistoryAsync(null!, orderId: 1));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetOrderModifyHistoryAsync(" ", orderId: 1));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetOrderModifyHistoryAsync("BTCUSDT"));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetOrderModifyHistoryAsync("BTCUSDT", origClientOrderId: " "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.UsdFutures.GetOrderModifyHistoryAsync("BTCUSDT", orderId: 1, limit: 101));

        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetOrderModifyHistoryAsync(null!, orderId: 1));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetOrderModifyHistoryAsync(" ", orderId: 1));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetOrderModifyHistoryAsync("BTCUSD_PERP"));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetOrderModifyHistoryAsync("BTCUSD_PERP", origClientOrderId: " "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.CoinFutures.GetOrderModifyHistoryAsync("BTCUSD_PERP", orderId: 1, limit: 101));
        Assert.Null(handler.RequestUri);
    }

    [Fact]
    public async Task OrderModifyHistory_RejectsReceiveWindowAboveCurrentMaximum()
    {
        var explicitHandler = new RecordingHttpMessageHandler("[]");
        using (var explicitClient = CreateClient(explicitHandler))
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.UsdFutures.GetOrderModifyHistoryAsync("BTCUSDT", orderId: 1, receiveWindow: 60_001));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.CoinFutures.GetOrderModifyHistoryAsync("BTCUSD_PERP", orderId: 1, receiveWindow: 60_001));
        }
        Assert.Null(explicitHandler.RequestUri);

        var configuredHandler = new RecordingHttpMessageHandler("[]");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.GetOrderModifyHistoryAsync("BTCUSDT", orderId: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.CoinFutures.GetOrderModifyHistoryAsync("BTCUSD_PERP", orderId: 1));
        Assert.Null(configuredHandler.RequestUri);
    }

    private static string DecodedQuery(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.RequestUri!.Query);

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
        Assert.Equal("api-key", Assert.Single(values));
        Assert.Contains("timestamp=", handler.RequestUri.Query);
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == path && request.Weight == 1 && request.Signed);
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
