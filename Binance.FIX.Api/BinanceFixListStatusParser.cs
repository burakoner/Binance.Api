using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using QuickFix;
using QuickFix.Fields;

namespace Binance.FIX.Api;

internal static class BinanceFixListStatusParser
{
    internal const int ClientListIdTag = 25014;
    internal const int OriginalClientListIdTag = 25015;
    internal const int ContingencyTypeTag = 1385;
    internal const int ListStatusTypeTag = 429;
    internal const int ListOrderStatusTag = 431;
    internal const int ListRejectReasonTag = 1386;
    internal const int OrderRejectReasonTag = 103;
    internal const int ErrorCodeTag = 25016;
    internal const int NumberOfOrdersTag = 73;
    internal const int NumberOfListTriggeringInstructionsTag = 25010;
    internal const int ListTriggerTypeTag = 25011;
    internal const int ListTriggerOrderIndexTag = 25012;
    internal const int ListTriggerActionTag = 25013;

    private static readonly int[] OrderFieldOrder =
    [
        Tags.Symbol,
        Tags.OrderID,
        Tags.ClOrdID,
        NumberOfListTriggeringInstructionsTag,
        OrderRejectReasonTag,
        ErrorCodeTag,
        Tags.Text,
        0
    ];

    private static readonly int[] TriggerFieldOrder =
    [
        ListTriggerTypeTag,
        ListTriggerOrderIndexTag,
        ListTriggerActionTag,
        0
    ];

    private static readonly string[] TimestampFormats =
        ["yyyyMMdd-HH:mm:ss", "yyyyMMdd-HH:mm:ss.fff", "yyyyMMdd-HH:mm:ss.ffffff"];

    internal static BinanceFixListStatus Parse(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var messageType = GetRequiredHeaderValue(message, Tags.MsgType, "MsgType");
        if (!string.Equals(messageType, "N", StringComparison.Ordinal))
        {
            throw Invalid(Tags.MsgType, messageType, "Expected ListStatus MsgType N.");
        }

        if (message.RepeatedTags.Count != 0)
        {
            throw Invalid(Tags.MsgType, messageType, "ListStatus contains repeated fields outside parsed groups.");
        }

        if (message.GetGroupTags().Any(tag => tag != NumberOfOrdersTag))
        {
            throw Invalid(NumberOfOrdersTag, null, "ListStatus contains an unsupported top-level repeating group.");
        }

        return new BinanceFixListStatus
        {
            Symbol = GetOptionalUtf8(message, Tags.Symbol),
            ListId = GetOptionalPrintable(message, Tags.ListID, allowEmpty: false),
            ClientListId = GetOptionalClientOrderId(message, ClientListIdTag),
            OriginalClientListId = GetOptionalClientOrderId(message, OriginalClientListIdTag),
            ContingencyType = ParseOptionalContingencyType(message),
            StatusType = ParseListStatusType(GetRequiredValue(message, ListStatusTypeTag, "ListStatusType")),
            OrderStatus = ParseListOrderStatus(GetRequiredValue(message, ListOrderStatusTag, "ListOrderStatus")),
            ListRejectReason = ParseOptionalListRejectReason(message),
            OrderRejectReason = ParseOptionalOrderRejectReason(message),
            TransactionTime = GetOptionalTimestamp(message, Tags.TransactTime),
            ErrorCode = GetOptionalInt64(message, ErrorCodeTag),
            ErrorText = GetOptionalPrintable(message, Tags.Text, allowEmpty: true),
            Orders = ParseOrders(message)
        };
    }

    private static ReadOnlyCollection<BinanceFixListStatusOrder> ParseOrders(Message message)
    {
        var declaredCount = GetOptionalGroupCount(message, NumberOfOrdersTag);
        var hasParsedGroups = message.GetGroupTags().Contains(NumberOfOrdersTag);
        if (!hasParsedGroups)
        {
            if (declaredCount is > 0 ||
                message.IsSetField(Tags.ClOrdID) ||
                message.IsSetField(Tags.OrderID) ||
                message.IsSetField(NumberOfListTriggeringInstructionsTag))
            {
                throw Invalid(
                    NumberOfOrdersTag,
                    declaredCount?.ToString(CultureInfo.InvariantCulture),
                    "Positive or populated NoOrders requires dictionary-parsed order groups.");
            }

            return Array.Empty<BinanceFixListStatusOrder>().ToList().AsReadOnly();
        }

        var actualCount = message.GroupCount(NumberOfOrdersTag);
        if (declaredCount != actualCount)
        {
            throw Invalid(
                NumberOfOrdersTag,
                declaredCount?.ToString(CultureInfo.InvariantCulture),
                $"Declared order count does not match {actualCount} parsed groups.");
        }

        var orders = new List<BinanceFixListStatusOrder>(actualCount);
        for (var index = 1; index <= actualCount; index++)
        {
            var group = new Group(NumberOfOrdersTag, Tags.Symbol, OrderFieldOrder);
            message.GetGroup(index, group);
            if (group.RepeatedTags.Count != 0)
            {
                throw Invalid(NumberOfOrdersTag, actualCount.ToString(CultureInfo.InvariantCulture), "An order group contains repeated fields.");
            }

            if (group.GetGroupTags().Any(tag => tag != NumberOfListTriggeringInstructionsTag))
            {
                throw Invalid(NumberOfListTriggeringInstructionsTag, null, "An order contains an unsupported repeating group.");
            }

            orders.Add(new BinanceFixListStatusOrder
            {
                ClientOrderId = GetRequiredClientOrderId(group, Tags.ClOrdID, "ClOrdID"),
                Symbol = GetRequiredUtf8(group, Tags.Symbol, "Symbol"),
                OrderId = GetOptionalInt64(group, Tags.OrderID),
                TriggeringInstructions = ParseTriggeringInstructions(group),
                OrderRejectReason = ParseOptionalOrderRejectReason(group),
                ErrorCode = GetOptionalInt64(group, ErrorCodeTag),
                ErrorText = GetOptionalPrintable(group, Tags.Text, allowEmpty: true)
            });
        }

        return orders.AsReadOnly();
    }

    private static ReadOnlyCollection<BinanceFixListTriggeringInstruction> ParseTriggeringInstructions(Group order)
    {
        var declaredCount = GetOptionalGroupCount(order, NumberOfListTriggeringInstructionsTag);
        var hasParsedGroups = order.GetGroupTags().Contains(NumberOfListTriggeringInstructionsTag);
        if (!hasParsedGroups)
        {
            if (declaredCount is > 0 ||
                order.IsSetField(ListTriggerTypeTag) ||
                order.IsSetField(ListTriggerOrderIndexTag) ||
                order.IsSetField(ListTriggerActionTag))
            {
                throw Invalid(
                    NumberOfListTriggeringInstructionsTag,
                    declaredCount?.ToString(CultureInfo.InvariantCulture),
                    "Positive or populated trigger count requires dictionary-parsed trigger groups.");
            }

            return Array.Empty<BinanceFixListTriggeringInstruction>().ToList().AsReadOnly();
        }

        var actualCount = order.GroupCount(NumberOfListTriggeringInstructionsTag);
        if (declaredCount != actualCount)
        {
            throw Invalid(
                NumberOfListTriggeringInstructionsTag,
                declaredCount?.ToString(CultureInfo.InvariantCulture),
                $"Declared trigger count does not match {actualCount} parsed groups.");
        }

        var instructions = new List<BinanceFixListTriggeringInstruction>(actualCount);
        for (var index = 1; index <= actualCount; index++)
        {
            var group = new Group(
                NumberOfListTriggeringInstructionsTag,
                ListTriggerTypeTag,
                TriggerFieldOrder);
            order.GetGroup(index, group);
            if (group.RepeatedTags.Count != 0 || group.GetGroupTags().Count != 0)
            {
                throw Invalid(NumberOfListTriggeringInstructionsTag, actualCount.ToString(CultureInfo.InvariantCulture), "A trigger group contains repeated or nested fields.");
            }

            var triggerOrderIndex = GetRequiredInt64(group, ListTriggerOrderIndexTag, "ListTriggerTriggerIndex");
            if (triggerOrderIndex < 0)
            {
                throw Invalid(ListTriggerOrderIndexTag, triggerOrderIndex.ToString(CultureInfo.InvariantCulture), "Trigger order index must be zero or greater.");
            }

            instructions.Add(new BinanceFixListTriggeringInstruction
            {
                TriggerType = ParseListTriggerType(GetRequiredValue(group, ListTriggerTypeTag, "ListTriggerType")),
                TriggerOrderIndex = triggerOrderIndex,
                Action = ParseListTriggerAction(GetRequiredValue(group, ListTriggerActionTag, "ListTriggerAction"))
            });
        }

        return instructions.AsReadOnly();
    }

    private static BinanceFixContingencyType? ParseOptionalContingencyType(FieldMap fields)
        => GetOptionalValue(fields, ContingencyTypeTag) switch
        {
            null => null,
            "1" => BinanceFixContingencyType.OneCancelsTheOther,
            "2" => BinanceFixContingencyType.OneTriggersTheOther,
            var value => throw Invalid(ContingencyTypeTag, value, "Unsupported ContingencyType.")
        };

    private static BinanceFixListStatusType ParseListStatusType(string value)
        => value switch
        {
            "2" => BinanceFixListStatusType.Response,
            "4" => BinanceFixListStatusType.ExecutionStarted,
            "5" => BinanceFixListStatusType.AllDone,
            "100" => BinanceFixListStatusType.Updated,
            _ => throw Invalid(ListStatusTypeTag, value, "Unsupported ListStatusType.")
        };

    private static BinanceFixListOrderStatus ParseListOrderStatus(string value)
        => value switch
        {
            "3" => BinanceFixListOrderStatus.Executing,
            "6" => BinanceFixListOrderStatus.AllDone,
            "7" => BinanceFixListOrderStatus.Rejected,
            _ => throw Invalid(ListOrderStatusTag, value, "Unsupported ListOrderStatus.")
        };

    private static BinanceFixListRejectReason? ParseOptionalListRejectReason(FieldMap fields)
        => GetOptionalValue(fields, ListRejectReasonTag) switch
        {
            null => null,
            "99" => BinanceFixListRejectReason.Other,
            var value => throw Invalid(ListRejectReasonTag, value, "Unsupported ListRejectReason.")
        };

    private static BinanceFixOrderRejectReason? ParseOptionalOrderRejectReason(FieldMap fields)
        => GetOptionalValue(fields, OrderRejectReasonTag) switch
        {
            null => null,
            "99" => BinanceFixOrderRejectReason.Other,
            var value => throw Invalid(OrderRejectReasonTag, value, "Unsupported OrdRejReason.")
        };

    private static BinanceFixListTriggerType ParseListTriggerType(string value)
        => value switch
        {
            "1" => BinanceFixListTriggerType.Activated,
            "2" => BinanceFixListTriggerType.PartiallyFilled,
            "3" => BinanceFixListTriggerType.Filled,
            _ => throw Invalid(ListTriggerTypeTag, value, "Unsupported ListTriggerType.")
        };

    private static BinanceFixListTriggerAction ParseListTriggerAction(string value)
        => value switch
        {
            "1" => BinanceFixListTriggerAction.Release,
            "2" => BinanceFixListTriggerAction.Cancel,
            _ => throw Invalid(ListTriggerActionTag, value, "Unsupported ListTriggerAction.")
        };

    private static int? GetOptionalGroupCount(FieldMap fields, int tag)
    {
        var value = GetOptionalValue(fields, tag);
        if (value is null)
        {
            return null;
        }

        if (!ulong.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var parsed))
        {
            throw Invalid(tag, value, "NUMINGROUP must be an unsigned 64-bit integer.");
        }

        if (parsed > int.MaxValue)
        {
            throw Invalid(tag, value, "NUMINGROUP exceeds the local in-memory parser capacity.");
        }

        return (int)parsed;
    }

    private static long GetRequiredInt64(FieldMap fields, int tag, string fieldName)
        => ParseInt64(GetRequiredValue(fields, tag, fieldName), tag);

    private static long? GetOptionalInt64(FieldMap fields, int tag)
    {
        var value = GetOptionalValue(fields, tag);
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

    private static DateTimeOffset? GetOptionalTimestamp(FieldMap fields, int tag)
    {
        var value = GetOptionalValue(fields, tag);
        if (value is null)
        {
            return null;
        }

        if (!DateTimeOffset.TryParseExact(
            value,
            TimestampFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out var timestamp))
        {
            throw Invalid(tag, value, "UTCTIMESTAMP must use seconds, milliseconds, or microseconds.");
        }

        return timestamp;
    }

    private static string? GetOptionalClientOrderId(FieldMap fields, int tag)
    {
        var value = GetOptionalValue(fields, tag);
        return value is null ? null : ValidateClientOrderId(value, tag);
    }

    private static string GetRequiredClientOrderId(FieldMap fields, int tag, string fieldName)
        => ValidateClientOrderId(GetRequiredValue(fields, tag, fieldName), tag);

    private static string ValidateClientOrderId(string value, int tag)
    {
        if (value.Length is < 1 or > 36 || value.Any(character => !IsClientOrderIdCharacter(character)))
        {
            throw Invalid(tag, value, "Client order ID does not match ^[a-zA-Z0-9-_]{1,36}$.");
        }

        return value;
    }

    private static string GetRequiredPrintable(FieldMap fields, int tag, string fieldName)
        => ValidatePrintable(GetRequiredValue(fields, tag, fieldName), tag, allowEmpty: false);

    private static string? GetOptionalPrintable(FieldMap fields, int tag, bool allowEmpty)
    {
        var value = GetOptionalValue(fields, tag);
        return value is null ? null : ValidatePrintable(value, tag, allowEmpty);
    }

    private static string GetRequiredUtf8(FieldMap fields, int tag, string fieldName)
        => ValidateUtf8(GetRequiredValue(fields, tag, fieldName), tag);

    private static string? GetOptionalUtf8(FieldMap fields, int tag)
    {
        var value = GetOptionalValue(fields, tag);
        return value is null ? null : ValidateUtf8(value, tag);
    }

    private static string ValidateUtf8(string value, int tag)
    {
        if (!BinanceFixUtf8FieldValidator.IsValidNonEmptyValue(value))
        {
            throw Invalid(tag, value, "STRING must be non-empty valid Unicode without control characters.");
        }

        return value;
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

    private static bool IsClientOrderIdCharacter(char character)
        => character is >= 'a' and <= 'z'
            or >= 'A' and <= 'Z'
            or >= '0' and <= '9'
            or '-' or '_';

    private static FormatException Invalid(int tag, string? value, string reason)
        => new($"Invalid Binance ListStatus field {tag} value '{value ?? "<missing>"}'. {reason}");
}
