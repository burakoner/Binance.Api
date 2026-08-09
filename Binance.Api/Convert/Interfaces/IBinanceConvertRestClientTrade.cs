namespace Binance.Api.Convert;

/// <summary>
/// Interface for the Binance Convert Trade Rest API client.
/// </summary>
public interface IBinanceConvertRestClientTrade
{
    /// <summary>
    /// Request a quote for a Convert token pair. A quote id is returned only when the account has enough funds to convert.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-convert/api/rest-api/trade#send-quote-request" /></para>
    /// </summary>
    /// <param name="fromAsset">Source asset, for example BTC</param>
    /// <param name="toAsset">Destination asset, for example USDT</param>
    /// <param name="fromAmount">Amount debited after conversion. Exactly one of fromAmount or toAmount must be provided.</param>
    /// <param name="toAmount">Amount credited after conversion. Exactly one of fromAmount or toAmount must be provided.</param>
    /// <param name="walletType">Wallet or wallet combination used for payment. The server default is SPOT.</param>
    /// <param name="validTime">Quote validity duration. The server default is 10 seconds.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceConvertQuote>> QuoteRequestAsync(string fromAsset, string toAsset, decimal? fromAmount = null, decimal? toAmount = null, BinanceConvertWalletType? walletType = null, BinanceConvertValidTime? validTime = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Accept the offered quote and execute the conversion
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-convert/api/rest-api/trade#accept-quote" /></para>
    /// </summary>
    /// <param name="quoteId">The quote id to accept</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceConvertResult>> AcceptQuoteAsync(string quoteId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get convert trade history
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-convert/api/rest-api/trade#get-convert-trade-history" /></para>
    /// </summary>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time. The interval from startTime cannot exceed 30 days.</param>
    /// <param name="limit">Maximum number of results. The value cannot exceed 1000; the server default is 100.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceListRangeResponse<BinanceConvertTrade>>> GetHistoryAsync(DateTime startTime, DateTime endTime, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get convert order status
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-convert/api/rest-api/trade#order-status" /></para>
    /// </summary>
    /// <param name="orderId">The order id. Exactly one of orderId or quoteId must be provided.</param>
    /// <param name="quoteId">The quote id. Exactly one of orderId or quoteId must be provided.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceConvertStatus>> GetStatusAsync(string? orderId = null, string? quoteId = null, CancellationToken ct = default);

    /// <summary>
    /// Enable users to place a limit order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-convert/api/rest-api/trade#place-limit-order" /></para>
    /// </summary>
    /// <param name="baseAsset">Base asset</param>
    /// <param name="quoteAsset">Quote asset</param>
    /// <param name="limitPrice">Symbol limit price (from baseAsset to quoteAsset)</param>
    /// <param name="side">Order side</param>
    /// <param name="expiredType">Order expiry duration</param>
    /// <param name="baseAmount">Base asset amount. Exactly one of baseAmount or quoteAmount must be provided.</param>
    /// <param name="quoteAmount">Quote asset amount. Exactly one of baseAmount or quoteAmount must be provided.</param>
    /// <param name="walletType">Wallet or wallet combination used for payment. The server default is SPOT.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <remarks>The current exchangeInfo response does not expose the fromIsBase field referenced by the Trade documentation. This client does not infer or reorder baseAsset and quoteAsset.</remarks>
    /// <exception cref="ArgumentException"></exception>
    Task<RestCallResult<BinanceConvertLimitOrder>> PlaceLimitOrderAsync(string baseAsset, string quoteAsset,
       decimal limitPrice,
       BinanceOrderSide side,
       BinanceConvertExpiredTime expiredType,
       decimal? baseAmount = null,
       decimal? quoteAmount = null,
       BinanceConvertWalletType? walletType = null,
       int? receiveWindow = null,
       CancellationToken ct = default);

    /// <summary>
    /// Enable users to cancel a limit order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-convert/api/rest-api/trade#cancel-limit-order" /></para>
    /// </summary>
    /// <param name="orderId">The order id returned by PlaceLimitOrderAsync</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceConvertLimitOrderStatus>> CancelLimitOrderAsync(long orderId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query current open limit orders
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-convert/api/rest-api/trade#query-limit-open-orders" /></para>
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceConvertOpenOrder>>> GetOpenLimitOrdersAsync(int? receiveWindow = null, CancellationToken ct = default);
}
