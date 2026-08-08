namespace Binance.Api.Spot;

/// <summary>
/// Average-price stream update.
/// </summary>
public record BinanceSpotStreamAveragePrice : BinanceSocketStreamEvent
{
    /// <summary>
    /// Symbol.
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Average-price interval, such as <c>5m</c>.
    /// </summary>
    [JsonProperty("i")]
    public string Interval { get; set; } = string.Empty;

    /// <summary>
    /// Average price.
    /// </summary>
    [JsonProperty("w")]
    public decimal AveragePrice { get; set; }

    /// <summary>
    /// Last trade time.
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime LastTradeTime { get; set; }
}
