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
        string? clientAlgoId = null,
        decimal? limitPrice = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (side is not (BinanceOrderSide.Buy or BinanceOrderSide.Sell))
            throw new ArgumentOutOfRangeException(nameof(side), "side must be BUY or SELL");
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "quantity must be greater than zero");
        if (duration is < 300 or > 86400)
            throw new ArgumentOutOfRangeException(nameof(duration), "duration must be between 300 and 86400 seconds");
        if (clientAlgoId is not null && clientAlgoId.Length != 32)
            throw new ArgumentException("clientAlgoId must contain exactly 32 characters when provided", nameof(clientAlgoId));
        clientAlgoId = BinanceHelpers.ApplyBrokerId(clientAlgoId, BinanceConstants.ClientOrderIdSpot, 32, _.RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection()
        {
            { "symbol", symbol },
            { "quantity", quantity.ToString(BinanceConstants.CI) },
            { "duration", duration },
        };
        parameters.AddEnum("side", side);
        parameters.AddOptional("clientAlgoId", clientAlgoId);
        parameters.AddOptional("limitPrice", limitPrice);

        return RequestAsync<BinanceAlgoOrderResult>(GetUrl(sapi, v1, "algo/spot/newOrderTwap"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceAlgoResult>> CancelAlgoOrderAsync(long algoId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "algoId", algoId },
        };
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAlgoResult>(GetUrl(sapi, v1, "algo/spot/order"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    private int? ValidateReceiveWindow(int? receiveWindow)
    {
        var normalizedReceiveWindow = _._.ReceiveWindow(receiveWindow);
        if (normalizedReceiveWindow > 60000)
            throw new ArgumentOutOfRangeException(nameof(receiveWindow), "receiveWindow cannot exceed 60000 milliseconds");

        return normalizedReceiveWindow;
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
