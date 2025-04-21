using Binance.Api.Spot;

namespace Binance.Api.Models.WebSocketApi.Futures;

/// <summary>
/// A event received by a Binance websocket
/// </summary>
public record BinanceFuturesStreamLiquidationData : BinanceSocketEvent
{
    /// <summary>
    /// The data of the event
    /// </summary>
    [JsonProperty("o")]
    public BinanceFuturesStreamLiquidation Data { get; set; } = default!;
}

/// <summary>
/// 
/// </summary>
public record BinanceFuturesStreamLiquidation : IBinanceFuturesLiquidation
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// Liquidation Sided
    /// </summary>
    [JsonProperty("S"), JsonConverter(typeof(OrderSideConverter))]
    public BinanceSpotOrderSide Side { get; set; }

    /// <summary>
    /// Liquidation order type
    /// </summary>
    [JsonProperty("o"), JsonConverter(typeof(FuturesOrderTypeConverter))]
    public FuturesOrderType Type { get; set; }

    /// <summary>
    /// Liquidation Time in Force
    /// </summary>
    [JsonProperty("f"), JsonConverter(typeof(TimeInForceConverter))]
    public BinanceSpotTimeInForce TimeInForce { get; set; }

    /// <summary>
    /// Liquidation Original Quantity
    /// </summary>
    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Liquidation order price
    /// </summary>
    [JsonProperty("p")]
    public decimal Price { get; set; }

    /// <summary>
    /// Liquidation Average Price
    /// </summary>
    [JsonProperty("ap")]
    public decimal AveragePrice { get; set; }

    /// <summary>
    /// Liquidation Order Status
    /// </summary>
    [JsonProperty("X"), JsonConverter(typeof(OrderStatusConverter))]
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Liquidation Last Filled Quantity
    /// </summary>
    [JsonProperty("l")]
    public decimal LastQuantityFilled { get; set; }

    /// <summary>
    /// Liquidation Accumulated fill quantity
    /// </summary>
    [JsonProperty("z")]
    public decimal QuantityFilled { get; set; }

    /// <summary>
    /// Liquidation Trade Time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}
