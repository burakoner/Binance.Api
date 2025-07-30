namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Cross Borrowable
/// </summary>
public record BinancePortfolioMarginCrossBorrowable
{
    /// <summary>
    /// Quantity of the asset that can be borrowed
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Limit for borrowing the asset
    /// </summary>
    [JsonProperty("borrowLimit")]
    public decimal Limit { get; set; }
}