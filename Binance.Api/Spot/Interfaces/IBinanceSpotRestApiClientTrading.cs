namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot REST API Client Trading Methods
/// </summary>
public interface IBinanceSpotRestApiClientTrading
{
    /// <summary>
    /// Event triggered when an order is canceled via this client. 
    /// Note that this does not trigger when using CancelAllOrdersAsync. Only available for Spot orders
    /// </summary>
    event Action<long>? OnOrderPlaced;

    /// <summary>
    /// Event triggered when an order is placed via this client. Only available for Spot orders
    /// </summary>
    event Action<long>? OnOrderCanceled;

    Task<RestCallResult<BinanceSpotOrder>> PlaceOrderAsync(string symbol, BinanceSpotOrderSide side, BinanceSpotOrderType type, decimal? quantity = null, decimal? quoteQuantity = null, decimal? price = null, decimal? stopPrice = null, decimal? icebergQuantity = null, string? newClientOrderId = null, BinanceSpotTimeInForce? timeInForce = null, BinanceSpotOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, long? trailingDelta = null, long? strategyId = null, int? strategyType = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceSpotOrderTest>> PlaceTestOrderAsync(string symbol, BinanceSpotOrderSide side, BinanceSpotOrderType type, decimal? quantity = null, decimal? quoteQuantity = null, decimal? price = null, decimal? stopPrice = null, decimal? icebergQuantity = null, string? newClientOrderId = null, BinanceSpotTimeInForce? timeInForce = null, BinanceSpotOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, long? trailingDelta = null, long? strategyId = null, int? strategyType = null, int? receiveWindow = null, bool? computeFeeRates = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceSpotOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceSpotOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceSpotOrder>>> CancelOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceReplaceOrderResult>> ReplaceOrderAsync(string symbol, BinanceSpotOrderSide side, BinanceSpotOrderType type, BinanceSpotOrderCancelReplaceMode mode, long? cancelOrderId = null, string? cancelClientOrderId = null, string? newClientOrderId = null, string? newCancelClientOrderId = null, decimal? quantity = null, decimal? quoteQuantity = null, decimal? price = null, decimal? stopPrice = null, decimal? icebergQuantity = null, BinanceSpotTimeInForce? timeInForce = null, BinanceSpotOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, BinanceSpotOrderCancelRestriction? cancelRestriction = null, long? trailingDelta = null, long? strategyId = null, int? strategyType = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceSpotOrder>>> GetOpenOrdersAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceSpotOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}