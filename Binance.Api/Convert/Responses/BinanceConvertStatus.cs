namespace Binance.Api.Convert;

/// <summary>
/// Convert trade info
/// </summary>
public record BinanceConvertStatus
{
    /// <summary>
    /// Order id
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    [JsonProperty("orderStatus")]
    public BinanceConvertOrderStatus Status { get; set; }

    /// <summary>
    /// From asset
    /// </summary>
    public string FromAsset { get; set; } = string.Empty;

    /// <summary>
    /// From quantity
    /// </summary>
    [JsonProperty("fromAmount")]
    public decimal FromQuantity { get; set; }

    /// <summary>
    /// To asset
    /// </summary>
    [JsonProperty("toAsset")]
    public string ToAsset { get; set; } = string.Empty;

    /// <summary>
    /// To quantity
    /// </summary>
    [JsonProperty("toAmount")]
    public decimal ToQuantity { get; set; }

    /// <summary>
    /// Price ratio
    /// </summary>
    public decimal Ratio { get; set; }

    /// <summary>
    /// Inverse price ratio
    /// </summary>
    public decimal InverseRatio { get; set; }

    /// <summary>
    /// Creation time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }
}
