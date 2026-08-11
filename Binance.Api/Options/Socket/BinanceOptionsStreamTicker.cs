namespace Binance.Api.Options;

/// <summary>
/// Price statistics of the last 24 hours
/// </summary>
public record BinanceOptionsStreamTicker : BinanceSocketStreamEvent
{
    /// <summary>
    /// The symbol the price is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The open price 24 hours ago
    /// </summary>
    [JsonProperty("o")]
    public decimal Open { get; set; }

    /// <summary>
    /// The highest price in the last 24 hours
    /// </summary>
    [JsonProperty("h")]
    public decimal High { get; set; }

    /// <summary>
    /// The lowest price in the last 24 hours
    /// </summary>
    [JsonProperty("l")]
    public decimal Low { get; set; }

    /// <summary>
    /// The most recent trade price
    /// </summary>
    [JsonProperty("c")]
    public decimal LastPrice { get; set; }

    /// <summary>
    /// Weighted average price
    /// </summary>
    [JsonProperty("w")]
    public decimal WeightedAveragePrice { get; set; }

    /// <summary>
    /// Trading volume in contracts during the last 24 hours
    /// </summary>
    [JsonProperty("v")]
    public decimal ContractVolume { get; set; }

    /// <summary>
    /// The quote asset volume traded in the last 24 hours
    /// </summary>
    [JsonProperty("q")]
    public decimal QuoteAmount { get; set; }

    /// <summary>
    /// The price change in percentage in the last 24 hours
    /// </summary>
    [JsonProperty("P")]
    public decimal PriceChangePercent { get; set; }

    /// <summary>
    /// The actual price change in the last 24 hours
    /// </summary>
    [JsonProperty("p")]
    public decimal PriceChange { get; set; }

    /// <summary>
    /// The most recent trade quantity
    /// </summary>
    [JsonProperty("Q")]
    public decimal LastQuantity { get; set; }

    /// <summary>
    /// The first trade ID in the last 24 hours
    /// </summary>
    [JsonProperty("F")]
    public long FirstTradeId { get; set; }

    /// <summary>
    /// The last trade ID in the last 24 hours
    /// </summary>
    [JsonProperty("L")]
    public long LastTradeId { get; set; }

    /// <summary>
    /// number of trades
    /// </summary>
    [JsonProperty("n")]
    public long TradeCount { get; set; }

    /// <summary>
    /// Statistics open time
    /// </summary>
    [JsonProperty("O")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime StatisticsOpenTime { get; set; }

    /// <summary>
    /// Statistics close time
    /// </summary>
    [JsonProperty("C")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime StatisticsCloseTime { get; set; }
}
