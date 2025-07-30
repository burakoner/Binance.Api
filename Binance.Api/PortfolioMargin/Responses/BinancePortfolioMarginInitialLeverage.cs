namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Initial Leverage
/// </summary>
public record BinancePortfolioMarginInitialLeverage
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Leverage
    /// </summary>
    [JsonProperty("leverage")]
    public decimal Leverage { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Initial Leverage for CM (Coin-Margined)
/// </summary>
public record BinancePortfolioMarginInitialLeverageCM : BinancePortfolioMarginPositionRisk
{
    /// <summary>
    /// Maximum Quantity
    /// </summary>
    [JsonProperty("maxQty")]
    public decimal MaximumQuantity { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Initial Leverage for UM (USD-Margined)
/// </summary>
public record BinancePortfolioMarginInitialLeverageUM : BinancePortfolioMarginPositionRisk
{
    /// <summary>
    /// Maximum Notional Value
    /// </summary>
    [JsonProperty("maxNotionalValue")]
    public decimal MaximumNotional { get; set; }
}