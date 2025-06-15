namespace Binance.Api.Shared;

/// <summary>
/// Rate limit info
/// </summary>
public record BinanceRateLimit
{
    /// <summary>
    /// The interval the rate limit uses to count
    /// </summary>
    public BinanceRateLimitInterval Interval { get; set; }

    /// <summary>
    /// The type the rate limit applies to
    /// </summary>
    [JsonProperty("rateLimitType")]
    public BinanceRateLimitType Type { get; set; }

    /// <summary>
    /// The amount of calls the limit is
    /// </summary>
    [JsonProperty("intervalNum")]
    public int IntervalNumber { get; set; }

    /// <summary>
    /// The amount of calls the limit is
    /// </summary>
    public int Limit { get; set; }
}

/// <summary>
/// Rate limit info
/// </summary>
public record BinanceCurrentRateLimit : BinanceRateLimit
{
    /// <summary>
    /// The current used amount
    /// </summary>
    [JsonProperty("count")]
    public int Count { get; set; }
}
