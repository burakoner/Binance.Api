using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Convert;

public class BinanceConvertReadOnlyTradeTests
{
    [Fact]
    public async Task GetHistory_UsesCurrentSignedContractAndDeserializesResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"list":[{"quoteId":"quote-1","orderId":940708407462087200,"orderStatus":"SUCCESS","fromAsset":"USDT","fromAmount":"20","toAsset":"BNB","toAmount":"0.06154036","ratio":"0.00307702","inverseRatio":"324.99","createTime":1624248872184}],"startTime":1623824139000,"endTime":1626416139000,"limit":1000,"moreData":false}""");
        using var client = CreateClient(handler, limiter);
        var startTime = DateTimeOffset.FromUnixTimeMilliseconds(1_623_824_139_000).UtcDateTime;
        var endTime = DateTimeOffset.FromUnixTimeMilliseconds(1_626_416_139_000).UtcDateTime;

        var result = await client.Convert.GetHistoryAsync(startTime, endTime, 1000, 60_000);

        Assert.True(result.Success);
        var trade = Assert.Single(result.Data.List);
        Assert.Equal("quote-1", trade.QuoteId);
        Assert.Equal(940_708_407_462_087_200L, trade.OrderId);
        Assert.Equal("SUCCESS", trade.Status);
        Assert.Equal("USDT", trade.FromAsset);
        Assert.Equal(20m, trade.FromQuantity);
        Assert.Equal("BNB", trade.ToAsset);
        Assert.Equal(0.06154036m, trade.ToQuantity);
        Assert.Equal(0.00307702m, trade.Ratio);
        Assert.Equal(324.99m, trade.InverseRatio);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_624_248_872_184).UtcDateTime, trade.CreateTime);
        Assert.Equal(startTime, result.Data.StartTime);
        Assert.Equal(endTime, result.Data.EndTime);
        Assert.Equal(1000, result.Data.Limit);
        Assert.False(result.Data.MoreData);
        AssertSignedGet(handler, limiter, "/sapi/v1/convert/tradeFlow", 3000);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("startTime=1623824139000", query);
        Assert.Contains("endTime=1626416139000", query);
        Assert.Contains("limit=1000", query);
        Assert.Contains("recvWindow=60000", query);
    }

    [Fact]
    public async Task GetHistory_RejectsInvalidRangeLimitOrReceiveWindow()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));
        var startTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.GetHistoryAsync(startTime, startTime.AddTicks(-1)));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.GetHistoryAsync(startTime, startTime.AddDays(30).AddTicks(1)));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Convert.GetHistoryAsync(startTime, startTime.AddDays(1), 1001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Convert.GetHistoryAsync(startTime, startTime.AddDays(1), receiveWindow: 60_001));
    }

    [Fact]
    public async Task GetStatus_UsesCurrentWeightParametersAndResponseModel()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"orderId":933256278426274400,"orderStatus":"SUCCESS","fromAsset":"BTC","fromAmount":"0.00054414","toAsset":"USDT","toAmount":"20","ratio":"36755","inverseRatio":"0.00002721","createTime":1623381330472}""");
        using var client = CreateClient(handler, limiter, TimeSpan.FromMilliseconds(5000));

        var result = await client.Convert.GetStatusAsync("order-1");

        Assert.True(result.Success);
        Assert.Equal(933_256_278_426_274_400L, result.Data.OrderId);
        Assert.Equal("SUCCESS", result.Data.Status);
        Assert.Equal("BTC", result.Data.FromAsset);
        Assert.Equal(0.00054414m, result.Data.FromQuantity);
        Assert.Equal("USDT", result.Data.ToAsset);
        Assert.Equal(20m, result.Data.ToQuantity);
        Assert.Equal(36_755m, result.Data.Ratio);
        Assert.Equal(0.00002721m, result.Data.InverseRatio);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_623_381_330_472).UtcDateTime, result.Data.CreateTime);
        AssertSignedGet(handler, limiter, "/sapi/v1/convert/orderStatus", 100);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("orderId=order-1", query);
        Assert.DoesNotContain("quoteId=", query);
        Assert.DoesNotContain("recvWindow=", query);
    }

    [Fact]
    public async Task GetStatus_RequiresExactlyOneNonEmptyIdentifier()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.GetStatusAsync());
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.GetStatusAsync(" ", "quote-1"));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.GetStatusAsync("order-1", " "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.GetStatusAsync("order-1", "quote-1"));

        var handler = new RecordingHttpMessageHandler("{}");
        using var quoteClient = CreateClient(handler);
        Assert.True((await quoteClient.Convert.GetStatusAsync(quoteId: "quote-1")).Success);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("quoteId=quote-1", query);
        Assert.DoesNotContain("orderId=", query);
    }

    [Fact]
    public async Task GetOpenLimitOrders_UsesDedicatedCurrentResponseModel()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"list":[{"quoteId":"quote-2","orderId":1603680255057330400,"orderStatus":"PROCESS","fromAsset":"BNB","fromAmount":"10","toAsset":"USDT","toAmount":"2317.89","ratio":"231.789","inverseRatio":"0.00431427","createTime":1614089498000,"expiredTimestamp":1614099498000}]}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Convert.GetOpenLimitOrdersAsync(60_000);

        Assert.True(result.Success);
        var order = Assert.Single(result.Data);
        Assert.Equal("quote-2", order.QuoteId);
        Assert.Equal(1_603_680_255_057_330_400L, order.OrderId);
        Assert.Equal("PROCESS", order.Status);
        Assert.Equal("BNB", order.FromAsset);
        Assert.Equal(10m, order.FromQuantity);
        Assert.Equal("USDT", order.ToAsset);
        Assert.Equal(2317.89m, order.ToQuantity);
        Assert.Equal(231.789m, order.Ratio);
        Assert.Equal(0.00431427m, order.InverseRatio);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_614_089_498_000).UtcDateTime, order.CreateTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_614_099_498_000).UtcDateTime, order.ExpireTime);
        AssertSignedGet(handler, limiter, "/sapi/v1/convert/limit/queryOpenOrders", 3000);
        Assert.Contains("recvWindow=60000", Uri.UnescapeDataString(handler.RequestUri!.Query));
    }

    [Fact]
    public async Task GetOpenLimitOrders_RejectsExplicitOrConfiguredReceiveWindowAboveMaximum()
    {
        using (var client = CreateClient(new RecordingHttpMessageHandler("{}")))
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Convert.GetOpenLimitOrdersAsync(60_001));

        using var configuredClient = CreateClient(
            new RecordingHttpMessageHandler("{}"),
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => configuredClient.Convert.GetOpenLimitOrdersAsync());
    }

    private static void AssertSignedGet(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        string path,
        int weight)
    {
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
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
