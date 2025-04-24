namespace Binance.Api.Futures;

/// <summary>
/// Parameters for a new futures batch order
/// </summary>
public record BinanceFuturesBatchOrderRequest
{
    /// <summary>
    /// Symbol of the order
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Side of the order
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Default Both for One-way Mode ; Long or Short for Hedge Mode. It must be sent with Hedge Mode.
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinancePositionSide? PositionSide { get; set; }

    /// <summary>
    /// Order type
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceFuturesOrderType Type { get; set; }

    /// <summary>
    /// Time in force
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceTimeInForce? TimeInForce { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public decimal? Quantity { get; set; }

    /// <summary>
    /// Reduce only, default false
    /// </summary>
    public bool? ReduceOnly { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// A unique id among open orders. Automatically generated if not sent.
    /// </summary>
    public string NewClientOrderId { get; set; } = string.Empty;

    /// <summary>
    /// Used with Stop/StopMarket or TakeProfit/TakeProfitMarket orders.
    /// </summary>
    public decimal? StopPrice { get; set; }

    /// <summary>
    /// Used with TrailingStopMarket orders, default as the latest price（supporting different workingType)
    /// </summary>
    public decimal? ActivationPrice { get; set; }

    /// <summary>
    /// Used with TrailingStopMarket orders, min 0.1, max 4 where 1 for 1%
    /// </summary>
    public decimal? CallbackRate { get; set; }

    /// <summary>
    /// Stop price triggered by: Mark or Contract. Default Contract
    /// </summary>
    public WorkingType? WorkingType { get; set; }

    /// <summary>
    /// Used with Stop/StopMarket or TakeProfit/TakeProfitMarket orders.
    /// </summary>
    public bool? PriceProtect { get; set; }

    /// <summary>
    /// Price match
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceFuturesPriceMatch? PriceMatch { get; set; }

    /// <summary>
    /// Self trade prevention mode
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceSelfTradePreventionMode? SelfTradePreventionMode { get; set; }
}
