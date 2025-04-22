namespace Binance.Api.Shared;

/// <summary>
/// List order status
/// </summary>
public enum BinanceListOrderStatus : byte
{
    /// <summary>
    /// Executing
    /// </summary>
    [Map("EXECUTING")]
    Executing = 1,

    /// <summary>
    /// Rejected
    /// </summary>
    [Map("REJECT")]
    Rejected = 2,

    /// <summary>
    /// Done
    /// </summary>
    [Map("ALL_DONE")]
    Done = 3
}
