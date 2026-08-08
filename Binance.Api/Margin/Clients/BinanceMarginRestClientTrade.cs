using Binance.Api.Spot;

namespace Binance.Api.Margin;

internal partial class BinanceMarginRestClient
{
    public event Action<long>? OnOrderPlaced;
    public event Action<long>? OnOrderCanceled;

    internal void InvokeOrderPlaced(long id) => OnOrderPlaced?.Invoke(id);
    internal void InvokeOrderCanceled(long id) => OnOrderCanceled?.Invoke(id);

    public Task<RestCallResult<BinanceMarginForcedLiquidationResult>> GetMarginForcedLiquidationHistoryAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? isolatedSymbol = null,
        long? current = null,
        long? size = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (startTime > endTime)
            throw new ArgumentException("startTime cannot be later than endTime", nameof(startTime));
        ValidateOptionalMarginSymbol(isolatedSymbol, nameof(isolatedSymbol));
        ValidateMarginPagination(current, size);

        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);
        parameters.AddOptional("current", current);
        parameters.AddOptional("size", size);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginForcedLiquidationResult>(GetUrl(sapi, v1, "margin/forceLiquidationRec"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceMarginSmallLiabilityAsset>>> GetSmallLiabilityExchangeAssetsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginSmallLiabilityAsset>>(GetUrl(sapi, v1, "margin/exchange-small-liability"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceMarginSmallLiabilityHistoryResult>> GetSmallLiabilityExchangeHistoryAsync(
        long current = 1,
        long size = 10,
        DateTime? startTime = null,
        DateTime? endTime = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (startTime > endTime)
            throw new ArgumentException("startTime cannot be later than endTime", nameof(startTime));
        ValidateMarginPagination(current, size);

        var parameters = new ParameterCollection();
        parameters.Add("current", current);
        parameters.Add("size", size);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginSmallLiabilityHistoryResult>(GetUrl(sapi, v1, "margin/exchange-small-liability-history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceMarginManualLiquidation>> LiquidateMarginAccountAsync(
        BinanceMarginLiquidationType type,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (type == BinanceMarginLiquidationType.IsolatedMargin && string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol is required for isolated Margin liquidation", nameof(symbol));
        if (symbol != null)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("symbol cannot be empty when provided", nameof(symbol));
            symbol.ValidateBinanceSymbol();
        }

        var normalizedReceiveWindow = ValidateMarginReceiveWindow(receiveWindow);
        var parameters = new ParameterCollection();
        parameters.AddEnum("type", type);
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", normalizedReceiveWindow);

        return RequestAsync<BinanceMarginManualLiquidation>(GetUrl(sapi, v1, "margin/manual-liquidation"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3_000);
    }

    public Task<RestCallResult<BinanceMarginLiquidationLoan>> GetLiquidationLoanAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginLiquidationLoan>(GetUrl(sapi, v1, "margin/liquidation-loan"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceMarginLiquidationLoanRepayment>> RepayLiquidationLoanAsync(
        string asset,
        decimal amount,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(asset))
            throw new ArgumentException("asset is required", nameof(asset));
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "amount must be greater than zero");

        var parameters = new ParameterCollection
        {
            { "asset", asset },
            { "amount", amount.ToString(BinanceConstants.CI) }
        };
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginLiquidationLoanRepayment>(GetUrl(sapi, v1, "margin/liquidation-loan/repay"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 100);
    }

    public Task<RestCallResult<BinanceMarginLiquidationLoanRepaymentHistory>> GetLiquidationLoanRepaymentHistoryAsync(
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? current = null,
        long? size = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (startTime > endTime)
            throw new ArgumentException("startTime cannot be later than endTime", nameof(startTime));
        if (startTime.HasValue && endTime.HasValue && endTime.Value - startTime.Value > TimeSpan.FromDays(90))
            throw new ArgumentException("The explicit liquidation-loan repayment history range cannot exceed 90 days", nameof(endTime));
        if (current is <= 0)
            throw new ArgumentOutOfRangeException(nameof(current), "current must be greater than zero when provided");
        if (size is <= 0)
            throw new ArgumentOutOfRangeException(nameof(size), "size must be greater than zero when provided");

        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", current);
        parameters.AddOptional("size", size);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginLiquidationLoanRepaymentHistory>(GetUrl(sapi, v1, "margin/liquidation-loan/repay-history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    private int? ValidateMarginReceiveWindow(int? receiveWindow)
    {
        var normalizedReceiveWindow = _.ReceiveWindow(receiveWindow);
        if (normalizedReceiveWindow > 60_000)
            throw new ArgumentOutOfRangeException(nameof(receiveWindow), "receiveWindow cannot exceed 60000 milliseconds");
        return normalizedReceiveWindow;
    }

    public Task<RestCallResult<List<BinanceMarginCanceledOrder>>> CancelAllMarginOrdersAsync(string symbol, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginCanceledOrder>>(GetUrl(sapi, v1, "margin/openOrders"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceMarginOrderOcoList>> CancelMarginOcoOrderAsync(string symbol, bool? isIsolated = null, long? orderListId = null, string? listClientOrderId = null, string? newClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (!orderListId.HasValue && string.IsNullOrWhiteSpace(listClientOrderId))
            throw new ArgumentException("Either orderListId or listClientOrderId must be sent");

        if (listClientOrderId != null && string.IsNullOrWhiteSpace(listClientOrderId))
            throw new ArgumentException("listClientOrderId cannot be empty when provided", nameof(listClientOrderId));

        if (newClientOrderId != null)
            newClientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptional("orderListId", orderListId);
        parameters.AddOptional("listClientOrderId", listClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginOrderOcoList>(GetUrl(sapi, v1, "margin/orderList"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<BinanceSpotOrderBase>> CancelMarginOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (!orderId.HasValue && string.IsNullOrWhiteSpace(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        if (origClientOrderId != null && string.IsNullOrWhiteSpace(origClientOrderId))
            throw new ArgumentException("origClientOrderId cannot be empty when provided", nameof(origClientOrderId));

        if (newClientOrderId != null)
            newClientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));


        var result = await RequestAsync<BinanceSpotOrderBase>(GetUrl(sapi, v1, "margin/order"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
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
        symbol.ValidateBinanceSymbol();
        var normalizedReceiveWindow = ValidateMarginReceiveWindow(receiveWindow);
        if (stopLimitPrice.HasValue && !stopLimitTimeInForce.HasValue)
            throw new ArgumentException("stopLimitTimeInForce is required when stopLimitPrice is provided", nameof(stopLimitTimeInForce));

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
        stopClientOrderId = BinanceHelpers.ApplyBrokerId(stopClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "quantity", quantity.ToString(BinanceConstants.CI) },
            { "price", price.ToString(BinanceConstants.CI) },
            { "stopPrice", stopPrice.ToString(BinanceConstants.CI) }
        };
        parameters.AddEnum("side", side);
        parameters.AddOptional("stopLimitPrice", stopLimitPrice?.ToString(BinanceConstants.CI));
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
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
        parameters.AddOptional("recvWindow", normalizedReceiveWindow);

        return await RequestAsync<BinanceMarginOrderOcoList>(GetUrl(sapi, v1, "margin/order/oco"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: BinanceMarginOrderListRequestBuilder.RequestWeight(sideEffectType)).ConfigureAwait(false);
    }

    public async Task<RestCallResult<BinanceMarginOrderList>> PlaceMarginOtoOrderAsync(BinanceMarginOtoOrderListRequest request, CancellationToken ct = default)
    {
        string? ApplyClientOrderId(string? clientOrderId)
            => BinanceHelpers.ApplyBrokerId(clientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = BinanceMarginOrderListRequestBuilder.Oto(request, ApplyClientOrderId);
        var result = await RequestAsync<BinanceMarginOrderList>(
            GetUrl(sapi, v1, "margin/order/oto"),
            HttpMethod.Post,
            ct,
            true,
            bodyParameters: parameters,
            requestWeight: BinanceMarginOrderListRequestBuilder.RequestWeight(request.SideEffectType)).ConfigureAwait(false);
        if (result)
        {
            foreach (var order in result.Data.Orders)
                InvokeOrderPlaced(order.OrderId);
        }

        return result;
    }

    public async Task<RestCallResult<BinanceMarginOrderList>> PlaceMarginOtocoOrderAsync(BinanceMarginOtocoOrderListRequest request, CancellationToken ct = default)
    {
        string? ApplyClientOrderId(string? clientOrderId)
            => BinanceHelpers.ApplyBrokerId(clientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = BinanceMarginOrderListRequestBuilder.Otoco(request, ApplyClientOrderId);
        var result = await RequestAsync<BinanceMarginOrderList>(
            GetUrl(sapi, v1, "margin/order/otoco"),
            HttpMethod.Post,
            ct,
            true,
            bodyParameters: parameters,
            requestWeight: BinanceMarginOrderListRequestBuilder.RequestWeight(request.SideEffectType)).ConfigureAwait(false);
        if (result)
        {
            foreach (var order in result.Data.Orders)
                InvokeOrderPlaced(order.OrderId);
        }

        return result;
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
        long? trailingDelta = null,
        bool? autoRepayAtCancel = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        var normalizedReceiveWindow = ValidateMarginReceiveWindow(receiveWindow);
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
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptionalEnum("sideEffectType", sideEffectType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("autoRepayAtCancel", autoRepayAtCancel);
        parameters.AddOptional("recvWindow", normalizedReceiveWindow);

        var result = await RequestAsync<BinancePlacedOrder>(GetUrl(sapi, v1, "margin/order"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: BinanceMarginOrderListRequestBuilder.RequestWeight(sideEffectType)).ConfigureAwait(false);
        if (result) InvokeOrderPlaced(result.Data.Id);

        return result;
    }

    public Task<RestCallResult<List<BinanceMarginCurrentOrderCountUsage>>> GetMarginOrderCountUsageAsync(
        bool? isIsolated = null,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (isIsolated == true && string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol is required when isIsolated is true", nameof(symbol));
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));

        var parameters = new ParameterCollection();
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginCurrentOrderCountUsage>>(GetUrl(sapi, v1, "margin/rateLimit/order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<List<BinanceMarginOrderOcoList>>> GetMarginOcoOrdersAsync(string? symbol = null, bool? isIsolated = null, long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, long? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (fromId != null && (startTime != null || endTime != null))
            throw new ArgumentException("Start/end time can only be provided without fromId parameter");
        if (startTime > endTime)
            throw new ArgumentException("startTime cannot be later than endTime", nameof(startTime));
        ValidateOcoMarginScope(symbol, isIsolated);
        if (limit is <= 0 or > 1_000)
            throw new ArgumentOutOfRangeException(nameof(limit), "limit must be between 1 and 1000 when provided");

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginOrderOcoList>>(GetUrl(sapi, v1, "margin/allOrderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 200);
    }

    public Task<RestCallResult<List<BinanceMarginOrder>>> GetMarginOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, long? limit = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        ValidateMarginTradeDateRange(startTime, endTime);
        if (limit is <= 0 or > 500)
            throw new ArgumentOutOfRangeException(nameof(limit), "limit must be between 1 and 500 when provided");

        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit);

        return RequestAsync<List<BinanceMarginOrder>>(GetUrl(sapi, v1, "margin/allOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 200);
    }

    public Task<RestCallResult<BinanceMarginOrderOcoList>> GetMarginOcoOrderAsync(string? symbol = null, bool? isIsolated = null, long? orderListId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderListId == null && string.IsNullOrWhiteSpace(origClientOrderId))
            throw new ArgumentException("Either orderListId or origClientOrderId must be sent");
        ValidateOcoMarginScope(symbol, isIsolated);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptional("orderListId", orderListId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginOrderOcoList>(GetUrl(sapi, v1, "margin/orderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceMarginOrderOcoList>>> GetMarginOpenOcoOrdersAsync(string? symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateOcoMarginScope(symbol, isIsolated);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginOrderOcoList>>(GetUrl(sapi, v1, "margin/openOrderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceMarginOrder>>> GetOpenMarginOrdersAsync(string? symbol = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (isIsolated == true && string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol is required when isIsolated is true", nameof(symbol));
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginOrder>>(GetUrl(sapi, v1, "margin/openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceMarginOrder>> GetMarginOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (orderId == null && string.IsNullOrWhiteSpace(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId should be provided");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginOrder>(GetUrl(sapi, v1, "margin/order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<List<BinanceMarginTrade>>> GetMarginUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null,
        long? limit = null, long? fromId = null, bool? isIsolated = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        ValidateMarginTradeDateRange(startTime, endTime);
        if (limit is <= 0 or > 1_000)
            throw new ArgumentOutOfRangeException(nameof(limit), "limit must be between 1 and 1000 when provided");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginTrade>>(GetUrl(sapi, v1, "margin/myTrades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    private static void ValidateOcoMarginScope(string? symbol, bool? isIsolated)
    {
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));
        if (isIsolated == true && symbol == null)
            throw new ArgumentException("symbol is required when isIsolated is true", nameof(symbol));
        if (isIsolated != true && symbol != null)
            throw new ArgumentException("symbol is not supported for Cross Margin OCO queries", nameof(symbol));
    }

    private static void ValidateMarginTradeDateRange(DateTime? startTime, DateTime? endTime)
    {
        if (startTime > endTime)
            throw new ArgumentException("startTime cannot be later than endTime", nameof(startTime));
        if (startTime.HasValue && endTime.HasValue && endTime.Value - startTime.Value >= TimeSpan.FromHours(24))
            throw new ArgumentException("The explicit query range must be less than 24 hours", nameof(endTime));
    }

    public Task<RestCallResult<List<BinanceMarginPreventedMatch>>> GetMarginPreventedMatchesAsync(
        string symbol,
        long? preventedMatchId = null,
        long? orderId = null,
        long? fromPreventedMatchId = null,
        bool? isIsolated = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol is required", nameof(symbol));
        symbol.ValidateBinanceSymbol();

        var hasValidCombination =
            preventedMatchId.HasValue && !orderId.HasValue && !fromPreventedMatchId.HasValue
            || orderId.HasValue && !preventedMatchId.HasValue;
        if (!hasValidCombination)
        {
            throw new ArgumentException(
                "Use preventedMatchId alone, orderId alone, or orderId with fromPreventedMatchId.");
        }

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("preventedMatchId", preventedMatchId);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("fromPreventedMatchId", fromPreventedMatchId);
        parameters.AddOptional("isIsolated", BinanceMarginOrderListRequestBuilder.FormatBoolean(isIsolated));
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginPreventedMatch>>(
            GetUrl(sapi, v1, "margin/myPreventedMatches"),
            HttpMethod.Get,
            ct,
            true,
            queryParameters: parameters,
            requestWeight: 10);
    }

    public async Task<RestCallResult<bool>> SmallLiabilityExchangeAsync(IEnumerable<string> assets, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (assets == null)
            throw new ArgumentNullException(nameof(assets));
        var assetList = assets.ToArray();
        if (assetList.Length is < 1 or > 10)
            throw new ArgumentOutOfRangeException(nameof(assets), "assets must contain between 1 and 10 entries");
        if (assetList.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("assets cannot contain an empty entry", nameof(assets));
        if (assetList.Any(asset => asset.Contains(',')))
            throw new ArgumentException("an individual asset cannot contain a comma", nameof(assets));

        var parameters = new ParameterCollection()
        {
            { "assetNames", string.Join(",", assetList) }
        };
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(GetUrl(sapi, v1, "margin/exchange-small-liability"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3_000).ConfigureAwait(false);
        return result.As(result.Success);
    }

}
