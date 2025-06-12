namespace Binance.Api.Options;

/// <summary>
/// Price statistics of the last 24 hours
/// </summary>
public class BinanceOptionsTicker
{
    /// <summary>
    /// The symbol the price is for
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The actual price change in the last 24 hours
    /// </summary>
    public decimal PriceChange { get; set; }

    /// <summary>
    /// The price change in percentage in the last 24 hours
    /// </summary>
    public decimal PriceChangePercent { get; set; }

    /// <summary>
    /// The most recent trade price
    /// </summary>
    public decimal LastPrice { get; set; }

    /// <summary>
    /// The most recent trade quantity
    /// </summary>
    [JsonProperty("lastQty")]
    public decimal LastQuantity { get; set; }

    /// <summary>
    /// The open price 24 hours ago
    /// </summary>
    public decimal Open { get; set; }

    /// <summary>
    /// The highest price in the last 24 hours
    /// </summary>
    public decimal High { get; set; }

    /// <summary>
    /// The lowest price in the last 24 hours
    /// </summary>
    public decimal Low { get; set; }

    /// <summary>
    /// The base volume traded in the last 24 hours
    /// </summary>
    public decimal Volume { get; set; }

    /// <summary>
    /// The quote asset volume traded in the last 24 hours
    /// </summary>
    [JsonProperty("amount")]
    public decimal QuoteVolume { get; set; }

    /// <summary>
    /// The best bid price in the order book
    /// </summary>
    [JsonProperty("bidPrice")]
    public decimal BestBidPrice { get; set; }

    /// <summary>
    /// The quantity of the best ask price in the order book
    /// </summary>
    [JsonProperty("AskQty")]
    public decimal BestAskQuantity { get; set; }

    /// <summary>
    /// Time at which this 24 hours opened
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// Time at which this 24 hours closed
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CloseTime { get; set; }

    /// <summary>
    /// The first trade ID in the last 24 hours
    /// </summary>
    public long FirstTradeId { get; set; }

    /// <summary>
    /// The amount of trades made in the last 24 hours
    /// </summary>
    public long TradeCount { get; set; }

    /// <summary>
    /// Strike Price
    /// </summary>
    public decimal StrikePrice { get; set; }

    /// <summary>
    /// Exercise Price
    /// </summary>
    public decimal ExercisePrice { get; set; }
}