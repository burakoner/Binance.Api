namespace Binance.Api.CopyTrading;

internal partial class BinanceCopyTradingRestClientFutures(BinanceCopyTradingRestClient parent) : IBinanceCopyTradingRestClientFutures
{
    // Api
    private const string v1 = "1";
    private const string v2 = "2";
    private const string v3 = "3";
    private const string sapi = "sapi";

    // Parent
    private BinanceRestApiClient __ { get; } = parent._;
    private BinanceCopyTradingRestClient _ { get; } = parent;

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;

    // Request
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

    // GetUrl
    private static Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.CopyTradingRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public Task<RestCallResult<BinanceCopyTradingFuturesLeadTraderStatus>> GetLeadTraderStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceCopyTradingFuturesLeadTraderStatus>(GetUrl(sapi, v1, "copyTrading/futures/userStatus"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<List<BinanceCopyTradingFuturesLeadTradingSymbol>>> GetLeadTradingSymbolsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceCopyTradingFuturesLeadTradingSymbol>>(GetUrl(sapi, v1, "copyTrading/futures/userStatus"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }
}