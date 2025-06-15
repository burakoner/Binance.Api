namespace Binance.Api.SimpleEarn;

internal partial class BinanceSimpleEarnRestClientLocked
{
    public Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnLockedRecord>>> GetSubscriptionsAsync(string? purchaseId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("purchaseId", purchaseId);
        parameters.AddOptional("asset", asset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceSimpleEarnLockedRecord>>(GetUrl(sapi, v1, "simple-earn/locked/history/subscriptionRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnLockedRedemptionRecord>>> GetRedemptionsAsync(string? positionId = null, string? redeemId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("positionId", positionId);
        parameters.AddOptional("redeemId", redeemId);
        parameters.AddOptional("asset", asset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceSimpleEarnLockedRedemptionRecord>>(GetUrl(sapi, v1, "simple-earn/locked/history/redemptionRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnLockedRewardRecord>>> GetRewardsAsync(string? positionId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("positionId", positionId);
        parameters.AddOptional("asset", asset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceSimpleEarnLockedRewardRecord>>(GetUrl(sapi, v1, "simple-earn/locked/history/rewardsRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

}