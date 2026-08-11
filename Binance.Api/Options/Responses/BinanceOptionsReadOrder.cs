namespace Binance.Api.Options;

/// <summary>
/// Fields shared by Options read-order responses
/// </summary>
public abstract record BinanceOptionsReadOrder
{
    /// <summary>
    /// System order identifier
    /// </summary>
    [JsonProperty("orderId")]
    public long Id { get; set; }

    /// <summary>
    /// Option symbol
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Order price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Order quantity
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Executed quantity
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }

    /// <summary>
    /// Order side
    /// </summary>
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Order type
    /// </summary>
    public BinanceOptionsOrderType Type { get; set; }

    /// <summary>
    /// Time in force
    /// </summary>
    public BinanceTimeInForce TimeInForce { get; set; }

    /// <summary>
    /// Whether the order is reduce only
    /// </summary>
    public bool ReduceOnly { get; set; }

    /// <summary>
    /// Order creation time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Last update time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// Order status
    /// </summary>
    public string Status { get; set; } = "";

    /// <summary>
    /// Average fill price
    /// </summary>
    [JsonProperty("avgPrice")]
    public decimal AveragePrice { get; set; }

    /// <summary>
    /// Client order identifier
    /// </summary>
    public string ClientOrderId { get; set; } = "";

    /// <summary>
    /// Price scale
    /// </summary>
    public long PriceScale { get; set; }

    /// <summary>
    /// Quantity scale
    /// </summary>
    public long QuantityScale { get; set; }

    /// <summary>
    /// Option side
    /// </summary>
    [JsonProperty("optionSide")]
    public BinanceOptionsSide OptionSide { get; set; }

    /// <summary>
    /// Quote asset
    /// </summary>
    public string QuoteAsset { get; set; } = "";

    /// <summary>
    /// Whether this is a market-maker-protection order
    /// </summary>
    [JsonProperty("mmp")]
    public bool MMP { get; set; }
}

/// <summary>
/// Options single-order query response
/// </summary>
public record BinanceOptionsOrderQuery : BinanceOptionsReadOrder
{
    /// <summary>
    /// Whether the order is post only
    /// </summary>
    public bool PostOnly { get; set; }

    /// <summary>
    /// Self-trade prevention mode
    /// </summary>
    public BinanceSelfTradePreventionMode SelfTradePreventionMode { get; set; }
}

/// <summary>
/// Options open-order response
/// </summary>
public record BinanceOptionsOpenOrder : BinanceOptionsReadOrder
{
    /// <summary>
    /// Self-trade prevention mode
    /// </summary>
    public BinanceSelfTradePreventionMode SelfTradePreventionMode { get; set; }
}

/// <summary>
/// Options order-history response
/// </summary>
public record BinanceOptionsOrderHistory : BinanceOptionsReadOrder;
