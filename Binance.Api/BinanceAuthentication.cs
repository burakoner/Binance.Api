namespace Binance.Api;

/*
 * Security Type	Description
 * ---------------- -------------------------------------------------------- 
 * NONE	            Endpoint can be accessed freely.
 * TRADE	        Endpoint requires sending a valid API-Key and signature.
 * USER_DATA	    Endpoint requires sending a valid API-Key and signature.
 * USER_STREAM	    Endpoint requires sending a valid API-Key.
 * 
 * https://developers.binance.com/docs/binance-spot-api-docs/rest-api/endpoint-security-type
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
        if (method == HttpMethod.Get) query.Add("timestamp", timestamp);
        else body.Add("timestamp", timestamp);

        // Set Uri Parameters
        uri = uri.SetParameters(query, serialization);

        // Signature
        if (Credentials.Type == ApiCredentialsType.HMAC)
        {
            var signbody = body != null && body.Count > 0 ? body.ToFormData() : uri.Query.Replace("?", "");
            var signature = SignHMACSHA256(signbody).ToLowerInvariant();
            query.Add("signature", signature);
        }
        else if (Credentials.Type == ApiCredentialsType.RsaXml || Credentials.Type == ApiCredentialsType.RsaXml)
        {
            var signbody = body != null && body.Count > 0 ? body.ToFormData() : uri.Query.Replace("?", "");
            var signature = SignRSASHA256(Encoding.ASCII.GetBytes(signbody), SignatureOutputType.Base64);
            query.Add("signature", signature);
        }
        else if (Credentials.Type == ApiCredentialsType.Ed25519)
        {
            var signbody = body != null && body.Count > 0 ? body.ToFormData() : uri.Query.Replace("?", "");
            var signature = SignEd25519(Encoding.ASCII.GetBytes(signbody), SignatureOutputType.Base64);
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

        if (Credentials.Type == ApiCredentialsType.HMAC)
        {
            var sign = SignHMACSHA256(paramString);
            var result = sortedParameters.ToDictionary(p => p.Key, p => p.Value);
            result.Add("signature", sign);
            return result;
        }
        else
        {
            var sign = SignRSASHA256(Encoding.ASCII.GetBytes(paramString), SignatureOutputType.Base64);
            var result = sortedParameters.ToDictionary(p => p.Key, p => p.Value);
            result.Add("signature", sign);
            return result;
        }
    }
}
