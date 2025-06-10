namespace Binance.Api.CryptoLoan;

/// <summary>
/// Interface for the Binance Algo REST API Crypto Loan > Flexible Rate -> Trade Methods
/// </summary>
public interface IBinanceCryptoLoanRestClientFlexibleTrade
{
    /// <summary>
    /// Borrow Flexible Loan
    /// <para><a href="https://developers.binance.com/docs/crypto_loan/flexible-rate/trade" /></para>
    /// </summary>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="collateralAsset">Collateral Coin</param>
    /// <param name="loanQuantity">Mandatory when collateralQuantity is empty</param>
    /// <param name="collateralQuantity">Mandatory when loanQuantity is empty</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceCryptoLoanFlexibleBorrow>> BorrowAsync(string loanAsset, string collateralAsset, decimal? loanQuantity = null, decimal? collateralQuantity = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Flexible Loan Repay
    /// <para><a href="https://developers.binance.com/docs/crypto_loan/flexible-rate/trade/Flexible-Loan-Repay" /></para>
    /// </summary>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="collateralAsset">Collateral Coin</param>
    /// <param name="quantity">Repay Amount</param>
    /// <param name="collateralReturn">Default: TRUE. TRUE: Return extra collateral to spot account; FALSE: Keep extra collateral in the order, and lower LTV.</param>
    /// <param name="fullRepayment">Default: FALSE. TRUE: Full repayment; FALSE: Partial repayment, based on loanAmount</param>
    /// <param name="repaymentType">Default: 1. 1: Repayment with loan asset; 2: Repayment with collateral</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceCryptoLoanFlexibleRepay>> RepayAsync(string loanAsset, string collateralAsset, decimal quantity, bool? collateralReturn = null, bool? fullRepayment = null, BinanceCryptoLoanFlexibleRepaymentType? repaymentType = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Flexible Loan Adjust LTV
    /// <para><a href="https://developers.binance.com/docs/crypto_loan/flexible-rate/trade/Flexible-Loan-Adjust-LTV" /></para>
    /// </summary>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="collateralAsset">Collateral Coin</param>
    /// <param name="quantity">Adjustment Amount</param>
    /// <param name="direction">Adjustment Direction</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceCryptoLoanFlexibleAdjustment>> AdjustAsync(string loanAsset, string collateralAsset, decimal quantity, BinanceCryptoLoanAdjustmentDirection direction, int? receiveWindow = null, CancellationToken ct = default);
}
