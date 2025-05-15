namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public Task<CallResult<BinanceSpotAccount>> GetAccountAsync(bool? omitZeroBalances = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("omitZeroBalances", omitZeroBalances?.ToString().ToLowerInvariant());

        return RequestAsync<BinanceSpotAccount>("ws-api/v3", $"account.status", parameters, true, true, weight: 20, ct: ct);
    }

    public Task<CallResult<List<BinanceCurrentRateLimit>>> GetRateLimitsAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbols", symbols);

        return RequestAsync<List<BinanceCurrentRateLimit>>("ws-api/v3", $"account.rateLimits.orders", parameters, true, true, weight: 40, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotOrder>>> GetOrdersAsync(string symbol, long? fromOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("orderId", fromOrderId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("limit", limit);

        return RequestAsync<List<BinanceSpotOrder>>("ws-api/v3", $"allOrders", parameters, true, true, weight: 20, ct: ct);
    }

    public Task<CallResult<List<BinanceOrderOcoList>>> GetOcoOrdersAsync(long? fromOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("fromId", fromOrderId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("limit", limit);

        return RequestAsync<List<BinanceOrderOcoList>>("ws-api/v3", $"allOrderLists", parameters, true, true, weight: 20, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotUserTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, long? fromOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("orderId", orderId);
        parameters.AddOptionalParameter("fromId", fromOrderId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptionalParameter("limit", limit);

        return RequestAsync<List<BinanceSpotUserTrade>>("ws-api/v3", $"myTrades", parameters, true, true, weight: 20, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotPreventedTrade>>> GetPreventedTradesAsync(string symbol, long? preventedTradeId = null, long? orderId = null, long? fromPreventedTradeId = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("orderId", orderId);
        parameters.AddOptionalParameter("preventedMatchId", preventedTradeId);
        parameters.AddOptionalParameter("fromPreventedMatchId", fromPreventedTradeId);
        parameters.AddOptionalParameter("limit", limit);
        int weight = preventedTradeId != null ? 2 : 20;

        return RequestAsync<List<BinanceSpotPreventedTrade>>("ws-api/v3", $"myPreventedMatches", parameters, true, true, weight: weight, ct: ct);
    }

    // TODO: Account allocations (USER_DATA)
    // TODO: Account Commission Rates (USER_DATA
    // TODO: Query Order Amendments (USER_DATA)
}