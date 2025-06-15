namespace Binance.Api.Futures;

/// <summary>
/// TickLevel Orderbook Historical Data Type
/// </summary>
public enum BinanceFuturesDataType : byte
{
    /// <summary>
    /// ADL
    /// </summary>
    [Map("T_DEPTH")]
    TickData = 1,

    /// <summary>
    /// Liquidation
    /// </summary>
    [Map("S_DEPTH")]
    Snapshot = 2
}
