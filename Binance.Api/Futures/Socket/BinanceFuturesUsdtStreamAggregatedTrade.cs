namespace Binance.Api.Futures;

/// <summary>
/// USDⓈ-M aggregate-trade update
/// </summary>
public record BinanceFuturesUsdtStreamAggregatedTrade : BinanceFuturesStreamEvent
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Aggregate trade ID
    /// </summary>
    [JsonProperty("a")]
    public long Id { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    [JsonProperty("p")]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity including all market trades
    /// </summary>
    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Normal market-trade quantity excluding trades involving RPI orders
    /// </summary>
    [JsonProperty("nq")]
    public decimal NormalQuantity { get; set; }

    /// <summary>
    /// First trade ID in the aggregation
    /// </summary>
    [JsonProperty("f")]
    public long FirstTradeId { get; set; }

    /// <summary>
    /// Last trade ID in the aggregation
    /// </summary>
    [JsonProperty("l")]
    public long LastTradeId { get; set; }

    /// <summary>
    /// Trade time
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TradeTime { get; set; }

    /// <summary>
    /// Whether the buyer was the market maker
    /// </summary>
    [JsonProperty("m")]
    public bool BuyerIsMaker { get; set; }

    /// <summary>
    /// Symbol type after UM/CM integration: 1 = UM, 2 = CM
    /// </summary>
    [JsonProperty("st")]
    public int SymbolType { get; set; }
}
