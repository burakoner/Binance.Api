namespace Binance.Api.Futures;

/// <summary>
/// COIN-M index price kline update
/// </summary>
public record BinanceFuturesStreamIndexPriceKline : BinanceFuturesStreamEvent
{
    /// <summary>
    /// The pair, for example <c>BTCUSD</c>
    /// </summary>
    [JsonProperty("ps")]
    public string Pair { get; set; } = string.Empty;

    /// <summary>
    /// The index price kline data
    /// </summary>
    [JsonProperty("k")]
    public BinanceFuturesStreamIndexPriceKlineData Kline { get; set; } = default!;
}

/// <summary>
/// COIN-M index price kline data
/// </summary>
public record BinanceFuturesStreamIndexPriceKlineData
{
    /// <summary>
    /// The open time of this candlestick
    /// </summary>
    [JsonProperty("t"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// The transaction time as named by Binance
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// The documented symbol field. Current COIN-M payloads contain <c>0</c>; use <see cref="BinanceFuturesStreamIndexPriceKline.Pair"/> for pair identity.
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
    /// Volume as named by Binance. Binance does not document its asset or contract unit for this stream.
    /// </summary>
    [JsonProperty("v")]
    public decimal Volume { get; set; }

    /// <summary>
    /// The number of trades as named by Binance
    /// </summary>
    [JsonProperty("n")]
    public long TradeCount { get; set; }

    /// <summary>
    /// Whether this candlestick is closed
    /// </summary>
    [JsonProperty("x")]
    public bool Final { get; set; }

    /// <summary>
    /// Quote volume as named by Binance. Binance does not document its asset or contract unit for this stream.
    /// </summary>
    [JsonProperty("q")]
    public decimal QuoteVolume { get; set; }

    /// <summary>
    /// Taker-buy volume as named by Binance. Binance does not document its asset or contract unit for this stream.
    /// </summary>
    [JsonProperty("V")]
    public decimal TakerBuyVolume { get; set; }

    /// <summary>
    /// Last-trade volume as named by Binance. Binance does not document its asset or contract unit for this stream.
    /// </summary>
    [JsonProperty("Q")]
    public decimal LastTradeVolume { get; set; }

    /// <summary>
    /// Best bid quantity as named by Binance. Binance does not document its asset or contract unit for this stream.
    /// </summary>
    [JsonProperty("B")]
    public decimal BestBidQuantity { get; set; }
}
