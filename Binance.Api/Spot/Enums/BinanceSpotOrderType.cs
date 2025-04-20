namespace Binance.Api.Spot.Enums;

/// <summary>
/// Order type for a spot order
/// </summary>
public enum BinanceSpotOrderType : byte
{
    /// <summary>
    /// Limit orders will be placed at a specific price. If the price isn't available in the order book for that asset the order will be added in the order book for someone to fill.
    /// </summary>
    [Map("LIMIT")]
    Limit = 1,

    /// <summary>
    /// Market order will be placed without a price. The order will be executed at the best price available at that time in the order book.
    /// </summary>
    [Map("MARKET")]
    Market = 2,

    /// <summary>
    /// Stop loss order. Will execute a market order when the price drops below a price to sell and therefor limit the loss
    /// </summary>
    [Map("STOP_LOSS")]
    StopLoss = 3,

    /// <summary>
    /// Stop loss order. Will execute a limit order when the price drops below a price to sell and therefor limit the loss
    /// </summary>
    [Map("STOP_LOSS_LIMIT")]
    StopLossLimit = 4,

    /// <summary>
    /// Take profit order. Will execute a market order when the price rises above a price to sell and therefor take a profit
    /// </summary>
    [Map("TAKE_PROFIT")]
    TakeProfit = 5,

    /// <summary>
    /// Take profit limit order. Will execute a limit order when the price rises above a price to sell and therefor take a profit
    /// </summary>
    [Map("TAKE_PROFIT_LIMIT")]
    TakeProfitLimit = 6,

    /// <summary>
    /// Same as a limit order, however it will fail if the order would immediately match, therefor preventing taker orders
    /// </summary>
    [Map("LIMIT_MAKER")]
    LimitMaker = 7,
}
