using Binance.Api.Margin.Responses;
using Binance.Api.Wallet;
using Binance.Net.Objects.Models.Spot.Margin;

namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Borrow and Repay Methods
/// </summary>
public interface IBinanceMarginRestApiClientBorrowRepay
{
    Task<RestCallResult<IEnumerable<BinanceMarginInterestRate>>> GetFutureHourlyInterestRateAsync(IEnumerable<string> assets, bool isolated, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceQueryRecords<BinanceInterestHistory>>> GetMarginInterestHistoryAsync(string? asset = null, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, bool? archived = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceTransaction>> MarginBorrowAsync(string asset, decimal quantity, bool? isIsolated = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceTransaction>> MarginRepayAsync(string asset, decimal quantity, bool? isIsolated = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceQueryRecords<BinanceLoan>>> GetMarginLoansAsync(string asset, long? transactionId = null, DateTime? startTime = null, DateTime? endTime = null, int? current = 1, int? limit = 10, string? isolatedSymbol = null, bool? archived = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceInterestRateHistory>>> GetMarginInterestRateHistoryAsync(string asset, string? vipLevel = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceMarginAmount>> GetMarginMaxBorrowAmountAsync(string asset, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default);
}