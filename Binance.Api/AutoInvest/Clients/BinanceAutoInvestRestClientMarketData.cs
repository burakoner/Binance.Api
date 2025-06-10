namespace Binance.Api.AutoInvest;

internal partial class BinanceAutoInvestRestClient
{
    public Task<RestCallResult<BinanceAutoInvestAssets>> GetAssetsAsync(CancellationToken ct = default)
    {
        return RequestAsync<BinanceAutoInvestAssets>(GetUrl(sapi, v1, "lending/auto-invest/all/asset"), HttpMethod.Get, ct, true, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAutoInvestSourceAssets>> GetSourceAssetsAsync(string usageType, string? targetAsset = null, bool? flexibleAllowedToUse = null, string? sourceType = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("targetAsset", targetAsset);
        parameters.Add("usageType", usageType);
        parameters.AddOptional("flexibleAllowedToUse", flexibleAllowedToUse);
        parameters.AddOptional("sourceType", sourceType);

        return RequestAsync<BinanceAutoInvestSourceAssets>(GetUrl(sapi, v1, "lending/auto-invest/source-asset/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAutoInvestTargetAssets>> GetTargetAssetsAsync(string? targetAsset = null, int? page = null, int? pageSize = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("targetAsset", targetAsset);
        parameters.AddOptional("page", page);
        parameters.AddOptional("pageSize", pageSize);

        return RequestAsync<BinanceAutoInvestTargetAssets>(GetUrl(sapi, v1, "lending/auto-invest/target-asset/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceAutoInvestRoi>>> GetTargetAssetRoisAsync(string asset, AutoInvestRoiType roiType, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("targetAsset", asset);
        parameters.AddEnum("hisRoiType", roiType);

        return RequestAsync<List<BinanceAutoInvestRoi>>(GetUrl(sapi, v1, "lending/auto-invest/target-asset/roi/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAutoInvestIndex>> GetIndexInfoAsync(string indexId, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.Add("indexId", indexId);

        return RequestAsync<BinanceAutoInvestIndex>(GetUrl(sapi, v1, "lending/auto-invest/index/info"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceAutoInvestPlan>> GetPlansAsync(BinanceAutoInvestPlanType planType, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("planType", planType);

        return RequestAsync<BinanceAutoInvestPlan>(GetUrl(sapi, v1, "lending/auto-invest/plan/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}