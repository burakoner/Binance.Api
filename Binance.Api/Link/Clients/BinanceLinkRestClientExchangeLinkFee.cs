namespace Binance.Api.Link;

internal partial class BinanceLinkRestClientExchangeLink
{
    public Task<RestCallResult<BinanceLinkSubAccountCommission>> SetCommissionAsync(string subAccountId, decimal makerCommission, decimal takerCommission, decimal? marginMakerCommission = null, decimal? marginTakerCommission = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"makerCommission", makerCommission.ToString(BinanceConstants.CI)},
            {"takerCommission", takerCommission.ToString(BinanceConstants.CI)}
        };
        parameters.AddOptional("marginMakerCommission", marginMakerCommission?.ToString(BinanceConstants.CI));
        parameters.AddOptional("marginTakerCommission", marginTakerCommission?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceLinkSubAccountCommission>(GetUrl(sapi, v1, "broker/subAccountApi/commission"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceLinkSubAccountFuturesCommission>> SetFuturesCommissionAdjustmentAsync(string subAccountId, string symbol, int makerAdjustment, int takerAdjustment, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        symbol.ValidateNotNull(nameof(symbol));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"symbol", symbol},
            {"makerAdjustment", makerAdjustment.ToString(BinanceConstants.CI)},
            {"takerAdjustment", takerAdjustment.ToString(BinanceConstants.CI)}
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceLinkSubAccountFuturesCommission>(GetUrl(sapi, v1, "broker/subAccountApi/commission/futures"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceLinkSubAccountFuturesCommission>>> GetFuturesCommissionAdjustmentAsync(string subAccountId, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId}
        };
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceLinkSubAccountFuturesCommission>>(GetUrl(sapi, v1, "broker/subAccountApi/commission/futures"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceLinkSubAccountCoinFuturesCommission>> SetCoinFuturesCommissionAdjustmentAsync(string subAccountId, string pair, int makerAdjustment, int takerAdjustment, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));
        pair.ValidateNotNull(nameof(pair));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
            {"pair", pair},
            {"makerAdjustment", makerAdjustment.ToString(BinanceConstants.CI)},
            {"takerAdjustment", takerAdjustment.ToString(BinanceConstants.CI)}
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceLinkSubAccountCoinFuturesCommission>(GetUrl(sapi, v1, "broker/subAccountApi/commission/coinFutures"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceLinkSubAccountFuturesCommission>>> GetCoinFuturesCommissionAdjustmentAsync(string subAccountId, string? pair = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection
                             {
                                 {"subAccountId", subAccountId}
                             };
        parameters.AddOptional("pair", pair);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceLinkSubAccountFuturesCommission>>(GetUrl(sapi, v1, "broker/subAccountApi/commission/coinFutures"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceLinkRebate>>> GetBrokerCommissionRebatesAsync(string subAccountId, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        subAccountId.ValidateNotNull(nameof(subAccountId));

        var parameters = new ParameterCollection
        {
            {"subAccountId", subAccountId},
        };
        parameters.AddOptionalMilliseconds("startTime", startDate);
        parameters.AddOptionalMilliseconds("endTime", endDate);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("size", size?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceLinkRebate>>(GetUrl(sapi, v1, "broker/rebate/recentRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<List<BinanceLinkFuturesRebate>>> GetBrokerFuturesCommissionRebatesAsync(BinanceFuturesType futuresType, DateTime startDate, DateTime endDate, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddMilliseconds("startTime", startDate);
        parameters.AddMilliseconds("endTime", endDate);
        parameters.AddEnum("futuresType", futuresType);
        parameters.AddOptional("page", page?.ToString(BinanceConstants.CI));
        parameters.AddOptional("size", size?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceLinkFuturesRebate>>(GetUrl(sapi, v1, "broker/rebate/futures/recentRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 0);
    }
}