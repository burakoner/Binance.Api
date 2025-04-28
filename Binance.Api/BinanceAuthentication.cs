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

    public Dictionary<string, object> AuthenticateSocketParameters(Dictionary<string, object> providedParameters)
    {
        var sortedParameters = new SortedDictionary<string, object>(providedParameters)
        {
            { "apiKey", Credentials.Key.GetString() },
            { "timestamp", DateTime.UtcNow.ConvertToMilliseconds() }
        };
        var paramString = string.Join("&", sortedParameters.Select(p => p.Key + "=" + Convert.ToString(p.Value, CultureInfo.InvariantCulture)));

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
