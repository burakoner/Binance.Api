namespace Binance.Api.AutoInvest;

/// <summary>
/// Auto invest index info
/// </summary>
public record BinanceAutoInvestIndex
{
    /// <summary>
    /// Index id
    /// </summary>
    [JsonProperty("indexId")]
    public long IndexId { get; set; }

    /// <summary>
    /// Index name
    /// </summary>
    [JsonProperty("indexName")]
    public string IndexName { get; set; } = string.Empty;

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceAutoInvestIndexStatus? Status { get; set; }

    /// <summary>
    /// Asset allocation
    /// </summary>
    [JsonProperty("assetAllocation")]
    public List<BinanceAutoInvestAssetIndex> AssetAllocation { get; set; } = [];
}

/// <summary>
/// Allocation
/// </summary>
public record BinanceAutoInvestAssetIndex
{
    /// <summary>
    /// Target asset
    /// </summary>
    [JsonProperty("targetAsset")]
    public string TargetAsset { get; set; } = string.Empty;

    /// <summary>
    /// Allocation
    /// </summary>
    [JsonProperty("allocation")]
    public decimal Allocation { get; set; }
}
