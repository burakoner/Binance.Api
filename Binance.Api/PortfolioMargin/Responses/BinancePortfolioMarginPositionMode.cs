namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Position Mode
/// </summary>
public record BinancePortfolioMarginPositionMode
{
    /// <summary>
    /// Dual Side Position
    /// </summary>
    [JsonProperty("dualSidePosition")]
    public bool DualSidePosition { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Position Mode for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginPositionModeCM : BinancePortfolioMarginPositionMode
{
}

/// <summary>
/// Binance Portfolio Margin Position Mode for USDT-Margined Futures
/// </summary>
public record BinancePortfolioMarginPositionModeUM : BinancePortfolioMarginPositionMode
{
}