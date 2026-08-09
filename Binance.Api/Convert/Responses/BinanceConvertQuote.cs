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
    /// Quote validity timestamp in Unix milliseconds
    /// </summary>
    [JsonProperty("validTimestamp")]
    public long ValidTimestamp { get; set; }

    /// <summary>
    /// Destination amount
    /// </summary>
    [JsonProperty("toAmount")]
    public decimal ToQuantity { get; set; }

    /// <summary>
    /// Source amount
    /// </summary>
    [JsonProperty("fromAmount")]
    public decimal FromQuantity { get; set; }
}
