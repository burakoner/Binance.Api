namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Conditional Order
/// </summary>
public record BinancePortfolioMarginConditionalOrder
{
    /// <summary>
    /// New Client Strategy ID
    /// </summary>
    [JsonProperty("newClientStrategyId")]
    public string NewClientStrategyId { get; set; } = "";

    /// <summary>
    /// Strategy ID
    /// </summary>
    [JsonProperty("strategyId")]
    public long StrategyId { get; set; }

    /// <summary>
    /// Strategy Status
    /// </summary>
    [JsonProperty("strategyStatus")]
    public string StrategyStatus { get; set; } = ""; // TODO: Enum

    /// <summary>
    /// Strategy Type
    /// </summary>
    [JsonProperty("strategyType")]
    public string StrategyType { get; set; } = ""; // TODO: Enum

    /// <summary>
    /// Original Quantity
    /// </summary>
    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Redeuce Only
    /// </summary>
    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    /// <summary>
    /// Side
    /// </summary>
    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Position Side
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    /// <summary>
    /// Stop Price
    /// </summary>
    [JsonProperty("stopPrice")]
    public decimal? StopPrice { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Time in Force
    /// </summary>
    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    /// <summary>
    /// Activate Price
    /// </summary>
    [JsonProperty("activatePrice")]
    public decimal? ActivatePrice { get; set; }

    /// <summary>
    /// Price Rate
    /// </summary>
    [JsonProperty("priceRate")]
    public decimal? PriceRate { get; set; }

    /// <summary>
    /// Book Time
    /// </summary>
    [JsonProperty("bookTime")]
    public DateTime? BookTime { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// Working Type
    /// </summary>
    [JsonProperty("workingType")]
    public string WorkingType { get; set; } = ""; // TODO: Enum

    /// <summary>
    /// Price Protect
    /// </summary>
    [JsonProperty("priceProtect")]
    public bool PriceProtect { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Conditional Order for Cross Margin
/// </summary>
public record BinancePortfolioMarginConditionalOrderCM : BinancePortfolioMarginConditionalOrder
{
    /// <summary>
    /// Pair
    /// </summary>
    [JsonProperty("pair")]
    public string Pair { get; set; } = "";
}

/// <summary>
/// Binance Portfolio Margin Conditional Order for Unified Margin
/// </summary>
public record BinancePortfolioMarginConditionalOrderUM : BinancePortfolioMarginConditionalOrder
{
    /// <summary>
    /// Self Trade Prevention Mode
    /// </summary>
    [JsonProperty("selfTradePreventionMode")]
    public BinanceSelfTradePreventionMode SelfTradePreventionMode { get; set; }

    /// <summary>
    /// Good Till Date
    /// </summary>
    [JsonProperty("goodTillDate")]
    public DateTime? GoodTillDate { get; set; }

    /// <summary>
    /// Price Match
    /// </summary>
    [JsonProperty("priceMatch")]
    public string PriceMatch { get; set; } = ""; // TODO: Enum
}