namespace Binance.ApiClient.Clients.RestApi.General;

public class BinanceRestApiNftClient
{
    // NFT
    // TODO: Get NFT Transaction History
    // TODO: Get NFT Deposit History
    // TODO: Get NFT Withdraw History
    // TODO: Get NFT Asset

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiGeneralClient GeneralClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => GeneralClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await GeneralClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiNftClient(BinanceRestApiClient root, BinanceRestApiGeneralClient general)
    {
        RootClient = root;
        GeneralClient = general;
    }

}