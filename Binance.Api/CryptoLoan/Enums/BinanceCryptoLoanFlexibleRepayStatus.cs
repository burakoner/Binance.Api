namespace Binance.Api.CryptoLoan;

/// <summary>
/// Flexible Repayment Status
/// </summary>
public enum BinanceCryptoLoanFlexibleRepayStatus : byte
{
    /// <summary>
    /// Repaying
    /// </summary>
    [Map("Repaying")]
    Repaying = 1,

    /// <summary>
    /// Repaid
    /// </summary>
    [Map("Repaid")]
    Repaid = 2,

    /// <summary>
    /// Failed
    /// </summary>
    [Map("Failed")]
    Failed = 3,
}
