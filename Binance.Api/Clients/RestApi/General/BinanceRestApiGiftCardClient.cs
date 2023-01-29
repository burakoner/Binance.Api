namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiGiftCardClient
{
    // Gift Card
    // TODO: Create a single-token gift card
    // TODO: Create a dual-token gift card
    // TODO: Redeem a Binance Gift Card
    // TODO: Verify Binance Gift Card by Gift Card Number
    // TODO: Fetch RSA Public Key
    // TODO: Fetch Token Limit

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiGiftCardClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }
}