namespace Binance.Api.CryptoLoan;

/// <summary>
/// Interface for the Binance Algo REST API Crypto Loan > Flexible Rate -> User Information Methods
/// </summary>
public interface IBinanceCryptoLoanRestClientFlexibleUser
{

    /// <summary>
    /// Get Flexible Loan LTV Adjustment History
    /// <para><a href="https://developers.binance.com/docs/crypto_loan/flexible-rate/user-information" /></para>
    /// </summary>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="collateralAsset">Collateral Coin</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="current">Current querying page. Start from 1; default: 1; max: 1000</param>
    /// <param name="limit">Default: 10; max: 100</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleAdjustmentRecord>>> GetAdjustmentHistoryAsync(string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the latest rate of collateral coin/loan coin when using collateral repay.
    /// <para><a href="https://developers.binance.com/docs/crypto_loan/flexible-rate/user-information/Check-Collateral-Repay-Rate" /></para>
    /// </summary>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="collateralAsset">Collateral Coin</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceCryptoLoanFlexibleCollateralRepayRate>> GetCollateralRepayRateAsync(string loanAsset, string collateralAsset, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Flexible Loan Borrow History
    /// <para><a href="https://developers.binance.com/docs/crypto_loan/flexible-rate/user-information/Get-Flexible-Loan-Borrow-History" /></para>
    /// </summary>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="collateralAsset">Collateral Coin</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="current">Current querying page. Start from 1; default: 1; max: 1000</param>
    /// <param name="limit">Default: 10; max: 100</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleBorrowHistory>>> GetBorrowHistoryAsync(string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Flexible Loan Ongoing Orders
    /// <para><a href="https://developers.binance.com/docs/crypto_loan/flexible-rate/user-information/Get-Flexible-Loan-Ongoing-Orders" /></para>
    /// </summary>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="collateralAsset">Collateral Coin</param>
    /// <param name="current">Current querying page. Start from 1; default: 1; max: 1000</param>
    /// <param name="limit">Default: 10; max: 100</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleOpenOrder>>> GetOpenBorrowOrdersAsync(string? loanAsset = null, string? collateralAsset = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Flexible Loan Liquidation History
    /// <para><a href="https://developers.binance.com/docs/crypto_loan/flexible-rate/user-information/Get-Flexible-Loan-Liquidation-History" /></para>
    /// </summary>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="collateralAsset">Collateral Coin</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="current">Current querying page. Start from 1; default: 1; max: 1000</param>
    /// <param name="limit">Default: 10; max: 100</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleLiquidationRecord>>> GetLiquidationHistoryAsync(string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Flexible Loan Repayment History
    /// <para><a href="https://developers.binance.com/docs/crypto_loan/flexible-rate/user-information/Get-Flexible-Loan-Repayment-History" /></para>
    /// </summary>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="collateralAsset">Collateral Coin</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="current">Current querying page. Start from 1; default: 1; max: 1000</param>
    /// <param name="limit">Default: 10; max: 100</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleRepayRecord>>> GetRepayHistoryAsync(string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}
