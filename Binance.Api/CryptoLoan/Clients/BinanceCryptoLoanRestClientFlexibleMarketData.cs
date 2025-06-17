namespace Binance.Api.CryptoLoan;

internal partial class BinanceCryptoLoanRestClientFlexible
{
    public Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleCollateralAsset>>> GetCollateralAssetsAsync(string asset, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("collateralCoin", asset);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceCryptoLoanFlexibleCollateralAsset>>(GetUrl(sapi, v2, "loan/flexible/collateral/data"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleInterestRate>>> GetInterestRateHistoryAsync(string asset, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "coin", asset }
        };
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceCryptoLoanFlexibleInterestRate>>(GetUrl(sapi, v2, "loan/interestRateHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleLoanableAsset>>> GetLoanableAssetsAsync(string asset, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("collateralCoin", asset);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceCryptoLoanFlexibleLoanableAsset>>(GetUrl(sapi, v2, "loan/flexible/loanable/data"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }
}