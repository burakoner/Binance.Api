namespace Binance.Api.Clients.RestApi.CoinFutures;

public class BinanceRestApiCoinFuturesPortfolioClient
{
    // Api
    private const string api = "dapi";
    private const string publicVersion = "1";
    private const string signedVersion = "1";

    // Portfolio
    // TODO: Portfolio Margin Exchange Information
    // TODO: Portfolio Margin Account Information

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiCoinFuturesClient CoinFuturesClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => CoinFuturesClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await CoinFuturesClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiCoinFuturesPortfolioClient(BinanceRestApiClient root, BinanceRestApiCoinFuturesClient coin)
    {
        RootClient = root;
        CoinFuturesClient = coin;
    }

}