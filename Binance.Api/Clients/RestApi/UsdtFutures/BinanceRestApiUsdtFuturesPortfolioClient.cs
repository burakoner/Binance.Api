namespace Binance.Api.Clients.RestApi.UsdtFutures;

public class BinanceRestApiUsdtFuturesPortfolioClient
{
    // Api
    private const string api = "fapi";
    private const string publicVersion = "1";
    private const string signedVersion = "1";

    // Portfolio
    // TODO: Portfolio Margin Exchange Information
    // TODO: Portfolio Margin Account Information

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiUsdtFuturesClient UsdtFuturesClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => UsdtFuturesClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await UsdtFuturesClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiUsdtFuturesPortfolioClient(BinanceRestApiClient root, BinanceRestApiUsdtFuturesClient usdt)
    {
        RootClient = root;
        UsdtFuturesClient = usdt;
    }

}