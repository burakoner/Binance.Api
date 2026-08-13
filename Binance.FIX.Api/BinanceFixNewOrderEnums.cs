namespace Binance.FIX.Api;

/// <summary>
/// Side of a Binance Spot FIX order.
/// </summary>
public enum BinanceFixOrderSide
{
    /// <summary>
    /// Buy order.
    /// </summary>
    Buy = 1,

    /// <summary>
    /// Sell order.
    /// </summary>
    Sell = 2
}

/// <summary>
/// Binance Spot order type represented by a NewOrderSingle message.
/// </summary>
public enum BinanceFixOrderType
{
    /// <summary>
    /// Market order.
    /// </summary>
    Market = 1,

    /// <summary>
    /// Limit order.
    /// </summary>
    Limit = 2,

    /// <summary>
    /// Post-only limit-maker order.
    /// </summary>
    LimitMaker = 3,

    /// <summary>
    /// Stop-loss order that becomes a market order when triggered.
    /// </summary>
    StopLoss = 4,

    /// <summary>
    /// Stop-loss order that becomes a limit order when triggered.
    /// </summary>
    StopLossLimit = 5,

    /// <summary>
    /// Take-profit order that becomes a market order when triggered.
    /// </summary>
    TakeProfit = 6,

    /// <summary>
    /// Take-profit order that becomes a limit order when triggered.
    /// </summary>
    TakeProfitLimit = 7,

    /// <summary>
    /// Pegged order using Binance's FIX OrdType value P.
    /// </summary>
    Pegged = 8
}

/// <summary>
/// Time-in-force value supported by Binance Spot FIX NewOrderSingle.
/// </summary>
public enum BinanceFixTimeInForce
{
    /// <summary>
    /// Good-till-canceled.
    /// </summary>
    GoodTillCanceled = 1,

    /// <summary>
    /// Immediate-or-cancel.
    /// </summary>
    ImmediateOrCancel = 3,

    /// <summary>
    /// Fill-or-kill.
    /// </summary>
    FillOrKill = 4
}

/// <summary>
/// Self-trade-prevention mode requested for an order.
/// </summary>
public enum BinanceFixSelfTradePreventionMode
{
    /// <summary>
    /// No self-trade prevention.
    /// </summary>
    None = 1,

    /// <summary>
    /// Expire the taker order.
    /// </summary>
    ExpireTaker = 2,

    /// <summary>
    /// Expire the maker order.
    /// </summary>
    ExpireMaker = 3,

    /// <summary>
    /// Expire both orders.
    /// </summary>
    ExpireBoth = 4,

    /// <summary>
    /// Decrement both orders by the prevented quantity.
    /// </summary>
    Decrement = 5,

    /// <summary>
    /// Transfer the prevented quantity.
    /// </summary>
    Transfer = 6
}

/// <summary>
/// Reference side of the order book used for a pegged order.
/// </summary>
public enum BinanceFixPegPriceType
{
    /// <summary>
    /// Peg to the best price on the opposite side of the book.
    /// </summary>
    MarketPeg = 4,

    /// <summary>
    /// Peg to the best price on the same side of the book.
    /// </summary>
    PrimaryPeg = 5
}
