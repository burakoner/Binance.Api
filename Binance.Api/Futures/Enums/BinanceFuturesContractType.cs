namespace Binance.Api.Futures;

/// <summary>
/// Type of contract
/// </summary>
public enum BinanceFuturesContractType : byte
{
    /// <summary>
    /// Unknown
    /// </summary>
    [Map("")]
    Unknown = 0,

    /// <summary>
    /// Perpetual
    /// </summary>
    [Map("PERPETUAL")]
    Perpetual,

    /// <summary>
    /// Current month
    /// </summary>
    [Map("CURRENT_MONTH")]
    CurrentMonth,

    /// <summary>
    /// Next month
    /// </summary>
    [Map("NEXT_MONTH")]
    NextMonth,

    /// <summary>
    /// Current quarter
    /// </summary>
    [Map("CURRENT_QUARTER")]
    CurrentQuarter,

    /// <summary>
    /// Next quarter
    /// </summary>
    [Map("NEXT_QUARTER")]
    NextQuarter,

    /// <summary>
    /// Perpetual delivering
    /// </summary>
    [Map("DELIVERING")]
    Delivering,

    /// <summary>
    /// Perpetual delivering
    /// </summary>
    [Map("PERPETUAL_DELIVERING")]
    PerpetualDelivering,

    /// <summary>
    /// Current quarter delivering
    /// </summary>
    [Map("CURRENT_QUARTER_DELIVERING")]
    CurrentQuarterDelivering,

    /// <summary>
    /// Next quarter delivering
    /// </summary>
    [Map("NEXT_QUARTER DELIVERING")]
    NextQuarterDelivering,
}
