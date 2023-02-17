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
    internal BinanceRestApiUsdtFuturesClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.ClientOptions; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiUsdtFuturesPortfolioClient(BinanceRestApiUsdtFuturesClient main)
    {
        MainClient = main;
    }

}