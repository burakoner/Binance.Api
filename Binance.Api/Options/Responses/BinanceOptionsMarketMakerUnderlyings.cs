namespace Binance.Api.Options;

/// <summary>
/// Binance Options Market Maker Underlyings
/// </summary>
public record BinanceOptionsMarketMakerUnderlyings
{
    /// <summary>
    /// Underlyings
    /// </summary>
    public List<string> Underlyings { get; set; } = [];
}