namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Transfer Methods
/// </summary>
public interface IBinanceMarginRestClientTransfer
{
    /// <summary>
    /// Gets Cross Margin transfer history in descending order.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/transfer#get-cross-margin-transfer-history" /></para>
    /// </summary>
    /// <param name="direction">Optional transfer direction.</param>
    /// <param name="asset">Optional asset filter.</param>
    /// <param name="startTime">Optional period start. The explicit period cannot exceed 30 days.</param>
    /// <param name="endTime">Optional period end. The explicit period cannot exceed 30 days.</param>
    /// <param name="current">Page number, minimum 1. The server default is 1.</param>
    /// <param name="size">Page size, between 1 and 100. The server default is 10.</param>
    /// <param name="isolatedSymbol">Optional Isolated Margin symbol.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of transfers</returns>
    Task<RestCallResult<BinanceMarginTransferHistoryResult>> GetMarginTransfersAsync(
        BinanceMarginTransferDirection? direction = null,
        string? asset = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? current = null,
        long? size = null,
        string? isolatedSymbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Query max transfer-out quantity 
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/transfer#query-max-transfer-out-amount" /></para>
    /// </summary>
    /// <param name="asset">Asset, for example <c>ETH</c>.</param>
    /// <param name="isolatedSymbol">Optional Isolated Margin symbol. Cross Margin is used when omitted.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Max quantity</returns>
    Task<RestCallResult<decimal>> GetMarginMaxTransferAmountAsync(string asset, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default);
}
