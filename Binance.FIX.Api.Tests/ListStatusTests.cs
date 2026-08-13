using System.Text;
using QuickFix;
using QuickFix.Fields;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class ListStatusTests
{
    private static readonly int[] OrderFieldOrder =
    [
        Tags.Symbol,
        Tags.OrderID,
        Tags.ClOrdID,
        BinanceFixListStatusParser.NumberOfListTriggeringInstructionsTag,
        BinanceFixListStatusParser.OrderRejectReasonTag,
        BinanceFixListStatusParser.ErrorCodeTag,
        Tags.Text,
        0
    ];

    private static readonly int[] TriggerFieldOrder =
    [
        BinanceFixListStatusParser.ListTriggerTypeTag,
        BinanceFixListStatusParser.ListTriggerOrderIndexTag,
        BinanceFixListStatusParser.ListTriggerActionTag,
        0
    ];

    [Fact]
    public void ParsesOfficialSampleWithNestedOrderOwnershipPreserved()
    {
        using var fixture = new FixListStatusDictionaryFixture();
        var result = BinanceFixListStatusParser.Parse(fixture.ParseOfficialSample(fixture.Dictionary));

        Assert.Equal("BTCUSDT", result.Symbol);
        Assert.Equal("25", result.ListId);
        Assert.Equal("1717726747805308656", result.ClientListId);
        Assert.Equal("1717726747805308656", result.OriginalClientListId);
        Assert.Equal(BinanceFixContingencyType.OneTriggersTheOther, result.ContingencyType);
        Assert.Equal(BinanceFixListStatusType.ExecutionStarted, result.StatusType);
        Assert.Equal(BinanceFixListOrderStatus.Executing, result.OrderStatus);
        Assert.Equal(DateTimeOffset.Parse("2024-06-07T02:19:07.836000+00:00"), result.TransactionTime);
        Assert.Equal(2, result.Orders.Count);
        Assert.Equal("w1717726747805308656", result.Orders[0].ClientOrderId);
        Assert.Equal(52, result.Orders[0].OrderId);
        Assert.Empty(result.Orders[0].TriggeringInstructions);
        Assert.Equal("p1717726747805308656", result.Orders[1].ClientOrderId);
        Assert.Equal(53, result.Orders[1].OrderId);

        var trigger = Assert.Single(result.Orders[1].TriggeringInstructions);
        Assert.Equal(BinanceFixListTriggerType.Filled, trigger.TriggerType);
        Assert.Equal(0, trigger.TriggerOrderIndex);
        Assert.Equal(BinanceFixListTriggerAction.Release, trigger.Action);
    }

    [Fact]
    public void PreservesPageLevelAndDictionaryOrderLevelErrorFields()
    {
        var message = CreateRequiredStatus();
        Set(message, BinanceFixListStatusParser.ListRejectReasonTag, "99");
        Set(message, BinanceFixListStatusParser.OrderRejectReasonTag, "99");
        Set(message, BinanceFixListStatusParser.ErrorCodeTag, "-2010");
        Set(message, Tags.Text, "list rejected");
        AddOrder(
            message,
            "order_1",
            orderId: null,
            orderRejectReason: "99",
            errorCode: "-1013",
            errorText: "order rejected");

        var result = BinanceFixListStatusParser.Parse(message);

        Assert.Equal(BinanceFixListRejectReason.Other, result.ListRejectReason);
        Assert.Equal(BinanceFixOrderRejectReason.Other, result.OrderRejectReason);
        Assert.Equal(-2010, result.ErrorCode);
        Assert.Equal("list rejected", result.ErrorText);
        var order = Assert.Single(result.Orders);
        Assert.Null(order.OrderId);
        Assert.Equal(BinanceFixOrderRejectReason.Other, order.OrderRejectReason);
        Assert.Equal(-1013, order.ErrorCode);
        Assert.Equal("order rejected", order.ErrorText);
    }

    [Fact]
    public void AcceptsDictionaryOptionalTopSymbolAndOrderId()
    {
        var message = CreateRequiredStatus();
        AddOrder(message, "order_1", orderId: null);

        var result = BinanceFixListStatusParser.Parse(message);

        Assert.Null(result.Symbol);
        Assert.Null(Assert.Single(result.Orders).OrderId);
    }

    [Fact]
    public void AcceptsPageDefinedStringListIdThroughTheNormalizedDictionary()
    {
        using var fixture = new FixListStatusDictionaryFixture();
        var body = $"35=N{Message.SOH}49=SPOT{Message.SOH}56=CLIENT{Message.SOH}34=1{Message.SOH}52=20240607-02:19:07.837191{Message.SOH}66=server-list{Message.SOH}429=4{Message.SOH}431=3{Message.SOH}";
        var prefix = $"8=FIX.4.4{Message.SOH}9={Encoding.ASCII.GetByteCount(body)}{Message.SOH}{body}";
        var checksum = Encoding.ASCII.GetBytes(prefix).Sum(value => value) % 256;
        var message = new Message();
        message.FromString(
            $"{prefix}10={checksum:000}{Message.SOH}",
            true,
            fixture.Dictionary,
            fixture.Dictionary,
            null,
            false);

        Assert.Equal("server-list", BinanceFixListStatusParser.Parse(message).ListId);
    }

    [Theory]
    [InlineData("2", BinanceFixListStatusType.Response)]
    [InlineData("4", BinanceFixListStatusType.ExecutionStarted)]
    [InlineData("5", BinanceFixListStatusType.AllDone)]
    [InlineData("100", BinanceFixListStatusType.Updated)]
    public void MapsEveryPublishedListStatusType(string value, BinanceFixListStatusType expected)
    {
        var message = CreateRequiredStatus();
        Set(message, BinanceFixListStatusParser.ListStatusTypeTag, value);

        Assert.Equal(expected, BinanceFixListStatusParser.Parse(message).StatusType);
    }

    [Theory]
    [InlineData("3", BinanceFixListOrderStatus.Executing)]
    [InlineData("6", BinanceFixListOrderStatus.AllDone)]
    [InlineData("7", BinanceFixListOrderStatus.Rejected)]
    public void MapsEveryPublishedListOrderStatus(string value, BinanceFixListOrderStatus expected)
    {
        var message = CreateRequiredStatus();
        Set(message, BinanceFixListStatusParser.ListOrderStatusTag, value);

        Assert.Equal(expected, BinanceFixListStatusParser.Parse(message).OrderStatus);
    }

    [Theory]
    [InlineData("1", BinanceFixListTriggerType.Activated, BinanceFixListTriggerAction.Release)]
    [InlineData("2", BinanceFixListTriggerType.PartiallyFilled, BinanceFixListTriggerAction.Cancel)]
    [InlineData("3", BinanceFixListTriggerType.Filled, BinanceFixListTriggerAction.Release)]
    public void MapsEveryPublishedTriggerTypeAndAction(
        string triggerType,
        BinanceFixListTriggerType expectedType,
        BinanceFixListTriggerAction expectedAction)
    {
        var message = CreateRequiredStatus();
        AddOrder(message, "order_1", 1, triggerType, expectedAction is BinanceFixListTriggerAction.Release ? "1" : "2");

        var trigger = Assert.Single(Assert.Single(BinanceFixListStatusParser.Parse(message).Orders).TriggeringInstructions);

        Assert.Equal(expectedType, trigger.TriggerType);
        Assert.Equal(expectedAction, trigger.Action);
    }

    [Fact]
    public void RejectsDictionarylessNestedMessageInsteadOfGuessingOwnership()
    {
        var wire = FixListStatusDictionaryFixture.OfficialSample.Replace('|', Message.SOH);
        var message = new Message();
        message.FromString(wire, true, null, null, null, false);

        Assert.Throws<FormatException>(() => BinanceFixListStatusParser.Parse(message));
    }

    [Fact]
    public void ParsesCurrentUtf8Symbols()
    {
        const string symbol = "这是测试币456";
        var message = CreateRequiredStatus();
        Set(message, Tags.Symbol, symbol);
        var order = AddOrder(message, "order_1", 1);
        Set(order, Tags.Symbol, symbol);
        message.ReplaceGroup(1, BinanceFixListStatusParser.NumberOfOrdersTag, order);

        var status = BinanceFixListStatusParser.Parse(message);

        Assert.Equal(symbol, status.Symbol);
        Assert.Equal(symbol, Assert.Single(status.Orders).Symbol);
    }

    [Theory]
    [InlineData(BinanceFixListStatusParser.ListStatusTypeTag, "99")]
    [InlineData(BinanceFixListStatusParser.ListOrderStatusTag, "99")]
    [InlineData(BinanceFixListStatusParser.ContingencyTypeTag, "3")]
    [InlineData(BinanceFixListStatusParser.ListRejectReasonTag, "1")]
    [InlineData(BinanceFixListStatusParser.OrderRejectReasonTag, "1")]
    public void RejectsUnknownTopLevelEnumValues(int tag, string value)
    {
        var message = CreateRequiredStatus();
        Set(message, tag, value);

        Assert.Throws<FormatException>(() => BinanceFixListStatusParser.Parse(message));
    }

    [Theory]
    [InlineData(BinanceFixListStatusParser.ListTriggerTypeTag, "4")]
    [InlineData(BinanceFixListStatusParser.ListTriggerActionTag, "3")]
    [InlineData(BinanceFixListStatusParser.ListTriggerOrderIndexTag, "-1")]
    [InlineData(BinanceFixListStatusParser.ListTriggerOrderIndexTag, "not-an-int")]
    public void RejectsInvalidNestedTriggerValues(int tag, string value)
    {
        var message = CreateRequiredStatus();
        var order = AddOrder(message, "order_1", 1, "1", "1");
        var trigger = new Group(
            BinanceFixListStatusParser.NumberOfListTriggeringInstructionsTag,
            BinanceFixListStatusParser.ListTriggerTypeTag,
            TriggerFieldOrder);
        order.GetGroup(1, trigger);
        Set(trigger, tag, value);
        order.ReplaceGroup(1, BinanceFixListStatusParser.NumberOfListTriggeringInstructionsTag, trigger);
        message.ReplaceGroup(1, BinanceFixListStatusParser.NumberOfOrdersTag, order);

        Assert.Throws<FormatException>(() => BinanceFixListStatusParser.Parse(message));
    }

    [Theory]
    [InlineData(BinanceFixListStatusParser.ListStatusTypeTag)]
    [InlineData(BinanceFixListStatusParser.ListOrderStatusTag)]
    public void RejectsMissingRequiredTopLevelFields(int tag)
    {
        var message = CreateRequiredStatus();
        message.RemoveField(tag);

        Assert.Throws<FormatException>(() => BinanceFixListStatusParser.Parse(message));
    }

    [Fact]
    public void RejectsMissingRequiredOrderOrTriggerFields()
    {
        var withoutClientOrderId = CreateRequiredStatus();
        var firstOrder = AddOrder(withoutClientOrderId, "order_1", 1);
        firstOrder.RemoveField(Tags.ClOrdID);
        withoutClientOrderId.ReplaceGroup(1, BinanceFixListStatusParser.NumberOfOrdersTag, firstOrder);
        Assert.Throws<FormatException>(() => BinanceFixListStatusParser.Parse(withoutClientOrderId));

        var withoutTriggerAction = CreateRequiredStatus();
        var secondOrder = AddOrder(withoutTriggerAction, "order_1", 1, "1", "1");
        var trigger = new Group(
            BinanceFixListStatusParser.NumberOfListTriggeringInstructionsTag,
            BinanceFixListStatusParser.ListTriggerTypeTag,
            TriggerFieldOrder);
        secondOrder.GetGroup(1, trigger);
        trigger.RemoveField(BinanceFixListStatusParser.ListTriggerActionTag);
        secondOrder.ReplaceGroup(1, BinanceFixListStatusParser.NumberOfListTriggeringInstructionsTag, trigger);
        withoutTriggerAction.ReplaceGroup(1, BinanceFixListStatusParser.NumberOfOrdersTag, secondOrder);
        Assert.Throws<FormatException>(() => BinanceFixListStatusParser.Parse(withoutTriggerAction));
    }

    [Fact]
    public void RejectsDeclaredOrderAndTriggerCountMismatches()
    {
        var orderMismatch = CreateRequiredStatus();
        AddOrder(orderMismatch, "order_1", 1);
        Set(orderMismatch, BinanceFixListStatusParser.NumberOfOrdersTag, "2");
        Assert.Throws<FormatException>(() => BinanceFixListStatusParser.Parse(orderMismatch));

        var triggerMismatch = CreateRequiredStatus();
        var order = AddOrder(triggerMismatch, "order_1", 1, "1", "1");
        Set(order, BinanceFixListStatusParser.NumberOfListTriggeringInstructionsTag, "2");
        triggerMismatch.ReplaceGroup(1, BinanceFixListStatusParser.NumberOfOrdersTag, order);
        Assert.Throws<FormatException>(() => BinanceFixListStatusParser.Parse(triggerMismatch));
    }

    [Theory]
    [InlineData(BinanceFixListStatusParser.ClientListIdTag, "bad id")]
    [InlineData(BinanceFixListStatusParser.OriginalClientListIdTag, "1234567890123456789012345678901234567")]
    [InlineData(Tags.TransactTime, "2024-06-07")]
    [InlineData(BinanceFixListStatusParser.ErrorCodeTag, "1.5")]
    [InlineData(Tags.Text, "bad\ntext")]
    public void RejectsMalformedScalarFields(int tag, string value)
    {
        var message = CreateRequiredStatus();
        Set(message, tag, value);

        Assert.Throws<FormatException>(() => BinanceFixListStatusParser.Parse(message));
    }

    [Fact]
    public void RejectsWrongMessageTypeAndNull()
    {
        var wrongType = CreateRequiredStatus();
        wrongType.Header.SetField(new MsgType(MsgType.EXECUTION_REPORT));

        Assert.Throws<FormatException>(() => BinanceFixListStatusParser.Parse(wrongType));
        Assert.Throws<ArgumentNullException>(() => BinanceFixListStatusParser.Parse(null!));
    }

    private static Message CreateRequiredStatus()
    {
        var message = new Message();
        message.Header.SetField(new MsgType("N"));
        Set(message, BinanceFixListStatusParser.ListStatusTypeTag, "4");
        Set(message, BinanceFixListStatusParser.ListOrderStatusTag, "3");
        return message;
    }

    private static Group AddOrder(
        Message message,
        string clientOrderId,
        long? orderId,
        string? triggerType = null,
        string triggerAction = "1",
        string? orderRejectReason = null,
        string? errorCode = null,
        string? errorText = null)
    {
        var order = new Group(
            BinanceFixListStatusParser.NumberOfOrdersTag,
            Tags.Symbol,
            OrderFieldOrder);
        Set(order, Tags.Symbol, "BTCUSDT");
        Set(order, Tags.ClOrdID, clientOrderId);
        if (orderId is not null)
        {
            Set(order, Tags.OrderID, orderId.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        if (triggerType is not null)
        {
            var trigger = new Group(
                BinanceFixListStatusParser.NumberOfListTriggeringInstructionsTag,
                BinanceFixListStatusParser.ListTriggerTypeTag,
                TriggerFieldOrder);
            Set(trigger, BinanceFixListStatusParser.ListTriggerTypeTag, triggerType);
            Set(trigger, BinanceFixListStatusParser.ListTriggerOrderIndexTag, "0");
            Set(trigger, BinanceFixListStatusParser.ListTriggerActionTag, triggerAction);
            order.AddGroup(trigger);
        }

        if (orderRejectReason is not null)
        {
            Set(order, BinanceFixListStatusParser.OrderRejectReasonTag, orderRejectReason);
        }

        if (errorCode is not null)
        {
            Set(order, BinanceFixListStatusParser.ErrorCodeTag, errorCode);
        }

        if (errorText is not null)
        {
            Set(order, Tags.Text, errorText);
        }

        message.AddGroup(order);
        return order;
    }

    private static void Set(FieldMap fields, int tag, string value)
        => fields.SetField(new StringField(tag, value));
}
