namespace Binance.Api.AutoInvest;

/// <summary>
/// Auto invest source asset info
/// </summary>
public record BinanceAutoInvestTargetAssets
{
    /// <summary>
    /// Target assets
    /// </summary>
    [JsonProperty("targetAssets")]
    public List<string> TargetAssets { get; set; } = [];

    /// <summary>
    /// Target asset list
    /// </summary>
    [JsonProperty("autoInvestAssetList")]
    public List<BinanceAutoInvestTargetAsset> Assets { get; set; } = [];
}

/// <summary>
/// Auto invest target asset
/// </summary>
public record BinanceAutoInvestTargetAsset
{
    /// <summary>
    /// Target asset
    /// </summary>
    [JsonProperty("targetAsset")]
    public string TargetAsset { get; set; } = string.Empty;

    /// <summary>
    /// Target asset list
    /// </summary>
    [JsonProperty("roiAndDimensionTypeList")]
    public List<BinanceAutoInvestTargetAssetRoi> Assets { get; set; } = [];
}

/// <summary>
/// Auto invest target asset roi
/// </summary>
public record BinanceAutoInvestTargetAssetRoi
{
    /// <summary>
    /// Simulate ROI
    /// </summary>
    [JsonProperty("simulateRoi")]
    public decimal SimulateRoi { get; set; }

    /// <summary>
    /// The dimension
    /// </summary>
    [JsonProperty("dimensionValue")]
    public decimal DimensionValue { get; set; }

    /// <summary>
    /// The dimension unit
    /// </summary>
    [JsonProperty("dimensionUnit")]
    public string DimensionUnit { get; set; } = string.Empty;
}
