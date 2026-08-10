namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClient
{
    public event Action<long>? OnOrderPlaced;
    public event Action<long>? OnOrderCanceled;

    internal void InvokeOrderPlaced(long id) => OnOrderPlaced?.Invoke(id);
    internal void InvokeOrderCanceled(long id) => OnOrderCanceled?.Invoke(id);

    public async Task<RestCallResult<BinanceOptionsOrder>> PlaceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceOptionsOrderType type,
        decimal quantity,
        decimal? price = null,
        BinanceTimeInForce? timeInForce = null,
        string? clientOrderId = null,
        bool? reduceOnly = null,
        bool? postOnly = null,
        bool? isMmp = null,
        long? receiveWindow = null,
        BinanceOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        CancellationToken ct = default)
    {
        ValidateOrderParameters(symbol, side, type, timeInForce, orderResponseType, selfTradePreventionMode, false);
        var normalizedReceiveWindow = ValidateReceiveWindow(receiveWindow);
        var rulesCheck = await CheckTradingRulesAsync(symbol, type, quantity, null, price, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceOptionsOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity ?? quantity;
        price = rulesCheck.Price;
        clientOrderId = BinanceHelpers.ApplyBrokerId(clientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddParameter("quantity", quantity.ToString(BinanceConstants.CI));
        parameters.AddOptional("price", price?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptional("clientOrderId", clientOrderId);
        parameters.AddOptional("reduceOnly", reduceOnly);
        parameters.AddOptional("postOnly", postOnly);
        parameters.AddOptional("isMmp", isMmp);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("recvWindow", normalizedReceiveWindow);

        var result = await RequestAsync<BinanceOptionsOrder>(GetUrl(eapi, v1, "order"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0).ConfigureAwait(false);
        if (result) InvokeOrderPlaced(result.Data.Id);

        return result;
    }

    public async Task<RestCallResult<List<BinanceOptionsOrder>>> PlaceOrdersAsync(IEnumerable<BinanceOptionsBatchOrderRequest> orders, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orders == null)
            throw new ArgumentNullException(nameof(orders));

        var orderList = orders.ToList();
        if (orderList.Count == 0 || orderList.Count > 10)
            throw new ArgumentException("Order list should contain between 1 and 10 orders", nameof(orders));

        if (orderList.Any(order => order == null))
            throw new ArgumentException("Order list cannot contain null items", nameof(orders));

        foreach (var order in orderList)
            ValidateOrderParameters(order.Symbol, order.Side, order.Type, order.TimeInForce, order.OrderResponseType, order.SelfTradePreventionMode, true);

        var normalizedReceiveWindow = ValidateReceiveWindow(receiveWindow);
        var normalizedOrders = orderList
            .Select(order => (Order: order, Quantity: order.Quantity, Price: order.Price))
            .ToList();

        if (RestOptions.EuropeanOptions.TradeRulesBehavior != BinanceTradeRulesBehavior.None)
        {
            for (var index = 0; index < normalizedOrders.Count; index++)
            {
                var order = normalizedOrders[index].Order;
                var rulesCheck = await CheckTradingRulesAsync(order.Symbol, order.Type, order.Quantity, null, order.Price, null, ct).ConfigureAwait(false);
                if (!rulesCheck.Passed)
                {
                    Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
                    return new RestCallResult<List<BinanceOptionsOrder>>(new ArgumentError(rulesCheck.ErrorMessage!));
                }

                normalizedOrders[index] = (order, rulesCheck.Quantity ?? order.Quantity, rulesCheck.Price);
            }
        }

        var parameters = new ParameterCollection();
        var parameterOrders = new ParameterCollection[normalizedOrders.Count];
        int i = 0;
        foreach (var normalizedOrder in normalizedOrders)
        {
            var order = normalizedOrder.Order;
            var orderParameters = new ParameterCollection()
            {
                { "symbol", order.Symbol }
            };

            orderParameters.AddEnum("side", order.Side);
            orderParameters.AddEnum("type", order.Type);
            orderParameters.AddParameter("quantity", normalizedOrder.Quantity.ToString(BinanceConstants.CI));
            orderParameters.AddOptional("price", normalizedOrder.Price?.ToString(BinanceConstants.CI));
            orderParameters.AddOptionalEnum("timeInForce", order.TimeInForce);
            var clientOrderId = BinanceHelpers.ApplyBrokerId(order.ClientOrderId, BinanceConstants.ClientOrderIdFutures, 36, RestOptions.AllowAppendingClientOrderId);
            orderParameters.AddOptional("clientOrderId", clientOrderId);
            orderParameters.AddOptional("reduceOnly", order.ReduceOnly?.ToString().ToLower());
            orderParameters.AddOptional("postOnly", order.PostOnly?.ToString().ToLower());
            orderParameters.AddOptional("isMmp", order.MMP?.ToString().ToLower());
            orderParameters.AddOptionalEnum("newOrderRespType", order.OrderResponseType);
            orderParameters.AddOptionalEnum("selfTradePreventionMode", order.SelfTradePreventionMode);
            parameterOrders[i] = orderParameters;
            i++;
        }

        parameters.Add("orders", JsonConvert.SerializeObject(parameterOrders));
        parameters.AddOptional("recvWindow", normalizedReceiveWindow);

        var response = await RequestAsync<List<BinanceOptionsOrder>>(GetUrl(eapi, v1, "batchOrders"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!response.Success) return response.As<List<BinanceOptionsOrder>>([]);

        foreach (var item in response.Data)
        {
            if (item.Id > 0) InvokeOrderPlaced(item.Id);
        }

        return response;
    }

    public async Task<RestCallResult<BinanceOptionsOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(symbol, nameof(symbol));
        if (!orderId.HasValue && string.IsNullOrWhiteSpace(clientOrderId))
            throw new ArgumentException("Either orderId or clientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("clientOrderId", clientOrderId);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceOptionsOrder>(GetUrl(eapi, v1, "order"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (result) InvokeOrderCanceled(result.Data.Id);
        return result;
    }

    public async Task<RestCallResult<List<BinanceOptionsOrder>>> CancelOrdersAsync(string symbol, IEnumerable<long>? orderIdList = null, IEnumerable<string>? origClientOrderIdList = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(symbol, nameof(symbol));
        var orderIds = orderIdList?.ToList() ?? [];
        var clientOrderIds = origClientOrderIdList?.ToList() ?? [];

        if (orderIds.Count == 0 && clientOrderIds.Count == 0)
            throw new ArgumentException("Either orderIdList or origClientOrderIdList must be sent");

        if (orderIds.Count > 10)
            throw new ArgumentException("orderIdList cannot contain more than 10 items");

        if (clientOrderIds.Count > 10)
            throw new ArgumentException("origClientOrderIdList cannot contain more than 10 items");

        if (clientOrderIds.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("origClientOrderIdList cannot contain empty items", nameof(origClientOrderIdList));

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };

        if (orderIds.Count > 0)
            parameters.AddParameter("orderIds", JsonConvert.SerializeObject(orderIds));

        if (clientOrderIds.Count > 0)
            parameters.AddParameter("clientOrderIds", JsonConvert.SerializeObject(clientOrderIds));

        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var response = await RequestAsync<List<BinanceOptionsOrder>>(GetUrl(eapi, v1, "batchOrders"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!response.Success) return response.As<List<BinanceOptionsOrder>>([]);

        foreach (var item in response.Data)
        {
            if (item.Id > 0) InvokeOrderCanceled(item.Id);
        }

        return response;
    }

    public Task<RestCallResult<BinanceOptionsCancelAllOrdersByUnderlyingResult>> CancelOrdersByUnderlyingAsync(string underlying, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(underlying, nameof(underlying));
        var parameters = new ParameterCollection();
        parameters.AddParameter("underlying", underlying);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsCancelAllOrdersByUnderlyingResult>(GetUrl(eapi, v1, "allOpenOrdersByUnderlying"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceOptionsCancelAllOrdersBySymbolResult>> CancelOrdersBySymbolAsync(string symbol, long? receiveWindow = null, CancellationToken ct = default)
    {
        ValidateRequiredValue(symbol, nameof(symbol));
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsCancelAllOrdersBySymbolResult>(GetUrl(eapi, v1, "allOpenOrders"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    private static void ValidateOrderParameters(
        string symbol,
        BinanceOrderSide side,
        BinanceOptionsOrderType type,
        BinanceTimeInForce? timeInForce,
        BinanceOrderResponseType? orderResponseType,
        BinanceSelfTradePreventionMode? selfTradePreventionMode,
        bool batch)
    {
        ValidateRequiredValue(symbol, nameof(symbol));
        if (side != BinanceOrderSide.Buy && side != BinanceOrderSide.Sell)
            throw new ArgumentOutOfRangeException(nameof(side), side, "side must be BUY or SELL");
        if (type != BinanceOptionsOrderType.Limit)
            throw new ArgumentOutOfRangeException(nameof(type), type, "type must be LIMIT");
        if (timeInForce.HasValue &&
            timeInForce != BinanceTimeInForce.GoodTillCanceled &&
            timeInForce != BinanceTimeInForce.ImmediateOrCancel &&
            timeInForce != BinanceTimeInForce.FillOrKill &&
            timeInForce != BinanceTimeInForce.GoodTillCrossing)
            throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, "timeInForce must be GTC, IOC, FOK or GTX");
        if (orderResponseType.HasValue &&
            orderResponseType != BinanceOrderResponseType.Acknowledge &&
            orderResponseType != BinanceOrderResponseType.Result)
            throw new ArgumentOutOfRangeException(nameof(orderResponseType), orderResponseType, "orderResponseType must be ACK or RESULT");
        if (selfTradePreventionMode.HasValue &&
            selfTradePreventionMode != BinanceSelfTradePreventionMode.ExpireTaker &&
            selfTradePreventionMode != BinanceSelfTradePreventionMode.ExpireMaker &&
            selfTradePreventionMode != BinanceSelfTradePreventionMode.ExpireBoth &&
            (batch || selfTradePreventionMode != BinanceSelfTradePreventionMode.None))
            throw new ArgumentOutOfRangeException(nameof(selfTradePreventionMode), selfTradePreventionMode, batch
                ? "Batch selfTradePreventionMode must be EXPIRE_TAKER, EXPIRE_MAKER or EXPIRE_BOTH"
                : "selfTradePreventionMode must be NONE, EXPIRE_TAKER, EXPIRE_MAKER or EXPIRE_BOTH");
    }

    private static void ValidateRequiredValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{parameterName} must be provided", parameterName);
    }

    public Task<RestCallResult<BinanceOptionsOrder>> GetOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (orderId == null && clientOrderId == null)
            throw new ArgumentException("Either orderId or clientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("clientOrderId", clientOrderId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsOrder>(GetUrl(eapi, v1, "order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceOptionsOrder>>> GetOrdersHistoryAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceOptionsOrder>>(GetUrl(eapi, v1, "historyOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3);
    }

    public Task<RestCallResult<List<BinanceOptionsOrder>>> GetOpenOrdersAsync(string? symbol = null, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceOptionsOrder>>(GetUrl(eapi, v1, "openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3);
    }

    public Task<RestCallResult<List<BinanceOptionsPosition>>> GetPositionsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceOptionsPosition>>(GetUrl(eapi, v1, "position"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceOptionsUserExercise>>> GetUserExerciseRecordsAsync(string? symbol=null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceOptionsUserExercise>>(GetUrl(eapi, v1, "exerciseRecord"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceOptionsUserTrade>>> GetUserTradesAsync(string? symbol=null, long? fromId=null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("fromId", fromId);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceOptionsUserTrade>>(GetUrl(eapi, v1, "userTrades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceOptionsTradFiAgreementResult>> SignTradFiOptionsAgreementAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsTradFiAgreementResult>(GetUrl(eapi, v1, "stock/contract"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 50);
    }

    public Task<RestCallResult<BinanceOptionsUserCommission>> GetUserCommissionAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsUserCommission>(GetUrl(eapi, v1, "commission"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

}
