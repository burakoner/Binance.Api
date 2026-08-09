namespace Binance.Api.Algo;

/// <summary>
/// Interface for the Binance Algo REST API Client Spot Methods
/// </summary>
public interface IBinanceAlgoRestClientSpot
{
    /// <summary>
    /// Place a new spot time weighted average price order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/advanced-trading-algo-trading/api/rest-api/spot-algo#time-weighted-average-price-spot-algo" /></para>
    /// </summary>
    /// <remarks>Binance permits at most 20 open Spot Algo orders</remarks>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="side">Order side</param>
    /// <param name="quantity">Order quantity. The maximum notional is symbol-dependent</param>
    /// <param name="duration">Duration in seconds, from 300 through 86400</param>
    /// <param name="clientAlgoId">Unique Algo order id containing exactly 32 characters. A value is generated when omitted</param>
    /// <param name="limitPrice">Limit price of the order. If null will use market price</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The submission result</returns>
    Task<RestCallResult<BinanceAlgoOrderResult>> PlaceTimeWeightedAveragePriceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        int duration,
        string? clientAlgoId = null,
        decimal? limitPrice = null,
        CancellationToken ct = default);

    /// <summary>
    /// Cancel a spot algo order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/advanced-trading-algo-trading/api/rest-api/spot-algo#cancel-algo-order-spot-algo" /></para>
    /// </summary>
    /// <param name="algoId">Algo order id to cancel</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoResult>> CancelAlgoOrderAsync(long algoId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get algo sub orders overview
    /// <para><a href="https://developers.binance.com/en/docs/catalog/advanced-trading-algo-trading/api/rest-api/spot-algo#query-sub-orders-spot-algo" /></para>
    /// </summary>
    /// <param name="algoId">Algo id</param>
    /// <param name="page">Page number. The server default is 1</param>
    /// <param name="pageSize">Records per page, between 1 and 100. The server default is 100</param>
    /// <param name="receiveWindow">The receive window for which this request is active, up to 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The sub orders and aggregate execution totals</returns>
    Task<RestCallResult<BinanceAlgoSubOrderList>> GetAlgoSubOrdersAsync(long algoId, long? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get all open spot algo orders
    /// <para><a href="https://developers.binance.com/en/docs/catalog/advanced-trading-algo-trading/api/rest-api/spot-algo#query-current-algo-open-orders-spot-algo" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active, up to 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The current open Spot Algo orders</returns>
    Task<RestCallResult<BinanceAlgoOrders>> GetOpenAlgoOrdersAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get historical spot algo orders
    /// <para><a href="https://developers.binance.com/en/docs/catalog/advanced-trading-algo-trading/api/rest-api/spot-algo#query-historical-algo-orders-spot-algo" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="side">Filter by side</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Page number. The server default is 1</param>
    /// <param name="pageSize">Records per page, between 1 and 100. The server default is 100</param>
    /// <param name="receiveWindow">The receive window for which this request is active, up to 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The historical Spot Algo orders</returns>
    Task<RestCallResult<BinanceAlgoOrders>> GetHistoricalAlgoOrdersAsync(string? symbol = null, BinanceOrderSide? side = null, DateTime? startTime = null, DateTime? endTime = null, long? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);
}
