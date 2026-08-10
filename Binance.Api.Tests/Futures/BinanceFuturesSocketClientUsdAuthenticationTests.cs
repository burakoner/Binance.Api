using System.Text;
using ApiSharp.Authentication;
using ApiSharp.WebSocket;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json.Linq;
using NSec.Cryptography;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesSocketClientUsdAuthenticationTests
{
    [Fact]
    public void SessionLogonRequest_UsesEd25519CredentialsAndCurrentParameters()
    {
        const long timestamp = 1_650_000_000_123;
        var algorithm = SignatureAlgorithm.Ed25519;
        using var key = Key.Create(algorithm, new KeyCreationParameters
        {
            ExportPolicy = KeyExportPolicies.AllowPlaintextExport
        });
        var privateKey = Encoding.ASCII.GetString(key.Export(KeyBlobFormat.PkixPrivateKeyText));
        var root = new BinanceSocketApiClient();
        root.SetApiCredentials("api-key", privateKey, ApiCredentialsType.Ed25519);
        var client = Assert.IsType<BinanceFuturesSocketClientUsd>(root.UsdFutures);

        var request = client.CreateSessionLogonRequest(60_000L, timestamp);

        Assert.Equal(BinanceFuturesSocketClientUsd.SessionLogonMethod, request.Method);
        Assert.Equal(2, BinanceFuturesSocketClientUsd.SessionRequestWeight);
        Assert.Equal("api-key", request.Params["apiKey"]);
        Assert.Equal(60_000L, Assert.IsType<long>(request.Params["recvWindow"]));
        Assert.Equal(timestamp, request.Params["timestamp"]);
        var signature = System.Convert.FromBase64String(Assert.IsType<string>(request.Params["signature"]));
        Assert.True(algorithm.Verify(
            key.PublicKey,
            Encoding.UTF8.GetBytes("apiKey=api-key&recvWindow=60000&timestamp=1650000000123"),
            signature));
    }

    [Fact]
    public void SessionLogonRequest_UsesConfiguredReceiveWindow()
    {
        var options = new BinanceSocketApiClientOptions
        {
            ReceiveWindow = TimeSpan.FromMilliseconds(4_321)
        };
        var root = new BinanceSocketApiClient(options);
        using var key = Key.Create(SignatureAlgorithm.Ed25519, new KeyCreationParameters
        {
            ExportPolicy = KeyExportPolicies.AllowPlaintextExport
        });
        root.SetApiCredentials(
            "api-key",
            Encoding.ASCII.GetString(key.Export(KeyBlobFormat.PkixPrivateKeyText)),
            ApiCredentialsType.Ed25519);
        var client = Assert.IsType<BinanceFuturesSocketClientUsd>(root.UsdFutures);

        var request = client.CreateSessionLogonRequest(null, 1);

        Assert.Equal(4_321L, Assert.IsType<long>(request.Params["recvWindow"]));
    }

    [Fact]
    public void SessionLogonRequest_RejectsMissingOrUnsupportedCredentialsAndInvalidReceiveWindow()
    {
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceFuturesSocketClientUsd>(root.UsdFutures);

        Assert.Throws<InvalidOperationException>(() => client.CreateSessionLogonRequest(null, 1));

        root.SetApiCredentials("api-key", "secret", ApiCredentialsType.HMAC);
        Assert.Throws<NotSupportedException>(() => client.CreateSessionLogonRequest(null, 1));

        using var key = Key.Create(SignatureAlgorithm.Ed25519, new KeyCreationParameters
        {
            ExportPolicy = KeyExportPolicies.AllowPlaintextExport
        });
        root.SetApiCredentials(
            "api-key",
            Encoding.ASCII.GetString(key.Export(KeyBlobFormat.PkixPrivateKeyText)),
            ApiCredentialsType.Ed25519);

        Assert.Throws<ArgumentOutOfRangeException>(() => client.CreateSessionLogonRequest(-1, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => client.CreateSessionLogonRequest(60_001, 1));
        client.CreateSessionLogonRequest(0, 1);
    }

    [Fact]
    public void SessionRequests_UseCurrentMethodsAndNoAuthenticationParameters()
    {
        var status = BinanceFuturesSocketClientUsd.CreateSessionRequest(
            BinanceFuturesSocketClientUsd.SessionStatusMethod);
        var logout = BinanceFuturesSocketClientUsd.CreateSessionRequest(
            BinanceFuturesSocketClientUsd.SessionLogoutMethod);

        Assert.Equal("session.status", status.Method);
        Assert.Empty(status.Params);
        Assert.Equal("session.logout", logout.Method);
        Assert.Empty(logout.Params);
    }

    [Fact]
    public void SessionStatus_MapsNullableAuthenticationAndConnectionState()
    {
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceFuturesSocketClientUsd>(root.UsdFutures);
        var response = client.Deserializer<BinanceResultWithRateLimits<BinanceFuturesUsdWebSocketSessionStatus>>(
            JToken.Parse(
                """
                {
                  "id": 1,
                  "status": 200,
                  "result": {
                    "apiKey": null,
                    "authorizedSince": null,
                    "connectedSince": 1649729873021,
                    "returnRateLimits": false,
                    "serverTime": 1649730611671
                  }
                }
                """));

        Assert.True(response.Success);
        Assert.Null(response.Data.Result.ApiKey);
        Assert.Null(response.Data.Result.AuthorizedSince);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_649_729_873_021).UtcDateTime, response.Data.Result.ConnectedSince);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_649_730_611_671).UtcDateTime, response.Data.Result.ServerTime);
        Assert.False(response.Data.Result.ReturnRateLimits);
    }

    [Fact]
    public void SessionRevocation_RecognizesOnlyTheDocumentedConnectionMessage()
    {
        var payload = BinanceFuturesSocketClientUsd.GetSessionRevocationPayload(JToken.Parse(
            """
            {
              "id": null,
              "status": 401,
              "error": {
                "code": -2015,
                "msg": "Invalid API-key, IP, or permissions for action."
              }
            }
            """));

        Assert.NotNull(payload);
        Assert.Equal(-2015, payload!["code"]!.Value<int>());
        Assert.Equal("Invalid API-key, IP, or permissions for action.", payload["msg"]!.Value<string>());

        Assert.Null(BinanceFuturesSocketClientUsd.GetSessionRevocationPayload(JToken.Parse(
            """{"id":1,"status":401,"error":{"code":-2015,"msg":"Invalid"}}""")));
        Assert.Null(BinanceFuturesSocketClientUsd.GetSessionRevocationPayload(JToken.Parse(
            """{"id":null,"status":401,"error":{"code":-1002,"msg":"Unauthorized"}}""")));
    }

    [Fact]
    public void ReauthenticationState_CannotExposeAStaleAuthenticatedKey()
    {
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceFuturesSocketClientUsd>(root.UsdFutures);
        var webSocketClient = new WebSocketClient(
            root.Logger,
            new WebSocketParameters(new Uri("wss://example.test/ws-fapi/v1"), true));
        var connection = new WebSocketConnection(root.Logger, client, webSocketClient, "session-test");
        var lifecycle = WebSocketSubscription.CreateForIdentifier(1, "session-test", true, true, _ => { });
        var session = new BinanceFuturesUsdWebSocketSession(
            connection,
            lifecycle,
            new BinanceFuturesUsdWebSocketSessionStatus { ApiKey = "old-key", AuthorizedSince = DateTime.UtcNow },
            5_000);

        Assert.False(session.Authenticated);
        Assert.Equal("old-key", session.LastStatus.ApiKey);
        Assert.True(lifecycle.Authenticated);

        session.MarkAuthenticationPending();

        Assert.False(session.Authenticated);
        Assert.Null(session.LastStatus.ApiKey);
        Assert.Null(session.LastStatus.AuthorizedSince);
        Assert.True(lifecycle.Authenticated);

        BinanceFuturesUsdWebSocketSessionRevocation? receivedRevocation = null;
        session.AuthenticationRevoked += revocation => receivedRevocation = revocation;
        var expectedRevocation = new BinanceFuturesUsdWebSocketSessionRevocation
        {
            Status = 401,
            Code = -2015,
            Message = "Invalid API-key, IP, or permissions for action."
        };

        session.MarkAuthenticationRevoked(expectedRevocation);

        Assert.False(lifecycle.Authenticated);
        Assert.Same(expectedRevocation, receivedRevocation);
        webSocketClient.Dispose();
    }

    [Fact]
    public void UsdFuturesClient_ExposesConnectionAuthenticationSurface()
    {
        var root = new BinanceSocketApiClient();

        Assert.IsAssignableFrom<IBinanceFuturesSocketClientUsdQueryAuthentication>(root.UsdFutures);
    }
}
