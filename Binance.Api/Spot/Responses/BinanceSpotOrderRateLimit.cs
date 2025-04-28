namespace Binance.Api.Spot;

/// <summary>
/// Rate limit info
/// </summary>
public record BinanceSpotOrderRateLimit : BinanceRateLimit
{
    /// <summary>
    /// The current used amount
    /// </summary>
    public int Count { get; set; }
}
