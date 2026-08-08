namespace Binance.Api.Spot;

/// <summary>
/// Execution rules for the requested Spot symbols.
/// </summary>
public record BinanceSpotExecutionRules
{
    /// <summary>
    /// Per-symbol execution rules.
    /// </summary>
    [JsonProperty("symbolRules")]
    public List<BinanceSpotSymbolExecutionRules> SymbolRules { get; set; } = [];
}

/// <summary>
/// Execution rules for one Spot symbol.
/// </summary>
public record BinanceSpotSymbolExecutionRules
{
    /// <summary>
    /// Symbol.
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Rules applied by the matching engine.
    /// </summary>
    public List<BinanceSpotExecutionRule> Rules { get; set; } = [];
}

/// <summary>
/// Spot matching-engine execution rule.
/// </summary>
public record BinanceSpotExecutionRule
{
    /// <summary>
    /// Rule type. The current documented value is <c>PRICE_RANGE</c>.
    /// </summary>
    public string RuleType { get; set; } = string.Empty;

    /// <summary>
    /// Maximum bid multiplier.
    /// </summary>
    public decimal BidLimitMultUp { get; set; }

    /// <summary>
    /// Minimum bid multiplier.
    /// </summary>
    public decimal BidLimitMultDown { get; set; }

    /// <summary>
    /// Maximum ask multiplier.
    /// </summary>
    public decimal AskLimitMultUp { get; set; }

    /// <summary>
    /// Minimum ask multiplier.
    /// </summary>
    public decimal AskLimitMultDown { get; set; }
}
