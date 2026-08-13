namespace Binance.FIX.Api;

/// <summary>
/// Target family of a Binance Spot FIX OrderCancelRequest.
/// </summary>
public enum BinanceFixOrderCancelTarget
{
    /// <summary>One order, which can also cancel its containing order list.</summary>
    Order = 1,

    /// <summary>An order list.</summary>
    OrderList = 2
}

/// <summary>
/// Optional state restriction applied to a cancellation.
/// </summary>
public enum BinanceFixCancelRestriction
{
    /// <summary>Cancel only when the target order is new.</summary>
    OnlyNew = 1,

    /// <summary>Cancel only when the target order is partially filled.</summary>
    OnlyPartiallyFilled = 2
}

/// <summary>
/// Request type identified by an OrderCancelReject.
/// </summary>
public enum BinanceFixCancelRejectResponseTo
{
    /// <summary>The rejected request was an OrderCancelRequest.</summary>
    OrderCancelRequest = 1
}

/// <summary>
/// Conservative resolution of an ambiguous OrderCancelRequest transport attempt.
/// </summary>
public enum BinanceFixCancelReconciliationStatus
{
    /// <summary>No exact correlated response resolved the ambiguous attempt.</summary>
    Unresolved = 0,

    /// <summary>
    /// A correlated canceled ExecutionReport was observed. An order-list cancellation can still require
    /// additional ExecutionReport and ListStatus messages before its whole lifecycle is complete.
    /// </summary>
    ExchangeCancellationObserved = 1,

    /// <summary>Binance processed and rejected the correlated cancellation.</summary>
    ExchangeRejected = 2
}
