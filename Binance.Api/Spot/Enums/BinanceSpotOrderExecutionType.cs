﻿namespace Binance.Api.Spot;

/// <summary>
/// The type of execution
/// </summary>
public enum BinanceSpotOrderExecutionType : byte
{
    /// <summary>
    /// New
    /// </summary>
    [Map("NEW")]
    New = 1,

    /// <summary>
    /// Canceled
    /// </summary>
    [Map("CANCELED")]
    Canceled,

    /// <summary>
    /// Replaced
    /// </summary>
    [Map("REPLACED")]
    Replaced,

    /// <summary>
    /// Rejected
    /// </summary>
    [Map("REJECTED")]
    Rejected,

    /// <summary>
    /// Trade
    /// </summary>
    [Map("TRADE")]
    Trade,

    /// <summary>
    /// Expired
    /// </summary>
    [Map("EXPIRED")]
    Expired,

    /// <summary>
    /// Amendment
    /// </summary>
    [Map("AMENDMENT")]
    Amendment,

    /// <summary>
    /// Self trade prevented
    /// </summary>
    [Map("TRADE_PREVENTION")]
    TradePrevention
}
