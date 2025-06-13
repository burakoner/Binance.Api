namespace Binance.Api.Rebate;

internal partial class BinanceRebateRestClient(BinanceRestApiClient root) : IBinanceRebateRestClient
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
        var url = BinanceAddress.Default.RebateRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public async Task<RestCallResult<BinanceRebateContainer<BinanceRebateSpotRecord>>> GetSpotRebateHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("page", page);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceRebateResult<BinanceRebateContainer<BinanceRebateSpotRecord>>>(GetUrl(sapi, v1, "rebate/taxQuery"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 12000).ConfigureAwait(false);
        if (!result.Success) return result.AsError<BinanceRebateContainer<BinanceRebateSpotRecord>>(result.Error!);

        return result.As(result.Data.Data);
    }
}