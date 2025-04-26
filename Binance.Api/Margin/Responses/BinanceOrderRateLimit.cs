namespace Binance.Api.Margin;

/// <summary>
/// Rate limit info
/// </summary>
public record BinanceCurrentRateLimit: BinanceRateLimit
{
    /// <summary>
    /// The current used amount
    /// </summary>
        [JsonProperty("count")]
    public int Count { get; set; }
}
