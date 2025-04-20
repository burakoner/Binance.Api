﻿namespace Binance.Api.Models.RestApi.FuturesAlgoOrders;

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
    public IEnumerable<BinanceAlgoSubOrder> SubOrders { get; set; } = [];
}

/// <summary>
/// Algo sub order info
/// </summary>
public record BinanceAlgoSubOrder
{
    /// <summary>
    /// Algo id
    /// </summary>
    public long AlgoId { get; set; }
    /// <summary>
    /// Order id
    /// </summary>
    public long OrderId { get; set; }
    /// <summary>
    /// Order status
    /// </summary>
    [JsonProperty("orderStatus")]
    [JsonConverter(typeof(OrderStatusConverter))]
    public OrderStatus Status { get; set; }
    /// <summary>
    /// Executed quantity
    /// </summary>
    [JsonProperty("executedQty")]
    public decimal ExecutedQuantity { get; set; }
    /// <summary>
    /// Exceuted amount
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
    public string FeeAsset { get; set; } = "";
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
    [JsonConverter(typeof(OrderSideConverter))]
    public BinanceSpotOrderSide Side { get; set; }
    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = "";
    /// <summary>
    /// Sub id
    /// </summary>
    public long SubId { get; set; }
    /// <summary>
    /// Time in force
    /// </summary>
    public string TimeInForce { get; set; } = "";
    /// <summary>
    /// Original quantity
    /// </summary>
    [JsonProperty("origQty")]
    public decimal OriginalQuantity { get; set; }
}
