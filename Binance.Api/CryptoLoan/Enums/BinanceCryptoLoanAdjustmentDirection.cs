namespace Binance.Api.CryptoLoan;

/// <summary>
/// Flexible Adjustment Direction
/// </summary>
public enum BinanceCryptoLoanAdjustmentDirection : byte
{
    /// <summary>
    /// Additional
    /// </summary>
    [Map("ADDITIONAL")]
    Additional = 1,

    /// <summary>
    /// Reduced
    /// </summary>
    [Map("REDUCED")]
    Reduced = 2,
}
