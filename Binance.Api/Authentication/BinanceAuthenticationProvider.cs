namespace Binance.Api.Authentication;

internal class BinanceAuthenticationProvider : AuthenticationProvider
{
    public BinanceAuthenticationProvider(ApiCredentials credentials) : base(credentials)
    {
    }

    public override void AuthenticateRestApi(RestApiClient apiClient, Uri uri, HttpMethod method, Dictionary<string, object> providedParameters, bool auth, ArraySerialization arraySerialization, HttpMethodParameterPosition parameterPosition, out SortedDictionary<string, object> uriParameters, out SortedDictionary<string, object> bodyParameters, out Dictionary<string, string> headers)
    {
        uriParameters = parameterPosition == HttpMethodParameterPosition.InUri ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
        bodyParameters = parameterPosition == HttpMethodParameterPosition.InBody ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
        headers = new Dictionary<string, string>() { { "X-MBX-APIKEY", Credentials.Key!.GetString() } };

        if (!auth)
            return;

        var parameters = parameterPosition == HttpMethodParameterPosition.InUri ? uriParameters : bodyParameters;
        var timestamp = GetMillisecondTimestamp(apiClient);
        parameters.Add("timestamp", timestamp);
        uri = uri.SetParameters(uriParameters, arraySerialization);
        parameters.Add("signature", SignHMACSHA256(parameterPosition == HttpMethodParameterPosition.InUri ? uri.Query.Replace("?", "") : parameters.ToFormData()));
    }

    public override void AuthenticateSocketApi()
    {
        throw new NotImplementedException();
    }

    public override void AuthenticateStreamApi()
    {
        throw new NotImplementedException();
    }
}
