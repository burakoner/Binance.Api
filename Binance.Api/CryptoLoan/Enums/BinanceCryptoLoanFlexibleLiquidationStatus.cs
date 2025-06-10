namespace Binance.Api.CryptoLoan;

/// <summary>
/// Flexible Liquidation Status
/// </summary>
public enum BinanceCryptoLoanFlexibleLiquidationStatus : byte
{
    /// <summary>
    /// Liquidating
    /// </summary>
    [Map("Liquidating")]
    Liquidating = 1,

    /// <summary>
    /// Liquidated
    /// </summary>
    [Map("Liquidated")]
    Liquidated = 2,
}
