using System.Text;
using ApiSharp.Authentication;
using Binance.Api.Shared;
using Binance.Api.Spot;
using Newtonsoft.Json.Linq;
using NSec.Cryptography;

namespace Binance.Api.Tests.Spot;

public class BinanceSpotSocketClientAuthenticationTests
{
    [Fact]
    public void SessionLogonRequest_UsesEd25519CredentialsAndCurrentParameters()
    {
        const long timestamp = 1_650_000_000_123;
        var algorithm = SignatureAlgorithm.Ed25519;
        var creationParameters = new KeyCreationParameters
        {
            ExportPolicy = KeyExportPolicies.AllowPlaintextExport
        };
        using var key = Key.Create(algorithm, creationParameters);
        var privateKey = Encoding.ASCII.GetString(key.Export(KeyBlobFormat.PkixPrivateKeyText));
        var root = new BinanceSocketApiClient();
        root.SetApiCredentials("api-key", privateKey, ApiCredentialsType.Ed25519);
        var client = Assert.IsType<BinanceSpotSocketClient>(root.Spot);

        var request = client.CreateSessionLogonRequest(6_000.346m, timestamp);

        Assert.Equal("session.logon", request.Method);
        Assert.Equal("api-key", request.Params["apiKey"]);
        Assert.Equal(6_000.346m, request.Params["recvWindow"]);
        Assert.Equal(timestamp, request.Params["timestamp"]);
        var signature = System.Convert.FromBase64String(Assert.IsType<string>(request.Params["signature"]));
        Assert.True(algorithm.Verify(
            key.PublicKey,
            Encoding.UTF8.GetBytes("apiKey=api-key&recvWindow=6000.346&timestamp=1650000000123"),
            signature));
    }

    [Fact]
    public void SessionLogonRequest_RejectsUnsupportedCredentialsAndReceiveWindow()
    {
        var root = new BinanceSocketApiClient();
        root.SetApiCredentials("api-key", "secret", ApiCredentialsType.HMAC);
        var client = Assert.IsType<BinanceSpotSocketClient>(root.Spot);

        Assert.Throws<NotSupportedException>(() => client.CreateSessionLogonRequest(null, 1));

        using var key = Key.Create(SignatureAlgorithm.Ed25519, new KeyCreationParameters
        {
            ExportPolicy = KeyExportPolicies.AllowPlaintextExport
        });
        root.SetApiCredentials(
            "api-key",
            Encoding.ASCII.GetString(key.Export(KeyBlobFormat.PkixPrivateKeyText)),
            ApiCredentialsType.Ed25519);

        Assert.Throws<ArgumentOutOfRangeException>(() => client.CreateSessionLogonRequest(-0.001m, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => client.CreateSessionLogonRequest(60_000.001m, 1));
        Assert.Throws<ArgumentException>(() => client.CreateSessionLogonRequest(5_000.0001m, 1));
        client.CreateSessionLogonRequest(5_000.1000m, 1);
    }

    [Fact]
    public void SessionRequests_UseCurrentMethodAndNoAuthenticationParameters()
    {
        var status = BinanceSpotSocketClient.CreateSessionRequest("session.status");
        var logout = BinanceSpotSocketClient.CreateSessionRequest("session.logout");

        Assert.Equal("session.status", status.Method);
        Assert.Empty(status.Params);
        Assert.Equal("session.logout", logout.Method);
        Assert.Empty(logout.Params);
    }

    [Fact]
    public void SessionStatus_MapsNullableAuthenticationAndConnectionState()
    {
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceSpotSocketClient>(root.Spot);
        var response = client.Deserializer<BinanceResultWithRateLimits<BinanceSpotWebSocketSessionStatus>>(
            JToken.Parse("""
                {
                  "id": 1,
                  "status": 200,
                  "result": {
                    "apiKey": null,
                    "authorizedSince": null,
                    "connectedSince": 1649729873021,
                    "returnRateLimits": false,
                    "serverTime": 1649730611671,
                    "userDataStream": false
                  }
                }
                """));

        Assert.True(response.Success);
        Assert.Null(response.Data.Result.ApiKey);
        Assert.Null(response.Data.Result.AuthorizedSince);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_649_729_873_021).UtcDateTime, response.Data.Result.ConnectedSince);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_649_730_611_671).UtcDateTime, response.Data.Result.ServerTime);
        Assert.False(response.Data.Result.ReturnRateLimits);
        Assert.False(response.Data.Result.UserDataStream);
    }
}
