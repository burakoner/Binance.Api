namespace Binance.Api.SimpleEarn;

/// <summary>
/// Simple Earn locked product reward record
/// </summary>
public record BinanceSimpleEarnLockedRewardRecord
{
    /// <summary>
    /// Asset
    /// </summary>
    [JsonProperty("asset")]
    public string Asset { get; set; } = string.Empty;

    /// <summary>
    /// Position id
    /// </summary>
    [JsonProperty("positionId")]
    public long PositionId { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Lock period
    /// </summary>
    [JsonProperty("lockPeriod")]
    public int LockPeriod { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Rewards type
    /// </summary>
    [JsonProperty("type")]
    public string? Type { get; set; }
}
