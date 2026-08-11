namespace Binance.Api.Options;

/// <summary>
/// Binance Options Market Maker Cancel All Countdown
/// </summary>
public record BinanceOptionsMarketMakerCountdown
{
    /// <summary>
    /// Underlying
    /// </summary>
    [JsonProperty("underlying")]
    public string Underlying { get; set; } = string.Empty;

    /// <summary>
    /// Countdown Time
    /// </summary>
    [JsonProperty("countdownTime")]
    public long CountdownTime { get; set; }
}
