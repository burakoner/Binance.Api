namespace Binance.Api.Options;

/// <summary>
/// Candlestick information for symbol
/// </summary>
[JsonConverter(typeof(ArrayConverter))]
public record BinanceOptionsKline
{
    /// <summary>
    /// Opening time
    /// </summary>
    [ArrayProperty(0), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// Opening price
    /// </summary>
    [ArrayProperty(1)]
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// Highest price
    /// </summary>
    [ArrayProperty(2)]
    public decimal HighPrice { get; set; }

    /// <summary>
    /// Lowest price
    /// </summary>
    [ArrayProperty(3)]
    public decimal LowPrice { get; set; }

    /// <summary>
    /// Closing price (latest price if the current candle has not closed)
    /// </summary>
    [ArrayProperty(4)]
    public decimal ClosePrice { get; set; }

    /// <summary>
    /// Trading volume(contracts)
    /// </summary>
    [ArrayProperty(5)]
    public decimal Volume { get; set; }

    /// <summary>
    /// Closing time
    /// </summary>
    [ArrayProperty(6), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CloseTime { get; set; }

    /// <summary>
    /// Trading amount(in quote asset)
    /// </summary>
    [ArrayProperty(7)]
    public decimal QuoteVolume { get; set; }

    /// <summary>
    /// Number of completed trades
    /// </summary>
    [ArrayProperty(8)]
    public int TradeCount { get; set; }

    /// <summary>
    /// Taker trading volume(contracts)
    /// </summary>
    [ArrayProperty(9)]
    public decimal TakerBuyBaseVolume { get; set; }

    /// <summary>
    /// Taker trade amount(in quote asset)
    /// </summary>
    [ArrayProperty(10)]
    public decimal TakerBuyQuoteVolume { get; set; }

    /// <summary>
    /// Ignored value published at tuple index 11
    /// </summary>
    [ArrayProperty(11)]
    public string IgnoredValue { get; set; } = "";
}
