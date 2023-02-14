using Binance.Api.Models.RestApi.FuturesAlgoOrders;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiFuturesAlgoClient
{
    // Futures Algo
    private const string placeVpOrderEndpoint = "algo/futures/newOrderVp";
    private const string placeTwapOrderEndpoint = "algo/futures/newOrderTwap";
    private const string cancelAlgoOrderEndpoint = "algo/futures/order";
    private const string getAlgoOpenOrdersEndpoint = "algo/futures/openOrders";
    private const string getAlgoHistoricalOrdersEndpoint = "algo/futures/historicalOrders";
    private const string getAlgoSubOrdersEndpoint = "algo/futures/subOrders";

    // Api
    private string _spotBaseAddress = "";

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiFuturesAlgoClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;

        _spotBaseAddress = Options.SpotOptions.BaseAddress;
    }

    #region Volume Participation(VP) New Order
    public async Task<RestCallResult<BinanceAlgoOrderResult>> PlaceVolumeParticipationOrderAsync(
        string symbol,
        OrderSide side,
        decimal quantity,
        OrderUrgency urgency,
        string clientOrderId = null,
        bool? reduceOnly = null,
        decimal? limitPrice = null,
        PositionSide? positionSide = null,
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol },
                { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
                { "urgency", EnumConverter.GetString(urgency) },
            };
        parameters.AddOptionalParameter("positionSide", positionSide == null ? null : JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)));
        parameters.AddOptionalParameter("clientAlgoId", clientOrderId);
        parameters.AddOptionalParameter("reduceOnly", reduceOnly);
        parameters.AddOptionalParameter("limitPrice", limitPrice);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var url = _spotBaseAddress.AppendPath("sapi", "v1", placeVpOrderEndpoint);
        return await SendRequestInternal<BinanceAlgoOrderResult>(new Uri(url), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
    }
    #endregion

    #region Time-Weighted Average Price(Twap) New Order
    public async Task<RestCallResult<BinanceAlgoOrderResult>> PlaceTimeWeightedAveragePriceOrderAsync(
        string symbol,
        OrderSide side,
        decimal quantity,
        int duration,
        string clientOrderId = null,
        bool? reduceOnly = null,
        decimal? limitPrice = null,
        PositionSide? positionSide = null,
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "symbol", symbol },
                { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
                { "duration", duration },
            };
        parameters.AddOptionalParameter("positionSide", positionSide == null ? null : JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)));
        parameters.AddOptionalParameter("clientAlgoId", clientOrderId);
        parameters.AddOptionalParameter("reduceOnly", reduceOnly);
        parameters.AddOptionalParameter("limitPrice", limitPrice);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var url = _spotBaseAddress.AppendPath("sapi", "v1", placeTwapOrderEndpoint);
        return await SendRequestInternal<BinanceAlgoOrderResult>(new Uri(url), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
    }
    #endregion

    #region Cancel Algo Order
    public async Task<RestCallResult<BinanceAlgoResult>> CancelAlgoOrderAsync(long algoOrderId, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "algoId", algoOrderId },
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var url = _spotBaseAddress.AppendPath("sapi", "v1", cancelAlgoOrderEndpoint);
        return await SendRequestInternal<BinanceAlgoResult>(new Uri(url), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
    }
    #endregion

    #region Query Current Algo Open Orders
    public async Task<RestCallResult<BinanceAlgoOrders>> GetOpenAlgoOrdersAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var url = _spotBaseAddress.AppendPath("sapi", "v1", getAlgoOpenOrdersEndpoint);
        return await SendRequestInternal<BinanceAlgoOrders>(new Uri(url), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
    }
    #endregion

    #region Query Historical Algo Orders
    public async Task<RestCallResult<BinanceAlgoOrders>> GetClosedAlgoOrdersAsync(string symbol = null, OrderSide? side = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("side", side == null ? null : JsonConvert.SerializeObject(side, new OrderSideConverter(false)));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page);
        parameters.AddOptionalParameter("pageSize", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var url = _spotBaseAddress.AppendPath("sapi", "v1", getAlgoHistoricalOrdersEndpoint);
        return await SendRequestInternal<BinanceAlgoOrders>(new Uri(url), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
    }
    #endregion

    #region Query Sub Orders
    public async Task<RestCallResult<BinanceAlgoSubOrderList>> GetAlgoSubOrdersAsync(long algoId, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>()
            {
                { "algoId", algoId }
            };
        parameters.AddOptionalParameter("page", page);
        parameters.AddOptionalParameter("pageSize", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var url = _spotBaseAddress.AppendPath("sapi", "v1", getAlgoSubOrdersEndpoint);
        return await SendRequestInternal<BinanceAlgoSubOrderList>(new Uri(url), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
    }
    #endregion
}