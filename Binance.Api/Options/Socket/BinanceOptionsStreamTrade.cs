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
    /// The base quantity of the trade
    /// </summary>
    [JsonProperty("q")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Buy Order Id
    /// </summary>
    [JsonProperty("b")]
    public long BuyOrderId { get; set; }

    /// <summary>
    /// Sell Order Id
    /// </summary>
    [JsonProperty("a")]
    public long SellOrderId { get; set; }

    /// <summary>
    /// The timestamp of the trade
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    /// <summary>
    /// Completed trade direction（-1 Sell，1 Buy）
    /// </summary>
    [JsonProperty("S")]
    public BinanceOptionsTradeSide Side { get; set; }

    /// <summary>
    /// trade type enum, "MARKET" for Orderbook trading, "BLOCK" for Block trade
    /// </summary>
    [JsonProperty("X")]
    public BinanceOptionsTradeType Type { get; set; }

}