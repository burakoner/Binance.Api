namespace Binance.Api.PortfolioMargin;

internal partial class BinancePortfolioMarginRestClient : IBinancePortfolioMarginRestClient
{
    // Api Version
    internal const string v1 = "1";
    internal const string papi = "papi";

    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.ApiOptions;

    // Interface Properties
    public IBinancePortfolioMarginRestClientCrossMargin CrossMargin { get; }
    public IBinancePortfolioMarginRestClientCoinFutures CoinFutures { get; }
    public IBinancePortfolioMarginRestClientUsdFutures UsdFutures { get; }

    // Constructor
    internal BinancePortfolioMarginRestClient(BinanceRestApiClient root)
    {
        _ = root;
        CrossMargin = new BinancePortfolioMarginRestClientCrossMargin(this);
        CoinFutures = new BinancePortfolioMarginRestClientCoinFutures(this);
        UsdFutures = new BinancePortfolioMarginRestClientUsdFutures(this);
    }

    internal Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => _.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.PortfolioMarginRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public async Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        var result = await RequestAsync<object>(GetUrl(papi, v1, "ping"), HttpMethod.Get, ct).ConfigureAwait(false);
        sw.Stop();

        return result.Success
            ? result.As(sw.Elapsed)
            : result.AsError<TimeSpan>(result.Error!);
    }

    public async Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default)
    {
        var result = await RequestAsync<BinanceServerTime>(GetUrl(papi, v1, "time"), HttpMethod.Get, ct, ignoreRatelimit: true).ConfigureAwait(false);

        return result.Success
            ? result.As(result.Data?.ServerTime ?? default)
            : result.AsError<DateTime>(result.Error!);
    }
}