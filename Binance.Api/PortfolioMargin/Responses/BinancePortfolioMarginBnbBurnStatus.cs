namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin BNB Burn Status
/// </summary>
public record BinancePortfolioMarginBnbBurnStatus
{
    /// <summary>
    /// Fee Burn
    /// </summary>
    [JsonProperty("feeBurn")]
    public bool FeeBurn { get; set; }
}

/// <summary>
/// Binance Portfolio Margin BNB Burn Status for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginBnbBurnStatusCM : BinancePortfolioMarginBnbBurnStatus
{
}

/// <summary>
/// Binance Portfolio Margin BNB Burn Status for USD-Margined Futures
/// </summary>
public record BinancePortfolioMarginBnbBurnStatusUM : BinancePortfolioMarginBnbBurnStatus
{
}