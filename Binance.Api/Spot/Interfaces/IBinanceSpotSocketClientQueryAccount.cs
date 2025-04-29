namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot Web Socket API Client Account Methods
/// </summary>
public interface IBinanceSpotSocketClientQueryAccount
{
    /// <summary>
    /// Gets account information, including balances
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/account-requests" /></para>
    /// </summary>
    /// <param name="omitZeroBalances">When true only return non-zero balances in the account</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotAccount>> GetAccountAsync(bool? omitZeroBalances = null, CancellationToken ct = default);

    /// <summary>
    /// Get order rate limit status
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/account-requests#unfilled-order-count-user_data" /></para>
    /// </summary>
    /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceCurrentRateLimit>>> GetOrderRateLimitsAsync(IEnumerable<string>? symbols = null, CancellationToken ct = default);

    /// <summary>
    /// Get order history
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/account-requests#account-order-history-user_data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="fromOrderId">Filter from order id</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotOrder>>> GetOrdersAsync(string symbol, long? fromOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Get Oco order history
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/trading-requests#order-lists" /></para>
    /// </summary>
    /// <param name="fromOrderId">Filter from order id</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceOrderOcoList>>> GetOcoOrdersAsync(long? fromOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets user trades for provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/account-requests#account-trade-history-user_data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="orderId">Filter by order id</param>
    /// <param name="fromOrderId">Filter from order id</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotUserTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, long? fromOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default);
    
    /// <summary>
    /// Get prevented trades because of self trade prevention
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/account-requests#account-prevented-matches-user_data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="preventedTradeId">Filter by prevented trade id</param>
    /// <param name="orderId">Filter by order id</param>
    /// <param name="fromPreventedTradeId">Filter from prevented id</param>
    /// <param name="limit">Max results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<IEnumerable<BinanceSpotPreventedTrade>>> GetPreventedTradesAsync(string symbol, long? preventedTradeId = null, long? orderId = null, long? fromPreventedTradeId = null, int? limit = null, CancellationToken ct = default);
}