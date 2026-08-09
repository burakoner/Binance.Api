namespace Binance.Api.Futures;

/// <summary>
/// Binance Coin futures trading websocket API
/// </summary>
public interface IBinanceFuturesSocketClientCoinQueryTrade
{
    /// <summary>
    /// Place a new order
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/websocket-api" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSD_PERP`</param>
    /// <param name="side">The order side (buy/sell)</param>
    /// <param name="type">The order type</param>
    /// <param name="timeInForce">Lifetime of the order (GoodTillCancel/ImmediateOrCancel/FillOrKill)</param>
    /// <param name="quantity">The quantity of the base symbol</param>
    /// <param name="positionSide">The position side</param>
    /// <param name="reduceOnly">Specify as true if the order is intended to only reduce the position</param>
    /// <param name="price">The price to use</param>
    /// <param name="newClientOrderId">Unique id for order</param>
    /// <param name="stopPrice">Used for stop orders</param>
    /// <param name="activationPrice">Used with TRAILING_STOP_MARKET orders, default as the latest price（supporting different workingType)</param>
    /// <param name="callbackRate">Used with TRAILING_STOP_MARKET orders</param>
    /// <param name="workingType">stopPrice triggered by: "MARK_PRICE", "CONTRACT_PRICE"</param>
    /// <param name="closePosition">Close-All，used with STOP_MARKET or TAKE_PROFIT_MARKET.</param>
    /// <param name="orderResponseType">The response type. Default Acknowledge</param>
    /// <param name="priceProtect">If true when price reaches stopPrice, difference between "MARK_PRICE" and "CONTRACT_PRICE" cannot be larger than "triggerProtect" of the symbol.</param>
    /// <param name="priceMatch">Only available for Limit/Stop/TakeProfit order</param>
    /// <param name="selfTradePreventionMode">Self trade prevention mode</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    Task<CallResult<BinanceFuturesOrder>> PlaceOrderAsync(string symbol, BinanceOrderSide side, BinanceFuturesOrderType type, decimal? quantity, decimal? price = null, decimal? stopPrice = null, string? newClientOrderId = null, BinancePositionSide? positionSide = null, BinanceTimeInForce? timeInForce = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, BinanceFuturesPriceMatch? priceMatch = null, BinanceFuturesWorkingType? workingType = null, bool? reduceOnly = null, bool? closePosition = null, bool? priceProtect = null, decimal? activationPrice = null, decimal? callbackRate = null, int? receiveWindow = null, CancellationToken ct = default);

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
    Task<CallResult<BinanceFuturesOrder>> ModifyOrderAsync(string symbol, BinanceOrderSide side, decimal quantity, decimal price, long? orderId = null, string? origClientOrderId = null, BinanceFuturesPriceMatch? priceMatch = null, long? modifyId = null, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels a pending order
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/trade/websocket-api/Cancel-Order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSD_PERP`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for canceled order</returns>
    Task<CallResult<BinanceFuturesOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves data for a specific order. Either orderId or origClientOrderId should be provided.
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/trade/websocket-api/Query-Order" /></para>
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
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/trade/websocket-api/Position-Info-V2" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSD_PERP`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    Task<CallResult<List<BinanceFuturesCoinPosition>>> GetPositionsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);
}
