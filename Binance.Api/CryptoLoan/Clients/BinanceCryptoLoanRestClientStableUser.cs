namespace Binance.Api.CryptoLoan;

internal partial class BinanceCryptoLoanRestClientStable
{
    public  Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanBorrowRecord>>> GetBorrowHistoryAsync(long? orderId = null, string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("loanAsset", loanAsset);
        parameters.AddOptional("collateralAsset", collateralAsset);
        parameters.AddOptional("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceQueryRecords<BinanceCryptoLoanBorrowRecord>>(_.GetUrl(sapi, v1, "loan/borrow/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public  Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanLtvAdjustRecord>>> GetLtvAdjustHistoryAsync(long? orderId = null, string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("loanAsset", loanAsset);
        parameters.AddOptional("collateralAsset", collateralAsset);
        parameters.AddOptional("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceQueryRecords<BinanceCryptoLoanLtvAdjustRecord>>(_.GetUrl(sapi, v1, "loan/ltv/adjustment/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public  Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanRepayRecord>>> GetRepayHistoryAsync(long? orderId = null, string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("loanAsset", loanAsset);
        parameters.AddOptional("collateralAsset", collateralAsset);
        parameters.AddOptional("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceQueryRecords<BinanceCryptoLoanRepayRecord>>(_.GetUrl(sapi, v1, "loan/repay/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }
}