using Binance.Api.Wallet;

namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Borrow and Repay Methods
/// </summary>
public interface IBinanceMarginRestClientBorrowRepay
{
    /// <summary>
    /// Get futures hourly interest rate
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-a-future-hourly-interest-rate-user_data" /></para>
    /// </summary>
    /// <param name="assets">Assets, for example `ETH`</param>
    /// <param name="isolated">Isolated or cross</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceMarginInterestRate>>> GetFutureHourlyInterestRateAsync(IEnumerable<string> assets, bool isolated, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get history of interest
    /// <para><a href="https://developers.binance.com/docs/margin_trading/borrow-and-repay/Get-Interest-History" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="page">Results page</param>
    /// <param name="startTime">Filter by startTime from</param>
    /// <param name="endTime">Filter by endTime from</param>
    /// <param name="isolatedSymbol">Filter by isolated symbol</param>
    /// <param name="limit">Limit of the amount of results</param>
    /// <param name="archived">Set to true for archived data from 6 months ago</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of interest events</returns>
    Task<RestCallResult<BinanceQueryRecords<BinanceMarginInterestHistory>>> GetMarginInterestHistoryAsync(string? asset = null, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, bool? archived = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Borrow. Apply for a loan. 
    /// <para><a href="https://developers.binance.com/docs/margin_trading/borrow-and-repay/Margin-Account-Borrow-Repay" /></para>
    /// </summary>
    /// <param name="asset">The asset being borrow, for example `ETH`</param>
    /// <param name="quantity">The quantity to be borrow</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="symbol">The isolated symbol</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Transaction Id</returns>
    Task<RestCallResult<BinanceWalletTransaction>> BorrowAsync(string asset, decimal quantity, bool? isIsolated = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Repay loan for margin account.
    /// <para><a href="https://developers.binance.com/docs/margin_trading/borrow-and-repay/Margin-Account-Borrow-Repay" /></para>
    /// </summary>
    /// <param name="asset">The asset being repay, for example `ETH`</param>
    /// <param name="quantity">The quantity to be borrow</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="symbol">The isolated symbol</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Transaction Id</returns>
    Task<RestCallResult<BinanceWalletTransaction>> RepayAsync(string asset, decimal quantity, bool? isIsolated = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query loan records
    /// <para><a href="https://developers.binance.com/docs/margin_trading/borrow-and-repay/Query-Borrow-Repay" /></para>
    /// </summary>
    /// <param name="asset">The records asset, for example `ETH`</param>
    /// <param name="transactionId">The id of loan transaction</param>
    /// <param name="startTime">Time to start getting records from</param>
    /// <param name="endTime">Time to stop getting records to</param>
    /// <param name="current">Number of page records</param>
    /// <param name="isolatedSymbol">Filter by isolated symbol</param>
    /// <param name="limit">The records count size need show</param>
    /// <param name="archived">Set to true for archived data from 6 months ago</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Loan records</returns>
    Task<RestCallResult<BinanceQueryRecords<BinanceLoan>>> GetMarginLoansAsync(string asset, long? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? current = 1, int? limit = 10, string? isolatedSymbol = null, bool? archived = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get history of interest rate
    /// <para><a href="https://developers.binance.com/docs/margin_trading/borrow-and-repay/Query-Margin-Interest-Rate-History" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="vipLevel">Vip level</param>
    /// <param name="startTime">Filter by startTime from</param>
    /// <param name="endTime">Filter by endTime from</param>
    /// <param name="limit">Limit of the amount of results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of interest rate</returns>
    Task<RestCallResult<List<BinanceMarginInterestRateHistory>>> GetMarginInterestRateHistoryAsync(string asset, string? vipLevel = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query max borrow quantity
    /// <para><a href="https://developers.binance.com/docs/margin_trading/borrow-and-repay/Query-Max-Borrow" /></para>
    /// </summary>
    /// <param name="asset">The records asset, for example `ETH`</param>
    /// <param name="isolatedSymbol">The isolated symbol</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Max quantity</returns>
    Task<RestCallResult<BinanceMarginAmount>> GetMarginMaxBorrowAmountAsync(string asset, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default);
}