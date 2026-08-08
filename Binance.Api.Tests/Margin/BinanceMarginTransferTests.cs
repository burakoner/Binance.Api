using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Margin;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginTransferTests
{
    [Fact]
    public async Task TransferHistory_UsesCurrentFiltersPaginationAndRawModel()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "rows":[{
                "amount":"0.10000000","asset":"BNB","status":"CONFIRMED","timestamp":1566898617,
                "txId":5240372201,"type":"ROLL_IN","transFrom":"SPOT","transTo":"ISOLATED_MARGIN",
                "fromSymbol":"BNBUSDT","toSymbol":"BTCUSDT"
              }],
              "total":3000000000
            }
            """);
        using var client = CreateClient(handler, limiter);
        var startTime = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        var endTime = startTime.AddDays(30);

        var result = await client.Margin.GetMarginTransfersAsync(
            BinanceMarginTransferDirection.RollIn,
            "BNB",
            startTime,
            endTime,
            current: 3_000_000_000,
            size: 100,
            isolatedSymbol: "BNBUSDT",
            receiveWindow: 5_000);

        Assert.True(result.Success);
        Assert.Equal(3_000_000_000, result.Data.Total);
        var row = Assert.Single(result.Data.Rows);
        Assert.Equal(5_240_372_201, row.TransactionId);
        Assert.Equal(1_566_898_617, row.Timestamp);
        Assert.Equal(BinanceMarginTransferDirection.RollIn, row.Direction);
        Assert.Equal("ISOLATED_MARGIN", row.TransferTo);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/sapi/v1/margin/transfer", handler.RequestUri!.AbsolutePath);
        var query = AssertSignedQuery(handler);
        Assert.Contains("asset=BNB", query);
        Assert.Contains("type=ROLL_IN", query);
        Assert.Contains("startTime=1780272000000", query);
        Assert.Contains("endTime=1782864000000", query);
        Assert.Contains("current=3000000000", query);
        Assert.Contains("size=100", query);
        Assert.Contains("isolatedSymbol=BNBUSDT", query);
        Assert.Contains("recvWindow=5000", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/transfer" && item.Weight == 1);
    }

    [Fact]
    public async Task OptionalHistoryAndMaxTransfer_UseCurrentSignedContracts()
    {
        var historyHandler = new RecordingHttpMessageHandler("""{"rows":[],"total":0}""");
        using (var client = CreateClient(historyHandler))
        {
            var result = await client.Margin.GetMarginTransfersAsync();

            Assert.True(result.Success);
            var query = AssertSignedQuery(historyHandler);
            Assert.DoesNotContain("asset=", query);
            Assert.DoesNotContain("type=", query);
            Assert.DoesNotContain("startTime=", query);
            Assert.DoesNotContain("endTime=", query);
        }

        var amountLimiter = new RecordingRateLimiter();
        var amountHandler = new RecordingHttpMessageHandler("""{"amount":"3.59498107"}""");
        using (var client = CreateClient(amountHandler, amountLimiter))
        {
            var result = await client.Margin.GetMarginMaxTransferAmountAsync("BTC", "BTCUSDT", 5_000);

            Assert.True(result.Success);
            Assert.Equal(3.59498107m, result.Data);
            Assert.Equal("/sapi/v1/margin/maxTransferable", amountHandler.RequestUri!.AbsolutePath);
            var query = AssertSignedQuery(amountHandler);
            Assert.Contains("asset=BTC", query);
            Assert.Contains("isolatedSymbol=BTCUSDT", query);
            Assert.Contains("recvWindow=5000", query);
            Assert.Contains(amountLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/maxTransferable" && item.Weight == 50);
        }
    }

    [Fact]
    public async Task TransferQueries_RejectDocumentedInvalidRequests()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));
        var startTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginTransfersAsync(asset: " "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginTransfersAsync(isolatedSymbol: " "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginTransfersAsync(startTime: startTime.AddDays(1), endTime: startTime));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginTransfersAsync(startTime: startTime, endTime: startTime.AddDays(31)));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginTransfersAsync(current: 0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginTransfersAsync(size: 0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginTransfersAsync(size: 101));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginTransfersAsync(receiveWindow: 60_001));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginMaxTransferAmountAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginMaxTransferAmountAsync("BTC", " "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetMarginMaxTransferAmountAsync("BTC", receiveWindow: 60_001));
    }

    private static string AssertSignedQuery(RecordingHttpMessageHandler handler)
    {
        Assert.Null(handler.Body);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        return query;
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
