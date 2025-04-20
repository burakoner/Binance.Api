namespace Binance.Api.Spot.Enums;

/// <summary>
/// Status of a symbol
/// </summary>
public enum BinanceSymbolStatus : byte
{
    /// <summary>
    /// Not trading yet
    /// </summary>
    [Map("PRE_TRADING")]
    PreTrading = 1,

    /// <summary>
    /// Pending trading
    /// </summary>
    [Map("PENDING_TRADING")]
    PendingTrading = 2,

    /// <summary>
    /// Trading
    /// </summary>
    [Map("TRADING")]
    Trading = 3,

    /// <summary>
    /// No longer trading
    /// </summary>
    [Map("POST_TRADING")]
    PostTrading = 4,

    /// <summary>
    /// Not trading
    /// </summary>
    [Map("END_OF_DAY")]
    EndOfDay = 5,

    /// <summary>
    /// Halted
    /// </summary>
    [Map("HALT")]
    Halt = 6,

    /// <summary>
    /// Auction Match
    /// </summary>
    [Map("AUCTION_MATCH")]
    AuctionMatch = 7,

    /// <summary>
    /// Break
    /// </summary>
    [Map("BREAK")]
    Break = 8,

    /// <summary>
    /// Closed
    /// </summary>
    [Map("CLOSE")]
    Close = 9,

    /// <summary>
    /// Pre delivering
    /// </summary>
    [Map("PRE_DELIVERING")]
    PreDelivering = 10,

    /// <summary>
    /// Delivering
    /// </summary>
    [Map("DELIVERING")]
    Delivering = 11,

    /// <summary>
    /// Pre settle
    /// </summary>
    [Map("PRE_SETTLE")]
    PreSettle = 12,

    /// <summary>
    /// Settings
    /// </summary>
    [Map("SETTLING")]
    Settling = 13
}
