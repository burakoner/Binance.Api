namespace Binance.Api.Convert;

/// <summary>
/// Open Convert limit order
/// </summary>
public record BinanceConvertOpenOrder
{
    /// <summary>
    /// Quote id
    /// </summary>
    public string QuoteId { get; set; } = string.Empty;

    /// <summary>
    /// Order id
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    [JsonProperty("orderStatus")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Source asset
    /// </summary>
    public string FromAsset { get; set; } = string.Empty;

    /// <summary>
    /// Source quantity
    /// </summary>
    [JsonProperty("fromAmount")]
    public decimal FromQuantity { get; set; }

    /// <summary>
    /// Destination asset
    /// </summary>
    public string ToAsset { get; set; } = string.Empty;

    /// <summary>
    /// Destination quantity
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

    /// <summary>
    /// Expiration time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("expiredTimestamp")]
    public DateTime ExpireTime { get; set; }
}
