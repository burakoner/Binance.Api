namespace Binance.Api.Options;

/// <summary>
/// Candlestick information for symbol
/// </summary>
public record BinanceOptionsKline
{
    /// <summary>
    /// Opening price
    /// </summary>
    [JsonProperty("open")]
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// Highest price
    /// </summary>
    [JsonProperty("high")]
    public decimal HighPrice { get; set; }

    /// <summary>
    /// Lowest price
    /// </summary>
    [JsonProperty("low")]
    public decimal LowPrice { get; set; }

    /// <summary>
    /// Closing price (latest price if the current candle has not closed)
    /// </summary>
    [JsonProperty("close")]
    public decimal ClosePrice { get; set; }

    /// <summary>
    /// Trading volume(contracts)
    /// </summary>
    [JsonProperty("volume")]
    public decimal Volume { get; set; }

    /// <summary>
    /// Trading amount(in quote asset)
    /// </summary>
    [JsonProperty("amount")]
    public decimal QuoteVolume { get; set; }

    /// <summary>
    /// Candle type
    /// </summary>
    [JsonProperty("interval")]
    public BinanceKlineInterval Interval { get; set; }

    /// <summary>
    /// Number of completed trades
    /// </summary>
    [JsonProperty("tradeCount")]
    public int TradeCount { get; set; }

    /// <summary>
    /// Taker trading volume(contracts)
    /// </summary>
    [JsonProperty("takerVolume")]
    public decimal TakerBuyBaseVolume { get; set; }

    /// <summary>
    /// Taker trade amount(in quote asset)
    /// </summary>
    [JsonProperty("takerAmount")]
    public decimal TakerBuyQuoteVolume { get; set; }

    /// <summary>
    /// Opening time
    /// </summary>
    [JsonProperty("openTime")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// Closing time
    /// </summary>
    [JsonProperty("closeTime")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CloseTime { get; set; }
}
