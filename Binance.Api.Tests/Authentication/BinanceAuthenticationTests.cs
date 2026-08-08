using System.Security.Cryptography;
using System.Text;
using ApiSharp;
using ApiSharp.Authentication;
using ApiSharp.Enums;
using ApiSharp.Extensions;
using NSec.Cryptography;

namespace Binance.Api.Tests.Authentication;

public class BinanceAuthenticationTests
{
    [Fact]
    public void SignedRestRequest_SignatureUsesQueryFollowedByBody()
    {
        const string secret = "test-secret";
        var authentication = new BinanceAuthentication(new ApiCredentials("api-key", secret));
        var query = new SortedDictionary<string, object>
        {
            { "queryValue", "query data" }
        };
        var body = new SortedDictionary<string, object>
        {
            { "bodyValue", "body data" }
        };
        var headers = new SortedDictionary<string, string>();

        authentication.AuthenticateRestApi(
            new TestRestApiClient(),
            new Uri("https://api.binance.com/api/v3/order"),
            HttpMethod.Post,
            true,
            ArraySerialization.Array,
            query,
            body,
            string.Empty,
            headers);

        Assert.DoesNotContain("timestamp", query.Keys);
        Assert.Contains("timestamp", body.Keys);
        var signature = Assert.IsType<string>(query["signature"]);
        query.Remove("signature");
        var expectedPayload = query.ToFormData() + body.ToFormData();
        var expectedSignature = System.Convert.ToHexString(
            HMACSHA256.HashData(Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(expectedPayload)))
            .ToLowerInvariant();
        Assert.Equal(expectedSignature, signature);
    }

    [Fact]
    public async Task SignedRequest_HmacSignatureUsesPercentEncodedBody()
    {
        const string secret = "test-secret";
        var handler = new RecordingHttpMessageHandler("{}");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(new ApiCredentials("api-key", secret), httpClient);

        var result = await client.Margin.CancelMarginOrderAsync("１２３４５６", orderId: 28);

        Assert.True(result.Success);
        Assert.Contains("symbol=%ef%bc%91%ef%bc%92%ef%bc%93%ef%bc%94%ef%bc%95%ef%bc%96", handler.Body);

        var expectedSignature = System.Convert.ToHexString(
            HMACSHA256.HashData(Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(handler.Body!)))
            .ToLowerInvariant();
        Assert.Equal(expectedSignature, GetQueryValue(handler.RequestUri!, "signature"));
    }

    [Fact]
    public async Task SignedRequest_RsaPemCredentialsProduceValidSignature()
    {
        using var rsa = RSA.Create(2_048);
        var credentials = new ApiCredentials("api-key", rsa.ExportPkcs8PrivateKeyPem(), ApiCredentialsType.RsaPem);
        var handler = new RecordingHttpMessageHandler("{}");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(credentials, httpClient);

        var result = await client.Margin.CancelMarginOrderAsync("BTCUSDT", orderId: 28);

        Assert.True(result.Success);
        var signature = System.Convert.FromBase64String(GetQueryValue(handler.RequestUri!, "signature"));
        Assert.True(rsa.VerifyData(
            Encoding.ASCII.GetBytes(handler.Body!),
            signature,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1));
    }

    [Fact]
    public void SignedSocketRequest_RsaPemSignatureUsesUtf8Payload()
    {
        using var rsa = RSA.Create(2_048);
        var authentication = new BinanceAuthentication(
            new ApiCredentials("api-key", rsa.ExportPkcs8PrivateKeyPem(), ApiCredentialsType.RsaPem));

        var parameters = authentication.AuthenticateSocketParameters(
            new Dictionary<string, object> { { "symbol", "１２３４５６" } },
            1_650_000_000_123);

        var signature = System.Convert.FromBase64String(Assert.IsType<string>(parameters["signature"]));
        Assert.True(rsa.VerifyData(
            Encoding.UTF8.GetBytes("apiKey=api-key&symbol=１２３４５６&timestamp=1650000000123"),
            signature,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SignedSocketRequest_Ed25519CredentialsProduceValidUtf8Signature(bool useBase64BodyOnly)
    {
        var algorithm = SignatureAlgorithm.Ed25519;
        var creationParameters = new KeyCreationParameters
        {
            ExportPolicy = KeyExportPolicies.AllowPlaintextExport
        };
        using var key = Key.Create(algorithm, creationParameters);
        var privateKey = Encoding.ASCII.GetString(key.Export(KeyBlobFormat.PkixPrivateKeyText));
        if (useBase64BodyOnly)
        {
            privateKey = privateKey
                .Replace("-----BEGIN PRIVATE KEY-----", string.Empty)
                .Replace("-----END PRIVATE KEY-----", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty)
                .Trim();
        }

        var authentication = new BinanceAuthentication(
            new ApiCredentials("api-key", privateKey, ApiCredentialsType.Ed25519));

        var parameters = authentication.AuthenticateSocketParameters(
            new Dictionary<string, object> { { "symbol", "１２３４５６" } },
            1_650_000_000_123);

        var signature = System.Convert.FromBase64String(Assert.IsType<string>(parameters["signature"]));
        Assert.True(algorithm.Verify(
            key.PublicKey,
            Encoding.UTF8.GetBytes("apiKey=api-key&symbol=１２３４５６&timestamp=1650000000123"),
            signature));
    }

    private static BinanceRestApiClient CreateClient(ApiCredentials credentials, HttpClient httpClient)
        => new(new BinanceRestApiClientOptions(credentials)
        {
            AutoTimestamp = false,
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });

    private static string GetQueryValue(Uri uri, string name)
    {
        var parameter = uri.Query
            .TrimStart('?')
            .Split('&')
            .Single(value => value.StartsWith(name + "=", StringComparison.Ordinal));
        return Uri.UnescapeDataString(parameter[(parameter.IndexOf('=') + 1)..]);
    }

    private sealed class TestRestApiClient : RestApiClient
    {
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new BinanceAuthentication(credentials);
    }
}
