namespace Binance.Api.Spot;

/// <summary>
/// The side of an order
/// </summary>
public enum BinanceSpotOrderSide : byte
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
