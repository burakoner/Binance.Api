using System.Security.Cryptography;
using System.Text;
using ApiSharp.Authentication;
using ApiSharp.WebSocket;
using Binance.Api.Spot;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Spot;

public class BinanceSpotSocketClientUserDataStreamTests
{
    [Fact]
    public void SignedSubscriptionRequest_UsesCurrentMethodParametersAndSignature()
    {
        const string secret = "test-secret";
        const long timestamp = 1_650_000_000_123;
        var root = new BinanceSocketApiClient();
        root.SetApiCredentials("api-key", secret, ApiCredentialsType.HMAC);
        var client = Assert.IsType<BinanceSpotSocketClient>(root.Spot);

        var request = client.CreateUserDataStreamRequest(6_000.346m, timestamp);

        Assert.Equal("userDataStream.subscribe.signature", request.Method);
        Assert.Equal("api-key", request.Params["apiKey"]);
        Assert.Equal(6_000.346m, request.Params["recvWindow"]);
        Assert.Equal(timestamp, request.Params["timestamp"]);
        var payload = "apiKey=api-key&recvWindow=6000.346&timestamp=1650000000123";
        var expectedSignature = System.Convert.ToHexString(
            HMACSHA256.HashData(Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(payload)));
        Assert.Equal(expectedSignature, request.Params["signature"]);
    }

    [Fact]
    public void ReconnectRefresh_ReplacesTimestampSignatureAndServerSubscriptionId()
    {
        var root = new BinanceSocketApiClient();
        root.SetApiCredentials("api-key", "test-secret", ApiCredentialsType.HMAC);
        var client = Assert.IsType<BinanceSpotSocketClient>(root.Spot);
        var request = client.CreateUserDataStreamRequest(null, 1_650_000_000_123);
        var originalRequestId = request.Id;
        var originalSignature = request.Params["signature"];
        request.SubscriptionId = 42;

        client.RefreshUserDataStreamRequest(request, 1_650_000_000_456);

        Assert.NotEqual(originalRequestId, request.Id);
        Assert.NotEqual(originalSignature, request.Params["signature"]);
        Assert.Equal(1_650_000_000_456, request.Params["timestamp"]);
        Assert.Null(request.SubscriptionId);
    }

    [Fact]
    public void ReceiveWindow_RejectsUndocumentedRangeAndPrecision()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceSpotSocketClient.ValidateUserDataStreamReceiveWindow(60_000.001m));
        Assert.Throws<ArgumentException>(() =>
            BinanceSpotSocketClient.ValidateUserDataStreamReceiveWindow(5_000.0001m));

        BinanceSpotSocketClient.ValidateUserDataStreamReceiveWindow(60_000m);
        BinanceSpotSocketClient.ValidateUserDataStreamReceiveWindow(5_000.123m);
    }

    [Fact]
    public void UnsubscribeRequest_SupportsOneOrAllSubscriptions()
    {
        var one = BinanceSpotSocketClient.CreateUserDataStreamUnsubscribeRequest(42);
        var all = BinanceSpotSocketClient.CreateUserDataStreamUnsubscribeRequest(null);

        Assert.Equal("userDataStream.unsubscribe", one.Method);
        Assert.Equal(42, one.Params["subscriptionId"]);
        Assert.Equal("userDataStream.unsubscribe", all.Method);
        Assert.Empty(all.Params);
    }

    [Fact]
    public void SubscriptionResponse_RequiresServerSubscriptionId()
    {
        var success = BinanceSpotSocketClient.ParseUserDataStreamSubscriptionResponse(
            JToken.Parse("""{"status":200,"result":{"subscriptionId":42}}"""));
        var missingId = BinanceSpotSocketClient.ParseUserDataStreamSubscriptionResponse(
            JToken.Parse("""{"status":200,"result":{}}"""));
        var serverError = BinanceSpotSocketClient.ParseUserDataStreamSubscriptionResponse(
            JToken.Parse("""{"status":400,"error":{"code":-1102,"msg":"Missing parameter"}}"""));

        Assert.True(success.Success);
        Assert.Equal(42, success.Data);
        Assert.False(missingId.Success);
        Assert.Contains("subscriptionId", missingId.Error!.Message);
        Assert.False(serverError.Success);
        Assert.Equal(-1102, serverError.Error!.Code);
    }

    [Fact]
    public void ExecutionReportEnvelope_MapsSubscriptionAndCurrentConditionalFields()
    {
        const string message = """
            {
              "subscriptionId": 42,
              "event": {
                "e": "executionReport",
                "E": 1499405658658,
                "s": "ETHBTC",
                "c": "client-id",
                "S": "BUY",
                "o": "LIMIT",
                "f": "GTC",
                "q": "1.00000000",
                "p": "0.10264410",
                "P": "0.00000000",
                "F": "0.00000000",
                "g": -1,
                "C": "",
                "x": "NEW",
                "X": "NEW",
                "r": "NONE",
                "i": 4293153,
                "l": "0.00000000",
                "z": "0.00000000",
                "L": "0.00000000",
                "n": "0",
                "N": null,
                "T": 1499405658657,
                "t": -1,
                "I": 8641984,
                "w": true,
                "m": false,
                "M": false,
                "O": 1499405658657,
                "Z": "0.00000000",
                "Y": "0.00000000",
                "Q": "0.00000000",
                "W": 1499405658657,
                "V": "NONE",
                "j": 1,
                "J": 1000000,
                "Cs": "BTCUSDT",
                "pl": "2.123456",
                "pL": "0.10000001",
                "pY": "0.21234562",
                "b": "ONE_PARTY_TRADE_REPORT",
                "a": 1234,
                "k": "SOR",
                "uS": true,
                "gP": "PRIMARY_PEG",
                "gOT": "PRICE_LEVEL",
                "gOV": 5,
                "gp": "1.00000000"
              }
            }
            """;
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceSpotSocketClient>(root.Spot);
        BinanceSpotStreamOrderUpdate? update = null;

        client.HandleUserDataStreamEvent(
            new WebSocketDataEvent<string>(message, DateTime.UtcNow),
            data => update = data.Data,
            null,
            null,
            null,
            null,
            null);

        Assert.NotNull(update);
        Assert.Equal(42, update.SubscriptionId);
        Assert.Equal(8_641_984, update.ExecutionId);
        Assert.Null(update.FeeAsset);
        Assert.Equal(1, update.StrategyId);
        Assert.Equal(1_000_000, update.StrategyType);
        Assert.Equal("BTCUSDT", update.CounterSymbol);
        Assert.Equal(2.123456m, update.PreventedExecutionQuantity);
        Assert.Equal(0.10000001m, update.PreventedExecutionPrice);
        Assert.Equal(0.21234562m, update.PreventedExecutionQuoteQuantity);
        Assert.Equal("ONE_PARTY_TRADE_REPORT", update.MatchType);
        Assert.Equal(1_234, update.AllocationId);
        Assert.Equal("SOR", update.WorkingFloor);
        Assert.True(update.UsedSor);
        Assert.Equal("PRIMARY_PEG", update.PeggedPriceType);
        Assert.Equal("PRICE_LEVEL", update.PeggedOffsetType);
        Assert.Equal(5, update.PeggedOffsetValue);
        Assert.Equal(1m, update.PeggedPrice);
    }

    [Fact]
    public void ExternalLockEnvelope_UsesTransactionTimeModel()
    {
        const string message = """
            {
              "subscriptionId": 7,
              "event": {
                "e": "externalLockUpdate",
                "E": 1581557507324,
                "a": "NEO",
                "d": "10.00000000",
                "T": 1581557507268
              }
            }
            """;
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceSpotSocketClient>(root.Spot);
        BinanceSpotStreamExternalLockUpdate? update = null;

        client.HandleUserDataStreamEvent(
            new WebSocketDataEvent<string>(message, DateTime.UtcNow),
            null,
            null,
            null,
            null,
            data => update = data.Data,
            null);

        Assert.NotNull(update);
        Assert.Equal(7, update.SubscriptionId);
        Assert.Equal("NEO", update.Asset);
        Assert.Equal(10m, update.Delta);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_581_557_507_268).UtcDateTime, update.TransactionTime);
    }
}
