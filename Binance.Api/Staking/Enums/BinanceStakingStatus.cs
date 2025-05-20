namespace Binance.Api.Staking;

/// <summary>
/// Reward type
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceStakingStatus : byte
{
    /// <summary>
    /// Success
    /// </summary>
    [Map("SUCCESS")]
    Success = 1,

    /// <summary>
    /// Pending
    /// </summary>
    [Map("PENDING")]
    Pending = 2,

    /// <summary>
    /// Failed
    /// </summary>
    [Map("FAILED")]
    Failed = 2,
}
