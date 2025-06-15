namespace Binance.Api.DualInvestment;

internal partial class BinanceDualInvestmentRestClient
{
    public Task<RestCallResult<BinanceDualInvestmentSubscription>> SubscribeAsync(long id, long orderId, decimal quantity, BinanceDualInvestmentAutoCompoundPlan autoCompoundPlan, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("id", id);
        parameters.AddParameter("orderId", orderId);
        parameters.AddParameter("depositAmount", quantity);
        parameters.AddEnum("autoCompoundPlan", autoCompoundPlan);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceDualInvestmentSubscription>(GetUrl(sapi, v1, "dci/product/subscribe"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceListTotalResponse<BinanceDualInvestmentPosition>>> GetPositionsAsync(BinanceDualInvestmentStatus? status = null, int? pageSize = null, int? pageIndex = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("status", status);
        parameters.AddOptional("pageSize", pageSize);
        parameters.AddOptional("pageIndex", pageIndex);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceListTotalResponse<BinanceDualInvestmentPosition>>(GetUrl(sapi, v1, "dci/product/positions"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceDualInvestmentAccount>> GetAccountAsync(CancellationToken ct = default)
    {
        return RequestAsync<BinanceDualInvestmentAccount>(GetUrl(sapi, v1, "dci/product/accounts"), HttpMethod.Get, ct, true, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceDualInvestmentPositionPlan>> SetAutoCompoundPlanAsync(long positionId, BinanceDualInvestmentAutoCompoundPlan autoCompoundPlan, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("positionId", positionId);
        parameters.AddEnum("AutoCompoundPlan", autoCompoundPlan);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceDualInvestmentPositionPlan>(GetUrl(sapi, v1, "dci/product/auto_compound/edit-status"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }
}