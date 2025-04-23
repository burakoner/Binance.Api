using Binance.Api.Futures.Responses;

namespace Binance.Api.Futures;

internal partial class BinanceRestApiClientFuturesUsd(BinanceRestApiClientFutures parent) : IBinanceRestApiClientFuturesUsd
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string fapi = "fapi";
    private const string sapi = "sapi";

    // Parent
    private BinanceRestApiClientFutures _ { get; } = parent;

    // Internal
    internal ILogger Logger => Logger;
    internal BinanceRestApiClientOptions Options => Options;
    internal DateTime? LastExchangeInfoUpdate { get; private set; }
    internal BinanceFuturesUsdtExchangeInfo? ExchangeInfo { get; private set; }

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
        => _._.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    // GetUrl
    private Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.CoinFuturesRestClientAddress.AppendPath(api);
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }
}