using Binance.Api.Models.RestApi.C2C;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiC2cClient
{
    // Api
    private const string marginApi = "sapi";
    private const string marginVersion = "1";

    // C2C
    private const string c2cTradeHistoryEndpoint = "c2c/orderMatch/listUserOrderHistory";

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.ClientOptions; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiC2cClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

    #region Get C2C Trade History
    public async Task<RestCallResult<IEnumerable<BinanceC2CUserTrade>>> GetC2CTradeHistoryAsync(BinanceSpotOrderSide side, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("tradeType", JsonConvert.SerializeObject(side, new OrderSideConverter(false)));
        parameters.AddOptionalParameter("startTimestamp", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTimestamp", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page);
        parameters.AddOptionalParameter("rows", pageSize);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<IEnumerable<BinanceC2CUserTrade>>>(GetUrl(c2cTradeHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
        if (!result.Success)
            return result.As<IEnumerable<BinanceC2CUserTrade>>(default);

        if (result.Data?.Code != 0)
            return result.AsError<IEnumerable<BinanceC2CUserTrade>>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion
}