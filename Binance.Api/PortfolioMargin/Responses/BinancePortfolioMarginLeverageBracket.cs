namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Leverage Bracket
/// </summary>
public record BinancePortfolioMarginLeverageBracket
{
    /// <summary>
    /// Symbol for which the leverage bracket applies
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";
}

/// <summary>
/// Binance Portfolio Margin Leverage Bracket Item
/// </summary>
public record BinancePortfolioMarginLeverageBracketItem
{
    /// <summary>
    /// Bracket level, indicating the tier of leverage
    /// </summary>
    [JsonProperty("bracket")]
    public decimal Bracket { get; set; }

    /// <summary>
    /// Initial leverage for the bracket
    /// </summary>
    [JsonProperty("initialLeverage")]
    public decimal InitialLeverage { get; set; }

    /// <summary>
    /// Maintenance margin ratio for the bracket
    /// </summary>
    [JsonProperty("maintMarginRatio")]
    public decimal MaintenanceMarginRatio { get; set; }

    /// <summary>
    /// Cumulated margin ratio for the bracket
    /// </summary>
    [JsonProperty("cum")]
    public decimal Cumulated { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Leverage Bracket for CM (Coin-Margined)
/// </summary>
public record BinancePortfolioMarginLeverageBracketItemCM : BinancePortfolioMarginLeverageBracketItem
{
    /// <summary>
    /// Quantity cap for the bracket
    /// </summary>
    [JsonProperty("qtyCap")]
    public decimal QuantityCap { get; set; }

    /// <summary>
    /// Quantity floor for the bracket
    /// </summary>
    [JsonProperty("qtyCap")]
    public decimal QuantityFloor { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Leverage Bracket for UM (USD-Margined)
/// </summary>
public record BinancePortfolioMarginLeverageBracketItemUM : BinancePortfolioMarginLeverageBracketItem
{
    /// <summary>
    /// Notional cap for the bracket
    /// </summary>
    [JsonProperty("notionalCap")]
    public decimal NotionalCap { get; set; }

    /// <summary>
    /// Notional floor for the bracket
    /// </summary>
    [JsonProperty("notionalFloor")]
    public decimal NotionalFloor { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Leverage Bracket for CM (Coin-Margined) and UM (USD-Margined)
/// </summary>
public record BinancePortfolioMarginLeverageBracketCM : BinancePortfolioMarginLeverageBracket
{
    /// <summary>
    /// Brackets for the leverage tiers
    /// </summary>
    [JsonProperty("brackets")]
    public List<BinancePortfolioMarginLeverageBracketItemCM> Brackets { get; set; } = [];
}

/// <summary>
/// Binance Portfolio Margin Leverage Bracket for UM (USD-Margined)
/// </summary>
public record BinancePortfolioMarginLeverageBracketUM : BinancePortfolioMarginLeverageBracket
{
    /// <summary>
    /// Notional coefficient for the leverage bracket
    /// </summary>
    [JsonProperty("notionalCoef")]
    public decimal NotionalCoefficient { get; set; }

    /// <summary>
    /// Brackets for the leverage tiers
    /// </summary>
    [JsonProperty("brackets")]
    public List<BinancePortfolioMarginLeverageBracketItemUM> Brackets { get; set; } = [];
}