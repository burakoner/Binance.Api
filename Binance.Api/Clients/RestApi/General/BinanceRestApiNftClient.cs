namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiNftClient
{
    // NFT
    // TODO: Get NFT Transaction History
    // TODO: Get NFT Deposit History
    // TODO: Get NFT Withdraw History
    // TODO: Get NFT Asset

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    RestParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiNftClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

}