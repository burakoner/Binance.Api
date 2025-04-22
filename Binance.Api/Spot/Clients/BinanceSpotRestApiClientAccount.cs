namespace Binance.Api.Spot;

internal partial class BinanceSpotRestApiClient
{
    public Task<RestCallResult<BinanceSpotAccount>> GetAccountAsync(bool? omitZeroBalances = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("omitZeroBalances", omitZeroBalances?.ToString().ToLowerInvariant());
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSpotAccount>(GetUrl(api, v3, "account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<IEnumerable<BinanceSpotUserTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var weight = orderId.HasValue ? 5 : 20;
        return RequestAsync<IEnumerable<BinanceSpotUserTrade>>(GetUrl(api, v3, "myTrades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<IEnumerable<BinanceOrderRateLimit>>> GetOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceOrderRateLimit>>(GetUrl(api, v3, "rateLimit/order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<IEnumerable<BinancePreventedTrade>>> GetPreventedTradesAsync(string symbol, long? orderId = null, long? preventedMatchId = null, long? fromPreventedMatchId = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("preventedMatchId", preventedMatchId);
        parameters.AddOptional("fromPreventedMatchId", fromPreventedMatchId);
        parameters.AddOptional("size", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var weight = preventedMatchId.HasValue ? 2 : 20;
        if (orderId.HasValue) weight = 20;
        return RequestAsync<IEnumerable<BinancePreventedTrade>>(GetUrl(api, v3, "myPreventedMatches"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }
}