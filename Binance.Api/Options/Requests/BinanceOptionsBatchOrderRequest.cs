namespace Binance.Api.Options;

/// <summary>
/// Parameters for a new options batch order
/// </summary>
public record BinanceOptionsBatchOrderRequest
{
    /// <summary>
    /// Creates a new Options batch order request
    /// </summary>
    /// <param name="symbol">Option symbol</param>
    /// <param name="side">Buy or sell side</param>
    /// <param name="type">Order type; only LIMIT is supported</param>
    /// <param name="quantity">Order quantity</param>
    public BinanceOptionsBatchOrderRequest(string symbol, BinanceOrderSide side, BinanceOptionsOrderType type, decimal quantity)
    {
        Symbol = symbol;
        Side = side;
        Type = type;
        Quantity = quantity;
    }

    /// <summary>
    /// Symbol of the order
    /// </summary>
    public string Symbol { get; set; }

    /// <summary>
    /// Side of the order
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Order Type
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceOptionsOrderType Type { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Time in force
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceTimeInForce? TimeInForce { get; set; }

    /// <summary>
    /// A unique id among open orders. Automatically generated if not sent.
    /// </summary>
    public string ClientOrderId { get; set; } = string.Empty;

    /// <summary>
    /// Reduce only, default false
    /// </summary>
    public bool? ReduceOnly { get; set; }

    /// <summary>
    /// Post only
    /// </summary>
    public bool? PostOnly { get; set; }

    /// <summary>
    /// Is MMP Order
    /// </summary>
    public bool? MMP { get; set; }

    /// <summary>
    /// Response type. ACK or RESULT; default is ACK
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceOrderResponseType? OrderResponseType { get; set; }

    /// <summary>
    /// Self-trade prevention mode
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceSelfTradePreventionMode? SelfTradePreventionMode { get; set; }
}
