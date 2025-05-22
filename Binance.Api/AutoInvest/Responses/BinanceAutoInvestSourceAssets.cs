namespace Binance.Api.AutoInvest;

/// <summary>
/// Auto invest source asset info
/// </summary>
public record BinanceAutoInvestSourceAssets
{
    /// <summary>
    /// Fee rate
    /// </summary>
    [JsonProperty("feeRate")]
    public decimal FeeRate { get; set; }

    /// <summary>
    /// Tax rate
    /// </summary>
    [JsonProperty("taxRate")]
    public decimal TaxRate { get; set; }

    /// <summary>
    /// Source assets
    /// </summary>
    [JsonProperty("sourceAssets")]
    public List<BinanceAutoInvestSourceAssetInfo> SourceAssets { get; set; } = [];
}

/// <summary>
/// BinanceAutoInvestSourceAssetInfo
/// </summary>
public record BinanceAutoInvestSourceAssetInfo
{
    /// <summary>
    /// Source asset
    /// </summary>
    [JsonProperty("sourceAsset")]
    public string SourceAsset { get; set; } = string.Empty;

    /// <summary>
    /// Asset min quantity
    /// </summary>
    [JsonProperty("assetMinAmount")]
    public decimal AssetMinQuantity { get; set; }

    /// <summary>
    /// Asset max quantity
    /// </summary>
    [JsonProperty("assetMaxAmount")]
    public decimal AssetMaxQuantity { get; set; }

    /// <summary>
    /// Scale
    /// </summary>
    [JsonProperty("scale")]
    public decimal Scale { get; set; }

    /// <summary>
    /// Flexible quantity
    /// </summary>
    [JsonProperty("flexibleAmount")]
    public decimal FlexibleQuantity { get; set; }
}
