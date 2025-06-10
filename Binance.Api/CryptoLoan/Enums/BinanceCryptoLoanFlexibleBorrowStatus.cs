namespace Binance.Api.CryptoLoan;

/// <summary>
/// Flexible Borrow Status
/// </summary>
public enum BinanceCryptoLoanFlexibleBorrowStatus : byte
{
    /// <summary>
    /// Pending
    /// </summary>
    [Map("PENDING")]
    Pending = 0,

    /// <summary>
    /// Processing
    /// </summary>
    [Map("Processing")]
    Processing = 1,

    /// <summary>
    /// Succeeds
    /// </summary>
    [Map("Succeeds", "SUCCESS")]
    Succeeds = 2,

    /// <summary>
    /// Failed
    /// </summary>
    [Map("Failed", "FAILED")]
    Failed = 3,
}
