namespace Binance.Api.SimpleEarn;

/// <summary>
/// Simple earn subscription type
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceSimpleEarnSubscriptionType : byte
{
    /// <summary>
    /// Auto subscribe
    /// </summary>
    [Map("AUTO")]
    Auto = 0,

    /// <summary>
    /// Normal
    /// </summary>
    [Map("NORMAL")]
    Normal = 1,

    /// <summary>
    /// Locked to flexible
    /// </summary>
    [Map("CONVERT")]
    Convert = 2,

    /// <summary>
    /// Flexible loan
    /// </summary>
    [Map("LOAN")]
    Loan = 3,

    /// <summary>
    /// Auto invest
    /// </summary>
    [Map("AI")]
    AutoInvest = 4,

    /// <summary>
    /// Locked saving to flexible
    /// </summary>
    [Map("TRANSFER")]
    Transfer = 5
}
