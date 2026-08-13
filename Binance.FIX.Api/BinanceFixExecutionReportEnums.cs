namespace Binance.FIX.Api;

/// <summary>
/// Raw FIX OrdType value reported by Binance. Contingent stop and take-profit semantics require the trigger direction as well.
/// </summary>
public enum BinanceFixOrdType
{
    /// <summary>Market order.</summary>
    Market = 1,

    /// <summary>Limit order.</summary>
    Limit = 2,

    /// <summary>Stop order that executes at market.</summary>
    Stop = 3,

    /// <summary>Stop order that executes as a limit order.</summary>
    StopLimit = 4,

    /// <summary>Pegged order.</summary>
    Pegged = 5
}

/// <summary>
/// Execution instruction reported by Binance Spot FIX.
/// </summary>
public enum BinanceFixExecutionInstruction
{
    /// <summary>Post-only; participate without initiating a trade.</summary>
    ParticipateDoNotInitiate = 1
}

/// <summary>
/// Reason for the current execution report.
/// </summary>
public enum BinanceFixExecutionType
{
    /// <summary>The order was accepted as new.</summary>
    New = 0,

    /// <summary>The order was canceled.</summary>
    Canceled = 1,

    /// <summary>The order was replaced.</summary>
    Replaced = 2,

    /// <summary>The request was rejected.</summary>
    Rejected = 3,

    /// <summary>A trade occurred.</summary>
    Trade = 4,

    /// <summary>The order expired.</summary>
    Expired = 5
}

/// <summary>
/// Current Binance order status.
/// </summary>
public enum BinanceFixOrderStatus
{
    /// <summary>New.</summary>
    New = 0,

    /// <summary>Partially filled.</summary>
    PartiallyFilled = 1,

    /// <summary>Filled.</summary>
    Filled = 2,

    /// <summary>Canceled.</summary>
    Canceled = 3,

    /// <summary>Pending cancellation.</summary>
    PendingCancel = 4,

    /// <summary>Rejected.</summary>
    Rejected = 5,

    /// <summary>Pending acceptance as new.</summary>
    PendingNew = 6,

    /// <summary>Expired. Binance converts EXPIRED_IN_MATCH to this FIX status.</summary>
    Expired = 7
}

/// <summary>
/// Price-trigger direction.
/// </summary>
public enum BinanceFixTriggerPriceDirection
{
    /// <summary>Trigger when the selected price rises to or through the trigger price.</summary>
    Up = 1,

    /// <summary>Trigger when the selected price falls to or through the trigger price.</summary>
    Down = 2
}

/// <summary>
/// Trade match type.
/// </summary>
public enum BinanceFixMatchType
{
    /// <summary>One-party trade report.</summary>
    OnePartyTradeReport = 1,

    /// <summary>Automatically matched trade.</summary>
    AutoMatch = 4
}

/// <summary>
/// Venue currently working an order.
/// </summary>
public enum BinanceFixWorkingFloor
{
    /// <summary>Exchange.</summary>
    Exchange = 1,

    /// <summary>Broker.</summary>
    Broker = 2,

    /// <summary>Smart Order Router.</summary>
    SmartOrderRouter = 3
}

/// <summary>
/// Miscellaneous fee type reported in the ExecutionReport repeating group.
/// </summary>
public enum BinanceFixMiscFeeType
{
    /// <summary>Exchange fee.</summary>
    ExchangeFees = 4
}

/// <summary>
/// FIX order rejection reason.
/// </summary>
public enum BinanceFixOrderRejectReason
{
    /// <summary>Other reason; inspect ErrorCode and Text.</summary>
    Other = 99
}

/// <summary>
/// Current dictionary-defined cause of order expiration.
/// </summary>
public enum BinanceFixExpiryReason
{
    /// <summary>Rejected.</summary>
    Rejected = 1,

    /// <summary>Canceled by the exchange.</summary>
    ExchangeCanceled = 2,

    /// <summary>Triggered by an OCO order.</summary>
    OcoTrigger = 3,

    /// <summary>The first phase of an OTO order expired.</summary>
    OtoPhaseOneExpired = 4,

    /// <summary>Unfilled immediate-or-cancel quantity expired.</summary>
    UnfilledImmediateOrCancelQuantityExpired = 5,

    /// <summary>Unfilled fill-or-kill order expired.</summary>
    UnfilledFillOrKillOrderExpired = 6,

    /// <summary>Insufficient liquidity.</summary>
    InsufficientLiquidity = 7,

    /// <summary>An execution-rule price range was exceeded.</summary>
    ExecutionRulePriceRangeExceeded = 8
}

/// <summary>
/// Resolution of an ambiguous NewOrderSingle transport attempt.
/// </summary>
public enum BinanceFixNewOrderReconciliationStatus
{
    /// <summary>No exact correlated report resolved the ambiguous attempt.</summary>
    Unresolved = 0,

    /// <summary>Binance processed the correlated request. Inspect the report for its current order state.</summary>
    ExchangeProcessed = 1,

    /// <summary>Binance processed and rejected the correlated request.</summary>
    ExchangeRejected = 2
}
