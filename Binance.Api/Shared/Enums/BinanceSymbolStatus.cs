namespace Binance.Api.Shared;

/// <summary>
/// Status of a symbol
/// </summary>
public enum BinanceSymbolStatus : byte
{
    /// <summary>
    /// Pending trading
    /// </summary>
    [Map("PENDING_TRADING")]
    PendingTrading = 1,

    /// <summary>
    /// Trading
    /// </summary>
    [Map("TRADING")]
    Trading = 2,

    /// <summary>
    /// Pre delivering
    /// </summary>
    [Map("PRE_DELIVERING")]
    PreDelivering = 3,

    /// <summary>
    /// Delivering
    /// </summary>
    [Map("DELIVERING")]
    Delivering = 4,

    /// <summary>
    /// Delivered
    /// </summary>
    [Map("DELIVERED")]
    Delivered = 5,

    /// <summary>
    /// Pre settle
    /// </summary>
    [Map("PRE_SETTLE")]
    PreSettle = 6,

    /// <summary>
    /// Settling
    /// </summary>
    [Map("SETTLING")]
    Settling = 7,

    /// <summary>
    /// Closed
    /// </summary>
    [Map("CLOSE")]
    Close = 8,

    /// <summary>
    /// Trading halted
    /// </summary>
    [Map("TRADING_HALT")]
    TradingHalt = 9,

    /// <summary>
    /// Only order cancellations are allowed
    /// </summary>
    [Map("TRADING_CANCEL_ONLY")]
    TradingCancelOnly = 10
}
