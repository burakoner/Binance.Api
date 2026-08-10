namespace Binance.Api.Futures;

/// <summary>
/// Binance Coin futures trading websocket API
/// </summary>
public interface IBinanceFuturesSocketClientCoinQueryTrade
{
    /// <summary>
    /// Places a new LIMIT or MARKET order. Conditional order types have moved to the REST Algo Order API
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-api/trade#new-order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSD_PERP`</param>
    /// <param name="side">The order side (buy/sell)</param>
    /// <param name="type">The normal order type; only Limit and Market are supported</param>
    /// <param name="timeInForce">Required for Limit orders; supports GTC, IOC, FOK, and GTX</param>
    /// <param name="quantity">The contract quantity; required and greater than zero</param>
    /// <param name="positionSide">BOTH, LONG, or SHORT; required by Binance in Hedge Mode</param>
    /// <param name="reduceOnly">Whether the order only reduces a position; cannot be sent in Hedge Mode</param>
    /// <param name="price">Limit price; exactly one of price or priceMatch is required for Limit orders</param>
    /// <param name="newClientOrderId">Optional unique ID matching `^[\.A-Z\:/a-z0-9_-]{1,36}$`</param>
    /// <param name="orderResponseType">ACK or RESULT response mode</param>
    /// <param name="priceMatch">Price matching mode for Limit orders; cannot be sent together with price</param>
    /// <param name="selfTradePreventionMode">Self trade prevention mode</param>
    /// <param name="receiveWindow">The int64 receive window in milliseconds; cannot exceed 60000</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The immediate placement acknowledgement. Fill-derived average and cumulative base values are not part of this response</returns>
    Task<CallResult<BinanceFuturesCoinSocketOrderAcknowledgement>> PlaceOrderAsync(string symbol, BinanceOrderSide side, BinanceFuturesOrderType type, decimal? quantity, decimal? price = null, string? newClientOrderId = null, BinancePositionSide? positionSide = null, BinanceTimeInForce? timeInForce = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, BinanceFuturesPriceMatch? priceMatch = null, bool? reduceOnly = null, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Modifies an existing LIMIT order. The amended order is reordered in the match queue
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-api/trade#modify-order" /></para>
    /// </summary>
    /// <remarks>An order can be modified fewer than 10000 times. Filter failures reject the amendment without changing the order. Binance cancels a partially filled order when the new quantity is less than or equal to its executed quantity, and cancels a GTX order when the new price would execute immediately.</remarks>
    /// <param name="symbol">The symbol, for example `ETHUSD_PERP`</param>
    /// <param name="side">Order side</param>
    /// <param name="quantity">The complete new order quantity</param>
    /// <param name="price">The complete new order price</param>
    /// <param name="orderId">The exchange order id. Either this or origClientOrderId is required; this id takes precedence when both are sent</param>
    /// <param name="origClientOrderId">The original client order id. Either this or orderId is required</param>
    /// <param name="priceMatch">Published by Binance, but currently unusable because the same live contract requires price and prohibits combining priceMatch with price; non-null values are rejected</param>
    /// <param name="modifyId">Optional user-defined modification identifier passed through without uniqueness validation and returned only when supplied</param>
    /// <param name="receiveWindow">The receive window in milliseconds; cannot exceed 60000</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The current modification acknowledgement. Immediate modify responses do not contain fill-derived average or cumulative quote/base values</returns>
    Task<CallResult<BinanceFuturesCoinSocketOrderAcknowledgement>> ModifyOrderAsync(string symbol, BinanceOrderSide side, decimal quantity, decimal price, long? orderId = null, string? origClientOrderId = null, BinanceFuturesPriceMatch? priceMatch = null, long? modifyId = null, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels a pending order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-api/trade#cancel-order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSD_PERP`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The immediate cancellation acknowledgement. Fill-derived average and cumulative base values are not part of this response</returns>
    Task<CallResult<BinanceFuturesCoinSocketOrderAcknowledgement>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves data for a specific order. Either orderId or origClientOrderId should be provided.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-api/trade#query-order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSD_PERP`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The specific order</returns>
    Task<CallResult<BinanceFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get position information
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-api/trade#position-information" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSD_PERP`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    Task<CallResult<List<BinanceFuturesCoinPosition>>> GetPositionsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);
}
