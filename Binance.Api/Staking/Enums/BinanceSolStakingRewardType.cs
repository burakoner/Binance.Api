namespace Binance.Api.Staking;

/// <summary>
/// Reward type
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceSolStakingRewardType : byte
{
    /// <summary>
    /// Claim
    /// </summary>
    [Map("CLAIM")]
    Claim = 1,

    /// <summary>
    /// Distribute
    /// </summary>
    [Map("DISTRIBUTE")]
    Distribute = 2,
}
