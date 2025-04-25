namespace Binance.Api.Algo;

/// <summary>
/// Interface for the Binance Algo REST API Client Futures Methods
/// </summary>
public interface IBinanceAlgoRestClientFutures
{
    /// <summary>
    /// Place a new Volume Participation order
    /// <para><a href="https://developers.binance.com/docs/algo/future-algo" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="side">Order side</param>
    /// <param name="quantity">Order quantity</param>
    /// <param name="urgency">Represent the relative speed of the current execution</param>
    /// <param name="clientOrderId">Client order id</param>
    /// <param name="reduceOnly">Reduce only</param>
    /// <param name="limitPrice">Limit price of the order. If null will use market price</param>
    /// <param name="positionSide">Position side</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoOrderResult>> PlaceVolumeParticipationOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        BinanceUrgency urgency,
        string? clientOrderId = null,
        bool? reduceOnly = null,
        decimal? limitPrice = null,
        BinancePositionSide? positionSide = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Place a new Time Weighted Average Price order
    /// <para><a href="https://developers.binance.com/docs/algo/future-algo/Time-Weighted-Average-Price-New-Order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="side">Order side</param>
    /// <param name="quantity">Order quantity</param>
    /// <param name="duration">Duration in seconds. Less than 5 minutes will be defaulted to 5 minutes, more than 24 hours will be defaulted to 24 hours.</param>
    /// <param name="clientOrderId">Client order id</param>
    /// <param name="reduceOnly">Reduce only</param>
    /// <param name="limitPrice">Limit price of the order. If null will use market price</param>
    /// <param name="positionSide">Position side</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoOrderResult>> PlaceTimeWeightedAveragePriceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        int duration,
        string? clientOrderId = null,
        bool? reduceOnly = null,
        decimal? limitPrice = null,
        BinancePositionSide? positionSide = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Cancel an algo order
    /// <para><a href="https://developers.binance.com/docs/algo/future-algo/Cancel-Algo-Order" /></para>
    /// </summary>
    /// <param name="algoOrderId">Algo id to cancel</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceAlgoResult>> CancelAlgoOrderAsync(long algoOrderId, int? receiveWindow = null, CancellationToken ct = default);

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
