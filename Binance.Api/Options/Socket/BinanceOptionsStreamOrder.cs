namespace Binance.Api.Options;

/// <summary>
/// Options order update
/// </summary>
public record BinanceOptionsStreamOrder : BinanceSocketStreamEvent
{
    /// <summary>
    /// The listen key the update was received for
    /// </summary>
    [JsonIgnore]
    public string ListenKey { get; set; } = string.Empty;

    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? TransactionTime { get; set; }

    /// <summary>
    /// Order data
    /// </summary>
    [JsonProperty("o")]
    public BinanceOptionsStreamOrderData Order { get; set; } = new();
}

/// <summary>
/// Options order update data
/// </summary>
public record BinanceOptionsStreamOrderData
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Client order identifier
    /// </summary>
    [JsonProperty("c")]
    public string ClientOrderId { get; set; } = string.Empty;

    /// <summary>
    /// Order side
    /// </summary>
    [JsonProperty("S")]
    public BinanceOrderSide? Side { get; set; }

    /// <summary>
    /// Order type
    /// </summary>
    [JsonProperty("o")]
    public BinanceOptionsOrderType? Type { get; set; }

    /// <summary>
    /// Time in force
    /// </summary>
    [JsonProperty("f")]
    public BinanceTimeInForce? TimeInForce { get; set; }

    /// <summary>
    /// Original quantity
    /// </summary>
    [JsonProperty("q")]
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Original price
    /// </summary>
    [JsonProperty("p")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Average price
    /// </summary>
    [JsonProperty("ap")]
    public decimal? AveragePrice { get; set; }

    /// <summary>
    /// Execution type
    /// </summary>
    [JsonProperty("x")]
    public BinanceOptionsExecutionType? ExecutionType { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    [JsonProperty("X")]
    public BinanceOrderStatus? Status { get; set; }

    /// <summary>
    /// Order identifier
    /// </summary>
    [JsonProperty("i")]
    public long? Id { get; set; }

    /// <summary>
    /// Last filled quantity
    /// </summary>
    [JsonProperty("l")]
    public decimal? LastFilledQuantity { get; set; }

    /// <summary>
    /// Accumulated filled quantity
    /// </summary>
    [JsonProperty("z")]
    public decimal? AccumulatedFilledQuantity { get; set; }

    /// <summary>
    /// Last filled price
    /// </summary>
    [JsonProperty("L")]
    public decimal? LastFilledPrice { get; set; }

    /// <summary>
    /// Commission asset
    /// </summary>
    [JsonProperty("N")]
    public string? CommissionAsset { get; set; }

    /// <summary>
    /// Commission; a negative value represents a fee charge
    /// </summary>
    [JsonProperty("n")]
    public decimal? Commission { get; set; }

    /// <summary>
    /// Order trade time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime? TradeTime { get; set; }

    /// <summary>
    /// Trade identifier
    /// </summary>
    [JsonProperty("t")]
    public long? TradeId { get; set; }

    /// <summary>
    /// Bid quantity
    /// </summary>
    [JsonProperty("b")]
    public decimal? BidQuantity { get; set; }

    /// <summary>
    /// Ask quantity
    /// </summary>
    [JsonProperty("a")]
    public decimal? AskQuantity { get; set; }

    /// <summary>
    /// Whether the trade was on the maker side
    /// </summary>
    [JsonProperty("m")]
    public bool? IsMaker { get; set; }

    /// <summary>
    /// Whether the order is reduce only
    /// </summary>
    [JsonProperty("R")]
    public bool? ReduceOnly { get; set; }

    /// <summary>
    /// Original order type
    /// </summary>
    [JsonProperty("ot")]
    public BinanceOptionsOrderType? OriginalType { get; set; }

    /// <summary>
    /// Realized profit for the trade
    /// </summary>
    [JsonProperty("rp")]
    public decimal? RealizedProfit { get; set; }

    /// <summary>
    /// Self-trade prevention mode
    /// </summary>
    [JsonProperty("V")]
    public BinanceSelfTradePreventionMode? SelfTradePreventionMode { get; set; }
}
