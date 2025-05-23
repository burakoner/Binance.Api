﻿namespace Binance.Api.Algo;

internal class BinanceAlgoRestClientSpot(BinanceAlgoRestClient parent) : IBinanceAlgoRestClientSpot
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Parent
    internal BinanceAlgoRestClient _ { get; } = parent;

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

        return _.RequestAsync<BinanceAlgoOrderResult>(_.GetUrl(sapi, v1, "algo/spot/newOrderTwap"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<BinanceAlgoResult>> CancelAlgoOrderAsync(long algoOrderId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "algoId", algoOrderId },
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceAlgoResult>(_.GetUrl(sapi, v1, "algo/spot/order"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
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

        return _.RequestAsync<BinanceAlgoSubOrderList>(_.GetUrl(sapi, v1, "algo/spot/subOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAlgoOrders>> GetOpenAlgoOrdersAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceAlgoOrders>(_.GetUrl(sapi, v1, "algo/spot/openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
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

        return _.RequestAsync<BinanceAlgoOrders>(_.GetUrl(sapi, v1, "algo/spot/historicalOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}