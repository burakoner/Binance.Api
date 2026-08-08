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
    /// Gets forced-liquidation records in descending update-time order.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#get-force-liquidation-record" /></para>
    /// </summary>
    /// <param name="startTime">Optional inclusive start time.</param>
    /// <param name="endTime">Optional inclusive end time.</param>
    /// <param name="isolatedSymbol">Optional Isolated Margin symbol filter.</param>
    /// <param name="current">Page number, minimum 1.</param>
    /// <param name="size">Records per page, between 1 and 100.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginForcedLiquidationResult>> GetMarginForcedLiquidationHistoryAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? isolatedSymbol = null,
        long? current = null,
        long? size = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Gets assets eligible for Cross Margin small-liability exchange.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#get-small-liability-exchange-coin-list" /></para>
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginSmallLiabilityAsset>>> GetSmallLiabilityExchangeAssetsAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets Cross Margin small-liability exchange history.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#get-small-liability-exchange-history" /></para>
    /// </summary>
    /// <param name="current">Page number, minimum 1. Binance requires the field; defaults to 1.</param>
    /// <param name="size">Records per page, between 1 and 100. Binance requires the field; defaults to 10.</param>
    /// <param name="startTime">Optional inclusive start time. Binance defaults to 30 days before the request.</param>
    /// <param name="endTime">Optional inclusive end time. Binance defaults to the request time.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginSmallLiabilityHistoryResult>> GetSmallLiabilityExchangeHistoryAsync(
        long current = 1,
        long size = 10,
        DateTime? startTime = null,
        DateTime? endTime = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

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
    /// Gets current Margin order-count usage.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-current-margin-order-count-usage" /></para>
    /// </summary>
    /// <param name="isIsolated">Whether to query Isolated Margin; defaults to Cross Margin.</param>
    /// <param name="symbol">Required when <paramref name="isIsolated" /> is true.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginCurrentOrderCountUsage>>> GetMarginOrderCountUsageAsync(bool? isIsolated = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves Margin OCO order lists matching the parameters.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-margin-accounts-all-oco" /></para>
    /// </summary>
    /// <param name="symbol">Mandatory for isolated margin, not supported for cross margin, for example `ETHUSDT`</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="fromId">Only return oco orders with id higher than this</param>
    /// <param name="startTime">Only return oco orders placed later than this. Only valid if fromId isn't provided</param>
    /// <param name="endTime">Only return oco orders placed before this. Only valid if fromId isn't provided</param>
    /// <param name="limit">Maximum results, between 1 and 1000; defaults to 500.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Order lists matching the parameters.</returns>
    Task<RestCallResult<List<BinanceMarginOrderOcoList>>> GetMarginOcoOrdersAsync(string? symbol = null, bool? isIsolated = null, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, long? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets all margin account orders for the provided symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-margin-accounts-all-orders" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get orders for, for example `ETHUSDT`</param>
    /// <param name="isIsolated">For isolated margin or not</param>
    /// <param name="orderId">If set, only orders with an order id higher than the provided will be returned</param>
    /// <param name="startTime">Optional start time. Together with <paramref name="endTime" />, the range must be less than 24 hours.</param>
    /// <param name="endTime">Optional end time. Together with <paramref name="startTime" />, the range must be less than 24 hours.</param>
    /// <param name="limit">Maximum results, between 1 and 500.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of Margin account orders.</returns>
    Task<RestCallResult<List<BinanceMarginOrder>>> GetMarginOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, long? limit = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves one Margin OCO order list. Either orderListId or origClientOrderId must be provided.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-margin-accounts-oco" /></para>
    /// </summary>
    /// <param name="symbol">Mandatory for isolated margin, not supported for cross margin, for example `ETHUSDT`</param>
    /// <param name="isIsolated">Whether to query Isolated Margin; defaults to Cross Margin.</param>
    /// <param name="orderListId">The list order id of the order</param>
    /// <param name="origClientOrderId">Original client order-list identifier. Either this or <paramref name="orderListId" /> must be provided.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The specific order list.</returns>
    Task<RestCallResult<BinanceMarginOrderOcoList>> GetMarginOcoOrderAsync(string? symbol = null, bool? isIsolated = null, long? orderListId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves open Margin OCO order lists.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-margin-accounts-open-oco" /></para>
    /// </summary>
    /// <param name="symbol">Mandatory for isolated margin, not supported for cross margin, for example `ETHUSDT`</param>
    /// <param name="isIsolated">Whether to query Isolated Margin; defaults to Cross Margin.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Open order lists.</returns>
    Task<RestCallResult<List<BinanceMarginOrderOcoList>>> GetMarginOpenOcoOrdersAsync(string? symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets open Margin account orders.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-margin-accounts-open-orders" /></para>
    /// </summary>
    /// <param name="symbol">Optional symbol filter for Cross Margin; required for Isolated Margin.</param>
    /// <param name="isIsolated">Whether to query Isolated Margin; defaults to Cross Margin.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of open Margin account orders.</returns>
    Task<RestCallResult<List<BinanceMarginOrder>>> GetOpenMarginOrdersAsync(string? symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieves one Margin account order. Either orderId or origClientOrderId must be provided.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-margin-accounts-order" /></para>
    /// </summary>
    /// <param name="symbol">The symbol the order is for, for example `ETHUSDT`</param>
    /// <param name="isIsolated">Whether to query Isolated Margin; defaults to Cross Margin.</param>
    /// <param name="orderId">The order id of the order</param>
    /// <param name="origClientOrderId">The client order id of the order</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The specific Margin account order.</returns>
    Task<RestCallResult<BinanceMarginOrder>> GetMarginOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets Margin account trades for a symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-margin-accounts-trade-list" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get trades for, for example `ETHUSDT`</param>
    /// <param name="orderId">Trades associated with orderId</param>
    /// <param name="startTime">Optional start time. Together with <paramref name="endTime" />, the range must be less than 24 hours.</param>
    /// <param name="endTime">Optional end time. Together with <paramref name="startTime" />, the range must be less than 24 hours.</param>
    /// <param name="limit">Maximum results, between 1 and 1000; defaults to 500.</param>
    /// <param name="fromId">TradeId to fetch from. Default gets most recent trades</param>
    /// <param name="isIsolated">Whether to query Isolated Margin; defaults to Cross Margin.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of Margin account trades.</returns>
    Task<RestCallResult<List<BinanceMarginTrade>>> GetMarginUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, long? limit = null, long? fromId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Queries prevented Margin matches for one symbol. Binance returns at most 500 matches per request.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-prevented-matches" /></para>
    /// </summary>
    /// <param name="symbol">Trading symbol, for example <c>BTCUSDT</c>.</param>
    /// <param name="preventedMatchId">A specific prevented-match identifier. Cannot be combined with order identifiers.</param>
    /// <param name="orderId">Order identifier whose prevented matches should be returned.</param>
    /// <param name="fromPreventedMatchId">Pagination cursor. Valid only together with <paramref name="orderId" />.</param>
    /// <param name="isIsolated">Whether to query an Isolated Margin account; defaults to Cross Margin.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginPreventedMatch>>> GetMarginPreventedMatchesAsync(
        string symbol,
        long? preventedMatchId = null,
        long? orderId = null,
        long? fromPreventedMatchId = null,
        bool? isIsolated = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Cross Margin Small Liability Exchange
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade/Small-Liability-Exchange" /></para>
    /// </summary>
    /// <param name="assets">Assets, for example `ETH`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> SmallLiabilityExchangeAsync(IEnumerable<string> assets, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Creates a low-latency Margin Special Key. Binance currently limits general eligibility to VIP
    /// level 7 or higher and requires acceptance of the Supplemental Product Terms. Cross Margin,
    /// Isolated Margin, and Portfolio Margin Pro are supported; Portfolio Margin is not.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#create-special-key" /></para>
    /// </summary>
    /// <param name="apiName">Required name for the new key.</param>
    /// <param name="symbol">Isolated Margin symbol. Omit for Cross Margin.</param>
    /// <param name="ipAddresses">Optional IP restrictions, between 1 and 30 entries.</param>
    /// <param name="publicKey">Optional RSA or Ed25519 public key. The wrapper form-encodes the value.</param>
    /// <param name="permissionMode">Optional Ed25519 permission mode; Binance defaults to Trade.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The newly issued API key and any server-issued secret.</returns>
    Task<RestCallResult<BinanceMarginSpecialKeyCreateResult>> CreateMarginSpecialKeyAsync(
        string apiName,
        string? symbol = null,
        IEnumerable<string>? ipAddresses = null,
        string? publicKey = null,
        BinanceMarginSpecialKeyPermissionMode? permissionMode = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Deletes a Margin Special Key by API key or every matching key by API name. When both are
    /// supplied, Binance ignores the API name. Deleting a key does not exit Special Key Mode.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#delete-special-key" /></para>
    /// </summary>
    /// <param name="apiKey">Specific Special Key to delete.</param>
    /// <param name="apiName">Name whose matching Special Keys should be deleted.</param>
    /// <param name="symbol">Isolated Margin symbol. Omit for Cross Margin.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<bool>> DeleteMarginSpecialKeyAsync(
        string? apiKey = null,
        string? apiName = null,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Replaces the IP restrictions for one Margin Special Key.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#edit-ip-for-special-key" /></para>
    /// </summary>
    /// <param name="apiKey">Special Key whose restrictions should be replaced.</param>
    /// <param name="ipAddresses">Required IP restrictions, between 1 and 30 entries.</param>
    /// <param name="symbol">Isolated Margin symbol. Omit for Cross Margin.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<bool>> UpdateMarginSpecialKeyIpAsync(
        string apiKey,
        IEnumerable<string> ipAddresses,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Exits Special Key Mode for a Cross Margin Classic account. On success Binance atomically
    /// deletes all existing Margin Special Keys, restores standard pre-execution checks, and starts a
    /// cooldown period. The account must not be in liquidation and must have no outstanding liability.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#exit-special-key-mode" /></para>
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<bool>> ExitMarginSpecialKeyModeAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets one Margin Special Key by its API key.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-special-key" /></para>
    /// </summary>
    /// <param name="apiKey">Required Special Key identifier.</param>
    /// <param name="symbol">Isolated Margin symbol. Omit for Cross Margin.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginSpecialKey>> GetMarginSpecialKeyAsync(
        string apiKey,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Gets the Margin Special Keys for the selected Cross or Isolated Margin scope.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/trade#query-special-key-list" /></para>
    /// </summary>
    /// <param name="symbol">Isolated Margin symbol. Omit for Cross Margin.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginSpecialKey>>> GetMarginSpecialKeysAsync(
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default);
}
