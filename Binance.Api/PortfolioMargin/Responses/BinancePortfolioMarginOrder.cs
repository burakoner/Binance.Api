namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Order
/// </summary>
public record BinancePortfolioMarginOrder
{
    /// <summary>
    /// Client Order ID
    /// </summary>
    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; } = "";

    /// <summary>
    /// Cumulative Quantity
    /// </summary>
    [JsonProperty("cumQty")]
    public decimal CumulativeQuantity { get; set; }

    /// <summary>
    /// Executed Quantity
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }

    /// <summary>
    /// Order ID
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Average Price
    /// </summary>
    [JsonProperty("avgPrice")]
    public decimal AveragePrice { get; set; }

    /// <summary>
    /// Original Quantity
    /// </summary>
    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Reduce Only
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
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinancePortfolioMarginOrderStatus Status { get; set; }

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
    /// Type
    /// </summary>
    [JsonProperty("type")]
    public BinancePortfolioMarginOrderType Type { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Order for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginOrderCM : BinancePortfolioMarginOrder
{
    /// <summary>
    /// Cumulative Base Quantity
    /// </summary>
    [JsonProperty("cumBase")]
    public decimal CumulativeBaseQuantity { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Order for USDT-Margined Futures
/// </summary>
public record BinancePortfolioMarginOrderUM : BinancePortfolioMarginOrder
{
    /// <summary>
    /// Cumulative Quote Quantity
    /// </summary>
    [JsonProperty("cumQuote")]
    public decimal CumulativeQuoteQuantity { get; set; }

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