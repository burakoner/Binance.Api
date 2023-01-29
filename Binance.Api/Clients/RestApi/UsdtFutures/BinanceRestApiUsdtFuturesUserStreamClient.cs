using Binance.ApiClient.Models.RestApi.UserDataStream;

namespace Binance.ApiClient.Clients.RestApi.UsdtFutures;

public class BinanceRestApiUsdtFuturesUserStreamClient
{
    // Api
    private const string api = "fapi";
    private const string signedVersion = "1";

    // User Stream
    private const string getFuturesListenKeyEndpoint = "listenKey";
    private const string keepFuturesListenKeyAliveEndpoint = "listenKey";
    private const string closeFuturesListenKeyEndpoint = "listenKey";

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiUsdtFuturesClient UsdtFuturesClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => UsdtFuturesClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await UsdtFuturesClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiUsdtFuturesUserStreamClient(BinanceRestApiClient root, BinanceRestApiUsdtFuturesClient usdt)
    {
        RootClient = root;
        UsdtFuturesClient = usdt;
    }

    #region Start User Data Stream
    public async Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<BinanceListenKey>(GetUrl(getFuturesListenKeyEndpoint, api, signedVersion), HttpMethod.Post, ct).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }
    #endregion

    #region Keepalive User Data Stream
    public async Task<RestCallResult<object>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new Dictionary<string, object>
        {
            { "listenKey", listenKey }
        };

        return await SendRequestInternal<object>(GetUrl(keepFuturesListenKeyAliveEndpoint, api, signedVersion), HttpMethod.Put, ct, parameters).ConfigureAwait(false);
    }
    #endregion

    #region Close User Data Stream
    public async Task<RestCallResult<object>> StopUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new Dictionary<string, object>
        {
            { "listenKey", listenKey }
        };

        return await SendRequestInternal<object>(GetUrl(closeFuturesListenKeyEndpoint, api, signedVersion), HttpMethod.Delete, ct, parameters).ConfigureAwait(false);
    }
    #endregion

}