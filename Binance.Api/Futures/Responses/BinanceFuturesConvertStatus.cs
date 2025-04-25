namespace Binance.Api.Futures;

/// <summary>
/// Convert order status
/// </summary>
public record BinanceFuturesConvertStatus
{
    /// <summary>
    /// Order id
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    [JsonProperty("orderStatus")]
    public BinanceFuturesConvertOrderStatus Status { get; set; }

    /// <summary>
    /// From asset
    /// </summary>
    [JsonProperty("fromAsset")]
    public string FromAsset { get; set; } = string.Empty;

    /// <summary>
    /// Quantity in the from asset
    /// </summary>
    [JsonProperty("fromAmount")]
    public decimal FromQuantity { get; set; }

    /// <summary>
    /// To asset
    /// </summary>
    [JsonProperty("toAsset")]
    public string ToAsset { get; set; } = string.Empty;

    /// <summary>
    /// Quantity in the to asset
    /// </summary>
    [JsonProperty("toAmount")]
    public decimal ToQuantity { get; set; }

    /// <summary>
    /// Ratio
    /// </summary>
    [JsonProperty("ratio")]
    public decimal Ratio { get; set; }

    /// <summary>
    /// Inverse ratio
    /// </summary>
    [JsonProperty("inverseRatio")]
    public decimal InverseRatio { get; set; }

    /// <summary>
    /// Create time
    /// </summary>
    [JsonProperty("createTime")]
    public DateTime CreateTime { get; set; }
}
