namespace Binance.FIX.Api;

/// <summary>
/// Describes what can be known immediately after attempting a FIX mutation write.
/// </summary>
public enum BinanceFixDeliveryStatus
{
    /// <summary>
    /// The mutation was rejected before the transport accepted a write attempt.
    /// </summary>
    NotSent = 0,

    /// <summary>
    /// The transport accepted or may have partially completed the write, but Binance has not confirmed the mutation.
    /// Reconcile through an execution report, Drop Copy, or REST; do not automatically replay it.
    /// </summary>
    UnknownDelivery = 1
}
