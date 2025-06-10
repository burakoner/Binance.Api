namespace Binance.Api.CryptoLoan;

/// <summary>
/// Interface for the Binance Algo REST API Crypto Loan > Flexible Rate -> Market Data Methods
/// </summary>
public interface IBinanceCryptoLoanRestClientFlexibleMarketData
{
    /// <summary>
    /// Get LTV information and collateral limit of flexible loan's collateral assets. The collateral limit is shown in USD value.
    /// <para><a href="https://developers.binance.com/docs/crypto_loan/flexible-rate/market-data" /></para>
    /// </summary>
    /// <param name="asset">Collateral Coin</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleCollateralAsset>>> GetCollateralAssetsAsync(string asset, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Check Flexible Loan interest rate history
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="current">Check current querying page, start from 1. Default：1；Max：1000.</param>
    /// <param name="limit">Default：10; Max：100.</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleInterestRate>>> GetInterestRateHistoryAsync(string asset, DateTime? startTime = null, DateTime? endTime = null, int? current = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get interest rate and borrow limit of flexible loanable assets. The borrow limit is shown in USD value.
    /// <para><a href="https://developers.binance.com/docs/crypto_loan/flexible-rate/market-data/Get-Flexible-Loan-Assets-Data" /></para>
    /// </summary>
    /// <param name="asset">Loan Coin</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceCryptoLoanFlexibleAsset>>> GetLoanableAssetsAsync(string asset, int? receiveWindow = null, CancellationToken ct = default);
}
