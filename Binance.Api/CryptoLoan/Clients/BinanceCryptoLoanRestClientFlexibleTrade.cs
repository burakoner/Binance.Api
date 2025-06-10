namespace Binance.Api.CryptoLoan;

internal partial class BinanceCryptoLoanRestClientFlexible
{
    public Task<RestCallResult<BinanceCryptoLoanFlexibleBorrow>> BorrowAsync(string loanAsset, string collateralAsset, decimal? loanQuantity = null, decimal? collateralQuantity = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "loanCoin", loanAsset },
            { "collateralCoin", collateralAsset }
        };
        parameters.AddOptional("loanAmount", loanQuantity);
        parameters.AddOptional("collateralAmount", collateralQuantity);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceCryptoLoanFlexibleBorrow>(GetUrl(sapi, v2, "loan/flexible/borrow"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000);
    }

    public Task<RestCallResult<BinanceCryptoLoanFlexibleRepay>> RepayAsync(string loanAsset, string collateralAsset, decimal quantity, bool? collateralReturn = null, bool? fullRepayment = null, BinanceCryptoLoanFlexibleRepaymentType? repaymentType = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "loanCoin", loanAsset },
            { "collateralCoin", collateralAsset },
            { "repayAmount", quantity }
        };
        parameters.AddOptional("collateralReturn", collateralReturn);
        parameters.AddOptional("fullRepayment", fullRepayment);
        parameters.AddOptionalEnum("repaymentType", repaymentType);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceCryptoLoanFlexibleRepay>(GetUrl(sapi, v2, "loan/flexible/repay"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000);
    }

    public Task<RestCallResult<BinanceCryptoLoanFlexibleAdjustment>> AdjustAsync(string loanAsset, string collateralAsset, decimal quantity, BinanceCryptoLoanAdjustmentDirection direction, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "loanCoin", loanAsset },
            { "collateralCoin", collateralAsset },
            { "repayAmount", quantity }
        };
        parameters.AddEnum("direction", direction);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceCryptoLoanFlexibleAdjustment>(GetUrl(sapi, v2, "loan/flexible/adjust/ltv"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000);
    }
}