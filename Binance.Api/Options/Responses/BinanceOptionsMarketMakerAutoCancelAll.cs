namespace Binance.Api.Options;

/// <summary>
/// Binance Options Market Maker Auto Cancel All Configuration
/// </summary>
public record BinanceOptionsMarketMakerAutoCancelAll
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
