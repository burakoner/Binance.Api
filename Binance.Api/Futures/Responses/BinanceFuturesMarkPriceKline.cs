namespace Binance.Api.Futures;

/// <summary>
/// Kline for mark or index price
/// </summary>
[JsonConverter(typeof(ArrayConverter))]
public record BinanceFuturesMarkPriceKline
{
    /// <summary>
    /// The time this candlestick opened
    /// </summary>
    [ArrayProperty(0), JsonConverter(typeof(DateTimeConverter))]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// The price at which this candlestick opened
    /// </summary>
    [ArrayProperty(1)]
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// The highest price in this candlestick
    /// </summary>
    [ArrayProperty(2)]
    public decimal HighPrice { get; set; }

    /// <summary>
    /// The lowest price in this candlestick
    /// </summary>
    [ArrayProperty(3)]
    public decimal LowPrice { get; set; }

    /// <summary>
    /// The price at which this candlestick closed
    /// </summary>
    [ArrayProperty(4)]
    public decimal ClosePrice { get; set; }

    /// <summary>
    /// The close time of this candlestick
    /// </summary>
    [ArrayProperty(6), JsonConverter(typeof(DateTimeConverter))]
    public DateTime CloseTime { get; set; }
}
