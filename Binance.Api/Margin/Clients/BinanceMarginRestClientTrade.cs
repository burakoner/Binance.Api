using Binance.Api.Spot;

namespace Binance.Api.Margin;

internal partial class BinanceMarginRestClient
{
    public event Action<long>? OnOrderPlaced;
    public event Action<long>? OnOrderCanceled;

    internal void InvokeOrderPlaced(long id) => OnOrderPlaced?.Invoke(id);
    internal void InvokeOrderCanceled(long id) => OnOrderCanceled?.Invoke(id);

    public Task<RestCallResult<BinanceQueryRecords<BinanceMarginForcedLiquidation>>> GetMarginForcedLiquidationHistoryAsync(int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 100);

        var parameters = new ParameterCollection();
        parameters.AddOptional("size", limit);
        parameters.AddOptional("page", page);
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceMarginForcedLiquidation>>(GetUrl(sapi, v1, "margin/forceLiquidationRec"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceMarginSmallLiabilityAsset>>> GetSmallLiabilityExchangeAssetsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginSmallLiabilityAsset>>(GetUrl(sapi, v1, "margin/exchange-small-liability"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceMarginSmallLiabilityHistory>>> GetSmallLiabilityExchangeHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceMarginSmallLiabilityHistory>>(GetUrl(sapi, v1, "margin/exchange-small-liability-history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<List<BinanceSpotOrderBase>>> CancelAllMarginOrdersAsync(string symbol, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptional("isIsolated", isIsolated);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceSpotOrderBase>>(GetUrl(sapi, v1, "margin/openOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceMarginOrderOcoList>> CancelMarginOcoOrderAsync(string symbol, bool? isIsolated = null, long? orderListId = null, string? listClientOrderId = null, string? newClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderListId.HasValue && string.IsNullOrEmpty(listClientOrderId))
            throw new ArgumentException("Either orderListId or listClientOrderId must be sent");

        if (listClientOrderId != null)
            listClientOrderId = BinanceHelpers.ApplyBrokerId(listClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        if (newClientOrderId != null)
            newClientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptional("isIsolated", isIsolated?.ToString());
        parameters.AddOptional("orderListId", orderListId);
        parameters.AddOptional("listClientOrderId", listClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginOrderOcoList>(GetUrl(sapi, v1, "margin/orderList"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<BinanceSpotOrderBase>> CancelMarginOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        if (origClientOrderId != null)
            origClientOrderId = BinanceHelpers.ApplyBrokerId(origClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        if (newClientOrderId != null)
            newClientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("isIsolated", isIsolated);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));


        var result = await RequestAsync<BinanceSpotOrderBase>(GetUrl(sapi, v1, "margin/order"), HttpMethod.Get, ct, true, bodyParameters: parameters, requestWeight: 10).ConfigureAwait(false);
        if (result) InvokeOrderCanceled(result.Data.Id);
        return result;
    }

    public async Task<RestCallResult<BinanceMarginOrderOcoList>> PlaceMarginOCOOrderAsync(string symbol,
        BinanceOrderSide side,
        decimal price,
        decimal stopPrice,
        decimal quantity,
        decimal? stopLimitPrice = null,
        BinanceTimeInForce? stopLimitTimeInForce = null,
        decimal? stopIcebergQuantity = null,
        decimal? limitIcebergQuantity = null,
        BinanceMarginSideEffectType? sideEffectType = null,
        bool? isIsolated = null,
        string? listClientOrderId = null,
        string? limitClientOrderId = null,
        string? stopClientOrderId = null,
        BinanceOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        bool? autoRepayAtCancel = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var rulesCheck = await ((BinanceSpotRestClient)_.Spot).CheckTradingRulesAsync(symbol, null, quantity, null, price, stopPrice, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            _.Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceMarginOrderOcoList>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity!.Value;
        price = rulesCheck.Price!.Value;
        stopPrice = rulesCheck.StopPrice!.Value;

        limitClientOrderId = BinanceHelpers.ApplyBrokerId(limitClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);
        stopClientOrderId = BinanceHelpers.ApplyBrokerId(stopClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId); ;

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "quantity", quantity.ToString(BinanceConstants.CI) },
            { "price", price.ToString(BinanceConstants.CI) },
            { "stopPrice", stopPrice.ToString(BinanceConstants.CI) }
        };
        parameters.AddEnum("side", side);
        parameters.AddOptional("stopLimitPrice", stopLimitPrice?.ToString(BinanceConstants.CI));
        parameters.AddOptional("isIsolated", isIsolated?.ToString());
        parameters.AddOptionalEnum("sideEffectType", sideEffectType);
        parameters.AddOptional("listClientOrderId", listClientOrderId);
        parameters.AddOptional("limitClientOrderId", limitClientOrderId);
        parameters.AddOptional("stopClientOrderId", stopClientOrderId);
        parameters.AddOptional("limitIcebergQty", limitIcebergQuantity?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("stopIcebergQty", stopIcebergQuantity?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("stopLimitTimeInForce", stopLimitTimeInForce);
        parameters.AddOptional("autoRepayAtCancel", autoRepayAtCancel);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return await RequestAsync<BinanceMarginOrderOcoList>(GetUrl(sapi, v1, "margin/order/oco"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinancePlacedOrder>> PlaceMarginOrderAsync(string symbol,
        BinanceOrderSide side,
        BinanceSpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        string? newClientOrderId = null,
        decimal? price = null,
        BinanceTimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        BinanceMarginSideEffectType? sideEffectType = null,
        bool? isIsolated = null,
        BinanceOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        bool? autoRepayAtCancel = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await ((BinanceSpotRestClient)_.Spot).CheckTradingRulesAsync(symbol, type, quantity, quoteQuantity, price, stopPrice, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinancePlacedOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;
        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity?.ToString(BinanceConstants.CI));
        parameters.AddOptional("quoteOrderQty", quoteQuantity?.ToString(BinanceConstants.CI));
        parameters.AddOptional("price", price?.ToString(BinanceConstants.CI));
        parameters.AddOptional("stopPrice", stopPrice?.ToString(BinanceConstants.CI));
        parameters.AddOptional("icebergQty", icebergQuantity?.ToString(BinanceConstants.CI));
        parameters.AddOptional("newClientOrderId", clientOrderId);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinancePlacedOrder>(GetUrl(sapi, v1, "margin/order"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6).ConfigureAwait(false);
        if (result) InvokeOrderPlaced(result.Data.Id);

        return result;
    }

    public Task<RestCallResult<List<BinanceCurrentRateLimit>>> GetMarginRateLimitsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceCurrentRateLimit>>(GetUrl(sapi, v1, "margin/rateLimit/order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<List<BinanceMarginOrderOcoList>>> GetMarginOcoOrdersAsync(string? symbol = null, bool? isIsolated = null, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (fromId != null && (startTime != null || endTime != null))
            throw new ArgumentException("Start/end time can only be provided without fromId parameter");

        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("isIsolated", isIsolated?.ToString());
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginOrderOcoList>>(GetUrl(sapi, v1, "margin/allOrderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 200);
    }

    public Task<RestCallResult<List<BinanceMarginOrder>>> GetMarginOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 500);

        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("isIsolated", isIsolated);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit);

        return RequestAsync<List<BinanceMarginOrder>>(GetUrl(sapi, v1, "margin/allOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 200);
    }

    public Task<RestCallResult<BinanceMarginOrderOcoList>> GetMarginOcoOrderAsync(string? symbol = null, bool? isIsolated = null, long? orderListId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderListId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderListId or origClientOrderId must be sent");

        if (origClientOrderId != null)
            origClientOrderId = BinanceHelpers.ApplyBrokerId(origClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("isIsolated", isIsolated.ToString());
        parameters.AddOptional("orderListId", orderListId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginOrderOcoList>(GetUrl(sapi, v1, "margin/orderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceMarginOrderOcoList>>> GetMarginOpenOcoOrdersAsync(string? symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("isIsolated", isIsolated?.ToString());
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginOrderOcoList>>(GetUrl(sapi, v1, "margin/openOrderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceMarginOrder>>> GetOpenMarginOrdersAsync(string? symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (isIsolated == true && symbol == null)
            throw new ArgumentException("Symbol must be provided for isolated margin");

        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("isIsolated", isIsolated);

        return RequestAsync<List<BinanceMarginOrder>>(GetUrl(sapi, v1, "margin/openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceMarginOrder>> GetMarginOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId should be provided");

        if (origClientOrderId != null)
            origClientOrderId = BinanceHelpers.ApplyBrokerId(origClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("isIsolated", isIsolated);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginOrder>(GetUrl(sapi, v1, "margin/order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceMarginTrade>>> GetMarginUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null,
        int? limit = null, long? fromId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("isIsolated", isIsolated);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginTrade>>(GetUrl(sapi, v1, "margin/myTrades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public async Task<RestCallResult<bool>> SmallLiabilityExchangeAsync(IEnumerable<string> assets, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "assetNames", string.Join(",", assets) }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(GetUrl(sapi, v1, "margin/exchange-small-liability"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
        return result.As(result.Success);
    }

}