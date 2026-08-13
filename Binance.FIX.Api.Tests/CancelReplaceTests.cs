using System.Globalization;
using QuickFix;
using QuickFix.Fields;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class CancelReplaceTests
{
    [Fact]
    public void MapsCompleteCancelAndNewSpecificSurface()
    {
        var newOrder = new BinanceFixNewOrderRequest(
            "new_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.LimitMaker,
            orderQuantity: 1.25m,
            price: 12.5m,
            timeInForce: BinanceFixTimeInForce.GoodTillCanceled,
            icebergQuantity: 0.25m,
            targetStrategy: long.MaxValue,
            strategyId: long.MinValue,
            selfTradePreventionMode: BinanceFixSelfTradePreventionMode.Transfer);
        var request = new BinanceFixCancelReplaceRequest(
            newOrder,
            BinanceFixCancelReplaceMode.AllowFailure,
            orderId: long.MinValue,
            originalClientOrderId: "original_1",
            cancelClientOrderId: "cancel_1",
            cancelRestriction: BinanceFixCancelRestriction.OnlyPartiallyFilled,
            orderRateLimitExceededMode: BinanceFixOrderRateLimitExceededMode.CancelOnly);

        var message = BinanceFixCancelReplaceMapper.CreateMessage(request);

        Assert.Equal(BinanceFixCancelReplaceMapper.MessageType, message.Header.GetString(Tags.MsgType));
        Assert.Equal("2", message.GetString(BinanceFixCancelReplaceMapper.ModeTag));
        Assert.Equal("2", message.GetString(BinanceFixCancelReplaceMapper.OrderRateLimitExceededModeTag));
        Assert.Equal(long.MinValue.ToString(CultureInfo.InvariantCulture), message.GetString(Tags.OrderID));
        Assert.Equal("cancel_1", message.GetString(BinanceFixCancelReplaceMapper.CancelClientOrderIdTag));
        Assert.Equal("original_1", message.GetString(Tags.OrigClOrdID));
        Assert.Equal("2", message.GetString(BinanceFixCancelReplaceMapper.CancelRestrictionTag));
        Assert.Equal("new_1", message.GetString(Tags.ClOrdID));
        Assert.Equal("BTCUSDT", message.GetString(Tags.Symbol));
        Assert.Equal("2", message.GetString(Tags.OrdType));
        Assert.Equal("6", message.GetString(Tags.ExecInst));
        Assert.Equal("1.25", message.GetString(Tags.OrderQty));
        Assert.Equal("12.5", message.GetString(Tags.Price));
        Assert.Equal("1", message.GetString(Tags.TimeInForce));
        Assert.Equal(long.MaxValue.ToString(CultureInfo.InvariantCulture), message.GetString(BinanceFixNewOrderMapper.TargetStrategyTag));
        Assert.Equal(long.MinValue.ToString(CultureInfo.InvariantCulture), message.GetString(BinanceFixNewOrderMapper.StrategyIdTag));
        Assert.Equal("6", message.GetString(BinanceFixNewOrderMapper.SelfTradePreventionModeTag));
        Assert.False(message.IsSetField(BinanceFixNewOrderMapper.SmartOrderRoutingTag));
    }

    [Theory]
    [InlineData(BinanceFixOrderType.Market, "1")]
    [InlineData(BinanceFixOrderType.Limit, "2")]
    [InlineData(BinanceFixOrderType.LimitMaker, "2")]
    [InlineData(BinanceFixOrderType.StopLoss, "3")]
    [InlineData(BinanceFixOrderType.StopLossLimit, "4")]
    [InlineData(BinanceFixOrderType.TakeProfit, "3")]
    [InlineData(BinanceFixOrderType.TakeProfitLimit, "4")]
    [InlineData(BinanceFixOrderType.Pegged, "P")]
    public void ReusesEveryValidatedNewOrderTypeExceptSor(
        BinanceFixOrderType orderType,
        string expectedOrdType)
    {
        var request = CreateRequest(newOrder: CreateNewOrder(orderType));

        var message = BinanceFixCancelReplaceMapper.CreateMessage(request);

        Assert.Equal(BinanceFixCancelReplaceMapper.MessageType, message.Header.GetString(Tags.MsgType));
        Assert.Equal(expectedOrdType, message.GetString(Tags.OrdType));
    }

    [Theory]
    [InlineData(BinanceFixCancelReplaceMode.StopOnFailure, "1")]
    [InlineData(BinanceFixCancelReplaceMode.AllowFailure, "2")]
    public void MapsEveryCancelFailureMode(BinanceFixCancelReplaceMode mode, string expected)
    {
        var message = BinanceFixCancelReplaceMapper.CreateMessage(CreateRequest(mode: mode));

        Assert.Equal(expected, message.GetString(BinanceFixCancelReplaceMapper.ModeTag));
    }

    [Theory]
    [InlineData(BinanceFixOrderRateLimitExceededMode.DoNothing, "1")]
    [InlineData(BinanceFixOrderRateLimitExceededMode.CancelOnly, "2")]
    public void MapsEveryOrderRateLimitMode(BinanceFixOrderRateLimitExceededMode mode, string expected)
    {
        var message = BinanceFixCancelReplaceMapper.CreateMessage(
            CreateRequest(orderRateLimitExceededMode: mode));

        Assert.Equal(
            expected,
            message.GetString(BinanceFixCancelReplaceMapper.OrderRateLimitExceededModeTag));
    }

    [Fact]
    public void RequiresPublishedCancelTargetAndValidEnums()
    {
        var newOrder = CreateNewOrder();

        Assert.Throws<ArgumentException>(() =>
            new BinanceFixCancelReplaceRequest(newOrder, BinanceFixCancelReplaceMode.StopOnFailure));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BinanceFixCancelReplaceRequest(
                newOrder,
                (BinanceFixCancelReplaceMode)3,
                orderId: 1));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BinanceFixCancelReplaceRequest(
                newOrder,
                BinanceFixCancelReplaceMode.StopOnFailure,
                orderId: 1,
                cancelRestriction: (BinanceFixCancelRestriction)3));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BinanceFixCancelReplaceRequest(
                newOrder,
                BinanceFixCancelReplaceMode.StopOnFailure,
                orderId: 1,
                orderRateLimitExceededMode: (BinanceFixOrderRateLimitExceededMode)3));
    }

    [Theory]
    [InlineData("")]
    [InlineData("with space")]
    [InlineData("client!")]
    [InlineData("1234567890123456789012345678901234567")]
    public void RejectsInvalidCancelPhaseClientIds(string value)
    {
        Assert.Throws<ArgumentException>(() =>
            new BinanceFixCancelReplaceRequest(
                CreateNewOrder(),
                BinanceFixCancelReplaceMode.StopOnFailure,
                originalClientOrderId: value));
        Assert.Throws<ArgumentException>(() =>
            new BinanceFixCancelReplaceRequest(
                CreateNewOrder(),
                BinanceFixCancelReplaceMode.StopOnFailure,
                orderId: 1,
                cancelClientOrderId: value));
    }

    [Fact]
    public void RejectsUnpublishedSor()
    {
        var sorOrder = new BinanceFixNewOrderRequest(
            "new_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1,
            smartOrderRouting: false);
        Assert.Throws<ArgumentException>(() => CreateRequest(newOrder: sorOrder));
    }

    [Fact]
    public void SamePublishedPhaseIdRemainsDeterministicByExecutionOutcome()
    {
        var request = CreateRequest(cancelClientOrderId: "new_1");
        var canceled = BinanceFixExecutionReportParser.Parse(
            CreateExecutionReport("new_1", "4", "4"));
        var accepted = BinanceFixExecutionReportParser.Parse(
            CreateExecutionReport("new_1", "0", "0"));
        var rejected = BinanceFixExecutionReportParser.Parse(
            CreateExecutionReport("new_1", "8", "8"));

        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.CancellationObserved,
            canceled.ReconcileCancelReplace(request, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.NewOrderAccepted,
            accepted.ReconcileCancelReplace(request, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.NewOrderRejected,
            rejected.ReconcileCancelReplace(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void RequestAndMapperRejectNulls()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new BinanceFixCancelReplaceRequest(
                null!,
                BinanceFixCancelReplaceMode.StopOnFailure,
                orderId: 1));
        Assert.Throws<ArgumentNullException>(() => BinanceFixCancelReplaceMapper.CreateMessage(null!));
    }

    [Fact]
    public void ReconcilesExactNewOrderAcceptedPhase()
    {
        var request = CreateRequest();
        var report = BinanceFixExecutionReportParser.Parse(
            CreateExecutionReport("new_1", "0", "0"));

        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.NewOrderAccepted,
            report.ReconcileCancelReplace(request, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.Unresolved,
            report.ReconcileCancelReplace(request, BinanceFixDeliveryStatus.NotSent));
    }

    [Theory]
    [InlineData("8", "8")]
    [InlineData("8", "0")]
    [InlineData("0", "8")]
    public void ReconcilesAnyExactNewOrderRejectionSignal(string executionType, string orderStatus)
    {
        var request = CreateRequest();
        var report = BinanceFixExecutionReportParser.Parse(
            CreateExecutionReport("new_1", executionType, orderStatus));

        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.NewOrderRejected,
            report.ReconcileCancelReplace(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void ReconcilesCanceledPhaseOnlyWithExplicitExactCancelId()
    {
        var request = CreateRequest();
        var message = CreateExecutionReport("cancel_1", "4", "4");
        Set(message, Tags.OrderID, "42");
        Set(message, Tags.OrigClOrdID, "original_1");
        var report = BinanceFixExecutionReportParser.Parse(message);

        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.CancellationObserved,
            report.ReconcileCancelReplace(request, BinanceFixDeliveryStatus.UnknownDelivery));

        var noCancelId = CreateRequest(cancelClientOrderId: null);
        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.Unresolved,
            report.ReconcileCancelReplace(noCancelId, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void RejectsMismatchedCancelReplaceExecutionPhase()
    {
        var request = CreateRequest();
        var wrongCase = BinanceFixExecutionReportParser.Parse(
            CreateExecutionReport("CANCEL_1", "4", "4"));
        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.Unresolved,
            wrongCase.ReconcileCancelReplace(request, BinanceFixDeliveryStatus.UnknownDelivery));

        var wrongSymbol = CreateExecutionReport("cancel_1", "4", "4");
        Set(wrongSymbol, Tags.Symbol, "ETHUSDT");
        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.Unresolved,
            BinanceFixExecutionReportParser.Parse(wrongSymbol)
                .ReconcileCancelReplace(request, BinanceFixDeliveryStatus.UnknownDelivery));

        var wrongTarget = CreateExecutionReport("cancel_1", "4", "4");
        Set(wrongTarget, Tags.OrderID, "43");
        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.Unresolved,
            BinanceFixExecutionReportParser.Parse(wrongTarget)
                .ReconcileCancelReplace(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void CancelRejectReconcilesOnlyExplicitExactCancelPhase()
    {
        var request = CreateRequest();
        var message = CreateCancelReject("cancel_1");
        Set(message, Tags.OrderID, "42");
        Set(message, Tags.OrigClOrdID, "original_1");
        Set(message, BinanceFixOrderCancelRejectParser.CancelRestrictionTag, "1");
        var reject = BinanceFixOrderCancelRejectParser.Parse(message);

        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.CancellationRejected,
            reject.ReconcileCancelReplace(request, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.Unresolved,
            reject.ReconcileCancelReplace(request, BinanceFixDeliveryStatus.NotSent));

        var noCancelId = CreateRequest(cancelClientOrderId: null);
        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.Unresolved,
            reject.ReconcileCancelReplace(noCancelId, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void CancelRejectRejectsMismatchedCancelReplaceEcho()
    {
        var request = CreateRequest();
        var message = CreateCancelReject("cancel_1");
        Set(message, Tags.OrderID, "43");
        var reject = BinanceFixOrderCancelRejectParser.Parse(message);

        Assert.Equal(
            BinanceFixCancelReplaceReconciliationStatus.Unresolved,
            reject.ReconcileCancelReplace(request, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    [Fact]
    public void CancelReplaceReconciliationRejectsNullRequests()
    {
        var report = BinanceFixExecutionReportParser.Parse(
            CreateExecutionReport("new_1", "0", "0"));
        var reject = BinanceFixOrderCancelRejectParser.Parse(CreateCancelReject("cancel_1"));

        Assert.Throws<ArgumentNullException>(() =>
            report.ReconcileCancelReplace(null!, BinanceFixDeliveryStatus.UnknownDelivery));
        Assert.Throws<ArgumentNullException>(() =>
            reject.ReconcileCancelReplace(null!, BinanceFixDeliveryStatus.UnknownDelivery));
    }

    private static BinanceFixCancelReplaceRequest CreateRequest(
        BinanceFixNewOrderRequest? newOrder = null,
        BinanceFixCancelReplaceMode mode = BinanceFixCancelReplaceMode.StopOnFailure,
        string? cancelClientOrderId = "cancel_1",
        BinanceFixOrderRateLimitExceededMode? orderRateLimitExceededMode = null)
        => new(
            newOrder ?? CreateNewOrder(),
            mode,
            orderId: 42,
            originalClientOrderId: "original_1",
            cancelClientOrderId,
            cancelRestriction: BinanceFixCancelRestriction.OnlyNew,
            orderRateLimitExceededMode);

    private static BinanceFixNewOrderRequest CreateNewOrder(
        BinanceFixOrderType orderType = BinanceFixOrderType.Limit)
    {
        var isLimit = orderType is BinanceFixOrderType.Limit
            or BinanceFixOrderType.LimitMaker
            or BinanceFixOrderType.StopLossLimit
            or BinanceFixOrderType.TakeProfitLimit;
        var isContingent = orderType is BinanceFixOrderType.StopLoss
            or BinanceFixOrderType.StopLossLimit
            or BinanceFixOrderType.TakeProfit
            or BinanceFixOrderType.TakeProfitLimit;

        return new BinanceFixNewOrderRequest(
            "new_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            orderType,
            orderQuantity: 1,
            price: isLimit ? 10 : null,
            timeInForce: isLimit && orderType is not BinanceFixOrderType.LimitMaker
                ? BinanceFixTimeInForce.GoodTillCanceled
                : null,
            triggerPrice: isContingent ? 9 : null,
            pegPriceType: orderType is BinanceFixOrderType.Pegged
                ? BinanceFixPegPriceType.PrimaryPeg
                : null);
    }

    private static Message CreateExecutionReport(
        string clientOrderId,
        string executionType,
        string orderStatus)
    {
        var message = new Message();
        message.Header.SetField(new MsgType(MsgType.EXECUTION_REPORT));
        Set(message, Tags.ClOrdID, clientOrderId);
        Set(message, Tags.OrdType, "2");
        Set(message, Tags.Side, "1");
        Set(message, Tags.Symbol, "BTCUSDT");
        Set(message, Tags.ExecType, executionType);
        Set(message, Tags.CumQty, "0");
        Set(message, Tags.LastQty, "0");
        Set(message, Tags.OrdStatus, orderStatus);
        return message;
    }

    private static Message CreateCancelReject(string clientOrderId)
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

    private static void Set(FieldMap fields, int tag, string value)
        => fields.SetField(new StringField(tag, value));
}
