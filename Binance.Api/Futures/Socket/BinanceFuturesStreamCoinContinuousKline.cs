namespace Binance.Api.Futures;

/// <summary>
/// COIN-M continuous contract kline update
/// </summary>
public record BinanceFuturesStreamCoinContinuousKline : BinanceFuturesStreamEvent
{
    /// <summary>
    /// The pair, for example <c>BTCUSD</c>
    /// </summary>
    [JsonProperty("ps")]
    public string Pair { get; set; } = string.Empty;

    /// <summary>
    /// The continuous contract type
    /// </summary>
    [JsonProperty("ct"), JsonConverter(typeof(MapConverter))]
    public BinanceFuturesContractType ContractType { get; set; } = BinanceFuturesContractType.Unknown;

    /// <summary>
    /// The kline data
    /// </summary>
    [JsonProperty("k")]
    public BinanceFuturesStreamCoinContinuousKlineData Kline { get; set; } = default!;
}

/// <summary>
/// COIN-M continuous contract kline data
/// </summary>
public record BinanceFuturesStreamCoinContinuousKlineData
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
    /// The interval of this candlestick
    /// </summary>
    [JsonProperty("i"), JsonConverter(typeof(MapConverter))]
    public BinanceKlineInterval Interval { get; set; }

    /// <summary>
    /// The first update ID
    /// </summary>
    [JsonProperty("f")]
    public long FirstUpdateId { get; set; }

    /// <summary>
    /// The last update ID
    /// </summary>
    [JsonProperty("L")]
    public long LastUpdateId { get; set; }

    /// <summary>
    /// The price at which this candlestick opened
    /// </summary>
    [JsonProperty("o")]
    public decimal OpenPrice { get; set; }

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
    /// The current close price of this candlestick
    /// </summary>
    [JsonProperty("c")]
    public decimal ClosePrice { get; set; }

    /// <summary>
    /// The contract volume traded during this candlestick
    /// </summary>
    [JsonProperty("v")]
    public decimal ContractVolume { get; set; }

    /// <summary>
    /// The number of trades in this candlestick
    /// </summary>
    [JsonProperty("n")]
    public long TradeCount { get; set; }

    /// <summary>
    /// Whether this candlestick is closed
    /// </summary>
    [JsonProperty("x")]
    public bool Final { get; set; }

    /// <summary>
    /// The base asset volume traded during this candlestick
    /// </summary>
    [JsonProperty("q")]
    public decimal BaseAssetVolume { get; set; }

    /// <summary>
    /// The taker buy contract volume of this candlestick
    /// </summary>
    [JsonProperty("V")]
    public decimal TakerBuyContractVolume { get; set; }

    /// <summary>
    /// The taker buy base asset volume of this candlestick
    /// </summary>
    [JsonProperty("Q")]
    public decimal TakerBuyBaseAssetVolume { get; set; }

    /// <summary>
    /// Ignore
    /// </summary>
    [JsonProperty("B")]
    public decimal Ignore { get; set; }
}
