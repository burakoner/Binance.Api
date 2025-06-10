namespace Binance.Api.CryptoLoan;

/// <summary>
/// Borrow status
/// </summary>
public enum BinanceCryptoLoanStableBorrowStatus : byte
{
    /// <summary>
    /// Accruing interest
    /// </summary>
    [Map("Accuring_Interest")]
    AccruingInterest = 1,

    /// <summary>
    /// Overdue
    /// </summary>
    [Map("Overdue")]
    Overdue = 2,

    /// <summary>
    /// Currently liquidating
    /// </summary>
    [Map("Liquidating")]
    Liquidating = 3,

    /// <summary>
    /// Repaying
    /// </summary>
    [Map("Repaying")]
    Repaying = 4,

    /// <summary>
    /// Repaid
    /// </summary>
    [Map("Repaid")]
    Repaid = 5,

    /// <summary>
    /// Liquidated
    /// </summary>
    [Map("Liquidated")]
    Liquidated = 6,

    /// <summary>
    /// Pending
    /// </summary>
    [Map("Pending")]
    Pending = 7,

    /// <summary>
    /// Failed
    /// </summary>
    [Map("Failed")]
    Failed = 8
}
