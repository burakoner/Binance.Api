namespace Binance.Api.Futures;

/// <summary>
/// Price statistics of the last 24 hours
/// </summary>
public class BinanceFuturesPrice
{
    /// <summary>
    /// The symbol the price is for
    /// </summary>
    public string Symbol { get; set; } = "";

    /// <summary>
    /// The most recent trade price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Transaction time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }
}