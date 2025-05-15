namespace Binance.Api.Broker;

/// <summary>
/// Brokerage transfer transaction status
/// </summary>
public enum BinanceBrokerTransferTransactionStatus : byte
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