namespace Binance.Api.SimpleEarn;

/// <summary>
/// Simple Earn Reward type
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceSimpleEarnRewardType : byte
{
    /// <summary>
    /// All reward types
    /// </summary>
    [Map("ALL")]
    All = 0,

    /// <summary>
    /// Bonus tiered APR
    /// </summary>
    [Map("BONUS")]
    BonusTieredApr = 1,

    /// <summary>
    /// Realtime APR
    /// </summary>
    [Map("REALTIME")]
    RealtimeApr = 2,

    /// <summary>
    /// Historical rewards
    /// </summary>
    [Map("REWARDS")]
    HistoricalRewards = 3,
}
