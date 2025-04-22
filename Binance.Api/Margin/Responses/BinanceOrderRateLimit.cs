namespace Binance.Api.Margin;

/// <summary>
/// Rate limit info
/// </summary>
public record BinanceCurrentRateLimit: BinanceRateLimit
{
    /// <summary>
    /// The current used amount
    /// </summary>
    public int Count { get; set; }
}
