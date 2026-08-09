namespace Binance.Api.Convert;

/// <summary>
/// Interface for the Binance Convert Trade Rest API client.
/// </summary>
public interface IBinanceConvertRestClientTrade
{
    /// <summary>
    /// Request a quote for convert asset (selling asset) for base asset (buying asset)
    /// <para><a href="https://developers.binance.com/docs/convert/trade" /></para>
    /// </summary>
    /// <param name="fromAsset">Quote asset, for example `ETH`</param>
    /// <param name="toAsset">Base asset, for example `ETH`</param>
    /// <param name="fromAmount">Quote quantity</param>
    /// <param name="toAmount">Quote quantity</param>
    /// <param name="walletType">The wallet type for convert</param>
    /// <param name="validTime">The valid time for quote</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceConvertQuote>> QuoteRequestAsync(string fromAsset, string toAsset, decimal? fromAmount = null, decimal? toAmount = null, BinanceConvertWalletType? walletType = null, BinanceConvertValidTime? validTime = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Accept the previously requested quote
    /// <para><a href="https://developers.binance.com/docs/convert/trade/Accept-Quote" /></para>
    /// </summary>
    /// <param name="quoteId">The quote id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
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
    /// <para><a href="https://developers.binance.com/docs/convert/trade/Place-Order" /></para>
    /// </summary>
    /// <param name="baseAsset">Base asset</param>
    /// <param name="quoteAsset">quote asset</param>
    /// <param name="limitPrice">Symbol limit price (from baseAsset to quoteAsset)</param>
    /// <param name="side">BUY or SELL</param>
    /// <param name="expiredType">1_D, 3_D, 7_D, 30_D (D means day)</param>
    /// <param name="baseAmount">Base asset amount. (One of baseAmount or quoteAmount is required)</param>
    /// <param name="quoteAmount">Quote asset amount. (One of baseAmount or quoteAmount is required)</param>
    /// <param name="walletType">SPOT or FUNDING or SPOT_FUNDING. It is to use which type of assets. Default is SPOT.</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
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
    /// <para><a href="https://developers.binance.com/docs/convert/trade/Cancel-Order" /></para>
    /// </summary>
    /// <param name="orderId">The orderId from placeOrder api</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceConvertLimitOrderStatus>> CancelLimitOrderAsync(string orderId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query current open limit orders
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-convert/api/rest-api/trade#query-limit-open-orders" /></para>
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot exceed 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceConvertOpenOrder>>> GetOpenLimitOrdersAsync(int? receiveWindow = null, CancellationToken ct = default);
}
