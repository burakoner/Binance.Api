using System;

namespace Binance.FIX.Api;

/// <summary>
/// Immutable public projection of the complete current Binance Spot FIX OrderCancelReject surface.
/// </summary>
public sealed class BinanceFixOrderCancelReject
{
    internal BinanceFixOrderCancelReject()
    {
    }

    /// <summary>Gets the client order ID of the cancel request.</summary>
    public string ClientOrderId { get; internal init; } = string.Empty;

    /// <summary>Gets the optional original client order ID echoed from the request.</summary>
    public string? OriginalClientOrderId { get; internal init; }

    /// <summary>Gets the optional exchange-assigned order ID echoed from the request.</summary>
    public long? OrderId { get; internal init; }

    /// <summary>Gets the optional original client list ID echoed from the request.</summary>
    public string? OriginalClientListId { get; internal init; }

    /// <summary>Gets the optional opaque exchange-assigned order-list ID echoed from the request.</summary>
    public string? ListId { get; internal init; }

    /// <summary>Gets the symbol echoed from the request.</summary>
    public string Symbol { get; internal init; } = string.Empty;

    /// <summary>Gets the optional cancel restriction echoed from the request.</summary>
    public BinanceFixCancelRestriction? CancelRestriction { get; internal init; }

    /// <summary>Gets the request type rejected by Binance.</summary>
    public BinanceFixCancelRejectResponseTo ResponseTo { get; internal init; }

    /// <summary>Gets the Binance API error code.</summary>
    public long ErrorCode { get; internal init; }

    /// <summary>Gets the human-readable error text.</summary>
    public string ErrorText { get; internal init; } = string.Empty;

    /// <summary>
    /// Reconciles this rejection against one ambiguous OrderCancelRequest transport attempt.
    /// Only an exact request ID and symbol match can resolve UnknownDelivery.
    /// </summary>
    /// <param name="request">The original caller-owned request.</param>
    /// <param name="deliveryStatus">The immediate transport result.</param>
    /// <returns>The conservative cancellation resolution.</returns>
    public BinanceFixCancelReconciliationStatus ReconcileCancel(
        BinanceFixOrderCancelRequest request,
        BinanceFixDeliveryStatus deliveryStatus)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (deliveryStatus is not BinanceFixDeliveryStatus.UnknownDelivery
            || !request.MatchesCoreResponse(ClientOrderId, Symbol)
            || !MatchesEchoedTarget(request))
        {
            return BinanceFixCancelReconciliationStatus.Unresolved;
        }

        return BinanceFixCancelReconciliationStatus.ExchangeRejected;
    }

    /// <summary>
    /// Reconciles this rejection as the cancellation-phase outcome of one ambiguous cancel-and-new attempt.
    /// An explicit CancelClOrdID is required because tag 11 belongs to the new order in the combined request.
    /// </summary>
    /// <param name="request">The original caller-owned request.</param>
    /// <param name="deliveryStatus">The immediate transport result.</param>
    /// <returns>The conservative phase-specific resolution.</returns>
    public BinanceFixCancelReplaceReconciliationStatus ReconcileCancelReplace(
        BinanceFixCancelReplaceRequest request,
        BinanceFixDeliveryStatus deliveryStatus)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (deliveryStatus is not BinanceFixDeliveryStatus.UnknownDelivery
            || request.CancelClientOrderId is null
            || !string.Equals(ClientOrderId, request.CancelClientOrderId, StringComparison.Ordinal)
            || !string.Equals(Symbol, request.NewOrder.Symbol, StringComparison.Ordinal)
            || ListId is not null
            || OriginalClientListId is not null
            || (OrderId is not null && OrderId != request.OrderId)
            || (OriginalClientOrderId is not null
                && !string.Equals(
                    OriginalClientOrderId,
                    request.OriginalClientOrderId,
                    StringComparison.Ordinal))
            || (CancelRestriction is not null && CancelRestriction != request.CancelRestriction))
        {
            return BinanceFixCancelReplaceReconciliationStatus.Unresolved;
        }

        return BinanceFixCancelReplaceReconciliationStatus.CancellationRejected;
    }

    private bool MatchesEchoedTarget(BinanceFixOrderCancelRequest request)
    {
        if (request.Target is BinanceFixOrderCancelTarget.Order)
        {
            if (ListId is not null || OriginalClientListId is not null)
            {
                return false;
            }

            if (OrderId is not null && OrderId != request.OrderId)
            {
                return false;
            }

            if (OriginalClientOrderId is not null
                && !string.Equals(
                    OriginalClientOrderId,
                    request.OriginalClientOrderId,
                    StringComparison.Ordinal))
            {
                return false;
            }
        }
        else
        {
            if (OrderId is not null || OriginalClientOrderId is not null)
            {
                return false;
            }

            if (ListId is not null
                && !string.Equals(ListId, request.ListId, StringComparison.Ordinal))
            {
                return false;
            }

            if (OriginalClientListId is not null
                && !string.Equals(
                    OriginalClientListId,
                    request.OriginalClientListId,
                    StringComparison.Ordinal))
            {
                return false;
            }
        }

        return CancelRestriction is null || CancelRestriction == request.CancelRestriction;
    }
}
