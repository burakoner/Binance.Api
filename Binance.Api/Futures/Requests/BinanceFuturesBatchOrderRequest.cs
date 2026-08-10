namespace Binance.Api.Futures;

/// <summary>
/// Parameters for a normal LIMIT or MARKET futures batch order
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
    /// Normal order type: LIMIT or MARKET
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
