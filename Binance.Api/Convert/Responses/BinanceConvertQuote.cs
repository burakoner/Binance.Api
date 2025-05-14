namespace Binance.Api.Convert;

/// <summary>
/// Convert Quote
/// </summary>
public record BinanceConvertQuote
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
}
