namespace Binance.Api.Futures;

/// <summary>
/// Binance USDⓈ-M Futures WebSocket API Trade endpoints
/// </summary>
public interface IBinanceFuturesSocketClientUsdQueryTrade
{
    /// <summary>
    /// Places a new LIMIT or MARKET order. Conditional order types have moved to the Algo Service and must use PlaceAlgoOrderAsync
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/trade#new-order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="side">The order side (buy/sell)</param>
    /// <param name="type">The order type; only Limit and Market are supported</param>
    /// <param name="timeInForce">Required for Limit orders; supports GTC, IOC, FOK, GTX, GTD, and RPI</param>
    /// <param name="quantity">The order quantity; required and greater than zero</param>
    /// <param name="positionSide">BOTH, LONG, or SHORT; required by Binance in Hedge Mode</param>
    /// <param name="reduceOnly">Whether the order only reduces a position; cannot be sent in Hedge Mode</param>
    /// <param name="price">Limit price; exactly one of price or priceMatch is required for Limit orders</param>
    /// <param name="newClientOrderId">Optional unique ID matching `^[\.A-Z\:/a-z0-9_-]{1,36}$`</param>
    /// <param name="orderResponseType">ACK or RESULT response mode</param>
    /// <param name="priceMatch">Price matching mode for Limit orders; cannot be sent together with price</param>
    /// <param name="selfTradePreventionMode">Self trade prevention mode</param>
    /// <param name="goodTillDate">Order cancel time for timeInForce GoodTillDate</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    Task<CallResult<BinanceFuturesOrder>> PlaceOrderAsync(string symbol, BinanceOrderSide side, BinanceFuturesOrderType type, decimal? quantity, decimal? price = null, string? newClientOrderId = null, BinancePositionSide? positionSide = null, BinanceTimeInForce? timeInForce = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, BinanceFuturesPriceMatch? priceMatch = null, bool? reduceOnly = null, DateTime? goodTillDate = null, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Places a native USDⓈ-M conditional Algo order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/trade#new-algo-order" /></para>
    /// </summary>
    /// <param name="symbol">Symbol, for example `ETHUSDT`</param>
    /// <param name="side">Order side</param>
    /// <param name="type">One of the five current conditional Algo order types</param>
    /// <param name="positionSide">Position side; required by Binance in Hedge Mode</param>
    /// <param name="timeInForce">IOC, GTC, FOK, or the separately documented GTD contract</param>
    /// <param name="quantity">Order quantity; cannot be sent with close-all</param>
    /// <param name="price">Order price; cannot be sent with price matching</param>
    /// <param name="triggerPrice">Trigger price</param>
    /// <param name="workingType">Trigger working price type</param>
    /// <param name="priceMatch">Price matching mode for STOP or TAKE_PROFIT</param>
    /// <param name="closePosition">Whether to close the entire position</param>
    /// <param name="priceProtect">Whether trigger-price protection is enabled</param>
    /// <param name="reduceOnly">Whether the order only reduces a position</param>
    /// <param name="activatePrice">Trailing-stop activation price</param>
    /// <param name="callbackRate">Trailing-stop callback rate from 0.1 through 10</param>
    /// <param name="clientAlgoId">Optional unique client Algo ID matching `^[\.A-Z\:/a-z0-9_-]{1,36}$`</param>
    /// <param name="orderResponseType">ACK or RESULT response mode</param>
    /// <param name="selfTradePreventionMode">Self-trade prevention mode</param>
    /// <param name="goodTillDate">Cancel time for GTD orders; must be more than 600 seconds in the future</param>
    /// <param name="receiveWindow">The receive window for which this request is active</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The placed native conditional Algo order</returns>
    Task<CallResult<BinanceFuturesAlgoOrderPlacementResult>> PlaceAlgoOrderAsync(string symbol, BinanceOrderSide side, BinanceFuturesAlgoOrderType type, BinancePositionSide? positionSide = null, BinanceTimeInForce? timeInForce = null, decimal? quantity = null, decimal? price = null, decimal? triggerPrice = null, BinanceFuturesWorkingType? workingType = null, BinanceFuturesPriceMatch? priceMatch = null, bool? closePosition = null, bool? priceProtect = null, bool? reduceOnly = null, decimal? activatePrice = null, decimal? callbackRate = null, string? clientAlgoId = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, DateTime? goodTillDate = null, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels an active native conditional Algo order by exchange or client Algo ID
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/trade#cancel-algo-order" /></para>
    /// </summary>
    /// <param name="algoId">Exchange-assigned Algo order ID. At least one identifier must be provided</param>
    /// <param name="clientAlgoId">Client-assigned Algo order ID. At least one identifier must be provided</param>
    /// <param name="receiveWindow">The receive window for which this request is active</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The cancellation result</returns>
    Task<CallResult<BinanceFuturesAlgoOrderCancellationResult>> CancelAlgoOrderAsync(long? algoId = null, string? clientAlgoId = null, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Modifies an existing LIMIT order. The amended order is reordered in the match queue
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/trade#modify-order" /></para>
    /// </summary>
    /// <remarks>An order can be modified fewer than 10000 times. Filter failures reject the amendment without changing the order. Binance cancels a partially filled order when the new quantity is less than or equal to its executed quantity, and cancels a GTX order when the new price would execute immediately.</remarks>
    /// <param name="symbol">The symbol, for example `ETHUSDT`</param>
    /// <param name="side">Order side</param>
    /// <param name="quantity">The complete new order quantity</param>
    /// <param name="price">The complete new order price</param>
    /// <param name="orderId">The exchange order id. Either this or origClientOrderId is required; this id takes precedence when both are sent</param>
    /// <param name="origClientOrderId">The original client order id. Either this or orderId is required</param>
    /// <param name="priceMatch">Published by Binance, but currently unusable because the same live contract requires price and prohibits combining priceMatch with price; non-null values are rejected</param>
    /// <param name="modifyId">Optional user-defined modification identifier passed through without uniqueness validation and returned only when supplied</param>
    /// <param name="receiveWindow">The receive window in milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The current modification acknowledgement. Immediate modify responses do not contain fill-derived average or cumulative quote/base values</returns>
    Task<CallResult<BinanceFuturesOrder>> ModifyOrderAsync(string symbol, BinanceOrderSide side, decimal quantity, decimal price, long? orderId = null, string? origClientOrderId = null, BinanceFuturesPriceMatch? priceMatch = null, long? modifyId = null, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels a pending order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/trade#cancel-order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for canceled order</returns>
    Task<CallResult<BinanceFuturesOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves data for a specific order. Either orderId or origClientOrderId should be provided.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/trade#query-order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The specific order</returns>
    Task<CallResult<BinanceFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets v1 position information for all symbols or a requested symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/trade#position-information" /></para>
    /// </summary>
    /// <remarks>Binance announced future deprecation without a removal date in 2024; the endpoint remains in the current catalog. Prefer v2 unless the full v1 contract is required.</remarks>
    /// <param name="symbol">Optional symbol filter, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">The receive window for which this request is active</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The v1 position details</returns>
    Task<CallResult<List<BinanceFuturesUsdtPosition>>> GetPositionsV1Async(string? symbol = null, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets v2 position information for symbols with a position or open orders
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/trade#position-information-v2" /></para>
    /// </summary>
    /// <param name="symbol">Optional symbol filter, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The v2 position information</returns>
    Task<CallResult<List<BinanceFuturesPositionV3>>> GetPositionsAsync(string? symbol = null, long? receiveWindow = null, CancellationToken ct = default);
}
