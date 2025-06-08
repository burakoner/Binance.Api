namespace Binance.Api.CryptoLoan;

internal class BinanceCryptoLoanRestClient : IBinanceCryptoLoanRestClient
{
    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;

    // Interface Properties
    public IBinanceCryptoLoanRestClientFlexible Flexible { get; }
    public IBinanceCryptoLoanRestClientStable Stable { get; }

    // Constructor
    internal BinanceCryptoLoanRestClient(BinanceRestApiClient root)
    {
        _ = root;
        Flexible = new BinanceCryptoLoanRestClientFlexible(this);
        Stable = new BinanceCryptoLoanRestClientStable(this);
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
        var url = BinanceAddress.Default.CryptoLoanRestApiAddress.AppendPath(api);
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }
}