using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Margin;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginCapitalFlowTests
{
    [Fact]
    public async Task CapitalFlow_UsesCurrentQueryAndResponseContract()
    {
        var startTime = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc);
        var endTime = startTime.AddDays(7);
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [{
              "id":3000000001,"tranId":3000000002,"timestamp":1691116657000,
              "asset":"USDT","symbol":"BTCUSDT","type":"COMMISSION_RETURN",
              "amount":"101.25000000","note":"INSTITUTIONAL_LOAN_REPAY"
            }]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.Margin.GetMarginCapitalFlowAsync(
            "USDT",
            "BTCUSDT",
            BinanceMarginCapitalFlowType.CommissionReturn,
            startTime,
            endTime,
            fromId: 3_000_000_000,
            limit: 1_000,
            receiveWindow: 5_000);

        Assert.True(result.Success);
        var row = Assert.Single(result.Data);
        Assert.Equal(3_000_000_001, row.Id);
        Assert.Equal(3_000_000_002, row.TransactionId);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_691_116_657_000).UtcDateTime, row.Timestamp);
        Assert.Equal(BinanceMarginCapitalFlowType.CommissionReturn, row.Type);
        Assert.Equal(101.25m, row.Amount);
        Assert.Equal(BinanceMarginCapitalFlowNote.InstitutionalLoanRepay, row.Note);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/sapi/v1/margin/capital-flow", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("asset=USDT", query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("type=COMMISSION_RETURN", query);
        Assert.Contains($"startTime={new DateTimeOffset(startTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains($"endTime={new DateTimeOffset(endTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains("fromId=3000000000", query);
        Assert.Contains("limit=1000", query);
        Assert.Contains("recvWindow=5000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/capital-flow" && item.Weight == 100);
    }

    [Fact]
    public async Task CapitalFlow_DoesNotInventOptionalFilters()
    {
        var handler = new RecordingHttpMessageHandler(
            """[{"id":1,"tranId":2,"timestamp":1691116657000,"asset":"USDT","type":"BORROW","amount":"10"}]""");
        using var client = CreateClient(handler);

        var result = await client.Margin.GetMarginCapitalFlowAsync();

        Assert.True(result.Success);
        var row = Assert.Single(result.Data);
        Assert.Equal(BinanceMarginCapitalFlowType.Borrow, row.Type);
        Assert.Null(row.Symbol);
        Assert.Null(row.Note);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.DoesNotContain("asset=", query);
        Assert.DoesNotContain("symbol=", query);
        Assert.DoesNotContain("type=", query);
        Assert.DoesNotContain("startTime=", query);
        Assert.DoesNotContain("endTime=", query);
        Assert.DoesNotContain("fromId=", query);
        Assert.DoesNotContain("limit=", query);
        Assert.DoesNotContain("recvWindow=", query);
    }

    [Fact]
    public async Task CapitalFlow_RejectsKnownInvalidRequests()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("[]"));
        var endTime = DateTime.UtcNow;

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginCapitalFlowAsync(startTime: endTime.AddDays(-8), endTime: endTime));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginCapitalFlowAsync(startTime: endTime, endTime: endTime.AddDays(-1)));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginCapitalFlowAsync(asset: " "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginCapitalFlowAsync(symbol: " "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginCapitalFlowAsync(limit: 1_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginCapitalFlowAsync(receiveWindow: 60_001));
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
