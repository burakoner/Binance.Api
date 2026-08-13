using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Binance.FIX.Api;

/// <summary>Immutable, validated public request for one Binance Spot FIX NewOrderList message.</summary>
public sealed class BinanceFixNewOrderListRequest
{
    /// <summary>Creates one current Binance-supported OCO, OTO, OTOCO, OPO, or OPOCO list.</summary>
    /// <param name="clientListId">Caller-owned list ID matching <c>^[a-zA-Z0-9-_]{1,36}$</c>.</param>
    /// <param name="listType">The supported order-list shape.</param>
    /// <param name="orders">Orders in the exact semantic order required by the selected list shape.</param>
    public BinanceFixNewOrderListRequest(
        string clientListId,
        BinanceFixOrderListType listType,
        IReadOnlyList<BinanceFixNewOrderRequest> orders)
    {
        ValidateClientListId(clientListId);
        ValidateEnum(listType, nameof(listType));
        ArgumentNullException.ThrowIfNull(orders);

        var expectedCount = listType is BinanceFixOrderListType.OneCancelsTheOther
            or BinanceFixOrderListType.OneTriggersTheOther
            or BinanceFixOrderListType.OnePaysTheOther
                ? 2
                : 3;
        if (orders.Count != expectedCount)
        {
            throw new ArgumentException(
                $"The selected order-list type requires exactly {expectedCount} orders.",
                nameof(orders));
        }

        var copy = new BinanceFixNewOrderRequest[orders.Count];
        for (var index = 0; index < orders.Count; index++)
        {
            var order = orders[index];
            if (order is null)
            {
                throw new ArgumentException("Order-list entries must not be null.", nameof(orders));
            }

            if (order.SmartOrderRouting is not null)
            {
                throw new ArgumentException(
                    "The current NewOrderList surface does not publish SOR.",
                    nameof(orders));
            }

            copy[index] = order;
        }

        ValidateOrderShape(listType, copy);

        ClientListId = clientListId;
        ListType = listType;
        Orders = new ReadOnlyCollection<BinanceFixNewOrderRequest>(copy);
    }

    /// <summary>Gets the caller-owned FIX tag 25014 value.</summary>
    public string ClientListId { get; }

    /// <summary>Gets the selected Binance order-list shape.</summary>
    public BinanceFixOrderListType ListType { get; }

    /// <summary>Gets the validated orders in Binance-required sequence.</summary>
    public IReadOnlyList<BinanceFixNewOrderRequest> Orders { get; }

    internal BinanceFixContingencyType ContingencyType
        => ListType is BinanceFixOrderListType.OneCancelsTheOther
            ? BinanceFixContingencyType.OneCancelsTheOther
            : BinanceFixContingencyType.OneTriggersTheOther;

    internal bool IsOnePaysTheOther
        => ListType is BinanceFixOrderListType.OnePaysTheOther
            or BinanceFixOrderListType.OnePaysTheOtherOneCancelsTheOther;

    private static void ValidateOrderShape(
        BinanceFixOrderListType listType,
        IReadOnlyList<BinanceFixNewOrderRequest> orders)
    {
        switch (listType)
        {
            case BinanceFixOrderListType.OneCancelsTheOther:
                ValidateOneCancelsTheOther(orders);
                break;
            case BinanceFixOrderListType.OneTriggersTheOther:
                RequireWorkingOrder(orders[0], nameof(orders));
                break;
            case BinanceFixOrderListType.OneTriggersTheOtherOneCancelsTheOther:
                RequireWorkingOrder(orders[0], nameof(orders));
                ValidatePendingPair(orders[1], orders[2], allowTakeProfitLimit: false, nameof(orders));
                break;
            case BinanceFixOrderListType.OnePaysTheOther:
                RequireOnePaysTheOtherSides(orders[0], orders[1], nameof(orders));
                RequireWorkingOrder(orders[0], nameof(orders));
                break;
            case BinanceFixOrderListType.OnePaysTheOtherOneCancelsTheOther:
                RequireOnePaysTheOtherSides(orders[0], orders[1], nameof(orders));
                if (orders[2].Side is not BinanceFixOrderSide.Sell)
                {
                    throw InvalidShape(nameof(orders));
                }

                RequireWorkingOrder(orders[0], nameof(orders));
                ValidatePendingPair(orders[1], orders[2], allowTakeProfitLimit: true, nameof(orders));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(listType), listType, "Unsupported value.");
        }
    }

    private static void ValidateOneCancelsTheOther(IReadOnlyList<BinanceFixNewOrderRequest> orders)
    {
        var below = orders[0];
        var above = orders[1];
        if (below.Side != above.Side)
        {
            throw InvalidShape(nameof(orders));
        }

        var supported = below.Side switch
        {
            BinanceFixOrderSide.Sell => IsStop(below) && IsLimitMakerOrTakeProfit(above),
            BinanceFixOrderSide.Buy => IsLimitMakerOrTakeProfit(below) && IsStop(above),
            _ => false
        };
        if (!supported)
        {
            throw InvalidShape(nameof(orders));
        }
    }

    private static void ValidatePendingPair(
        BinanceFixNewOrderRequest below,
        BinanceFixNewOrderRequest above,
        bool allowTakeProfitLimit,
        string parameterName)
    {
        if (below.Side != above.Side)
        {
            throw InvalidShape(parameterName);
        }

        var supported = below.Side switch
        {
            BinanceFixOrderSide.Sell => IsStop(below)
                && (IsLimitMakerOrTakeProfit(above)
                    || (allowTakeProfitLimit && above.OrderType is BinanceFixOrderType.TakeProfitLimit)),
            BinanceFixOrderSide.Buy => IsLimitMakerOrTakeProfit(below) && IsStop(above),
            _ => false
        };
        if (!supported)
        {
            throw InvalidShape(parameterName);
        }
    }

    private static void RequireWorkingOrder(BinanceFixNewOrderRequest order, string parameterName)
    {
        if (order.OrderType is not (BinanceFixOrderType.Limit or BinanceFixOrderType.LimitMaker))
        {
            throw InvalidShape(parameterName);
        }
    }

    private static void RequireOnePaysTheOtherSides(
        BinanceFixNewOrderRequest working,
        BinanceFixNewOrderRequest pending,
        string parameterName)
    {
        if (working.Side is not BinanceFixOrderSide.Buy || pending.Side is not BinanceFixOrderSide.Sell)
        {
            throw InvalidShape(parameterName);
        }
    }

    private static bool IsStop(BinanceFixNewOrderRequest order)
        => order.OrderType is BinanceFixOrderType.StopLoss or BinanceFixOrderType.StopLossLimit;

    private static bool IsLimitMakerOrTakeProfit(BinanceFixNewOrderRequest order)
        => order.OrderType is BinanceFixOrderType.LimitMaker or BinanceFixOrderType.TakeProfit;

    private static ArgumentException InvalidShape(string parameterName)
        => new(
            "Order sides, types, or sequence do not match the selected Binance order-list shape.",
            parameterName);

    private static void ValidateClientListId(string value)
    {
        if (value is not { Length: >= 1 and <= 36 })
        {
            throw InvalidClientListId();
        }

        foreach (var character in value)
        {
            if ((character is >= 'a' and <= 'z')
                || (character is >= 'A' and <= 'Z')
                || (character is >= '0' and <= '9')
                || character is '-' or '_')
            {
                continue;
            }

            throw InvalidClientListId();
        }
    }

    private static ArgumentException InvalidClientListId()
        => new(
            "ClientListId must contain 1-36 ASCII letters, digits, hyphens, or underscores.",
            "clientListId");

    private static void ValidateEnum<TEnum>(TEnum value, string parameterName)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(value))
        {
            throw new ArgumentOutOfRangeException(parameterName, value, "Unsupported value.");
        }
    }
}
