namespace Binance.Api.VipLoan;

/// <summary>
/// Interface for the Binance VIP Loan REST API Client -> Market Data Methods
/// </summary>
public interface IBinanceVipLoanRestClientMarketData
{
    /// <summary>
    /// Get Borrow Interest Rate
    /// <para><a href="https://developers.binance.com/docs/vip_loan/market-data" /></para>
    /// </summary>
    /// <param name="asset">Max 10 assets, Multiple split by ","</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceVipLoanInterestRate>>> GetInterestRateAsync(string asset, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get VIP Loan Interest Rate History (USER_DATA)
    /// <para><a href="https://developers.binance.com/docs/vip_loan/market-data/Get-VIP-Loan-Interest-Rate-History" /></para>
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="current">Check current querying page, start from 1. Default：1；Max：1000.</param>
    /// <param name="limit">Default：10; Max：100.</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceVipLoanInterestRate>>> GetInterestRateHistoryAsync(string asset, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get interest rate and borrow limit of loanable assets. The borrow limit is shown in USD value.
    /// <para><a href="https://developers.binance.com/docs/vip_loan/market-data/Get-Loanable-Assets-Data" /></para>
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="vipLevel">VIP Level</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceVipLoanLoanableAsset>>> GetLoanableAssetsAsync(string? asset = null, int? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Collateral Asset Data
    /// <para><a href="https://developers.binance.com/docs/vip_loan/market-data/Get-Collateral-Asset-Data" /></para>
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceVipLoanCollateralAsset>>> GetCollateralAssetsAsync(string asset, int? receiveWindow = null, CancellationToken ct = default);
}
