using Binance.Api.Models.RestApi.Convert;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiConvertClient
{
    // Api
    private const string marginApi = "sapi";
    private const string marginVersion = "1";

    // Convert
    // TODO: List All Convert Pairs
    // TODO: Query order quantity precision per asset
    // TODO: Send quote request
    // TODO: Accept Quote
    // TODO: Order status
    private const string convertTradeHistoryEndpoint = "convert/tradeFlow";

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiConvertClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

    #region Get Convert Trade History
    public async Task<RestCallResult<BinanceListResult<BinanceConvertTrade>>> GetConvertTradeHistoryAsync(DateTime startTime, DateTime endTime, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceListResult<BinanceConvertTrade>>(GetUrl(convertTradeHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100).ConfigureAwait(false);
    }
    #endregion
}