namespace Binance.Api.CryptoLoan;

/// <summary>
/// Flexible Adjustment Status
/// </summary>
public enum BinanceCryptoLoanFlexibleAdjustmentStatus : byte
{
    /// <summary>
    /// Processing
    /// </summary>
    [Map("Processing")]
    Processing = 1,

    /// <summary>
    /// Succeeds
    /// </summary>
    [Map("Succeeds")]
    Succeeds = 2,

    /// <summary>
    /// Failed
    /// </summary>
    [Map("Failed")]
    Failed = 3,
}
