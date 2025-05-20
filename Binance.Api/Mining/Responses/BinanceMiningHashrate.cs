namespace Binance.Api.Mining;

/// <summary>
/// Hash rate
/// </summary>
public record BinanceMiningHashrate
{
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("time")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Hashrate
    /// </summary>
    public decimal HashRate { get; set; }

    /// <summary>
    /// Rejected
    /// </summary>
    public decimal Reject { get; set; }
}
