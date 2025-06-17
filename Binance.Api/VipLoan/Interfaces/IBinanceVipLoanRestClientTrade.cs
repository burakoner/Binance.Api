namespace Binance.Api.VipLoan;

/// <summary>
/// Interface for the Binance VIP Loan REST API Client -> Trade Methods
/// </summary>
public interface IBinanceVipLoanRestClientTrade
{
    /// <summary>
    /// VIP loan is available for VIP users only.
    /// <para><a href="https://developers.binance.com/docs/vip_loan/trade/VIP-Loan-Borrow" /></para>
    /// </summary>
    /// <param name="loanAccountId">Loan Account ID</param>
    /// <param name="loanCoin">Loan Asset</param>
    /// <param name="loanQuantity">Loan Quantity</param>
    /// <param name="collateralAccountId">Collateral Account Id</param>
    /// <param name="collateralCoin">Collateral Coin</param>
    /// <param name="isFlexibleRate">Default: TRUE. TRUE : flexible rate; FALSE: fixed rate</param>
    /// <param name="loanTerm">Mandatory for fixed rate. Optional for fixed interest rate. Eg: 30/60 days</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceVipLoanBorrow>> BorrowAsync(long loanAccountId, string loanCoin, decimal loanQuantity, long collateralAccountId, string collateralCoin, bool isFlexibleRate, int? loanTerm = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// VIP loan is available for VIP users only.
    /// <para><a href="https://developers.binance.com/docs/vip_loan/trade/VIP-Loan-Repay" /></para>
    /// </summary>
    /// <param name="orderId">Order Id</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceVipLoanRepay>> RepayAsync(string orderId, decimal quantity, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// VIP loan is available for VIP users only.
    /// <para><a href="https://developers.binance.com/docs/vip_loan/trade" /></para>
    /// </summary>
    /// <param name="orderId">Order Id</param>
    /// <param name="loanTerm">Loan Term</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceVipLoanRenew>> RenewAsync(string orderId, int loanTerm, int? receiveWindow = null, CancellationToken ct = default);
}
