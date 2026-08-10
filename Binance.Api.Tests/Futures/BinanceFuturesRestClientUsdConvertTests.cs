using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdConvertTests
{
    [Fact]
    public async Task GetConvertSymbolsAsync_UsesCurrentPublicContractAndDeserializesLimits()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "fromAsset": "BTC",
                "toAsset": "USDT",
                "fromAssetMinAmount": "0.00000001",
                "fromAssetMaxAmount": "50.12345678",
                "toAssetMinAmount": "20.00000001",
                "toAssetMaxAmount": "2500000.12345678"
              }
            ]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetConvertSymbolsAsync("BTC", "USDT");

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/convert/exchangeInfo", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);

        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("fromAsset=BTC", query);
        Assert.Contains("toAsset=USDT", query);
        Assert.DoesNotContain("timestamp=", query);
        Assert.DoesNotContain("signature=", query);
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == "/fapi/v1/convert/exchangeInfo" && request.Weight == 20 && !request.Signed);

        var pair = Assert.Single(result.Data!);
        Assert.Equal("BTC", pair.FromAsset);
        Assert.Equal("USDT", pair.ToAsset);
        Assert.Equal(0.00000001m, pair.FromAssetMinQuantity);
        Assert.Equal(50.12345678m, pair.FromAssetMaxQuantity);
        Assert.Equal(20.00000001m, pair.ToAssetMinQuantity);
        Assert.Equal(2500000.12345678m, pair.ToAssetMaxQuantity);
    }

    [Fact]
    public async Task ConvertQuoteRequestAsync_ForwardsCurrentSignedBodyAndDeserializesQuote()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "quoteId": "12415572564",
              "ratio": "38163.70000001",
              "inverseRatio": "0.00002620",
              "validTimestamp": 1623319461670,
              "toAmount": "3816.37000001",
              "fromAmount": "0.10000001"
            }
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.ConvertQuoteRequestAsync(
            "BTC",
            "USDT",
            fromAmount: 0.10000001m,
            toAmount: 3816.37000001m,
            validTime: "10s",
            receiveWindow: 60_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal("/fapi/v1/convert/getQuote", handler.RequestUri!.AbsolutePath);
        Assert.Equal("application/x-www-form-urlencoded", handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));

        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("signature=", query);
        Assert.DoesNotContain("timestamp=", query);
        Assert.DoesNotContain("recvWindow=", query);

        var body = ParseBody(handler);
        Assert.Equal("BTC", body["fromAsset"]);
        Assert.Equal("USDT", body["toAsset"]);
        Assert.Equal("0.10000001", body["fromAmount"]);
        Assert.Equal("3816.37000001", body["toAmount"]);
        Assert.Equal("10s", body["validTime"]);
        Assert.Equal("60000", body["recvWindow"]);
        Assert.True(body.ContainsKey("timestamp"));
        Assert.Equal(7, body.Count);
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == "/fapi/v1/convert/getQuote" && request.Weight == 50 && request.Signed);

        Assert.Equal("12415572564", result.Data!.QuoteId);
        Assert.Equal(38163.70000001m, result.Data.Ratio);
        Assert.Equal(0.00002620m, result.Data.InverseRatio);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1623319461670).UtcDateTime, result.Data.ValidTimestamp);
        Assert.Equal(3816.37000001m, result.Data.ToQuantity);
        Assert.Equal(0.10000001m, result.Data.FromQuantity);
    }

    [Fact]
    public async Task ConvertQuoteRequestAsync_AllowsOfficialGeneratedConnectorAmountCombinations()
    {
        var noAmountHandler = new RecordingHttpMessageHandler("{}");
        using (var noAmountClient = CreateClient(noAmountHandler))
        {
            var result = await noAmountClient.UsdFutures.ConvertQuoteRequestAsync("BTC", "USDT");
            Assert.True(result.Success);
            var body = ParseBody(noAmountHandler);
            Assert.DoesNotContain("fromAmount", body.Keys);
            Assert.DoesNotContain("toAmount", body.Keys);
        }

        var bothAmountsHandler = new RecordingHttpMessageHandler("{}");
        using var bothAmountsClient = CreateClient(bothAmountsHandler);
        var bothResult = await bothAmountsClient.UsdFutures.ConvertQuoteRequestAsync("BTC", "USDT", 1m, 2m);

        Assert.True(bothResult.Success);
        var bothBody = ParseBody(bothAmountsHandler);
        Assert.Equal("1", bothBody["fromAmount"]);
        Assert.Equal("2", bothBody["toAmount"]);
    }

    [Fact]
    public async Task ConvertAcceptQuoteAsync_UsesCurrentSignedBodyAndStringOrderId()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"orderId":"933256278426274426","createTime":1623381330472,"orderStatus":"PROCESS"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.ConvertAcceptQuoteAsync("12415572564", 60_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal("/fapi/v1/convert/acceptQuote", handler.RequestUri!.AbsolutePath);
        Assert.Equal("application/x-www-form-urlencoded", handler.ContentType);
        var body = ParseBody(handler);
        Assert.Equal("12415572564", body["quoteId"]);
        Assert.Equal("60000", body["recvWindow"]);
        Assert.True(body.ContainsKey("timestamp"));
        Assert.Equal(3, body.Count);
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == "/fapi/v1/convert/acceptQuote" && request.Weight == 200 && request.Signed);

        Assert.Equal("933256278426274426", result.Data!.OrderId);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1623381330472).UtcDateTime, result.Data.CreateTime);
        Assert.Equal("PROCESS", result.Data.Status);
    }

    [Fact]
    public async Task GetConvertOrderStatusAsync_ForwardsSchemaPermittedIdentifiersAndDeserializesStatus()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "orderId": 933256278426274400,
              "orderStatus": "SUCCESS",
              "fromAsset": "BTC",
              "fromAmount": "0.00054414",
              "toAsset": "USDT",
              "toAmount": "20.00000001",
              "ratio": "36755.00000001",
              "inverseRatio": "0.00002721",
              "createTime": 1623381330472
            }
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetConvertOrderStatusAsync("12415572564", "933256278426274400");

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/convert/orderStatus", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("quoteId=12415572564", query);
        Assert.Contains("orderId=933256278426274400", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.DoesNotContain("recvWindow=", query);
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == "/fapi/v1/convert/orderStatus" && request.Weight == 50 && request.Signed);

        Assert.Equal(933256278426274400L, result.Data!.OrderId);
        Assert.Equal("SUCCESS", result.Data.Status);
        Assert.Equal("BTC", result.Data.FromAsset);
        Assert.Equal(0.00054414m, result.Data.FromQuantity);
        Assert.Equal("USDT", result.Data.ToAsset);
        Assert.Equal(20.00000001m, result.Data.ToQuantity);
        Assert.Equal(36755.00000001m, result.Data.Ratio);
        Assert.Equal(0.00002721m, result.Data.InverseRatio);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1623381330472).UtcDateTime, result.Data.CreateTime);
    }

    [Fact]
    public async Task GetConvertOrderStatusAsync_AllowsNoIdentifiersFromOfficialGeneratedConnector()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);

        var result = await client.UsdFutures.GetConvertOrderStatusAsync();

        Assert.True(result.Success);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.DoesNotContain("quoteId=", query);
        Assert.DoesNotContain("orderId=", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
    }

    [Fact]
    public async Task ConvertSignedEndpoints_RejectReceiveWindowAboveCurrentMaximumBeforeTransport()
    {
        var explicitHandler = new RecordingHttpMessageHandler("{}");
        using (var explicitClient = CreateClient(explicitHandler))
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.UsdFutures.ConvertQuoteRequestAsync("BTC", "USDT", receiveWindow: 60_001));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.UsdFutures.ConvertAcceptQuoteAsync("quote-id", 60_001));
        }
        Assert.Null(explicitHandler.RequestUri);

        var configuredHandler = new RecordingHttpMessageHandler("{}");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.ConvertQuoteRequestAsync("BTC", "USDT"));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.ConvertAcceptQuoteAsync("quote-id"));
        Assert.Null(configuredHandler.RequestUri);
    }

    [Fact]
    public async Task ConvertSignedEndpoints_RejectMissingRequiredStringsBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            client.UsdFutures.ConvertQuoteRequestAsync(null!, "USDT"));
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            client.UsdFutures.ConvertQuoteRequestAsync("BTC", null!));
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            client.UsdFutures.ConvertAcceptQuoteAsync(null!));
        Assert.Null(handler.RequestUri);
    }

    private static Dictionary<string, string> ParseBody(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.Body!).Split('&').Select(value => value.Split('=', 2)).ToDictionary(value => value[0], value => value[1]);

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
