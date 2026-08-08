namespace Binance.Api.Margin;

/// <summary>
/// Current Margin order-count usage for one rate-limit interval.
/// </summary>
public record BinanceMarginCurrentOrderCountUsage
{
    /// <summary>
    /// Rate-limit type.
    /// </summary>
    [JsonProperty("rateLimitType")]
    [JsonConverter(typeof(MapConverter))]
    public BinanceRateLimitType Type { get; set; }

    /// <summary>
    /// Rate-limit interval unit.
    /// </summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceRateLimitInterval Interval { get; set; }

    /// <summary>
    /// Number of interval units in the window.
    /// </summary>
    [JsonProperty("intervalNum")]
    public long IntervalNumber { get; set; }

    /// <summary>
    /// Maximum order count for the window.
    /// </summary>
    public long Limit { get; set; }

    /// <summary>
    /// Current order count in the window.
    /// </summary>
    public long Count { get; set; }
}
