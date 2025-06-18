namespace Binance.Api.PortfolioMargin;

/// <summary>
/// The status of an order
/// </summary>
public enum BinancePortfolioMarginOrderStatus : byte
{
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
}
