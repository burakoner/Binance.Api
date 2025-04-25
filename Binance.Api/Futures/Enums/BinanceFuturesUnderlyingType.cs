namespace Binance.Api.Futures;

/// <summary>
/// Underlying Type
/// </summary>
public enum BinanceFuturesUnderlyingType : byte
{
    /// <summary>
    /// Coin
    /// </summary>
    [Map("COIN")]
    Coin = 1,

    /// <summary>
    /// Index
    /// </summary>
    [Map("INDEX")]
    Index = 2
}
