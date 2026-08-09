using ApiSharp.WebSocket;
using Binance.Api.Margin;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginUserDataStreamTests
{
    [Fact]
    public async Task CreateListenToken_SendsCurrentApiKeyOnlyRequestAndMapsExpiration()
    {
        var handler = new RecordingHttpMessageHandler(
            """{"token":"listen-token","expirationTime":1758792204196}""");
        using var httpClient = new HttpClient(handler);
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });

        var result = await client.Margin.CreateUserDataStreamAsync(
            symbol: "BNBUSDT",
            isIsolated: true,
            validity: 86_400_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal("/sapi/v1/userListenToken", handler.RequestUri!.AbsolutePath);
        Assert.Empty(handler.RequestUri.Query);
        Assert.Equal("api-key", Assert.Single(handler.Headers["X-MBX-APIKEY"]));
        Assert.Equal("isIsolated=true&symbol=BNBUSDT&validity=86400000", handler.Body);
        Assert.DoesNotContain("timestamp", handler.Body);
        Assert.DoesNotContain("signature", handler.Body);
        Assert.Equal("listen-token", result.Data.Token);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_758_792_204_196).UtcDateTime, result.Data.ExpirationTime);
    }

    [Fact]
    public async Task CreateListenToken_EnforcesDocumentedConditionalSymbolAndMaximumValidity()
    {
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            RateLimiterEnabled = false
        });

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.CreateUserDataStreamAsync(isIsolated: true));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.CreateUserDataStreamAsync(validity: 86_400_001));
    }

    [Fact]
    public void SocketRequests_UseCurrentListenTokenAndUnsubscribeContracts()
    {
        const long largeSubscriptionId = 4_294_967_296L;
        var subscribe = BinanceMarginSocketClient.CreateSubscriptionRequest("listen-token");
        subscribe.SubscriptionId = largeSubscriptionId;
        var unsubscribeOne = BinanceMarginSocketClient.CreateUnsubscribeRequest(largeSubscriptionId);
        var unsubscribeAll = BinanceMarginSocketClient.CreateUnsubscribeRequest(null);
        var socket = new TestMarginSocketClient(new BinanceSocketApiClient());

        Assert.Equal("userDataStream.subscribe.listenToken", subscribe.Method);
        Assert.Equal("listen-token", subscribe.Params["listenToken"]);
        Assert.Equal("userDataStream.unsubscribe", unsubscribeOne.Method);
        Assert.Equal(largeSubscriptionId, Assert.IsType<long>(unsubscribeOne.Params["subscriptionId"]));
        Assert.Equal("userDataStream.unsubscribe", unsubscribeAll.Method);
        Assert.Empty(unsubscribeAll.Params);
        Assert.True(socket.MatchesUserDataStream(
            subscribe,
            JToken.Parse("""{"subscriptionId":4294967296,"event":{"e":"balanceUpdate"}}""")));
        Assert.False(socket.MatchesUserDataStream(
            subscribe,
            JToken.Parse("""{"subscriptionId":42,"event":{"e":"balanceUpdate"}}""")));
    }

    [Fact]
    public void SubscriptionResponse_RequiresSubscriptionAndExpirationFields()
    {
        var success = BinanceMarginSocketClient.ParseSubscriptionResponse(
            JToken.Parse("""{"status":200,"result":{"subscriptionId":4294967296,"expirationTime":1749094553955907}}"""));
        var missingExpiration = BinanceMarginSocketClient.ParseSubscriptionResponse(
            JToken.Parse("""{"status":200,"result":{"subscriptionId":4294967296}}"""));
        var invalidToken = BinanceMarginSocketClient.ParseSubscriptionResponse(
            JToken.Parse("""{"status":400,"error":{"code":-1209,"msg":"Invalid listen token"}}"""));

        Assert.True(success.Success);
        Assert.Equal(4_294_967_296L, success.Data.SubscriptionId);
        Assert.Equal(1_749_094_553_955_907, success.Data.ExpirationTime);
        Assert.False(missingExpiration.Success);
        Assert.Contains("expirationTime", missingExpiration.Error!.Message);
        Assert.False(invalidToken.Success);
        Assert.Equal(-1209, invalidToken.Error!.Code);
    }

    [Fact]
    public void Extension_ReplacesReconnectTokenAndServerLifecycleState()
    {
        var active = BinanceMarginSocketClient.CreateSubscriptionRequest("old-token");
        active.SubscriptionId = 4_294_967_296L;
        active.ExpirationTime = 100;
        var replacement = BinanceMarginSocketClient.CreateSubscriptionRequest("new-token");

        BinanceMarginSocketClient.ApplySubscriptionExtension(
            active,
            replacement,
            new BinanceMarginUserDataStreamStatus(4_294_967_297L, 200));

        Assert.Equal(replacement.Id, active.Id);
        Assert.Equal("new-token", active.Params["listenToken"]);
        Assert.Equal(4_294_967_297L, active.SubscriptionId);
        Assert.Equal(200, active.ExpirationTime);
    }

    [Fact]
    public void ExecutionReportEnvelope_MapsCurrentConditionalStringFieldsAndInt64Subscription()
    {
        const string message = """
            {
              "subscriptionId": 4294967296,
              "event": {
                "e":"executionReport","E":1499405658658,"s":"ETHBTC","c":"client-id",
                "S":"BUY","o":"LIMIT","f":"GTC","q":"1.00000000","p":"0.10264410",
                "P":"0.00000000","F":"0.00000000","g":-1,"C":"","x":"NEW","X":"NEW",
                "r":"-2010","i":4293153,"l":"0.00000000","z":"0.00000000","L":"0.00000000",
                "n":"0","N":null,"T":1499405658657,"t":-1,"I":9223372036854775806,"w":true,"m":false,
                "M":false,"O":1499405658657,"Z":"0.00000000","Y":"0.00000000","Q":"0.00000000",
                "W":1499405658657,"V":"NONE","d":"4","D":"1668680518494","j":"1","J":"1000000",
                "v":"3","A":"3.000000","B":"3.000000","u":"1","U":"37","Cs":"BTCUSDT",
                "pl":"2.123456","pL":"0.10000001","pY":"0.21234562",
                "b":"ONE_PARTY_TRADE_REPORT","a":"1234","k":"SOR","uS":true
              }
            }
            """;
        var root = new BinanceSocketApiClient();
        var socket = Assert.IsType<BinanceMarginSocketClient>(root.Margin);
        BinanceMarginStreamOrderUpdate? update = null;

        socket.HandleUserDataStreamEvent(
            new WebSocketDataEvent<string>(message, DateTime.UtcNow),
            data => update = data.Data,
            null,
            null,
            null,
            null);

        Assert.NotNull(update);
        Assert.Equal(4_294_967_296L, update.SubscriptionId);
        Assert.Equal("-2010", update.RejectReason);
        Assert.Equal(9_223_372_036_854_775_806L, update.IgnoredValueI);
        Assert.Equal(4L, update.TrailingDelta);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_668_680_518_494).UtcDateTime, update.TrailingTime);
        Assert.Equal(1L, update.StrategyId);
        Assert.Equal(1_000_000L, update.StrategyType);
        Assert.Equal(3L, update.PreventedMatchId);
        Assert.Equal(3m, update.PreventedQuantity);
        Assert.Equal(3m, update.LastPreventedQuantity);
        Assert.Equal(1L, update.TradeGroupId);
        Assert.Equal(37L, update.CounterOrderId);
        Assert.Equal("BTCUSDT", update.CounterSymbol);
        Assert.Equal(2.123456m, update.PreventedExecutionQuantity);
        Assert.Equal(0.10000001m, update.PreventedExecutionPrice);
        Assert.Equal(0.21234562m, update.PreventedExecutionQuoteQuantity);
        Assert.Equal("ONE_PARTY_TRADE_REPORT", update.MatchType);
        Assert.Equal(1_234L, update.AllocationId);
        Assert.Equal("SOR", update.WorkingFloor);
        Assert.True(update.UsedSor);
        Assert.Null(typeof(BinanceMarginStreamOrderUpdate).GetProperty("ExecutionId"));
    }

    [Fact]
    public void EventStreamTerminatedEnvelope_MapsSubscriptionId()
    {
        const string message = """{"subscriptionId":4294967296,"event":{"e":"eventStreamTerminated","E":1759089357377}}""";
        var root = new BinanceSocketApiClient();
        var socket = Assert.IsType<BinanceMarginSocketClient>(root.Margin);
        BinanceMarginStreamUpdate? update = null;

        socket.HandleUserDataStreamEvent(
            new WebSocketDataEvent<string>(message, DateTime.UtcNow),
            null,
            null,
            null,
            null,
            data => update = data.Data);

        Assert.NotNull(update);
        Assert.Equal(4_294_967_296L, update.SubscriptionId);
        Assert.Equal("eventStreamTerminated", update.Event);
    }

    private sealed class TestMarginSocketClient(BinanceSocketApiClient root) : BinanceMarginSocketClient(root)
    {
        internal bool MatchesUserDataStream(BinanceMarginUserDataStreamRequest request, JToken message)
            => MessageMatchesHandler(null!, message, request);
    }
}
