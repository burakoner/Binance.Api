using System;
using System.Globalization;
using QuickFix;
using QuickFix.Fields;

namespace Binance.FIX.Api;

internal static class BinanceFixCancelReplaceMapper
{
    internal const string MessageType = "XCN";
    internal const int ModeTag = 25033;
    internal const int OrderRateLimitExceededModeTag = 25038;
    internal const int CancelClientOrderIdTag = 25034;
    internal const int CancelRestrictionTag = 25002;

    internal static Message CreateMessage(BinanceFixCancelReplaceRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var message = BinanceFixNewOrderMapper.CreateMessage(request.NewOrder);
        message.Header.SetField(new MsgType(MessageType));
        message.SetField(new IntField(ModeTag, (int)request.Mode));

        if (request.OrderRateLimitExceededMode is not null)
        {
            message.SetField(new IntField(
                OrderRateLimitExceededModeTag,
                (int)request.OrderRateLimitExceededMode.Value));
        }

        if (request.OrderId is not null)
        {
            message.SetField(new StringField(
                Tags.OrderID,
                request.OrderId.Value.ToString(CultureInfo.InvariantCulture)));
        }

        if (request.CancelClientOrderId is not null)
        {
            message.SetField(new StringField(CancelClientOrderIdTag, request.CancelClientOrderId));
        }

        if (request.OriginalClientOrderId is not null)
        {
            message.SetField(new OrigClOrdID(request.OriginalClientOrderId));
        }

        if (request.CancelRestriction is not null)
        {
            message.SetField(new IntField(CancelRestrictionTag, (int)request.CancelRestriction.Value));
        }

        return message;
    }
}
