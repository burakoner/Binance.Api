namespace Binance.Api.Algo;

internal class BinanceAlgoRestClientFutures(BinanceAlgoRestClient parent) : IBinanceAlgoRestClientFutures
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

    public Task<RestCallResult<BinanceAlgoOrderResult>> PlaceVolumeParticipationOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        BinanceUrgency urgency,
        string? clientAlgoId = null,
        bool? reduceOnly = null,
        decimal? limitPrice = null,
        BinancePositionSide? positionSide = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateNewFuturesAlgoOrder(symbol, side, quantity, positionSide, reduceOnly);
        if (urgency is not (BinanceUrgency.Low or BinanceUrgency.Medium or BinanceUrgency.High))
            throw new ArgumentOutOfRangeException(nameof(urgency), "urgency must be LOW, MEDIUM, or HIGH");
        clientAlgoId = PrepareClientAlgoId(clientAlgoId);

        var parameters = new ParameterCollection()
        {
            { "symbol", symbol },
            { "quantity", quantity.ToString(BinanceConstants.CI) },
        };
        parameters.AddEnum("side", side);
        parameters.AddEnum("urgency", urgency);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("clientAlgoId", clientAlgoId);
        parameters.AddOptional("reduceOnly", reduceOnly);
        parameters.AddOptional("limitPrice", limitPrice);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAlgoOrderResult>(GetUrl(sapi, v1, "algo/futures/newOrderVp"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 300);
    }

    public Task<RestCallResult<BinanceAlgoOrderResult>> PlaceTimeWeightedAveragePriceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        int duration,
        string? clientAlgoId = null,
        bool? reduceOnly = null,
        decimal? limitPrice = null,
        BinancePositionSide? positionSide = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateNewFuturesAlgoOrder(symbol, side, quantity, positionSide, reduceOnly);
        if (duration is < 300 or > 86400)
            throw new ArgumentOutOfRangeException(nameof(duration), "duration must be between 300 and 86400 seconds");
        clientAlgoId = PrepareClientAlgoId(clientAlgoId);

        var parameters = new ParameterCollection()
        {
            { "symbol", symbol },
            { "quantity", quantity.ToString(BinanceConstants.CI) },
            { "duration", duration },
        };
        parameters.AddEnum("side", side);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("clientAlgoId", clientAlgoId);
        parameters.AddOptional("reduceOnly", reduceOnly);
        parameters.AddOptional("limitPrice", limitPrice);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAlgoOrderResult>(GetUrl(sapi, v1, "algo/futures/newOrderTwap"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000);
    }

    private static void ValidateNewFuturesAlgoOrder(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        BinancePositionSide? positionSide,
        bool? reduceOnly)
    {
        symbol.ValidateBinanceSymbol();
        if (side is not (BinanceOrderSide.Buy or BinanceOrderSide.Sell))
            throw new ArgumentOutOfRangeException(nameof(side), "side must be BUY or SELL");
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "quantity must be greater than zero");
        if (positionSide is not null && positionSide is not (BinancePositionSide.Both or BinancePositionSide.Long or BinancePositionSide.Short))
            throw new ArgumentOutOfRangeException(nameof(positionSide), "positionSide must be BOTH, LONG, or SHORT");
        if (reduceOnly is not null && (positionSide == BinancePositionSide.Long || positionSide == BinancePositionSide.Short))
            throw new ArgumentException("reduceOnly cannot be sent in Hedge Mode", nameof(reduceOnly));
    }

    private string PrepareClientAlgoId(string? clientAlgoId)
    {
        if (clientAlgoId is not null && clientAlgoId.Length != 32)
            throw new ArgumentException("clientAlgoId must contain exactly 32 characters when provided", nameof(clientAlgoId));

        return BinanceHelpers.ApplyBrokerId(clientAlgoId, BinanceConstants.ClientOrderIdFutures, 32, _.RestOptions.AllowAppendingClientOrderId);
    }

    private int? ValidateReceiveWindow(int? receiveWindow)
    {
        var normalizedReceiveWindow = _._.ReceiveWindow(receiveWindow);
        if (normalizedReceiveWindow > 60000)
            throw new ArgumentOutOfRangeException(nameof(receiveWindow), "receiveWindow cannot exceed 60000 milliseconds");

        return normalizedReceiveWindow;
    }

    public Task<RestCallResult<BinanceAlgoResult>> CancelAlgoOrderAsync(long algoId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "algoId", algoId },
        };
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAlgoResult>(GetUrl(sapi, v1, "algo/futures/order"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 1);
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

        return RequestAsync<BinanceAlgoSubOrderList>(GetUrl(sapi, v1, "algo/futures/subOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAlgoOrders>> GetOpenAlgoOrdersAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAlgoOrders>(GetUrl(sapi, v1, "algo/futures/openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
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

        return RequestAsync<BinanceAlgoOrders>(GetUrl(sapi, v1, "algo/futures/historicalOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}
