namespace Binance.Api.Options;

/// <summary>
/// Options Trade Side
/// </summary>
public enum BinanceOptionsTradeSide : int
{
    /// <summary>
    /// Sell
    /// </summary>
    [Map("-1")]
    Sell = -1,

    /// <summary>
    /// Buy
    /// </summary>
    [Map("1")]
    Buy = 1,
}
