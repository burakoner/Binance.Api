namespace Binance.Api.Futures;

/// <summary>
/// Position information
/// </summary>
public record BinanceFuturesPositionV3
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Position side
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    /// <summary>
    /// Position amount
    /// </summary>
    [JsonProperty("positionAmt")]
    public decimal PositionAmt { get; set; }

    /// <summary>
    /// Entry price
    /// </summary>
    [JsonProperty("entryPrice")]
    public decimal EntryPrice { get; set; }

    /// <summary>
    /// Break even price
    /// </summary>
    [JsonProperty("breakEvenPrice")]
    public decimal BreakEvenPrice { get; set; }

    /// <summary>
    /// Mark price
    /// </summary>
    [JsonProperty("markPrice")]
    public decimal MarkPrice { get; set; }

    /// <summary>
    /// Unrealized profit
    /// </summary>
    [JsonProperty("unRealizedProfit")]
    public decimal UnrealizedProfit { get; set; }

    /// <summary>
    /// Liquidation price
    /// </summary>
    [JsonProperty("liquidationPrice")]
    public decimal LiquidationPrice { get; set; }

    /// <summary>
    /// Isolated margin
    /// </summary>
    [JsonProperty("isolatedMargin")]
    public decimal IsolatedMargin { get; set; }

    /// <summary>
    /// Notional
    /// </summary>
    [JsonProperty("notional")]
    public decimal Notional { get; set; }

    /// <summary>
    /// Margin asset
    /// </summary>
    [JsonProperty("marginAsset")]
    public string MarginAsset { get; set; } = string.Empty;

    /// <summary>
    /// Isolated wallet
    /// </summary>
    [JsonProperty("isolatedWallet")]
    public decimal IsolatedWallet { get; set; }

    /// <summary>
    /// Initial margin
    /// </summary>
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Maintenance margin
    /// </summary>
    [JsonProperty("maintMargin")]
    public decimal MaintenanceMargin { get; set; }

    /// <summary>
    /// Position initial margin
    /// </summary>
    [JsonProperty("positionInitialMargin")]
    public decimal PositionInitialMargin { get; set; }

    /// <summary>
    /// Open order initial margin
    /// </summary>
    [JsonProperty("openOrderInitialMargin")]
    public decimal OpenOrderInitialMargin { get; set; }

    /// <summary>
    /// Auto deleverage
    /// </summary>
    [JsonProperty("adl")]
    public decimal Adl { get; set; }

    /// <summary>
    /// Bid notional
    /// </summary>
    [JsonProperty("bidNotional")]
    public decimal BidNotional { get; set; }

    /// <summary>
    /// Ask notional
    /// </summary>
    [JsonProperty("askNotional")]
    public decimal AskNotional { get; set; }

    /// <summary>
    /// Update time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Leverage
    /// </summary>
    [JsonProperty("leverage")]
    public decimal? Leverage { get; set; }

    /// <summary>
    /// Max notional value of the position
    /// </summary>
    [JsonProperty("maxNotionalValue")]
    public decimal? MaxNotionalValue { get; set; }

    /// <summary>
    /// Max notional value of the position
    /// </summary>
    [JsonProperty("marginType")]
    public BinanceFuturesMarginType? MarginType { get; set; }

    /// <summary>
    /// Is auto add margin enabled
    /// </summary>
    [JsonProperty("isAutoAddMargin")]
    public bool? IsAutoAddMargin { get; set; }
}
