using Binance.Api.Wallet;

namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Borrow and Repay Methods
/// </summary>
public interface IBinanceMarginRestClientBorrowRepay
{
    /// <summary>
    /// Get the next hourly interest rate for one or more assets.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/borrow-repay#get-future-hourly-interest-rate" /></para>
    /// </summary>
    /// <param name="assets">Assets to query. Between 1 and 20 assets can be provided.</param>
    /// <param name="isolated">Whether to query Isolated Margin instead of Cross Margin.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginInterestRate>>> GetFutureHourlyInterestRateAsync(
        IEnumerable<string> assets,
        bool isolated,
        CancellationToken ct = default);

    /// <summary>
    /// Get Margin interest history.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/borrow-repay#get-interest-history" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset.</param>
    /// <param name="isolatedSymbol">Filter by Isolated Margin symbol.</param>
    /// <param name="startTime">Start of the requested period.</param>
    /// <param name="endTime">End of the requested period. An explicit range cannot exceed 30 days.</param>
    /// <param name="current">Page number, starting at 1.</param>
    /// <param name="size">Page size, up to 100.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The maximum is 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginInterestHistoryResult>> GetMarginInterestHistoryAsync(
        string? asset = null,
        string? isolatedSymbol = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? current = null,
        long? size = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Borrow an asset for Cross or Isolated Margin.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/borrow-repay#margin-account-borrow-repay" /></para>
    /// </summary>
    /// <param name="asset">Asset to borrow.</param>
    /// <param name="quantity">Quantity to borrow.</param>
    /// <param name="isIsolated">Whether this is an Isolated Margin operation.</param>
    /// <param name="symbol">Required for Isolated Margin and unsupported for Cross Margin.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The maximum is 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceWalletTransaction>> BorrowAsync(
        string asset,
        decimal quantity,
        bool isIsolated = false,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Repay an asset for Cross or Isolated Margin.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/borrow-repay#margin-account-borrow-repay" /></para>
    /// </summary>
    /// <param name="asset">Asset to repay.</param>
    /// <param name="quantity">Quantity to repay.</param>
    /// <param name="isIsolated">Whether this is an Isolated Margin operation.</param>
    /// <param name="symbol">Required for Isolated Margin and unsupported for Cross Margin.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The maximum is 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceWalletTransaction>> RepayAsync(
        string asset,
        decimal quantity,
        bool isIsolated = false,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get borrow or repay history.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/borrow-repay#query-borrow-repay-records-in-margin-account" /></para>
    /// </summary>
    /// <param name="type">Whether to query borrow or repay records.</param>
    /// <param name="asset">Filter by asset.</param>
    /// <param name="isolatedSymbol">Filter by Isolated Margin symbol.</param>
    /// <param name="transactionId">Filter by transaction id. This takes precedence over the time range.</param>
    /// <param name="startTime">Start of the requested period.</param>
    /// <param name="endTime">End of the requested period.</param>
    /// <param name="current">Page number, starting at 1.</param>
    /// <param name="size">Page size, up to 100.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The maximum is 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginBorrowRepayHistory>> GetMarginBorrowRepayHistoryAsync(
        BinanceMarginBorrowRepayType type,
        string? asset = null,
        string? isolatedSymbol = null,
        long? transactionId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? current = null,
        long? size = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get Margin interest-rate history.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/borrow-repay#query-margin-interest-rate-history" /></para>
    /// </summary>
    /// <param name="asset">Asset to query.</param>
    /// <param name="vipLevel">VIP level to query.</param>
    /// <param name="startTime">Start of the requested period.</param>
    /// <param name="endTime">End of the requested period. An explicit range cannot exceed 30 days.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The maximum is 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginInterestRateHistory>>> GetMarginInterestRateHistoryAsync(
        string asset,
        long? vipLevel = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Query the maximum borrowable quantity.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/borrow-repay#query-max-borrow" /></para>
    /// </summary>
    /// <param name="asset">Asset to query.</param>
    /// <param name="isolatedSymbol">Isolated Margin symbol.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The maximum is 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginAmount>> GetMarginMaxBorrowAmountAsync(
        string asset,
        string? isolatedSymbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default);
}
