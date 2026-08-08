namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot Web Socket API Client Trading Methods
/// </summary>
public interface IBinanceSpotSocketClientQueryTrading
{
    /// <summary>
    /// Places a new order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="side">The order side (buy/sell)</param>
    /// <param name="type">The order type</param>
    /// <param name="timeInForce">Lifetime of the order (GoodTillCancel/ImmediateOrCancel/FillOrKill)</param>
    /// <param name="orderResponseType">Response type</param>
    /// <param name="quantity">The quantity of the symbol</param>
    /// <param name="quoteQuantity">The quantity of the quote symbol. Only valid for market orders</param>
    /// <param name="price">The price to use</param>
    /// <param name="newClientOrderId">Unique id for order</param>
    /// <param name="stopPrice">Used for stop orders</param>
    /// <param name="icebergQuantity">Used for iceberg orders</param>
    /// <param name="trailingDelta">Trailing delta value for order in BIPS. A value of 1 means 0.01% trailing delta.</param>
    /// <param name="strategyId">Strategy id</param>
    /// <param name="strategyType">Strategy type</param>
    /// <param name="selfTradePreventionMode">Self trade prevention mode</param>
    /// <param name="pegPriceType">Reference side used to determine a pegged price</param>
    /// <param name="pegOffsetValue">Price level offset for a pegged order; maximum 100</param>
    /// <param name="pegOffsetType">Unit used for the pegged-price offset</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for the placed order</returns>
    Task<CallResult<BinanceSpotOrder>> PlaceOrderAsync(string symbol, BinanceOrderSide side, BinanceSpotOrderType type, decimal? quantity = null, decimal? quoteQuantity = null, decimal? price = null, decimal? stopPrice = null, decimal? icebergQuantity = null, string? newClientOrderId = null, BinanceTimeInForce? timeInForce = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, long? trailingDelta = null, long? strategyId = null, long? strategyType = null, BinanceSpotPegPriceType? pegPriceType = null, int? pegOffsetValue = null, BinanceSpotPegOffsetType? pegOffsetType = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Places a new test order. Test orders are not actually being executed and just test the functionality.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="side">The order side (buy/sell)</param>
    /// <param name="type">The order type (limit/market)</param>
    /// <param name="timeInForce">Lifetime of the order (GoodTillCancel/ImmediateOrCancel)</param>
    /// <param name="orderResponseType">Response type</param>
    /// <param name="quantity">The quantity of the symbol</param>
    /// <param name="quoteQuantity">The quantity of the quote symbol. Only valid for market orders</param>
    /// <param name="price">The price to use</param>
    /// <param name="newClientOrderId">Unique id for order</param>
    /// <param name="stopPrice">Used for stop orders</param>
    /// <param name="icebergQuantity">User for iceberg orders</param>
    /// <param name="trailingDelta">Trailing delta value for order in BIPS. A value of 1 means 0.01% trailing delta.</param>
    /// <param name="strategyId">Strategy id</param>
    /// <param name="strategyType">Strategy type</param>
    /// <param name="selfTradePreventionMode">Self trade prevention mode</param>
    /// <param name="pegPriceType">Reference side used to determine a pegged price</param>
    /// <param name="pegOffsetValue">Price level offset for a pegged order; maximum 100</param>
    /// <param name="pegOffsetType">Unit used for the pegged-price offset</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="computeFeeRates">Compute fee rates</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for the placed test order</returns>
    Task<CallResult<BinanceSpotOrderTest>> PlaceTestOrderAsync(string symbol, BinanceOrderSide side, BinanceSpotOrderType type, decimal? quantity = null, decimal? quoteQuantity = null, decimal? price = null, decimal? stopPrice = null, decimal? icebergQuantity = null, string? newClientOrderId = null, BinanceTimeInForce? timeInForce = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, long? trailingDelta = null, long? strategyId = null, long? strategyType = null, BinanceSpotPegPriceType? pegPriceType = null, int? pegOffsetValue = null, BinanceSpotPegOffsetType? pegOffsetType = null, decimal? receiveWindow = null, bool? computeFeeRates = null, CancellationToken ct = default);

    /// <summary>
    /// Get order by either orderId or clientOrderId
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#order-status" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="orderId">Order id</param>
    /// <param name="origClientOrderId">Client order id</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel an order by either orderId or clientOrderId
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="orderId">Order id</param>
    /// <param name="origClientOrderId">Client order id</param>
    /// <param name="newClientOrderId">New client order id for the order</param>
    /// <param name="cancelRestriction">Cancel restriction</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, BinanceSpotOrderCancelRestriction? cancelRestriction = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel an existing order and place a new order on the same symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
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
    /// <param name="selfTradePreventionMode">Self trade prevention mode</param>
    /// <param name="cancelRestriction">Cancel restriction</param>
    /// <param name="trailingDelta">Trailing delta value for order in BIPS. A value of 1 means 0.01% trailing delta.</param>
    /// <param name="strategyId">Strategy id</param>
    /// <param name="strategyType">Strategy type</param>
    /// <param name="orderRateLimitExceededMode">Behavior when the unfilled-order rate limit is exceeded</param>
    /// <param name="pegPriceType">Reference side used to determine a pegged price</param>
    /// <param name="pegOffsetValue">Price level offset for a pegged order; maximum 100</param>
    /// <param name="pegOffsetType">Unit used for the pegged-price offset</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotReplaceOrderResult>> ReplaceOrderAsync(string symbol, BinanceOrderSide side, BinanceSpotOrderType type, BinanceSpotOrderCancelReplaceMode mode, long? cancelOrderId = null, string? cancelClientOrderId = null, string? newClientOrderId = null, string? newCancelClientOrderId = null, decimal? quantity = null, decimal? quoteQuantity = null, decimal? price = null, decimal? stopPrice = null, decimal? icebergQuantity = null, BinanceTimeInForce? timeInForce = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, BinanceSpotOrderCancelRestriction? cancelRestriction = null, long? trailingDelta = null, long? strategyId = null, long? strategyType = null, BinanceSpotOrderRateLimitExceededMode? orderRateLimitExceededMode = null, BinanceSpotPegPriceType? pegPriceType = null, int? pegOffsetValue = null, BinanceSpotPegOffsetType? pegOffsetType = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>Reduces an order's quantity without losing its order-book priority.</summary>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    Task<CallResult<BinanceSpotOrderAmendResult>> AmendOrderAsync(string symbol, decimal newQuantity, long? orderId = null, string? originalClientOrderId = null, string? newClientOrderId = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get open orders
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/account#open-orders-status" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbols, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<List<BinanceSpotOrder>>> GetOpenOrdersAsync(string? symbol = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel all open orders for the symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<List<BinanceSpotOrder>>> CancelOrdersAsync(string symbol, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>Cancels an entire order list.</summary>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    Task<CallResult<BinanceSpotOrderList>> CancelOrderListAsync(string symbol, long? orderListId = null, string? listClientOrderId = null, string? newClientOrderId = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>Places a one-cancels-the-other order list.</summary>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    Task<CallResult<BinanceSpotOrderList>> PlaceOcoOrderListAsync(BinanceSpotOcoOrderListRequest request, CancellationToken ct = default);

    /// <summary>Places a one-pays-the-other order list.</summary>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    Task<CallResult<BinanceSpotOrderList>> PlaceOpoOrderListAsync(BinanceSpotOpoOrderListRequest request, CancellationToken ct = default);

    /// <summary>Places a one-pays-one-cancels-the-other order list.</summary>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    Task<CallResult<BinanceSpotOrderList>> PlaceOpocoOrderListAsync(BinanceSpotOpocoOrderListRequest request, CancellationToken ct = default);

    /// <summary>Places a one-triggers-the-other order list.</summary>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    Task<CallResult<BinanceSpotOrderList>> PlaceOtoOrderListAsync(BinanceSpotOtoOrderListRequest request, CancellationToken ct = default);

    /// <summary>Places a one-triggers-one-cancels-the-other order list.</summary>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    Task<CallResult<BinanceSpotOrderList>> PlaceOtocoOrderListAsync(BinanceSpotOtocoOrderListRequest request, CancellationToken ct = default);

    /// <summary>Places a new order using Smart Order Routing.</summary>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    Task<CallResult<List<BinanceSpotOrder>>> PlaceSorOrderAsync(string symbol, BinanceOrderSide side, BinanceSpotOrderType type, decimal quantity, decimal? price = null, string? newClientOrderId = null, BinanceTimeInForce? timeInForce = null, BinanceOrderResponseType? orderResponseType = null, decimal? icebergQuantity = null, long? strategyId = null, long? strategyType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>Tests a Smart Order Routing order without sending it to the matching engine.</summary>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/trade" /></para>
    Task<CallResult<BinanceSpotOrderTest>> PlaceSorTestOrderAsync(string symbol, BinanceOrderSide side, BinanceSpotOrderType type, decimal quantity, decimal? price = null, string? newClientOrderId = null, BinanceTimeInForce? timeInForce = null, BinanceOrderResponseType? orderResponseType = null, decimal? icebergQuantity = null, long? strategyId = null, long? strategyType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, decimal? receiveWindow = null, bool? computeFeeRates = null, CancellationToken ct = default);
}
