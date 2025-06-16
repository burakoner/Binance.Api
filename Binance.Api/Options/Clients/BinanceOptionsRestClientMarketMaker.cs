namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClientMarketMaker(BinanceOptionsRestClient parent) : IBinanceOptionsRestClientMarketMaker
{
    // Api
    private const string v1 = "1";
    private const string eapi = "eapi";

    // Parent
    private BinanceRestApiClient __ { get; } = parent._;
    private BinanceOptionsRestClient _ { get; } = parent;

    // Internal
    private ILogger Logger => __.Logger;
    private BinanceRestApiClientOptions RestOptions => __.ApiOptions;

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
        => _.GetUrl(api, version, endpoint);
}