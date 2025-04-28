namespace Binance.Api.Futures;

/// <summary>
/// Type of auto close
/// </summary>
public enum BinanceFuturesAutoCloseType : byte
{
    /// <summary>
    /// ADL
    /// </summary>
    [Map("ADL")]
    ADL = 1,

    /// <summary>
    /// Liquidation
    /// </summary>
    [Map("LIQUIDATION")]
    Liquidation = 2
}
