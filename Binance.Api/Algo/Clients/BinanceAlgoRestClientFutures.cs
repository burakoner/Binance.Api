namespace Binance.Api.Algo;

internal class BinanceAlgoRestClientFutures(BinanceAlgoRestClient parent) : IBinanceAlgoRestClientFutures
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Parent
    internal BinanceAlgoRestClient _ { get; } = parent;

    public Task<RestCallResult<BinanceAlgoOrderResult>> PlaceVolumeParticipationOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        BinanceUrgency urgency,
        string? clientOrderId = null,
        bool? reduceOnly = null,
        decimal? limitPrice = null,
        BinancePositionSide? positionSide = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        clientOrderId = BinanceHelpers.ApplyBrokerId(clientOrderId, BinanceConstants.ClientOrderIdFutures, 36, _.RestApiOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection()
        {
            { "symbol", symbol },
            { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
        };
        parameters.AddEnum("side", side);
        parameters.AddEnum("urgency", urgency);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("clientAlgoId", clientOrderId);
        parameters.AddOptional("reduceOnly", reduceOnly);
        parameters.AddOptional("limitPrice", limitPrice);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceAlgoOrderResult>(_.GetUrl(sapi, v1, "algo/futures/newOrderVp"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceAlgoOrderResult>> PlaceTimeWeightedAveragePriceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        int duration,
        string? clientOrderId = null,
        bool? reduceOnly = null,
        decimal? limitPrice = null,
        BinancePositionSide? positionSide = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        clientOrderId = BinanceHelpers.ApplyBrokerId(clientOrderId, BinanceConstants.ClientOrderIdFutures, 36, _.RestApiOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection()
        {
            { "symbol", symbol },
            { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
            { "duration", duration },
        };
        parameters.AddEnum("side", side);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("clientAlgoId", clientOrderId);
        parameters.AddOptional("reduceOnly", reduceOnly);
        parameters.AddOptional("limitPrice", limitPrice);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceAlgoOrderResult>(_.GetUrl(sapi, v1, "algo/futures/newOrderTwap"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceAlgoResult>> CancelAlgoOrderAsync(long algoOrderId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "algoId", algoOrderId },
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceAlgoResult>(_.GetUrl(sapi, v1, "algo/futures/order"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
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

        return _.RequestAsync<BinanceAlgoSubOrderList>(_.GetUrl(sapi, v1, "algo/futures/subOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAlgoOrders>> GetOpenAlgoOrdersAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceAlgoOrders>(_.GetUrl(sapi, v1, "algo/futures/openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
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

        return _.RequestAsync<BinanceAlgoOrders>(_.GetUrl(sapi, v1, "algo/futures/historicalOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}