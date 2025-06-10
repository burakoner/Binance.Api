namespace Binance.Api.Algo;

internal class BinanceAlgoRestClientSpot(BinanceAlgoRestClient parent) : IBinanceAlgoRestClientSpot
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Parent
    private BinanceAlgoRestClient _ { get; } = parent;

    private Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => _._.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    private Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.AlgoTradingRestApiAddress;
        if (!string.IsNullOrEmpty(api)) url = url.AppendPath($"{api}");
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    public Task<RestCallResult<BinanceAlgoOrderResult>> PlaceTimeWeightedAveragePriceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        int duration,
        string? clientOrderId = null,
        decimal? limitPrice = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        clientOrderId = BinanceHelpers.ApplyBrokerId(clientOrderId, BinanceConstants.ClientOrderIdSpot, 36, _.RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection()
        {
            { "symbol", symbol },
            { "quantity", quantity.ToString(BinanceConstants.CI) },
            { "duration", duration },
        };
        parameters.AddEnum("side", side);
        parameters.AddOptional("clientAlgoId", clientOrderId);
        parameters.AddOptional("limitPrice", limitPrice);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAlgoOrderResult>(GetUrl(sapi, v1, "algo/spot/newOrderTwap"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceAlgoResult>> CancelAlgoOrderAsync(long algoOrderId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "algoId", algoOrderId },
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAlgoResult>(GetUrl(sapi, v1, "algo/spot/order"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAlgoSubOrderList>> GetAlgoSubOrdersAsync(long algoId, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "algoId", algoId }
        };
        parameters.AddOptional("page", page);
        parameters.AddOptional("pageSize", limit);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAlgoSubOrderList>(GetUrl(sapi, v1, "algo/spot/subOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAlgoOrders>> GetOpenAlgoOrdersAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAlgoOrders>(GetUrl(sapi, v1, "algo/spot/openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAlgoOrders>> GetClosedAlgoOrdersAsync(string? symbol = null, BinanceOrderSide? side = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptionalEnum("side", side);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("page", page);
        parameters.AddOptional("pageSize", limit);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAlgoOrders>(GetUrl(sapi, v1, "algo/spot/historicalOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}