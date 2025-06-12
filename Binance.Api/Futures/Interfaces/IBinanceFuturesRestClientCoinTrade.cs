namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Coin Futures Trade endpoints
/// </summary>
public interface IBinanceFuturesRestClientCoinTrade
{
    /// <summary>
    /// Event triggered when an order is placed via this client. Only available for Spot orders
    /// </summary>
    event Action<long>? OnOrderPlaced;

    /// <summary>
    /// Event triggered when an order is canceled via this client. Note that this does not trigger when using CancelAllOrdersAsync. Only available for Spot orders
    /// </summary>
    event Action<long>? OnOrderCanceled;

    /// <summary>
    /// Places a new order
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
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
    /// <returns>Id's for the placed order</returns>
    Task<RestCallResult<BinanceFuturesOrder>> PlaceOrderAsync(
       string symbol,
       BinanceOrderSide side,
       BinanceFuturesOrderType type,
       decimal? quantity,
       decimal? price = null,
       decimal? stopPrice = null,
       string? newClientOrderId = null,
       BinancePositionSide? positionSide = null,
       BinanceTimeInForce? timeInForce = null,
       BinanceOrderResponseType? orderResponseType = null,
       BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
       BinanceFuturesPriceMatch? priceMatch = null,
       BinanceFuturesWorkingType? workingType = null,
       bool? reduceOnly = null,
       bool? closePosition = null,
       bool? priceProtect = null,
       decimal? activationPrice = null,
       decimal? callbackRate = null,
       int? receiveWindow = null,
       CancellationToken ct = default);

    /// <summary>
    /// Place multiple orders in one call
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Place-Multiple-Orders" /></para>
    /// </summary>
    /// <param name="orders">The orders to place</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Returns a list of call results, one for each order. The order the results are in is the order the orders were sent</returns>
    Task<RestCallResult<List<CallResult<BinanceFuturesOrder>>>> PlaceOrdersAsync(IEnumerable<BinanceFuturesBatchOrderRequest> orders, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Modify Order (TRADE)
    // TODO: Modify Multiple Orders(TRADE)
    // TODO: Get Order Modify History (USER_DATA)

    /// <summary>
    /// Cancels a pending order
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Cancel-Order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for canceled order</returns>
    Task<RestCallResult<BinanceFuturesOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels multiple orders
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Cancel-Multiple-Orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="orderIdList">The list of order ids to cancel</param>
    /// <param name="origClientOrderIdList">The list of client order ids to cancel</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for canceled order</returns>
    Task<RestCallResult<List<CallResult<BinanceFuturesOrder>>>> CancelOrdersAsync(string symbol, IEnumerable<long>? orderIdList = null, IEnumerable<string>? origClientOrderIdList = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels all open orders
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Cancel-All-Open-Orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for canceled order</returns>
    Task<RestCallResult<BinanceResult>> CancelAllOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel all open orders of the specified symbol at the end of the specified countdown. This rest endpoint means to ensure your open orders are canceled in case of an outage. The endpoint should be called repeatedly as heartbeats
    /// so that the existing countdown time can be canceled and replaced by a new one.
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Auto-Cancel-All-Open-Orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol, for example `BTCUSD_PERP`</param>
    /// <param name="countDownTime">The time after which all open orders should cancel, or 0 to cancel an existing timer</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Countdown result</returns>
    Task<RestCallResult<BinanceFuturesCountDownResult>> CancelAllOrdersAfterTimeoutAsync(string symbol, TimeSpan countDownTime, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves data for a specific order. Either orderId or origClientOrderId should be provided.
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Query-Order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The specific order</returns>
    Task<RestCallResult<BinanceFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets all orders for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/All-Orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get orders for, for example `BTCUSD_PERP`</param>
    /// <param name="orderId">If set, only orders with an order id higher than the provided will be returned</param>
    /// <param name="startTime">If set, only orders placed after this time will be returned</param>
    /// <param name="endTime">If set, only orders placed before this time will be returned</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of orders</returns>
    Task<RestCallResult<List<BinanceFuturesOrder>>> GetOrdersAsync(string? symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets a list of open orders
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Current-All-Open-Orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get open orders for, for example `BTCUSD_PERP`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of open orders</returns>
    Task<RestCallResult<List<BinanceFuturesOrder>>> GetOpenOrdersAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves data for a specific open order. Either orderId or origClientOrderId should be provided.
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Query-Current-Open-Order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `BTCUSD_PERP`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The specific order</returns>
    Task<RestCallResult<BinanceFuturesOrder>> GetOpenOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets a list of users forced orders
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Users-Force-Orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get forced orders for, for example `BTCUSD_PERP`</param>
    /// <param name="closeType">Filter by reason for close</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of forced orders</returns>
    Task<RestCallResult<List<BinanceFuturesOrder>>> GetForcedOrdersAsync(string? symbol = null, BinanceFuturesAutoCloseType? closeType = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets all user trades for provided symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Account-Trade-List" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get trades for, for example `BTCUSD_PERP`</param>
    /// <param name="pair">Symbol to get trades for, for example `BTCUSD`</param>
    /// <param name="limit">The max number of results</param>
    /// <param name="fromId">TradeId to fetch from. Default gets most recent trades</param>
    /// <param name="orderId">Get the trades for a specific order</param>
    /// <param name="startTime">Orders newer than this date will be retrieved</param>
    /// <param name="endTime">Orders older than this date will be retrieved</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of trades</returns>
    Task<RestCallResult<List<BinanceFuturesCoinUserTrade>>> GetUserTradesAsync(string? symbol = null, string? pair = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, long? orderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets account position information
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Position-Information" /></para>
    /// </summary>
    /// <param name="marginAsset">Filter by margin asset, for example `ETH`</param>
    /// <param name="pair">Filter by pair, for example `BTCUSD`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of Positions</returns>
    Task<RestCallResult<List<BinanceFuturesCoinPosition>>> GetPositionsAsync(string? marginAsset = null, string? pair = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Change user's position mode (Hedge Mode or One-way Mode ) on EVERY symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Change-Position-Mode" /></para>
    /// </summary>
    /// <param name="dualPositionSide">User position mode</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether the request was successful</returns>
    Task<RestCallResult<BinanceResult>> SetPositionModeAsync(bool dualPositionSide, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Change the margin type for an open position
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Change-Margin-Type" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to change the position type for, for example `BTCUSD_PERP`</param>
    /// <param name="marginType">The type of margin to use</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether the request was successful</returns>
    Task<RestCallResult<BinanceResult>> SetMarginTypeAsync(string symbol, BinanceFuturesMarginType marginType, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Requests to change the initial leverage of the given symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Change-Initial-Leverage" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to change the initial leverage for, for example `BTCUSD_PERP`</param>
    /// <param name="leverage">The amount of initial leverage to change to</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Result of the initial leverage change request</returns>
    Task<RestCallResult<BinanceFuturesInitialLeverageChangeResult>> SetInitialLeverageAsync(string symbol, int leverage, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get position ADL quantile estimations
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Position-ADL-Quantile-Estimation" /></para>
    /// </summary>
    /// <param name="symbol">Only get for this symbol, for example `BTCUSD_PERP`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Change the margin on an open position
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Modify-Isolated-Position-Margin" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to adjust the position margin for, for example `BTCUSD_PERP`</param>
    /// <param name="quantity">The amount of margin to be used</param>
    /// <param name="type">Whether to reduce or add margin to the position</param>
    /// <param name="positionSide">Default BOTH for One-way Mode ; LONG or SHORT for Hedge Mode. It must be sent with Hedge Mode.</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The new position margin</returns>
    Task<RestCallResult<BinanceFuturesPositionMarginResult>> SetPositionMarginAsync(string symbol, decimal quantity, BinanceFuturesMarginChangeDirectionType type, BinancePositionSide? positionSide = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Requests the margin change history for a specific symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/trade/rest-api/Get-Position-Margin-Change-History" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get margin history for, for example `BTCUSD_PERP`</param>
    /// <param name="type">Filter the history by the direction of margin change</param>
    /// <param name="startTime">Margin changes newer than this date will be retrieved</param>
    /// <param name="endTime">Margin changes older than this date will be retrieved</param>
    /// <param name="limit">The max number of results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of all margin changes for the symbol</returns>
    Task<RestCallResult<List<BinanceFuturesMarginChangeHistoryResult>>> GetMarginChangeHistoryAsync(string symbol, BinanceFuturesMarginChangeDirectionType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}
