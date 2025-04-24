namespace Binance.Api.Shared;

/// <summary>
/// The interval for the period
/// </summary>
public enum BinancePeriodInterval : int
{
    /// <summary>
    /// 5m
    /// </summary>
    [Map("5m")]
    FiveMinutes = 60 * 5,

    /// <summary>
    /// 15m
    /// </summary>
    [Map("15m")]
    FifteenMinutes = 60 * 15,

    /// <summary>
    /// 30m
    /// </summary>
    [Map("30m")]
    ThirtyMinutes = 60 * 30,

    /// <summary>
    /// 1h
    /// </summary>
    [Map("1h")]
    OneHour = 60 * 60,

    /// <summary>
    /// 2h
    /// </summary>
    [Map("2h")]
    TwoHour = 60 * 60 * 2,

    /// <summary>
    /// 4h
    /// </summary>
    [Map("4h")]
    FourHour = 60 * 60 * 4,

    /// <summary>
    /// 6h
    /// </summary>
    [Map("6h")]
    SixHour = 60 * 60 * 6,

    /// <summary>
    /// 12h
    /// </summary>
    [Map("12h")]
    TwelveHour = 60 * 60 * 12,

    /// <summary>
    /// 1d
    /// </summary>
    [Map("1d")]
    OneDay = 60 * 60 * 24,
}
