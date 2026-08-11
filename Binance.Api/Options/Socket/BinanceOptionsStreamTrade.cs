namespace Binance.Api.Options;

/// <summary>
/// Binance Options Trade
/// </summary>
public record BinanceOptionsStreamTrade : BinanceSocketStreamEvent
{
    /// <summary>
    /// Symbol of the trade
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The id of the trade
    /// </summary>
    [JsonProperty("t")]
    public long TradeId { get; set; }

    /// <summary>
    /// The price of the trade
    /// </summary>
    [JsonProperty("p")]
    public decimal Price { get; set; }

    /// <summary>
    /// Trade quantity in contracts
    /// </summary>
    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// The timestamp of the trade
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    /// <summary>
    /// Completed trade direction
    /// </summary>
    [JsonProperty("S")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// trade type enum, "MARKET" for Orderbook trading, "BLOCK" for Block trade
    /// </summary>
    [JsonProperty("X")]
    [JsonConverter(typeof(MapConverter))]
    public BinanceOptionsTradeType Type { get; set; }

    /// <summary>
    /// Whether the buyer is the market maker
    /// </summary>
    [JsonProperty("m")]
    public bool BuyerIsMarketMaker { get; set; }
}
