namespace Binance.Api.Shared;

/// <summary>
/// The side of an order
/// </summary>
public enum BinanceOrderSide : byte
{
    /// <summary>
    /// Buy
    /// </summary>
    [Map("BUY")]
    Buy = 1,

    /// <summary>
    /// Sell
    /// </summary>
    [Map("SELL")]
    Sell = 2
}
