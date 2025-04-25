namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Transfer Methods
/// </summary>
public interface IBinanceMarginRestClientTransfer
{
    /// <summary>
    /// Get history of transfers
    /// <para><a href="https://developers.binance.com/docs/margin_trading/transfer" /></para>
    /// </summary>
    /// <param name="direction">The direction of the the transfers to retrieve</param>
    /// <param name="page">Results page</param>
    /// <param name="startTime">Filter by startTime from</param>
    /// <param name="endTime">Filter by endTime from</param>
    /// <param name="limit">Limit of the amount of results</param>
    /// <param name="isolatedSymbol">Filter by isolated symbol</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of transfers</returns>
    Task<RestCallResult<BinanceQueryRecords<BinanceTransferHistory>>> GetMarginTransferHistoryAsync(BinanceTransferDirection direction, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query max transfer-out quantity 
    /// <para><a href="https://developers.binance.com/docs/margin_trading/transfer/Query-Max-Transfer-Out-Amount" /></para>
    /// </summary>
    /// <param name="asset">The records asset, for example `ETH`</param>
    /// <param name="isolatedSymbol">The isolated symbol</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Max quantity</returns>
    Task<RestCallResult<decimal>> GetMarginMaxTransferAmountAsync(string asset, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default);
}