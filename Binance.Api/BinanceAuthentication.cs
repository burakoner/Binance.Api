namespace Binance.Api;

/*
 * Security Type	Description
 * ---------------- -------------------------------------------------------- 
 * NONE	            Endpoint can be accessed freely.
 * TRADE	        Endpoint requires sending a valid API-Key and signature.
 * USER_DATA	    Endpoint requires sending a valid API-Key and signature.
 * USER_STREAM	    Endpoint requires sending a valid API-Key.
 * 
 * https://developers.binance.com/en/docs/products/spot/rest-api
 */
internal class BinanceAuthentication(ApiCredentials credentials) : AuthenticationProvider(credentials)
{
    public override void AuthenticateRestApi(RestApiClient apiClient, Uri uri, HttpMethod method, bool signed, ArraySerialization serialization, SortedDictionary<string, object> query, SortedDictionary<string, object> body, string bodyContent, SortedDictionary<string, string> headers)
    {
        // Check Point
        if (Credentials == null || Credentials.Key == null) return;

        // Api Key
        var apikey = Credentials.Key.GetString();
        if (string.IsNullOrEmpty(apikey)) return;

        // Key
        headers.Add("X-MBX-APIKEY", apikey);

        // Check Point
        if (!signed) return;

        // Timestamp
        var timestamp = GetMillisecondTimestamp(apiClient);
        if (method == HttpMethod.Get || body.Count == 0) query.Add("timestamp", timestamp);
        else body.Add("timestamp", timestamp);

        // Set Uri Parameters
        uri = uri.SetParameters(query, serialization);

        // Signature
        var queryString = uri.Query.TrimStart('?');
        var bodyString = body.Count > 0 ? body.ToFormData() : string.Empty;
        var signaturePayload = queryString + bodyString;
        if (Credentials.Type == ApiCredentialsType.HMAC)
        {
            var signature = SignHMACSHA256(signaturePayload).ToLowerInvariant();
            query.Add("signature", signature);
        }
        else if (Credentials.Type == ApiCredentialsType.RsaXml || Credentials.Type == ApiCredentialsType.RsaPem)
        {
            var signature = SignRSASHA256(Encoding.ASCII.GetBytes(signaturePayload), SignatureOutputType.Base64);
            query.Add("signature", signature);
        }
        else if (Credentials.Type == ApiCredentialsType.Ed25519)
        {
            var signature = SignEd25519Base64(Encoding.ASCII.GetBytes(signaturePayload));
            query.Add("signature", signature);
        }
    }

    public Dictionary<string, object> AuthenticateSocketParameters(Dictionary<string, object> providedParameters, long timestamp)
    {
        var sortedParameters = new SortedDictionary<string, object>(providedParameters)
        {
            { "apiKey", Credentials.Key.GetString() },
            { "timestamp", timestamp },
        };
        var paramString = string.Join("&", sortedParameters.Select(p => p.Key + "=" + System.Convert.ToString(p.Value, BinanceConstants.CI)));

        string signature;
        if (Credentials.Type == ApiCredentialsType.HMAC)
        {
            signature = SignHMACSHA256(paramString);
        }
        else if (Credentials.Type == ApiCredentialsType.RsaXml || Credentials.Type == ApiCredentialsType.RsaPem)
        {
            signature = SignRSASHA256(Encoding.UTF8.GetBytes(paramString), SignatureOutputType.Base64);
        }
        else if (Credentials.Type == ApiCredentialsType.Ed25519)
        {
            signature = SignEd25519Base64(Encoding.UTF8.GetBytes(paramString));
        }
        else
        {
            throw new NotSupportedException($"Unsupported credential type: {Credentials.Type}");
        }

        var result = sortedParameters.ToDictionary(p => p.Key, p => p.Value);
        result.Add("signature", signature);
        return result;
    }

    private string SignEd25519Base64(byte[] payload)
    {
#if NET8_0_OR_GREATER
        var secret = Credentials.Secret.GetString().Trim();
        if (!secret.Contains("-----BEGIN PRIVATE KEY-----", StringComparison.Ordinal))
        {
            secret = $"-----BEGIN PRIVATE KEY-----\r\n{secret}\r\n-----END PRIVATE KEY-----\r\n";
        }

        var algorithm = NSec.Cryptography.SignatureAlgorithm.Ed25519;
        using var key = NSec.Cryptography.Key.Import(
            algorithm,
            Encoding.ASCII.GetBytes(secret),
            NSec.Cryptography.KeyBlobFormat.PkixPrivateKeyText);
        return System.Convert.ToBase64String(algorithm.Sign(key, payload));
#else
        throw new NotSupportedException("Ed25519 Algorithm is supported only .Net 8.0 or greater.");
#endif
    }
}
