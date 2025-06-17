namespace Binance.Api.VipLoan;

internal partial class BinanceVipLoanRestClient
{
    public Task<RestCallResult<BinanceVipLoanBorrow>> BorrowAsync(long loanAccountId, string loanCoin, decimal loanQuantity, long collateralAccountId, string collateralCoin, bool isFlexibleRate, int? loanTerm = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "loanAccountId", loanAccountId },
            { "loanCoin", loanCoin },
            { "loanAmount", loanQuantity },
            { "collateralAccountId", collateralAccountId },
            { "collateralCoin", collateralCoin },
            { "isFlexibleRate", isFlexibleRate }
        };
        parameters.AddOptional("loanTerm", loanTerm);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceVipLoanBorrow>(GetUrl(sapi, v1, "loan/vip/borrow"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000);
    }

    public Task<RestCallResult<BinanceVipLoanRepay>> RepayAsync(string orderId, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "orderId", orderId },
            { "amount", quantity },
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceVipLoanRepay>(GetUrl(sapi, v1, "loan/vip/repay"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000);
    }

    public Task<RestCallResult<BinanceVipLoanRenew>> RenewAsync(string orderId, int loanTerm, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "orderId", orderId },
            { "loanTerm", loanTerm },
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceVipLoanRenew>(GetUrl(sapi, v1, "loan/vip/renew"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000);
    }
}