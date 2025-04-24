namespace Binance.Api.Futures;

/// <summary>
/// The direction to change futures margin
/// </summary>
public enum BinanceFuturesMarginChangeDirectionType : byte
{
    /// <summary>
    /// Add margin
    /// </summary>
    [Map("1")]
    Add = 1,

    /// <summary>
    /// Reduce Margin
    /// </summary>
    [Map("2")]
    Reduce = 2
}
