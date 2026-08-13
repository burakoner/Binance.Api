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
        ApplySharedFields(message, request);

        if (request.SmartOrderRouting is not null)
        {
            message.SetField(new BooleanField(SmartOrderRoutingTag, request.SmartOrderRouting.Value));
        }

        return message;
    }

    internal static void ApplySharedFields(FieldMap fields, BinanceFixNewOrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(fields);
        ArgumentNullException.ThrowIfNull(request);

        fields.SetField(new ClOrdID(request.ClientOrderId));
        fields.SetField(new OrdType(GetOrdType(request.OrderType)));
        fields.SetField(new Side(GetSide(request.Side)));
        fields.SetField(new Symbol(request.Symbol));

        SetDecimal(fields, Tags.OrderQty, request.OrderQuantity);
        SetDecimal(fields, Tags.CashOrderQty, request.CashOrderQuantity);
        SetDecimal(fields, Tags.Price, request.Price);
        SetDecimal(fields, Tags.MaxFloor, request.IcebergQuantity);

        if (request.OrderType is BinanceFixOrderType.LimitMaker)
        {
            fields.SetField(new ExecInst("6"));
        }

        if (request.TimeInForce is not null)
        {
            fields.SetField(new TimeInForce((char)('0' + (int)request.TimeInForce.Value)));
        }

        SetInt64(fields, TargetStrategyTag, request.TargetStrategy);
        SetInt64(fields, StrategyIdTag, request.StrategyId);

        if (request.SelfTradePreventionMode is not null)
        {
            fields.SetField(new CharField(
                SelfTradePreventionModeTag,
                (char)('0' + (int)request.SelfTradePreventionMode.Value)));
        }

        ApplyTriggerFields(fields, request);
        ApplyPegFields(fields, request);
    }

    private static void ApplyTriggerFields(FieldMap fields, BinanceFixNewOrderRequest request)
    {
        if (request.OrderType is not (BinanceFixOrderType.StopLoss
            or BinanceFixOrderType.StopLossLimit
            or BinanceFixOrderType.TakeProfit
            or BinanceFixOrderType.TakeProfitLimit))
        {
            return;
        }

        fields.SetField(new CharField(TriggerTypeTag, '4'));
        fields.SetField(new CharField(TriggerActionTag, '1'));
        fields.SetField(new CharField(TriggerPriceTypeTag, '2'));
        fields.SetField(new CharField(TriggerPriceDirectionTag, GetTriggerDirection(request)));
        SetDecimal(fields, TriggerPriceTag, request.TriggerPrice);
        SetInt64(fields, TriggerTrailingDeltaBipsTag, request.TriggerTrailingDeltaBips);
    }

    private static void ApplyPegFields(FieldMap fields, BinanceFixNewOrderRequest request)
    {
        if (request.OrderType is not BinanceFixOrderType.Pegged)
        {
            return;
        }

        fields.SetField(new CharField(PegPriceTypeTag, (char)('0' + (int)request.PegPriceType!.Value)));
        fields.SetField(new IntField(PegMoveTypeTag, 1));
        if (request.PegOffsetValue is not null)
        {
            fields.SetField(new CharField(PegOffsetTypeTag, '3'));
            fields.SetField(new DecimalField(PegOffsetValueTag, request.PegOffsetValue.Value));
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

    private static void SetDecimal(FieldMap fields, int tag, decimal? value)
    {
        if (value is not null)
        {
            fields.SetField(new DecimalField(tag, value.Value));
        }
    }

    private static void SetInt64(FieldMap fields, int tag, long? value)
    {
        if (value is not null)
        {
            fields.SetField(new StringField(tag, value.Value.ToString(CultureInfo.InvariantCulture)));
        }
    }
}
