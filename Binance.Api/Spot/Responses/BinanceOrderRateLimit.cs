namespace Binance.Api.Spot;

/// <summary>
/// Rate limit info
/// </summary>
public record BinanceOrderRateLimit : BinanceRateLimit
{
    /// <summary>
    /// The current used amount
    /// </summary>
    public int Count { get; set; }
}
