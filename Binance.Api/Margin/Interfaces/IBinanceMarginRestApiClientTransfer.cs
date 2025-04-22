using Binance.Api.Margin.Responses;

namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Transfer Methods
/// </summary>
public interface IBinanceMarginRestApiClientTransfer
{
    Task<RestCallResult<BinanceQueryRecords<BinanceTransferHistory>>> GetMarginTransferHistoryAsync(BinanceTransferDirection direction, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<decimal>> GetMarginMaxTransferAmountAsync(string asset, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default);
}