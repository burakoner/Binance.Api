namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Quantity
/// </summary>
public record BinancePortfolioMarginQuantity
{
    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
}