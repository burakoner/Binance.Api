namespace Binance.Api.Futures;

/// <summary>
/// Type of Margin
/// </summary>
public enum BinanceFuturesMarginType : byte
{
    /// <summary>
    /// Isolated margin
    /// </summary>
    [Map("ISOLATED", "isolated")]
    Isolated = 1,

    /// <summary>
    /// Crossed margin
    /// </summary>
    [Map("CROSSED", "cross")]
    Cross = 2
}
