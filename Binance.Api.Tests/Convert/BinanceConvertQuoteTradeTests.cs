using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Convert;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Convert;

public class BinanceConvertQuoteTradeTests
{
    [Fact]
    public async Task QuoteRequest_UsesCurrentSignedBodyAndDeserializesQuote()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"quoteId":"12415572564","ratio":"38163.7","inverseRatio":"0.0000262","validTimestamp":1623319461670,"toAmount":"3816.37","fromAmount":"0.1"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Convert.QuoteRequestAsync(
            "BTC",
            "USDT",
            fromAmount: 0.1m,
            walletType: BinanceConvertWalletType.SpotFundingEarn,
            validTime: BinanceConvertValidTime.OneMinute,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.Equal("12415572564", result.Data.QuoteId);
        Assert.Equal(38_163.7m, result.Data.Ratio);
        Assert.Equal(0.0000262m, result.Data.InverseRatio);
        Assert.Equal(1_623_319_461_670L, result.Data.ValidTimestamp);
        Assert.Equal(3_816.37m, result.Data.ToQuantity);
        Assert.Equal(0.1m, result.Data.FromQuantity);
        AssertSignedPost(handler, limiter, "/sapi/v1/convert/getQuote", 200);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("fromAsset=BTC", body);
        Assert.Contains("toAsset=USDT", body);
        Assert.Contains("fromAmount=0.1", body);
        Assert.DoesNotContain("toAmount=", body);
        Assert.Contains("walletType=SPOT_FUNDING_EARN", body);
        Assert.Contains("validTime=1m", body);
        Assert.Contains("recvWindow=60000", body);
    }

    [Fact]
    public async Task QuoteRequest_RequiresAssetsExactlyOneAmountAndValidReceiveWindow()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.QuoteRequestAsync(" ", "USDT", fromAmount: 1));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.QuoteRequestAsync("BTC", " ", fromAmount: 1));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.QuoteRequestAsync("BTC", "USDT"));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.QuoteRequestAsync("BTC", "USDT", 1, 2));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Convert.QuoteRequestAsync("BTC", "USDT", fromAmount: 1, receiveWindow: 60_001));

        var handler = new RecordingHttpMessageHandler("{}");
        using (var toAmountClient = CreateClient(handler))
            Assert.True((await toAmountClient.Convert.QuoteRequestAsync("BTC", "USDT", toAmount: 2)).Success);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("toAmount=2", body);
        Assert.DoesNotContain("fromAmount=", body);

        using var configuredClient = CreateClient(
            new RecordingHttpMessageHandler("{}"),
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => configuredClient.Convert.QuoteRequestAsync("BTC", "USDT", fromAmount: 1));
    }

    [Fact]
    public async Task AcceptQuote_UsesCurrentSignedBodyAndDeserializesResult()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"orderId":"933256278426274426","createTime":1623381330472,"orderStatus":"PROCESS"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Convert.AcceptQuoteAsync("quote-1", 60_000);

        Assert.True(result.Success);
        Assert.Equal("933256278426274426", result.Data.OrderId);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_623_381_330_472).UtcDateTime, result.Data.CreateTime);
        Assert.Equal(BinanceConvertOrderStatus.Process, result.Data.Status);
        AssertSignedPost(handler, limiter, "/sapi/v1/convert/acceptQuote", 500);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("quoteId=quote-1", body);
        Assert.Contains("recvWindow=60000", body);
    }

    [Fact]
    public async Task AcceptQuote_RejectsEmptyIdentifierOrInvalidReceiveWindow()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));

        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.AcceptQuoteAsync(null!));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Convert.AcceptQuoteAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Convert.AcceptQuoteAsync("quote-1", 60_001));

        using var configuredClient = CreateClient(
            new RecordingHttpMessageHandler("{}"),
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => configuredClient.Convert.AcceptQuoteAsync("quote-1"));
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
