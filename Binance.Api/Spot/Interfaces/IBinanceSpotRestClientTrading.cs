namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot REST API Client Trading Methods
/// </summary>
public interface IBinanceSpotRestClientTrading
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

    /// <summary>
    /// Places a new order
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/trading-endpoints" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="side">The order side (buy/sell)</param>
    /// <param name="type">The order type</param>
    /// <param name="timeInForce">Lifetime of the order (GoodTillCancel/ImmediateOrCancel/FillOrKill)</param>
    /// <param name="quantity">The quantity of the symbol</param>
    /// <param name="quoteQuantity">The quantity of the quote symbol. Only valid for market orders</param>
    /// <param name="price">The price to use</param>
    /// <param name="newClientOrderId">Unique id for order</param>
    /// <param name="stopPrice">Used for stop orders</param>
    /// <param name="icebergQuantity">Used for iceberg orders</param>
    /// <param name="orderResponseType">Used for the response JSON</param>
    /// <param name="trailingDelta">Trailing delta value for order in BIPS. A value of 1 means 0.01% trailing delta.</param>
    /// <param name="strategyId">Strategy id</param>
    /// <param name="strategyType">Strategy type</param>
    /// <param name="selfTradePreventionMode">Self trade prevention mode</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for the placed order</returns>
    Task<RestCallResult<BinanceSpotOrder>> PlaceOrderAsync(string symbol, BinanceOrderSide side, BinanceSpotOrderType type, decimal? quantity = null, decimal? quoteQuantity = null, decimal? price = null, decimal? stopPrice = null, decimal? icebergQuantity = null, string? newClientOrderId = null, BinanceTimeInForce? timeInForce = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, long? trailingDelta = null, long? strategyId = null, int? strategyType = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Places a new test order. Test orders are not actually being executed and just test the functionality.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/trading-endpoints#test-new-order-trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="side">The order side (buy/sell)</param>
    /// <param name="type">The order type (limit/market)</param>
    /// <param name="timeInForce">Lifetime of the order (GoodTillCancel/ImmediateOrCancel)</param>
    /// <param name="quantity">The quantity of the symbol</param>
    /// <param name="quoteQuantity">The quantity of the quote symbol. Only valid for market orders</param>
    /// <param name="price">The price to use</param>
    /// <param name="newClientOrderId">Unique id for order</param>
    /// <param name="stopPrice">Used for stop orders</param>
    /// <param name="icebergQuantity">User for iceberg orders</param>
    /// <param name="orderResponseType">Used for the response JSON</param>
    /// <param name="trailingDelta">Trailing delta value for order in BIPS. A value of 1 means 0.01% trailing delta.</param>
    /// <param name="strategyId">Strategy id</param>
    /// <param name="strategyType">Strategy type</param>
    /// <param name="selfTradePreventionMode">Self trade prevention mode</param>
    /// <param name="computeFeeRates">Whether fee rates should be calculated or not</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Fee info if computeCommissionRates was set to true, else empty</returns>
    Task<RestCallResult<BinanceSpotOrderTest>> PlaceTestOrderAsync(string symbol, BinanceOrderSide side, BinanceSpotOrderType type, decimal? quantity = null, decimal? quoteQuantity = null, decimal? price = null, decimal? stopPrice = null, decimal? icebergQuantity = null, string? newClientOrderId = null, BinanceTimeInForce? timeInForce = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, long? trailingDelta = null, long? strategyId = null, int? strategyType = null, int? receiveWindow = null, bool? computeFeeRates = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves data for a specific order. Either orderId or origClientOrderId should be provided.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/trading-endpoints#query-order-user_data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The specific order</returns>
    Task<RestCallResult<BinanceSpotOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels a pending order
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/trading-endpoints#cancel-order-trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="newClientOrderId">Unique identifier for this cancel</param>
    /// <param name="cancelRestriction">Restrict cancellation based on order state</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for canceled order</returns>
    Task<RestCallResult<BinanceSpotOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, BinanceSpotOrderCancelRestriction? cancelRestriction = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels all open orders on a symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/trading-endpoints#cancel-all-open-orders-on-a-symbol-trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for canceled order</returns>
    Task<RestCallResult<IEnumerable<BinanceSpotOrder>>> CancelOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel an existing order and place a new order on the same symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/trading-endpoints#cancel-an-existing-order-and-send-a-new-order-trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="side">The order side (buy/sell)</param>
    /// <param name="type">The order type</param>
    /// <param name="mode">Replacement behavior</param>
    /// <param name="cancelOrderId">The order id to cancel. Either this or cancelClientOrderId should be provided</param>
    /// <param name="cancelClientOrderId">The client order id to cancel. Either this or cancelOrderId should be provided</param>
    /// <param name="newCancelClientOrderId">New client order id for the canceled order</param>
    /// <param name="timeInForce">Lifetime of the order (GoodTillCancel/ImmediateOrCancel/FillOrKill)</param>
    /// <param name="quantity">The quantity of the symbol</param>
    /// <param name="quoteQuantity">The quantity of the quote symbol. Only valid for market orders</param>
    /// <param name="price">The price to use</param>
    /// <param name="newClientOrderId">Unique id for order</param>
    /// <param name="stopPrice">Used for stop orders</param>
    /// <param name="icebergQuantity">Used for iceberg orders</param>
    /// <param name="orderResponseType">Used for the response JSON</param>
    /// <param name="trailingDelta">Trailing delta value for order in BIPS. A value of 1 means 0.01% trailing delta.</param>
    /// <param name="strategyId">Strategy id</param>
    /// <param name="strategyType">Strategy type</param>
    /// <param name="cancelRestriction">Restrict cancellation based on order state</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSpotReplaceOrderResult>> ReplaceOrderAsync(string symbol, BinanceOrderSide side, BinanceSpotOrderType type, BinanceSpotOrderCancelReplaceMode mode, long? cancelOrderId = null, string? cancelClientOrderId = null, string? newClientOrderId = null, string? newCancelClientOrderId = null, decimal? quantity = null, decimal? quoteQuantity = null, decimal? price = null, decimal? stopPrice = null, decimal? icebergQuantity = null, BinanceTimeInForce? timeInForce = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, BinanceSpotOrderCancelRestriction? cancelRestriction = null, long? trailingDelta = null, long? strategyId = null, int? strategyType = null, int? receiveWindow = null, CancellationToken ct = default);
    
    // TODO: Order Amend Keep Priority (TRADE)

    /// <summary>
    /// Gets a list of open orders
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/trading-endpoints#current-open-orders-user_data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get open orders for, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of open orders</returns>
    Task<RestCallResult<IEnumerable<BinanceSpotOrder>>> GetOpenOrdersAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets all orders for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/trading-endpoints#all-orders-user_data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get orders for, for example `ETHUSDT`</param>
    /// <param name="orderId">If set, only orders with an order id higher than the provided will be returned</param>
    /// <param name="startTime">If set, only orders placed after this time will be returned</param>
    /// <param name="endTime">If set, only orders placed before this time will be returned</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of orders</returns>
    Task<RestCallResult<IEnumerable<BinanceSpotOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
    
    // TODO: New order using SOR (TRADE)

}