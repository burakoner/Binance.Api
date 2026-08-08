using ApiSharp.Models;
using Binance.Api.Shared;
using Binance.Api.Spot;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Binance.Api.Tests;

public class BinanceTransportRateLimitTests
{
    [Theory]
    [InlineData(418)]
    [InlineData(429)]
    public async Task RestRateLimitResponse_ExposesRetryAfterAndBlocksFollowingRequestsWithoutRetrying(int statusCodeValue)
    {
        var statusCode = (HttpStatusCode)statusCodeValue;
        var handler = new RateLimitHttpMessageHandler(statusCode, "60");
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions
        {
            AutoTimestamp = false,
            HttpClient = new HttpClient(handler),
            RateLimiterEnabled = false
        });
        var before = DateTime.UtcNow.AddSeconds(59);

        var result = await client.Spot.PingAsync();

        Assert.False(result.Success);
        Assert.Equal(statusCode, result.Response!.StatusCode);
        var error = Assert.IsType<BinanceRateLimitError>(result.Error);
        Assert.Equal(-1003, error.Code);
        Assert.True(error.RetryAfter >= before);
        Assert.True(error.RetryAfter <= DateTime.UtcNow.AddSeconds(61));
        Assert.Equal(1, handler.RequestCount);

        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var blockedResult = await client.Spot.PingAsync(cancellation.Token);

        Assert.False(blockedResult.Success);
        Assert.IsType<CancellationRequestedError>(blockedResult.Error);
        Assert.Equal(1, handler.RequestCount);
    }

    [Fact]
    public void RetryAfterParsers_UseRestSecondsOrHttpDateAndWebSocketUnixMilliseconds()
    {
        var restSeconds = BinanceServerRateLimitGuard.ParseRestRetryAfter(
            [new KeyValuePair<string, IEnumerable<string>>("Retry-After", ["2"])]);
        Assert.InRange(restSeconds!.Value, DateTime.UtcNow.AddSeconds(1), DateTime.UtcNow.AddSeconds(3));

        var httpDate = DateTimeOffset.UtcNow.AddMinutes(1).ToString("R");
        var restDate = BinanceServerRateLimitGuard.ParseRestRetryAfter(
            [new KeyValuePair<string, IEnumerable<string>>("retry-after", [httpDate])]);
        Assert.InRange(restDate!.Value, DateTime.UtcNow.AddSeconds(58), DateTime.UtcNow.AddSeconds(61));

        const long retryAfterMilliseconds = 1_659_146_400_000;
        var webSocket = BinanceServerRateLimitGuard.ParseWebSocketRetryAfter(JToken.FromObject(new
        {
            error = new { data = new { retryAfter = retryAfterMilliseconds } }
        }));
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(retryAfterMilliseconds).UtcDateTime, webSocket);

        var typedErrorData = JToken.FromObject(new
        {
            serverTime = retryAfterMilliseconds - 1_000,
            retryAfter = retryAfterMilliseconds
        }).ToObject<BinanceResultErrorData>();
        Assert.Equal(
            DateTimeOffset.FromUnixTimeMilliseconds(retryAfterMilliseconds - 1_000).UtcDateTime,
            typedErrorData!.ServerTime);
        Assert.Equal(webSocket, typedErrorData.RetryAfter);

        Assert.Null(BinanceServerRateLimitGuard.ParseRestRetryAfter([]));
        Assert.Null(BinanceServerRateLimitGuard.ParseWebSocketRetryAfter(JToken.Parse("{}")));
        Assert.Null(BinanceServerRateLimitGuard.ParseWebSocketRetryAfter(
            JToken.Parse("""{"error":{"data":{"retryAfter":"invalid"}}}""")));
    }

    [Fact]
    public void UserDataSubscriptionRateLimitResponses_PreserveServerBackoff()
    {
        var retryAfter = DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeMilliseconds();
        var response = JToken.FromObject(new
        {
            id = 17,
            status = 429,
            error = new
            {
                code = -1003,
                msg = "Too many requests",
                data = new { retryAfter }
            }
        });

        var spotError = Assert.IsType<BinanceRateLimitError>(
            BinanceSpotSocketClient.ParseUserDataStreamSubscriptionResponse(response).Error);
        var marginError = Assert.IsType<BinanceRateLimitError>(
            Binance.Api.Margin.BinanceMarginSocketClient.ParseSubscriptionResponse(response).Error);

        var expected = DateTimeOffset.FromUnixTimeMilliseconds(retryAfter).UtcDateTime;
        Assert.Equal(expected, spotError.RetryAfter);
        Assert.Equal(expected, marginError.RetryAfter);
    }

    [Fact]
    public void SpotWebSocketRateLimitResponse_IsHandledAndSetsTheSharedGuard()
    {
        var root = new BinanceSocketApiClient();
        var client = new TestSpotSocketClient(root);
        var retryAfter = DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeMilliseconds();
        var request = new BinanceSocketQuery { Id = 17, Method = "time" };
        var response = JToken.FromObject(new
        {
            id = 17,
            status = 429,
            error = new
            {
                code = -1003,
                msg = "Too many requests",
                data = new { serverTime = 1_659_142_907_531, retryAfter }
            },
            rateLimits = Array.Empty<object>()
        });

        var handled = client.HandleRateLimitResponse(request, response, out var result);

        Assert.True(handled);
        var error = Assert.IsType<BinanceRateLimitError>(result!.Error);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(retryAfter).UtcDateTime, error.RetryAfter);
        Assert.Equal(error.RetryAfter, root.ServerRateLimitGuard.RetryAfter);
    }

    private sealed class TestSpotSocketClient(BinanceSocketApiClient root) : BinanceSpotSocketClient(root)
    {
        internal bool HandleRateLimitResponse(
            BinanceSocketQuery request,
            JToken response,
            out CallResult<object>? result)
            => HandleQueryResponse<object>(null!, request, response, out result);
    }

    private sealed class RateLimitHttpMessageHandler(HttpStatusCode statusCode, string? retryAfter) : HttpMessageHandler
    {
        internal int RequestCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestCount++;
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent("""{"code":-1003,"msg":"Too many requests"}""")
            };
            if (retryAfter != null)
                response.Headers.TryAddWithoutValidation("Retry-After", retryAfter);
            return Task.FromResult(response);
        }
    }
}
