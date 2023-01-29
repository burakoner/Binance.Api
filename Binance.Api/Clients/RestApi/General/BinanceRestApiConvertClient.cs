using Binance.ApiClient.Models.RestApi;
using Binance.ApiClient.Models.RestApi.Convert;

namespace Binance.ApiClient.Clients.RestApi.General;

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
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiGeneralClient GeneralClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => GeneralClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await GeneralClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiConvertClient(BinanceRestApiClient root, BinanceRestApiGeneralClient general)
    {
        RootClient = root;
        GeneralClient = general;
    }

    #region Get Convert Trade History
    public async Task<RestCallResult<BinanceListResult<BinanceConvertTrade>>> GetConvertTradeHistoryAsync(DateTime startTime, DateTime endTime, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceListResult<BinanceConvertTrade>>(GetUrl(convertTradeHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 100).ConfigureAwait(false);
    }
    #endregion
}