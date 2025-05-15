namespace Binance.Api.Shared;

/// <summary>
/// Futures account type
/// </summary>
public enum BinanceFuturesType : byte
{
    /// <summary>
    /// USDT Margined Futures
    /// </summary>
    [Map("1")]
    UsdtMarginedFutures = 1,

    /// <summary>
    /// COIN Margined Futures
    /// </summary>
    [Map("2")]
    CoinMarginedFutures = 2
}
