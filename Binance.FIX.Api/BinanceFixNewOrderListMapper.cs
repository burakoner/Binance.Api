using System;
using QuickFix;
using QuickFix.Fields;

namespace Binance.FIX.Api;

internal static class BinanceFixNewOrderListMapper
{
    internal const string MessageType = "E";
    internal const int NumberOfOrdersTag = 73;
    internal const int ContingencyTypeTag = 1385;
    internal const int NumberOfListTriggeringInstructionsTag = 25010;
    internal const int ListTriggerTypeTag = 25011;
    internal const int ListTriggerOrderIndexTag = 25012;
    internal const int ListTriggerActionTag = 25013;
    internal const int ClientListIdTag = 25014;
    internal const int OnePaysTheOtherTag = 25046;

    private static readonly int[] OrderFieldOrder =
    [
        Tags.ClOrdID,
        Tags.OrderQty,
        Tags.OrdType,
        Tags.ExecInst,
        Tags.Price,
        BinanceFixNewOrderMapper.TriggerTypeTag,
        BinanceFixNewOrderMapper.TriggerActionTag,
        BinanceFixNewOrderMapper.TriggerPriceTag,
        BinanceFixNewOrderMapper.TriggerPriceTypeTag,
        BinanceFixNewOrderMapper.TriggerPriceDirectionTag,
        BinanceFixNewOrderMapper.TriggerTrailingDeltaBipsTag,
        BinanceFixNewOrderMapper.PegOffsetValueTag,
        BinanceFixNewOrderMapper.PegPriceTypeTag,
        BinanceFixNewOrderMapper.PegMoveTypeTag,
        BinanceFixNewOrderMapper.PegOffsetTypeTag,
        Tags.Side,
        Tags.Symbol,
        Tags.TimeInForce,
        Tags.MaxFloor,
        Tags.CashOrderQty,
        BinanceFixNewOrderMapper.TargetStrategyTag,
        BinanceFixNewOrderMapper.StrategyIdTag,
        BinanceFixNewOrderMapper.SelfTradePreventionModeTag,
        NumberOfListTriggeringInstructionsTag,
        0
    ];

    private static readonly int[] TriggerFieldOrder =
        [ListTriggerTypeTag, ListTriggerOrderIndexTag, ListTriggerActionTag, 0];

    internal static Message CreateMessage(BinanceFixNewOrderListRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var message = new Message();
        message.Header.SetField(new MsgType(MessageType));
        message.SetField(new StringField(ClientListIdTag, request.ClientListId));
        message.SetField(new IntField(ContingencyTypeTag, (int)request.ContingencyType));
        if (request.IsOnePaysTheOther)
        {
            message.SetField(new BooleanField(OnePaysTheOtherTag, true));
        }

        for (var index = 0; index < request.Orders.Count; index++)
        {
            var group = new Group(NumberOfOrdersTag, Tags.ClOrdID, OrderFieldOrder);
            BinanceFixNewOrderMapper.ApplySharedFields(group, request.Orders[index]);
            ApplyListTriggers(group, request, index);
            message.AddGroup(group);
        }

        return message;
    }

    private static void ApplyListTriggers(
        Group orderGroup,
        BinanceFixNewOrderListRequest request,
        int orderIndex)
    {
        switch (request.ListType)
        {
            case BinanceFixOrderListType.OneCancelsTheOther:
                var otherIndex = orderIndex == 0 ? 1 : 0;
                AddTrigger(
                    orderGroup,
                    TriggerTypeFor(request.Orders[otherIndex]),
                    otherIndex,
                    BinanceFixListTriggerAction.Cancel);
                break;
            case BinanceFixOrderListType.OneTriggersTheOther:
            case BinanceFixOrderListType.OnePaysTheOther:
                if (orderIndex == 1)
                {
                    AddTrigger(
                        orderGroup,
                        BinanceFixListTriggerType.Filled,
                        0,
                        BinanceFixListTriggerAction.Release);
                }

                break;
            case BinanceFixOrderListType.OneTriggersTheOtherOneCancelsTheOther:
            case BinanceFixOrderListType.OnePaysTheOtherOneCancelsTheOther:
                if (orderIndex is 1 or 2)
                {
                    AddTrigger(
                        orderGroup,
                        BinanceFixListTriggerType.Filled,
                        0,
                        BinanceFixListTriggerAction.Cancel);
                    var counterpartIndex = orderIndex == 1 ? 2 : 1;
                    AddTrigger(
                        orderGroup,
                        TriggerTypeFor(request.Orders[counterpartIndex]),
                        counterpartIndex,
                        BinanceFixListTriggerAction.Cancel);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(request),
                    request.ListType,
                    "Unsupported order-list type.");
        }
    }

    private static BinanceFixListTriggerType TriggerTypeFor(BinanceFixNewOrderRequest triggerOrder)
        => triggerOrder.OrderType is BinanceFixOrderType.LimitMaker
            ? BinanceFixListTriggerType.PartiallyFilled
            : BinanceFixListTriggerType.Activated;

    private static void AddTrigger(
        Group orderGroup,
        BinanceFixListTriggerType triggerType,
        int triggerOrderIndex,
        BinanceFixListTriggerAction action)
    {
        var trigger = new Group(
            NumberOfListTriggeringInstructionsTag,
            ListTriggerTypeTag,
            TriggerFieldOrder);
        trigger.SetField(new CharField(ListTriggerTypeTag, (char)('0' + (int)triggerType)));
        trigger.SetField(new IntField(ListTriggerOrderIndexTag, triggerOrderIndex));
        trigger.SetField(new CharField(ListTriggerActionTag, (char)('0' + (int)action)));
        orderGroup.AddGroup(trigger);
    }
}
