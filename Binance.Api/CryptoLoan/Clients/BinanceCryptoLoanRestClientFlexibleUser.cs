namespace Binance.Api.CryptoLoan;

internal partial class BinanceCryptoLoanRestClientFlexible
{
    public Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleAdjustmentRecord>>> GetAdjustmentHistoryAsync(string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("loanCoin", loanAsset);
        parameters.AddOptional("collateralCoin", collateralAsset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceCryptoLoanFlexibleAdjustmentRecord>>(GetUrl(sapi, v2, "loan/flexible/ltv/adjustment/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceCryptoLoanFlexibleCollateralRepayRate>> GetCollateralRepayRateAsync(string loanAsset, string collateralAsset, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("loanCoin", loanAsset);
        parameters.AddOptional("collateralCoin", collateralAsset);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceCryptoLoanFlexibleCollateralRepayRate>(GetUrl(sapi, v2, "loan/flexible/repay/rate"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 6000);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleBorrowHistory>>> GetBorrowHistoryAsync(string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("loanCoin", loanAsset);
        parameters.AddOptional("collateralCoin", collateralAsset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceCryptoLoanFlexibleBorrowHistory>>(GetUrl(sapi, v2, "loan/flexible/borrow/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleOngoingOrder>>> GetOngoingOrdersAsync(string? loanAsset = null, string? collateralAsset = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("loanCoin", loanAsset);
        parameters.AddOptional("collateralCoin", collateralAsset);
        parameters.AddOptional("current", current);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceCryptoLoanFlexibleOngoingOrder>>(GetUrl(sapi, v2, "loan/flexible/ongoing/orders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 300);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleLiquidationRecord>>> GetLiquidationHistoryAsync(string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("loanCoin", loanAsset);
        parameters.AddOptional("collateralCoin", collateralAsset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceCryptoLoanFlexibleLiquidationRecord>>(GetUrl(sapi, v2, "loan/flexible/liquidation/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleRepayRecord>>> GetRepayHistoryAsync(string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("loanCoin", loanAsset);
        parameters.AddOptional("collateralCoin", collateralAsset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceCryptoLoanFlexibleRepayRecord>>(GetUrl(sapi, v2, "loan/flexible/repay/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }
}