namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Auto Repay
/// </summary>
public record BinancePortfolioMarginAutoRepay
{
    /// <summary>
    /// Auto Repay
    /// </summary>
    [JsonProperty("autoRepay")]
    public bool AutoRepay { get; set; }
}