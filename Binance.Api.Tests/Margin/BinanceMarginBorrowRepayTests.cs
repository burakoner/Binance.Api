using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Margin;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginBorrowRepayTests
{
    [Fact]
    public async Task HourlyRateAndInterestHistory_UseCurrentQueryContracts()
    {
        var hourlyLimiter = new RecordingRateLimiter();
        var hourlyHandler = new RecordingHttpMessageHandler(
            """[{"asset":"BTC","nextHourlyInterestRate":"0.00000500"}]""");
        using (var client = CreateClient(hourlyHandler, hourlyLimiter))
        {
            var result = await client.Margin.GetFutureHourlyInterestRateAsync(["BTC", "ETH"], true);

            Assert.True(result.Success);
            Assert.Equal(0.000005m, Assert.Single(result.Data).NextHourlyInterestRate);
            Assert.Equal(HttpMethod.Get, hourlyHandler.Method);
            Assert.Equal("/sapi/v1/margin/next-hourly-interest-rate", hourlyHandler.RequestUri!.AbsolutePath);
            Assert.Null(hourlyHandler.Body);
            var query = Uri.UnescapeDataString(hourlyHandler.RequestUri.Query);
            Assert.Contains("assets=BTC,ETH", query);
            Assert.Contains("isIsolated=TRUE", query);
            Assert.DoesNotContain("recvWindow", query);
            Assert.Contains(hourlyLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/next-hourly-interest-rate" && item.Weight == 100);
        }

        var startTime = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc);
        var endTime = startTime.AddDays(30);
        var historyLimiter = new RecordingRateLimiter();
        var historyHandler = new RecordingHttpMessageHandler(
            """
            {
              "total":3000000000,
              "rows":[{
                "txId":123,"interestAccuredTime":1714492800000,"asset":"USDT","rawAsset":"USDT",
                "principal":"100.00000000","interest":"0.01000000","interestRate":"0.00010000",
                "type":"ON_BORROW","isolatedSymbol":"BTCUSDT"
              }]
            }
            """);
        using (var client = CreateClient(historyHandler, historyLimiter))
        {
            var result = await client.Margin.GetMarginInterestHistoryAsync(
                "USDT",
                "BTCUSDT",
                startTime,
                endTime,
                current: 2,
                size: 100,
                receiveWindow: 5_000);

            Assert.True(result.Success);
            Assert.Equal(3_000_000_000L, result.Data.Total);
            var row = Assert.Single(result.Data.Rows);
            Assert.Equal(123, row.TransactionId);
            Assert.Equal(BinanceMarginInterestType.OnBorrow, row.Type);
            Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_714_492_800_000).UtcDateTime, row.InterestAccruedTime);
            Assert.Equal(0.01m, row.InterestQuantity);
            Assert.Equal("/sapi/v1/margin/interestHistory", historyHandler.RequestUri!.AbsolutePath);
            Assert.Null(historyHandler.Body);
            var query = Uri.UnescapeDataString(historyHandler.RequestUri.Query);
            Assert.Contains("asset=USDT", query);
            Assert.Contains("isolatedSymbol=BTCUSDT", query);
            Assert.Contains("current=2", query);
            Assert.Contains("size=100", query);
            Assert.DoesNotContain("archived", query);
            Assert.Contains(historyLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/interestHistory" && item.Weight == 1);
        }
    }

    [Fact]
    public async Task BorrowAndRepay_UseRequiredBodyFieldsAndCurrentWeight()
    {
        var borrowLimiter = new RecordingRateLimiter();
        var borrowHandler = new RecordingHttpMessageHandler("""{"tranId":1001}""");
        using (var client = CreateClient(borrowHandler, borrowLimiter))
        {
            var result = await client.Margin.BorrowAsync("USDT", 250.5m, receiveWindow: 5_000);

            Assert.True(result.Success);
            Assert.Equal(1_001, result.Data.TransactionId);
            Assert.Equal(HttpMethod.Post, borrowHandler.Method);
            Assert.Equal("/sapi/v1/margin/borrow-repay", borrowHandler.RequestUri!.AbsolutePath);
            var body = Uri.UnescapeDataString(borrowHandler.Body!);
            Assert.Contains("asset=USDT", body);
            Assert.Contains("isIsolated=FALSE", body);
            Assert.Contains("amount=250.5", body);
            Assert.Contains("type=BORROW", body);
            Assert.Contains("recvWindow=5000", body);
            Assert.Contains(borrowLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/borrow-repay" && item.Weight == 1_500);
        }

        var repayLimiter = new RecordingRateLimiter();
        var repayHandler = new RecordingHttpMessageHandler("""{"tranId":1002}""");
        using (var client = CreateClient(repayHandler, repayLimiter))
        {
            var result = await client.Margin.RepayAsync("USDT", 100m, true, "BTCUSDT");

            Assert.True(result.Success);
            Assert.Equal(1_002, result.Data.TransactionId);
            var body = Uri.UnescapeDataString(repayHandler.Body!);
            Assert.Contains("isIsolated=TRUE", body);
            Assert.Contains("symbol=BTCUSDT", body);
            Assert.Contains("type=REPAY", body);
            Assert.Contains(repayLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/borrow-repay" && item.Weight == 1_500);
        }
    }

    [Fact]
    public async Task BorrowRepayHistory_SupportsBothTypesWithoutInventedDates()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "total":3000000001,
              "rows":[{
                "type":"AUTO","isolatedSymbol":"BTCUSDT","amount":"25.00000000","asset":"USDT",
                "interest":"0.01000000","principal":"24.99000000","status":"CONFIRMED",
                "timestamp":1714492800000,"txId":987654321
              }]
            }
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.Margin.GetMarginBorrowRepayHistoryAsync(BinanceMarginBorrowRepayType.Repay);

        Assert.True(result.Success);
        Assert.Equal(3_000_000_001L, result.Data.Total);
        var row = Assert.Single(result.Data.Rows);
        Assert.Equal("AUTO", row.ActionType);
        Assert.Equal(BinanceMarginStatus.Confirmed, row.Status);
        Assert.Equal(987_654_321, row.TransactionId);
        Assert.Equal("/sapi/v1/margin/borrow-repay", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("type=REPAY", query);
        Assert.DoesNotContain("startTime", query);
        Assert.DoesNotContain("endTime", query);
        Assert.DoesNotContain("asset=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/borrow-repay" && item.Weight == 10);
    }

    [Fact]
    public async Task RateHistoryAndMaximumBorrow_UseCurrentContracts()
    {
        var startTime = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc);
        var rateLimiter = new RecordingRateLimiter();
        var rateHandler = new RecordingHttpMessageHandler(
            """[{"asset":"USDT","dailyInterestRate":"0.00020000","timestamp":1714492800000,"vipLevel":3}]""");
        using (var client = CreateClient(rateHandler, rateLimiter))
        {
            var result = await client.Margin.GetMarginInterestRateHistoryAsync(
                "USDT",
                vipLevel: 3,
                startTime: startTime,
                endTime: startTime.AddDays(29));

            Assert.True(result.Success);
            var row = Assert.Single(result.Data);
            Assert.Equal(3, row.VipLevel);
            Assert.Equal(0.0002m, row.DailyInterest);
            var query = Uri.UnescapeDataString(rateHandler.RequestUri!.Query);
            Assert.Contains("asset=USDT", query);
            Assert.Contains("vipLevel=3", query);
            Assert.DoesNotContain("limit=", query);
            Assert.Contains(rateLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/interestRateHistory" && item.Weight == 1);
        }

        var maxLimiter = new RecordingRateLimiter();
        var maxHandler = new RecordingHttpMessageHandler(
            """{"amount":"1000.50000000","borrowLimit":"5000.00000000"}""");
        using (var client = CreateClient(maxHandler, maxLimiter))
        {
            var result = await client.Margin.GetMarginMaxBorrowAmountAsync("USDT", "BTCUSDT", 5_000);

            Assert.True(result.Success);
            Assert.Equal(1_000.5m, result.Data.Quantity);
            Assert.Equal(5_000m, result.Data.BorrowLimit);
            Assert.Equal("/sapi/v1/margin/maxBorrowable", maxHandler.RequestUri!.AbsolutePath);
            var query = Uri.UnescapeDataString(maxHandler.RequestUri.Query);
            Assert.Contains("isolatedSymbol=BTCUSDT", query);
            Assert.Contains("recvWindow=5000", query);
            Assert.Contains(maxLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/maxBorrowable" && item.Weight == 750);
        }
    }

    [Fact]
    public async Task BorrowRepayMethods_RejectDocumentedInvalidRequests()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));
        var endTime = DateTime.UtcNow;

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetFutureHourlyInterestRateAsync([], false));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetFutureHourlyInterestRateAsync(Enumerable.Repeat("BTC", 21), false));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.BorrowAsync("USDT", 0));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.BorrowAsync("USDT", 1, true));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.RepayAsync("USDT", 1, false, "BTCUSDT"));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginInterestHistoryAsync(startTime: endTime.AddDays(-31), endTime: endTime));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginBorrowRepayHistoryAsync(BinanceMarginBorrowRepayType.Borrow, startTime: endTime.AddDays(-8), endTime: endTime));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginBorrowRepayHistoryAsync(BinanceMarginBorrowRepayType.Borrow, "USDT", startTime: endTime.AddDays(-31), endTime: endTime));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginInterestRateHistoryAsync("USDT", startTime: endTime.AddDays(-31), endTime: endTime));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginInterestHistoryAsync(current: 0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginInterestHistoryAsync(size: 101));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginMaxBorrowAmountAsync("USDT", receiveWindow: 60_001));
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
