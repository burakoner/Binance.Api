using System;

namespace Binance.FIX.Api;

/// <summary>
/// Immutable public projection of the complete current Binance Spot FIX OrderAmendReject surface.
/// </summary>
public sealed class BinanceFixOrderAmendReject
{
    internal BinanceFixOrderAmendReject()
    {
    }

    /// <summary>Gets the client order ID echoed from the amendment.</summary>
    public string ClientOrderId { get; internal init; } = string.Empty;

    /// <summary>Gets the optional original client order ID echoed from the amendment.</summary>
    public string? OriginalClientOrderId { get; internal init; }

    /// <summary>Gets the optional exchange-assigned order ID echoed from the amendment.</summary>
    public long? OrderId { get; internal init; }

    /// <summary>Gets the symbol echoed from the amendment.</summary>
    public string Symbol { get; internal init; } = string.Empty;

    /// <summary>Gets the requested new total order quantity.</summary>
    public decimal NewQuantity { get; internal init; }

    /// <summary>Gets the Binance API error code.</summary>
    public long ErrorCode { get; internal init; }

    /// <summary>Gets the human-readable error text.</summary>
    public string ErrorText { get; internal init; } = string.Empty;

    /// <summary>
    /// Reconciles this rejection against one ambiguous OrderAmendKeepPriorityRequest attempt.
    /// </summary>
    /// <param name="request">The original caller-owned request.</param>
    /// <param name="deliveryStatus">The immediate transport result.</param>
    /// <returns>The conservative amendment resolution.</returns>
    public BinanceFixOrderAmendReconciliationStatus ReconcileAmend(
        BinanceFixOrderAmendRequest request,
        BinanceFixDeliveryStatus deliveryStatus)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (deliveryStatus is not BinanceFixDeliveryStatus.UnknownDelivery
            || !request.MatchesCoreResponse(ClientOrderId, Symbol, NewQuantity)
            || !request.MatchesEchoedTarget(OrderId, OriginalClientOrderId))
        {
            return BinanceFixOrderAmendReconciliationStatus.Unresolved;
        }

        return BinanceFixOrderAmendReconciliationStatus.ExchangeRejected;
    }
}
