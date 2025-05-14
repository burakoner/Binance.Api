namespace Binance.Api.Convert;

/// <summary>
/// Valid Time
/// </summary>
public enum BinanceConvertValidTime : byte
{
    /// <summary>
    /// 10 seconds
    /// </summary>
    [Map("10s")]
    TenSeconds = 10,

    /// <summary>
    /// 30 seconds
    /// </summary>
    [Map("30s")]
    ThirtySeconds = 30,

    /// <summary>
    /// 1 minute
    /// </summary>
    [Map("1m")]
    OneMinute = 60,
}
