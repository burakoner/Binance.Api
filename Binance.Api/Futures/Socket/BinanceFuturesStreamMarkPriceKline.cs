namespace Binance.Api.Futures;

/// <summary>
/// COIN-M mark price kline update
/// </summary>
public record BinanceFuturesStreamMarkPriceKline : BinanceFuturesStreamEvent
{
    /// <summary>
    /// The pair, for example <c>BTCUSD</c>
    /// </summary>
    [JsonProperty("ps")]
    public string Pair { get; set; } = string.Empty;

    /// <summary>
    /// The mark price kline data
    /// </summary>
    [JsonProperty("k")]
    public BinanceFuturesStreamMarkPriceKlineData Kline { get; set; } = default!;
}

/// <summary>
/// COIN-M mark price kline data
/// </summary>
public record BinanceFuturesStreamMarkPriceKlineData
{
    /// <summary>
    /// The open time of this candlestick
    /// </summary>
    [JsonProperty("t"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// The close time of this candlestick
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CloseTime { get; set; }

    /// <summary>
    /// The symbol, for example <c>BTCUSD_PERP</c>
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// The interval of this candlestick
    /// </summary>
    [JsonProperty("i"), JsonConverter(typeof(MapConverter))]
    public BinanceKlineInterval Interval { get; set; }

    /// <summary>
    /// The <c>f</c> field, which Binance documents as ignored
    /// </summary>
    [JsonProperty("f")]
    public long IgnoredValueF { get; set; }

    /// <summary>
    /// The <c>L</c> field, which Binance documents as ignored
    /// </summary>
    [JsonProperty("L")]
    public long IgnoredValueL { get; set; }

    /// <summary>
    /// The price at which this candlestick opened
    /// </summary>
    [JsonProperty("o")]
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// The current close price of this candlestick
    /// </summary>
    [JsonProperty("c")]
    public decimal ClosePrice { get; set; }

    /// <summary>
    /// The current highest price in this candlestick
    /// </summary>
    [JsonProperty("h")]
    public decimal HighPrice { get; set; }

    /// <summary>
    /// The current lowest price in this candlestick
    /// </summary>
    [JsonProperty("l")]
    public decimal LowPrice { get; set; }

    /// <summary>
    /// The <c>v</c> field, which Binance documents as ignored
    /// </summary>
    [JsonProperty("v")]
    public decimal IgnoredValueV { get; set; }

    /// <summary>
    /// The number of basic data points in this candlestick
    /// </summary>
    [JsonProperty("n")]
    public long BasicDataCount { get; set; }

    /// <summary>
    /// Whether this candlestick is closed
    /// </summary>
    [JsonProperty("x")]
    public bool Final { get; set; }

    /// <summary>
    /// The <c>q</c> field, which Binance documents as ignored
    /// </summary>
    [JsonProperty("q")]
    public decimal IgnoredValueQ { get; set; }

    /// <summary>
    /// The <c>V</c> field, which Binance documents as ignored
    /// </summary>
    [JsonProperty("V")]
    public decimal IgnoredValueUpperV { get; set; }

    /// <summary>
    /// The <c>Q</c> field, which Binance documents as ignored
    /// </summary>
    [JsonProperty("Q")]
    public decimal IgnoredValueUpperQ { get; set; }

    /// <summary>
    /// The <c>B</c> field, which Binance documents as ignored
    /// </summary>
    [JsonProperty("B")]
    public decimal IgnoredValueB { get; set; }
}
