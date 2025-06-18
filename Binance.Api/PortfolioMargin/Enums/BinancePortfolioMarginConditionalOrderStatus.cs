namespace Binance.Api.PortfolioMargin;

/// <summary>
/// The status of an order
/// </summary>
public enum BinancePortfolioMarginConditionalOrderStatus : byte
{
    /// <summary>
    /// Order is new
    /// </summary>
    [Map("NEW")]
    New = 1,

    /// <summary>
    /// The order has been canceled
    /// </summary>
    [Map("CANCELED")]
    Canceled = 2,

    /// <summary>
    /// conditional order is triggered
    /// </summary>
    [Map("TRIGGERED")]
    Triggered = 3,

    /// <summary>
    /// triggered order is filled
    /// </summary>
    [Map("FINISHED")]
    Finished = 4,

    /// <summary>
    /// The order has expired
    /// </summary>
    [Map("EXPIRED")]
    Expired = 5,
}
