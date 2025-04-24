namespace Binance.Api.Futures;

/// <summary>
/// Long Short Ratio Info
/// </summary>
public record BinanceFuturesLongShortRatio
{
    /// <summary>
    /// The symbol or pair the information is about
    /// </summary>
    [JsonProperty("symbol")]
    public string? Symbol { get; set; }

    /// <summary>
    /// Pair
    /// </summary>
    [JsonProperty("pair")]
    public string? Pair { get; set; }

    /// <summary>
    /// long/short ratio
    /// </summary>
    [JsonProperty("longShortRatio")]
    public decimal LongShortRatio { get; set; }

    /// <summary>
    /// longs percentage (in decimal form)
    /// </summary>
    [JsonProperty("longAccount")]
    public decimal LongAccount { get; set; }

    /// <summary>
    /// shorts percentage (in decimal form)
    /// </summary>
    [JsonProperty("shortAccount")]
    public decimal ShortAccount { get; set; }
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? Timestamp { get; set; }
}