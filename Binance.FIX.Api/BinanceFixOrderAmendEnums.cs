namespace Binance.FIX.Api;

/// <summary>
/// Conservative resolution of an ambiguous OrderAmendKeepPriorityRequest transport attempt.
/// </summary>
public enum BinanceFixOrderAmendReconciliationStatus
{
    /// <summary>No exact correlated response resolved the ambiguous attempt.</summary>
    Unresolved = 0,

    /// <summary>
    /// Binance reported the replacement of one order. An order-list amendment can still require
    /// a subsequent ListStatus message.
    /// </summary>
    ExchangeReplacementObserved = 1,

    /// <summary>Binance processed and rejected the correlated amendment.</summary>
    ExchangeRejected = 2
}
