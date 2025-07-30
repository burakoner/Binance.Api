namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Position Risk
/// </summary>
public record BinancePortfolioMarginPositionRisk
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Position Quantity
    /// </summary>
    [JsonProperty("positionAmt")]
    public decimal PositionQuantity { get; set; }

    /// <summary>
    /// Entry Price
    /// </summary>
    [JsonProperty("entryPrice")]
    public decimal EntryPrice { get; set; }

    /// <summary>
    /// Mark Price
    /// </summary>
    [JsonProperty("markPrice")]
    public decimal MarkPrice { get; set; }

    /// <summary>
    /// Unrealized Profit
    /// </summary>
    [JsonProperty("unRealizedProfit")]
    public decimal UnRealizedProfit { get; set; }

    /// <summary>
    /// Liquidation Price
    /// </summary>
    [JsonProperty("liquidationPrice")]
    public decimal LiquidationPrice { get; set; }

    /// <summary>
    /// Leverage
    /// </summary>
    [JsonProperty("leverage")]
    public decimal Leverage { get; set; }

    /// <summary>
    /// Position Side
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

}

/// <summary>
/// Binance Portfolio Margin Position Risk for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginPositionRiskCM : BinancePortfolioMarginPositionRisk
{
    /// <summary>
    /// Maximum Quantity
    /// </summary>
    [JsonProperty("maxQty")]
    public decimal MaximumQuantity { get; set; }

    /// <summary>
    /// Notional Value
    /// </summary>
    [JsonProperty("notionalValue")]
    public decimal Notional { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Position Risk for USDT-Margined Futures
/// </summary>
public record BinancePortfolioMarginPositionRiskUM : BinancePortfolioMarginPositionRisk
{
    /// <summary>
    /// Maximum Notional Value
    /// </summary>
    [JsonProperty("maxNotionalValue")]
    public decimal MaximumNotional { get; set; }

    /// <summary>
    /// Notional Value
    /// </summary>
    [JsonProperty("notional")]
    public decimal Notional { get; set; }
}