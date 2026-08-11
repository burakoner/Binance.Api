using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Options;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsAccountFundingFlowTests
{
    [Fact]
    public async Task GetAccountFundingFlow_UsesCurrentSignedInt64ContractAndExactResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """[{"id":1125899906842624000,"asset":"USDT","amount":"-0.552","type":"FEE","createDate":1592449456000}]""");
        using var client = CreateClient(handler, limiter);
        var start = DateTimeOffset.FromUnixTimeMilliseconds(1_623_319_461_670).UtcDateTime;
        var end = DateTimeOffset.FromUnixTimeMilliseconds(1_641_782_889_000).UtcDateTime;

        var result = await client.Options.GetAccountFundingFlowAsync(
            BinanceOptionsFundingFlowCurrency.Usdt,
            recordId: 3_000_000_000,
            startTime: start,
            endTime: end,
            limit: 1000,
            receiveWindow: 3_000_000_000);

        Assert.True(result.Success);
        var flow = Assert.Single(result.Data);
        Assert.Equal(1_125_899_906_842_624_000L, flow.Id);
        Assert.Equal("USDT", flow.Asset);
        Assert.Equal(-0.552m, flow.Amount);
        Assert.Equal("FEE", flow.Type);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_592_449_456_000).UtcDateTime, flow.CreateDate);

        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/eapi/v1/bill", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("currency=USDT", query);
        Assert.DoesNotContain("symbol=", query);
        Assert.Contains("recordId=3000000000", query);
        Assert.Contains("startTime=1623319461670", query);
        Assert.Contains("endTime=1641782889000", query);
        Assert.Contains("limit=1000", query);
        Assert.Contains("recvWindow=3000000000", query);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item =>
            item.Endpoint == "/eapi/v1/bill" && item.Weight == 1 && item.Signed);

        Assert.Equal(
            ["Amount", "Asset", "CreateDate", "Id", "Type"],
            typeof(BinanceOptionsAccountFundingFlow).GetProperties().Select(property => property.Name).OrderBy(name => name).ToArray());
    }

    [Fact]
    public async Task GetAccountFundingFlow_UsesConfiguredInt64ReceiveWindowWithoutInventingACeiling()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, defaultReceiveWindow: TimeSpan.FromMilliseconds(3_000_000_000L));

        var result = await client.Options.GetAccountFundingFlowAsync(BinanceOptionsFundingFlowCurrency.Usdt);

        Assert.True(result.Success);
        Assert.Contains("recvWindow=3000000000", Uri.UnescapeDataString(handler.RequestUri!.Query));
    }

    [Fact]
    public async Task GetAccountFundingFlow_RejectsUnknownCurrencyAndOnlyPublishedLimitMaximum()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("[]"));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.Options.GetAccountFundingFlowAsync((BinanceOptionsFundingFlowCurrency)99));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            client.Options.GetAccountFundingFlowAsync(BinanceOptionsFundingFlowCurrency.Usdt, limit: 1001));

        var method = typeof(IBinanceOptionsRestClientAccount)
            .GetMethod(nameof(IBinanceOptionsRestClientAccount.GetAccountFundingFlowAsync))!;
        Assert.Equal(typeof(BinanceOptionsFundingFlowCurrency), method.GetParameters().Single(parameter => parameter.Name == "currency").ParameterType);
        Assert.Equal(typeof(long?), method.GetParameters().Single(parameter => parameter.Name == "limit").ParameterType);
        Assert.Equal(typeof(long?), method.GetParameters().Single(parameter => parameter.Name == "receiveWindow").ParameterType);
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
