namespace Binance.Api.Futures;

/// <summary>
/// Wrapper for kline information for a symbol
/// </summary>
internal record BinanceFuturesStreamIndexKlineWrapper : BinanceFuturesStreamEvent
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
    public BinanceFuturesStreamIndexKline Kline { get; set; } = default!;
}

/// <summary>
/// Index kline
/// </summary>
public record BinanceFuturesStreamIndexKline
{
    /// <summary>
    /// Symbol
    /// </summary>
    [JsonIgnore]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Open time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("t")]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// Close time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("T")]
    public DateTime CloseTime { get; set; }

    /// <summary>
    /// Kline interval
    /// </summary>
    [JsonProperty("i")]
    public BinanceKlineInterval Interval { get; set; }

    /// <summary>
    /// Open price of the kline
    /// </summary>
    [JsonProperty("o")]
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// Close price of the kline
    /// </summary>
    [JsonProperty("c")]
    public decimal ClosePrice { get; set; }

    /// <summary>
    /// High price of the kline
    /// </summary>
    [JsonProperty("h")]
    public decimal HighPrice { get; set; }

    /// <summary>
    /// Low price of the kline
    /// </summary>
    [JsonProperty("l")]
    public decimal LowPrice { get; set; }

    /// <summary>
    /// Number of basic data
    /// </summary>
    [JsonProperty("n")]
    public int NumberOfBasicData { get; set; }

    /// <summary>
    /// Is the kline closed
    /// </summary>
    [JsonProperty("x")]
    public bool Closed { get; set; }
}
