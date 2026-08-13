namespace Binance.FIX.Api;

/// <summary>
/// Behavior when the cancellation phase of a cancel-and-new request fails.
/// </summary>
public enum BinanceFixCancelReplaceMode
{
    /// <summary>Do not attempt the new order when cancellation fails.</summary>
    StopOnFailure = 1,

    /// <summary>Attempt the new order even when cancellation fails.</summary>
    AllowFailure = 2
}

/// <summary>
/// Behavior when the account exceeds the unfilled-order rate limit during cancel-and-new processing.
/// </summary>
public enum BinanceFixOrderRateLimitExceededMode
{
    /// <summary>Do not cancel the existing order.</summary>
    DoNothing = 1,

    /// <summary>Cancel the existing order without placing the new order.</summary>
    CancelOnly = 2
}

/// <summary>
/// One phase-specific observation used to reconcile an ambiguous cancel-and-new transport attempt.
/// No single value claims that the complete two-phase operation is finished.
/// </summary>
public enum BinanceFixCancelReplaceReconciliationStatus
{
    /// <summary>No exact correlated response resolved a phase of the ambiguous attempt.</summary>
    Unresolved = 0,

    /// <summary>The cancellation phase produced a correlated canceled ExecutionReport.</summary>
    CancellationObserved = 1,

    /// <summary>The cancellation phase produced a correlated OrderCancelReject.</summary>
    CancellationRejected = 2,

    /// <summary>The new-order phase produced a correlated accepted-new ExecutionReport.</summary>
    NewOrderAccepted = 3,

    /// <summary>The new-order phase produced a correlated rejected ExecutionReport.</summary>
    NewOrderRejected = 4
}
