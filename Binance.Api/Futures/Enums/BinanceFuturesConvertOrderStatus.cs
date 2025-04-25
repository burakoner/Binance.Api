namespace Binance.Api.Futures;

/// <summary>
/// Convert Order status
/// </summary>
public enum BinanceFuturesConvertOrderStatus : byte
{
    /// <summary>
    /// Process
    /// </summary>
    [Map("PROCESS")]
    Process = 1,

    /// <summary>
    /// Accept success
    /// </summary>
    [Map("ACCEPT_SUCCESS")]
    AcceptSuccess = 2,

    /// <summary>
    /// Success
    /// </summary>
    [Map("SUCCESS")]
    Success = 3,

    /// <summary>
    /// Fail
    /// </summary>
    [Map("FAIL")]
    Fail = 4
}
