namespace Binance.Api.Convert;

/// <summary>
/// Convert trade info
/// </summary>
public record BinanceConvertTrade
{
    /// <summary>
    /// Quote id
    /// </summary>
    [JsonProperty("quoteId")]
    public string QuoteId { get; set; } = string.Empty;

    /// <summary>
    /// Order id
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    [JsonProperty("orderStatus")]
    public BinanceConvertOrderStatus Status { get; set; }

    /// <summary>
    /// Quote asset 
    /// </summary>
    [JsonProperty("fromAsset")]
    public string QuoteAsset { get; set; } = string.Empty;

    /// <summary>
    /// Quote quantity
    /// </summary>
    [JsonProperty("fromAmount")]
    public decimal QuoteQuantity { get; set; }

    /// <summary>
    /// Base asset
    /// </summary>
    [JsonProperty("toAsset")]
    public string BaseAsset { get; set; } = string.Empty;

    /// <summary>
    /// Base quantity
    /// </summary>
    [JsonProperty("toAmount")]
    public decimal BaseQuantity { get; set; }

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
    /// Creation time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("createTime")]
    public DateTime CreateTime { get; set; }
}
