namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Trading Quantitative Rules Indicators
/// </summary>
public record BinancePortfolioMarginTradingQuantitativeRulesIndicators
{
    /// <summary>
    /// Indicators
    /// </summary>
    [JsonProperty("indicators")]
    public Dictionary<string, BinancePortfolioMarginTradingQuantitativeRulesIndicator> Indicators { get; set; } = [];

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime UpdateTime { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Trading Quantitative Rules Indicator
/// </summary>
public record BinancePortfolioMarginTradingQuantitativeRulesIndicator
{
    /// <summary>
    /// Is Locked
    /// </summary>
    [JsonProperty("isLocked")]
    public bool IsLocked { get; set; }

    /// <summary>
    /// Planned Recover Time
    /// </summary>
    [JsonProperty("plannedRecoverTime")]
    public DateTime PlannedRecoverTime { get; set; }

    /// <summary>
    /// Indicator
    /// </summary>
    [JsonProperty("indicator")]
    public string Indicator { get; set; } = "";

    /// <summary>
    /// Value
    /// </summary>
    [JsonProperty("value")]
    public decimal Value { get; set; }

    /// <summary>
    /// Trigger Value
    /// </summary>
    [JsonProperty("triggerValue")]
    public decimal TriggerValue { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Trading Quantitative Rules Indicators for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginTradingQuantitativeRulesIndicatorsCM : BinancePortfolioMarginTradingQuantitativeRulesIndicators
{
}

/// <summary>
/// Binance Portfolio Margin Trading Quantitative Rules Indicators for USDT-Margined Futures
/// </summary>
public record BinancePortfolioMarginTradingQuantitativeRulesIndicatorsUM : BinancePortfolioMarginTradingQuantitativeRulesIndicators
{
}