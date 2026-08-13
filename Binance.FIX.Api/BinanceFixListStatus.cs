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
