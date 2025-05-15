namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance USD Futures Convert endpoints
/// </summary>
public interface IBinanceFuturesRestClientUsdConvert
{
    /// <summary>
    /// Get list of convert symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/convert" /></para>
    /// </summary>
    /// <param name="fromAsset">From asset</param>
    /// <param name="toAsset">To asset</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<List<BinanceFuturesConvertSymbol>>> GetConvertSymbolsAsync(string? fromAsset = null, string? toAsset = null, CancellationToken ct = default);

    /// <summary>
    /// Get a convert quote
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/convert/Send-quote-request" /></para>
    /// </summary>
    /// <param name="fromAsset">The from asset, for example `ETH`</param>
    /// <param name="toAsset">The to asset, for example `USD`</param>
    /// <param name="fromQuantity">The from asset quantity, either this or toQuantity should be provided</param>
    /// <param name="toQuantity">The to asset quantity, either this or fromQuantity should be provided</param>
    /// <param name="validTime">The time the quote should be valid for</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceFuturesConvertQuote>> ConvertQuoteRequestAsync(string fromAsset, string toAsset, decimal? fromQuantity = null, decimal? toQuantity = null, BinanceValidTime? validTime = null, CancellationToken ct = default);

    /// <summary>
    /// Accept a convert quote
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/convert/Accept-Quote" /></para>
    /// </summary>
    /// <param name="quoteId">Quote id previously requested</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceFuturesConvertQuoteResult>> ConvertAcceptQuoteAsync(string quoteId, CancellationToken ct = default);

    /// <summary>
    /// Get status of a convert order
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/convert/Order-Status" /></para>
    /// </summary>
    /// <param name="quoteId">The quote id. Either this or orderId should be provided</param>
    /// <param name="orderId">The order id. Either this or quoteId should be provided</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceFuturesConvertStatus>> GetConvertOrderStatusAsync(string? quoteId = null, string? orderId = null, CancellationToken ct = default);
}