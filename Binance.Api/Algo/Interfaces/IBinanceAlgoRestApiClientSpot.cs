namespace Binance.Api.Algo;

/// <summary>
/// Interface for the Binance Algo REST API Client Spot Methods
/// </summary>
public interface IBinanceAlgoRestApiClientSpot
{
    /// <summary>
    /// Place a new spot time weighted average price order
    /// <para><a href="https://developers.binance.com/docs/algo/spot-algo/Time-Weighted-Average-Price-New-Order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="side">Order side</param>
    /// <param name="quantity">Order quantity</param>
    /// <param name="duration">Duration in seconds. 300 - 86400</param>
    /// <param name="clientOrderId">Client order id</param>
    /// <param name="limitPrice">Limit price of the order. If null will use market price</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoOrderResult>> PlaceTimeWeightedAveragePriceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        int duration,
        string? clientOrderId = null,
        decimal? limitPrice = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Cancel a spot algo order
    /// <para><a href="https://developers.binance.com/docs/algo/spot-algo/Cancel-Algo-Order" /></para>
    /// </summary>
    /// <param name="algoOrderId">Algo order id to cancel</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoResult>> CancelAlgoOrderAsync(long algoOrderId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get algo sub orders overview
    /// <para><a href="https://developers.binance.com/docs/algo/spot-algo/Query-Sub-Orders" /></para>
    /// </summary>
    /// <param name="algoId">Algo id</param>
    /// <param name="page">Page</param>
    /// <param name="limit">Max results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoSubOrderList>> GetAlgoSubOrdersAsync(long algoId, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get all open spot algo orders
    /// <para><a href="https://developers.binance.com/docs/algo/spot-algo/Query-Current-Algo-Open-Orders" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoOrders>> GetOpenAlgoOrdersAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get list of closed algo orders
    /// <para><a href="https://developers.binance.com/docs/algo/spot-algo/Query-Historical-Algo-Orders" /></para>
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