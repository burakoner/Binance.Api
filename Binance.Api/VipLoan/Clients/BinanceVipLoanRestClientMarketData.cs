namespace Binance.Api.VipLoan;

internal partial class BinanceVipLoanRestClient
{
    public Task<RestCallResult<BinanceRowsResult<BinanceVipLoanInterestRate>>> GetInterestRateAsync(string asset, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "loanCoin", asset }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceVipLoanInterestRate>>(GetUrl(sapi, v1, "loan/vip/request/interestRate"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceVipLoanInterestRate>>> GetInterestRateHistoryAsync(string asset, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "coin", asset }
        };
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceVipLoanInterestRate>>(GetUrl(sapi, v1, "loan/vip/interestRateHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceVipLoanLoanableAsset>>> GetLoanableAssetsAsync(string? asset=null, int? vipLevel=null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("loanCoin", asset);
        parameters.AddOptional("vipLevel", vipLevel);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceVipLoanLoanableAsset>>(GetUrl(sapi, v1, "loan/vip/loanable/data"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceVipLoanCollateralAsset>>> GetCollateralAssetsAsync(string asset, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("collateralCoin", asset);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceVipLoanCollateralAsset>>(GetUrl(sapi, v1, "loan/vip/collateral/data"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400);
    }
}