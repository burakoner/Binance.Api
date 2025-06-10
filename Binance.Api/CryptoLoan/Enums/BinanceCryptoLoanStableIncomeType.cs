namespace Binance.Api.CryptoLoan;

/// <summary>
/// Income type
/// </summary>
public enum BinanceCryptoLoanStableIncomeType : byte
{
    /// <summary>
    /// Borrow in
    /// </summary>
    [Map("borrowIn")]
    BorrowIn = 1,

    /// <summary>
    /// Collateral spent
    /// </summary>
    [Map("collateralSpent")]
    CollateralSpent = 2,

    /// <summary>
    /// Repay amount
    /// </summary>
    [Map("repayAmount")]
    RepayAmount = 3,

    /// <summary>
    /// Collateral return
    /// </summary>
    [Map("collateralReturn")]
    CollateralReturn = 4,

    /// <summary>
    /// Add collateral
    /// </summary>
    [Map("addCollateral")]
    AddCollateral = 5,

    /// <summary>
    /// Remove collateral
    /// </summary>
    [Map("removeCollateral")]
    RemoveCollateral = 6,

    /// <summary>
    /// Collateral return after liquidation
    /// </summary>
    [Map("collateralReturnAfterLiquidation")]
    CollateralReturnAfterLiquidation = 7
}
