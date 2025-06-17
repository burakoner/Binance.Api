namespace Binance.Api.Options;

/// <summary>
/// Wrapper for kline information for a symbol
/// </summary>
internal record BinanceOptionsStreamKlineWrapper : BinanceSocketStreamEvent
{
    /// <summary>
    /// The symbol the data is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The data
    /// </summary>
    [JsonProperty("k")]
    public BinanceOptionsStreamKline Kline { get; set; } = default!;
}

/// <summary>
/// The kline data
/// </summary>
public record BinanceOptionsStreamKline
{
    /// <summary>
    /// The open time of this candlestick
    /// </summary>
    [JsonProperty("t"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// The volume traded during this candlestick
    /// </summary>
    [JsonProperty("v")]
    public decimal Volume { get; set; }

    /// <summary>
    /// The close time of this candlestick
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CloseTime { get; set; }

    /// <summary>
    /// The volume traded during this candlestick in the asset form
    /// </summary>
    [JsonProperty("q")]
    public decimal QuoteVolume { get; set; }

    /// <summary>
    /// The symbol this candlestick is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The interval of this candlestick
    /// </summary>
    [JsonProperty("i")]
    public BinanceKlineInterval Interval { get; set; }

    /// <summary>
    /// The first trade id in this candlestick
    /// </summary>
    [JsonProperty("f")]
    public long FirstTrade { get; set; }

    /// <summary>
    /// The last trade id in this candlestick
    /// </summary>
    [JsonProperty("L")]
    public long LastTrade { get; set; }

    /// <summary>
    /// The open price of this candlestick
    /// </summary>
    [JsonProperty("o")]
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// The close price of this candlestick
    /// </summary>
    [JsonProperty("c")]
    public decimal ClosePrice { get; set; }

    /// <summary>
    /// The highest price of this candlestick
    /// </summary>
    [JsonProperty("h")]
    public decimal HighPrice { get; set; }

    /// <summary>
    /// The lowest price of this candlestick
    /// </summary>
    [JsonProperty("l")]
    public decimal LowPrice { get; set; }

    /// <summary>
    /// The amount of trades in this candlestick
    /// </summary>
    [JsonProperty("n")]
    public int TradeCount { get; set; }

    /// <summary>
    /// Taker buy base asset volume
    /// </summary>
    [JsonProperty("V")]
    public decimal TakerBuyBaseVolume { get; set; }

    /// <summary>
    /// Taker buy quote asset volume
    /// </summary>
    [JsonProperty("Q")]
    public decimal TakerBuyQuoteVolume { get; set; }

    /// <summary>
    /// Boolean indicating whether this candlestick is closed
    /// </summary>
    [JsonProperty("x")]
    public bool Final { get; set; }
}
