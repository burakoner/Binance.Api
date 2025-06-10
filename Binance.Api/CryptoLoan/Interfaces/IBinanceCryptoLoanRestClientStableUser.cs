namespace Binance.Api.CryptoLoan;

/// <summary>
/// Interface for the Binance Algo REST API Crypto Loan > Stable Rate -> User Information Methods
/// </summary>
public interface IBinanceCryptoLoanRestClientStableUser
{
    /// <summary>
    /// Get borrow order history
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#borrow-get-loan-borrow-history-user_data" /></para>
    /// </summary>
    /// <param name="orderId">Filter by order id</param>
    /// <param name="loanAsset">Filter by loan asset</param>
    /// <param name="collateralAsset">Filter by collateral asset</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Page number</param>
    /// <param name="limit">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanStableBorrowRecord>>> GetBorrowHistoryAsync(long? orderId = null, string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get LTV adjustment history
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#adjust-ltv-get-loan-ltv-adjustment-history-user_data" /></para>
    /// </summary>
    /// <param name="orderId">Filter by order id</param>
    /// <param name="loanAsset">Filter by loan asset</param>
    /// <param name="collateralAsset">Filter by collateral asset</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Page number</param>
    /// <param name="limit">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanStableAdjustmentRecord>>> GetAdjustmentHistoryAsync(long? orderId = null, string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get loan repayment history
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#repay-get-loan-repayment-history-user_data" /></para>
    /// </summary>
    /// <param name="orderId">Filter by order id</param>
    /// <param name="loanAsset">Filter by loan asset</param>
    /// <param name="collateralAsset">Filter by collateral asset</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Page number</param>
    /// <param name="limit">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanStableRepayRecord>>> GetRepayHistoryAsync(long? orderId = null, string? loanAsset = null, string? collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}
