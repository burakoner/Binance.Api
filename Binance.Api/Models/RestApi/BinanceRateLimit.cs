namespace Binance.Api.Models.RestApi;

/// <summary>
/// Rate limit info
/// </summary>
public record BinanceRateLimit
{
    /// <summary>
    /// The interval the rate limit uses to count
    /// </summary>
    public RateLimitInterval Interval { get; set; }

    /// <summary>
    /// The type the rate limit applies to
    /// </summary>
    [JsonProperty("rateLimitType")]
    public RateLimitType Type { get; set; }

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
