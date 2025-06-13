namespace Binance.Api.Link;

/// <summary>
/// Transfer transaction status
/// </summary>
public enum BinanceLinkTransferTransactionStatus : byte
{
    /// <summary>
    /// Init
    /// </summary>
    [Map("INIT")]
    Init = 1,

    /// <summary>
    /// Process
    /// </summary>
    [Map("PROCESS")]
    Process = 2,

    /// <summary> 
    /// Success 
    /// </summary>
    [Map("SUCCESS")]
    Success = 3,

    /// <summary> 
    /// Failure 
    /// </summary>
    [Map("FAILURE")]
    Failure = 4,
}