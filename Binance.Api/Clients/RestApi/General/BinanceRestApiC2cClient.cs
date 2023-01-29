using Binance.ApiClient.Models.RestApi;
using Binance.ApiClient.Models.RestApi.C2C;

namespace Binance.ApiClient.Clients.RestApi.General;

public class BinanceRestApiC2cClient
{
    // Api
    private const string marginApi = "sapi";
    private const string marginVersion = "1";

    // C2C
    private const string c2cTradeHistoryEndpoint = "c2c/orderMatch/listUserOrderHistory";

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiGeneralClient GeneralClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => GeneralClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await GeneralClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiC2cClient(BinanceRestApiClient root, BinanceRestApiGeneralClient general)
    {
        RootClient = root;
        GeneralClient = general;
    }

    #region Get C2C Trade History
    public async Task<RestCallResult<IEnumerable<BinanceC2CUserTrade>>> GetC2CTradeHistoryAsync(OrderSide side, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("tradeType", JsonConvert.SerializeObject(side, new OrderSideConverter(false)));
        parameters.AddOptionalParameter("startTimestamp", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTimestamp", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page);
        parameters.AddOptionalParameter("rows", pageSize);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<IEnumerable<BinanceC2CUserTrade>>>(GetUrl(c2cTradeHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        if (!result.Success)
            return result.As<IEnumerable<BinanceC2CUserTrade>>(default);

        if (result.Data?.Code != 0)
            return result.AsError<IEnumerable<BinanceC2CUserTrade>>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion
}