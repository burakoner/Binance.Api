namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Force Order
/// </summary>
public record BinancePortfolioMarginForceOrder
{
    /// <summary>
    /// Order ID
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinancePortfolioMarginOrderStatus Status { get; set; }

    /// <summary>
    /// Client Order ID
    /// </summary>
    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; } = "";

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Average Price
    /// </summary>
    [JsonProperty("avgPrice")]
    public decimal? AveragePrice { get; set; }

    /// <summary>
    /// Original Quantity
    /// </summary>
    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }

    /// <summary>
    /// Executed Quantity
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }

    /// <summary>
    /// Time in Force
    /// </summary>
    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    /// <summary>
    /// The type of the order
    /// </summary>
    [JsonProperty("type")]
    public BinancePortfolioMarginOrderType Type { get; set; }

    /// <summary>
    /// Reduce Only
    /// </summary>
    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    /// <summary>
    /// Side of the order
    /// </summary>
    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Position Side
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    /// <summary>
    /// Original Type
    /// </summary>
    [JsonProperty("origType")]
    public BinancePortfolioMarginOrderType OriginalType { get; set; }

    /// <summary>
    /// Time
    /// </summary>
    [JsonProperty("time")]
    public DateTime Time { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}

/// <summary>
/// Binance Portfolio Margin Force Order for Coin-Margined Futures
/// </summary>
public record BinancePortfolioMarginForceOrderCM: BinancePortfolioMarginForceOrder
{
    /// <summary>
    /// Pair
    /// </summary>
    [JsonProperty("pair")]
    public string Pair { get; set; } = "";

    /// <summary>
    /// Cummulative Base Quantity
    /// </summary>
    [JsonProperty("cumBase")]
    public decimal CummulativeBaseQuantity { get; set; }

}

/// <summary>
/// Binance Portfolio Margin Force Order for USDT-Margined Futures
/// </summary>
public record BinancePortfolioMarginForceOrderUM : BinancePortfolioMarginForceOrder
{
    /// <summary>
    /// Cummulative Base Quantity
    /// </summary>
    [JsonProperty("cumQuote")]
    public decimal CummulativeQuoteQuantity { get; set; }
}