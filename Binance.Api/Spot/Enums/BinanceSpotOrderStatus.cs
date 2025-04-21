namespace Binance.Api.Spot;

/// <summary>
/// The status of an order
/// </summary>
public enum BinanceSpotOrderStatus : byte
{
    /// <summary>
    /// Order is new
    /// </summary>
    [Map("NEW")]
    New = 1,

    /// <summary>
    /// Order is not yet active
    /// </summary>
    [Map("PENDING_NEW")]
    PendingNew = 2,

    /// <summary>
    /// Order is partly filled, still has quantity left to fill
    /// </summary>
    [Map("PARTIALLY_FILLED")]
    PartiallyFilled = 3,

    /// <summary>
    /// The order has been filled and completed
    /// </summary>
    [Map("FILLED")]
    Filled = 4,

    /// <summary>
    /// The order has been canceled
    /// </summary>
    [Map("CANCELED")]
    Canceled = 5,

    /// <summary>
    /// The order is in the process of being canceled  (currently unused)
    /// </summary>
    [Map("PENDING_CANCEL")]
    PendingCancel = 6,

    /// <summary>
    /// The order has been rejected
    /// </summary>
    [Map("REJECTED")]
    Rejected = 7,

    /// <summary>
    /// The order has expired
    /// </summary>
    [Map("EXPIRED")]
    Expired = 8,

    /// <summary>
    /// Expired because of trigger SelfTradePrevention
    /// </summary>
    [Map("EXPIRED_IN_MATCH")]
    ExpiredInMatch = 9
}
