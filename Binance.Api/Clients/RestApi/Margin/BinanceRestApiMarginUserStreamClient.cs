using Binance.Api.Models.RestApi.UserDataStream;

namespace Binance.Api.Clients.RestApi.Margin;

public class BinanceRestApiMarginUserStreamClient
{
    // Api
    protected const string marginApi = "sapi";
    protected const string marginVersion = "1";

    // User Data Stream - Cross Margin
    private const string crossMarginCreateListenKeyEndpoint = "userDataStream";
    private const string crossMarginUpdateListenKeyEndpoint = "userDataStream";
    private const string crossMarginDeleteListenKeyEndpoint = "userDataStream";

    // User Data Stream - Isolated Margin
    private const string isolatedMarginCreateListenKeyEndpoint = "userDataStream/isolated";
    private const string isolatedMarginUpdateListenKeyEndpoint = "userDataStream/isolated";
    private const string isolatedMarginDeleteListenKeyEndpoint = "userDataStream/isolated";

    // Internal References
    internal BinanceRestApiMarginClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiMarginUserStreamClient(BinanceRestApiMarginClient main)
    {
        MainClient = main;
    }

    #region Create a ListenKey (Cross Margin)
    public async Task<RestCallResult<string>> CreateCrossMarginUserStreamListenKeyAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<BinanceListenKey>(GetUrl(crossMarginCreateListenKeyEndpoint, marginApi, marginVersion), HttpMethod.Post, ct).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }
    #endregion

    #region Ping/Keep-alive a ListenKey (Cross Margin)
    public async Task<RestCallResult<object>> KeepAliveCrossMarginUserStreamListenKeyAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey },
            };

        return await SendRequestInternal<object>(GetUrl(crossMarginUpdateListenKeyEndpoint, marginApi, marginVersion), HttpMethod.Put, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Close a ListenKey (Cross Margin)
    public async Task<RestCallResult<object>> StopCrossMarginUserStreamListenKeyAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey }
            };

        return await SendRequestInternal<object>(GetUrl(crossMarginDeleteListenKeyEndpoint, marginApi, marginVersion), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Create a ListenKey (Isolated Margin)
    public async Task<RestCallResult<string>> StartIsolatedMarginUserStreamAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new Dictionary<string, object>()
        {
            {"symbol", symbol}
        };

        var result = await SendRequestInternal<BinanceListenKey>(GetUrl(isolatedMarginCreateListenKeyEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }
    #endregion

    #region Ping/Keep-alive a ListenKey (Isolated Margin)
    public async Task<RestCallResult<object>> KeepAliveIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new Dictionary<string, object>
        {
            { "listenKey", listenKey },
            {"symbol", symbol}
        };

        return await SendRequestInternal<object>(GetUrl(isolatedMarginUpdateListenKeyEndpoint, marginApi, marginVersion), HttpMethod.Put, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Close a ListenKey (Isolated Margin)
    public async Task<RestCallResult<object>> CloseIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));
        var parameters = new Dictionary<string, object>
        {
            { "listenKey", listenKey },
            {"symbol", symbol}
        };

        return await SendRequestInternal<object>(GetUrl(isolatedMarginDeleteListenKeyEndpoint, marginApi, marginVersion), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

}