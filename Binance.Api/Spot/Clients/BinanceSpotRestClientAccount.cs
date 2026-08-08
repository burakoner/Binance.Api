namespace Binance.Api.Spot;

internal partial class BinanceSpotRestClient
{
    public Task<RestCallResult<BinanceSpotAccount>> GetAccountAsync(bool? omitZeroBalances = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("omitZeroBalances", omitZeroBalances?.ToString().ToLowerInvariant());
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<BinanceSpotAccount>(GetUrl(api, v3, "account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<List<BinanceSpotUserTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotAccountValidation.UserTrades(orderId, startTime, endTime, fromId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));
        parameters.AddOptional("fromId", fromId?.ToString(BinanceConstants.CI));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        var weight = orderId.HasValue ? 5 : 20;
        return RequestAsync<List<BinanceSpotUserTrade>>(GetUrl(api, v3, "myTrades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceSpotOrderRateLimit>>> GetRateLimitsAsync(decimal? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<List<BinanceSpotOrderRateLimit>>(GetUrl(api, v3, "rateLimit/order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 40);
    }

    public Task<RestCallResult<List<BinanceSpotPreventedTrade>>> GetPreventedTradesAsync(string symbol, long? orderId = null, long? preventedMatchId = null, long? fromPreventedMatchId = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotAccountValidation.PreventedMatches(orderId, preventedMatchId, fromPreventedMatchId, limit);

        var parameters = new ParameterCollection()
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("preventedMatchId", preventedMatchId);
        parameters.AddOptional("fromPreventedMatchId", fromPreventedMatchId);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        var weight = preventedMatchId.HasValue ? 2 : 20;
        if (orderId.HasValue) weight = 20;
        return RequestAsync<List<BinanceSpotPreventedTrade>>(GetUrl(api, v3, "myPreventedMatches"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<BinanceSpotOrderList>> GetOrderListAsync(long? orderListId = null, string? originalClientOrderId = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderListId.HasValue && string.IsNullOrWhiteSpace(originalClientOrderId))
            throw new ArgumentException("Either orderListId or originalClientOrderId must be provided.");

        var parameters = new ParameterCollection();
        parameters.AddOptional("orderListId", orderListId);
        parameters.AddOptional("origClientOrderId", originalClientOrderId);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<BinanceSpotOrderList>(GetUrl(api, v3, "orderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<List<BinanceSpotOrderList>>> GetOrderListsAsync(long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotAccountValidation.OrderListHistory(fromId, startTime, endTime);

        var parameters = new ParameterCollection();
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<List<BinanceSpotOrderList>>(GetUrl(api, v3, "allOrderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<List<BinanceSpotOrderList>>> GetOpenOrderListsAsync(decimal? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));
        return RequestAsync<List<BinanceSpotOrderList>>(GetUrl(api, v3, "openOrderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 6);
    }

    public Task<RestCallResult<List<BinanceSpotAllocation>>> GetAllocationsAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, long? fromAllocationId = null, int? limit = null, long? orderId = null, decimal? receiveWindow = null, CancellationToken ct = default)
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

        return RequestAsync<List<BinanceSpotAllocation>>(GetUrl(api, v3, "myAllocations"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<BinanceSpotCommissionRates>> GetCommissionRatesAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        return RequestAsync<BinanceSpotCommissionRates>(GetUrl(api, v3, "account/commission"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<List<BinanceSpotOrderAmendment>>> GetOrderAmendmentsAsync(string symbol, long orderId, long? fromExecutionId = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection { { "symbol", symbol }, { "orderId", orderId } };
        parameters.AddOptional("fromExecutionId", fromExecutionId);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<List<BinanceSpotOrderAmendment>>(GetUrl(api, v3, "order/amendments"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 4);
    }

    public Task<RestCallResult<BinanceSpotAccountFilters>> GetAccountFiltersAsync(string symbol, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));
        return RequestAsync<BinanceSpotAccountFilters>(GetUrl(api, v3, "myFilters"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 40);
    }
}
