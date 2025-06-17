namespace Binance.Api.Options;

/// <summary>
/// Options Trade Type
/// </summary>
public enum BinanceOptionsTradeType : byte
{
    /// <summary>
    /// Orderbook Trading
    /// </summary>
    [Map("MARKET")]
    BookTrading = 1,

    /// <summary>
    /// Block Trade
    /// </summary>
    [Map("BLOCK")]
    BlockTrading = 2,
}
