namespace Binance.Api.Shared;

/// <summary>
/// Rate limit on what unit
/// </summary>
public enum BinanceRateLimitInterval : byte
{
    /// <summary>
    /// Seconds
    /// </summary>
    [Map("SECOND")]
    Second = 1,

    /// <summary>
    /// Minutes
    /// </summary>
    [Map("MINUTE")]
    Minute = 2,

    /// <summary>
    /// Days
    /// </summary>
    [Map("DAY")]
    Day = 3
}
