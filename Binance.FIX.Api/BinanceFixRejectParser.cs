using System;
using System.Globalization;
using System.Linq;
using QuickFix;
using QuickFix.Fields;

namespace Binance.FIX.Api;

internal static class BinanceFixRejectParser
{
    internal const int ErrorCodeTag = 25016;

    internal static BinanceFixReject Parse(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var messageType = GetRequiredHeaderValue(message, Tags.MsgType, "MsgType");
        if (!string.Equals(messageType, MsgType.REJECT, StringComparison.Ordinal))
        {
            throw Invalid(Tags.MsgType, messageType, "Expected Reject MsgType 3.");
        }

        if (message.RepeatedTags.Count != 0)
        {
            throw Invalid(Tags.MsgType, messageType, "Reject cannot contain repeated fields.");
        }

        return new BinanceFixReject
        {
            ReferencedSequenceNumber = GetOptionalInt64(message, Tags.RefSeqNum),
            ReferencedTagId = GetOptionalInt64(message, Tags.RefTagID),
            ReferencedMessageType = GetOptionalPrintable(message, Tags.RefMsgType, allowEmpty: false),
            SessionRejectReason = ParseOptionalSessionRejectReason(message),
            ErrorCode = GetOptionalInt64(message, ErrorCodeTag),
            ErrorText = GetOptionalPrintable(message, Tags.Text, allowEmpty: true)
        };
    }

    private static BinanceFixSessionRejectReason? ParseOptionalSessionRejectReason(Message message)
        => GetOptionalValue(message, Tags.SessionRejectReason) switch
        {
            null => null,
            "0" => BinanceFixSessionRejectReason.InvalidTagNumber,
            "1" => BinanceFixSessionRejectReason.RequiredTagMissing,
            "2" => BinanceFixSessionRejectReason.TagNotDefinedForMessageType,
            "3" => BinanceFixSessionRejectReason.UndefinedTag,
            "5" => BinanceFixSessionRejectReason.IncorrectValue,
            "6" => BinanceFixSessionRejectReason.IncorrectDataFormat,
            "8" => BinanceFixSessionRejectReason.SignatureProblem,
            "10" => BinanceFixSessionRejectReason.SendingTimeAccuracyProblem,
            "12" => BinanceFixSessionRejectReason.XmlValidationError,
            "13" => BinanceFixSessionRejectReason.TagAppearsMoreThanOnce,
            "14" => BinanceFixSessionRejectReason.TagOutOfRequiredOrder,
            "15" => BinanceFixSessionRejectReason.RepeatingGroupFieldsOutOfOrder,
            "16" => BinanceFixSessionRejectReason.IncorrectRepeatingGroupCount,
            "99" => BinanceFixSessionRejectReason.Other,
            var value => throw Invalid(Tags.SessionRejectReason, value, "Unsupported SessionRejectReason.")
        };

    private static long? GetOptionalInt64(Message message, int tag)
    {
        var value = GetOptionalValue(message, tag);
        if (value is null)
        {
            return null;
        }

        if (!long.TryParse(value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var result))
        {
            throw Invalid(tag, value, "INT must be a signed 64-bit integer.");
        }

        return result;
    }

    private static string? GetOptionalPrintable(Message message, int tag, bool allowEmpty)
    {
        var value = GetOptionalValue(message, tag);
        if (value is null)
        {
            return null;
        }

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

    private static string? GetOptionalValue(FieldMap fields, int tag)
        => fields.IsSetField(tag) ? fields.GetString(tag) : null;

    private static FormatException Invalid(int tag, string? value, string reason)
        => new($"Invalid Binance Reject field {tag} value '{value ?? "<missing>"}'. {reason}");
}
