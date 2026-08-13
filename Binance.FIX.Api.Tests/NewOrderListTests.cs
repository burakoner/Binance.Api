using QuickFix;
using QuickFix.Fields;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class NewOrderListTests
{
    private static readonly int[] OrderFieldOrder =
    [
        Tags.ClOrdID,
        Tags.OrderQty,
        Tags.OrdType,
        Tags.ExecInst,
        Tags.Price,
        BinanceFixNewOrderMapper.TriggerTypeTag,
        BinanceFixNewOrderMapper.TriggerActionTag,
        BinanceFixNewOrderMapper.TriggerPriceTag,
        BinanceFixNewOrderMapper.TriggerPriceTypeTag,
        BinanceFixNewOrderMapper.TriggerPriceDirectionTag,
        BinanceFixNewOrderMapper.TriggerTrailingDeltaBipsTag,
        BinanceFixNewOrderMapper.PegOffsetValueTag,
        BinanceFixNewOrderMapper.PegPriceTypeTag,
        BinanceFixNewOrderMapper.PegMoveTypeTag,
        BinanceFixNewOrderMapper.PegOffsetTypeTag,
        Tags.Side,
        Tags.Symbol,
        Tags.TimeInForce,
        Tags.MaxFloor,
        Tags.CashOrderQty,
        BinanceFixNewOrderMapper.TargetStrategyTag,
        BinanceFixNewOrderMapper.StrategyIdTag,
        BinanceFixNewOrderMapper.SelfTradePreventionModeTag,
        BinanceFixNewOrderListMapper.NumberOfListTriggeringInstructionsTag,
        0
    ];

    private static readonly int[] TriggerFieldOrder =
    [
        BinanceFixNewOrderListMapper.ListTriggerTypeTag,
        BinanceFixNewOrderListMapper.ListTriggerOrderIndexTag,
        BinanceFixNewOrderListMapper.ListTriggerActionTag,
        0
    ];

    [Fact]
    public void MapsExactSellOcoInstructions()
    {
        var request = CreateList(
            BinanceFixOrderListType.OneCancelsTheOther,
            CreateOrder("below", BinanceFixOrderType.StopLoss, BinanceFixOrderSide.Sell),
            CreateOrder("above", BinanceFixOrderType.LimitMaker, BinanceFixOrderSide.Sell));

        var message = BinanceFixNewOrderListMapper.CreateMessage(request);

        AssertHeader(message, request, "1", expectedOnePaysTheOther: false);
        AssertTrigger(GetOrder(message, 1), 1, "2", "1", "2");
        AssertTrigger(GetOrder(message, 2), 1, "1", "0", "2");
    }

    [Fact]
    public void MapsOtoReleaseAndOpoWireDifference()
    {
        var oto = CreateList(
            BinanceFixOrderListType.OneTriggersTheOther,
            CreateOrder("working", BinanceFixOrderType.Limit, BinanceFixOrderSide.Sell),
            CreateOrder("pending", BinanceFixOrderType.Market, BinanceFixOrderSide.Buy));
        var opo = CreateList(
            BinanceFixOrderListType.OnePaysTheOther,
            CreateOrder("paying", BinanceFixOrderType.LimitMaker, BinanceFixOrderSide.Buy),
            CreateOrder("paid", BinanceFixOrderType.Market, BinanceFixOrderSide.Sell));

        var otoMessage = BinanceFixNewOrderListMapper.CreateMessage(oto);
        var opoMessage = BinanceFixNewOrderListMapper.CreateMessage(opo);

        AssertHeader(otoMessage, oto, "2", expectedOnePaysTheOther: false);
        Assert.Equal(0, GetOrder(otoMessage, 1).GroupCount(BinanceFixNewOrderListMapper.NumberOfListTriggeringInstructionsTag));
        AssertTrigger(GetOrder(otoMessage, 2), 1, "3", "0", "1");
        AssertHeader(opoMessage, opo, "2", expectedOnePaysTheOther: true);
        AssertTrigger(GetOrder(opoMessage, 2), 1, "3", "0", "1");
    }

    [Fact]
    public void MapsExactOtocoInstructions()
    {
        var request = CreateList(
            BinanceFixOrderListType.OneTriggersTheOtherOneCancelsTheOther,
            CreateOrder("working", BinanceFixOrderType.Limit, BinanceFixOrderSide.Buy),
            CreateOrder("below", BinanceFixOrderType.StopLossLimit, BinanceFixOrderSide.Sell),
            CreateOrder("above", BinanceFixOrderType.LimitMaker, BinanceFixOrderSide.Sell));

        var message = BinanceFixNewOrderListMapper.CreateMessage(request);

        AssertHeader(message, request, "2", expectedOnePaysTheOther: false);
        Assert.Equal(0, GetOrder(message, 1).GroupCount(BinanceFixNewOrderListMapper.NumberOfListTriggeringInstructionsTag));
        AssertTrigger(GetOrder(message, 2), 1, "3", "0", "2");
        AssertTrigger(GetOrder(message, 2), 2, "2", "2", "2");
        AssertTrigger(GetOrder(message, 3), 1, "3", "0", "2");
        AssertTrigger(GetOrder(message, 3), 2, "1", "1", "2");
    }

    [Fact]
    public void MapsCurrentOpocoFlagAndTakeProfitLimitVariant()
    {
        var request = CreateList(
            BinanceFixOrderListType.OnePaysTheOtherOneCancelsTheOther,
            CreateOrder("working", BinanceFixOrderType.Limit, BinanceFixOrderSide.Buy),
            CreateOrder("below", BinanceFixOrderType.StopLoss, BinanceFixOrderSide.Sell),
            CreateOrder("above", BinanceFixOrderType.TakeProfitLimit, BinanceFixOrderSide.Sell));

        var message = BinanceFixNewOrderListMapper.CreateMessage(request);

        AssertHeader(message, request, "2", expectedOnePaysTheOther: true);
        AssertTrigger(GetOrder(message, 2), 1, "3", "0", "2");
        AssertTrigger(GetOrder(message, 2), 2, "1", "2", "2");
        AssertTrigger(GetOrder(message, 3), 1, "3", "0", "2");
        AssertTrigger(GetOrder(message, 3), 2, "1", "1", "2");
    }

    [Fact]
    public void ReusesCompleteSharedNewOrderFieldsWithoutSor()
    {
        var working = new BinanceFixNewOrderRequest(
            "working",
            "这是测试币456",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.LimitMaker,
            orderQuantity: 1.25m,
            price: 12.5m,
            timeInForce: BinanceFixTimeInForce.GoodTillCanceled,
            icebergQuantity: 0.25m,
            targetStrategy: long.MaxValue,
            strategyId: long.MinValue,
            selfTradePreventionMode: BinanceFixSelfTradePreventionMode.Transfer);
        var pending = new BinanceFixNewOrderRequest(
            "pending",
            "这是测试币456",
            BinanceFixOrderSide.Sell,
            BinanceFixOrderType.Pegged,
            orderQuantity: 2.5m,
            pegPriceType: BinanceFixPegPriceType.PrimaryPeg,
            pegOffsetValue: -2m);
        var request = CreateList(BinanceFixOrderListType.OneTriggersTheOther, working, pending);

        var message = BinanceFixNewOrderListMapper.CreateMessage(request);
        var first = GetOrder(message, 1);
        var second = GetOrder(message, 2);

        Assert.Equal("这是测试币456", first.GetString(Tags.Symbol));
        Assert.Equal("1.25", first.GetString(Tags.OrderQty));
        Assert.Equal("6", first.GetString(Tags.ExecInst));
        Assert.Equal("0.25", first.GetString(Tags.MaxFloor));
        Assert.Equal(long.MaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture), first.GetString(BinanceFixNewOrderMapper.TargetStrategyTag));
        Assert.Equal(long.MinValue.ToString(System.Globalization.CultureInfo.InvariantCulture), first.GetString(BinanceFixNewOrderMapper.StrategyIdTag));
        Assert.Equal("6", first.GetString(BinanceFixNewOrderMapper.SelfTradePreventionModeTag));
        Assert.Equal("P", second.GetString(Tags.OrdType));
        Assert.Equal("5", second.GetString(BinanceFixNewOrderMapper.PegPriceTypeTag));
        Assert.Equal("1", second.GetString(BinanceFixNewOrderMapper.PegMoveTypeTag));
        Assert.Equal("3", second.GetString(BinanceFixNewOrderMapper.PegOffsetTypeTag));
        Assert.Equal("-2", second.GetString(BinanceFixNewOrderMapper.PegOffsetValueTag));
        Assert.False(first.IsSetField(BinanceFixNewOrderMapper.SmartOrderRoutingTag));
        Assert.False(second.IsSetField(BinanceFixNewOrderMapper.SmartOrderRoutingTag));
    }

    [Fact]
    public void MapsPublishedTriggerDirectionWithoutTriggerPrice()
    {
        var working = CreateOrder("working", BinanceFixOrderType.Limit, BinanceFixOrderSide.Buy);
        var trailing = new BinanceFixNewOrderRequest(
            "trailing",
            "BTCUSDT",
            BinanceFixOrderSide.Sell,
            BinanceFixOrderType.StopLoss,
            orderQuantity: 1,
            triggerTrailingDeltaBips: 125);
        var request = CreateList(BinanceFixOrderListType.OneTriggersTheOther, working, trailing);

        var pending = GetOrder(BinanceFixNewOrderListMapper.CreateMessage(request), 2);

        Assert.False(pending.IsSetField(BinanceFixNewOrderMapper.TriggerPriceTag));
        Assert.Equal("D", pending.GetString(BinanceFixNewOrderMapper.TriggerPriceDirectionTag));
        Assert.Equal("125", pending.GetString(BinanceFixNewOrderMapper.TriggerTrailingDeltaBipsTag));
    }

    [Fact]
    public void AcceptsEveryPublishedOcoAndOtocoSideTypePattern()
    {
        _ = CreateList(
            BinanceFixOrderListType.OneCancelsTheOther,
            CreateOrder("a", BinanceFixOrderType.StopLoss, BinanceFixOrderSide.Sell),
            CreateOrder("b", BinanceFixOrderType.TakeProfit, BinanceFixOrderSide.Sell));
        _ = CreateList(
            BinanceFixOrderListType.OneCancelsTheOther,
            CreateOrder("a", BinanceFixOrderType.TakeProfit, BinanceFixOrderSide.Buy),
            CreateOrder("b", BinanceFixOrderType.StopLossLimit, BinanceFixOrderSide.Buy));
        _ = CreateList(
            BinanceFixOrderListType.OneCancelsTheOther,
            CreateOrder("a", BinanceFixOrderType.LimitMaker, BinanceFixOrderSide.Buy),
            CreateOrder("b", BinanceFixOrderType.StopLoss, BinanceFixOrderSide.Buy));
        _ = CreateList(
            BinanceFixOrderListType.OneTriggersTheOtherOneCancelsTheOther,
            CreateOrder("w", BinanceFixOrderType.LimitMaker, BinanceFixOrderSide.Sell),
            CreateOrder("a", BinanceFixOrderType.LimitMaker, BinanceFixOrderSide.Buy),
            CreateOrder("b", BinanceFixOrderType.StopLoss, BinanceFixOrderSide.Buy));
        _ = CreateList(
            BinanceFixOrderListType.OneTriggersTheOtherOneCancelsTheOther,
            CreateOrder("w", BinanceFixOrderType.Limit, BinanceFixOrderSide.Buy),
            CreateOrder("a", BinanceFixOrderType.StopLoss, BinanceFixOrderSide.Sell),
            CreateOrder("b", BinanceFixOrderType.TakeProfit, BinanceFixOrderSide.Sell));
        _ = CreateList(
            BinanceFixOrderListType.OneTriggersTheOtherOneCancelsTheOther,
            CreateOrder("w", BinanceFixOrderType.Limit, BinanceFixOrderSide.Sell),
            CreateOrder("a", BinanceFixOrderType.TakeProfit, BinanceFixOrderSide.Buy),
            CreateOrder("b", BinanceFixOrderType.StopLossLimit, BinanceFixOrderSide.Buy));
        _ = CreateList(
            BinanceFixOrderListType.OnePaysTheOtherOneCancelsTheOther,
            CreateOrder("w", BinanceFixOrderType.LimitMaker, BinanceFixOrderSide.Buy),
            CreateOrder("a", BinanceFixOrderType.StopLoss, BinanceFixOrderSide.Sell),
            CreateOrder("b", BinanceFixOrderType.LimitMaker, BinanceFixOrderSide.Sell));
    }

    [Theory]
    [InlineData("")]
    [InlineData("list id")]
    [InlineData("list!")]
    [InlineData("1234567890123456789012345678901234567")]
    public void RejectsInvalidClientListId(string clientListId)
    {
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderListRequest(
            clientListId,
            BinanceFixOrderListType.OneTriggersTheOther,
            [
                CreateOrder("working", BinanceFixOrderType.Limit, BinanceFixOrderSide.Buy),
                CreateOrder("pending", BinanceFixOrderType.Market, BinanceFixOrderSide.Sell)
            ]));
    }

    [Fact]
    public void RejectsInvalidCountsNullsEnumsAndSor()
    {
        var working = CreateOrder("working", BinanceFixOrderType.Limit, BinanceFixOrderSide.Buy);
        var pending = CreateOrder("pending", BinanceFixOrderType.Market, BinanceFixOrderSide.Sell);
        var sor = new BinanceFixNewOrderRequest(
            "sor",
            "BTCUSDT",
            BinanceFixOrderSide.Sell,
            BinanceFixOrderType.Market,
            orderQuantity: 1,
            smartOrderRouting: true);

        Assert.Throws<ArgumentNullException>(() => new BinanceFixNewOrderListRequest(
            "list_1",
            BinanceFixOrderListType.OneTriggersTheOther,
            null!));
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderListRequest(
            "list_1",
            BinanceFixOrderListType.OneTriggersTheOther,
            [working]));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixNewOrderListRequest(
            "list_1",
            (BinanceFixOrderListType)99,
            [working, pending, pending]));
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderListRequest(
            "list_1",
            BinanceFixOrderListType.OneTriggersTheOther,
            [working, null!]));
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderListRequest(
            "list_1",
            BinanceFixOrderListType.OneTriggersTheOther,
            [working, sor]));
        Assert.Throws<ArgumentNullException>(() => BinanceFixNewOrderListMapper.CreateMessage(null!));
    }

    [Fact]
    public void RejectsUnsupportedShapesAndKeepsPublishedOpocoExceptionNarrow()
    {
        Assert.Throws<ArgumentException>(() => CreateList(
            BinanceFixOrderListType.OneCancelsTheOther,
            CreateOrder("a", BinanceFixOrderType.StopLoss, BinanceFixOrderSide.Sell),
            CreateOrder("b", BinanceFixOrderType.LimitMaker, BinanceFixOrderSide.Buy)));
        Assert.Throws<ArgumentException>(() => CreateList(
            BinanceFixOrderListType.OneTriggersTheOther,
            CreateOrder("a", BinanceFixOrderType.Market, BinanceFixOrderSide.Buy),
            CreateOrder("b", BinanceFixOrderType.Market, BinanceFixOrderSide.Sell)));
        Assert.Throws<ArgumentException>(() => CreateList(
            BinanceFixOrderListType.OnePaysTheOther,
            CreateOrder("a", BinanceFixOrderType.Limit, BinanceFixOrderSide.Sell),
            CreateOrder("b", BinanceFixOrderType.Market, BinanceFixOrderSide.Buy)));
        Assert.Throws<ArgumentException>(() => CreateList(
            BinanceFixOrderListType.OneTriggersTheOtherOneCancelsTheOther,
            CreateOrder("w", BinanceFixOrderType.Limit, BinanceFixOrderSide.Buy),
            CreateOrder("a", BinanceFixOrderType.StopLoss, BinanceFixOrderSide.Sell),
            CreateOrder("b", BinanceFixOrderType.TakeProfitLimit, BinanceFixOrderSide.Sell)));
    }

    [Fact]
    public void CopiesCallerOrderCollection()
    {
        var source = new List<BinanceFixNewOrderRequest>
        {
            CreateOrder("working", BinanceFixOrderType.Limit, BinanceFixOrderSide.Buy),
            CreateOrder("pending", BinanceFixOrderType.Market, BinanceFixOrderSide.Sell)
        };
        var request = new BinanceFixNewOrderListRequest(
            "list_1",
            BinanceFixOrderListType.OneTriggersTheOther,
            source);

        source[1] = CreateOrder("changed", BinanceFixOrderType.Market, BinanceFixOrderSide.Sell);

        Assert.Equal("pending", request.Orders[1].ClientOrderId);
    }

    private static BinanceFixNewOrderListRequest CreateList(
        BinanceFixOrderListType listType,
        params BinanceFixNewOrderRequest[] orders)
        => new("list_1", listType, orders);

    private static BinanceFixNewOrderRequest CreateOrder(
        string clientOrderId,
        BinanceFixOrderType orderType,
        BinanceFixOrderSide side)
        => orderType switch
        {
            BinanceFixOrderType.Market => new(
                clientOrderId,
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1),
            BinanceFixOrderType.Limit => new(
                clientOrderId,
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                price: 10,
                timeInForce: BinanceFixTimeInForce.GoodTillCanceled),
            BinanceFixOrderType.LimitMaker => new(
                clientOrderId,
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                price: 10),
            BinanceFixOrderType.StopLoss => new(
                clientOrderId,
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                triggerPrice: 9),
            BinanceFixOrderType.StopLossLimit => new(
                clientOrderId,
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                price: 9,
                timeInForce: BinanceFixTimeInForce.GoodTillCanceled,
                triggerPrice: 10),
            BinanceFixOrderType.TakeProfit => new(
                clientOrderId,
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                triggerPrice: 11),
            BinanceFixOrderType.TakeProfitLimit => new(
                clientOrderId,
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                price: 11,
                timeInForce: BinanceFixTimeInForce.GoodTillCanceled,
                triggerPrice: 10),
            BinanceFixOrderType.Pegged => new(
                clientOrderId,
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                pegPriceType: BinanceFixPegPriceType.MarketPeg),
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, null)
        };

    private static Group GetOrder(Message message, int oneBasedIndex)
    {
        var group = new Group(BinanceFixNewOrderListMapper.NumberOfOrdersTag, Tags.ClOrdID, OrderFieldOrder);
        message.GetGroup(oneBasedIndex, group);
        return group;
    }

    private static void AssertHeader(
        Message message,
        BinanceFixNewOrderListRequest request,
        string expectedContingencyType,
        bool expectedOnePaysTheOther)
    {
        Assert.Equal(BinanceFixNewOrderListMapper.MessageType, message.Header.GetString(Tags.MsgType));
        Assert.Equal(request.ClientListId, message.GetString(BinanceFixNewOrderListMapper.ClientListIdTag));
        Assert.Equal(expectedContingencyType, message.GetString(BinanceFixNewOrderListMapper.ContingencyTypeTag));
        Assert.Equal(request.Orders.Count, message.GetInt(BinanceFixNewOrderListMapper.NumberOfOrdersTag));
        Assert.Equal(expectedOnePaysTheOther, message.IsSetField(BinanceFixNewOrderListMapper.OnePaysTheOtherTag));
        if (expectedOnePaysTheOther)
        {
            Assert.Equal("Y", message.GetString(BinanceFixNewOrderListMapper.OnePaysTheOtherTag));
        }
    }

    private static void AssertTrigger(
        Group order,
        int oneBasedIndex,
        string expectedType,
        string expectedOrderIndex,
        string expectedAction)
    {
        var trigger = new Group(
            BinanceFixNewOrderListMapper.NumberOfListTriggeringInstructionsTag,
            BinanceFixNewOrderListMapper.ListTriggerTypeTag,
            TriggerFieldOrder);
        order.GetGroup(oneBasedIndex, trigger);

        Assert.Equal(expectedType, trigger.GetString(BinanceFixNewOrderListMapper.ListTriggerTypeTag));
        Assert.Equal(expectedOrderIndex, trigger.GetString(BinanceFixNewOrderListMapper.ListTriggerOrderIndexTag));
        Assert.Equal(expectedAction, trigger.GetString(BinanceFixNewOrderListMapper.ListTriggerActionTag));
    }
}
