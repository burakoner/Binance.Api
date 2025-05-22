namespace Binance.Api.AutoInvest;

/// <summary>
/// Interface for the Binance Auto Invest Market Data Rest API client.
/// </summary>
public interface IBinanceAutoInvestRestClientMarketData
{
    /// <summary>
    /// Get auto invest source and target assets
    /// <para><a href="https://developers.binance.com/docs/auto_invest/market-data" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestAssets>> GetAssetsAsync(CancellationToken ct = default);

    /// <summary>
    /// Get source assets info
    /// <para><a href="https://developers.binance.com/docs/auto_invest/market-data/Query-source-asset-list" /></para>
    /// </summary>
    /// <param name="targetAsset">Filter by target asset</param>
    /// <param name="usageType">Usage type, "RECURRING" or "ONE_TIME"</param>
    /// <param name="flexibleAllowedToUse">Allowed to be used for flexible</param>
    /// <param name="sourceType">MAIN_SITE (default) or TR (Turkey users)</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestSourceAssets>> GetSourceAssetsAsync(string usageType, string? targetAsset = null, bool? flexibleAllowedToUse = null, string? sourceType = null, CancellationToken ct = default);

    /// <summary>
    /// Get target assets info
    /// <para><a href="https://developers.binance.com/docs/auto_invest/market-data/Get-target-asset-list" /></para>
    /// </summary>
    /// <param name="targetAsset">Filter by target asset</param>
    /// <param name="page">Current page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAutoInvestTargetAssets>> GetTargetAssetsAsync(string? targetAsset = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

    /// <summary>
    /// Get target asset ROIs
    /// <para><a href="https://developers.binance.com/docs/auto_invest/market-data/Get-target-asset-ROI-data" /></para>
    /// </summary>
    /// <param name="asset">The asset, for example `ETH`</param>
    /// <param name="roiType">ROI type</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<List<BinanceAutoInvestRoi>>> GetTargetAssetRoisAsync(string asset, AutoInvestRoiType roiType, CancellationToken ct = default);

    /// <summary>
    /// Get index info
    /// <para><a href="https://developers.binance.com/docs/auto_invest/market-data/Query-Index-Details" /></para>
    /// </summary>
    /// <param name="indexId">The id</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestIndex>> GetIndexInfoAsync(string indexId, CancellationToken ct = default);

    /// <summary>
    /// Get auto invest plans
    /// <para><a href="https://developers.binance.com/docs/auto_invest/market-data/Get-list-of-plans" /></para>
    /// </summary>
    /// <param name="planType">Type of plans</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceAutoInvestPlan>> GetPlansAsync(BinanceAutoInvestPlanType planType, CancellationToken ct = default);
}