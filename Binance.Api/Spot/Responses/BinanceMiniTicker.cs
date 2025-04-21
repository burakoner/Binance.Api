namespace Binance.Api.Spot;

/// <summary>
/// Price statistics of the last 24 hours
/// </summary>
public class BinanceMiniTicker
{
    /// <summary>
    /// The symbol the price is for
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The open price 24 hours ago
    /// </summary>
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// The highest price in the last 24 hours
    /// </summary>
    public decimal HighPrice { get; set; }

    /// <summary>
    /// The lowest price in the last 24 hours
    /// </summary>
    public decimal LowPrice { get; set; }

    /// <summary>
    /// The most recent trade price
    /// </summary>
    public decimal LastPrice { get; set; }

    /// <summary>
    /// The base volume traded in the last 24 hours
    /// </summary>
    public decimal Volume { get; set; }

    /// <summary>
    /// The quote asset volume traded in the last 24 hours
    /// </summary>
    public decimal QuoteVolume { get; set; }

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
    [JsonProperty("firstId")]
    public long FirstTradeId { get; set; }

    /// <summary>
    /// The last trade ID in the last 24 hours
    /// </summary>
    [JsonProperty("lastId")]
    public long LastTradeId { get; set; }

    /// <summary>
    /// The amount of trades made in the last 24 hours
    /// </summary>
    [JsonProperty("count")]
    public long TotalTrades { get; set; }
}
