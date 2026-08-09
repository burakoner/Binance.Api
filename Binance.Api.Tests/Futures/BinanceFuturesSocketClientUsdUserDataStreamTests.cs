using ApiSharp.Authentication;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesSocketClientUsdUserDataStreamTests
{
    [Fact]
    public void Lifecycle_UsesCurrentApiKeyOnlyWebSocketContracts()
    {
        Assert.Equal("ws-fapi/v1", BinanceFuturesSocketClientUsd.UserDataStreamPath);
        Assert.Equal("userDataStream.start", BinanceFuturesSocketClientUsd.StartUserDataStreamMethod);
        Assert.Equal("userDataStream.ping", BinanceFuturesSocketClientUsd.KeepAliveUserDataStreamMethod);
        Assert.Equal("userDataStream.stop", BinanceFuturesSocketClientUsd.StopUserDataStreamMethod);
        Assert.Equal(1, BinanceFuturesSocketClientUsd.UserDataStreamIpWeight);
        Assert.True(BinanceFuturesSocketClientUsd.UserDataStreamRequiresApiKey);
        Assert.False(BinanceFuturesSocketClientUsd.UserDataStreamRequiresSignature);
        Assert.True(typeof(IBinanceFuturesSocketClientUsdQueryUserDataStream)
            .IsAssignableFrom(typeof(IBinanceFuturesSocketClientUsd)));
    }

    [Fact]
    public async Task Lifecycle_RequiresApiCredentialsBeforeTransport()
    {
        var client = new BinanceSocketApiClient();

        await Assert.ThrowsAsync<InvalidOperationException>(() => client.UsdFutures.StartUserDataStreamAsync());
        await Assert.ThrowsAsync<InvalidOperationException>(() => client.UsdFutures.KeepAliveUserDataStreamAsync());
        await Assert.ThrowsAsync<InvalidOperationException>(() => client.UsdFutures.StopUserDataStreamAsync());

        var keyOnlyClient = new BinanceSocketApiClient(new BinanceSocketApiClientOptions
        {
            ApiCredentials = new ApiCredentials("api-key")
        });
        var signedError = await Assert.ThrowsAsync<InvalidOperationException>(() => keyOnlyClient.UsdFutures.GetBalancesAsync());
        Assert.Equal("No API secret provided for signed endpoint", signedError.Message);
    }

    [Fact]
    public void Lifecycle_DeserializesCurrentResponseEnvelopes()
    {
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceFuturesSocketClientUsd>(root.UsdFutures);

        var start = client.Deserializer<BinanceResultWithRateLimits<BinanceListenKey>>(
            JToken.Parse("""
                {
                  "id":"start-request",
                  "status":200,
                  "result":{"listenKey":"start-listen-key"},
                  "rateLimits":[
                    {"rateLimitType":"REQUEST_WEIGHT","interval":"MINUTE","intervalNum":1,"limit":2400,"count":1}
                  ]
                }
                """));

        Assert.True(start.Success);
        Assert.Equal("start-request", start.Data.Id);
        Assert.Equal(200, start.Data.Status);
        Assert.Equal("start-listen-key", start.Data.Result.ListenKey);
        Assert.Single(start.Data.Ratelimits);
        Assert.Equal(BinanceRateLimitType.RequestWeight, start.Data.Ratelimits[0].Type);
        Assert.Equal(1, start.Data.Ratelimits[0].Count);

        var keepAlive = client.Deserializer<BinanceResultWithRateLimits<BinanceListenKey>>(
            JToken.Parse("""
                {
                  "id":"ping-request",
                  "status":200,
                  "result":{"listenKey":"kept-alive-listen-key"},
                  "rateLimits":[]
                }
                """));

        Assert.True(keepAlive.Success);
        Assert.Equal("kept-alive-listen-key", keepAlive.Data.Result.ListenKey);

        var stop = client.Deserializer<BinanceResultWithRateLimits<object>>(
            JToken.Parse("""
                {
                  "id":"stop-request",
                  "status":200,
                  "result":{},
                  "rateLimits":[]
                }
                """));

        Assert.True(stop.Success);
        Assert.Equal("stop-request", stop.Data.Id);
        Assert.NotNull(stop.Data.Result);
    }
}
