namespace Binance.Api.Futures;

/// <summary>
/// Type of working
/// </summary>
public enum BinanceFuturesWorkingType : byte
{
    /// <summary>
    /// Mark price type
    /// </summary>
    [Map("MARK_PRICE")]
    Mark = 1,

    /// <summary>
    /// Contract price type
    /// </summary>
    [Map("CONTRACT_PRICE")]
    Contract = 2
}
