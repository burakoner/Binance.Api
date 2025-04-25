namespace Binance.Api.Futures;

/// <summary>
/// Convert quote info
/// </summary>
public record BinanceFuturesConvertQuote
{
    /// <summary>
    /// Quote id
    /// </summary>
    [JsonProperty("quoteId")]
    public string QuoteId { get; set; } = string.Empty;
    /// <summary>
    /// 
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
    /// Until when the quote is valid
    /// </summary>
    [JsonProperty("validTimestamp")]
    public DateTime ValidTimestamp { get; set; }

    /// <summary>
    /// To quantity
    /// </summary>
    [JsonProperty("toAmount")]
    public decimal ToQuantity { get; set; }

    /// <summary>
    /// From quantity
    /// </summary>
    [JsonProperty("fromAmount")]
    public decimal FromQuantity { get; set; }
}
