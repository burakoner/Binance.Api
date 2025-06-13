namespace Binance.Api.Link;

/// <summary>
/// Account Type
/// </summary>
public enum BinanceLinkAccountType : byte
{
    /// <summary> 
    /// Spot 
    /// </summary>
    [Map("SPOT")]
    Spot = 1,

    /// <summary> 
    /// Futures USDT
    /// </summary>
    [Map("USDT_FUTURE")]
    FuturesUsdt = 2,

    /// <summary>
    /// Futures Coin
    /// </summary>
    [Map("COIN_FUTURE")]
    FuturesCoin = 3
}