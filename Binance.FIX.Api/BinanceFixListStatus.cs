using System;
using System.Collections.Generic;

namespace Binance.FIX.Api;

/// <summary>
/// Immutable public projection of the current Binance Spot FIX ListStatus documentation/dictionary union.
/// </summary>
public sealed class BinanceFixListStatus
{
    internal BinanceFixListStatus()
    {
    }

    /// <summary>Gets the optional list symbol. The page marks it required while the official dictionary marks it optional.</summary>
    public string? Symbol { get; internal init; }

    /// <summary>Gets the optional exchange-assigned list identifier.</summary>
    public string? ListId { get; internal init; }

    /// <summary>Gets the optional request-assigned list identifier.</summary>
    public string? ClientListId { get; internal init; }

    /// <summary>Gets the optional prior request-assigned list identifier.</summary>
    public string? OriginalClientListId { get; internal init; }

    /// <summary>Gets the optional order-list contingency family.</summary>
    public BinanceFixContingencyType? ContingencyType { get; internal init; }

    /// <summary>Gets the required list lifecycle event.</summary>
    public BinanceFixListStatusType StatusType { get; internal init; }

    /// <summary>Gets the required aggregate list-order state.</summary>
    public BinanceFixListOrderStatus OrderStatus { get; internal init; }

    /// <summary>Gets the optional list rejection reason.</summary>
    public BinanceFixListRejectReason? ListRejectReason { get; internal init; }

    /// <summary>Gets the optional page-defined list-level order rejection reason.</summary>
    public BinanceFixOrderRejectReason? OrderRejectReason { get; internal init; }

    /// <summary>Gets the optional event timestamp.</summary>
    public DateTimeOffset? TransactionTime { get; internal init; }

    /// <summary>Gets the optional page-defined list-level API error code.</summary>
    public long? ErrorCode { get; internal init; }

    /// <summary>Gets the optional page-defined list-level error text.</summary>
    public string? ErrorText { get; internal init; }

    /// <summary>Gets the dictionary-preserved order entries.</summary>
    public IReadOnlyList<BinanceFixListStatusOrder> Orders { get; internal init; } = [];

    /// <summary>
    /// Reconciles this status against one ambiguous NewOrderList transport attempt.
    /// The account-wide ListStatus feed resolves only an ordinally equal ClListID with no
    /// contradictory contingency or nested order identity.
    /// </summary>
    /// <param name="request">The original caller-owned request.</param>
    /// <param name="deliveryStatus">The immediate transport result.</param>
    /// <returns>The exact published aggregate list-order state, or unresolved.</returns>
    public BinanceFixNewOrderListReconciliationStatus ReconcileNewOrderList(
        BinanceFixNewOrderListRequest request,
        BinanceFixDeliveryStatus deliveryStatus)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (deliveryStatus is not BinanceFixDeliveryStatus.UnknownDelivery
            || !MatchesNewOrderList(request))
        {
            return BinanceFixNewOrderListReconciliationStatus.Unresolved;
        }

        return OrderStatus switch
        {
            BinanceFixListOrderStatus.Executing
                => BinanceFixNewOrderListReconciliationStatus.ExchangeExecuting,
            BinanceFixListOrderStatus.AllDone
                => BinanceFixNewOrderListReconciliationStatus.ExchangeAllDone,
            BinanceFixListOrderStatus.Rejected
                => BinanceFixNewOrderListReconciliationStatus.ExchangeRejected,
            _ => BinanceFixNewOrderListReconciliationStatus.Unresolved
        };
    }

    private bool MatchesNewOrderList(BinanceFixNewOrderListRequest request)
    {
        if (!string.Equals(ClientListId, request.ClientListId, StringComparison.Ordinal)
            || (ContingencyType is not null && ContingencyType != request.ContingencyType)
            || Orders.Count > request.Orders.Count)
        {
            return false;
        }

        var matchedRequestOrders = new bool[request.Orders.Count];
        foreach (var statusOrder in Orders)
        {
            var matchIndex = -1;
            for (var index = 0; index < request.Orders.Count; index++)
            {
                var requestOrder = request.Orders[index];
                if (!matchedRequestOrders[index]
                    && string.Equals(
                        statusOrder.ClientOrderId,
                        requestOrder.ClientOrderId,
                        StringComparison.Ordinal)
                    && string.Equals(statusOrder.Symbol, requestOrder.Symbol, StringComparison.Ordinal))
                {
                    matchIndex = index;
                    break;
                }
            }

            if (matchIndex < 0)
            {
                return false;
            }

            matchedRequestOrders[matchIndex] = true;
        }

        return true;
    }
}

/// <summary>One order entry nested in a Binance Spot FIX ListStatus.</summary>
public sealed class BinanceFixListStatusOrder
{
    internal BinanceFixListStatusOrder()
    {
    }

    /// <summary>Gets the required request-assigned order identifier.</summary>
    public string ClientOrderId { get; internal init; } = string.Empty;

    /// <summary>Gets the required order symbol.</summary>
    public string Symbol { get; internal init; } = string.Empty;

    /// <summary>Gets the optional exchange-assigned order identifier. The page marks it required while the official dictionary marks it optional.</summary>
    public long? OrderId { get; internal init; }

    /// <summary>Gets the nested triggering instructions for this order.</summary>
    public IReadOnlyList<BinanceFixListTriggeringInstruction> TriggeringInstructions { get; internal init; } = [];

    /// <summary>Gets the optional dictionary-defined order rejection reason.</summary>
    public BinanceFixOrderRejectReason? OrderRejectReason { get; internal init; }

    /// <summary>Gets the optional dictionary-defined order API error code.</summary>
    public long? ErrorCode { get; internal init; }

    /// <summary>Gets the optional dictionary-defined order error text.</summary>
    public string? ErrorText { get; internal init; }
}

/// <summary>One instruction nested under one ListStatus order entry.</summary>
public sealed class BinanceFixListTriggeringInstruction
{
    internal BinanceFixListTriggeringInstruction()
    {
    }

    /// <summary>Gets the required order-state condition.</summary>
    public BinanceFixListTriggerType TriggerType { get; internal init; }

    /// <summary>Gets the required zero-based target order index.</summary>
    public long TriggerOrderIndex { get; internal init; }

    /// <summary>Gets the required action.</summary>
    public BinanceFixListTriggerAction Action { get; internal init; }
}
