namespace Binance.Api.Margin;

/// <summary>
/// Small liability history
/// </summary>
public record BinanceMarginSmallLiabilityHistory
{
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Target asset
    /// </summary>
    public string TargetAsset { get; set; } = string.Empty;

    /// <summary>
    /// Target quantity
    /// </summary>
    [JsonProperty("targetAmount")]
    public decimal TargetQuantity { get; set; }

    /// <summary>
    /// Biz type
    /// </summary>
    public string BizType { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}
