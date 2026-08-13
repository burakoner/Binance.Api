using System;
using System.Globalization;
using QuickFix;
using QuickFix.Fields;

namespace Binance.FIX.Api;

internal static class BinanceFixOrderAmendMapper
{
    internal const string MessageType = "XAK";

    internal static Message CreateMessage(BinanceFixOrderAmendRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var message = new Message();
        message.Header.SetField(new MsgType(MessageType));
        message.SetField(new ClOrdID(request.ClientOrderId));
        message.SetField(new Symbol(request.Symbol));
        message.SetField(new StringField(
            Tags.OrderQty,
            request.NewQuantity.ToString(CultureInfo.InvariantCulture)));

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

        return message;
    }
}
