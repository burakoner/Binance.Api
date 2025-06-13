namespace Binance.Api.InstitutionalLoan;

internal partial class BinanceInstitutionalLoanRestClient(BinanceRestApiClient root) : IBinanceInstitutionalLoanRestClient
{
    // Api
    private const string v1 = "1";
    private const string sapi = "sapi";

    // Parent
    private BinanceRestApiClient _ { get; } = root;

    // Internal
    private ILogger Logger => _.Logger;
    private BinanceRestApiClientOptions Options => _.ApiOptions;

    private Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => _.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    private Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.InstitutionalLoanRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public Task<RestCallResult<List<BinanceInstitutionalLoanGroup>>> GetLoanGroupsAsync(CancellationToken ct = default)
    {
        return RequestAsync<List<BinanceInstitutionalLoanGroup>>(GetUrl(sapi, v1, "margin/loan-groups"), HttpMethod.Get, ct, true, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceInstitutionalLoanGroup>> GetLoanGroupAsync(long groupId, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("groupId", groupId);

        return RequestAsync<BinanceInstitutionalLoanGroup>(GetUrl(sapi, v1, "margin/loan-group"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3000);
    }
}