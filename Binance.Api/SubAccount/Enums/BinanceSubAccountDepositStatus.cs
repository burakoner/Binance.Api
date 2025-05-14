namespace Binance.Api.SubAccount;

/// <summary>
/// The status of a deposit
/// </summary>
public enum BinanceSubAccountDepositStatus : byte
{
    /// <summary>
    /// Pending
    /// </summary>
    [Map("0")]
    Pending = 0,
    
    /// <summary>
    /// Success
    /// </summary>
    [Map("1")]
    Success = 1,

    /// <summary>
    /// Completed
    /// </summary>
    [Map("6")]
    Completed = 6
}
