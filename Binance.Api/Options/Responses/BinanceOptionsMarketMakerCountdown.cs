namespace Binance.Api.Options;

/// <summary>
/// Binance Options Market Maker Cancel All Countdown
/// </summary>
public record BinanceOptionsMarketMakerCountdown
{
    /// <summary>
    /// Underlying
    /// </summary>
    public string Underlying { get; set; } = "";

    /// <summary>
    /// Countdown Time
    /// </summary>
    public long CountdownTime { get; set; }
}