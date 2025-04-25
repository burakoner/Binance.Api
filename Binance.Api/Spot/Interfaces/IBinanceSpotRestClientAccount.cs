namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot REST API Client Account Methods
/// </summary>
public interface IBinanceSpotRestClientAccount
{
    /// <summary>
    /// Gets account information, including balances
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/account-endpoints" /></para>
    /// </summary>
    /// <param name="omitZeroBalances">When true only return non-zero balances in the account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The account information</returns>
    Task<RestCallResult<BinanceSpotAccount>> GetAccountAsync(bool? omitZeroBalances = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets user trades for provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/account-endpoints#account-trade-list-user_data" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get trades for, for example `ETHUSDT`</param>
    /// <param name="orderId">Get trades for this order id</param>
    /// <param name="limit">The max number of results</param>
    /// <param name="fromId">TradeId to fetch from. Default gets most recent trades</param>
    /// <param name="startTime">Orders newer than this date will be retrieved</param>
    /// <param name="endTime">Orders older than this date will be retrieved</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of trades</returns>
    Task<RestCallResult<IEnumerable<BinanceSpotUserTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the current used order rate limits
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/account-endpoints#query-unfilled-order-count-user_data" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<IEnumerable<BinanceOrderRateLimit>>> GetOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get prevented matches because of self trade prevention
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/account-endpoints#query-prevented-matches-user_data" /></para>
    /// </summary>
    /// <param name="symbol">Symbol, for example `ETHUSDT`</param>
    /// <param name="preventedMatchId">Filter by prevented match id</param>
    /// <param name="orderId">Filter by order id</param>
    /// <param name="fromPreventedMatchId">Filter by min prevented match id</param>
    /// <param name="limit">Max results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<IEnumerable<BinancePreventedTrade>>> GetPreventedTradesAsync(string symbol, long? orderId = null, long? preventedMatchId = null, long? fromPreventedMatchId = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Query Allocations (USER_DATA)
    // TODO: Query Commission Rates (USER_DATA)
    // TODO: Query Order Amendments (USER_DATA)
}