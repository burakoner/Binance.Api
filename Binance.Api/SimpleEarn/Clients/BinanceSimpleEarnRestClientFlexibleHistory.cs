namespace Binance.Api.SimpleEarn;

internal partial class BinanceSimpleEarnRestClientFlexible
{
    public Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexibleRecord>>> GetSubscriptionsAsync(string? productId = null, string? purchaseId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("productId", productId);
        parameters.AddOptional("purchaseId", purchaseId);
        parameters.AddOptional("asset", asset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceSimpleEarnFlexibleRecord>>(GetUrl(sapi, v1, "simple-earn/flexible/history/subscriptionRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexibleRedemptionRecord>>> GetRedemptionsAsync(string? productId = null, string? redeemId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("productId", productId);
        parameters.AddOptional("redeemId", redeemId);
        parameters.AddOptional("asset", asset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceSimpleEarnFlexibleRedemptionRecord>>(GetUrl(sapi, v1, "simple-earn/flexible/history/redemptionRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexibleRewardRecord>>> GetRewardsAsync(BinanceSimpleEarnRewardType type, string? productId = null, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("type", type);
        parameters.AddOptional("productId", productId);
        parameters.AddOptional("asset", asset);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceSimpleEarnFlexibleRewardRecord>>(GetUrl(sapi, v1, "simple-earn/flexible/history/rewardsRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexibleCollateralRecord>>> GetCollateralsAsync(string productId, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("productId", productId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceSimpleEarnFlexibleCollateralRecord>>(GetUrl(sapi, v1, "simple-earn/flexible/history/collateralRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceRowsResult<BinanceSimpleEarnFlexibleRateRecord>>> GetRatesAsync(string productId, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("productId", productId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceRowsResult<BinanceSimpleEarnFlexibleRateRecord>>(GetUrl(sapi, v1, "simple-earn/flexible/history/rateHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }
}