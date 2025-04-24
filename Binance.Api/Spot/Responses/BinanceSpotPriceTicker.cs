namespace Binance.Api.Spot;

/// <summary>
/// The price of a symbol
/// </summary>
public record BinanceSpotPriceTicker
{
    /// <summary>
    /// The symbol the price is for
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The price of the symbol
    /// </summary>
    public decimal Price { get; set; }
}