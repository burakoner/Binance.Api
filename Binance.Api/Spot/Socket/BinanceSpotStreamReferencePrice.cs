namespace Binance.Api.Spot;

/// <summary>
/// Reference-price stream update.
/// </summary>
public record BinanceSpotStreamReferencePrice
{
    /// <summary>
    /// Event type.
    /// </summary>
    [JsonProperty("e")]
    public string Event { get; set; } = string.Empty;

    /// <summary>
    /// Symbol.
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Reference price, or <see langword="null"/> when no reference price is set.
    /// </summary>
    [JsonProperty("r")]
    public decimal? ReferencePrice { get; set; }

    /// <summary>
    /// Matching-engine time at which the reference price was valid.
    /// </summary>
    [JsonProperty("t"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}
