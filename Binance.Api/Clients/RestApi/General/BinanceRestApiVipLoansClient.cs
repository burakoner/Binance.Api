namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiVipLoansClient
{
    // VIP Loans
    // TODO: Get VIP Loan Ongoing Orders
    // TODO: VIP Loan Repay
    // TODO: Get VIP Loan Repayment History
    // TODO: Check Locked Value of VIP Collateral Account

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.ClientOptions; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiVipLoansClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

}