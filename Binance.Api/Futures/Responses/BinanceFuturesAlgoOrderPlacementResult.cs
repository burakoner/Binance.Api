namespace Binance.Api.Futures;

/// <summary>
/// Native USD-M conditional Algo order placement result
/// </summary>
public record BinanceFuturesAlgoOrderPlacementResult
{
    /// <summary>Exchange-assigned Algo order ID</summary>
    [JsonProperty("algoId")]
    public long AlgoId { get; set; }

    /// <summary>Client-assigned Algo order ID</summary>
    [JsonProperty("clientAlgoId")]
    public string ClientAlgoId { get; set; } = string.Empty;

    /// <summary>Algo type</summary>
    [JsonProperty("algoType")]
    public string AlgoType { get; set; } = string.Empty;

    /// <summary>Conditional order type</summary>
    [JsonProperty("orderType")]
    public string OrderType { get; set; } = string.Empty;

    /// <summary>Symbol</summary>
    [JsonProperty("symbol")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>Order side</summary>
    [JsonProperty("side")]
    public string Side { get; set; } = string.Empty;

    /// <summary>Position side</summary>
    [JsonProperty("positionSide")]
    public string PositionSide { get; set; } = string.Empty;

    /// <summary>Time in force</summary>
    [JsonProperty("timeInForce")]
    public string TimeInForce { get; set; } = string.Empty;

    /// <summary>Order quantity</summary>
    [JsonProperty("quantity")]
    public decimal Quantity { get; set; }

    /// <summary>Algo order status</summary>
    [JsonProperty("algoStatus")]
    public string Status { get; set; } = string.Empty;

    /// <summary>Trigger price</summary>
    [JsonProperty("triggerPrice")]
    public decimal TriggerPrice { get; set; }

    /// <summary>Order price</summary>
    [JsonProperty("price")]
    public decimal Price { get; set; }

    /// <summary>Raw iceberg quantity; Binance may publish the literal string "null"</summary>
    [JsonProperty("icebergQuantity")]
    public string? IcebergQuantity { get; set; }

    /// <summary>Self-trade prevention mode</summary>
    [JsonProperty("selfTradePreventionMode")]
    public string SelfTradePreventionMode { get; set; } = string.Empty;

    /// <summary>Working price type</summary>
    [JsonProperty("workingType")]
    public string WorkingType { get; set; } = string.Empty;

    /// <summary>Price matching mode</summary>
    [JsonProperty("priceMatch")]
    public string PriceMatch { get; set; } = string.Empty;

    /// <summary>Whether the order closes the entire position</summary>
    [JsonProperty("closePosition")]
    public bool ClosePosition { get; set; }

    /// <summary>Whether trigger-price protection is enabled</summary>
    [JsonProperty("priceProtect")]
    public bool PriceProtect { get; set; }

    /// <summary>Whether the order only reduces a position</summary>
    [JsonProperty("reduceOnly")]
    public bool ReduceOnly { get; set; }

    /// <summary>Raw trailing-stop activation price; empty for other order types</summary>
    [JsonProperty("activatePrice")]
    public string ActivatePrice { get; set; } = string.Empty;

    /// <summary>Raw trailing-stop callback rate; empty for other order types</summary>
    [JsonProperty("callbackRate")]
    public string CallbackRate { get; set; } = string.Empty;

    /// <summary>Raw create-time value</summary>
    [JsonProperty("createTime")]
    public long CreateTime { get; set; }

    /// <summary>Raw update-time value</summary>
    [JsonProperty("updateTime")]
    public long UpdateTime { get; set; }

    /// <summary>Raw trigger-time value</summary>
    [JsonProperty("triggerTime")]
    public long TriggerTime { get; set; }

    /// <summary>Raw good-till-date value</summary>
    [JsonProperty("goodTillDate")]
    public long GoodTillDate { get; set; }
}
