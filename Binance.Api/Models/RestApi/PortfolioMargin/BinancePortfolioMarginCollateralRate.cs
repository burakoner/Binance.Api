namespace Binance.ApiClient.Models.RestApi.PortfolioMargin;

/// <summary>
/// Portfolio margin collateral rate info
/// </summary>
public class BinancePortfolioMarginCollateralRate
{
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; }

    /// <summary>
    /// Collateral rate
    /// </summary>
    public decimal CollateralRate { get; set; }
}
