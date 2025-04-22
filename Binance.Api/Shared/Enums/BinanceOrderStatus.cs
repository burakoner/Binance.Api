namespace Binance.Api.Shared;

/// <summary>
/// The status of an order
/// </summary>
public enum BinanceOrderStatus : byte
{
    /// <summary>
    /// Order is not yet active
    /// </summary>
    [Map("PENDING_NEW")]
    PendingNew = 0,

    /// <summary>
    /// Order is new
    /// </summary>
    [Map("NEW")]
    New = 1,

    /// <summary>
    /// Order is partly filled, still has quantity left to fill
    /// </summary>
    [Map("PARTIALLY_FILLED")]
    PartiallyFilled = 2,

    /// <summary>
    /// The order has been filled and completed
    /// </summary>
    [Map("FILLED")]
    Filled = 3,

    /// <summary>
    /// The order is in the process of being canceled  (currently unused)
    /// </summary>
    [Map("PENDING_CANCEL")]
    PendingCancel = 4,

    /// <summary>
    /// The order has been canceled
    /// </summary>
    [Map("CANCELED")]
    Canceled = 5,

    /// <summary>
    /// The order has been rejected
    /// </summary>
    [Map("REJECTED")]
    Rejected = 6,

    /// <summary>
    /// The order has expired
    /// </summary>
    [Map("EXPIRED")]
    Expired = 7,

    /// <summary>
    /// Expired because of trigger SelfTradePrevention
    /// </summary>
    [Map("EXPIRED_IN_MATCH")]
    ExpiredInMatch = 8,

    /// <summary>
    /// Counterparty Liquidation
    /// </summary>
    [Map("ADL")]
    Adl = 9,

    /// <summary>
    /// Counterparty Liquidation
    /// </summary>
    [Map("NEW_ADL")]
    NewAdl = 10,

    /// <summary>
    /// Liquidation with Insurance Fund
    /// </summary>
    [Map("INSURANCE")]
    Insurance = 11,

    /// <summary>
    /// Liquidation with Insurance Fund
    /// </summary>
    [Map("NEW_INSURANCE")]
    NewInsurance = 12,
}
