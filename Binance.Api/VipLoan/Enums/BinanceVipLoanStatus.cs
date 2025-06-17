namespace Binance.Api.VipLoan;

/// <summary>
/// Binance VIP Loan Repay Status
/// </summary>
public enum BinanceVipLoanStatus : byte
{
    /// <summary>
    /// Pending
    /// </summary>>
    [Map("Pending")]
    Pending = 1,

    /// <summary>
    /// Repaying
    /// </summary>>
    [Map("Repaying")]
    Repaying = 2,

    /// <summary>
    /// Repaid
    /// </summary>
    [Map("Repaid")]
    Repaid = 3,

    /// <summary>
    /// Liquidating
    /// </summary>>
    [Map("Liquidating")]
    Liquidating = 4,

    /// <summary>
    /// Liquidated
    /// </summary>>
    [Map("Liquidated")]
    Liquidated = 5,

    /// <summary>
    /// Overdue
    /// </summary>>
    [Map("Overdue")]
    Overdue = 6,

    /// <summary>
    /// Failed
    /// </summary>>
    [Map("Failed")]
    Failed = 7,

    /// <summary>
    /// Accruing_Interest
    /// </summary>>
    [Map("Accruing_Interest")]
    AccruingInterest = 8,
}