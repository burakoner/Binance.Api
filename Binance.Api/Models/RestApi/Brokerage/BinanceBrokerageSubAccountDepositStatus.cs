namespace Binance.ApiClient.Models.RestApi.Brokerage;

/// <summary>
/// Sub Account Deposit Status
/// </summary>
public enum BinanceBrokerageSubAccountDepositStatus
{
    /// <summary>
    /// Pending
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Success
    /// </summary>
    Success = 1,

    /// <summary>
    /// Credited but cannot withdraw
    /// </summary>
    CreditedButCannotWithdraw = 6,
}