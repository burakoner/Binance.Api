namespace Binance.Api.Convert;

/// <summary>
/// Convert Order status
/// </summary>
public enum BinanceConvertOrderStatus : byte
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
    AcceptSuccess,

    /// <summary>
    /// Success
    /// </summary>
    [Map("SUCCESS")]
    Success,

    /// <summary>
    /// Fail
    /// </summary>
    [Map("FAIL")]
    Fail
}
