namespace Binance.Api.Futures;

/// <summary>
/// Native conditional Algo order update
/// </summary>
public record BinanceFuturesStreamAlgoUpdate : BinanceFuturesStreamEvent
{
    /// <summary>
    /// Algo order update data
    /// </summary>
    [JsonProperty("o")]
    public BinanceFuturesStreamAlgoUpdateData UpdateData { get; set; } = new();

    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// The listen key the update was for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;
}

/// <summary>
/// Native conditional Algo order update data
/// </summary>
public record BinanceFuturesStreamAlgoUpdateData
{
    /// <summary>
    /// Client-assigned Algo order ID
    /// </summary>
    [JsonProperty("caid")]
    public string? ClientAlgoId { get; set; }

    /// <summary>
    /// Exchange-assigned Algo order ID
    /// </summary>
    [JsonProperty("aid")]
    public long? AlgoId { get; set; }

    /// <summary>
    /// Algo type
    /// </summary>
    [JsonProperty("at")]
    public string? AlgoType { get; set; }

    /// <summary>
    /// Conditional order type
    /// </summary>
    [JsonProperty("o")]
    public string? OrderType { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string? Symbol { get; set; }

    /// <summary>
    /// Order side
    /// </summary>
    [JsonProperty("S")]
    public string? Side { get; set; }

    /// <summary>
    /// Position side
    /// </summary>
    [JsonProperty("ps")]
    public string? PositionSide { get; set; }

    /// <summary>
    /// Time in force
    /// </summary>
    [JsonProperty("f")]
    public string? TimeInForce { get; set; }

    /// <summary>
    /// Order quantity
    /// </summary>
    [JsonProperty("q")]
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Algo order status
    /// </summary>
    [JsonProperty("X")]
    public string? Status { get; set; }

    /// <summary>
    /// Matching-engine order ID
    /// </summary>
    [JsonProperty("ai")]
    public string? ActualOrderId { get; set; }

    /// <summary>
    /// Matching-engine average fill price, only provided after triggering
    /// </summary>
    [JsonProperty("ap")]
    public decimal? ActualPrice { get; set; }

    /// <summary>
    /// Matching-engine executed quantity, only provided after triggering
    /// </summary>
    [JsonProperty("aq")]
    public decimal? ActualQuantity { get; set; }

    /// <summary>
    /// Actual matching-engine order type, only provided after triggering
    /// </summary>
    [JsonProperty("act")]
    public string? ActualOrderType { get; set; }

    /// <summary>
    /// Trigger price
    /// </summary>
    [JsonProperty("tp")]
    public decimal? TriggerPrice { get; set; }

    /// <summary>
    /// Order price
    /// </summary>
    [JsonProperty("p")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Self-trade prevention mode
    /// </summary>
    [JsonProperty("V")]
    public string? SelfTradePreventionMode { get; set; }

    /// <summary>
    /// Working price type
    /// </summary>
    [JsonProperty("wt")]
    public string? WorkingType { get; set; }

    /// <summary>
    /// Price matching mode
    /// </summary>
    [JsonProperty("pm")]
    public string? PriceMatch { get; set; }

    /// <summary>
    /// Whether the order closes the entire position
    /// </summary>
    [JsonProperty("cp")]
    public bool? ClosePosition { get; set; }

    /// <summary>
    /// Whether trigger-price protection is enabled
    /// </summary>
    [JsonProperty("pP")]
    public bool? PriceProtect { get; set; }

    /// <summary>
    /// Whether the order only reduces a position
    /// </summary>
    [JsonProperty("R")]
    public bool? ReduceOnly { get; set; }

    /// <summary>
    /// Raw trigger time in milliseconds
    /// </summary>
    [JsonProperty("tt")]
    public long? TriggerTime { get; set; }

    /// <summary>
    /// Raw good-till-date time in milliseconds
    /// </summary>
    [JsonProperty("gtd")]
    public long? GoodTillDate { get; set; }

    /// <summary>
    /// Algo order failure reason
    /// </summary>
    [JsonProperty("rm")]
    public string? FailureReason { get; set; }

    /// <summary>
    /// Whether the Algo order has activated. Currently meaningful only for trailing-stop orders
    /// </summary>
    [JsonProperty("ia")]
    public bool? IsActivated { get; set; }
}
