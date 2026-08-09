namespace Binance.Api.Futures;

/// <summary>
/// Supported native USD-M conditional Algo order types
/// </summary>
public enum BinanceFuturesAlgoOrderType : byte
{
    /// <summary>Stop limit order</summary>
    [Map("STOP")]
    Stop = 1,

    /// <summary>Stop market order</summary>
    [Map("STOP_MARKET")]
    StopMarket = 2,

    /// <summary>Take-profit limit order</summary>
    [Map("TAKE_PROFIT")]
    TakeProfit = 3,

    /// <summary>Take-profit market order</summary>
    [Map("TAKE_PROFIT_MARKET")]
    TakeProfitMarket = 4,

    /// <summary>Trailing-stop market order</summary>
    [Map("TRAILING_STOP_MARKET")]
    TrailingStopMarket = 5
}
