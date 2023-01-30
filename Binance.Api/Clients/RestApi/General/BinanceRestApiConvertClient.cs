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
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    RestParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceListResult<BinanceConvertTrade>>(GetUrl(convertTradeHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 100).ConfigureAwait(false);
    }
    #endregion
}