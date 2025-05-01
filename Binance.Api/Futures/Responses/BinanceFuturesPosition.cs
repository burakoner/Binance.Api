namespace Binance.Api.Futures;

/// <summary>
/// Base position info
/// </summary>
public record BinanceFuturesPositionBase
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Entry price
    /// </summary>
    [JsonProperty("entryPrice")]
    public decimal EntryPrice { get; set; }

    /// <summary>
    /// Leverage
    /// </summary>
    [JsonProperty("leverage")]
    public int Leverage { get; set; }

    /*
    /// <summary>
    /// Unrealized profit
    /// </summary>
    [JsonProperty("unrealizedProfit")]
    public decimal UnrealizedPnl { get; set; }
    [JsonInclude, JsonProperty("unRealizedProfit")]
    internal decimal UnRealizedPnl { set => UnrealizedPnl = value; }
    */

    /// <summary>
    /// Position side
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }
}

/// <summary>
/// Position info
/// </summary>
public record BinanceFuturesPositionInfoBase: BinanceFuturesPositionBase
{
    /// <summary>
    /// Initial margin
    /// </summary>
    [JsonProperty("initialMargin")]
    public decimal InitialMargin { get; set; }

    /// <summary>
    /// Maintenance margin
    /// </summary>
    [JsonProperty("maintMargin")]
    public decimal MaintMargin { get; set; }

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
    /// Isolated
    /// </summary>
    [JsonProperty("isolated")]
    public bool Isolated { get; set; }

    /// <summary>
    /// Position quantity
    /// </summary>
    [JsonProperty("positionAmt")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Last update time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// Usdt position info
/// </summary>
public record BinanceFuturesUsdPositionInfo : BinanceFuturesPositionInfoBase
{
    /// <summary>
    /// Max notional
    /// </summary>
    [JsonProperty("maxNotional")]
    public decimal MaxNotional { get; set; }
}

/// <summary>
/// Coin position info
/// </summary>
public record BinanceFuturesCoinPositionInfo : BinanceFuturesPositionInfoBase
{
    /// <summary>
    /// Break even price
    /// </summary>
    [JsonProperty("breakEvenPrice")]
    public decimal BreakEvenPrice { get; set; }

    /// <summary>
    /// Max quantity
    /// </summary>
    [JsonProperty("maxQty")]
    public decimal MaxQuantity { get; set; }
}

/// <summary>
/// Base position details
/// </summary>
public record BinanceFuturesPositionDetailsBase: BinanceFuturesPositionBase
{
    /// <summary>
    /// Margin type
    /// </summary>
    [JsonProperty("marginType")]
    public BinanceFuturesMarginType MarginType { get; set; }

    /// <summary>
    /// Is auto add margin
    /// </summary>
    [JsonProperty("isAutoAddMargin")]
    public bool IsAutoAddMargin { get; set; }

    /// <summary>
    /// Isolated margin
    /// </summary>
    [JsonProperty("isolatedMargin")]
    public decimal IsolatedMargin { get; set; }

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
    [JsonProperty("positionAmt")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Break even price
    /// </summary>
    [JsonProperty("breakEvenPrice")]
    public decimal BreakEvenPrice { get; set; }

    /// <summary>
    /// Last update time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("updateTime")]
    public DateTime UpdateTime { get; set; }
}

/// <summary>
/// Usdt position details
/// </summary>
public record BinanceFuturesUsdtPosition : BinanceFuturesPositionDetailsBase
{
    /// <summary>
    /// Max notional
    /// </summary>
    [JsonProperty("maxNotionalValue")]
    public decimal MaxNotional { get; set; }

    /// <summary>
    /// Notional value
    /// </summary>
    [JsonProperty("notional")]
    public decimal Notional { get; set; }

    /// <summary>
    /// Isolated wallet
    /// </summary>
    [JsonProperty("isolatedWallet")]
    public decimal IsolatedWallet { get; set; }
}

/// <summary>
/// Coin position info
/// </summary>
public record BinanceFuturesCoinPosition : BinanceFuturesPositionDetailsBase
{
    /// <summary>
    /// Notional value
    /// </summary>
    [JsonProperty("notionalValue")]
    public decimal NotionalValue { get; set; }

    /// <summary>
    /// Max quantity
    /// </summary>
    [JsonProperty("maxQty")]
    public decimal MaxQuantity { get; set; }
}
