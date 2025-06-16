namespace Binance.Api.Options;

/// <summary>
/// Binance Options Market Maker Auto Cancel All Configuration
/// </summary>
public record BinanceOptionsMarketMakerAutoCancelAll
{
    /// <summary>
    /// Underlying
    /// </summary>
    public string Underlying { get; set; } = "";

    /// <summary>
    /// Countdown Time
    /// </summary>
    [JsonProperty("countdownTime")]
    public long CountdownTime { get; set; }
}