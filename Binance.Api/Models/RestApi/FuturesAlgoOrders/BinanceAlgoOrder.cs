namespace Binance.Api.Models.RestApi.FuturesAlgoOrders;

/// <summary>
/// Algo orders
/// </summary>
public record BinanceAlgoOrders
{
    /// <summary>
    /// Total items
    /// </summary>
    public int Total { get; set; }
    /// <summary>
    /// Orders
    /// </summary>
    public IEnumerable<BinanceAlgoOrder> Orders { get; set; } = [];
}

/// <summary>
/// Algo order info
/// </summary>
public record BinanceAlgoOrder
{
    /// <summary>
    /// Algo id
    /// </summary>
    public long AlgoId { get; set; }
    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";
    /// <summary>
    /// Order side
    /// </summary>
    [JsonConverter(typeof(OrderSideConverter))]
    public OrderSide Side { get; set; }
    /// <summary>
    /// Position side
    /// </summary>
    [JsonConverter(typeof(PositionSideConverter))]
    public PositionSide? PositionSide { get; set; }
    /// <summary>
    /// Total quantity
    /// </summary>
    [JsonProperty("totalQty")]
    public decimal TotalQuantity { get; set; }
    /// <summary>
    /// Executed quantity
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }
    /// <summary>
    /// exceuted amount
    /// </summary>
    [JsonProperty("executedAmt")]
    public decimal ExecutedAmount { get; set; }
    /// <summary>
    /// Average price
    /// </summary>
    [JsonProperty("avgPrice")]
    public decimal AveragePrice { get; set; }
    /// <summary>
    /// Client algo id
    /// </summary>
    public string ClientAlgoId { get; set; } = "";
    /// <summary>
    /// Book time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime BookTime { get; set; }
    /// <summary>
    /// End time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? EndTime { get; set; }
    /// <summary>
    /// Status
    /// </summary>
    public string AlgoStatus { get; set; } = "";
    /// <summary>
    /// Algo type
    /// </summary>
    public string AlgoType { get; set; } = "";
    /// <summary>
    /// Urgency
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public OrderUrgency? Urgency { get; set; }
}
