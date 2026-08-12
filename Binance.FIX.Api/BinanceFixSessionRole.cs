namespace Binance.FIX.Api;

/// <summary>
/// A Binance Spot FIX session's isolated role.
/// </summary>
public enum BinanceFixSessionRole
{
    /// <summary>
    /// Order placement, cancellation, limit queries, and account execution reports.
    /// </summary>
    OrderEntry = 0,

    /// <summary>
    /// Read-only account execution-report and list-status delivery.
    /// </summary>
    DropCopy = 1,

    /// <summary>
    /// Public instrument and market-data queries and subscriptions.
    /// </summary>
    MarketData = 2
}
