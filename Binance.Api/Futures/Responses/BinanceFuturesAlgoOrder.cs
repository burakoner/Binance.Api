namespace Binance.Api.Futures;

/// <summary>
/// Shared native USD-M conditional Algo order information
/// </summary>
public record BinanceFuturesAlgoOrderInfo
{
    /// <summary>
    /// Exchange-assigned Algo order ID
    /// </summary>
    [JsonProperty("algoId")]
    public long AlgoId { get; set; }

    /// <summary>
    /// Client-assigned Algo order ID
    /// </summary>
    [JsonProperty("clientAlgoId")]
    public string ClientAlgoId { get; set; } = string.Empty;

    /// <summary>
    /// Algo type
    /// </summary>
    [JsonProperty("algoType")]
    public string AlgoType { get; set; } = string.Empty;

    /// <summary>
    /// Conditional order type
    /// </summary>
    [JsonProperty("orderType")]
    public string OrderType { get; set; } = string.Empty;

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Order side
    /// </summary>
    [JsonProperty("side")]
    public string Side { get; set; } = string.Empty;

    /// <summary>
    /// Position side
    /// </summary>
    [JsonProperty("positionSide")]
    public string PositionSide { get; set; } = string.Empty;

    /// <summary>
    /// Time in force
    /// </summary>
    [JsonProperty("timeInForce")]
    public string TimeInForce { get; set; } = string.Empty;

    /// <summary>
    /// Order quantity
    /// </summary>
    [JsonProperty("quantity")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Algo order status
    /// </summary>
    [JsonProperty("algoStatus")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Triggered order ID, or an empty string when not triggered
    /// </summary>
    [JsonProperty("actualOrderId")]
    public string ActualOrderId { get; set; } = string.Empty;

    /// <summary>
    /// Triggered order average price, or zero when not triggered
    /// </summary>
    [JsonProperty("actualPrice")]
    public decimal ActualPrice { get; set; }

    /// <summary>
    /// Trigger price
    /// </summary>
    [JsonProperty("triggerPrice")]
    public decimal TriggerPrice { get; set; }

    /// <summary>
    /// Order price
    /// </summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Raw iceberg quantity value. Binance may publish the literal string "null"
    /// </summary>
    [JsonProperty("icebergQuantity")]
    public string? IcebergQuantity { get; set; }

    /// <summary>
    /// Take-profit order type
    /// </summary>
    [JsonProperty("tpOrderType")]
    public string TakeProfitOrderType { get; set; } = string.Empty;

    /// <summary>
    /// Self-trade prevention mode
    /// </summary>
    [JsonProperty("selfTradePreventionMode")]
    public string SelfTradePreventionMode { get; set; } = string.Empty;

    /// <summary>
    /// Working price type
    /// </summary>
    [JsonProperty("workingType")]
    public string WorkingType { get; set; } = string.Empty;

    /// <summary>
    /// Price matching mode
    /// </summary>
    [JsonProperty("priceMatch")]
    public string PriceMatch { get; set; } = string.Empty;

    /// <summary>
    /// Whether the order closes the entire position
    /// </summary>
    [JsonProperty("closePosition")]
    public bool ClosePosition { get; set; }

    /// <summary>
    /// Whether trigger-price protection is enabled
    /// </summary>
    [JsonProperty("priceProtect")]
    public bool PriceProtect { get; set; }

    /// <summary>
    /// Whether the order only reduces a position
    /// </summary>
    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    /// <summary>
    /// Raw create-time value
    /// </summary>
    [JsonProperty("createTime")]
    public long CreateTime { get; set; }

    /// <summary>
    /// Raw update-time value
    /// </summary>
    [JsonProperty("updateTime")]
    public long UpdateTime { get; set; }

    /// <summary>
    /// Raw trigger-time value
    /// </summary>
    [JsonProperty("triggerTime")]
    public long TriggerTime { get; set; }

    /// <summary>
    /// Raw good-till-date value
    /// </summary>
    [JsonProperty("goodTillDate")]
    public long GoodTillDate { get; set; }
}

/// <summary>
/// Native USD-M conditional Algo order query result
/// </summary>
public record BinanceFuturesAlgoOrder : BinanceFuturesAlgoOrderInfo
{
    /// <summary>
    /// Triggered order type, present only after triggering
    /// </summary>
    [JsonProperty("actualType")]
    public string? ActualOrderType { get; set; }

    /// <summary>
    /// Triggered order quantity, present only after a fill or partial fill
    /// </summary>
    [JsonProperty("actualQty")]
    public decimal? ActualQuantity { get; set; }
}

/// <summary>
/// Native USD-M conditional Algo order list item
/// </summary>
public record BinanceFuturesAlgoOrderListItem : BinanceFuturesAlgoOrderInfo
{
    /// <summary>
    /// Take-profit trigger price
    /// </summary>
    [JsonProperty("tpTriggerPrice")]
    public decimal TakeProfitTriggerPrice { get; set; }

    /// <summary>
    /// Take-profit order price
    /// </summary>
    [JsonProperty("tpPrice")]
    public decimal TakeProfitPrice { get; set; }

    /// <summary>
    /// Stop-loss trigger price
    /// </summary>
    [JsonProperty("slTriggerPrice")]
    public decimal StopLossTriggerPrice { get; set; }

    /// <summary>
    /// Stop-loss order price
    /// </summary>
    [JsonProperty("slPrice")]
    public decimal StopLossPrice { get; set; }
}
