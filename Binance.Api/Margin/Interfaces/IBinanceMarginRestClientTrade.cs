using Binance.Api.Spot;

namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Trade Methods
/// </summary>
public interface IBinanceMarginRestClientTrade
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
    /// Get history of forced liquidations
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade" /></para>
    /// </summary>
    /// <param name="page">Results page</param>
    /// <param name="startTime">Filter by startTime from</param>
    /// <param name="endTime">Filter by endTime from</param>
    /// <param name="isolatedSymbol">Filter by isolated symbol</param>
    /// <param name="limit">Limit of the amount of results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of forced liquidations</returns>
    Task<RestCallResult<BinanceRowsResult<BinanceMarginForcedLiquidation>>> GetMarginForcedLiquidationHistoryAsync(int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query the coins which can be small liability exchange
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Get-Small-Liability-Exchange-Coin-List" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceMarginSmallLiabilityAsset>>> GetSmallLiabilityExchangeAssetsAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Small liability Exchange History
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Get-Small-Liability-Exchange-History" /></para>
    /// </summary>
    /// <param name="startTime">Filter by startTime</param>
    /// <param name="endTime">Filter by endTime</param>
    /// <param name="page">The page</param>
    /// <param name="limit">Results per page</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceRowsResult<BinanceMarginSmallLiabilityHistory>>> GetSmallLiabilityExchangeHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Manually liquidates a Cross or supported Isolated Margin account.
    /// Cross Margin Classic and Pro are supported; Isolated Margin is available only in restricted regions.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#margin-manual-liquidation" /></para>
    /// </summary>
    /// <param name="type">Account scope to liquidate.</param>
    /// <param name="symbol">Required when liquidating an Isolated Margin account.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginManualLiquidation>> LiquidateMarginAccountAsync(BinanceMarginLiquidationType type, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the current Cross Margin liquidation-loan balance.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-liquidation-loan" /></para>
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginLiquidationLoan>> GetLiquidationLoanAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Repays a Cross Margin liquidation loan from the Spot wallet.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#liquidation-loan-repay" /></para>
    /// </summary>
    /// <param name="asset">Asset used for repayment.</param>
    /// <param name="amount">Amount to repay; must be greater than zero and cannot exceed the outstanding balance.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginLiquidationLoanRepayment>> RepayLiquidationLoanAsync(string asset, decimal amount, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets Cross Margin liquidation-loan repayment history. Binance returns only SUCCESS and PENDING records
    /// and limits the available history to the most recent 90 days.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-liquidation-loan-repay-history" /></para>
    /// </summary>
    /// <param name="startTime">Start time; defaults to seven days ago.</param>
    /// <param name="endTime">End time; defaults to now.</param>
    /// <param name="current">Page number; server default is 1.</param>
    /// <param name="size">Page size; server default is 50.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginLiquidationLoanRepaymentHistory>> GetLiquidationLoanRepaymentHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, long? current = null, long? size = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel all active orders for a symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#margin-account-cancel-all-open-orders-on-a-symbol-trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the to cancel orders for, for example `ETHUSDT`</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for canceled order</returns>
    Task<RestCallResult<List<BinanceMarginCanceledOrder>>> CancelAllMarginOrdersAsync(string symbol, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancels a pending margin oco order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#margin-account-cancel-oco-trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="orderListId">The id of the order list to cancel</param>
    /// <param name="listClientOrderId">The client order id of the order list to cancel</param>
    /// <param name="newClientOrderId">The new client order list id for the order list</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for canceled order</returns>
    Task<RestCallResult<BinanceMarginOrderOcoList>> CancelMarginOcoOrderAsync(string symbol, bool? isIsolated = null, long? orderListId = null, string? listClientOrderId = null, string? newClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel an active order for margin account
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#margin-account-cancel-order-trade" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="newClientOrderId">Unique identifier for this cancel</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for canceled order</returns>
    Task<RestCallResult<BinanceSpotOrderBase>> CancelMarginOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Places a new margin OCO(One cancels other) order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#margin-account-new-oco" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="side">The order side (buy/sell)</param>
    /// <param name="stopLimitTimeInForce">Lifetime of the stop order (GoodTillCancel/ImmediateOrCancel/FillOrKill)</param>
    /// <param name="quantity">The quantity of the symbol</param>
    /// <param name="price">The price to use</param>
    /// <param name="stopPrice">The stop price</param>
    /// <param name="stopLimitPrice">The price for the stop limit order</param>
    /// <param name="stopClientOrderId">Client id for the stop order</param>
    /// <param name="limitClientOrderId">Client id for the limit order</param>
    /// <param name="listClientOrderId">Client id for the order list</param>
    /// <param name="limitIcebergQuantity">Iceberg quantity for the limit order</param>
    /// <param name="sideEffectType">Side effect type</param>
    /// <param name="isIsolated">Is isolated</param>
    /// <param name="orderResponseType">Order response type</param>
    /// <param name="stopIcebergQuantity">Iceberg quantity for the stop order</param>
    /// <param name="selfTradePreventionMode">Self trade prevention mode</param>
    /// <param name="autoRepayAtCancel">Only when MARGIN_BUY or AUTO_BORROW_REPAY order takes effect, true means that the debt generated by the order needs to be repay after the order is cancelled. The default is true</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Order list info</returns>
    Task<RestCallResult<BinanceMarginOrderOcoList>> PlaceMarginOCOOrderAsync(string symbol, BinanceOrderSide side, decimal price, decimal stopPrice, decimal quantity, decimal? stopLimitPrice = null, BinanceTimeInForce? stopLimitTimeInForce = null, decimal? stopIcebergQuantity = null, decimal? limitIcebergQuantity = null, BinanceMarginSideEffectType? sideEffectType = null, bool? isIsolated = null, string? listClientOrderId = null, string? limitClientOrderId = null, string? stopClientOrderId = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, bool? autoRepayAtCancel = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Places a Margin one-triggers-the-other order list.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#margin-account-new-oto" /></para>
    /// </summary>
    /// <param name="request">Current OTO request contract.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginOrderList>> PlaceMarginOtoOrderAsync(BinanceMarginOtoOrderListRequest request, CancellationToken ct = default);

    /// <summary>
    /// Places a Margin one-triggers-one-cancels-the-other order list.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#margin-account-new-otoco" /></para>
    /// </summary>
    /// <param name="request">Current OTOCO request contract.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginOrderList>> PlaceMarginOtocoOrderAsync(BinanceMarginOtocoOrderListRequest request, CancellationToken ct = default);

    /// <summary>
    /// Margin account new order
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#margin-account-new-order" /></para>
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
    /// <param name="sideEffectType">Side effect type for this order</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="orderResponseType">Used for the response JSON</param>
    /// <param name="selfTradePreventionMode">Self trade prevention mode</param>
    /// <param name="trailingDelta">Trailing delta used by supported stop and take-profit orders</param>
    /// <param name="autoRepayAtCancel">Only when MARGIN_BUY or AUTO_BORROW_REPAY order takes effect, true means that the debt generated by the order needs to be repay after the order is cancelled. The default is true</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Id's for the placed order</returns>
    Task<RestCallResult<BinancePlacedOrder>> PlaceMarginOrderAsync(string symbol, BinanceOrderSide side, BinanceSpotOrderType type, decimal? quantity = null, decimal? quoteQuantity = null, string? newClientOrderId = null, decimal? price = null, BinanceTimeInForce? timeInForce = null, decimal? stopPrice = null, decimal? icebergQuantity = null, BinanceMarginSideEffectType? sideEffectType = null, bool? isIsolated = null, BinanceOrderResponseType? orderResponseType = null, BinanceSelfTradePreventionMode? selfTradePreventionMode = null, long? trailingDelta = null, bool? autoRepayAtCancel = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get isolated margin order rate limits
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Query-Current-Margin-Order-Count-Usage" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceCurrentRateLimit>>> GetMarginRateLimitsAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a list of margin oco orders matching the parameters
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Query-Margin-Account-All-OCO" /></para>
    /// </summary>
    /// <param name="symbol">Mandatory for isolated margin, not supported for cross margin, for example `ETHUSDT`</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="fromId">Only return oco orders with id higher than this</param>
    /// <param name="startTime">Only return oco orders placed later than this. Only valid if fromId isn't provided</param>
    /// <param name="endTime">Only return oco orders placed before this. Only valid if fromId isn't provided</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Order lists matching the parameters</returns>
    Task<RestCallResult<List<BinanceMarginOrderOcoList>>> GetMarginOcoOrdersAsync(string? symbol = null, bool? isIsolated = null, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets all margin account orders for the provided symbol
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Query-Margin-Account-All-Orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get orders for, for example `ETHUSDT`</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="orderId">If set, only orders with an order id higher than the provided will be returned</param>
    /// <param name="startTime">If set, only orders placed after this time will be returned</param>
    /// <param name="endTime">If set, only orders placed before this time will be returned</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of margin account orders</returns>
    Task<RestCallResult<List<BinanceMarginOrder>>> GetMarginOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves data for a specific margin oco order. Either orderListId or listClientOrderId should be provided.
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Query-Margin-Account-OCO" /></para>
    /// </summary>
    /// <param name="symbol">Mandatory for isolated margin, not supported for cross margin, for example `ETHUSDT`</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="orderListId">The list order id of the order</param>
    /// <param name="origClientOrderId">Either orderListId or listClientOrderId must be provided</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The specific order list</returns>
    Task<RestCallResult<BinanceMarginOrderOcoList>> GetMarginOcoOrderAsync(string? symbol = null, bool? isIsolated = null, long? orderListId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a list of open margin oco orders
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Query-Margin-Account-Open-OCO" /></para>
    /// </summary>
    /// <param name="symbol">Mandatory for isolated margin, not supported for cross margin, for example `ETHUSDT`</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Open order lists</returns>
    Task<RestCallResult<List<BinanceMarginOrderOcoList>>> GetMarginOpenOcoOrdersAsync(string? symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets a list of open margin account orders
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Query-Margin-Account-Open-Orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get open orders for, for example `ETHUSDT`</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of open margin account orders</returns>
    Task<RestCallResult<List<BinanceMarginOrder>>> GetOpenMarginOrdersAsync(string? symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves data for a specific margin account order. Either orderId or origClientOrderId should be provided.
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Query-Margin-Account-Order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The specific margin account order</returns>
    Task<RestCallResult<BinanceMarginOrder>> GetMarginOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets all user margin account trades for provided symbol
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Query-Margin-Account-Trade-List" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get trades for, for example `ETHUSDT`</param>
    /// <param name="orderId">Trades associated with orderId</param>
    /// <param name="startTime">Orders newer than this date will be retrieved</param>
    /// <param name="endTime">Orders older than this date will be retrieved</param>
    /// <param name="limit">The max number of results</param>
    /// <param name="fromId">TradeId to fetch from. Default gets most recent trades</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of margin account trades</returns>
    Task<RestCallResult<List<BinanceMarginTrade>>> GetMarginUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Cross Margin Small Liability Exchange
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Small-Liability-Exchange" /></para>
    /// </summary>
    /// <param name="assets">Assets, for example `ETH`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> SmallLiabilityExchangeAsync(IEnumerable<string> assets, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Create Special Key(Low-Latency Trading)(TRADE)
    // TODO: Delete Special Key(Low-Latency Trading)(TRADE)
    // TODO: Edit ip for Special Key(Low-Latency Trading)(TRADE)
    // TODO: Query Special key List(Low Latency Trading)(TRADE)
    // TODO: Query Special key(Low Latency Trading)(TRADE)
}
