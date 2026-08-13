using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using QuickFix;
using QuickFix.Fields;

namespace Binance.FIX.Api;

internal static class BinanceFixExecutionReportParser
{
    internal const int OrderCreationTimeTag = 25018;
    internal const int CumulativeQuoteQuantityTag = 25017;
    internal const int TradeIdTag = 1003;
    internal const int AllocationIdTag = 70;
    internal const int MatchTypeTag = 574;
    internal const int WorkingFloorTag = 25021;
    internal const int TrailingTimeTag = 25022;
    internal const int WorkingTimeTag = 25023;
    internal const int PreventedMatchIdTag = 25024;
    internal const int PreventedExecutionPriceTag = 25025;
    internal const int PreventedExecutionQuantityTag = 25026;
    internal const int TradeGroupIdTag = 25027;
    internal const int CounterSymbolTag = 25028;
    internal const int CounterOrderIdTag = 25029;
    internal const int PreventedQuantityTag = 25030;
    internal const int LastPreventedQuantityTag = 25031;
    internal const int SmartOrderRoutingTag = 25032;
    internal const int NumberOfMiscellaneousFeesTag = 136;
    internal const int MiscellaneousFeeAmountTag = 137;
    internal const int MiscellaneousFeeCurrencyTag = 138;
    internal const int MiscellaneousFeeTypeTag = 139;
    internal const int OrderRejectReasonTag = 103;
    internal const int ErrorCodeTag = 25016;
    internal const int ExpiryReasonTag = 25056;
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
    internal const int PeggedPriceTag = 839;
    internal const int TargetStrategyTag = 847;
    internal const int StrategyIdTag = 7940;
    internal const int SelfTradePreventionModeTag = 25001;

    private static readonly int[] MiscellaneousFeeFieldOrder =
        [MiscellaneousFeeAmountTag, MiscellaneousFeeCurrencyTag, MiscellaneousFeeTypeTag, 0];

    private static readonly string[] TimestampFormats =
        ["yyyyMMdd-HH:mm:ss", "yyyyMMdd-HH:mm:ss.fff", "yyyyMMdd-HH:mm:ss.ffffff"];

    internal static BinanceFixExecutionReport Parse(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var messageType = GetRequiredHeaderValue(message, Tags.MsgType, "MsgType");
        if (!string.Equals(messageType, MsgType.EXECUTION_REPORT, StringComparison.Ordinal))
        {
            throw Invalid(Tags.MsgType, messageType, "Expected ExecutionReport MsgType 8.");
        }

        var targetStrategy = GetOptionalInt64(message, TargetStrategyTag);
        if (targetStrategy is < 1_000_000)
        {
            throw Invalid(TargetStrategyTag, targetStrategy.Value.ToString(CultureInfo.InvariantCulture), "TargetStrategy must be at least 1,000,000.");
        }

        return new BinanceFixExecutionReport
        {
            ExecutionId = GetOptionalPrintable(message, Tags.ExecID, allowEmpty: false),
            ClientOrderId = GetOptionalClientOrderId(message, Tags.ClOrdID),
            OriginalClientOrderId = GetOptionalClientOrderId(message, Tags.OrigClOrdID),
            OrderId = GetOptionalInt64(message, Tags.OrderID),
            OrderQuantity = GetOptionalDecimal(message, Tags.OrderQty, requireNonNegative: true),
            OrderType = ParseOrderType(GetRequiredValue(message, Tags.OrdType, "OrdType")),
            Side = ParseSide(GetRequiredValue(message, Tags.Side, "Side")),
            Symbol = GetRequiredUtf8(message, Tags.Symbol, "Symbol"),
            ExecutionInstruction = ParseOptionalExecutionInstruction(message),
            Price = GetOptionalDecimal(message, Tags.Price, requireNonNegative: true),
            HasPriceMovementTrigger = HasOptionalFixedValue(message, TriggerTypeTag, "4"),
            HasActivateTriggerAction = HasOptionalFixedValue(message, TriggerActionTag, "1"),
            TriggerPrice = GetOptionalDecimal(message, TriggerPriceTag, requireNonNegative: true),
            UsesLastTradeTriggerPrice = HasOptionalFixedValue(message, TriggerPriceTypeTag, "2"),
            TriggerPriceDirection = ParseOptionalTriggerDirection(message),
            TriggerTrailingDeltaBips = GetOptionalInt64(message, TriggerTrailingDeltaBipsTag),
            PegOffsetValue = GetOptionalDecimal(message, PegOffsetValueTag, requireNonNegative: false),
            PegPriceType = ParseOptionalPegPriceType(message),
            HasFixedPegMoveType = HasOptionalFixedValue(message, PegMoveTypeTag, "1"),
            HasPriceTierPegOffsetType = HasOptionalFixedValue(message, PegOffsetTypeTag, "3"),
            PeggedPrice = GetOptionalDecimal(message, PeggedPriceTag, requireNonNegative: true),
            TimeInForce = ParseOptionalTimeInForce(message),
            TransactionTime = GetOptionalTimestamp(message, Tags.TransactTime),
            OrderCreationTime = GetOptionalTimestamp(message, OrderCreationTimeTag),
            IcebergQuantity = GetOptionalDecimal(message, Tags.MaxFloor, requireNonNegative: true),
            ListId = GetOptionalPrintable(message, Tags.ListID, allowEmpty: false),
            CashOrderQuantity = GetOptionalDecimal(message, Tags.CashOrderQty, requireNonNegative: true),
            TargetStrategy = targetStrategy,
            StrategyId = GetOptionalInt64(message, StrategyIdTag),
            SelfTradePreventionMode = ParseOptionalSelfTradePreventionMode(message),
            ExecutionType = ParseExecutionType(GetRequiredValue(message, Tags.ExecType, "ExecType")),
            CumulativeQuantity = GetRequiredDecimal(message, Tags.CumQty, "CumQty", requireNonNegative: true),
            LeavesQuantity = GetOptionalDecimal(message, Tags.LeavesQty, requireNonNegative: true),
            CumulativeQuoteQuantity = GetOptionalDecimal(message, CumulativeQuoteQuantityTag, requireNonNegative: true),
            AggressorIndicator = GetOptionalBoolean(message, Tags.AggressorIndicator),
            TradeId = GetOptionalPrintable(message, TradeIdTag, allowEmpty: false),
            LastPrice = GetOptionalDecimal(message, Tags.LastPx, requireNonNegative: true),
            LastQuantity = GetRequiredDecimal(message, Tags.LastQty, "LastQty", requireNonNegative: true),
            OrderStatus = ParseOrderStatus(GetRequiredValue(message, Tags.OrdStatus, "OrdStatus")),
            AllocationId = GetOptionalInt64(message, AllocationIdTag),
            MatchType = ParseOptionalMatchType(message),
            WorkingFloor = ParseOptionalWorkingFloor(message),
            TrailingTime = GetOptionalTimestamp(message, TrailingTimeTag),
            WorkingIndicator = GetOptionalBoolean(message, Tags.WorkingIndicator),
            WorkingTime = GetOptionalTimestamp(message, WorkingTimeTag),
            PreventedMatchId = GetOptionalInt64(message, PreventedMatchIdTag),
            PreventedExecutionPrice = GetOptionalDecimal(message, PreventedExecutionPriceTag, requireNonNegative: true),
            PreventedExecutionQuantity = GetOptionalDecimal(message, PreventedExecutionQuantityTag, requireNonNegative: true),
            TradeGroupId = GetOptionalInt64(message, TradeGroupIdTag),
            CounterSymbol = GetOptionalUtf8(message, CounterSymbolTag),
            CounterOrderId = GetOptionalInt64(message, CounterOrderIdTag),
            PreventedQuantity = GetOptionalDecimal(message, PreventedQuantityTag, requireNonNegative: true),
            LastPreventedQuantity = GetOptionalDecimal(message, LastPreventedQuantityTag, requireNonNegative: true),
            SmartOrderRouting = GetOptionalBoolean(message, SmartOrderRoutingTag),
            MiscellaneousFees = ParseMiscellaneousFees(message),
            OrderRejectReason = ParseOptionalOrderRejectReason(message),
            ErrorCode = GetOptionalInt64(message, ErrorCodeTag),
            ErrorText = GetOptionalPrintable(message, Tags.Text, allowEmpty: true),
            ExpiryReason = ParseOptionalExpiryReason(message)
        };
    }

    private static ReadOnlyCollection<BinanceFixMiscFee> ParseMiscellaneousFees(Message message)
    {
        var declaredCount = GetOptionalGroupCount(message, NumberOfMiscellaneousFeesTag);
        var hasParsedGroups = message.GetGroupTags().Contains(NumberOfMiscellaneousFeesTag);
        if (hasParsedGroups)
        {
            if (message.RepeatedTags.Count != 0)
            {
                throw Invalid(NumberOfMiscellaneousFeesTag, declaredCount?.ToString(CultureInfo.InvariantCulture), "ExecutionReport contains repeated fields outside parsed fee groups.");
            }

            var actualCount = message.GroupCount(NumberOfMiscellaneousFeesTag);
            if (declaredCount != actualCount)
            {
                throw Invalid(
                    NumberOfMiscellaneousFeesTag,
                    declaredCount?.ToString(CultureInfo.InvariantCulture),
                    $"Declared fee count does not match {actualCount} parsed groups.");
            }

            var parsedFees = new List<BinanceFixMiscFee>(actualCount);
            for (var index = 1; index <= actualCount; index++)
            {
                var group = new Group(
                    NumberOfMiscellaneousFeesTag,
                    MiscellaneousFeeAmountTag,
                    MiscellaneousFeeFieldOrder);
                message.GetGroup(index, group);
                if (group.RepeatedTags.Count != 0)
                {
                    throw Invalid(NumberOfMiscellaneousFeesTag, actualCount.ToString(CultureInfo.InvariantCulture), "A fee group contains repeated fields.");
                }

                parsedFees.Add(ParseFee(
                    GetRequiredValue(group, MiscellaneousFeeAmountTag, "MiscFeeAmt"),
                    GetRequiredValue(group, MiscellaneousFeeCurrencyTag, "MiscFeeCurr"),
                    GetRequiredValue(group, MiscellaneousFeeTypeTag, "MiscFeeType")));
            }

            return parsedFees.AsReadOnly();
        }

        return ParseDictionarylessFees(message, declaredCount);
    }

    private static ReadOnlyCollection<BinanceFixMiscFee> ParseDictionarylessFees(
        Message message,
        int? declaredCount)
    {
        var feeTags = new HashSet<int>
        {
            MiscellaneousFeeAmountTag,
            MiscellaneousFeeCurrencyTag,
            MiscellaneousFeeTypeTag
        };
        if (message.RepeatedTags.Any(field => !feeTags.Contains(field.Tag)))
        {
            throw Invalid(NumberOfMiscellaneousFeesTag, declaredCount?.ToString(CultureInfo.InvariantCulture), "ExecutionReport contains a repeated non-fee field.");
        }

        var hasTopLevelFeeField = feeTags.Any(message.IsSetField);
        if (declaredCount is null or 0)
        {
            if (hasTopLevelFeeField || message.RepeatedTags.Any(field => feeTags.Contains(field.Tag)))
            {
                throw Invalid(NumberOfMiscellaneousFeesTag, declaredCount?.ToString(CultureInfo.InvariantCulture), "Fee fields exist without a positive NoMiscFees count.");
            }

            return Array.Empty<BinanceFixMiscFee>().ToList().AsReadOnly();
        }

        if (!feeTags.All(message.IsSetField))
        {
            throw Invalid(NumberOfMiscellaneousFeesTag, declaredCount.Value.ToString(CultureInfo.InvariantCulture), "The first fee entry is incomplete.");
        }

        var repeatedFees = message.RepeatedTags.Where(field => feeTags.Contains(field.Tag)).ToArray();
        if (repeatedFees.Length % 3 != 0
            || declaredCount.Value != (repeatedFees.Length / 3) + 1)
        {
            throw Invalid(NumberOfMiscellaneousFeesTag, declaredCount.Value.ToString(CultureInfo.InvariantCulture), "Repeated fee field count does not match NoMiscFees.");
        }

        var fees = new List<BinanceFixMiscFee>(declaredCount.Value)
        {
            ParseFee(
                message.GetString(MiscellaneousFeeAmountTag),
                message.GetString(MiscellaneousFeeCurrencyTag),
                message.GetString(MiscellaneousFeeTypeTag))
        };

        for (var feeIndex = 1; feeIndex < declaredCount.Value; feeIndex++)
        {
            var offset = (feeIndex - 1) * 3;
            if (repeatedFees[offset].Tag != MiscellaneousFeeAmountTag
                || repeatedFees[offset + 1].Tag != MiscellaneousFeeCurrencyTag
                || repeatedFees[offset + 2].Tag != MiscellaneousFeeTypeTag)
            {
                throw Invalid(NumberOfMiscellaneousFeesTag, declaredCount.Value.ToString(CultureInfo.InvariantCulture), "Repeated fee fields are out of order.");
            }

            fees.Add(ParseFee(
                repeatedFees[offset].ToString()!,
                repeatedFees[offset + 1].ToString()!,
                repeatedFees[offset + 2].ToString()!));
        }

        return fees.AsReadOnly();
    }

    private static BinanceFixMiscFee ParseFee(string amount, string currency, string type)
        => new(
            ParseDecimal(amount, MiscellaneousFeeAmountTag, requireNonNegative: true),
            ValidateUtf8(currency, MiscellaneousFeeCurrencyTag),
            type switch
            {
                "4" => BinanceFixMiscFeeType.ExchangeFees,
                _ => throw Invalid(MiscellaneousFeeTypeTag, type, "Unsupported MiscFeeType.")
            });

    private static BinanceFixOrdType ParseOrderType(string value)
        => value switch
        {
            "1" => BinanceFixOrdType.Market,
            "2" => BinanceFixOrdType.Limit,
            "3" => BinanceFixOrdType.Stop,
            "4" => BinanceFixOrdType.StopLimit,
            "P" => BinanceFixOrdType.Pegged,
            _ => throw Invalid(Tags.OrdType, value, "Unsupported OrdType.")
        };

    private static BinanceFixOrderSide ParseSide(string value)
        => value switch
        {
            "1" => BinanceFixOrderSide.Buy,
            "2" => BinanceFixOrderSide.Sell,
            _ => throw Invalid(Tags.Side, value, "Unsupported Side.")
        };

    private static BinanceFixExecutionType ParseExecutionType(string value)
        => value switch
        {
            "0" => BinanceFixExecutionType.New,
            "4" => BinanceFixExecutionType.Canceled,
            "5" => BinanceFixExecutionType.Replaced,
            "8" => BinanceFixExecutionType.Rejected,
            "F" => BinanceFixExecutionType.Trade,
            "C" => BinanceFixExecutionType.Expired,
            _ => throw Invalid(Tags.ExecType, value, "Unsupported ExecType.")
        };

    private static BinanceFixOrderStatus ParseOrderStatus(string value)
        => value switch
        {
            "0" => BinanceFixOrderStatus.New,
            "1" => BinanceFixOrderStatus.PartiallyFilled,
            "2" => BinanceFixOrderStatus.Filled,
            "4" => BinanceFixOrderStatus.Canceled,
            "6" => BinanceFixOrderStatus.PendingCancel,
            "8" => BinanceFixOrderStatus.Rejected,
            "A" => BinanceFixOrderStatus.PendingNew,
            "C" => BinanceFixOrderStatus.Expired,
            _ => throw Invalid(Tags.OrdStatus, value, "Unsupported OrdStatus.")
        };

    private static BinanceFixExecutionInstruction? ParseOptionalExecutionInstruction(Message message)
        => GetOptionalValue(message, Tags.ExecInst) switch
        {
            null => null,
            "6" => BinanceFixExecutionInstruction.ParticipateDoNotInitiate,
            var value => throw Invalid(Tags.ExecInst, value, "Unsupported ExecInst.")
        };

    private static BinanceFixTriggerPriceDirection? ParseOptionalTriggerDirection(Message message)
        => GetOptionalValue(message, TriggerPriceDirectionTag) switch
        {
            null => null,
            "U" => BinanceFixTriggerPriceDirection.Up,
            "D" => BinanceFixTriggerPriceDirection.Down,
            var value => throw Invalid(TriggerPriceDirectionTag, value, "Unsupported TriggerPriceDirection.")
        };

    private static BinanceFixPegPriceType? ParseOptionalPegPriceType(Message message)
        => GetOptionalValue(message, PegPriceTypeTag) switch
        {
            null => null,
            "4" => BinanceFixPegPriceType.MarketPeg,
            "5" => BinanceFixPegPriceType.PrimaryPeg,
            var value => throw Invalid(PegPriceTypeTag, value, "Unsupported PegPriceType.")
        };

    private static BinanceFixTimeInForce? ParseOptionalTimeInForce(Message message)
        => GetOptionalValue(message, Tags.TimeInForce) switch
        {
            null => null,
            "1" => BinanceFixTimeInForce.GoodTillCanceled,
            "3" => BinanceFixTimeInForce.ImmediateOrCancel,
            "4" => BinanceFixTimeInForce.FillOrKill,
            var value => throw Invalid(Tags.TimeInForce, value, "Unsupported TimeInForce.")
        };

    private static BinanceFixSelfTradePreventionMode? ParseOptionalSelfTradePreventionMode(Message message)
        => GetOptionalValue(message, SelfTradePreventionModeTag) switch
        {
            null => null,
            "1" => BinanceFixSelfTradePreventionMode.None,
            "2" => BinanceFixSelfTradePreventionMode.ExpireTaker,
            "3" => BinanceFixSelfTradePreventionMode.ExpireMaker,
            "4" => BinanceFixSelfTradePreventionMode.ExpireBoth,
            "5" => BinanceFixSelfTradePreventionMode.Decrement,
            "6" => BinanceFixSelfTradePreventionMode.Transfer,
            var value => throw Invalid(SelfTradePreventionModeTag, value, "Unsupported SelfTradePreventionMode.")
        };

    private static BinanceFixMatchType? ParseOptionalMatchType(Message message)
        => GetOptionalValue(message, MatchTypeTag) switch
        {
            null => null,
            "1" => BinanceFixMatchType.OnePartyTradeReport,
            "4" => BinanceFixMatchType.AutoMatch,
            var value => throw Invalid(MatchTypeTag, value, "Unsupported MatchType.")
        };

    private static BinanceFixWorkingFloor? ParseOptionalWorkingFloor(Message message)
        => GetOptionalValue(message, WorkingFloorTag) switch
        {
            null => null,
            "1" => BinanceFixWorkingFloor.Exchange,
            "2" => BinanceFixWorkingFloor.Broker,
            "3" => BinanceFixWorkingFloor.SmartOrderRouter,
            var value => throw Invalid(WorkingFloorTag, value, "Unsupported WorkingFloor.")
        };

    private static BinanceFixOrderRejectReason? ParseOptionalOrderRejectReason(Message message)
        => GetOptionalValue(message, OrderRejectReasonTag) switch
        {
            null => null,
            "99" => BinanceFixOrderRejectReason.Other,
            var value => throw Invalid(OrderRejectReasonTag, value, "Unsupported OrdRejReason.")
        };

    private static BinanceFixExpiryReason? ParseOptionalExpiryReason(Message message)
        => GetOptionalValue(message, ExpiryReasonTag) switch
        {
            null => null,
            "1" => BinanceFixExpiryReason.Rejected,
            "2" => BinanceFixExpiryReason.ExchangeCanceled,
            "3" => BinanceFixExpiryReason.OcoTrigger,
            "4" => BinanceFixExpiryReason.OtoPhaseOneExpired,
            "5" => BinanceFixExpiryReason.UnfilledImmediateOrCancelQuantityExpired,
            "6" => BinanceFixExpiryReason.UnfilledFillOrKillOrderExpired,
            "7" => BinanceFixExpiryReason.InsufficientLiquidity,
            "8" => BinanceFixExpiryReason.ExecutionRulePriceRangeExceeded,
            var value => throw Invalid(ExpiryReasonTag, value, "Unsupported ExpiryReason.")
        };

    private static bool HasOptionalFixedValue(Message message, int tag, string expected)
    {
        var value = GetOptionalValue(message, tag);
        if (value is null)
        {
            return false;
        }

        if (!string.Equals(value, expected, StringComparison.Ordinal))
        {
            throw Invalid(tag, value, $"Expected fixed value {expected}.");
        }

        return true;
    }

    private static int? GetOptionalGroupCount(Message message, int tag)
    {
        var value = GetOptionalValue(message, tag);
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

    private static decimal? GetOptionalDecimal(Message message, int tag, bool requireNonNegative)
    {
        var value = GetOptionalValue(message, tag);
        return value is null ? null : ParseDecimal(value, tag, requireNonNegative);
    }

    private static decimal GetRequiredDecimal(
        Message message,
        int tag,
        string fieldName,
        bool requireNonNegative)
        => ParseDecimal(GetRequiredValue(message, tag, fieldName), tag, requireNonNegative);

    private static decimal ParseDecimal(string value, int tag, bool requireNonNegative)
    {
        if (!decimal.TryParse(
            value,
            NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out var result))
        {
            throw Invalid(tag, value, "Fixed-point value is invalid.");
        }

        if (requireNonNegative && result < 0)
        {
            throw Invalid(tag, value, "Value must not be negative.");
        }

        return result;
    }

    private static bool? GetOptionalBoolean(Message message, int tag)
        => GetOptionalValue(message, tag) switch
        {
            null => null,
            "Y" => true,
            "N" => false,
            var value => throw Invalid(tag, value, "BOOLEAN must be Y or N.")
        };

    private static DateTimeOffset? GetOptionalTimestamp(Message message, int tag)
    {
        var value = GetOptionalValue(message, tag);
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

    private static string? GetOptionalClientOrderId(Message message, int tag)
    {
        var value = GetOptionalValue(message, tag);
        if (value is null)
        {
            return null;
        }

        if (value.Length is < 1 or > 36 || value.Any(character => !IsClientOrderIdCharacter(character)))
        {
            throw Invalid(tag, value, "Client order ID does not match ^[a-zA-Z0-9-_]{1,36}$.");
        }

        return value;
    }

    private static string GetRequiredPrintable(Message message, int tag, string fieldName)
        => ValidatePrintable(GetRequiredValue(message, tag, fieldName), tag, allowEmpty: false);

    private static string? GetOptionalPrintable(Message message, int tag, bool allowEmpty)
    {
        var value = GetOptionalValue(message, tag);
        return value is null ? null : ValidatePrintable(value, tag, allowEmpty);
    }

    private static string GetRequiredUtf8(Message message, int tag, string fieldName)
        => ValidateUtf8(GetRequiredValue(message, tag, fieldName), tag);

    private static string? GetOptionalUtf8(Message message, int tag)
    {
        var value = GetOptionalValue(message, tag);
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
        => (character is >= 'a' and <= 'z')
            || (character is >= 'A' and <= 'Z')
            || (character is >= '0' and <= '9')
            || character is '-' or '_';

    private static FormatException Invalid(int tag, string? value, string reason)
        => new($"Invalid Binance ExecutionReport field {tag} value '{value ?? "<missing>"}'. {reason}");
}
