namespace Binance.Api.Futures;

/// <summary>
/// Wrapper for kline information for a symbol
/// </summary>
public record BinanceStreamKlineData: BinanceFuturesStreamEvent
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
    public BinanceStreamKline Data { get; set; } = default!;
}

/// <summary>
/// The kline data
/// </summary>
public record BinanceStreamKline
{
    /// <summary>
    /// The open time of this candlestick
    /// </summary>
    [JsonProperty("t"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <inheritdoc />
    [JsonProperty("v")]
    public decimal Volume { get; set; }

    /// <summary>
    /// The close time of this candlestick
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CloseTime { get; set; }

    /// <inheritdoc />
    [JsonProperty("q")]
    public decimal QuoteVolume { get; set; }

    /// <summary>
    /// The symbol this candlestick is for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;
    
    /// <summary>
    /// The interval of this candlestick
    /// </summary>
    [JsonProperty("i"), JsonConverter(typeof(MapConverter))]
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
    /// The taker buy base asset volume of this candlestick
    /// </summary>
    [JsonProperty("V")]
    public decimal TakerBuyBaseVolume { get; set; }

    /// <summary>
    /// The taker buy quote asset volume of this candlestick
    /// </summary>
    [JsonProperty("Q")]
    public decimal TakerBuyQuoteVolume { get; set; }

    /// <summary>
    /// Boolean indicating whether this candlestick is closed
    /// </summary>
    [JsonProperty("x")]
    public bool Final { get; set; }
}
