namespace Binance.Api.Models.RestApi.PortfolioMargin;

/// <summary>
/// Bankruptcy loan info
/// </summary>
public record BinancePortfolioMarginLoan
{
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; } = "";
    /// <summary>
    /// Loan amount
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
}
