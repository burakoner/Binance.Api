namespace Binance.Api.Futures;

/// <summary>
/// Wrapper for kline information for a symbol
/// </summary>
internal record BinanceFuturesStreamCoinKlineWrapper : BinanceFuturesStreamEvent
{
    /// <summary>
    /// The symbol the data is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The data
    /// </summary>
    [JsonProperty("k")]
    public BinanceFuturesStreamCoinKline Kline { get; set; } = default!;
}

/// <summary>
/// The kline data
/// </summary>
public record BinanceFuturesStreamCoinKline
{
    /// <summary>
    /// The open time of this candlestick
    /// </summary>
    [JsonProperty("t"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// The price at which this candlestick opened
    /// </summary>
    [JsonProperty("o")]
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// The highest price in this candlestick
    /// </summary>
    [JsonProperty("h")]
    public decimal HighPrice { get; set; }

    /// <summary>
    /// The lowest price in this candlestick
    /// </summary>
    [JsonProperty("l")]
    public decimal LowPrice { get; set; }

    /// <summary>
    /// The price at which this candlestick closed
    /// </summary>
    [JsonProperty("c")]
    public decimal ClosePrice { get; set; }

    /// <summary>
    /// The volume traded during this candlestick
    /// </summary>
    [JsonProperty("q")]
    public decimal Volume { get; set; }

    /// <summary>
    /// The close time of this candlestick
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CloseTime { get; set; }

    /// <summary>
    /// The volume traded during this candlestick in the asset form
    /// </summary>
    [JsonProperty("v")]
    public decimal QuoteVolume { get; set; }

    /// <summary>
    /// The amount of trades in this candlestick
    /// </summary>
    [JsonProperty("n")]
    public int TradeCount { get; set; }

    /// <summary>
    /// Taker buy base asset volume
    /// </summary>
    [JsonProperty("Q")]
    public decimal TakerBuyBaseVolume { get; set; }

    /// <summary>
    /// Taker buy quote asset volume
    /// </summary>
    [JsonProperty("V")]
    public decimal TakerBuyQuoteVolume { get; set; }

    /// <summary>
    /// The symbol this candlestick is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

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
    /// Boolean indicating whether this candlestick is closed
    /// </summary>
    [JsonProperty("x")]
    public bool Final { get; set; }
}
