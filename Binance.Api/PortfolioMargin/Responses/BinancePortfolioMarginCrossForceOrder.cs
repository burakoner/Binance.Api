namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Binance Portfolio Margin Cross Force Order
/// </summary>
public record BinancePortfolioMarginCrossForceOrder
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
    /// Price at which the order was placed
    /// </summary>
    [JsonProperty("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Average Price
    /// </summary>
    [JsonProperty("avgPrice")]
    public decimal? AveragePrice { get; set; }

    /// <summary>
    /// Quantity of the asset ordered
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

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
    /// Side
    /// </summary>
    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Update Time
    /// </summary>
    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}