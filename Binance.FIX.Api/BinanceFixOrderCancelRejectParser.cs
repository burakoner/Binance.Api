using System;
using System.Globalization;
using System.Linq;
using QuickFix;
using QuickFix.Fields;

namespace Binance.FIX.Api;

internal static class BinanceFixOrderCancelRejectParser
{
    internal const int OriginalClientListIdTag = 25015;
    internal const int CancelRestrictionTag = 25002;
    internal const int CancelRejectResponseToTag = 434;
    internal const int ErrorCodeTag = 25016;

    internal static BinanceFixOrderCancelReject Parse(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var messageType = GetRequiredHeaderValue(message, Tags.MsgType, "MsgType");
        if (!string.Equals(messageType, MsgType.ORDER_CANCEL_REJECT, StringComparison.Ordinal))
        {
            throw Invalid(Tags.MsgType, messageType, "Expected OrderCancelReject MsgType 9.");
        }

        if (message.RepeatedTags.Count != 0)
        {
            throw Invalid(Tags.MsgType, messageType, "OrderCancelReject cannot contain repeated fields.");
        }

        return new BinanceFixOrderCancelReject
        {
            ClientOrderId = GetRequiredClientId(message, Tags.ClOrdID, "ClOrdID"),
            OriginalClientOrderId = GetOptionalClientId(message, Tags.OrigClOrdID),
            OrderId = GetOptionalInt64(message, Tags.OrderID),
            OriginalClientListId = GetOptionalClientId(message, OriginalClientListIdTag),
            ListId = GetOptionalPrintable(message, Tags.ListID, allowEmpty: false),
            Symbol = GetRequiredPrintable(message, Tags.Symbol, "Symbol", allowEmpty: false),
            CancelRestriction = ParseOptionalCancelRestriction(message),
            ResponseTo = ParseResponseTo(GetRequiredValue(message, CancelRejectResponseToTag, "CxlRejResponseTo")),
            ErrorCode = GetRequiredInt64(message, ErrorCodeTag, "ErrorCode"),
            ErrorText = GetRequiredPrintable(message, Tags.Text, "Text", allowEmpty: false)
        };
    }

    private static BinanceFixCancelRestriction? ParseOptionalCancelRestriction(Message message)
        => GetOptionalValue(message, CancelRestrictionTag) switch
        {
            null => null,
            "1" => BinanceFixCancelRestriction.OnlyNew,
            "2" => BinanceFixCancelRestriction.OnlyPartiallyFilled,
            var value => throw Invalid(CancelRestrictionTag, value, "Unsupported CancelRestrictions value.")
        };

    private static BinanceFixCancelRejectResponseTo ParseResponseTo(string value)
        => value switch
        {
            "1" => BinanceFixCancelRejectResponseTo.OrderCancelRequest,
            _ => throw Invalid(CancelRejectResponseToTag, value, "Unsupported CxlRejResponseTo value.")
        };

    private static long GetRequiredInt64(Message message, int tag, string fieldName)
        => ParseInt64(GetRequiredValue(message, tag, fieldName), tag);

    private static long? GetOptionalInt64(Message message, int tag)
    {
        var value = GetOptionalValue(message, tag);
        return value is null ? null : ParseInt64(value, tag);
    }

    private static long ParseInt64(string value, int tag)
    {
        if (!long.TryParse(value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var result))
        {
            throw Invalid(tag, value, "INT must be a signed 64-bit integer.");
        }

        return result;
    }

    private static string GetRequiredClientId(Message message, int tag, string fieldName)
        => ValidateClientId(GetRequiredValue(message, tag, fieldName), tag);

    private static string? GetOptionalClientId(Message message, int tag)
    {
        var value = GetOptionalValue(message, tag);
        return value is null ? null : ValidateClientId(value, tag);
    }

    private static string ValidateClientId(string value, int tag)
    {
        if (value.Length is < 1 or > 36 || value.Any(character => !IsClientIdCharacter(character)))
        {
            throw Invalid(tag, value, "Client ID does not match ^[a-zA-Z0-9-_]{1,36}$.");
        }

        return value;
    }

    private static string GetRequiredPrintable(
        Message message,
        int tag,
        string fieldName,
        bool allowEmpty)
        => ValidatePrintable(GetRequiredValue(message, tag, fieldName), tag, allowEmpty);

    private static string? GetOptionalPrintable(Message message, int tag, bool allowEmpty)
    {
        var value = GetOptionalValue(message, tag);
        return value is null ? null : ValidatePrintable(value, tag, allowEmpty);
    }

    private static string ValidatePrintable(string value, int tag, bool allowEmpty)
    {
        if (!allowEmpty && value.Length == 0)
        {
            throw Invalid(tag, value, "STRING must not be empty.");
        }

        if (value.Any(character => character is < ' ' or > '~'))
        {
            throw Invalid(tag, value, "STRING must contain printable ASCII only.");
        }

        return value;
    }

    private static string GetRequiredHeaderValue(Message message, int tag, string fieldName)
    {
        if (!message.Header.IsSetField(tag))
        {
            throw Invalid(tag, null, $"Missing required header field {fieldName}.");
        }

        return message.Header.GetString(tag);
    }

    private static string GetRequiredValue(FieldMap fields, int tag, string fieldName)
    {
        if (!fields.IsSetField(tag))
        {
            throw Invalid(tag, null, $"Missing required field {fieldName}.");
        }

        return fields.GetString(tag);
    }

    private static string? GetOptionalValue(FieldMap fields, int tag)
        => fields.IsSetField(tag) ? fields.GetString(tag) : null;

    private static bool IsClientIdCharacter(char character)
        => (character is >= 'a' and <= 'z')
            || (character is >= 'A' and <= 'Z')
            || (character is >= '0' and <= '9')
            || character is '-' or '_';

    private static FormatException Invalid(int tag, string? value, string reason)
        => new($"Invalid Binance OrderCancelReject field {tag} value '{value ?? "<missing>"}'. {reason}");
}
