namespace Binance.Api.CryptoLoan;

/// <summary>
/// Flexible Repayment Type
/// </summary>
public enum BinanceCryptoLoanFlexibleRepaymentType : byte
{
    /// <summary>
    /// Repayment with loan asset
    /// </summary>
    [Map("1")]
    RepaymentWithLoanAsset= 1,

    /// <summary>
    /// Repayment with collateral
    /// </summary>
    [Map("2")]
    RepaymentWithCollateral = 2,
}
