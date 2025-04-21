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
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiGiftCardClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }
}