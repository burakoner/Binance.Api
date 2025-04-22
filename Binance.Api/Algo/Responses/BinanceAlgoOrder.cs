namespace Binance.Api.Algo;

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
    [JsonProperty("algoId")]
    public long AlgoId { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Order side
    /// </summary>
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Position side
    /// </summary>
    public BinancePositionSide? PositionSide { get; set; }

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
    /// Executed amount
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
    public string ClientAlgoId { get; set; } = string.Empty;

    /// <summary>
    /// The order id as assigned by the client without the prefix
    /// </summary>
    public string RequestClientAlgoId => ClientAlgoId
        .TrimStart(BinanceConstants.ClientOrderIdPrefixSpot.ToCharArray())
        .TrimStart(BinanceConstants.ClientOrderIdPrefixFutures.ToCharArray());

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
    [JsonProperty("algoStatus")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Algo type
    /// </summary>
    [JsonProperty("algoType")]
    public BinanceAlgoType Type { get; set; }

    /// <summary>
    /// Urgency
    /// </summary>
    public BinanceUrgency? Urgency { get; set; }
}
