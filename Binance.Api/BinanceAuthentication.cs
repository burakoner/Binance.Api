namespace Binance.Api;

internal class BinanceAuthentication(ApiCredentials credentials) : AuthenticationProvider(credentials)
{
    public override void AuthenticateRestApi(RestApiClient apiClient, Uri uri, HttpMethod method, bool signed, ArraySerialization serialization, SortedDictionary<string, object> query, SortedDictionary<string, object> body, string bodyContent, SortedDictionary<string, string> headers)
    {
        // Check Point
        if (!signed) return;

        // Timestamp
        var timestamp = GetMillisecondTimestamp(apiClient);
        if (method == HttpMethod.Get) query.Add("timestamp", timestamp);
        else body.Add("timestamp", timestamp);

        // Set Uri Parameters
        uri = uri.SetParameters(query, serialization);

        // Key
        headers.Add("X-MBX-APIKEY", Credentials.Key.GetString());

        // Signature
        if (Credentials.Type == ApiCredentialsType.HMAC)
        {
            var signbody = body != null && body.Count > 0 ? body.ToFormData() : uri.Query.Replace("?", "");
            var signature = SignHMACSHA256(signbody).ToLowerInvariant();
            query.Add("signature", signature);
        }
        else
        {
            var signbody = body != null && body.Count > 0 ? body.ToFormData() : uri.Query.Replace("?", "");
            var signature = SignRSASHA256(Encoding.ASCII.GetBytes(signbody), SignatureOutputType.Base64);
            query.Add("signature", signature);
        }
    }
}
