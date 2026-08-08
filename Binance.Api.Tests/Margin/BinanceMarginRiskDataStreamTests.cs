using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using ApiSharp.WebSocket;
using Binance.Api.Margin;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginRiskDataStreamTests
{
    [Fact]
    public async Task RestMethods_UseCurrentApiKeyOnlyContractsAndWeights()
    {
        var startLimiter = new RecordingRateLimiter();
        var startHandler = new RecordingHttpMessageHandler("""{"listenKey":"risk-key"}""");
        using (var client = CreateClient(startHandler, startLimiter))
        {
            var result = await client.Margin.StartRiskDataStreamAsync();

            Assert.True(result.Success);
            Assert.Equal("risk-key", result.Data);
            Assert.Equal(HttpMethod.Post, startHandler.Method);
            Assert.Equal("/sapi/v1/margin/listen-key", startHandler.RequestUri!.AbsolutePath);
            Assert.Empty(startHandler.RequestUri.Query);
            Assert.Null(startHandler.Body);
            Assert.Equal("api-key", Assert.Single(startHandler.Headers["X-MBX-APIKEY"]));
            Assert.Contains(startLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/listen-key" && item.Weight == 1);
        }

        var keepAliveLimiter = new RecordingRateLimiter();
        var keepAliveHandler = new RecordingHttpMessageHandler("{}");
        using (var client = CreateClient(keepAliveHandler, keepAliveLimiter))
        {
            var result = await client.Margin.KeepAliveRiskDataStreamAsync("risk-key");

            Assert.True(result.Success);
            Assert.Equal(HttpMethod.Put, keepAliveHandler.Method);
            Assert.Equal("listenKey=risk-key", keepAliveHandler.Body);
            Assert.DoesNotContain("timestamp", keepAliveHandler.Body);
            Assert.Contains(keepAliveLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/listen-key" && item.Weight == 1);
        }

        var closeLimiter = new RecordingRateLimiter();
        var closeHandler = new RecordingHttpMessageHandler("{}");
        using (var client = CreateClient(closeHandler, closeLimiter))
        {
            var result = await client.Margin.CloseRiskDataStreamAsync();

            Assert.True(result.Success);
            Assert.Equal(HttpMethod.Delete, closeHandler.Method);
            Assert.Empty(closeHandler.RequestUri!.Query);
            Assert.Null(closeHandler.Body);
            Assert.Contains(closeLimiter.Requests, item => item.Endpoint == "/sapi/v1/margin/listen-key" && item.Weight == 3_000);
        }
    }

    [Fact]
    public async Task KeepAlive_RejectsMissingListenKey()
    {
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            RateLimiterEnabled = false
        });

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.KeepAliveRiskDataStreamAsync(""));
    }

    [Fact]
    public void StreamAddressAndEvents_MatchCurrentRiskContract()
    {
        Assert.Equal(
            "wss://margin-stream.binance.com/ws/risk-key",
            BinanceMarginSocketClient.GetRiskDataStreamAddress("risk-key"));

        var root = new BinanceSocketApiClient();
        var socket = Assert.IsType<BinanceMarginSocketClient>(root.Margin);
        BinanceMarginRiskLevelUpdate? marginLevel = null;
        BinanceMarginLiabilityUpdate? liability = null;

        socket.HandleRiskDataStreamEvent(
            new WebSocketDataEvent<string>(
                """{"e":"MARGIN_LEVEL_STATUS_CHANGE","E":1701949763462,"l":"1.1","s":"MARGIN_CALL"}""",
                DateTime.UtcNow),
            data => marginLevel = data.Data,
            null);
        socket.HandleRiskDataStreamEvent(
            new WebSocketDataEvent<string>(
                """{"e":"USER_LIABILITY_CHANGE","E":1701949801133,"a":"BTC","t":"BORROW","p":"0.00000100","i":"0.00000001"}""",
                DateTime.UtcNow),
            null,
            data => liability = data.Data);

        Assert.NotNull(marginLevel);
        Assert.Equal("MARGIN_LEVEL_STATUS_CHANGE", marginLevel.Event);
        Assert.Equal(1.1m, marginLevel.MarginLevel);
        Assert.Equal("MARGIN_CALL", marginLevel.Status);
        Assert.NotNull(liability);
        Assert.Equal("USER_LIABILITY_CHANGE", liability.Event);
        Assert.Equal("BTC", liability.Asset);
        Assert.Equal("BORROW", liability.Type);
        Assert.Equal(0.000001m, liability.PrincipalQuantity);
        Assert.Equal(0.00000001m, liability.InterestQuantity);
    }

    private static BinanceRestApiClient CreateClient(RecordingHttpMessageHandler handler, IRateLimiter limiter)
    {
        var options = new BinanceRestApiClientOptions(new ApiCredentials("api-key", "api-secret"))
        {
            HttpClient = new HttpClient(handler),
            AutoTimestamp = false,
            RateLimiterEnabled = true
        };
#pragma warning disable CS0612
        options.RateLimiters = [limiter];
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
