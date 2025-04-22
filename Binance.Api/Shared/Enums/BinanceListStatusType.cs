namespace Binance.Api.Shared;

/// <summary>
/// List status type
/// </summary>
public enum BinanceListStatusType : byte
{
    /// <summary>
    /// Failed action
    /// </summary>
    [Map("RESPONSE")]
    Response = 1,

    /// <summary>
    /// Placed
    /// </summary>
    [Map("EXEC_STARTED")]
    ExecutionStarted = 2,

    /// <summary>
    /// Order list is done
    /// </summary>
    [Map("ALL_DONE")]
    Done = 3
}
