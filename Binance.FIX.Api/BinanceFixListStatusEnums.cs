namespace Binance.FIX.Api;

/// <summary>Published contingency family of a Binance Spot FIX order list.</summary>
public enum BinanceFixContingencyType
{
    /// <summary>One order cancels the other order.</summary>
    OneCancelsTheOther = 1,

    /// <summary>One order triggers the other order or orders.</summary>
    OneTriggersTheOther = 2
}

/// <summary>Published lifecycle event represented by a Binance Spot FIX ListStatus.</summary>
public enum BinanceFixListStatusType
{
    /// <summary>Response to an order-list request.</summary>
    Response = 2,

    /// <summary>Order-list execution started.</summary>
    ExecutionStarted = 4,

    /// <summary>All order-list processing is complete.</summary>
    AllDone = 5,

    /// <summary>An existing order list was updated.</summary>
    Updated = 100
}

/// <summary>Published aggregate order state represented by a Binance Spot FIX ListStatus.</summary>
public enum BinanceFixListOrderStatus
{
    /// <summary>One or more list orders are executing.</summary>
    Executing = 3,

    /// <summary>All list orders are done.</summary>
    AllDone = 6,

    /// <summary>The list request was rejected.</summary>
    Rejected = 7
}

/// <summary>
/// Resolution of an ambiguous NewOrderList transport attempt.
/// </summary>
public enum BinanceFixNewOrderListReconciliationStatus
{
    /// <summary>No exact correlated ListStatus resolved the ambiguous attempt.</summary>
    Unresolved = 0,

    /// <summary>Binance reported EXECUTING for the exact correlated order list.</summary>
    ExchangeExecuting = 1,

    /// <summary>Binance reported ALL_DONE for the exact correlated order list.</summary>
    ExchangeAllDone = 2,

    /// <summary>Binance reported REJECT for the exact correlated order-list request.</summary>
    ExchangeRejected = 3
}

/// <summary>Published reason for rejecting a Binance Spot FIX order list.</summary>
public enum BinanceFixListRejectReason
{
    /// <summary>Another reason; inspect the order-level and list-level error fields.</summary>
    Other = 99
}

/// <summary>Published order-state condition of a list triggering instruction.</summary>
public enum BinanceFixListTriggerType
{
    /// <summary>The referenced order was activated.</summary>
    Activated = 1,

    /// <summary>The referenced order was partially filled.</summary>
    PartiallyFilled = 2,

    /// <summary>The referenced order was filled.</summary>
    Filled = 3
}

/// <summary>Published action of a list triggering instruction.</summary>
public enum BinanceFixListTriggerAction
{
    /// <summary>Release the pending order.</summary>
    Release = 1,

    /// <summary>Cancel the target order.</summary>
    Cancel = 2
}
