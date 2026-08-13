using System.Globalization;
using QuickFix;
using QuickFix.Fields;
using Xunit;

namespace Binance.FIX.Api.Tests;

public class NewOrderSingleTests
{
    [Theory]
    [InlineData(BinanceFixOrderType.Market, "1", null)]
    [InlineData(BinanceFixOrderType.Limit, "2", null)]
    [InlineData(BinanceFixOrderType.LimitMaker, "2", "6")]
    [InlineData(BinanceFixOrderType.StopLoss, "3", null)]
    [InlineData(BinanceFixOrderType.StopLossLimit, "4", null)]
    [InlineData(BinanceFixOrderType.TakeProfit, "3", null)]
    [InlineData(BinanceFixOrderType.TakeProfitLimit, "4", null)]
    [InlineData(BinanceFixOrderType.Pegged, "P", null)]
    public void MapsEveryPublishedBinanceOrderType(
        BinanceFixOrderType orderType,
        string expectedOrdType,
        string? expectedExecInst)
    {
        var message = BinanceFixNewOrderMapper.CreateMessage(CreateValidRequest(orderType));

        Assert.Equal(MsgType.ORDER_SINGLE, message.Header.GetString(Tags.MsgType));
        Assert.Equal(expectedOrdType, message.GetString(Tags.OrdType));
        Assert.Equal(expectedExecInst is not null, message.IsSetField(Tags.ExecInst));
        if (expectedExecInst is not null)
        {
            Assert.Equal(expectedExecInst, message.GetString(Tags.ExecInst));
        }
    }

    [Theory]
    [InlineData(BinanceFixOrderType.StopLoss, BinanceFixOrderSide.Buy, "U")]
    [InlineData(BinanceFixOrderType.StopLoss, BinanceFixOrderSide.Sell, "D")]
    [InlineData(BinanceFixOrderType.StopLossLimit, BinanceFixOrderSide.Buy, "U")]
    [InlineData(BinanceFixOrderType.StopLossLimit, BinanceFixOrderSide.Sell, "D")]
    [InlineData(BinanceFixOrderType.TakeProfit, BinanceFixOrderSide.Buy, "D")]
    [InlineData(BinanceFixOrderType.TakeProfit, BinanceFixOrderSide.Sell, "U")]
    [InlineData(BinanceFixOrderType.TakeProfitLimit, BinanceFixOrderSide.Buy, "D")]
    [InlineData(BinanceFixOrderType.TakeProfitLimit, BinanceFixOrderSide.Sell, "U")]
    public void DerivesExactContingentConstantsAndDirection(
        BinanceFixOrderType orderType,
        BinanceFixOrderSide side,
        string expectedDirection)
    {
        var request = CreateValidRequest(
            orderType,
            side,
            triggerPrice: 9.5m,
            triggerTrailingDeltaBips: 125);

        var message = BinanceFixNewOrderMapper.CreateMessage(request);

        Assert.Equal("4", message.GetString(BinanceFixNewOrderMapper.TriggerTypeTag));
        Assert.Equal("1", message.GetString(BinanceFixNewOrderMapper.TriggerActionTag));
        Assert.Equal("2", message.GetString(BinanceFixNewOrderMapper.TriggerPriceTypeTag));
        Assert.Equal(expectedDirection, message.GetString(BinanceFixNewOrderMapper.TriggerPriceDirectionTag));
        Assert.Equal("9.5", message.GetString(BinanceFixNewOrderMapper.TriggerPriceTag));
        Assert.Equal("125", message.GetString(BinanceFixNewOrderMapper.TriggerTrailingDeltaBipsTag));
    }

    [Fact]
    public void MapsCompleteSharedFieldSurfaceWithInvariantSigned64Values()
    {
        var previousCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("tr-TR");
            var request = new BinanceFixNewOrderRequest(
                "client-1",
                "BTCUSDT",
                BinanceFixOrderSide.Buy,
                BinanceFixOrderType.LimitMaker,
                orderQuantity: 1.25m,
                price: 12.5m,
                timeInForce: BinanceFixTimeInForce.GoodTillCanceled,
                icebergQuantity: 0.25m,
                targetStrategy: long.MaxValue,
                strategyId: long.MinValue,
                selfTradePreventionMode: BinanceFixSelfTradePreventionMode.Transfer,
                smartOrderRouting: false);

            var message = BinanceFixNewOrderMapper.CreateMessage(request);

            Assert.Equal("client-1", message.GetString(Tags.ClOrdID));
            Assert.Equal("1.25", message.GetString(Tags.OrderQty));
            Assert.Equal("2", message.GetString(Tags.OrdType));
            Assert.Equal("6", message.GetString(Tags.ExecInst));
            Assert.Equal("12.5", message.GetString(Tags.Price));
            Assert.Equal("1", message.GetString(Tags.Side));
            Assert.Equal("BTCUSDT", message.GetString(Tags.Symbol));
            Assert.Equal("1", message.GetString(Tags.TimeInForce));
            Assert.Equal("0.25", message.GetString(Tags.MaxFloor));
            Assert.Equal(long.MaxValue.ToString(CultureInfo.InvariantCulture), message.GetString(BinanceFixNewOrderMapper.TargetStrategyTag));
            Assert.Equal(long.MinValue.ToString(CultureInfo.InvariantCulture), message.GetString(BinanceFixNewOrderMapper.StrategyIdTag));
            Assert.Equal("6", message.GetString(BinanceFixNewOrderMapper.SelfTradePreventionModeTag));
            Assert.Equal("N", message.GetString(BinanceFixNewOrderMapper.SmartOrderRoutingTag));
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }
    }

    [Theory]
    [InlineData(BinanceFixTimeInForce.GoodTillCanceled, "1")]
    [InlineData(BinanceFixTimeInForce.ImmediateOrCancel, "3")]
    [InlineData(BinanceFixTimeInForce.FillOrKill, "4")]
    public void MapsEveryTimeInForceAsAnAsciiCharacter(
        BinanceFixTimeInForce timeInForce,
        string expectedValue)
    {
        var request = new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Limit,
            orderQuantity: 1,
            price: 10,
            timeInForce: timeInForce);

        var message = BinanceFixNewOrderMapper.CreateMessage(request);

        Assert.Equal(expectedValue, message.GetString(Tags.TimeInForce));
    }

    [Theory]
    [InlineData(BinanceFixSelfTradePreventionMode.None, "1")]
    [InlineData(BinanceFixSelfTradePreventionMode.ExpireTaker, "2")]
    [InlineData(BinanceFixSelfTradePreventionMode.ExpireMaker, "3")]
    [InlineData(BinanceFixSelfTradePreventionMode.ExpireBoth, "4")]
    [InlineData(BinanceFixSelfTradePreventionMode.Decrement, "5")]
    [InlineData(BinanceFixSelfTradePreventionMode.Transfer, "6")]
    public void MapsEverySelfTradePreventionMode(
        BinanceFixSelfTradePreventionMode mode,
        string expectedValue)
    {
        var request = new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1,
            selfTradePreventionMode: mode);

        var message = BinanceFixNewOrderMapper.CreateMessage(request);

        Assert.Equal(expectedValue, message.GetString(BinanceFixNewOrderMapper.SelfTradePreventionModeTag));
    }

    [Fact]
    public void MapsQuoteQuantityMarketOrderWithoutBaseQuantity()
    {
        var request = new BinanceFixNewOrderRequest(
            "quote_market",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            cashOrderQuantity: 100.25m);

        var message = BinanceFixNewOrderMapper.CreateMessage(request);

        Assert.False(message.IsSetField(Tags.OrderQty));
        Assert.Equal("100.25", message.GetString(Tags.CashOrderQty));
    }

    [Theory]
    [InlineData(BinanceFixPegPriceType.MarketPeg, "4")]
    [InlineData(BinanceFixPegPriceType.PrimaryPeg, "5")]
    public void MapsPeggedOrderConstants(BinanceFixPegPriceType pegPriceType, string expectedPegPriceType)
    {
        var request = new BinanceFixNewOrderRequest(
            "pegged_order",
            "BTCUSDT",
            BinanceFixOrderSide.Sell,
            BinanceFixOrderType.Pegged,
            orderQuantity: 1.5m,
            pegPriceType: pegPriceType,
            pegOffsetValue: 2.5m);

        var message = BinanceFixNewOrderMapper.CreateMessage(request);

        Assert.Equal("P", message.GetString(Tags.OrdType));
        Assert.Equal("2", message.GetString(Tags.Side));
        Assert.Equal(expectedPegPriceType, message.GetString(BinanceFixNewOrderMapper.PegPriceTypeTag));
        Assert.Equal("1", message.GetString(BinanceFixNewOrderMapper.PegMoveTypeTag));
        Assert.Equal("3", message.GetString(BinanceFixNewOrderMapper.PegOffsetTypeTag));
        Assert.Equal("2.5", message.GetString(BinanceFixNewOrderMapper.PegOffsetValueTag));
    }

    [Theory]
    [InlineData("")]
    [InlineData("contains space")]
    [InlineData("client.id")]
    [InlineData("1234567890123456789012345678901234567")]
    public void RejectsClientOrderIdOutsidePublishedRegex(string clientOrderId)
    {
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            clientOrderId,
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1));
    }

    [Theory]
    [InlineData("client_1")]
    [InlineData("123456789012345678901234567890123456")]
    public void AcceptsClientOrderIdPublishedBoundaries(string clientOrderId)
    {
        var request = new BinanceFixNewOrderRequest(
            clientOrderId,
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1);

        Assert.Equal(clientOrderId, request.ClientOrderId);
    }

    [Theory]
    [InlineData("")]
    [InlineData("BTC\nUSDT")]
    public void RejectsInvalidUnicodeSymbol(string symbol)
    {
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            symbol,
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1));
    }

    [Theory]
    [InlineData("BTÇUSDT")]
    [InlineData("这是测试币456")]
    public void AcceptsCurrentUtf8SymbolBoundary(string symbol)
    {
        var request = new BinanceFixNewOrderRequest(
            "client_1",
            symbol,
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1);

        Assert.Equal(symbol, request.Symbol);
        Assert.Equal(symbol, BinanceFixNewOrderMapper.CreateMessage(request).GetString(Tags.Symbol));
    }

    [Fact]
    public void RejectsUnpairedUnicodeSurrogateInSymbol()
    {
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTC\ud800USDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1));
    }

    [Fact]
    public void RejectsUndefinedEnums()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            (BinanceFixOrderSide)0,
            BinanceFixOrderType.Market,
            orderQuantity: 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            (BinanceFixOrderType)0,
            orderQuantity: 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Limit,
            orderQuantity: 1,
            price: 1,
            timeInForce: (BinanceFixTimeInForce)0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1,
            selfTradePreventionMode: (BinanceFixSelfTradePreventionMode)0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Pegged,
            orderQuantity: 1,
            pegPriceType: (BinanceFixPegPriceType)0));
    }

    [Fact]
    public void RejectsInvalidMarketQuantityCombinations()
    {
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market));
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1,
            cashOrderQuantity: 10));
    }

    [Fact]
    public void RejectsMissingLimitFields()
    {
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Limit,
            price: 10,
            timeInForce: BinanceFixTimeInForce.GoodTillCanceled));
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Limit,
            orderQuantity: 1,
            timeInForce: BinanceFixTimeInForce.GoodTillCanceled));
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Limit,
            orderQuantity: 1,
            price: 10));
    }

    [Fact]
    public void RejectsImmediateLimitMakerTimeInForce()
    {
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.LimitMaker,
            orderQuantity: 1,
            price: 10,
            timeInForce: BinanceFixTimeInForce.ImmediateOrCancel));
    }

    [Fact]
    public void RejectsMissingOrMisplacedContingentFields()
    {
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.StopLoss,
            orderQuantity: 1));
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1,
            triggerPrice: 10));
    }

    [Fact]
    public void RejectsInvalidIcebergCombination()
    {
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Limit,
            orderQuantity: 1,
            price: 10,
            timeInForce: BinanceFixTimeInForce.ImmediateOrCancel,
            icebergQuantity: 0.5m));
    }

    [Fact]
    public void RejectsInvalidPegCombination()
    {
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Pegged,
            orderQuantity: 1));
        Assert.Throws<ArgumentException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Limit,
            orderQuantity: 1,
            price: 10,
            timeInForce: BinanceFixTimeInForce.GoodTillCanceled,
            pegPriceType: BinanceFixPegPriceType.PrimaryPeg));
    }

    [Fact]
    public void RejectsNumericValuesOutsideLocalSafetyDomain()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.StopLoss,
            orderQuantity: 1,
            triggerTrailingDeltaBips: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new BinanceFixNewOrderRequest(
            "client_1",
            "BTCUSDT",
            BinanceFixOrderSide.Buy,
            BinanceFixOrderType.Market,
            orderQuantity: 1,
            targetStrategy: 999_999));
    }

    [Fact]
    public void MapperRejectsNullRequest()
    {
        Assert.Throws<ArgumentNullException>(() => BinanceFixNewOrderMapper.CreateMessage(null!));
    }

    private static BinanceFixNewOrderRequest CreateValidRequest(
        BinanceFixOrderType orderType,
        BinanceFixOrderSide side = BinanceFixOrderSide.Buy,
        decimal? triggerPrice = null,
        long? triggerTrailingDeltaBips = null)
        => orderType switch
        {
            BinanceFixOrderType.Market => new(
                "client_1",
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1),
            BinanceFixOrderType.Limit => new(
                "client_1",
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                price: 10,
                timeInForce: BinanceFixTimeInForce.GoodTillCanceled),
            BinanceFixOrderType.LimitMaker => new(
                "client_1",
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                price: 10),
            BinanceFixOrderType.StopLoss or BinanceFixOrderType.TakeProfit => new(
                "client_1",
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                triggerPrice: triggerPrice ?? 10,
                triggerTrailingDeltaBips: triggerTrailingDeltaBips),
            BinanceFixOrderType.StopLossLimit or BinanceFixOrderType.TakeProfitLimit => new(
                "client_1",
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                price: 10,
                timeInForce: BinanceFixTimeInForce.GoodTillCanceled,
                triggerPrice: triggerPrice ?? 9.5m,
                triggerTrailingDeltaBips: triggerTrailingDeltaBips),
            BinanceFixOrderType.Pegged => new(
                "client_1",
                "BTCUSDT",
                side,
                orderType,
                orderQuantity: 1,
                pegPriceType: BinanceFixPegPriceType.PrimaryPeg),
            _ => throw new ArgumentOutOfRangeException(nameof(orderType), orderType, "Unsupported order type.")
        };
}
