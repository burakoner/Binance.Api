namespace Binance.Api.Spot;

internal partial class BinanceSpotRestClient
{
    public event Action<long>? OnOrderPlaced;
    public event Action<long>? OnOrderCanceled;

    internal void InvokeOrderPlaced(long id) => OnOrderPlaced?.Invoke(id);
    internal void InvokeOrderCanceled(long id) => OnOrderCanceled?.Invoke(id);

    public async Task<RestCallResult<BinanceSpotOrder>> PlaceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceSpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        string? newClientOrderId = null,
        BinanceTimeInForce? timeInForce = null,
        BinanceOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        long? trailingDelta = null,
        long? strategyId = null,
        long? strategyType = null,
        BinanceSpotPegPriceType? pegPriceType = null,
        int? pegOffsetValue = null,
        BinanceSpotPegOffsetType? pegOffsetType = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.StrategyType(strategyType);
        BinanceSpotTradeValidation.Peg(type, pegPriceType, pegOffsetValue, pegOffsetType);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));
        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradingRulesAsync(symbol, type, quantity, quoteQuantity, price, stopPrice,  ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceSpotOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;
        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
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
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptionalEnum("pegPriceType", pegPriceType);
        parameters.AddOptional("pegOffsetValue", pegOffsetValue);
        parameters.AddOptionalEnum("pegOffsetType", pegOffsetType);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        var result = await RequestAsync<BinanceSpotOrder>(GetUrl(api, v3, "order"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (result) InvokeOrderPlaced(result.Data.Id);

        return result;
    }

    public async Task<RestCallResult<BinanceSpotOrderTest>> PlaceTestOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceSpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        string? newClientOrderId = null,
        BinanceTimeInForce? timeInForce = null,
        BinanceOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        long? trailingDelta = null,
        long? strategyId = null,
        long? strategyType = null,
        BinanceSpotPegPriceType? pegPriceType = null,
        int? pegOffsetValue = null,
        BinanceSpotPegOffsetType? pegOffsetType = null,
        decimal? receiveWindow = null,
        bool? computeFeeRates = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.StrategyType(strategyType);
        BinanceSpotTradeValidation.Peg(type, pegPriceType, pegOffsetValue, pegOffsetType);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));
        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradingRulesAsync(symbol, type, quantity, quoteQuantity, price, stopPrice, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceSpotOrderTest>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity?.ToString(BinanceConstants.CI));
        parameters.AddOptional("quoteOrderQty", quoteQuantity?.ToString(BinanceConstants.CI));
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("price", price?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptional("stopPrice", stopPrice?.ToString(BinanceConstants.CI));
        parameters.AddOptional("icebergQty", icebergQuantity?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptionalEnum("pegPriceType", pegPriceType);
        parameters.AddOptional("pegOffsetValue", pegOffsetValue);
        parameters.AddOptionalEnum("pegOffsetType", pegOffsetType);
        parameters.AddOptional("computeCommissionRates", computeFeeRates?.ToString(BinanceConstants.CI).ToLowerInvariant());
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        var weight = computeFeeRates == true ? 20 : 1;
        return await RequestAsync<BinanceSpotOrderTest>(GetUrl(api, v3, "order/test"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: weight).ConfigureAwait(false);
    }

    public Task<RestCallResult<BinanceSpotOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<BinanceSpotOrder>(GetUrl(api, v3, "order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 4);
    }

    public async Task<RestCallResult<BinanceSpotOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, BinanceSpotOrderCancelRestriction? cancelRestriction = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.OrderIdentifiers(orderId, origClientOrderId);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptionalEnum("cancelRestrictions", cancelRestriction);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        var result = await RequestAsync<BinanceSpotOrder>(GetUrl(api, v3, "order"), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        if (result) InvokeOrderCanceled(result.Data.Id);
        return result;
    }
    
    public async Task<RestCallResult<List<BinanceSpotOrder>>> CancelOrdersAsync(string symbol, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        var result = await RequestAsync<List<BinanceSpotOrder>>(GetUrl(api, v3, "openOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        if (result) foreach (var order in result.Data) InvokeOrderCanceled(order.Id);
        return result;
    }

    public async Task<RestCallResult<BinanceSpotReplaceOrderResult>> ReplaceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceSpotOrderType type,
        BinanceSpotOrderCancelReplaceMode mode,
        long? cancelOrderId = null,
        string? cancelClientOrderId = null,
        string? newClientOrderId = null,
        string? newCancelClientOrderId = null,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        BinanceTimeInForce? timeInForce = null,
        BinanceOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        BinanceSpotOrderCancelRestriction? cancelRestriction = null,
        long? trailingDelta = null,
        long? strategyId = null,
        long? strategyType = null,
        BinanceSpotOrderRateLimitExceededMode? orderRateLimitExceededMode = null,
        BinanceSpotPegPriceType? pegPriceType = null,
        int? pegOffsetValue = null,
        BinanceSpotPegOffsetType? pegOffsetType = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.OrderIdentifiers(cancelOrderId, cancelClientOrderId);
        BinanceSpotTradeValidation.StrategyType(strategyType);
        BinanceSpotTradeValidation.Peg(type, pegPriceType, pegOffsetValue, pegOffsetType);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));

        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradingRulesAsync(symbol, type, quantity, quoteQuantity, price, stopPrice,  ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceSpotReplaceOrderResult>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddEnum("cancelReplaceMode", mode);
        parameters.AddOptional("cancelOrderId", cancelOrderId);
        parameters.AddOptional("cancelOrigClientOrderId", cancelClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("cancelNewClientOrderId", newCancelClientOrderId);
        parameters.AddOptional("quantity", quantity);
        parameters.AddOptional("quoteOrderQty", quoteQuantity);
        parameters.AddOptional("price", price);
        parameters.AddOptional("stopPrice", stopPrice);
        parameters.AddOptional("icebergQty", icebergQuantity);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptionalEnum("cancelRestrictions", cancelRestriction);
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptionalEnum("orderRateLimitExceededMode", orderRateLimitExceededMode);
        parameters.AddOptionalEnum("pegPriceType", pegPriceType);
        parameters.AddOptional("pegOffsetValue", pegOffsetValue);
        parameters.AddOptionalEnum("pegOffsetType", pegOffsetType);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        var result = await RequestAsync<BinanceSpotReplaceOrderResult>(GetUrl(api, v3, "order/cancelReplace"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (!result && result.Raw != null)
        {
            // Attempt to parse the error
            var jsonData = result.Raw.ToJToken(Logger);
            if (jsonData != null)
            {
                var dataNode = jsonData["data"];
                if (dataNode == null)
                    return result;

                var error = dataNode?["cancelResult"]?.ToString() == "FAILURE" ? dataNode!["cancelResponse"] : jsonData["data"]!["newOrderResponse"];
                if (error != null && error.HasValues)
                    return result.AsError<BinanceSpotReplaceOrderResult>(new ServerError(error!.Value<int>("code"), error.Value<string>("msg")!));
            }
        }

        if (result && result.Data.NewOrderResult == BinanceSpotOrderOperationResult.Success)
            InvokeOrderPlaced(result.Data.NewOrderResponse!.Id);

        return result;
    }

    public Task<RestCallResult<BinanceSpotOrderAmendResult>> AmendOrderAsync(
        string symbol,
        decimal newQuantity,
        long? orderId = null,
        string? originalClientOrderId = null,
        string? newClientOrderId = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.Amend(newQuantity, orderId, originalClientOrderId);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "newQty", newQuantity.ToString(BinanceConstants.CI) }
        };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", originalClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<BinanceSpotOrderAmendResult>(GetUrl(api, v3, "order/amend/keepPriority"), HttpMethod.Put, ct, true, bodyParameters: parameters, requestWeight: 4);
    }

    public async Task<RestCallResult<BinanceSpotOrder>> PlaceSorOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceSpotOrderType type,
        decimal quantity,
        decimal? price = null,
        string? newClientOrderId = null,
        BinanceTimeInForce? timeInForce = null,
        BinanceOrderResponseType? orderResponseType = null,
        decimal? icebergQuantity = null,
        long? strategyId = null,
        long? strategyType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.SmartOrderRouting(type, quantity, strategyType);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));

        var rulesCheck = await CheckTradingRulesAsync(symbol, type, quantity, null, price, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceSpotOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity!.Value;
        price = rulesCheck.Price;
        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);
        var parameters = CreateSorOrderParameters(symbol, side, type, quantity, price, clientOrderId, timeInForce, orderResponseType, icebergQuantity, strategyId, strategyType, selfTradePreventionMode, receiveWindow, null);

        var result = await RequestAsync<BinanceSpotOrder>(GetUrl(api, v3, "sor/order"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (result)
            InvokeOrderPlaced(result.Data.Id);
        return result;
    }

    public async Task<RestCallResult<BinanceSpotOrderTest>> PlaceSorTestOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceSpotOrderType type,
        decimal quantity,
        decimal? price = null,
        string? newClientOrderId = null,
        BinanceTimeInForce? timeInForce = null,
        BinanceOrderResponseType? orderResponseType = null,
        decimal? icebergQuantity = null,
        long? strategyId = null,
        long? strategyType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        decimal? receiveWindow = null,
        bool? computeFeeRates = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.SmartOrderRouting(type, quantity, strategyType);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));

        var rulesCheck = await CheckTradingRulesAsync(symbol, type, quantity, null, price, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceSpotOrderTest>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity!.Value;
        price = rulesCheck.Price;
        var parameters = CreateSorOrderParameters(symbol, side, type, quantity, price, newClientOrderId, timeInForce, orderResponseType, icebergQuantity, strategyId, strategyType, selfTradePreventionMode, receiveWindow, computeFeeRates);
        var weight = computeFeeRates == true ? 20 : 1;
        return await RequestAsync<BinanceSpotOrderTest>(GetUrl(api, v3, "sor/order/test"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: weight).ConfigureAwait(false);
    }

    private ParameterCollection CreateSorOrderParameters(
        string symbol,
        BinanceOrderSide side,
        BinanceSpotOrderType type,
        decimal quantity,
        decimal? price,
        string? newClientOrderId,
        BinanceTimeInForce? timeInForce,
        BinanceOrderResponseType? orderResponseType,
        decimal? icebergQuantity,
        long? strategyId,
        long? strategyType,
        BinanceSelfTradePreventionMode? selfTradePreventionMode,
        decimal? receiveWindow,
        bool? computeFeeRates)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddParameter("quantity", quantity.ToString(BinanceConstants.CI));
        parameters.AddOptional("price", price?.ToString(BinanceConstants.CI));
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("icebergQty", icebergQuantity?.ToString(BinanceConstants.CI));
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));
        parameters.AddOptional("computeCommissionRates", computeFeeRates?.ToString(BinanceConstants.CI).ToLowerInvariant());
        return parameters;
    }

    public async Task<RestCallResult<List<BinanceSpotOrder>>> GetOpenOrdersAsync(string? symbol = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        if (symbol != null)
            symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return await RequestAsync<List<BinanceSpotOrder>>(GetUrl(api, v3, "openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 80 : 6).ConfigureAwait(false);
    }

    public Task<RestCallResult<List<BinanceSpotOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        BinanceSpotAccountValidation.HistoryRange(startTime, endTime);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<List<BinanceSpotOrder>>(GetUrl(api, v3, "allOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

}
