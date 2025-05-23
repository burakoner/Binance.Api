﻿namespace Binance.Api.Spot;

/// <summary>
/// The reason the order was rejected
/// </summary>
public enum BinanceSpotOrderRejectReason : byte
{
    /// <summary>
    /// Not rejected
    /// </summary>
    [Map("NONE")]
    None = 1,

    /// <summary>
    /// Unknown instrument
    /// </summary>
    [Map("UNKNOWN_INSTRUMENT")]
    UnknownInstrument,

    /// <summary>
    /// Closed market
    /// </summary>
    [Map("MARKET_CLOSED")]
    MarketClosed,

    /// <summary>
    /// Quantity out of bounds
    /// </summary>
    [Map("PRICE_QTY_EXCEED_HARD_LIMITS")]
    PriceQuantityExceedsHardLimits,

    /// <summary>
    /// Unknown order
    /// </summary>
    [Map("UNKNOWN_ORDER")]
    UnknownOrder,

    /// <summary>
    /// Duplicate
    /// </summary>
    [Map("DUPLICATE_ORDER")]
    DuplicateOrder,

    /// <summary>
    /// Unknown account
    /// </summary>
    [Map("UNKNOWN_ACCOUNT")]
    UnknownAccount,

    /// <summary>
    /// Not enough balance
    /// </summary>
    [Map("INSUFFICIENT_BALANCE")]
    InsufficientBalance,

    /// <summary>
    /// Account not active
    /// </summary>
    [Map("ACCOUNT_INACTIVE")]
    AccountInactive,

    /// <summary>
    /// Cannot settle
    /// </summary>
    [Map("ACCOUNT_CANNOT_SETTLE")]
    AccountCannotSettle,

    /// <summary>
    /// Stop price would trigger immediately
    /// </summary>
    [Map("STOP_PRICE_WOULD_TRIGGER_IMMEDIATELY")]
    StopPriceWouldTrigger
}
