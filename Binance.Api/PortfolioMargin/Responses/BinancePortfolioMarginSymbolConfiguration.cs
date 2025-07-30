namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Symbol Configuration
/// </summary>
public record BinancePortfolioMarginSymbolConfiguration
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Margin Type
    /// </summary>
    [JsonProperty("marginType")]
    public string MarginType { get; set; } = "";// TODO: Enum

    /// <summary>
    /// Is Auto Add Margin
    /// </summary>
    [JsonProperty("isAutoAddMargin")]
    public bool IsAutoAddMargin { get; set; }

    /// <summary>
    /// Leverage
    /// </summary>
    [JsonProperty("leverage")]
    public decimal Leverage { get; set; }

    /// <summary>
    /// Maximum Leverage
    /// </summary>
    [JsonProperty("maxNotionalValue")]
    public decimal MaximumNotional { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Symbol Configuration for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginSymbolConfigurationCM : BinancePortfolioMarginSymbolConfiguration
{
}

/// <summary>
/// Binance Portfolio Margin Symbol Configuration for USDT-Margined Futures
/// </summary>
public record BinancePortfolioMarginSymbolConfigurationUM : BinancePortfolioMarginSymbolConfiguration
{
}