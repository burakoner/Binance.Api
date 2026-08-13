using System;
using System.Collections.Generic;

namespace Binance.FIX.Api;

/// <summary>
/// Immutable public projection of the complete current Binance Spot FIX ExecutionReport surface.
/// </summary>
public sealed class BinanceFixExecutionReport
{
    internal BinanceFixExecutionReport()
    {
    }

    /// <summary>Gets the optional opaque execution ID.</summary>
    public string? ExecutionId { get; internal init; }

    /// <summary>Gets the optional client order ID.</summary>
    public string? ClientOrderId { get; internal init; }

    /// <summary>Gets the optional original client order ID.</summary>
    public string? OriginalClientOrderId { get; internal init; }

    /// <summary>Gets the optional exchange-assigned order ID.</summary>
    public long? OrderId { get; internal init; }

    /// <summary>Gets the optional base-asset order quantity.</summary>
    public decimal? OrderQuantity { get; internal init; }

    /// <summary>Gets the raw FIX order type.</summary>
    public BinanceFixOrdType OrderType { get; internal init; }

    /// <summary>Gets the order side.</summary>
    public BinanceFixOrderSide Side { get; internal init; }

    /// <summary>Gets the Spot symbol.</summary>
    public string Symbol { get; internal init; } = string.Empty;

    /// <summary>Gets the optional execution instruction.</summary>
    public BinanceFixExecutionInstruction? ExecutionInstruction { get; internal init; }

    /// <summary>Gets the optional order price.</summary>
    public decimal? Price { get; internal init; }

    /// <summary>Gets whether the trigger type is PRICE_MOVEMENT.</summary>
    public bool HasPriceMovementTrigger { get; internal init; }

    /// <summary>Gets whether the trigger action is ACTIVATE.</summary>
    public bool HasActivateTriggerAction { get; internal init; }

    /// <summary>Gets the optional trigger price.</summary>
    public decimal? TriggerPrice { get; internal init; }

    /// <summary>Gets whether the trigger price source is LAST_TRADE.</summary>
    public bool UsesLastTradeTriggerPrice { get; internal init; }

    /// <summary>Gets the optional trigger direction.</summary>
    public BinanceFixTriggerPriceDirection? TriggerPriceDirection { get; internal init; }

    /// <summary>Gets the optional trailing delta in basis points.</summary>
    public long? TriggerTrailingDeltaBips { get; internal init; }

    /// <summary>Gets the optional peg offset value.</summary>
    public decimal? PegOffsetValue { get; internal init; }

    /// <summary>Gets the optional peg price reference.</summary>
    public BinanceFixPegPriceType? PegPriceType { get; internal init; }

    /// <summary>Gets whether the peg move type is the required FIXED value.</summary>
    public bool HasFixedPegMoveType { get; internal init; }

    /// <summary>Gets whether the peg offset type is PRICE_TIER.</summary>
    public bool HasPriceTierPegOffsetType { get; internal init; }

    /// <summary>Gets the optional current pegged price.</summary>
    public decimal? PeggedPrice { get; internal init; }

    /// <summary>Gets the optional time-in-force value.</summary>
    public BinanceFixTimeInForce? TimeInForce { get; internal init; }

    /// <summary>Gets the optional event timestamp.</summary>
    public DateTimeOffset? TransactionTime { get; internal init; }

    /// <summary>Gets the optional order-creation timestamp.</summary>
    public DateTimeOffset? OrderCreationTime { get; internal init; }

    /// <summary>Gets the optional visible iceberg quantity.</summary>
    public decimal? IcebergQuantity { get; internal init; }

    /// <summary>Gets the optional opaque order-list ID.</summary>
    public string? ListId { get; internal init; }

    /// <summary>Gets the optional quote-asset order quantity.</summary>
    public decimal? CashOrderQuantity { get; internal init; }

    /// <summary>Gets the optional strategy type.</summary>
    public long? TargetStrategy { get; internal init; }

    /// <summary>Gets the optional caller-owned strategy ID.</summary>
    public long? StrategyId { get; internal init; }

    /// <summary>Gets the optional self-trade-prevention mode.</summary>
    public BinanceFixSelfTradePreventionMode? SelfTradePreventionMode { get; internal init; }

    /// <summary>Gets why this execution report was emitted.</summary>
    public BinanceFixExecutionType ExecutionType { get; internal init; }

    /// <summary>Gets the cumulative executed base-asset quantity.</summary>
    public decimal CumulativeQuantity { get; internal init; }

    /// <summary>Gets the optional remaining executable quantity.</summary>
    public decimal? LeavesQuantity { get; internal init; }

    /// <summary>Gets the optional cumulative quote-asset quantity.</summary>
    public decimal? CumulativeQuoteQuantity { get; internal init; }

    /// <summary>Gets whether the order was the aggressor, when reported.</summary>
    public bool? AggressorIndicator { get; internal init; }

    /// <summary>Gets the optional opaque trade ID.</summary>
    public string? TradeId { get; internal init; }

    /// <summary>Gets the optional last execution price.</summary>
    public decimal? LastPrice { get; internal init; }

    /// <summary>Gets the last execution quantity.</summary>
    public decimal LastQuantity { get; internal init; }

    /// <summary>Gets the current order status.</summary>
    public BinanceFixOrderStatus OrderStatus { get; internal init; }

    /// <summary>Gets the optional exchange allocation ID.</summary>
    public long? AllocationId { get; internal init; }

    /// <summary>Gets the optional match type.</summary>
    public BinanceFixMatchType? MatchType { get; internal init; }

    /// <summary>Gets the optional working floor.</summary>
    public BinanceFixWorkingFloor? WorkingFloor { get; internal init; }

    /// <summary>Gets the optional trailing-stop activation timestamp.</summary>
    public DateTimeOffset? TrailingTime { get; internal init; }

    /// <summary>Gets whether the order is working on the order book, when reported.</summary>
    public bool? WorkingIndicator { get; internal init; }

    /// <summary>Gets when the order appeared on the order book.</summary>
    public DateTimeOffset? WorkingTime { get; internal init; }

    /// <summary>Gets the optional prevented match ID.</summary>
    public long? PreventedMatchId { get; internal init; }

    /// <summary>Gets the optional prevented execution price.</summary>
    public decimal? PreventedExecutionPrice { get; internal init; }

    /// <summary>Gets the optional prevented execution quantity.</summary>
    public decimal? PreventedExecutionQuantity { get; internal init; }

    /// <summary>Gets the optional trade-group ID.</summary>
    public long? TradeGroupId { get; internal init; }

    /// <summary>Gets the optional counter symbol for an STP expiry.</summary>
    public string? CounterSymbol { get; internal init; }

    /// <summary>Gets the optional counter-order ID for an STP expiry.</summary>
    public long? CounterOrderId { get; internal init; }

    /// <summary>Gets the optional total prevented quantity.</summary>
    public decimal? PreventedQuantity { get; internal init; }

    /// <summary>Gets the optional last prevented quantity.</summary>
    public decimal? LastPreventedQuantity { get; internal init; }

    /// <summary>Gets whether Smart Order Routing was used, when reported.</summary>
    public bool? SmartOrderRouting { get; internal init; }

    /// <summary>Gets every miscellaneous-fee repeating-group entry.</summary>
    public IReadOnlyList<BinanceFixMiscFee> MiscellaneousFees { get; internal init; } = Array.Empty<BinanceFixMiscFee>();

    /// <summary>Gets the optional FIX order rejection reason.</summary>
    public BinanceFixOrderRejectReason? OrderRejectReason { get; internal init; }

    /// <summary>Gets the optional Binance API error code.</summary>
    public long? ErrorCode { get; internal init; }

    /// <summary>Gets the optional human-readable error text.</summary>
    public string? ErrorText { get; internal init; }

    /// <summary>Gets the optional dictionary-defined expiry reason.</summary>
    public BinanceFixExpiryReason? ExpiryReason { get; internal init; }

    /// <summary>
    /// Reconciles this report against one ambiguous NewOrderSingle transport attempt.
    /// Only an exact, ordinal ClOrdID match resolves UnknownDelivery.
    /// </summary>
    /// <param name="request">The original caller-owned request.</param>
    /// <param name="deliveryStatus">The immediate transport result.</param>
    /// <returns>The conservative exchange-processing resolution.</returns>
    public BinanceFixNewOrderReconciliationStatus ReconcileNewOrder(
        BinanceFixNewOrderRequest request,
        BinanceFixDeliveryStatus deliveryStatus)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (deliveryStatus is not BinanceFixDeliveryStatus.UnknownDelivery
            || !string.Equals(ClientOrderId, request.ClientOrderId, StringComparison.Ordinal))
        {
            return BinanceFixNewOrderReconciliationStatus.Unresolved;
        }

        return ExecutionType is BinanceFixExecutionType.Rejected
            || OrderStatus is BinanceFixOrderStatus.Rejected
                ? BinanceFixNewOrderReconciliationStatus.ExchangeRejected
                : BinanceFixNewOrderReconciliationStatus.ExchangeProcessed;
    }

    /// <summary>
    /// Reconciles this report against one ambiguous OrderCancelRequest transport attempt.
    /// A correlated canceled report proves only that one cancellation was observed; an order-list
    /// cancellation can produce additional ExecutionReport and ListStatus messages.
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
            || !MatchesCancelTarget(request)
            || ExecutionType is not BinanceFixExecutionType.Canceled
            || OrderStatus is not BinanceFixOrderStatus.Canceled)
        {
            return BinanceFixCancelReconciliationStatus.Unresolved;
        }

        return BinanceFixCancelReconciliationStatus.ExchangeCancellationObserved;
    }

    private bool MatchesCancelTarget(BinanceFixOrderCancelRequest request)
    {
        if (request.OrderId is not null && OrderId is not null && request.OrderId != OrderId)
        {
            return false;
        }

        if (request.OriginalClientOrderId is not null
            && OriginalClientOrderId is not null
            && !string.Equals(
                request.OriginalClientOrderId,
                OriginalClientOrderId,
                StringComparison.Ordinal))
        {
            return false;
        }

        return request.ListId is null
            || ListId is null
            || string.Equals(request.ListId, ListId, StringComparison.Ordinal);
    }
}
