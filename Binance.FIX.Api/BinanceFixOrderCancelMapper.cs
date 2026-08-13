using System;
using System.Globalization;
using QuickFix;
using QuickFix.Fields;

namespace Binance.FIX.Api;

internal static class BinanceFixOrderCancelMapper
{
    internal const int OriginalClientListIdTag = 25015;
    internal const int CancelRestrictionTag = 25002;

    internal static Message CreateMessage(BinanceFixOrderCancelRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var message = new Message();
        message.Header.SetField(new MsgType(MsgType.ORDER_CANCEL_REQUEST));
        message.SetField(new ClOrdID(request.ClientOrderId));
        message.SetField(new Symbol(request.Symbol));

        if (request.OrderId is not null)
        {
            message.SetField(new StringField(
                Tags.OrderID,
                request.OrderId.Value.ToString(CultureInfo.InvariantCulture)));
        }

        if (request.OriginalClientOrderId is not null)
        {
            message.SetField(new OrigClOrdID(request.OriginalClientOrderId));
        }

        if (request.ListId is not null)
        {
            message.SetField(new StringField(Tags.ListID, request.ListId));
        }

        if (request.OriginalClientListId is not null)
        {
            message.SetField(new StringField(OriginalClientListIdTag, request.OriginalClientListId));
        }

        if (request.CancelRestriction is not null)
        {
            message.SetField(new IntField(CancelRestrictionTag, (int)request.CancelRestriction.Value));
        }

        return message;
    }
}
