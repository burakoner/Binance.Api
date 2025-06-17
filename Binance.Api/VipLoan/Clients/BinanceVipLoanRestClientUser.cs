namespace Binance.Api.VipLoan;

internal partial class BinanceVipLoanRestClient
{
    public Task<RestCallResult<BinanceRowsResult<BinanceVipLoanOngingOrder>>> GetOngoingOrdersAsync(long? orderId = null, long? collateralAccountId = null, string? loanAsset = null, string? collateralAsset = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("collateralAccountId", collateralAccountId);
        parameters.AddOptional("loanCoin", loanAsset);
        parameters.AddOptional("collateralCoin", collateralAsset);
        parameters.AddOptional("current", current);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceVipLoanOngingOrder>>(GetUrl(sapi, v1, "loan/vip/ongoing/orders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceVipLoanRepayHistory>>> GetRepayHistoryAsync(long? orderId = null, string? loanAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("loanCoin", loanAsset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceVipLoanRepayHistory>>(GetUrl(sapi, v1, "loan/vip/repay/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceVipLoanAccruedInterest>>> GetAccruedInterestAsync(long? orderId = null, string? loanAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("loanCoin", loanAsset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceVipLoanAccruedInterest>>(GetUrl(sapi, v1, "loan/vip/accruedInterest"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceVipLoanCollateralAccount>>> CheckCollateralAccountAsync(long? orderId = null, long? collateralAccountId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("collateralAccountId", collateralAccountId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceVipLoanCollateralAccount>>(GetUrl(sapi, v1, "loan/vip/collateral/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 6000);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceVipLoanApplicationStatus>>> GetApplicationStatusAsync(int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("current", current);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceVipLoanApplicationStatus>>(GetUrl(sapi, v1, "loan/vip/request/data"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }
}