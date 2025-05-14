namespace Binance.Api.SubAccount;

/// <summary>
/// Transfer account type
/// </summary>
public enum BinanceSubAccountTransferAccountType : byte
{
    /// <summary>
    /// Spot
    /// </summary>
    [Map("SPOT")]
    Spot = 1,

    /// <summary>
    /// USDT-M future
    /// </summary>
    [Map("USDT_FUTURE")]
    UsdtFuture = 2,

    /// <summary>
    /// Coin-M future
    /// </summary>
    [Map("COIN_FUTURE")]
    CoinFuture = 3,

    /// <summary>
    /// Margin
    /// </summary>
    [Map("MARGIN")]
    Margin = 4,

    /// <summary>
    /// Isolated margin
    /// </summary>
    [Map("ISOLATED_MARGIN")]
    IsolatedMargin = 5,

    /// <summary>
    /// Alpha
    /// </summary>
    [Map("ALPHA")]
    Alpha = 6
}
