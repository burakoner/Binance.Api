namespace Binance.Api.SimpleEarn;

internal partial class BinanceSimpleEarnRestClientFlexible(BinanceSimpleEarnRestClient parent) : IBinanceSimpleEarnRestClientFlexible
{
    // Api
    private const string v1 = "1";
    private const string v2 = "2";
    private const string v3 = "3";
    private const string fapi = "fapi";
    private const string sapi = "sapi";

    // Parent
    private BinanceRestApiClient __ { get; } = parent._;
    private BinanceSimpleEarnRestClient _ { get; } = parent;

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;

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

    private static Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.SimpleEarnRestApiAddress.AppendPath(api);
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }
}