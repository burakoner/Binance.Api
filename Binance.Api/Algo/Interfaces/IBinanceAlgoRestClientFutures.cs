namespace Binance.Api.Algo;

/// <summary>
/// Interface for the Binance Algo REST API Client Futures Methods
/// </summary>
public interface IBinanceAlgoRestClientFutures
{
    /// <summary>
    /// Place a new Volume Participation order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/advanced-trading-algo-trading/api/rest-api/future-algo#volume-participation-future-algo" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="side">Order side</param>
    /// <param name="quantity">Order quantity. The notional must be greater than 10,000 USDT and less than 1,000,000 USDT</param>
    /// <param name="urgency">Relative execution speed: LOW, MEDIUM, or HIGH</param>
    /// <param name="clientAlgoId">Unique Algo order id containing exactly 32 characters. A value is generated when omitted</param>
    /// <param name="reduceOnly">Reduce only. Cannot be sent in Hedge Mode or when opening a position</param>
    /// <param name="limitPrice">Limit price of the order. If null will use market price</param>
    /// <param name="positionSide">BOTH for One-way Mode; LONG or SHORT for Hedge Mode. Required in Hedge Mode</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The submission result. A successful submission does not guarantee execution; query the order status for the final outcome</returns>
    Task<RestCallResult<BinanceAlgoOrderResult>> PlaceVolumeParticipationOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        BinanceUrgency urgency,
        string? clientAlgoId = null,
        bool? reduceOnly = null,
        decimal? limitPrice = null,
        BinancePositionSide? positionSide = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Place a new Time Weighted Average Price order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/advanced-trading-algo-trading/api/rest-api/future-algo#time-weighted-average-price-future-algo" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="side">Order side</param>
    /// <param name="quantity">Order quantity. The notional must be greater than 1,000 USDT and less than 1,000,000 USDT</param>
    /// <param name="duration">Duration in seconds, from 300 through 86400</param>
    /// <param name="clientAlgoId">Unique Algo order id containing exactly 32 characters. A value is generated when omitted</param>
    /// <param name="reduceOnly">Reduce only. Cannot be sent in Hedge Mode or when opening a position</param>
    /// <param name="limitPrice">Limit price of the order. If null will use market price</param>
    /// <param name="positionSide">BOTH for One-way Mode; LONG or SHORT for Hedge Mode. Required in Hedge Mode</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The submission result. A successful submission does not guarantee execution; query the order status for the final outcome</returns>
    Task<RestCallResult<BinanceAlgoOrderResult>> PlaceTimeWeightedAveragePriceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        int duration,
        string? clientAlgoId = null,
        bool? reduceOnly = null,
        decimal? limitPrice = null,
        BinancePositionSide? positionSide = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Cancel an algo order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/advanced-trading-algo-trading/api/rest-api/future-algo#cancel-algo-order-future-algo" /></para>
    /// </summary>
    /// <param name="algoId">Algo id to cancel</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoResult>> CancelAlgoOrderAsync(long algoId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get algo sub orders overview
    /// <para><a href="https://developers.binance.com/docs/algo/future-algo/Query-Sub-Orders" /></para>
    /// </summary>
    /// <param name="algoId">Algo id</param>
    /// <param name="page">Page</param>
    /// <param name="limit">Max results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoSubOrderList>> GetAlgoSubOrdersAsync(long algoId, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get list of open algo orders
    /// <para><a href="https://developers.binance.com/docs/algo/future-algo/Query-Current-Algo-Open-Orders" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoOrders>> GetOpenAlgoOrdersAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get list of closed algo orders
    /// <para><a href="https://developers.binance.com/docs/algo/future-algo/Query-Historical-Algo-Orders" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="side">Filter by side</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Page</param>
    /// <param name="limit">Max results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoOrders>> GetClosedAlgoOrdersAsync(string? symbol = null, BinanceOrderSide? side = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}
