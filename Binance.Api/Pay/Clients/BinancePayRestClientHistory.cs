namespace Binance.Api.Pay;

internal partial class BinancePayRestClientHistory(BinancePayRestClient parent) : IBinancePayRestClientHistory
{
    // Api
    private const string v1 = "1";
    private const string sapi = "sapi";

    // Parent
    private BinanceRestApiClient __ { get; } = parent._;
    private BinancePayRestClient _ { get; } = parent;

    // Internal
    private ILogger Logger => _.Logger;
    private BinanceRestApiClientOptions Options => __.ApiOptions;

    private Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => __.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    private Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.PayHistoryRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public async Task<RestCallResult<List<BinancePayHistoryTransaction>>> GetHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceDataResponse<List<BinancePayHistoryTransaction>>>(GetUrl(sapi, v1, "pay/transactions"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
        if (!result.Success) return result.AsError<List<BinancePayHistoryTransaction>>(result.Error!);
        if (result.Data.Data == null) return result.AsError<List<BinancePayHistoryTransaction>>(new ServerError("No data returned from the API."));

        return result.As(result.Data.Data);
    }
}