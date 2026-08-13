using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using QuickFix;
using QuickFix.Fields;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class ExecutionReportTests
{
    [Fact]
    public void ParsesCompleteCurrentExecutionReportSurface()
    {
        var message = CreateRequiredReport("F", "1", "client_1");
        Set(message, Tags.ExecID, "exec-A");
        Set(message, Tags.OrigClOrdID, "original_1");
        Set(message, Tags.OrderID, long.MaxValue.ToString(CultureInfo.InvariantCulture));
        Set(message, Tags.OrderQty, "1.5");
        Set(message, Tags.OrdType, "P");
        Set(message, Tags.Side, "2");
        Set(message, Tags.ExecInst, "6");
        Set(message, Tags.Price, "10.25");
        Set(message, BinanceFixExecutionReportParser.TriggerTypeTag, "4");
        Set(message, BinanceFixExecutionReportParser.TriggerActionTag, "1");
        Set(message, BinanceFixExecutionReportParser.TriggerPriceTag, "9.75");
        Set(message, BinanceFixExecutionReportParser.TriggerPriceTypeTag, "2");
        Set(message, BinanceFixExecutionReportParser.TriggerPriceDirectionTag, "D");
        Set(message, BinanceFixExecutionReportParser.TriggerTrailingDeltaBipsTag, "125");
        Set(message, BinanceFixExecutionReportParser.PegOffsetValueTag, "-2.5");
        Set(message, BinanceFixExecutionReportParser.PegPriceTypeTag, "5");
        Set(message, BinanceFixExecutionReportParser.PegMoveTypeTag, "1");
        Set(message, BinanceFixExecutionReportParser.PegOffsetTypeTag, "3");
        Set(message, BinanceFixExecutionReportParser.PeggedPriceTag, "10.5");
        Set(message, Tags.TimeInForce, "1");
        Set(message, Tags.TransactTime, "20260813-12:34:56.123456");
        Set(message, BinanceFixExecutionReportParser.OrderCreationTimeTag, "20260813-12:30:00.123");
        Set(message, Tags.MaxFloor, "0.25");
        Set(message, Tags.ListID, "list-A");
        Set(message, Tags.CashOrderQty, "20.5");
        Set(message, BinanceFixExecutionReportParser.TargetStrategyTag, "1000000");
        Set(message, BinanceFixExecutionReportParser.StrategyIdTag, long.MinValue.ToString(CultureInfo.InvariantCulture));
        Set(message, BinanceFixExecutionReportParser.SelfTradePreventionModeTag, "6");
        Set(message, Tags.CumQty, "1");
        Set(message, Tags.LeavesQty, "0.5");
        Set(message, BinanceFixExecutionReportParser.CumulativeQuoteQuantityTag, "12.75");
        Set(message, Tags.AggressorIndicator, "Y");
        Set(message, BinanceFixExecutionReportParser.TradeIdTag, "trade-A");
        Set(message, Tags.LastPx, "10.1");
        Set(message, Tags.LastQty, "1");
        Set(message, BinanceFixExecutionReportParser.AllocationIdTag, "42");
        Set(message, BinanceFixExecutionReportParser.MatchTypeTag, "4");
        Set(message, BinanceFixExecutionReportParser.WorkingFloorTag, "3");
        Set(message, BinanceFixExecutionReportParser.TrailingTimeTag, "20260813-12:31:00");
        Set(message, Tags.WorkingIndicator, "N");
        Set(message, BinanceFixExecutionReportParser.WorkingTimeTag, "20260813-12:32:00.123456");
        Set(message, BinanceFixExecutionReportParser.PreventedMatchIdTag, "43");
        Set(message, BinanceFixExecutionReportParser.PreventedExecutionPriceTag, "10.2");
        Set(message, BinanceFixExecutionReportParser.PreventedExecutionQuantityTag, "0.1");
        Set(message, BinanceFixExecutionReportParser.TradeGroupIdTag, "44");
        Set(message, BinanceFixExecutionReportParser.CounterSymbolTag, "ETHUSDT");
        Set(message, BinanceFixExecutionReportParser.CounterOrderIdTag, "45");
        Set(message, BinanceFixExecutionReportParser.PreventedQuantityTag, "0.2");
        Set(message, BinanceFixExecutionReportParser.LastPreventedQuantityTag, "0.1");
        Set(message, BinanceFixExecutionReportParser.SmartOrderRoutingTag, "N");
        AddFee(message, "0.01", "USDT");
        AddFee(message, "0.02", "BNB");
        Set(message, BinanceFixExecutionReportParser.OrderRejectReasonTag, "99");
        Set(message, BinanceFixExecutionReportParser.ErrorCodeTag, "-2010");
        Set(message, Tags.Text, "current error text");
        Set(message, BinanceFixExecutionReportParser.ExpiryReasonTag, "8");

        var report = BinanceFixExecutionReportParser.Parse(message);

        Assert.Equal("exec-A", report.ExecutionId);
        Assert.Equal("client_1", report.ClientOrderId);
        Assert.Equal("original_1", report.OriginalClientOrderId);
        Assert.Equal(long.MaxValue, report.OrderId);
        Assert.Equal(1.5m, report.OrderQuantity);
        Assert.Equal(BinanceFixOrdType.Pegged, report.OrderType);
        Assert.Equal(BinanceFixOrderSide.Sell, report.Side);
        Assert.Equal("BTCUSDT", report.Symbol);
        Assert.Equal(BinanceFixExecutionInstruction.ParticipateDoNotInitiate, report.ExecutionInstruction);
        Assert.Equal(10.25m, report.Price);
        Assert.True(report.HasPriceMovementTrigger);
        Assert.True(report.HasActivateTriggerAction);
        Assert.Equal(9.75m, report.TriggerPrice);
        Assert.True(report.UsesLastTradeTriggerPrice);
        Assert.Equal(BinanceFixTriggerPriceDirection.Down, report.TriggerPriceDirection);
        Assert.Equal(125, report.TriggerTrailingDeltaBips);
        Assert.Equal(-2.5m, report.PegOffsetValue);
        Assert.Equal(BinanceFixPegPriceType.PrimaryPeg, report.PegPriceType);
        Assert.True(report.HasFixedPegMoveType);
        Assert.True(report.HasPriceTierPegOffsetType);
        Assert.Equal(10.5m, report.PeggedPrice);
        Assert.Equal(BinanceFixTimeInForce.GoodTillCanceled, report.TimeInForce);
        Assert.Equal(ParseTimestamp("20260813-12:34:56.123456"), report.TransactionTime);
        Assert.Equal(ParseTimestamp("20260813-12:30:00.123"), report.OrderCreationTime);
        Assert.Equal(0.25m, report.IcebergQuantity);
        Assert.Equal("list-A", report.ListId);
        Assert.Equal(20.5m, report.CashOrderQuantity);
        Assert.Equal(1_000_000, report.TargetStrategy);
        Assert.Equal(long.MinValue, report.StrategyId);
        Assert.Equal(BinanceFixSelfTradePreventionMode.Transfer, report.SelfTradePreventionMode);
        Assert.Equal(BinanceFixExecutionType.Trade, report.ExecutionType);
        Assert.Equal(1m, report.CumulativeQuantity);
        Assert.Equal(0.5m, report.LeavesQuantity);
        Assert.Equal(12.75m, report.CumulativeQuoteQuantity);
        Assert.True(report.AggressorIndicator);
        Assert.Equal("trade-A", report.TradeId);
        Assert.Equal(10.1m, report.LastPrice);
        Assert.Equal(1m, report.LastQuantity);
        Assert.Equal(BinanceFixOrderStatus.PartiallyFilled, report.OrderStatus);
        Assert.Equal(42, report.AllocationId);
        Assert.Equal(BinanceFixMatchType.AutoMatch, report.MatchType);
        Assert.Equal(BinanceFixWorkingFloor.SmartOrderRouter, report.WorkingFloor);
        Assert.Equal(ParseTimestamp("20260813-12:31:00"), report.TrailingTime);
        Assert.False(report.WorkingIndicator);
        Assert.Equal(ParseTimestamp("20260813-12:32:00.123456"), report.WorkingTime);
        Assert.Equal(43, report.PreventedMatchId);
        Assert.Equal(10.2m, report.PreventedExecutionPrice);
        Assert.Equal(0.1m, report.PreventedExecutionQuantity);
        Assert.Equal(44, report.TradeGroupId);
        Assert.Equal("ETHUSDT", report.CounterSymbol);
        Assert.Equal(45, report.CounterOrderId);
        Assert.Equal(0.2m, report.PreventedQuantity);
        Assert.Equal(0.1m, report.LastPreventedQuantity);
        Assert.False(report.SmartOrderRouting);
        Assert.Collection(
            report.MiscellaneousFees,
            fee =>
            {
                Assert.Equal(0.01m, fee.Amount);
                Assert.Equal("USDT", fee.Currency);
                Assert.Equal(BinanceFixMiscFeeType.ExchangeFees, fee.Type);
            },
            fee =>
            {
                Assert.Equal(0.02m, fee.Amount);
                Assert.Equal("BNB", fee.Currency);
                Assert.Equal(BinanceFixMiscFeeType.ExchangeFees, fee.Type);
            });
        Assert.Equal(BinanceFixOrderRejectReason.Other, report.OrderRejectReason);
        Assert.Equal(-2010, report.ErrorCode);
        Assert.Equal("current error text", report.ErrorText);
        Assert.Equal(BinanceFixExpiryReason.ExecutionRulePriceRangeExceeded, report.ExpiryReason);
        Assert.Throws<NotSupportedException>(() =>
            ((IList<BinanceFixMiscFee>)report.MiscellaneousFees).Add(report.MiscellaneousFees[0]));
    }

    [Fact]
    public void RecoversEveryFeeFromDictionarylessQuickFixParsing()
    {
        var wireMessage = CreateDictionarylessWireMessage(
            "136=2",
            "137=0.01",
            "138=USDT",
            "139=4",
            "137=0.02",
            "138=BNB",
            "139=4");
        var message = ParseWithoutDictionary(wireMessage);

        Assert.Equal(3, message.RepeatedTags.Count);

        var report = BinanceFixExecutionReportParser.Parse(message);

        Assert.Collection(
            report.MiscellaneousFees,
            fee => Assert.Equal((0.01m, "USDT"), (fee.Amount, fee.Currency)),
            fee => Assert.Equal((0.02m, "BNB"), (fee.Amount, fee.Currency)));
    }

    [Fact]
    public void RejectsDictionarylessFeeCountMismatch()
    {
        var message = ParseWithoutDictionary(CreateDictionarylessWireMessage(
            "136=2",
            "137=0.01",
            "138=USDT",
            "139=4"));

        Assert.Throws<FormatException>(() => BinanceFixExecutionReportParser.Parse(message));
    }

    [Fact]
    public void RejectsDictionarylessRepeatedNonFeeField()
    {
        var message = ParseWithoutDictionary(CreateDictionarylessWireMessage("55=ETHUSDT"));

        Assert.Throws<FormatException>(() => BinanceFixExecutionReportParser.Parse(message));
    }

    [Theory]
    [InlineData(Tags.OrdType)]
    [InlineData(Tags.Side)]
    [InlineData(Tags.Symbol)]
    [InlineData(Tags.ExecType)]
    [InlineData(Tags.CumQty)]
    [InlineData(Tags.LastQty)]
    [InlineData(Tags.OrdStatus)]
    public void RejectsMissingRequiredBodyField(int tag)
    {
        var message = CreateRequiredReport();
        message.RemoveField(tag);

        Assert.Throws<FormatException>(() => BinanceFixExecutionReportParser.Parse(message));
    }

    [Fact]
    public void RejectsWrongMessageType()
    {
        var message = CreateRequiredReport();
        message.Header.SetField(new MsgType(MsgType.ORDER_SINGLE));

        Assert.Throws<FormatException>(() => BinanceFixExecutionReportParser.Parse(message));
    }

    [Fact]
    public void ParsesCurrentUtf8SymbolFields()
    {
        const string symbol = "这是测试币456";
        var message = CreateRequiredReport();
        Set(message, Tags.Symbol, symbol);
        Set(message, BinanceFixExecutionReportParser.CounterSymbolTag, symbol);
        AddFee(message, "0.01", symbol);

        var report = BinanceFixExecutionReportParser.Parse(message);

        Assert.Equal(symbol, report.Symbol);
        Assert.Equal(symbol, report.CounterSymbol);
        Assert.Equal(symbol, Assert.Single(report.MiscellaneousFees).Currency);
    }

    [Theory]
    [InlineData("0", BinanceFixExecutionType.New)]
    [InlineData("4", BinanceFixExecutionType.Canceled)]
    [InlineData("5", BinanceFixExecutionType.Replaced)]
    [InlineData("8", BinanceFixExecutionType.Rejected)]
    [InlineData("F", BinanceFixExecutionType.Trade)]
    [InlineData("C", BinanceFixExecutionType.Expired)]
    public void ParsesEveryExecutionType(string wireValue, BinanceFixExecutionType expected)
    {
        var report = BinanceFixExecutionReportParser.Parse(CreateRequiredReport(wireValue));

        Assert.Equal(expected, report.ExecutionType);
    }

    [Theory]
    [InlineData("0", BinanceFixOrderStatus.New)]
    [InlineData("1", BinanceFixOrderStatus.PartiallyFilled)]
    [InlineData("2", BinanceFixOrderStatus.Filled)]
    [InlineData("4", BinanceFixOrderStatus.Canceled)]
    [InlineData("6", BinanceFixOrderStatus.PendingCancel)]
    [InlineData("8", BinanceFixOrderStatus.Rejected)]
    [InlineData("A", BinanceFixOrderStatus.PendingNew)]
    [InlineData("C", BinanceFixOrderStatus.Expired)]
    public void ParsesEveryOrderStatus(string wireValue, BinanceFixOrderStatus expected)
    {
        var report = BinanceFixExecutionReportParser.Parse(CreateRequiredReport(orderStatus: wireValue));

        Assert.Equal(expected, report.OrderStatus);
    }

    [Theory]
    [InlineData("1", BinanceFixOrdType.Market)]
    [InlineData("2", BinanceFixOrdType.Limit)]
    [InlineData("3", BinanceFixOrdType.Stop)]
    [InlineData("4", BinanceFixOrdType.StopLimit)]
    [InlineData("P", BinanceFixOrdType.Pegged)]
    public void ParsesEveryRawOrderType(string wireValue, BinanceFixOrdType expected)
    {
        var message = CreateRequiredReport();
        Set(message, Tags.OrdType, wireValue);

        var report = BinanceFixExecutionReportParser.Parse(message);

        Assert.Equal(expected, report.OrderType);
    }

    [Theory]
    [InlineData("1", BinanceFixExpiryReason.Rejected)]
    [InlineData("2", BinanceFixExpiryReason.ExchangeCanceled)]
    [InlineData("3", BinanceFixExpiryReason.OcoTrigger)]
    [InlineData("4", BinanceFixExpiryReason.OtoPhaseOneExpired)]
    [InlineData("5", BinanceFixExpiryReason.UnfilledImmediateOrCancelQuantityExpired)]
    [InlineData("6", BinanceFixExpiryReason.UnfilledFillOrKillOrderExpired)]
    [InlineData("7", BinanceFixExpiryReason.InsufficientLiquidity)]
    [InlineData("8", BinanceFixExpiryReason.ExecutionRulePriceRangeExceeded)]
    public void ParsesEveryDictionaryExpiryReason(string wireValue, BinanceFixExpiryReason expected)
    {
        var message = CreateRequiredReport();
        Set(message, BinanceFixExecutionReportParser.ExpiryReasonTag, wireValue);

        var report = BinanceFixExecutionReportParser.Parse(message);

        Assert.Equal(expected, report.ExpiryReason);
    }

    [Theory]
    [InlineData(BinanceFixExecutionReportParser.OrderCreationTimeTag, "1723550400000")]
    [InlineData(Tags.CumQty, "1e2")]
    [InlineData(Tags.LastQty, "-1")]
    [InlineData(Tags.ExecType, "1")]
    [InlineData(Tags.OrdStatus, "3")]
    [InlineData(BinanceFixExecutionReportParser.TargetStrategyTag, "999999")]
    [InlineData(BinanceFixExecutionReportParser.StrategyIdTag, "9223372036854775808")]
    [InlineData(BinanceFixExecutionReportParser.SmartOrderRoutingTag, "true")]
    [InlineData(BinanceFixExecutionReportParser.NumberOfMiscellaneousFeesTag, "18446744073709551615")]
    public void RejectsInvalidPublishedFieldDomain(int tag, string value)
    {
        var message = CreateRequiredReport();
        Set(message, tag, value);

        Assert.Throws<FormatException>(() => BinanceFixExecutionReportParser.Parse(message));
    }

    [Fact]
    public void RejectsInvalidClientOrderId()
    {
        var message = CreateRequiredReport(clientOrderId: "invalid.id");

        Assert.Throws<FormatException>(() => BinanceFixExecutionReportParser.Parse(message));
    }

    [Fact]
    public void ReconcilesOnlyUnknownDeliveryWithExactOrdinalClientOrderId()
    {
        var request = CreateNewOrderRequest("client_1");
        var matching = BinanceFixExecutionReportParser.Parse(CreateRequiredReport(clientOrderId: "client_1"));
        var differentCase = BinanceFixExecutionReportParser.Parse(CreateRequiredReport(clientOrderId: "CLIENT_1"));
        var missingId = BinanceFixExecutionReportParser.Parse(CreateRequiredReport(clientOrderId: null));

        Assert.Equal(
            BinanceFixNewOrderReconciliationStatus.ExchangeProcessed,
            matching.ReconcileNewOrder(request, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Equal(
            BinanceFixNewOrderReconciliationStatus.Unresolved,
            matching.ReconcileNewOrder(request, BinanceFixDeliveryStatus.NotSent));
        Assert.Equal(
            BinanceFixNewOrderReconciliationStatus.Unresolved,
            differentCase.ReconcileNewOrder(request, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Equal(
            BinanceFixNewOrderReconciliationStatus.Unresolved,
            missingId.ReconcileNewOrder(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Theory]
    [InlineData("8", "8")]
    [InlineData("8", "0")]
    [InlineData("0", "8")]
    public void AnyCorrelatedRejectionSignalResolvesAsExchangeRejected(
        string executionType,
        string orderStatus)
    {
        var request = CreateNewOrderRequest("client_1");
        var report = BinanceFixExecutionReportParser.Parse(
            CreateRequiredReport(executionType, orderStatus, "client_1"));

        Assert.Equal(
            BinanceFixNewOrderReconciliationStatus.ExchangeRejected,
            report.ReconcileNewOrder(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void ReconciliationRejectsNullRequest()
    {
        var report = BinanceFixExecutionReportParser.Parse(CreateRequiredReport());

        Assert.Throws<ArgumentNullException>(() =>
            report.ReconcileNewOrder(null!, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void ParserRejectsNullMessage()
    {
        Assert.Throws<ArgumentNullException>(() => BinanceFixExecutionReportParser.Parse(null!));
    }

    private static Message CreateRequiredReport(
        string executionType = "0",
        string orderStatus = "0",
        string? clientOrderId = "client_1")
    {
        var message = new Message();
        message.Header.SetField(new MsgType(MsgType.EXECUTION_REPORT));
        if (clientOrderId is not null)
        {
            Set(message, Tags.ClOrdID, clientOrderId);
        }

        Set(message, Tags.OrdType, "2");
        Set(message, Tags.Side, "1");
        Set(message, Tags.Symbol, "BTCUSDT");
        Set(message, Tags.ExecType, executionType);
        Set(message, Tags.CumQty, "0");
        Set(message, Tags.LastQty, "0");
        Set(message, Tags.OrdStatus, orderStatus);
        return message;
    }

    private static BinanceFixNewOrderRequest CreateNewOrderRequest(string clientOrderId)
        => new(
            clientOrderId,
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1);

    private static void AddFee(Message message, string amount, string currency)
    {
        var group = new Group(
            BinanceFixExecutionReportParser.NumberOfMiscellaneousFeesTag,
            BinanceFixExecutionReportParser.MiscellaneousFeeAmountTag,
            [
                BinanceFixExecutionReportParser.MiscellaneousFeeAmountTag,
                BinanceFixExecutionReportParser.MiscellaneousFeeCurrencyTag,
                BinanceFixExecutionReportParser.MiscellaneousFeeTypeTag,
                0
            ]);
        Set(group, BinanceFixExecutionReportParser.MiscellaneousFeeAmountTag, amount);
        Set(group, BinanceFixExecutionReportParser.MiscellaneousFeeCurrencyTag, currency);
        Set(group, BinanceFixExecutionReportParser.MiscellaneousFeeTypeTag, "4");
        message.AddGroup(group);
    }

    private static string CreateDictionarylessWireMessage(params string[] additionalFields)
    {
        var bodyFields = new List<string>
        {
            "35=8",
            "40=2",
            "54=1",
            "55=BTCUSDT",
            "150=0",
            "14=0",
            "32=0",
            "39=0"
        };
        bodyFields.AddRange(additionalFields);
        var body = string.Join(Message.SOH, bodyFields) + Message.SOH;
        var messageWithoutChecksum = $"8=FIX.4.4{Message.SOH}9={Encoding.ASCII.GetByteCount(body)}{Message.SOH}{body}";
        var checksum = Encoding.ASCII.GetBytes(messageWithoutChecksum).Sum(value => value) % 256;
        return $"{messageWithoutChecksum}10={checksum:000}{Message.SOH}";
    }

    private static Message ParseWithoutDictionary(string wireMessage)
    {
        var message = new Message();
        message.FromString(
            wireMessage,
            validate: true,
            transportDict: null,
            appDict: null,
            msgFactory: null,
            ignoreBody: false);
        return message;
    }

    private static DateTimeOffset ParseTimestamp(string value)
        => DateTimeOffset.ParseExact(
            value,
            ["yyyyMMdd-HH:mm:ss", "yyyyMMdd-HH:mm:ss.fff", "yyyyMMdd-HH:mm:ss.ffffff"],
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

    private static void Set(FieldMap fields, int tag, string value)
        => fields.SetField(new StringField(tag, value));
}
