namespace Binance.Api.SubAccount;

/// <summary>
/// Sub account position risk
/// </summary>
public record BinanceSubAccountFuturesPositionRisk
{
    /// <summary>
    /// The entry price
    /// </summary>
    [JsonProperty("entryPrice")]
    public decimal EntryPrice { get; set; }

    /// <summary>
    /// Leverage
    /// </summary>
    [JsonProperty("leverage")]
    public decimal Leverage { get; set; }

    /// <summary>
    /// Max notional
    /// </summary>
    [JsonProperty("maxNotional")]
    public decimal MaxNotional { get; set; }

    /// <summary>
    /// Liquidation price
    /// </summary>
    [JsonProperty("liquidationPrice")]
    public decimal LiquidationPrice { get; set; }

    /// <summary>
    /// Mark price
    /// </summary>
    [JsonProperty("markPrice")]
    public decimal MarkPrice { get; set; }

    /// <summary>
    /// Position quantity
    /// </summary>
    [JsonProperty("positionAmount")]
    public decimal PositionQuantity { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Unrealized profit
    /// </summary>
    [JsonProperty("unrealizedProfit")]
    public decimal UnrealizedProfit { get; set; }
}
