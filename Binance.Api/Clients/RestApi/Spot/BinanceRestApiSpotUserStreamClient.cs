using Binance.Api.Models.RestApi.UserDataStream;

namespace Binance.Api.Clients.RestApi.Spot;

public class BinanceRestApiSpotUserStreamClient
{
    // Api
    protected const string api = "api";

    // User Data Stream
    private const string spotCreateListenKeyEndpoint = "userDataStream";
    private const string spotUpdateListenKeyEndpoint = "userDataStream";
    private const string spotDeleteListenKeyEndpoint = "userDataStream";

    // Internal References
    internal BinanceRestApiSpotClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.ClientOptions; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiSpotUserStreamClient(BinanceRestApiSpotClient main)
    {
        MainClient = main;
    }

    #region Create a ListenKey (Spot)
    public async Task<RestCallResult<string>> CreateSpotUserStreamListenKeyAsync(CancellationToken ct = default)
    {
        var result = await SendRequestInternal<BinanceListenKey>(GetUrl(spotCreateListenKeyEndpoint, api, "3"), HttpMethod.Post, ct, true).ConfigureAwait(false);
        return result.As(result.Data?.ListenKey!);
    }
    #endregion

    #region Ping/Keep-alive a ListenKey (Spot)
    public async Task<RestCallResult<object>> KeepAliveSpotUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey }
            };

        return await SendRequestInternal<object>(GetUrl(spotUpdateListenKeyEndpoint, api, "3"), HttpMethod.Put, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Close a ListenKey (Spot)
    public async Task<RestCallResult<object>> StopSpotUserStreamAsync(string listenKey, CancellationToken ct = default)
    {
        listenKey.ValidateNotNull(nameof(listenKey));

        var parameters = new Dictionary<string, object>
            {
                { "listenKey", listenKey }
            };

        return await SendRequestInternal<object>(GetUrl(spotDeleteListenKeyEndpoint, api, "3"), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

}