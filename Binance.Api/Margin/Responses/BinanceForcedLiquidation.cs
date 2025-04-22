namespace Binance.Api.Margin;

/// <summary>
/// Forced liquidation info
/// </summary>
public record BinanceForcedLiquidation
{
    /// <summary>
    /// Average price
    /// </summary>
    [JsonProperty("avgPrice")]
    public decimal AveragePrice { get; set; }

    /// <summary>
    /// The executed quantity
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }

    /// <summary>
    /// Order id
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Total quantity
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Side
    /// </summary>
    [JsonConverter(typeof(OrderSideConverter))]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Time in force
    /// </summary>
    [JsonConverter(typeof(TimeInForceConverter))]
    public BinanceTimeInForce TimeInForce { get; set; }

    /// <summary>
    /// Last update time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("updatedTime")]
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// Is isolated margin
    /// </summary>
    public bool IsIsolated { get; set; }
}
