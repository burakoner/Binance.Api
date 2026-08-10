namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance USD Futures Convert endpoints
/// </summary>
public interface IBinanceFuturesRestClientUsdConvert
{
    /// <summary>
    /// Get list of convert symbols
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/convert#list-all-convert-pairs" /></para>
    /// </summary>
    /// <param name="fromAsset">From asset</param>
    /// <param name="toAsset">To asset</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<List<BinanceFuturesConvertSymbol>>> GetConvertSymbolsAsync(string? fromAsset = null, string? toAsset = null, CancellationToken ct = default);

    /// <summary>
    /// Get a convert quote
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/convert#send-quote-request" /></para>
    /// </summary>
    /// <param name="fromAsset">The from asset, for example `ETH`</param>
    /// <param name="toAsset">The to asset, for example `USD`</param>
    /// <param name="fromAmount">Amount debited after conversion. Either this or toAmount should be provided.</param>
    /// <param name="toAmount">Amount credited after conversion. Either this or fromAmount should be provided.</param>
    /// <param name="validTime">Quote validity duration. The current documentation specifies 10s and defaults to 10s.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <remarks>The current official generated connector does not enforce a fromAmount/toAmount combination constraint, so values are forwarded unchanged.</remarks>
    Task<RestCallResult<BinanceFuturesConvertQuote>> ConvertQuoteRequestAsync(string fromAsset, string toAsset, decimal? fromAmount = null, decimal? toAmount = null, string? validTime = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Accept a convert quote
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/convert#accept-the-offered-quote" /></para>
    /// </summary>
    /// <param name="quoteId">Quote id previously requested</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceFuturesConvertQuoteResult>> ConvertAcceptQuoteAsync(string quoteId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get status of a convert order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/convert#order-status" /></para>
    /// </summary>
    /// <param name="quoteId">The quote id. Either this or orderId should be provided</param>
    /// <param name="orderId">The order id. Either this or quoteId should be provided</param>
    /// <param name="ct">Cancellation token</param>
    /// <remarks>The current official generated connector does not enforce a quoteId/orderId combination constraint, so values are forwarded unchanged.</remarks>
    Task<RestCallResult<BinanceFuturesConvertStatus>> GetConvertOrderStatusAsync(string? quoteId = null, string? orderId = null, CancellationToken ct = default);
}
