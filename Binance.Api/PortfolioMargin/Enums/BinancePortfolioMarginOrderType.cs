namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Portfolio Margin Order Type
/// </summary>
public enum BinancePortfolioMarginOrderType : byte
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
}
