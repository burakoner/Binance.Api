namespace Binance.FIX.Api;

/// <summary>
/// Controls whether Binance may process concurrently submitted messages out of order.
/// </summary>
public enum BinanceFixMessageHandling
{
    /// <summary>
    /// Binance may process messages out of order. Sequence numbers must still be contiguous.
    /// </summary>
    Unordered = 1,

    /// <summary>
    /// Binance processes messages in sequence-number order. This is the wrapper default.
    /// </summary>
    Sequential = 2
}
