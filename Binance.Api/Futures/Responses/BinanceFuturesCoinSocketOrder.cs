namespace Binance.Api.Futures;

/// <summary>
/// COIN-M WebSocket API query-order result
/// </summary>
public record BinanceFuturesCoinSocketOrder
{
    /// <summary>
    /// Exchange order id
    /// </summary>
    [JsonProperty("orderId")]
    public long Id { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Pair
    /// </summary>
    [JsonProperty("pair")]
    public string Pair { get; set; } = string.Empty;

    /// <summary>
    /// Order status
    /// </summary>
    [JsonProperty("status")]
    public BinanceOrderStatus Status { get; set; }

    /// <summary>
    /// Client order id
    /// </summary>
    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; } = string.Empty;

    /// <summary>
    /// Client order id without the broker prefix
    /// </summary>
    public string RequestClientOrderId => BinanceHelpers.RemoveBrokerId(ClientOrderId);

    /// <summary>
    /// Order price
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Average execution price
    /// </summary>
    [JsonProperty("avgPrice")]
    public decimal AveragePrice { get; set; }

    /// <summary>
    /// Original order quantity
    /// </summary>
    [JsonProperty("origQty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Executed quantity
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal QuantityFilled { get; set; }

    /// <summary>
    /// Cumulative filled contract quantity
    /// </summary>
    [JsonProperty("cumQty")]
    public decimal CumulativeQuantity { get; set; }

    /// <summary>
    /// Cumulative filled base-asset amount
    /// </summary>
    [JsonProperty("cumBase")]
    public decimal BaseQuantityFilled { get; set; }

    /// <summary>
    /// Time in force
    /// </summary>
    [JsonProperty("timeInForce")]
    public BinanceTimeInForce TimeInForce { get; set; }

    /// <summary>
    /// Order type
    /// </summary>
    [JsonProperty("type")]
    public BinanceFuturesOrderType Type { get; set; }

    /// <summary>
    /// Whether the order only reduces a position
    /// </summary>
    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    /// <summary>
    /// Whether the order closes the complete position
    /// </summary>
    [JsonProperty("closePosition")]
    public bool ClosePosition { get; set; }

    /// <summary>
    /// Order side
    /// </summary>
    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Position side
    /// </summary>
    [JsonProperty("positionSide")]
    public BinancePositionSide PositionSide { get; set; }

    /// <summary>
    /// Stop price
    /// </summary>
    [JsonProperty("stopPrice")]
    public decimal StopPrice { get; set; }

    /// <summary>
    /// Trigger working type
    /// </summary>
    [JsonProperty("workingType")]
    public BinanceFuturesWorkingType WorkingType { get; set; }

    /// <summary>
    /// Whether trigger-price protection is enabled
    /// </summary>
    [JsonProperty("priceProtect")]
    public bool PriceProtect { get; set; }

    /// <summary>
    /// Original order type
    /// </summary>
    [JsonProperty("origType")]
    public BinanceFuturesOrderType OriginalType { get; set; }

    /// <summary>
    /// Self-trade prevention mode
    /// </summary>
    [JsonProperty("selfTradePreventionMode")]
    public BinanceSelfTradePreventionMode SelfTradePreventionMode { get; set; }

    /// <summary>
    /// Order creation time
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Order update time
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// Price matching mode
    /// </summary>
    [JsonProperty("priceMatch")]
    public BinanceFuturesPriceMatch PriceMatch { get; set; }
}
