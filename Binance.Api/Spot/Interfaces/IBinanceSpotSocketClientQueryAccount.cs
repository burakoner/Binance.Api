namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot Web Socket API Client Account Methods
/// </summary>
public interface IBinanceSpotSocketClientQueryAccount
{
    /// <summary>
    /// Gets account information, including balances
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#account-status" /></para>
    /// </summary>
    /// <param name="omitZeroBalances">When true only return non-zero balances in the account</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotAccount>> GetAccountAsync(bool? omitZeroBalances = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get order rate limit status
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#account-rate-limits-orders" /></para>
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<List<BinanceCurrentRateLimit>>> GetRateLimitsAsync(decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get order history
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#all-orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="orderId">Filter from order id</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max results</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<List<BinanceSpotOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get order-list history
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#all-order-lists" /></para>
    /// </summary>
    /// <param name="fromId">Filter from order-list id</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max results</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<List<BinanceSpotOrderList>>> GetOrderListsAsync(long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets user trades for provided symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#my-trades" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="orderId">Filter by order id</param>
    /// <param name="fromId">Filter from trade id</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="limit">Max results</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<List<BinanceSpotUserTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default);
    
    /// <summary>
    /// Get prevented trades because of self trade prevention
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#my-prevented-matches" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="preventedMatchId">Filter by prevented match id</param>
    /// <param name="orderId">Filter by order id</param>
    /// <param name="fromPreventedMatchId">Filter from prevented id</param>
    /// <param name="limit">Max results</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<List<BinanceSpotPreventedTrade>>> GetPreventedTradesAsync(string symbol, long? preventedMatchId = null, long? orderId = null, long? fromPreventedMatchId = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>Gets one order list by Binance or client identifier.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#order-list-status" /></para>
    /// </summary>
    Task<CallResult<BinanceSpotOrderList>> GetOrderListAsync(long? orderListId = null, string? originalClientOrderId = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>Gets all currently open order lists.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#open-order-lists-status" /></para>
    /// </summary>
    Task<CallResult<List<BinanceSpotOrderList>>> GetOpenOrderListsAsync(decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>Gets allocations resulting from Smart Order Routing.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#my-allocations" /></para>
    /// </summary>
    Task<CallResult<List<BinanceSpotAllocation>>> GetAllocationsAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, long? fromAllocationId = null, int? limit = null, long? orderId = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>Gets current commission rates for a symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#account-commission" /></para>
    /// </summary>
    Task<CallResult<BinanceSpotCommissionRates>> GetCommissionRatesAsync(string symbol, CancellationToken ct = default);

    /// <summary>Gets all amendments of an order.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#order-amendments" /></para>
    /// </summary>
    Task<CallResult<List<BinanceSpotOrderAmendment>>> GetOrderAmendmentsAsync(string symbol, long orderId, long? fromExecutionId = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>Gets exchange, symbol, and asset filters relevant to this account for a symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#my-filters" /></para>
    /// </summary>
    Task<CallResult<BinanceSpotAccountFilters>> GetAccountFiltersAsync(string symbol, decimal? receiveWindow = null, CancellationToken ct = default);
}
