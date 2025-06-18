namespace Binance.Api.PortfolioMargin;

internal partial class BinancePortfolioMarginRestClientUsdFutures(BinancePortfolioMarginRestClient parent) : IBinancePortfolioMarginRestClientUsdFutures
{
    // Api
    private const string v1 = "1";
    private const string v2 = "2";
    private const string v3 = "3";
    private const string papi = "papi";

    // Parent
    private BinanceRestApiClient __ { get; } = parent._;
    private BinancePortfolioMarginRestClient _ { get; } = parent;

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
        => _.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    // GetUrl
    private Uri GetUrl(string api, string version, string endpoint)
        => _.GetUrl(api, version, endpoint);
}