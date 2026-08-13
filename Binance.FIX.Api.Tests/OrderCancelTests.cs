using System.Globalization;
using System.Text;
using QuickFix;
using QuickFix.Fields;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class OrderCancelTests
{
    [Fact]
    public void MapsCompleteOrderTargetWithSigned64Id()
    {
        var request = BinanceFixOrderCancelRequest.ForOrder(
            "cancel_1",
            "BTCUSDT",
            long.MinValue,
            "original-1",
            BinanceFixCancelRestriction.OnlyPartiallyFilled);

        var message = BinanceFixOrderCancelMapper.CreateMessage(request);

        Assert.Equal(BinanceFixOrderCancelTarget.Order, request.Target);
        Assert.Equal(MsgType.ORDER_CANCEL_REQUEST, message.Header.GetString(Tags.MsgType));
        Assert.Equal("cancel_1", message.GetString(Tags.ClOrdID));
        Assert.Equal("BTCUSDT", message.GetString(Tags.Symbol));
        Assert.Equal(long.MinValue.ToString(CultureInfo.InvariantCulture), message.GetString(Tags.OrderID));
        Assert.Equal("original-1", message.GetString(Tags.OrigClOrdID));
        Assert.Equal("2", message.GetString(BinanceFixOrderCancelMapper.CancelRestrictionTag));
        Assert.False(message.IsSetField(Tags.ListID));
        Assert.False(message.IsSetField(BinanceFixOrderCancelMapper.OriginalClientListIdTag));
    }

    [Fact]
    public void MapsCompleteOrderListTargetAndPreservesOpaqueListId()
    {
        var request = BinanceFixOrderCancelRequest.ForOrderList(
            "cancel_2",
            "ETHUSDT",
            "list-A",
            "original_list-1",
            BinanceFixCancelRestriction.OnlyNew);

        var message = BinanceFixOrderCancelMapper.CreateMessage(request);

        Assert.Equal(BinanceFixOrderCancelTarget.OrderList, request.Target);
        Assert.Equal("list-A", message.GetString(Tags.ListID));
        Assert.Equal(
            "original_list-1",
            message.GetString(BinanceFixOrderCancelMapper.OriginalClientListIdTag));
        Assert.Equal("1", message.GetString(BinanceFixOrderCancelMapper.CancelRestrictionTag));
        Assert.False(message.IsSetField(Tags.OrderID));
        Assert.False(message.IsSetField(Tags.OrigClOrdID));
    }

    [Fact]
    public void AcceptsEitherPublishedIdentifierWithinEachTargetFamily()
    {
        var byOrderId = BinanceFixOrderCancelRequest.ForOrder("c1", "BTCUSDT", orderId: 1);
        var byOriginalOrderId = BinanceFixOrderCancelRequest.ForOrder(
            "c2",
            "BTCUSDT",
            originalClientOrderId: "o1");
        var byListId = BinanceFixOrderCancelRequest.ForOrderList("c3", "BTCUSDT", listId: "10");
        var byOriginalListId = BinanceFixOrderCancelRequest.ForOrderList(
            "c4",
            "BTCUSDT",
            originalClientListId: "l1");

        Assert.Equal(1, byOrderId.OrderId);
        Assert.Equal("o1", byOriginalOrderId.OriginalClientOrderId);
        Assert.Equal("10", byListId.ListId);
        Assert.Equal("l1", byOriginalListId.OriginalClientListId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("with space")]
    [InlineData("client!")]
    [InlineData("1234567890123456789012345678901234567")]
    public void RejectsInvalidRequestClientIds(string clientId)
    {
        Assert.Throws<ArgumentException>(() =>
            BinanceFixOrderCancelRequest.ForOrder(clientId, "BTCUSDT", orderId: 1));
        Assert.Throws<ArgumentException>(() =>
            BinanceFixOrderCancelRequest.ForOrder(
                "cancel_1",
                "BTCUSDT",
                originalClientOrderId: clientId));
        Assert.Throws<ArgumentException>(() =>
            BinanceFixOrderCancelRequest.ForOrderList(
                "cancel_1",
                "BTCUSDT",
                originalClientListId: clientId));
    }

    [Theory]
    [InlineData("")]
    [InlineData("BTC\nUSDT")]
    public void RejectsInvalidPrintableRequestStrings(string value)
    {
        Assert.Throws<ArgumentException>(() =>
            BinanceFixOrderCancelRequest.ForOrder("cancel_1", value, orderId: 1));
        Assert.Throws<ArgumentException>(() =>
            BinanceFixOrderCancelRequest.ForOrderList("cancel_1", "BTCUSDT", listId: value));
    }

    [Fact]
    public void AcceptsCurrentUtf8SymbolOnRequestAndReject()
    {
        const string symbol = "这是测试币456";
        var request = BinanceFixOrderCancelRequest.ForOrder("cancel_1", symbol, orderId: 1);
        var message = CreateRequiredReject();
        Set(message, Tags.Symbol, symbol);

        Assert.Equal(symbol, request.Symbol);
        Assert.Equal(symbol, BinanceFixOrderCancelRejectParser.Parse(message).Symbol);
    }

    [Fact]
    public void RejectsMissingTargetIdentifierAndUndefinedRestriction()
    {
        Assert.Throws<ArgumentException>(() =>
            BinanceFixOrderCancelRequest.ForOrder("cancel_1", "BTCUSDT"));
        Assert.Throws<ArgumentException>(() =>
            BinanceFixOrderCancelRequest.ForOrderList("cancel_1", "BTCUSDT"));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceFixOrderCancelRequest.ForOrder(
                "cancel_1",
                "BTCUSDT",
                orderId: 1,
                cancelRestriction: (BinanceFixCancelRestriction)3));
    }

    [Fact]
    public void MapperRejectsNullRequest()
    {
        Assert.Throws<ArgumentNullException>(() => BinanceFixOrderCancelMapper.CreateMessage(null!));
    }

    [Fact]
    public void ParsesCompleteCurrentCancelRejectSurface()
    {
        var message = CreateRequiredReject();
        Set(message, Tags.OrigClOrdID, "original-1");
        Set(message, Tags.OrderID, long.MaxValue.ToString(CultureInfo.InvariantCulture));
        Set(message, BinanceFixOrderCancelRejectParser.OriginalClientListIdTag, "original_list-1");
        Set(message, Tags.ListID, "list-A");
        Set(message, BinanceFixOrderCancelRejectParser.CancelRestrictionTag, "2");
        Set(message, BinanceFixOrderCancelRejectParser.ErrorCodeTag, long.MinValue.ToString(CultureInfo.InvariantCulture));

        var reject = BinanceFixOrderCancelRejectParser.Parse(message);

        Assert.Equal("cancel_1", reject.ClientOrderId);
        Assert.Equal("original-1", reject.OriginalClientOrderId);
        Assert.Equal(long.MaxValue, reject.OrderId);
        Assert.Equal("original_list-1", reject.OriginalClientListId);
        Assert.Equal("list-A", reject.ListId);
        Assert.Equal("BTCUSDT", reject.Symbol);
        Assert.Equal(BinanceFixCancelRestriction.OnlyPartiallyFilled, reject.CancelRestriction);
        Assert.Equal(BinanceFixCancelRejectResponseTo.OrderCancelRequest, reject.ResponseTo);
        Assert.Equal(long.MinValue, reject.ErrorCode);
        Assert.Equal("Unknown order sent.", reject.ErrorText);
    }

    [Theory]
    [InlineData(Tags.ClOrdID)]
    [InlineData(Tags.Symbol)]
    [InlineData(BinanceFixOrderCancelRejectParser.CancelRejectResponseToTag)]
    [InlineData(BinanceFixOrderCancelRejectParser.ErrorCodeTag)]
    [InlineData(Tags.Text)]
    public void RejectsMissingRequiredCancelRejectField(int tag)
    {
        var message = CreateRequiredReject();
        message.RemoveField(tag);

        Assert.Throws<FormatException>(() => BinanceFixOrderCancelRejectParser.Parse(message));
    }

    [Fact]
    public void RejectsWrongCancelRejectMessageType()
    {
        var message = CreateRequiredReject();
        message.Header.SetField(new MsgType(MsgType.EXECUTION_REPORT));

        Assert.Throws<FormatException>(() => BinanceFixOrderCancelRejectParser.Parse(message));
    }

    [Theory]
    [InlineData(BinanceFixOrderCancelRejectParser.CancelRestrictionTag, "3")]
    [InlineData(BinanceFixOrderCancelRejectParser.CancelRejectResponseToTag, "2")]
    [InlineData(BinanceFixOrderCancelRejectParser.ErrorCodeTag, "9223372036854775808")]
    [InlineData(Tags.OrderID, "-9223372036854775809")]
    [InlineData(Tags.ClOrdID, "invalid id")]
    [InlineData(Tags.OrigClOrdID, "invalid!")]
    [InlineData(BinanceFixOrderCancelRejectParser.OriginalClientListIdTag, "invalid!")]
    [InlineData(Tags.ListID, "")]
    [InlineData(Tags.Text, "")]
    public void RejectsInvalidCancelRejectFieldDomain(int tag, string value)
    {
        var message = CreateRequiredReject();
        Set(message, tag, value);

        Assert.Throws<FormatException>(() => BinanceFixOrderCancelRejectParser.Parse(message));
    }

    [Fact]
    public void RejectsDictionarylessDuplicateCancelRejectField()
    {
        var message = ParseWithoutDictionary(CreateDictionarylessRejectWire("55=ETHUSDT"));

        Assert.Throws<FormatException>(() => BinanceFixOrderCancelRejectParser.Parse(message));
    }

    [Fact]
    public void CancelRejectReconcilesOnlyExactUnknownDeliveryRequest()
    {
        var request = BinanceFixOrderCancelRequest.ForOrder(
            "cancel_1",
            "BTCUSDT",
            orderId: 42,
            originalClientOrderId: "original-1",
            cancelRestriction: BinanceFixCancelRestriction.OnlyNew);
        var message = CreateRequiredReject();
        Set(message, Tags.OrderID, "42");
        Set(message, Tags.OrigClOrdID, "original-1");
        Set(message, BinanceFixOrderCancelRejectParser.CancelRestrictionTag, "1");
        var reject = BinanceFixOrderCancelRejectParser.Parse(message);

        Assert.Equal(
            BinanceFixCancelReconciliationStatus.ExchangeRejected,
            reject.ReconcileCancel(request, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Equal(
            BinanceFixCancelReconciliationStatus.Unresolved,
            reject.ReconcileCancel(request, BinanceFixDeliveryStatus.NotSent));

        Set(message, Tags.OrderID, "43");
        var wrongTarget = BinanceFixOrderCancelRejectParser.Parse(message);
        Assert.Equal(
            BinanceFixCancelReconciliationStatus.Unresolved,
            wrongTarget.ReconcileCancel(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void CancelRejectRejectsCrossTargetAndCaseInsensitiveCorrelation()
    {
        var request = BinanceFixOrderCancelRequest.ForOrderList(
            "cancel_1",
            "BTCUSDT",
            listId: "list-A");
        var message = CreateRequiredReject(clientOrderId: "CANCEL_1");
        Set(message, Tags.ListID, "list-A");
        var differentCase = BinanceFixOrderCancelRejectParser.Parse(message);
        Assert.Equal(
            BinanceFixCancelReconciliationStatus.Unresolved,
            differentCase.ReconcileCancel(request, BinanceFixDeliveryStatus.UnknownDelivery));

        message = CreateRequiredReject();
        Set(message, Tags.OrderID, "42");
        var crossTarget = BinanceFixOrderCancelRejectParser.Parse(message);
        Assert.Equal(
            BinanceFixCancelReconciliationStatus.Unresolved,
            crossTarget.ReconcileCancel(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void CanceledExecutionReportReconcilesAsObservedNotListComplete()
    {
        var request = BinanceFixOrderCancelRequest.ForOrderList(
            "cancel_1",
            "BTCUSDT",
            originalClientListId: "original_list-1");
        var report = BinanceFixExecutionReportParser.Parse(CreateCancelExecutionReport());

        Assert.Equal(
            BinanceFixCancelReconciliationStatus.ExchangeCancellationObserved,
            report.ReconcileCancel(request, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Equal(
            BinanceFixCancelReconciliationStatus.Unresolved,
            report.ReconcileCancel(request, BinanceFixDeliveryStatus.NotSent));
    }

    [Theory]
    [InlineData("0", "4")]
    [InlineData("4", "0")]
    [InlineData("0", "0")]
    public void ExecutionReportRequiresBothCanceledSignals(string executionType, string orderStatus)
    {
        var request = BinanceFixOrderCancelRequest.ForOrder("cancel_1", "BTCUSDT", orderId: 42);
        var report = BinanceFixExecutionReportParser.Parse(
            CreateCancelExecutionReport(executionType, orderStatus));

        Assert.Equal(
            BinanceFixCancelReconciliationStatus.Unresolved,
            report.ReconcileCancel(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void ExecutionReportRejectsMismatchedCancelTarget()
    {
        var request = BinanceFixOrderCancelRequest.ForOrder("cancel_1", "BTCUSDT", orderId: 42);
        var message = CreateCancelExecutionReport();
        Set(message, Tags.OrderID, "43");
        var report = BinanceFixExecutionReportParser.Parse(message);

        Assert.Equal(
            BinanceFixCancelReconciliationStatus.Unresolved,
            report.ReconcileCancel(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void ReconciliationAndParserRejectNulls()
    {
        var reject = BinanceFixOrderCancelRejectParser.Parse(CreateRequiredReject());
        var report = BinanceFixExecutionReportParser.Parse(CreateCancelExecutionReport());

        Assert.Throws<ArgumentNullException>(() =>
            reject.ReconcileCancel(null!, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Throws<ArgumentNullException>(() =>
            report.ReconcileCancel(null!, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Throws<ArgumentNullException>(() => BinanceFixOrderCancelRejectParser.Parse(null!));
    }

    private static Message CreateRequiredReject(string clientOrderId = "cancel_1")
    {
        var message = new Message();
        message.Header.SetField(new MsgType(MsgType.ORDER_CANCEL_REJECT));
        Set(message, Tags.ClOrdID, clientOrderId);
        Set(message, Tags.Symbol, "BTCUSDT");
        Set(message, BinanceFixOrderCancelRejectParser.CancelRejectResponseToTag, "1");
        Set(message, BinanceFixOrderCancelRejectParser.ErrorCodeTag, "-1013");
        Set(message, Tags.Text, "Unknown order sent.");
        return message;
    }

    private static Message CreateCancelExecutionReport(
        string executionType = "4",
        string orderStatus = "4")
    {
        var message = new Message();
        message.Header.SetField(new MsgType(MsgType.EXECUTION_REPORT));
        Set(message, Tags.ClOrdID, "cancel_1");
        Set(message, Tags.OrdType, "2");
        Set(message, Tags.Side, "1");
        Set(message, Tags.Symbol, "BTCUSDT");
        Set(message, Tags.ExecType, executionType);
        Set(message, Tags.CumQty, "0");
        Set(message, Tags.LastQty, "0");
        Set(message, Tags.OrdStatus, orderStatus);
        return message;
    }

    private static string CreateDictionarylessRejectWire(params string[] additionalFields)
    {
        var bodyFields = new List<string>
        {
            "35=9",
            "11=cancel_1",
            "55=BTCUSDT",
            "434=1",
            "25016=-1013",
            "58=Unknown order sent."
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

    private static void Set(FieldMap fields, int tag, string value)
        => fields.SetField(new StringField(tag, value));
}
