namespace Binance.Api.Convert;

/// <summary>
/// Convert Limit Order
/// </summary>
public record BinanceConvertLimitOrder
{
    /// <summary>
    /// Quote id
    /// </summary>
    [JsonProperty("quoteId")]
    public string? QuoteId { get; set; }

    /// <summary>
    /// Price ratio
    /// </summary>
    [JsonProperty("ratio")]
    public decimal Ratio { get; set; }

    /// <summary>
    /// Inverse price ratio
    /// </summary>
    [JsonProperty("inverseRatio")]
    public decimal InverseRatio { get; set; }

    /// <summary>
    /// Valid Timestamp
    /// </summary>
    [JsonProperty("validTimestamp")]
    public long ValidTimestamp { get; set; }

    /// <summary>
    /// Base quantity
    /// </summary>
    [JsonProperty("toAmount")]
    public decimal ToQuantity { get; set; }

    /// <summary>
    /// Quote quantity
    /// </summary>
    [JsonProperty("fromAmount")]
    public decimal FromQuantity { get; set; }

    /// <summary>
    /// Creation time
    /// </summary>
    [JsonProperty("createTime")]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// Expiration time
    /// </summary>
    [JsonProperty("expiredTimestamp")]
    public DateTime? ExpireTime { get; set; }
}
