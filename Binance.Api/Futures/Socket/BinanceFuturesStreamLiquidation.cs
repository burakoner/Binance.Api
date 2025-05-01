namespace Binance.Api.Futures;

/// <summary>
/// A event received by a Binance websocket
/// </summary>
public record BinanceFuturesStreamLiquidationData : BinanceFuturesStreamEvent
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
public record BinanceFuturesStreamLiquidation
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Liquidation Sided
    /// </summary>
    [JsonProperty("S")]
    public BinanceOrderSide Side { get; set; }
    
    /// <summary>
    /// Liquidation order type
    /// </summary>
    [JsonProperty("o")]
    public BinanceFuturesOrderType Type { get; set; }
    
    /// <summary>
    /// Liquidation Time in Force
    /// </summary>
    [JsonProperty("f")]
    public BinanceTimeInForce TimeInForce { get; set; }
    
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
    [JsonProperty("X")]
    public BinanceOrderStatus Status { get; set; }
    
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
