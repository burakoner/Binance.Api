namespace Binance.Api.AutoInvest;

/// <summary>
/// Subscription cycle
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum AutoInvestSubscriptionCycle : int
{
    /// <summary>
    /// One hour
    /// </summary>
    [Map("H1")]
    OneHour = 60 * 60,

    /// <summary>
    /// Four hour
    /// </summary>
    [Map("H4")]
    FourHour = 4 * 60 * 60,

    /// <summary>
    /// Eight hour
    /// </summary>
    [Map("H8")]
    EightHour = 8 * 60 * 60,

    /// <summary>
    /// Twelve hour
    /// </summary>
    [Map("H12")]
    TwelveHour = 12 * 60 * 60,

    /// <summary>
    /// Daily
    /// </summary>
    [Map("DAILY")]
    Daily = 24 * 60 * 60,

    /// <summary>
    /// Weekly
    /// </summary>
    [Map("WEEKLY")]
    Weekly = 7 * 24 * 60 * 60,

    /// <summary>
    /// Bi-Weekly
    /// </summary>
    [Map("BI_WEEKLY")]
    BiWeekly = 14 * 24 * 60 * 60,

    /// <summary>
    /// Monthly
    /// </summary>
    [Map("MONTHLY")]
    Monthly = 30 * 24 * 60 * 60,
}
