namespace Binance.Api.Futures;

/// <summary>
/// Price statistics of the last 24 hours
/// </summary>
public record BinanceFuturesPrice
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

/// <summary>
/// Futures-Coin price
/// </summary>
public record BinanceFuturesCoinPrice : BinanceFuturesPrice
{
    /// <summary>
    /// Name of the pair
    /// </summary>
    [JsonProperty("ps")]
    public string Pair { get; set; } = "";
}
