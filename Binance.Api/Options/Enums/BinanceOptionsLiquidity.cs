namespace Binance.Api.Options;

/// <summary>
/// Options Liquidity
/// </summary>
public enum BinanceOptionsLiquidity : byte
{
    /// <summary>
    /// Taker
    /// </summary>
    [Map("TAKER")]
    Taker = 1,

    /// <summary>
    /// Maker
    /// </summary>
    [Map("MAKER")]
    Maker = 2,
}
