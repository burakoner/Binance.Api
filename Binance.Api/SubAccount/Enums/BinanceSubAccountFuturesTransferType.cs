namespace Binance.Api.SubAccount;

/// <summary>
/// Futures account transfer type
/// </summary>
public enum BinanceSubAccountFuturesTransferType : byte
{
    /// <summary>
    /// From spot to USDT-M futures account
    /// </summary>
    [Map("1")]
    FromSpotToUsdtFutures = 1,

    /// <summary>
    /// From USDT-M futures to spot account
    /// </summary>
    [Map("2")]
    FromUsdtFuturesToSpot = 2,

    /// <summary>
    /// From spot to COIN-M futures account
    /// </summary>
    [Map("3")]
    FromSpotToCoinFutures = 3,

    /// <summary>
    /// From COIN-M futures to spot account
    /// </summary>
    [Map("4")]
    FromCoinFuturesToSpot = 4
}
