using Binance.Api.Models.RestApi.Account;
using Binance.Api.Models.RestApi.Futures;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiFuturesTransferClient
{
    // Api
    private const string sapi = "sapi";

    // Futures
    private const string futuresTransferEndpoint = "futures/transfer";
    private const string futuresTransferHistoryEndpoint = "futures/transfer";

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiFuturesTransferClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

    #region New Future Account Transfer
    public async Task<RestCallResult<BinanceTransaction>> TransferFuturesAccountAsync(string asset, decimal quantity, FuturesTransferType transferType, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
                { "type", JsonConvert.SerializeObject(transferType, new FuturesTransferTypeConverter(false)) }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceTransaction>(GetUrl(futuresTransferEndpoint, sapi, "1"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Get Future Account Transaction History List
    public async Task<RestCallResult<BinanceQueryRecords<BinanceSpotFuturesTransfer>>> GetFuturesTransferHistoryAsync(string asset, DateTime startTime, DateTime? endTime = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "asset", asset },
                { "startTime", startTime.ConvertToMilliseconds()! }
            };

        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceSpotFuturesTransfer>>(GetUrl(futuresTransferHistoryEndpoint, sapi, "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

}