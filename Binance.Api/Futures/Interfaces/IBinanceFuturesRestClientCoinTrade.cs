namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Coin Futures Trade endpoints
/// </summary>
public interface IBinanceFuturesRestClientCoinTrade
{
    /// <summary>
    /// Event triggered when a normal order is placed via this client
    /// </summary>
    event Action<long>? OnOrderPlaced;

    /// <summary>
    /// Event triggered when a normal order is canceled via this client. Note that this does not trigger when using CancelAllOrdersAsync
    /// </summary>
    event Action<long>? OnOrderCanceled;

    /// <summary>
    /// Places a new LIMIT or MARKET order. Conditional order types have moved to the Algo Service and must use PlaceAlgoOrderAsync
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#new-order-trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="side">The order side (buy/sell)</param>
    /// <param name="type">The normal order type: LIMIT or MARKET</param>
    /// <param name="timeInForce">Lifetime of the order (GoodTillCancel/ImmediateOrCancel/FillOrKill)</param>
    /// <param name="quantity">The quantity of the base symbol</param>
    /// <param name="positionSide">The position side</param>
    /// <param name="reduceOnly">Specify as true if the order is intended to only reduce the position</param>
    /// <param name="price">The price to use</param>
    /// <param name="newClientOrderId">Unique id for order</param>
    /// <param name="orderResponseType">The response type. Default Acknowledge</param>
    /// <param name="priceMatch">Price matching mode for LIMIT orders</param>
    /// <param name="selfTradePreventionMode">Self trade prevention mode</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The immediate placement acknowledgement. Fill-derived average and cumulative base values are not part of this response</returns>
    Task<RestCallResult<BinanceFuturesCoinRestOrderAcknowledgement>> PlaceOrderAsync(
       string symbol,
       BinanceOrderSide side,
       BinanceFuturesOrderType type,
       decimal? quantity,
       decimal? price = null,
       string? newClientOrderId = null,
       BinancePositionSide? positionSide = null,
       BinanceTimeInForce? timeInForce = null,
       BinanceOrderResponseType? orderResponseType = null,
       BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
       BinanceFuturesPriceMatch? priceMatch = null,
       bool? reduceOnly = null,
       int? receiveWindow = null,
       CancellationToken ct = default);

    /// <summary>
    /// Places between one and five LIMIT or MARKET orders in one call. Conditional orders must use PlaceAlgoOrderAsync individually
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#place-multiple-orders-trade" /></para>
    /// </summary>
    /// <param name="orders">The normal LIMIT or MARKET orders to place</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>One immediate acknowledgement result per request item, in request order</returns>
    Task<RestCallResult<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>> PlaceOrdersAsync(IEnumerable<BinanceFuturesBatchOrderRequest> orders, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Modifies an existing LIMIT order and moves it to the back of the match queue
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#modify-order-trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="side">The existing order side</param>
    /// <param name="quantity">The complete new order quantity; required together with price after the COIN-M architecture migration</param>
    /// <param name="price">The new order price; required together with quantity after the COIN-M architecture migration</param>
    /// <param name="orderId">The exchange order id. Either this or origClientOrderId is required; this id takes precedence when both are sent</param>
    /// <param name="origClientOrderId">The original client order id. Either this or orderId is required</param>
    /// <param name="priceMatch">Published by Binance, but currently unusable because the same live contract requires price and prohibits combining priceMatch with price; non-null values are rejected</param>
    /// <param name="modifyId">Optional user-defined modification identifier passed through without uniqueness validation and returned only when supplied</param>
    /// <param name="receiveWindow">The receive window in milliseconds; cannot exceed 60000</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The current modification acknowledgement. Immediate modify responses do not contain fill-derived average or cumulative quote/base values</returns>
    Task<RestCallResult<BinanceFuturesCoinRestOrderAcknowledgement>> ModifyOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        decimal price,
        long? orderId = null,
        string? origClientOrderId = null,
        BinanceFuturesPriceMatch? priceMatch = null,
        long? modifyId = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Modifies between one and five existing LIMIT orders concurrently
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#modify-multiple-orders-trade" /></para>
    /// </summary>
    /// <param name="orders">One to five amendments. Every item requires symbol, side, quantity, price, and either OrderId or OriginalClientOrderId</param>
    /// <param name="receiveWindow">The receive window in milliseconds; cannot exceed 60000</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>One immediate acknowledgement result per request item in request order. Matching itself is concurrent and its order is not guaranteed</returns>
    Task<RestCallResult<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>> ModifyOrdersAsync(IEnumerable<BinanceFuturesBatchModifyRequest> orders, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the modification history for one order. History older than three months is unavailable
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#get-order-modify-history" /></para>
    /// </summary>
    /// <param name="symbol">The order symbol, for example `BTCUSD_PERP`</param>
    /// <param name="orderId">The exchange order id. Either this or origClientOrderId is required; this id takes precedence when both are sent</param>
    /// <param name="origClientOrderId">The original client order id. Either this or orderId is required</param>
    /// <param name="startTime">Inclusive modification-time lower bound</param>
    /// <param name="endTime">Inclusive modification-time upper bound</param>
    /// <param name="limit">Maximum number of results; defaults to 50 and cannot exceed 100</param>
    /// <param name="receiveWindow">The receive window in milliseconds; cannot exceed 60000</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The order's available modification history, including a conditional ModifyId inside each amendment</returns>
    public Task<RestCallResult<List<BinanceFuturesOrderModifyHistory>>> GetOrderModifyHistoryAsync(string symbol, long? orderId = null, string? origClientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels a pending order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#cancel-order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The immediate cancellation acknowledgement. Fill-derived average and cumulative base values are not part of this response</returns>
    Task<RestCallResult<BinanceFuturesCoinRestOrderAcknowledgement>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels multiple orders
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#cancel-multiple-orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="orderIdList">The list of order ids to cancel</param>
    /// <param name="origClientOrderIdList">The list of client order ids to cancel</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>One immediate cancellation acknowledgement result per requested order</returns>
    Task<RestCallResult<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>> CancelOrdersAsync(string symbol, IEnumerable<long>? orderIdList = null, IEnumerable<string>? origClientOrderIdList = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels all open orders
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#cancel-all-open-orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for canceled order</returns>
    Task<RestCallResult<bool>> CancelAllOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel all open orders of the specified symbol at the end of the specified countdown. This rest endpoint means to ensure your open orders are canceled in case of an outage. The endpoint should be called repeatedly as heartbeats
    /// so that the existing countdown time can be canceled and replaced by a new one.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#auto-cancel-all-open-orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="countDownTime">The time after which all open orders should cancel, or 0 to cancel an existing timer</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Countdown result</returns>
    Task<RestCallResult<BinanceFuturesCountDownResult>> CancelAllOrdersAfterTimeoutAsync(string symbol, TimeSpan countDownTime, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Places a native COIN-M conditional Algo order
    /// <para><a href="https://developers.binance.com/en/docs/products/derivatives-trading-coin-futures/Important-CM-UM-Integration-Notice" /></para>
    /// </summary>
    /// <param name="symbol">Symbol, for example `BTCUSD_PERP`</param>
    /// <param name="side">Order side</param>
    /// <param name="type">Conditional Algo order type</param>
    /// <param name="positionSide">Position side; required by Binance in Hedge Mode</param>
    /// <param name="timeInForce">Time in force</param>
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
    Task<RestCallResult<BinanceFuturesAlgoOrderPlacementResult>> PlaceAlgoOrderAsync(string symbol, BinanceOrderSide side, BinanceFuturesAlgoOrderType type, BinancePositionSide? positionSide = null, BinanceTimeInForce? timeInForce = null, decimal? quantity = null, decimal? price = null, decimal? triggerPrice = null, BinanceFuturesWorkingType? workingType = null, BinanceFuturesPriceMatch? priceMatch = null, bool? closePosition = null, bool? priceProtect = null, bool? reduceOnly = null, decimal? activatePrice = null, decimal? callbackRate = null, string? clientAlgoId = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, DateTime? goodTillDate = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels an active native COIN-M conditional Algo order by exchange or client Algo ID
    /// <para><a href="https://developers.binance.com/en/docs/products/derivatives-trading-coin-futures/Important-CM-UM-Integration-Notice" /></para>
    /// </summary>
    /// <param name="algoId">Exchange-assigned Algo order ID. At least one identifier must be provided</param>
    /// <param name="clientAlgoId">Client-assigned Algo order ID. At least one identifier must be provided</param>
    /// <param name="receiveWindow">The receive window for which this request is active. Maximum 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The cancellation result</returns>
    Task<RestCallResult<BinanceFuturesAlgoOrderCancellationResult>> CancelAlgoOrderAsync(long? algoId = null, string? clientAlgoId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets open native COIN-M conditional Algo orders for one symbol or all symbols
    /// <para><a href="https://developers.binance.com/en/docs/products/derivatives-trading-coin-futures/Important-CM-UM-Integration-Notice" /></para>
    /// </summary>
    /// <param name="symbol">Optional symbol filter. Omitting it returns every symbol and consumes IP weight 40</param>
    /// <param name="algoType">Optional Algo type filter</param>
    /// <param name="algoId">Optional exchange-assigned Algo order ID</param>
    /// <param name="receiveWindow">The receive window for which this request is active. Maximum 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The matching open conditional Algo orders</returns>
    Task<RestCallResult<List<BinanceFuturesAlgoOrderListItem>>> GetOpenAlgoOrdersAsync(string? symbol = null, string? algoType = null, long? algoId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves data for a specific order. Either orderId or origClientOrderId should be provided.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#query-order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The specific order</returns>
    Task<RestCallResult<BinanceFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets all COIN-M orders for exactly one symbol or pair. Pair queries return orders for every symbol in that pair and cannot include an order ID.
    /// When no time bounds are sent, Binance returns the latest seven days; an explicit query period must be shorter than seven days.
    /// Canceled or expired orders with no fill are unavailable after three days, and all orders are unavailable after 90 days.
    /// This is a signed USER_DATA query with the current post-migration flat IP weight 5.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#all-orders" /></para>
    /// </summary>
    /// <param name="symbol">Optional symbol scope, for example <c>BTCUSD_PERP</c>; mutually exclusive with <paramref name="pair"/>.</param>
    /// <param name="pair">Optional pair scope, for example <c>BTCUSD</c>; mutually exclusive with <paramref name="symbol"/>.</param>
    /// <param name="orderId">Optional inclusive order identifier; valid only with a symbol.</param>
    /// <param name="startTime">Optional query start time.</param>
    /// <param name="endTime">Optional query end time. Together with <paramref name="startTime"/>, the period must be shorter than seven days.</param>
    /// <param name="limit">Optional result limit from 1 through 100; server default is 50.</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The matching orders.</returns>
    Task<RestCallResult<List<BinanceFuturesOrder>>> GetOrdersAsync(string? symbol = null, string? pair = null, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets current open orders for a symbol, every symbol in a pair, or all symbols.
    /// A single-symbol query consumes IP weight 1; pair and unfiltered queries consume IP weight 40.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#current-all-open-orders" /></para>
    /// </summary>
    /// <param name="symbol">Optional symbol filter, for example <c>BTCUSD_PERP</c>.</param>
    /// <param name="pair">Optional pair filter, for example <c>BTCUSD</c>.</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of open orders</returns>
    Task<RestCallResult<List<BinanceFuturesOrder>>> GetOpenOrdersAsync(string? symbol = null, string? pair = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves data for a specific open order. Either orderId or origClientOrderId should be provided.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#query-current-open-order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The specific order</returns>
    Task<RestCallResult<BinanceFuturesOrder>> GetOpenOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the account's COIN-M liquidation and ADL orders from the available past 90 days.
    /// Omitting <paramref name="autoCloseType"/> returns both types. This signed USER_DATA query consumes the current
    /// post-migration IP weight 20 with a symbol and 50 without one.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#users-force-orders" /></para>
    /// </summary>
    /// <param name="symbol">Optional symbol scope, for example <c>BTCUSD_PERP</c>.</param>
    /// <param name="autoCloseType">Optional liquidation or ADL filter.</param>
    /// <param name="startTime">Optional query start time within the available 90-day history.</param>
    /// <param name="endTime">Optional query end time within the available 90-day history.</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="limit">Optional result limit; server default is 50 and maximum is 100.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The matching liquidation and ADL orders.</returns>
    Task<RestCallResult<List<BinanceFuturesOrder>>> GetForcedOrdersAsync(string? symbol = null, BinanceFuturesAutoCloseType? autoCloseType = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, int? limit = null, CancellationToken ct = default);

    /// <summary>
    /// Gets COIN-M account trades for exactly one symbol or pair. Pair queries return trades for every symbol in the pair,
    /// cannot include <paramref name="fromId"/> or <paramref name="orderId"/>, and order identifiers are supported only with a symbol.
    /// When no time bounds are sent, Binance returns the last seven days; an explicit time range cannot exceed seven days.
    /// This is a signed USER_DATA query with the current post-migration flat IP weight 5.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#account-trade-list" /></para>
    /// </summary>
    /// <param name="symbol">Optional symbol scope, for example <c>BTCUSD_PERP</c>; mutually exclusive with <paramref name="pair"/>.</param>
    /// <param name="pair">Optional pair scope, for example <c>BTCUSD</c>; mutually exclusive with <paramref name="symbol"/>.</param>
    /// <param name="limit">Optional result limit from 1 through 1000; server default is 50.</param>
    /// <param name="fromId">Optional inclusive trade identifier; only valid with symbol and without a time bound.</param>
    /// <param name="orderId">Optional string order identifier; only valid with symbol.</param>
    /// <param name="startTime">Optional start time.</param>
    /// <param name="endTime">Optional end time.</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The matching account trades.</returns>
    Task<RestCallResult<List<BinanceFuturesCoinUserTrade>>> GetUserTradesAsync(string? symbol = null, string? pair = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, string? orderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets current COIN-M REST position risk information. Omitting both filters returns positions for all symbols with <c>TRADING</c> status.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#position-information" /></para>
    /// </summary>
    /// <param name="marginAsset">Optional margin asset filter, for example <c>BTC</c>.</param>
    /// <param name="pair">Optional pair filter, for example <c>BTCUSD</c>.</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of Positions</returns>
    Task<RestCallResult<List<BinanceFuturesCoinPositionRisk>>> GetPositionsAsync(string? marginAsset = null, string? pair = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Change user's position mode (Hedge Mode or One-way Mode ) on EVERY symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#change-position-mode" /></para>
    /// </summary>
    /// <param name="dualPositionSide">User position mode</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether the request was successful</returns>
    Task<RestCallResult<bool>> SetPositionModeAsync(bool dualPositionSide, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Change the margin type for an open position
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#change-margin-type" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to change the position type for, for example `BTCUSD_PERP`</param>
    /// <param name="marginType">The type of margin to use</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether the request was successful</returns>
    Task<RestCallResult<bool>> SetMarginTypeAsync(string symbol, BinanceFuturesMarginType marginType, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Requests to change the initial leverage of the given symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#change-initial-leverage" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to change the initial leverage for, for example `BTCUSD_PERP`</param>
    /// <param name="leverage">The amount of initial leverage to change to</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Result of the initial leverage change request</returns>
    Task<RestCallResult<BinanceFuturesInitialLeverageChangeResult>> SetInitialLeverageAsync(string symbol, int leverage, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get position ADL quantile estimations
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#position-adl-quantile-estimation" /></para>
    /// </summary>
    /// <param name="symbol">Only get for this symbol, for example `BTCUSD_PERP`</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Change the margin on an open position
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#modify-isolated-position-margin" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to adjust the position margin for, for example `BTCUSD_PERP`</param>
    /// <param name="quantity">The amount of margin to be used</param>
    /// <param name="type">Whether to reduce or add margin to the position</param>
    /// <param name="positionSide">Default BOTH for One-way Mode ; LONG or SHORT for Hedge Mode. It must be sent with Hedge Mode.</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The new position margin</returns>
    Task<RestCallResult<BinanceFuturesPositionMarginResult>> SetPositionMarginAsync(string symbol, decimal quantity, BinanceFuturesMarginChangeDirectionType type, BinancePositionSide? positionSide = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Requests the margin change history for a specific symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/trade#get-position-margin-change-history" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get margin history for, for example `BTCUSD_PERP`</param>
    /// <param name="type">Filter the history by the direction of margin change</param>
    /// <param name="startTime">Margin changes newer than this date will be retrieved</param>
    /// <param name="endTime">Margin changes older than this date will be retrieved</param>
    /// <param name="limit">The max number of results</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, at most 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of all margin changes for the symbol</returns>
    Task<RestCallResult<List<BinanceFuturesMarginChangeHistoryResult>>> GetMarginChangeHistoryAsync(string symbol, BinanceFuturesMarginChangeDirectionType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}
