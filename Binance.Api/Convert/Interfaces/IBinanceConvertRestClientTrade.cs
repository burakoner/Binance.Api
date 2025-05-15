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
    /// <para><a href="https://developers.binance.com/docs/convert/trade/Get-Convert-Trade-History" /></para>
    /// </summary>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max amount of results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceListResult<BinanceConvertTrade>>> GetHistoryAsync(DateTime startTime, DateTime endTime, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get convert order status
    /// <para><a href="https://developers.binance.com/docs/convert/trade/Order-Status" /></para>
    /// </summary>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="quoteId">The quote id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceConvertStatus>> GetStatusAsync(string? orderId = null, string? quoteId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Enable users to place a limit order
    /// <para><a href="https://developers.binance.com/docs/convert/trade/Place-Order" /></para>
    /// </summary>
    /// <param name="baseAsset">base asset (use the response fromIsBase from GET /sapi/v1/convert/exchangeInfo api to check which one is baseAsset )</param>
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
    /// Request a quote for the requested token pairs
    /// <para><a href="https://developers.binance.com/docs/convert/trade/Query-Order" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceConvertLimitOrder>>> GetLimitOrdersAsync(int? receiveWindow = null, CancellationToken ct = default);
}