namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin ADL Quantile
/// </summary>
public record BinancePortfolioMarginAdlQuantile
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// ADL Quantile
    /// </summary>
    [JsonProperty("adlQuantile")]
    public Dictionary<BinancePositionSide, int> AdlQuantile { get; set; } = [];
}

/// <summary>
/// Binance Portfolio Margin ADL Quantile for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginAdlQuantileCM: BinancePortfolioMarginAdlQuantile
{
}

/// <summary>
/// Binance Portfolio Margin ADL Quantile for USDT-Margined Futures
/// </summary>
public record BinancePortfolioMarginAdlQuantileUM : BinancePortfolioMarginAdlQuantile
{
}