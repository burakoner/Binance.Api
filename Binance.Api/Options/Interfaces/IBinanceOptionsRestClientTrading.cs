namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options REST API Client Trading Methods
/// </summary>
public interface IBinanceOptionsRestClientTrading
{
    /// <summary>
    /// Event triggered when an order is canceled via this client. 
    /// Note that this does not trigger when using CancelAllOrdersAsync. Only available for Options orders
    /// </summary>
    event Action<long>? OnOrderPlaced;

    /// <summary>
    /// Event triggered when an order is placed via this client. Only available for Options orders
    /// </summary>
    event Action<long>? OnOrderCanceled;

    /// <summary>
    /// Send a new order.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="side">Buy/sell direction: SELL, BUY</param>
    /// <param name="type">Order Type: LIMIT(only support limit)</param>
    /// <param name="quantity">Order Quantity</param>
    /// <param name="price">Order Price</param>
    /// <param name="timeInForce">Time in force method（Default GTC）</param>
    /// <param name="clientOrderId">User-defined order ID cannot be repeated in pending orders</param>
    /// <param name="reduceOnly">Reduce Only(Default false)</param>
    /// <param name="postOnly">Post Only (Default false)</param>
    /// <param name="isMmp">is market maker protection order, true/false</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsOrder>> PlaceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceOptionsOrderType type,
        decimal? quantity = null,
        decimal? price = null,
        BinanceTimeInForce? timeInForce = null,
        string? clientOrderId = null,
        bool? reduceOnly = null,
        bool? postOnly = null,
        bool? isMmp = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Send multiple option orders.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade/Place-Multiple-Orders" /></para>
    /// </summary>
    /// <param name="orders">order list. Max 5 orders</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsOrder>>> PlaceOrdersAsync(IEnumerable<BinanceOptionsBatchOrderRequest> orders, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel an active order.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade/Cancel-Option-Order" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="orderId">Order ID, e.g 4611875134427365377</param>
    /// <param name="clientOrderId">User-defined order ID, e.g 10000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel multiple orders.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade/Cancel-Multiple-Option-Orders" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="orderIdList">Order ID, e.g [4611875134427365377,4611875134427365378]</param>
    /// <param name="origClientOrderIdList">User-defined order ID, e.g ["my_id_1","my_id_2"]</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsOrder>>> CancelOrdersAsync(string symbol, IEnumerable<long>? orderIdList = null, IEnumerable<string>? origClientOrderIdList = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel all active orders on specified underlying.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade/Cancel-All-Option-Orders-By-Underlying" /></para>
    /// </summary>
    /// <param name="underlying">Option underlying, e.g BTCUSDT</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<long>> CancelOrdersByUnderlyingAsync(string underlying, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel all active order on a symbol.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade/Cancel-all-Option-orders-on-specific-symbol" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> CancelOrdersBySymbolAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Check an order status.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade/Query-Single-Order" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="orderId">Order ID, e.g 4611875134427365377</param>
    /// <param name="clientOrderId">User-defined order ID, e.g 10000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsOrder>> GetOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query all finished orders within 5 days, finished status: CANCELLED FILLED REJECTED.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade/Query-Option-Order-History" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="orderId">Order ID, e.g 4611875134427365377</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Number of result sets returned Default:100 Max:1000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsOrder>>> GetOrdersHistoryAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query current all open orders, status: ACCEPTED PARTIALLY_FILLED
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade/Query-Current-Open-Option-Orders" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="orderId">Order ID, e.g 4611875134427365377</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Number of result sets returned Default:100 Max:1000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsOrder>>> GetOpenOrdersAsync(string? symbol = null, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get current position information.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade/Option-Position-Information" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsPosition>>> GetPositionsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get account exercise records.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade/User-Exercise-Record" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Number of result sets returned Default:100 Max:1000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsUserExercise>>> GetUserExerciseRecordsAsync(string? symbol = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get trades for a specific account and symbol.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/trade/Account-Trade-List" /></para>
    /// </summary>
    /// <param name="symbol">Option trading pair, e.g BTC-200730-9000-C</param>
    /// <param name="fromId">Trade id to fetch from. Default gets most recent trades, e.g 4611875134427365376</param>
    /// <param name="startTime">Start Time</param>
    /// <param name="endTime">End Time</param>
    /// <param name="limit">Number of result sets returned Default:100 Max:1000</param>
    /// <param name="receiveWindow">Receive Window</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceOptionsUserTrade>>> GetUserTradesAsync(string? symbol = null, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);
}