namespace Binance.Api.Wallet;

/// <summary>
/// The status of a withdrawal
/// </summary>
public enum BinanceWithdrawalStatus : byte
{
    /// <summary>
    /// Email has been send
    /// </summary>
    [Map("0")]
    EmailSend = 0,

    /// <summary>
    /// Withdrawal has been canceled
    /// </summary>
    [Map("1")]
    Canceled = 1,

    /// <summary>
    /// Withdrawal is awaiting approval
    /// </summary>
    [Map("2")]
    AwaitingApproval = 2,

    /// <summary>
    /// Withdrawal has been rejected
    /// </summary>
    [Map("3")]
    Rejected = 3,

    /// <summary>
    /// Withdrawal is processing
    /// </summary>
    [Map("4")]
    Processing = 4,

    /// <summary>
    /// Withdrawal has failed
    /// </summary>
    [Map("5")]
    Failure = 5,

    /// <summary>
    /// Withdrawal has been processed
    /// </summary>
    [Map("6")]
    Completed = 6
}
