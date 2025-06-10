namespace Binance.Api.CryptoLoan;

internal partial class BinanceCryptoLoanRestClientStable
{
    public Task<RestCallResult<BinanceCryptoLoanStableBorrow>> BorrowAsync(string loanAsset, string collateralAsset, int loanTerm, decimal? loanQuantity = null, decimal? collateralQuantity = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "loanCoin", loanAsset },
            { "collateralCoin", collateralAsset },
            { "loanTerm", loanTerm },
        };
        parameters.AddOptional("loanAmount", loanQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("collateralAmount", collateralQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceCryptoLoanStableBorrow>(_.GetUrl(sapi, v1, "loan/borrow"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 36000);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanStableOpenOrder>>> GetOpenBorrowOrdersAsync(long? orderId = null, string? loanAsset = null, string? collateralAsset = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("loanAsset", loanAsset);
        parameters.AddOptional("collateralAsset", collateralAsset);
        parameters.AddOptional("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceQueryRecords<BinanceCryptoLoanStableOpenOrder>>(_.GetUrl(sapi, v1, "loan/ongoing/orders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 300);
    }

    public Task<RestCallResult<BinanceCryptoLoanStableRepay>> RepayAsync(long orderId, decimal quantity, bool? repayWithBorrowedAsset = null, bool? collateralReturn = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "orderId", orderId },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddOptional("type", repayWithBorrowedAsset == null ? null : repayWithBorrowedAsset.Value ? "1" : "2");
        parameters.AddOptional("collateralReturn", collateralReturn);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceCryptoLoanStableRepay>(_.GetUrl(sapi, v1, "loan/repay"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000);
    }

    public Task<RestCallResult<BinanceCryptoLoanStableAdjustment>> AdjustAsync(long orderId, decimal quantity, BinanceCryptoLoanAdjustmentDirection direction, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "orderId", orderId },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
        };
        parameters.AddEnum("direction", direction);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceCryptoLoanStableAdjustment>(_.GetUrl(sapi, v1, "loan/adjust/ltv"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanStableAsset>>> GetLoanableAssetsAsync(string? loanAsset = null, int? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("vipLevel", vipLevel);
        parameters.AddOptional("loanAsset", loanAsset);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceQueryRecords<BinanceCryptoLoanStableAsset>>(_.GetUrl(sapi, v1, "loan/loanable/data"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanStableCollateralAsset>>> GetCollateralAssetsAsync(string? collateralAsset = null, int? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("vipLevel", vipLevel);
        parameters.AddOptional("collateralCoin", collateralAsset);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceQueryRecords<BinanceCryptoLoanStableCollateralAsset>>(_.GetUrl(sapi, v1, "loan/collateral/data"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceCryptoLoanStableRepayRate>> GetCollateralRepayRateAsync(string loanAsset, string collateralAsset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "loanCoin", loanAsset },
            { "collateralCoin", collateralAsset },
            { "repayAmount", quantity.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceCryptoLoanStableRepayRate>(_.GetUrl(sapi, v1, "loan/repay/collateral/rate"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 6000);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanStableMarginCall>>> CustomizeMarginCallAsync(decimal marginCall, string? orderId = null, string? collateralAsset = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "marginCall", marginCall.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("collateralCoin", collateralAsset);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceQueryRecords<BinanceCryptoLoanStableMarginCall>>(_.GetUrl(sapi, v1, "loan/customize/margin_call"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000);
    }
}