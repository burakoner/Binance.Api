namespace Binance.Api.Futures;

/// <summary>
/// Liquidation order stream event
/// </summary>
public record BinanceFuturesStreamLiquidation : BinanceFuturesStreamEvent
{
    /// <summary>
    /// Liquidation order
    /// </summary>
    [JsonProperty("o")]
    public BinanceFuturesStreamLiquidationOrder Order { get; set; } = default!;

    /// <summary>
    /// Pair symbol. Present on merged UM events; CM events carry the pair inside <see cref="Order" />
    /// </summary>
    [JsonProperty("ps")]
    public string? Pair { get; set; }

    /// <summary>
    /// Symbol type for merged all-market events: 1 for USDⓈ-M, 2 for COIN-M
    /// </summary>
    [JsonProperty("st")]
    public int? SymbolType { get; set; }
}

/// <summary>
/// Liquidation order details
/// </summary>
public record BinanceFuturesStreamLiquidationOrder
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Pair symbol. Present on COIN-M events
    /// </summary>
    [JsonProperty("ps")]
    public string? Pair { get; set; }

    /// <summary>
    /// Liquidation Sided
    /// </summary>
    [JsonProperty("S"), JsonConverter(typeof(MapConverter))]
    public BinanceOrderSide Side { get; set; }
    
    /// <summary>
    /// Liquidation order type
    /// </summary>
    [JsonProperty("o"), JsonConverter(typeof(MapConverter))]
    public BinanceFuturesOrderType Type { get; set; }
    
    /// <summary>
    /// Liquidation Time in Force
    /// </summary>
    [JsonProperty("f"), JsonConverter(typeof(MapConverter))]
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
    [JsonProperty("X"), JsonConverter(typeof(MapConverter))]
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
