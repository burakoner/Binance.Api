using System.Globalization;
using System.Text;
using QuickFix;
using QuickFix.Fields;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class OrderAmendTests
{
    [Fact]
    public void MapsCompleteAmendWithInvariantQuantityAndSigned64Id()
    {
        var previousCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("tr-TR");
            var request = new BinanceFixOrderAmendRequest(
                "amended_1",
                "BTCUSDT",
                1.2500m,
                long.MinValue,
                "original-1");

            var message = BinanceFixOrderAmendMapper.CreateMessage(request);

            Assert.Equal(BinanceFixOrderAmendMapper.MessageType, message.Header.GetString(Tags.MsgType));
            Assert.Equal("amended_1", message.GetString(Tags.ClOrdID));
            Assert.Equal("BTCUSDT", message.GetString(Tags.Symbol));
            Assert.Equal("1.2500", message.GetString(Tags.OrderQty));
            Assert.Equal(long.MinValue.ToString(CultureInfo.InvariantCulture), message.GetString(Tags.OrderID));
            Assert.Equal("original-1", message.GetString(Tags.OrigClOrdID));
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }
    }

    [Fact]
    public void AcceptsEitherTargetIdentifierAndUnchangedClientOrderId()
    {
        var byOrderId = new BinanceFixOrderAmendRequest("a1", "BTCUSDT", 1m, orderId: 1);
        var unchangedClientId = new BinanceFixOrderAmendRequest(
            "same_id",
            "BTCUSDT",
            1m,
            originalClientOrderId: "same_id");

        Assert.Equal(1, byOrderId.OrderId);
        Assert.Null(byOrderId.OriginalClientOrderId);
        Assert.Null(unchangedClientId.OrderId);
        Assert.Equal("same_id", unchangedClientId.OriginalClientOrderId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("with space")]
    [InlineData("client!")]
    [InlineData("1234567890123456789012345678901234567")]
    public void RejectsInvalidClientIds(string clientId)
    {
        Assert.Throws<ArgumentException>(() =>
            new BinanceFixOrderAmendRequest(clientId, "BTCUSDT", 1m, orderId: 1));
        Assert.Throws<ArgumentException>(() =>
            new BinanceFixOrderAmendRequest(
                "amended_1",
                "BTCUSDT",
                1m,
                originalClientOrderId: clientId));
    }

    [Theory]
    [InlineData("")]
    [InlineData("BTC\nUSDT")]
    public void RejectsInvalidSymbols(string symbol)
    {
        Assert.Throws<ArgumentException>(() =>
            new BinanceFixOrderAmendRequest("amended_1", symbol, 1m, orderId: 1));
    }

    [Fact]
    public void AcceptsCurrentUtf8SymbolOnRequestAndReject()
    {
        const string symbol = "这是测试币456";
        var request = new BinanceFixOrderAmendRequest("amended_1", symbol, 1m, orderId: 1);
        var message = CreateRequiredReject();
        Set(message, Tags.Symbol, symbol);

        Assert.Equal(symbol, request.Symbol);
        Assert.Equal(symbol, BinanceFixOrderAmendRejectParser.Parse(message).Symbol);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-0.1")]
    public void RejectsNonPositiveRequestQuantity(string value)
    {
        var quantity = decimal.Parse(value, CultureInfo.InvariantCulture);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BinanceFixOrderAmendRequest("amended_1", "BTCUSDT", quantity, orderId: 1));
    }

    [Fact]
    public void RejectsMissingTargetAndNullMapperRequest()
    {
        Assert.Throws<ArgumentException>(() =>
            new BinanceFixOrderAmendRequest("amended_1", "BTCUSDT", 1m));
        Assert.Throws<ArgumentNullException>(() => BinanceFixOrderAmendMapper.CreateMessage(null!));
    }

    [Fact]
    public void ParsesCompleteCurrentAmendRejectSurface()
    {
        var message = CreateRequiredReject();
        Set(message, Tags.OrigClOrdID, "original-1");
        Set(message, Tags.OrderID, long.MaxValue.ToString(CultureInfo.InvariantCulture));
        Set(
            message,
            BinanceFixOrderAmendRejectParser.ErrorCodeTag,
            long.MinValue.ToString(CultureInfo.InvariantCulture));

        var reject = BinanceFixOrderAmendRejectParser.Parse(message);

        Assert.Equal("amended_1", reject.ClientOrderId);
        Assert.Equal("original-1", reject.OriginalClientOrderId);
        Assert.Equal(long.MaxValue, reject.OrderId);
        Assert.Equal("BTCUSDT", reject.Symbol);
        Assert.Equal(1.25m, reject.NewQuantity);
        Assert.Equal(long.MinValue, reject.ErrorCode);
        Assert.Equal("Order amend failed.", reject.ErrorText);
    }

    [Theory]
    [InlineData(Tags.ClOrdID)]
    [InlineData(Tags.Symbol)]
    [InlineData(Tags.OrderQty)]
    [InlineData(BinanceFixOrderAmendRejectParser.ErrorCodeTag)]
    [InlineData(Tags.Text)]
    public void RejectsMissingRequiredAmendRejectField(int tag)
    {
        var message = CreateRequiredReject();
        message.RemoveField(tag);

        Assert.Throws<FormatException>(() => BinanceFixOrderAmendRejectParser.Parse(message));
    }

    [Fact]
    public void RejectsWrongAmendRejectMessageType()
    {
        var message = CreateRequiredReject();
        message.Header.SetField(new MsgType(MsgType.ORDER_CANCEL_REJECT));

        Assert.Throws<FormatException>(() => BinanceFixOrderAmendRejectParser.Parse(message));
    }

    [Theory]
    [InlineData(Tags.ClOrdID, "invalid id")]
    [InlineData(Tags.OrigClOrdID, "invalid!")]
    [InlineData(Tags.OrderID, "9223372036854775808")]
    [InlineData(Tags.OrderQty, "0")]
    [InlineData(Tags.OrderQty, "-1")]
    [InlineData(Tags.OrderQty, "1e2")]
    [InlineData(BinanceFixOrderAmendRejectParser.ErrorCodeTag, "-9223372036854775809")]
    [InlineData(Tags.Text, "")]
    public void RejectsInvalidAmendRejectFieldDomain(int tag, string value)
    {
        var message = CreateRequiredReject();
        Set(message, tag, value);

        Assert.Throws<FormatException>(() => BinanceFixOrderAmendRejectParser.Parse(message));
    }

    [Fact]
    public void RejectsDictionarylessDuplicateAmendRejectField()
    {
        var message = ParseWithoutDictionary(CreateDictionarylessRejectWire("55=ETHUSDT"));

        Assert.Throws<FormatException>(() => BinanceFixOrderAmendRejectParser.Parse(message));
    }

    [Fact]
    public void AmendRejectReconcilesOnlyExactUnknownDeliveryRequest()
    {
        var request = new BinanceFixOrderAmendRequest(
            "amended_1",
            "BTCUSDT",
            1.25m,
            orderId: 42,
            originalClientOrderId: "original-1");
        var message = CreateRequiredReject();
        Set(message, Tags.OrderID, "42");
        Set(message, Tags.OrigClOrdID, "original-1");
        var reject = BinanceFixOrderAmendRejectParser.Parse(message);

        Assert.Equal(
            BinanceFixOrderAmendReconciliationStatus.ExchangeRejected,
            reject.ReconcileAmend(request, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Equal(
            BinanceFixOrderAmendReconciliationStatus.Unresolved,
            reject.ReconcileAmend(request, BinanceFixDeliveryStatus.NotSent));
    }

    [Theory]
    [InlineData(Tags.ClOrdID, "AMENDED_1")]
    [InlineData(Tags.Symbol, "ETHUSDT")]
    [InlineData(Tags.OrderQty, "1.24")]
    [InlineData(Tags.OrderID, "43")]
    [InlineData(Tags.OrigClOrdID, "original-2")]
    public void AmendRejectDoesNotReconcileMismatchedCorrelation(int tag, string value)
    {
        var request = new BinanceFixOrderAmendRequest(
            "amended_1",
            "BTCUSDT",
            1.25m,
            orderId: 42,
            originalClientOrderId: "original-1");
        var message = CreateRequiredReject();
        Set(message, Tags.OrderID, "42");
        Set(message, Tags.OrigClOrdID, "original-1");
        Set(message, tag, value);
        var reject = BinanceFixOrderAmendRejectParser.Parse(message);

        Assert.Equal(
            BinanceFixOrderAmendReconciliationStatus.Unresolved,
            reject.ReconcileAmend(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    public void ReplacedExecutionReportReconcilesAsObservedForCurrentOpenStates(string orderStatus)
    {
        var request = new BinanceFixOrderAmendRequest(
            "amended_1",
            "BTCUSDT",
            1.25m,
            orderId: 42,
            originalClientOrderId: "original-1");
        var report = BinanceFixExecutionReportParser.Parse(CreateReplacedExecutionReport(orderStatus));

        Assert.Equal(
            BinanceFixOrderAmendReconciliationStatus.ExchangeReplacementObserved,
            report.ReconcileAmend(request, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Equal(
            BinanceFixOrderAmendReconciliationStatus.Unresolved,
            report.ReconcileAmend(request, BinanceFixDeliveryStatus.NotSent));
    }

    [Fact]
    public void ExecutionReportMaySupplyOrderIdWhenRequestUsedOriginalClientId()
    {
        var request = new BinanceFixOrderAmendRequest(
            "amended_1",
            "BTCUSDT",
            1.25m,
            originalClientOrderId: "original-1");
        var report = BinanceFixExecutionReportParser.Parse(CreateReplacedExecutionReport());

        Assert.Equal(
            BinanceFixOrderAmendReconciliationStatus.ExchangeReplacementObserved,
            report.ReconcileAmend(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Theory]
    [InlineData(Tags.ClOrdID, "AMENDED_1")]
    [InlineData(Tags.Symbol, "ETHUSDT")]
    [InlineData(Tags.OrderQty, "1.24")]
    [InlineData(Tags.OrderID, "43")]
    [InlineData(Tags.OrigClOrdID, "original-2")]
    [InlineData(Tags.ExecType, "0")]
    public void ExecutionReportDoesNotReconcileMismatchedAmendCorrelation(int tag, string value)
    {
        var request = new BinanceFixOrderAmendRequest(
            "amended_1",
            "BTCUSDT",
            1.25m,
            orderId: 42,
            originalClientOrderId: "original-1");
        var message = CreateReplacedExecutionReport();
        Set(message, tag, value);
        var report = BinanceFixExecutionReportParser.Parse(message);

        Assert.Equal(
            BinanceFixOrderAmendReconciliationStatus.Unresolved,
            report.ReconcileAmend(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void ExecutionReportRequiresAmendedQuantityForReconciliation()
    {
        var request = new BinanceFixOrderAmendRequest("amended_1", "BTCUSDT", 1.25m, orderId: 42);
        var message = CreateReplacedExecutionReport();
        message.RemoveField(Tags.OrderQty);
        var report = BinanceFixExecutionReportParser.Parse(message);

        Assert.Equal(
            BinanceFixOrderAmendReconciliationStatus.Unresolved,
            report.ReconcileAmend(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void ReconciliationAndParserRejectNulls()
    {
        var reject = BinanceFixOrderAmendRejectParser.Parse(CreateRequiredReject());
        var report = BinanceFixExecutionReportParser.Parse(CreateReplacedExecutionReport());

        Assert.Throws<ArgumentNullException>(() =>
            reject.ReconcileAmend(null!, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Throws<ArgumentNullException>(() =>
            report.ReconcileAmend(null!, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Throws<ArgumentNullException>(() => BinanceFixOrderAmendRejectParser.Parse(null!));
    }

    private static Message CreateRequiredReject()
    {
        var message = new Message();
        message.Header.SetField(new MsgType(BinanceFixOrderAmendRejectParser.MessageType));
        Set(message, Tags.ClOrdID, "amended_1");
        Set(message, Tags.Symbol, "BTCUSDT");
        Set(message, Tags.OrderQty, "1.25");
        Set(message, BinanceFixOrderAmendRejectParser.ErrorCodeTag, "-1013");
        Set(message, Tags.Text, "Order amend failed.");
        return message;
    }

    private static Message CreateReplacedExecutionReport(string orderStatus = "0")
    {
        var message = new Message();
        message.Header.SetField(new MsgType(MsgType.EXECUTION_REPORT));
        Set(message, Tags.ClOrdID, "amended_1");
        Set(message, Tags.OrigClOrdID, "original-1");
        Set(message, Tags.OrderID, "42");
        Set(message, Tags.OrderQty, "1.25");
        Set(message, Tags.OrdType, "2");
        Set(message, Tags.Side, "1");
        Set(message, Tags.Symbol, "BTCUSDT");
        Set(message, Tags.ExecType, "5");
        Set(message, Tags.CumQty, "0");
        Set(message, Tags.LastQty, "0");
        Set(message, Tags.OrdStatus, orderStatus);
        return message;
    }

    private static string CreateDictionarylessRejectWire(params string[] additionalFields)
    {
        var bodyFields = new List<string>
        {
            "35=XAR",
            "11=amended_1",
            "55=BTCUSDT",
            "38=1.25",
            "25016=-1013",
            "58=Order amend failed."
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
