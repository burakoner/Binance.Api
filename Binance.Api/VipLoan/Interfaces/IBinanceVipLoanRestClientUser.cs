namespace Binance.Api.VipLoan;

/// <summary>
/// Interface for the Binance VIP Loan REST API Client -> User Information Methods
/// </summary>
public interface IBinanceVipLoanRestClientUser
{
    /// <summary>
    /// VIP loan is available for VIP users only.
    /// <para><a href="https://developers.binance.com/docs/vip_loan/user-information" /></para>
    /// </summary>
    /// <param name="orderId">Order Id</param>
    /// <param name="collateralAccountId">Collateral Account Id</param>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="collateralAsset">Collateral Coin</param>
    /// <param name="current">Currently querying page. Start from 1, Default:1, Max: 1000.</param>
    /// <param name="limit">Default: 10, Max: 100</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceVipLoanOngingOrder>>> GetOngoingOrdersAsync(long? orderId = null, long? collateralAccountId = null, string? loanAsset = null, string? collateralAsset = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// VIP loan is available for VIP users only.
    /// <para><a href="https://developers.binance.com/docs/vip_loan/user-information/Get-VIP-Loan-Repayment-History" /></para>
    /// </summary>
    /// <param name="orderId">Order Id</param>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="current">Currently querying page. Start from 1, Default:1, Max: 1000.</param>
    /// <param name="limit">Default: 10, Max: 100</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceVipLoanRepayHistory>>> GetRepayHistoryAsync(long? orderId = null, string? loanAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get VIP Loan Accrued Interest (USER_DATA)
    /// <para><a href="https://developers.binance.com/docs/vip_loan/user-information/Get-VIP-Loan-Accrued-Interest" /></para>
    /// </summary>
    /// <param name="orderId">Order Id</param>
    /// <param name="loanAsset">Loan Coin</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="current">Currently querying page. Start from 1, Default:1, Max: 1000.</param>
    /// <param name="limit">Default: 10, Max: 100</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceVipLoanAccruedInterest>>> GetAccruedInterestAsync(long? orderId = null, string? loanAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// VIP loan is available for VIP users only
    /// <para><a href="https://developers.binance.com/docs/vip_loan/user-information/Check-Locked-Value-of-VIP-Collateral-Account" /></para>
    /// </summary>
    /// <param name="orderId">Order Id</param>
    /// <param name="collateralAccountId">Collateral Account Id</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceVipLoanCollateralAccount>>> CheckCollateralAccountAsync(long? orderId = null, long? collateralAccountId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Application Status
    /// <para><a href="https://developers.binance.com/docs/vip_loan/user-information/Query-Application-Status" /></para>
    /// </summary>
    /// <param name="current">Currently querying page. Start from 1, Default:1, Max: 1000.</param>
    /// <param name="limit">Default: 10, Max: 100</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceVipLoanApplicationStatus>>> GetApplicationStatusAsync(int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}
