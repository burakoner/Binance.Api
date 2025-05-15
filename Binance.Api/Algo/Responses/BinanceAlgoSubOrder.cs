namespace Binance.Api.Algo;

/// <summary>
/// Sub order list
/// </summary>
public record BinanceAlgoSubOrderList
{
    /// <summary>
    /// Amount of sub orders
    /// </summary>
    public int Total { get; set; }

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
    /// Sub orders
    /// </summary>
    public List<BinanceAlgoSubOrder> SubOrders { get; set; } = [];
}

/// <summary>
/// Algo sub order info
/// </summary>
public record BinanceAlgoSubOrder
{
    /// <summary>
    /// Algo id
    /// </summary>
    [JsonProperty("algoId")]
    public long AlgoId { get; set; }

    /// <summary>
    /// Order id
    /// </summary>
    public long OrderId { get; set; }
    
    /// <summary>
    /// Order status
    /// </summary>
    [JsonProperty("orderStatus")]
    public BinanceOrderStatus Status { get; set; }

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
    /// Fee amount
    /// </summary>
    [JsonProperty("feeAmt")]
    public decimal FeeAmount { get; set; }

    /// <summary>
    /// Fee asset
    /// </summary>
    [JsonProperty("feeAsset")]
    public string FeeAsset { get; set; } = string.Empty;

    /// <summary>
    /// Book time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime BookTime { get; set; }

    /// <summary>
    /// Average price
    /// </summary>
    [JsonProperty("avgPrice")]
    public decimal AveragePrice { get; set; }

    /// <summary>
    /// Side
    /// </summary>
    [JsonProperty("side")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Sub id
    /// </summary>
    public long SubId { get; set; }

    /// <summary>
    /// Time in force
    /// </summary>
    public BinanceTimeInForce TimeInForce { get; set; }

    /// <summary>
    /// Original quantity
    /// </summary>
    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }
}
