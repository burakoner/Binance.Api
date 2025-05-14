namespace Binance.Api.Convert;

/// <summary>
/// Expired Time
/// </summary>
public enum BinanceConvertExpiredTime : byte
{
    /// <summary>
    /// One Day
    /// </summary>
    [Map("1_D")]
    OneDay = 1,

    /// <summary>
    /// Three Days
    /// </summary>
    [Map("3_D")]
    ThreeDays = 3,

    /// <summary>
    /// Three Days
    /// </summary>
    [Map("7_D")]
    SevenDays = 7,

    /// <summary>
    /// Thirty Days
    /// </summary>
    [Map("30_D")]
    ThirtyDays = 30,
}
