namespace Binance.Api.Shared;

/// <summary>
/// Valid Time
/// </summary>
public enum BinanceValidTime : byte
{
    /// <summary>
    /// 10 seconds
    /// </summary>
    [Map("0")]
    TenSeconds = 0,

    /// <summary>
    /// 30 seconds
    /// </summary>
    [Map("1")]
    ThirtySeconds = 1,

    /// <summary>
    /// 1 minute
    /// </summary>
    [Map("2")]
    OneMinute = 2,

    /// <summary>
    /// 2 minutes
    /// </summary>
    [Map("3")]
    TwoMinutes = 3,
}
