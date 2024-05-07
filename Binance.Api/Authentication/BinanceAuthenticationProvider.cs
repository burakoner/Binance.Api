namespace Binance.Api.Authentication;

internal class BinanceAuthenticationProvider : AuthenticationProvider
{
    public BinanceAuthenticationProvider(ApiCredentials credentials) : base(credentials)
    {
    }

    public override void AuthenticateRestApi(RestApiClient apiClient, Uri uri, HttpMethod method, bool signed, ArraySerialization serialization, SortedDictionary<string, object> query, SortedDictionary<string, object> body, string bodyContent, SortedDictionary<string, string> headers)
    {
        // Check Point
        if (!signed) return;

        // Api Key
        headers.Add("X-MBX-APIKEY", Credentials.Key!.GetString());

        // Timestamp
        var timestamp = GetMillisecondTimestamp(apiClient);
        query.Add("timestamp", timestamp);
        uri = uri.SetParameters(query, serialization);

        // Signature
        headers.Add("signature", SignHMACSHA256(body == null || body.Count == 0 ? uri.Query.Replace("?", "") : body.ToFormData()));
    }

    public override void AuthenticateTcpSocketApi()
    {
        throw new NotImplementedException();
    }

    public override void AuthenticateWebSocketApi()
    {
        throw new NotImplementedException();
    }
}
