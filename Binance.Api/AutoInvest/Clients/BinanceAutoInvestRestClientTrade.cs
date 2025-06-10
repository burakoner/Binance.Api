namespace Binance.Api.AutoInvest;

internal partial class BinanceAutoInvestRestClient
{
    public Task<RestCallResult<BinanceAutoInvestTradeResult>> OneTimeTransactionAsync(string sourceType, string requestId, decimal subscriptionQuantity, string sourceAsset, bool flexibleAllowedToUse, long indexId, Dictionary<string, decimal> subscriptionDetails, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("sourceType", sourceType);
        parameters.Add("requestId", requestId);
        parameters.Add("subscriptionAmount", subscriptionQuantity);
        parameters.Add("sourceAsset", sourceAsset);
        parameters.Add("flexibleAllowedToUse", flexibleAllowedToUse);
        parameters.Add("indexid", indexId);
        parameters.Add("details", subscriptionDetails.Select(x => new
        {
            targetAsset = x.Key,
            percentage = x.Value
        }).ToList());

        return RequestAsync<BinanceAutoInvestTradeResult>(GetUrl(sapi, v1, "lending/auto-invest/one-off"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAutoInvestEditStatusResult>> SetPlanStatusAsync(long planId, BinanceAutoInvestPlanStatus status, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("planId", planId);
        parameters.AddEnum("status", status);

        return RequestAsync<BinanceAutoInvestEditStatusResult>(GetUrl(sapi, v1, "lending/auto-invest/plan/edit-status"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAutoInvestEditResult>> SetPlanAsync(string planId, decimal subscriptionQuantity, AutoInvestSubscriptionCycle subscriptionCycle, string sourceAsset, Dictionary<string, decimal> subscriptionDetails, int? subscriptionStartDay = null, string? subscriptionStartWeekday = null, int? subscriptionStartTime = null, bool? flexibleAllowedToUse = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("planId", planId);
        parameters.Add("subscriptionAmount", subscriptionQuantity);
        parameters.AddEnum("subscriptionCycle", subscriptionCycle);
        parameters.Add("sourceAsset", sourceAsset);
        parameters.Add("details", subscriptionDetails.Select(x => new
        {
            targetAsset = x.Key,
            percentage = x.Value
        }).ToList());
        parameters.AddOptional("subscriptionStartDay", subscriptionStartDay);
        parameters.AddOptional("subscriptionStartWeekday", subscriptionStartWeekday);
        parameters.AddOptional("subscriptionStartTime", subscriptionStartTime);
        parameters.AddOptional("flexibleAllowedToUse", flexibleAllowedToUse);

        return RequestAsync<BinanceAutoInvestEditResult>(GetUrl(sapi, v1, "lending/auto-invest/plan/edit"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAutoInvestRedemptionResult>> RedeemAsync(string indexId, string requestId, int redemptionPercentage, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("indexId", indexId);
        parameters.Add("requestId", requestId);
        parameters.Add("redemptionPercentage", redemptionPercentage);

        return RequestAsync<BinanceAutoInvestRedemptionResult>(GetUrl(sapi, v1, "lending/auto-invest/redeem"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceListRecords<BinanceAutoInvestTransaction>>> GetSubscriptionHistoryAsync(long? planId = null, DateTime? startTime = null, DateTime? endTime = null, string? targetAsset = null, BinanceAutoInvestPlanType? planType = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("planId", planId);
        parameters.AddOptionalMillisecondsString("startTime", startTime);
        parameters.AddOptionalMillisecondsString("endTime", endTime);
        parameters.AddOptional("targetAsset", targetAsset);
        parameters.AddOptionalEnum("planType", planType);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);

        return RequestAsync<BinanceListRecords<BinanceAutoInvestTransaction>>(GetUrl(sapi, v1, "lending/auto-invest/history/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAutoInvestOneTimeTransaction>> GetOneTimeTransactionAsync(long transactionId, string requestId, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("transactionId", transactionId);
        parameters.Add("requestId", requestId);

        return RequestAsync<BinanceAutoInvestOneTimeTransaction>(GetUrl(sapi, v1, "lending/auto-invest/one-off/status"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAutoInvestTradeResult>> CreatePlanAsync(string sourceType, BinanceAutoInvestPlanType planType, decimal subscriptionQuantity, AutoInvestSubscriptionCycle subscriptionCycle, int subscriptionStartTime, string sourceAsset, Dictionary<string, decimal> subscriptionDetails, string? requestId = null, int? subscriptionStartDay = null, string? subscriptionStartWeekday = null, bool? flexibleAllowedToUse = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("sourceType", sourceType);
        parameters.AddEnum("planType", planType);
        parameters.Add("subscriptionAmount", subscriptionQuantity);
        parameters.AddEnum("subscriptionCycle", subscriptionCycle);
        parameters.Add("subscriptionStartTime", subscriptionStartTime);
        parameters.Add("sourceAsset", sourceAsset);
        parameters.Add("details", subscriptionDetails.Select(x => new
        {
            targetAsset = x.Key,
            percentage = x.Value
        }).ToList());
        parameters.AddOptional("requestId", requestId);
        parameters.AddOptional("subscriptionStartDay", subscriptionStartDay);
        parameters.AddOptional("subscriptionStartWeekday", subscriptionStartWeekday);
        parameters.AddOptional("flexibleAllowedToUse", flexibleAllowedToUse);

        return RequestAsync<BinanceAutoInvestTradeResult>(GetUrl(sapi, v1, "lending/auto-invest/plan/add"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceAutoInvestRedemption>>> GetRedemptionHistoryAsync(long requestId, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, string? asset = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("requestId", requestId);
        parameters.AddOptionalMillisecondsString("", startTime);
        parameters.AddOptionalMillisecondsString("", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("asset", asset);

        return RequestAsync<List<BinanceAutoInvestRedemption>>(GetUrl(sapi, v1, "lending/auto-invest/redeem/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAutoInvestHoldings>> GetHoldingsAsync(long? planId = null, string? requestId = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("planId", planId);
        parameters.AddOptional("requestId", requestId);

        return RequestAsync<BinanceAutoInvestHoldings>(GetUrl(sapi, v1, "lending/auto-invest/plan/id"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAutoInvestIndexPlanPosition>> GetPositionAsync(long indexId, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("indexId", indexId);

        return RequestAsync<BinanceAutoInvestIndexPlanPosition>(GetUrl(sapi, v1, "lending/auto-invest/index/user-summary"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceAutoInvestRebalance>>> GetRebalanceHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMillisecondsString("startTime", startTime);
        parameters.AddOptionalMillisecondsString("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);

        return RequestAsync<List<BinanceAutoInvestRebalance>>(GetUrl(sapi, v1, "lending/auto-invest/rebalance/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}