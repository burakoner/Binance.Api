using System;
using System.Globalization;
using QuickFix;
using QuickFix.Fields;

namespace Binance.FIX.Api;

internal static class BinanceFixNewOrderMapper
{
    internal const int TargetStrategyTag = 847;
    internal const int StrategyIdTag = 7940;
    internal const int SelfTradePreventionModeTag = 25001;
    internal const int TriggerTypeTag = 1100;
    internal const int TriggerActionTag = 1101;
    internal const int TriggerPriceTag = 1102;
    internal const int TriggerPriceTypeTag = 1107;
    internal const int TriggerPriceDirectionTag = 1109;
    internal const int TriggerTrailingDeltaBipsTag = 25009;
    internal const int PegOffsetValueTag = 211;
    internal const int PegMoveTypeTag = 835;
    internal const int PegOffsetTypeTag = 836;
    internal const int PegPriceTypeTag = 1094;
    internal const int SmartOrderRoutingTag = 25032;

    internal static Message CreateMessage(BinanceFixNewOrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var message = new Message();
        message.Header.SetField(new MsgType(MsgType.ORDER_SINGLE));
        message.SetField(new ClOrdID(request.ClientOrderId));
        message.SetField(new OrdType(GetOrdType(request.OrderType)));
        message.SetField(new Side(GetSide(request.Side)));
        message.SetField(new Symbol(request.Symbol));

        SetDecimal(message, Tags.OrderQty, request.OrderQuantity);
        SetDecimal(message, Tags.CashOrderQty, request.CashOrderQuantity);
        SetDecimal(message, Tags.Price, request.Price);
        SetDecimal(message, Tags.MaxFloor, request.IcebergQuantity);

        if (request.OrderType is BinanceFixOrderType.LimitMaker)
        {
            message.SetField(new ExecInst("6"));
        }

        if (request.TimeInForce is not null)
        {
            message.SetField(new TimeInForce((char)('0' + (int)request.TimeInForce.Value)));
        }

        SetInt64(message, TargetStrategyTag, request.TargetStrategy);
        SetInt64(message, StrategyIdTag, request.StrategyId);

        if (request.SelfTradePreventionMode is not null)
        {
            message.SetField(new CharField(
                SelfTradePreventionModeTag,
                (char)('0' + (int)request.SelfTradePreventionMode.Value)));
        }

        ApplyTriggerFields(message, request);
        ApplyPegFields(message, request);

        if (request.SmartOrderRouting is not null)
        {
            message.SetField(new BooleanField(SmartOrderRoutingTag, request.SmartOrderRouting.Value));
        }

        return message;
    }

    private static void ApplyTriggerFields(Message message, BinanceFixNewOrderRequest request)
    {
        if (request.OrderType is not (BinanceFixOrderType.StopLoss
            or BinanceFixOrderType.StopLossLimit
            or BinanceFixOrderType.TakeProfit
            or BinanceFixOrderType.TakeProfitLimit))
        {
            return;
        }

        message.SetField(new CharField(TriggerTypeTag, '4'));
        message.SetField(new CharField(TriggerActionTag, '1'));
        message.SetField(new CharField(TriggerPriceTypeTag, '2'));
        message.SetField(new CharField(TriggerPriceDirectionTag, GetTriggerDirection(request)));
        SetDecimal(message, TriggerPriceTag, request.TriggerPrice);
        SetInt64(message, TriggerTrailingDeltaBipsTag, request.TriggerTrailingDeltaBips);
    }

    private static void ApplyPegFields(Message message, BinanceFixNewOrderRequest request)
    {
        if (request.OrderType is not BinanceFixOrderType.Pegged)
        {
            return;
        }

        message.SetField(new CharField(PegPriceTypeTag, (char)('0' + (int)request.PegPriceType!.Value)));
        message.SetField(new IntField(PegMoveTypeTag, 1));
        if (request.PegOffsetValue is not null)
        {
            message.SetField(new CharField(PegOffsetTypeTag, '3'));
            message.SetField(new DecimalField(PegOffsetValueTag, request.PegOffsetValue.Value));
        }
    }

    private static char GetOrdType(BinanceFixOrderType orderType)
        => orderType switch
        {
            BinanceFixOrderType.Market => '1',
            BinanceFixOrderType.Limit or BinanceFixOrderType.LimitMaker => '2',
            BinanceFixOrderType.StopLoss or BinanceFixOrderType.TakeProfit => '3',
            BinanceFixOrderType.StopLossLimit or BinanceFixOrderType.TakeProfitLimit => '4',
            BinanceFixOrderType.Pegged => 'P',
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, "Unsupported order type.")
        };

    private static char GetSide(BinanceFixOrderSide side)
        => (char)('0' + (int)side);

    private static char GetTriggerDirection(BinanceFixNewOrderRequest request)
    {
        var triggerUp = request.OrderType switch
        {
            BinanceFixOrderType.StopLoss or BinanceFixOrderType.StopLossLimit =>
                request.Side is BinanceFixOrderSide.Buy,
            BinanceFixOrderType.TakeProfit or BinanceFixOrderType.TakeProfitLimit =>
                request.Side is BinanceFixOrderSide.Sell,
            _ => throw new ArgumentOutOfRangeException(
                nameof(request),
                request.OrderType,
                "Only contingent orders have a trigger direction.")
        };

        return triggerUp ? 'U' : 'D';
    }

    private static void SetDecimal(Message message, int tag, decimal? value)
    {
        if (value is not null)
        {
            message.SetField(new DecimalField(tag, value.Value));
        }
    }

    private static void SetInt64(Message message, int tag, long? value)
    {
        if (value is not null)
        {
            message.SetField(new StringField(tag, value.Value.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
