namespace Binance.Api.Spot;

public partial class BinanceSpotSocketClient
{
    #region Get Account Info

    /// <inheritdoc />
    public async Task<CallResult<BinanceResponse<BinanceAccountInfo>>> GetAccountInfoAsync(bool? omitZeroBalances = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("omitZeroBalances", omitZeroBalances?.ToString().ToLowerInvariant());
        return await _client.QueryAsync<BinanceAccountInfo>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"account.status", parameters, true, true, weight: 20, ct: ct).ConfigureAwait(false);
    }

    #endregion

    #region Get Order Rate Limits

    /// <inheritdoc />
    public async Task<CallResult<BinanceResponse<IEnumerable<BinanceCurrentRateLimit>>>> GetOrderRateLimitsAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbols", symbols);
        return await _client.QueryAsync<IEnumerable<BinanceCurrentRateLimit>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"account.rateLimits.orders", parameters, true, true, weight: 40, ct: ct).ConfigureAwait(false);
    }

    #endregion

    public async Task<CallResult<BinanceResponse<IEnumerable<BinanceOrder>>>> GetOrdersAsync(string symbol, long? fromOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("orderId", fromOrderId);
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("limit", limit);
        return await _client.QueryAsync<IEnumerable<BinanceOrder>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"allOrders", parameters, true, true, weight: 20, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<BinanceResponse<IEnumerable<BinanceOrderOcoList>>>> GetOcoOrdersAsync(long? fromOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("fromId", fromOrderId);
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("limit", limit);
        return await _client.QueryAsync<IEnumerable<BinanceOrderOcoList>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"allOrderLists", parameters, true, true, weight: 20, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<BinanceResponse<IEnumerable<BinanceTrade>>>> GetUserTradesAsync(string symbol, long? orderId = null, long? fromOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("orderId", orderId);
        parameters.AddOptionalParameter("fromId", fromOrderId);
        parameters.AddOptionalParameter("startTime", DateTimeConverter.ConvertToMilliseconds(startTime));
        parameters.AddOptionalParameter("endTime", DateTimeConverter.ConvertToMilliseconds(endTime));
        parameters.AddOptionalParameter("limit", limit);
        return await _client.QueryAsync<IEnumerable<BinanceTrade>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"myTrades", parameters, true, true, weight: 20, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<BinanceResponse<IEnumerable<BinancePreventedTrade>>>> GetPreventedTradesAsync(string symbol, long? preventedTradeId = null, long? orderId = null, long? fromPreventedTradeId = null, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptionalParameter("orderId", orderId);
        parameters.AddOptionalParameter("preventedMatchId", preventedTradeId);
        parameters.AddOptionalParameter("fromPreventedMatchId", fromPreventedTradeId);
        parameters.AddOptionalParameter("limit", limit);
        int weight = preventedTradeId != null ? 2 : 20;
        return await _client.QueryAsync<IEnumerable<BinancePreventedTrade>>(_client.ClientOptions.Environment.SpotSocketApiAddress.AppendPath("ws-api/v3"), $"myPreventedMatches", parameters, true, true, weight: weight, ct: ct).ConfigureAwait(false);
    }

    // TODO: Account allocations (USER_DATA)
    // TODO: Account Commission Rates (USER_DATA
    // TODO: Query Order Amendments (USER_DATA)

}