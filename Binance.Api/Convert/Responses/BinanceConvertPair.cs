namespace Binance.Api.Convert;

/// <summary>
/// Binance Convert Exchange Info Pair
/// </summary>
public record BinanceConvertPair
{
    /// <summary>
    /// From Asset
    /// </summary>
    public string FromAsset { get; set; } = "";

    /// <summary>
    /// To Asset
    /// </summary>
    public string ToAsset { get; set; } = "";

    /// <summary>
    /// From Asset Minimum Amount
    /// </summary>
    [JsonProperty("fromAssetMinAmount")]
    public decimal FromAssetMinimumAmount { get; set; }

    /// <summary>
    /// From Asset Maximum Amount
    /// </summary>
    [JsonProperty("fromAssetMaxAmount")]
    public decimal FromAssetMaximumAmount { get; set; }

    /// <summary>
    /// To Asset Minimum Amount
    /// </summary>
    [JsonProperty("toAssetMinAmount")]
    public decimal ToAssetMinimumAmount { get; set; }

    /// <summary>
    /// To Asset Maximum Amount
    /// </summary>
    [JsonProperty("toAssetMaxAmount")]
    public decimal ToAssetMaximumAmount { get; set; }
}
