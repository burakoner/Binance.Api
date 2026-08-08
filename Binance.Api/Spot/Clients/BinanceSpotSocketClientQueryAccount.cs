namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public Task<CallResult<BinanceSpotAccount>> GetAccountAsync(bool? omitZeroBalances = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("omitZeroBalances", omitZeroBalances?.ToString().ToLowerInvariant());
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<BinanceSpotAccount>("ws-api/v3", $"account.status", parameters, true, true, weight: 20, ct: ct);
    }

    public Task<CallResult<List<BinanceCurrentRateLimit>>> GetRateLimitsAsync(decimal? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<List<BinanceCurrentRateLimit>>("ws-api/v3", $"account.rateLimits.orders", parameters, true, true, weight: 40, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotAccountValidation.HistoryRange(startTime, endTime);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<List<BinanceSpotOrder>>("ws-api/v3", $"allOrders", parameters, true, true, weight: 20, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotOrderList>>> GetOrderListsAsync(long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotAccountValidation.OrderListHistory(fromId, startTime, endTime);

        var parameters = new ParameterCollection();
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<List<BinanceSpotOrderList>>("ws-api/v3", $"allOrderLists", parameters, true, true, weight: 20, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotUserTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotAccountValidation.UserTrades(orderId, startTime, endTime, fromId);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        var weight = orderId.HasValue ? 5 : 20;
        return RequestAsync<List<BinanceSpotUserTrade>>("ws-api/v3", $"myTrades", parameters, true, true, weight: weight, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotPreventedTrade>>> GetPreventedTradesAsync(string symbol, long? preventedMatchId = null, long? orderId = null, long? fromPreventedMatchId = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotAccountValidation.PreventedMatches(orderId, preventedMatchId, fromPreventedMatchId, limit);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("preventedMatchId", preventedMatchId);
        parameters.AddOptional("fromPreventedMatchId", fromPreventedMatchId);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));
        int weight = preventedMatchId != null ? 2 : 20;

        return RequestAsync<List<BinanceSpotPreventedTrade>>("ws-api/v3", $"myPreventedMatches", parameters, true, true, weight: weight, ct: ct);
    }

    public Task<CallResult<BinanceSpotOrderList>> GetOrderListAsync(long? orderListId = null, string? originalClientOrderId = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderListId.HasValue && string.IsNullOrWhiteSpace(originalClientOrderId))
            throw new ArgumentException("Either orderListId or originalClientOrderId must be provided.");

        var parameters = new ParameterCollection();
        parameters.AddOptional("orderListId", orderListId);
        parameters.AddOptional("origClientOrderId", originalClientOrderId);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));
        return RequestAsync<BinanceSpotOrderList>("ws-api/v3", "orderList.status", parameters, true, true, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotOrderList>>> GetOpenOrderListsAsync(decimal? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));
        return RequestAsync<List<BinanceSpotOrderList>>("ws-api/v3", "openOrderLists.status", parameters, true, true, weight: 6, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotAllocation>>> GetAllocationsAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, long? fromAllocationId = null, int? limit = null, long? orderId = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotAccountValidation.Allocations(orderId, fromAllocationId, startTime, endTime);

        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("fromAllocationId", fromAllocationId);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));
        return RequestAsync<List<BinanceSpotAllocation>>("ws-api/v3", "myAllocations", parameters, true, true, weight: 20, ct: ct);
    }

    public Task<CallResult<BinanceSpotCommissionRates>> GetCommissionRatesAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        return RequestAsync<BinanceSpotCommissionRates>("ws-api/v3", "account.commission", parameters, true, true, weight: 20, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotOrderAmendment>>> GetOrderAmendmentsAsync(string symbol, long orderId, long? fromExecutionId = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol }, { "orderId", orderId } };
        parameters.AddOptional("fromExecutionId", fromExecutionId);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));
        return RequestAsync<List<BinanceSpotOrderAmendment>>("ws-api/v3", "order.amendments", parameters, true, true, weight: 4, ct: ct);
    }

    public Task<CallResult<BinanceSpotAccountFilters>> GetAccountFiltersAsync(string symbol, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));
        return RequestAsync<BinanceSpotAccountFilters>("ws-api/v3", "myFilters", parameters, true, true, weight: 40, ct: ct);
    }
}
