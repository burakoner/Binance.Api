namespace Binance.Api.Futures;

/// <summary>
/// Price matching type
/// </summary>
public enum BinanceFuturesPriceMatch : byte
{
    /// <summary>
    /// No price match
    /// </summary>
    [Map("NONE")]
    None = 0,

    /// <summary>
    /// Counterparty best price
    /// </summary>
    [Map("OPPONENT")]
    Opponent = 1,

    /// <summary>
    /// Counterparty 5th best price
    /// </summary>
    [Map("OPPONENT_5")]
    Opponent5 = 5,

    /// <summary>
    /// Counterparty 10th best price
    /// </summary>
    [Map("OPPONENT_10")]
    Opponent10 = 10,

    /// <summary>
    /// Counterparty 20th best price
    /// </summary>
    [Map("OPPONENT_20")]
    Opponent20 = 20,

    /// <summary>
    /// The best price on the same side of the order book
    /// </summary>
    [Map("QUEUE")]
    Queue = 101,

    /// <summary>
    /// The 5th best price on the same side of the order book
    /// </summary>
    [Map("QUEUE_5")]
    Queue5 = 105,

    /// <summary>
    /// The 10th best price on the same side of the order book
    /// </summary>
    [Map("QUEUE_10")]
    Queue10 = 110,

    /// <summary>
    /// The 20th best price on the same side of the order book
    /// </summary>
    [Map("QUEUE_20")]
    Queue20 = 120,
}
