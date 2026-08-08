namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot REST API Client Account Methods
/// </summary>
public interface IBinanceSpotRestClientAccount
{
    /// <summary>
    /// Gets account information, including balances
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/account#get-account" /></para>
    /// </summary>
    /// <param name="omitZeroBalances">When true only return non-zero balances in the account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The account information</returns>
    Task<RestCallResult<BinanceSpotAccount>> GetAccountAsync(bool? omitZeroBalances = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets user trades for provided symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/account#my-trades" /></para>
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
    Task<RestCallResult<List<BinanceSpotUserTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the current used order rate limits
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/account#rate-limit-order" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceSpotOrderRateLimit>>> GetRateLimitsAsync(decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get prevented matches because of self trade prevention
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/account#my-prevented-matches" /></para>
    /// </summary>
    /// <param name="symbol">Symbol, for example `ETHUSDT`</param>
    /// <param name="preventedMatchId">Filter by prevented match id</param>
    /// <param name="orderId">Filter by order id</param>
    /// <param name="fromPreventedMatchId">Filter by min prevented match id</param>
    /// <param name="limit">Max results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceSpotPreventedTrade>>> GetPreventedTradesAsync(string symbol, long? orderId = null, long? preventedMatchId = null, long? fromPreventedMatchId = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets one order list by Binance or client identifier.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/account#get-order-list" /></para>
    /// </summary>
    Task<RestCallResult<BinanceSpotOrderList>> GetOrderListAsync(long? orderListId = null, string? originalClientOrderId = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets order-list history.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/account#all-order-list" /></para>
    /// </summary>
    Task<RestCallResult<List<BinanceSpotOrderList>>> GetOrderListsAsync(long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets all currently open order lists.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/account#open-order-list" /></para>
    /// </summary>
    Task<RestCallResult<List<BinanceSpotOrderList>>> GetOpenOrderListsAsync(decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets allocations resulting from Smart Order Routing.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/account#my-allocations" /></para>
    /// </summary>
    Task<RestCallResult<List<BinanceSpotAllocation>>> GetAllocationsAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, long? fromAllocationId = null, int? limit = null, long? orderId = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets current commission rates for a symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/account#account-commission" /></para>
    /// </summary>
    Task<RestCallResult<BinanceSpotCommissionRates>> GetCommissionRatesAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets all amendments of an order.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/account#order-amendments" /></para>
    /// </summary>
    Task<RestCallResult<List<BinanceSpotOrderAmendment>>> GetOrderAmendmentsAsync(string symbol, long orderId, long? fromExecutionId = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets exchange, symbol, and asset filters relevant to this account for a symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/account#my-filters" /></para>
    /// </summary>
    Task<RestCallResult<BinanceSpotAccountFilters>> GetAccountFiltersAsync(string symbol, decimal? receiveWindow = null, CancellationToken ct = default);
}
