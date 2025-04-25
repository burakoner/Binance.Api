namespace Binance.Api.Futures;

/// <summary>
/// Convert symbol info
/// </summary>
public record BinanceFuturesConvertSymbol
{
    /// <summary>
    /// From asset
    /// </summary>
    [JsonProperty("fromAsset")]
    public string FromAsset { get; set; } = string.Empty;

    /// <summary>
    /// To asset
    /// </summary>
    [JsonProperty("toAsset")]
    public string ToAsset { get; set; } = string.Empty;

    /// <summary>
    /// Minimal convert from asset quantity
    /// </summary>
    [JsonProperty("fromAssetMinAmount")]
    public decimal FromAssetMinQuantity { get; set; }

    /// <summary>
    /// Maximal convert from asset quantity
    /// </summary>
    [JsonProperty("fromAssetMaxAmount")]
    public decimal FromAssetMaxQuantity { get; set; }

    /// <summary>
    /// Minimal convert to asset quantity
    /// </summary>
    [JsonProperty("toAssetMinAmount")]
    public decimal ToAssetMinQuantity { get; set; }

    /// <summary>
    /// Maximal convert to asset quantity
    /// </summary>
    [JsonProperty("toAssetMaxAmount")]
    public decimal ToAssetMaxQuantity { get; set; }
}
