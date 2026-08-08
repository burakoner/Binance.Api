using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Margin;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginLiquidationTests
{
    [Fact]
    public async Task LoanAndHistory_UseCurrentSignedQueryContracts()
    {
        var loanLimiter = new RecordingRateLimiter();
        var loanHandler = new RecordingHttpMessageHandler("""
            {"asset":"USDC","amount":"1000.00000000","repaidAmount":"300.00000000","remainingAmount":"700.00000000"}
            """);
        using (var client = CreateClient(loanHandler, loanLimiter))
        {
            var result = await client.Margin.GetLiquidationLoanAsync(5_000);

            Assert.True(result.Success);
            Assert.Equal(1_000m, result.Data.TotalAmount);
            Assert.Equal(300m, result.Data.RepaidAmount);
            Assert.Equal(700m, result.Data.RemainingAmount);
            Assert.Equal(HttpMethod.Get, loanHandler.Method);
            Assert.Equal("/sapi/v1/margin/liquidation-loan", loanHandler.RequestUri!.AbsolutePath);
            Assert.Null(loanHandler.Body);
            var query = Uri.UnescapeDataString(loanHandler.RequestUri.Query);
            Assert.Contains("recvWindow=5000", query);
            Assert.Contains("timestamp=", query);
            Assert.Contains("signature=", query);
            Assert.Contains(loanLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/liquidation-loan" && item.Weight == 100);
        }

        var startTime = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc);
        var endTime = startTime.AddDays(89);
        var historyLimiter = new RecordingRateLimiter();
        var historyHandler = new RecordingHttpMessageHandler("""
            {
              "total":2,
              "rows":[{"repayId":12345678,"asset":"USDC","amount":"300.00000000","status":"PENDING","createTime":1714492800000}]
            }
            """);
        using (var client = CreateClient(historyHandler, historyLimiter))
        {
            var result = await client.Margin.GetLiquidationLoanRepaymentHistoryAsync(startTime, endTime, current: 2, size: 25, receiveWindow: 6_000);

            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Total);
            var repayment = Assert.Single(result.Data.Rows);
            Assert.Equal(12_345_678, repayment.RepaymentId);
            Assert.Equal(300m, repayment.Amount);
            Assert.Equal(BinanceMarginLiquidationLoanRepaymentStatus.Pending, repayment.Status);
            Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_714_492_800_000).UtcDateTime, repayment.CreateTime);
            Assert.Equal(HttpMethod.Get, historyHandler.Method);
            Assert.Equal("/sapi/v1/margin/liquidation-loan/repay-history", historyHandler.RequestUri!.AbsolutePath);
            Assert.Null(historyHandler.Body);
            var query = Uri.UnescapeDataString(historyHandler.RequestUri.Query);
            Assert.Contains($"startTime={new DateTimeOffset(startTime).ToUnixTimeMilliseconds()}", query);
            Assert.Contains($"endTime={new DateTimeOffset(endTime).ToUnixTimeMilliseconds()}", query);
            Assert.Contains("current=2", query);
            Assert.Contains("size=25", query);
            Assert.Contains(historyLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/liquidation-loan/repay-history" && item.Weight == 100);
        }
    }

    [Fact]
    public async Task RepayAndManualLiquidation_UseCurrentSignedBodyContracts()
    {
        var repayLimiter = new RecordingRateLimiter();
        var repayHandler = new RecordingHttpMessageHandler("""
            {"repayId":12345678,"asset":"USDC","amount":"300.25000000","status":"SUCCESS","createTime":1714492800000}
            """);
        using (var client = CreateClient(repayHandler, repayLimiter))
        {
            var result = await client.Margin.RepayLiquidationLoanAsync("USDC", 300.25m, 5_000);

            Assert.True(result.Success);
            Assert.Equal(BinanceMarginLiquidationLoanRepaymentStatus.Success, result.Data.Status);
            Assert.Equal(300.25m, result.Data.Amount);
            Assert.Equal(HttpMethod.Post, repayHandler.Method);
            Assert.Equal("/sapi/v1/margin/liquidation-loan/repay", repayHandler.RequestUri!.AbsolutePath);
            var body = Uri.UnescapeDataString(repayHandler.Body!);
            Assert.Contains("asset=USDC", body);
            Assert.Contains("amount=300.25", body);
            Assert.Contains("recvWindow=5000", body);
            Assert.Contains("timestamp=", body);
            Assert.Contains(repayLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/liquidation-loan/repay" && item.Weight == 100);
        }

        var liquidationLimiter = new RecordingRateLimiter();
        var liquidationHandler = new RecordingHttpMessageHandler("""
            {"asset":"ETH","interest":"0.00083334","principal":"0.001","liabilityAsset":"USDT","liabilityQty":0.3552}
            """);
        using (var client = CreateClient(liquidationHandler, liquidationLimiter))
        {
            var result = await client.Margin.LiquidateMarginAccountAsync(
                BinanceMarginLiquidationType.IsolatedMargin,
                "ETHUSDT",
                5_000);

            Assert.True(result.Success);
            Assert.Equal(0.00083334m, result.Data.Interest);
            Assert.Equal(0.001m, result.Data.Principal);
            Assert.Equal(0.3552m, result.Data.LiabilityQuantity);
            Assert.Equal(HttpMethod.Post, liquidationHandler.Method);
            Assert.Equal("/sapi/v1/margin/manual-liquidation", liquidationHandler.RequestUri!.AbsolutePath);
            var body = Uri.UnescapeDataString(liquidationHandler.Body!);
            Assert.Contains("type=ISOLATED", body);
            Assert.Contains("symbol=ETHUSDT", body);
            Assert.Contains("recvWindow=5000", body);
            Assert.Contains(liquidationLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/manual-liquidation" && item.Weight == 3_000);
        }
    }

    [Fact]
    public async Task LiquidationMethods_RejectDocumentedInvalidRequests()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));
        var endTime = DateTime.UtcNow;

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.LiquidateMarginAccountAsync(BinanceMarginLiquidationType.IsolatedMargin));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.LiquidateMarginAccountAsync(BinanceMarginLiquidationType.CrossMargin, " "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.RepayLiquidationLoanAsync("USDC", 0));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetLiquidationLoanRepaymentHistoryAsync(endTime.AddDays(-91), endTime));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetLiquidationLoanRepaymentHistoryAsync(current: 0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetLiquidationLoanRepaymentHistoryAsync(size: 0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetLiquidationLoanAsync(60_001));
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
