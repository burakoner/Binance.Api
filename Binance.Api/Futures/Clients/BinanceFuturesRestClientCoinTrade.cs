namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientCoin
{
    public event Action<long>? OnOrderPlaced;
    public event Action<long>? OnOrderCanceled;

    internal void InvokeOrderPlaced(long id) => OnOrderPlaced?.Invoke(id);
    internal void InvokeOrderCanceled(long id) => OnOrderCanceled?.Invoke(id);

    public async Task<RestCallResult<BinanceFuturesOrder>> PlaceOrderAsync(
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
        WorkingType? workingType = null,
        bool? reduceOnly = null,
        bool? closePosition = null,
        bool? priceProtect = null,
        decimal? activationPrice = null,
        decimal? callbackRate = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (closePosition == true && positionSide != null)
        {
            if (positionSide == BinancePositionSide.Short && side == BinanceOrderSide.Sell)
                throw new ArgumentException("Can't close short position with order side sell");
            if (positionSide == BinancePositionSide.Long && side == BinanceOrderSide.Buy)
                throw new ArgumentException("Can't close long position with order side buy");
        }

        if (orderResponseType == BinanceOrderResponseType.Full)
            throw new ArgumentException("OrderResponseType.Full is not supported in Futures");

        var rulesCheck = await CheckTradeRulesAsync(symbol, quantity, null, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceFuturesOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;

        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdFutures, 36, RestApiOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection()
        {
            { "symbol", symbol },
        };

        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("newClientOrderId", clientOrderId);
        parameters.AddOptional("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("activationPrice", activationPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("callbackRate", callbackRate?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("workingType", workingType);
        parameters.AddOptional("reduceOnly", reduceOnly?.ToString().ToLower());
        parameters.AddOptional("closePosition", closePosition?.ToString().ToLower());
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));
        parameters.AddOptionalEnum("priceMatch", priceMatch);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("priceProtect", priceProtect?.ToString().ToUpper());

        var result = await RequestAsync<BinanceFuturesOrder>(GetUrl(dapi, v1, "order"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
        if (result) InvokeOrderPlaced(result.Data.Id);
        return result;
    }

    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesOrder>>>> PlaceMultipleOrdersAsync(IEnumerable<BinanceFuturesBatchOrderRequest> orders, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orders.Count() <= 0 || orders.Count() > 5)
            throw new ArgumentException("Order list should be at least 1 and max 5 orders");

        if (RestApiOptions.CoinFuturesOptions.TradeRulesBehavior != BinanceTradeRulesBehavior.None)
        {
            foreach (var order in orders)
            {
                var rulesCheck = await CheckTradeRulesAsync(order.Symbol, order.Quantity, null, order.Price, order.StopPrice, order.Type, ct).ConfigureAwait(false);
                if (!rulesCheck.Passed)
                {
                    Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
                    return new RestCallResult<IEnumerable<CallResult<BinanceFuturesOrder>>>(new ArgumentError(rulesCheck.ErrorMessage!));
                }

                order.Quantity = rulesCheck.Quantity;
                order.Price = rulesCheck.Price;
                order.StopPrice = rulesCheck.StopPrice;
            }
        }

        var parameters = new ParameterCollection();

        var parameterOrders = new ParameterCollection[orders.Count()];
        int i = 0;
        foreach (var order in orders)
        {
            var orderParameters = new ParameterCollection()
                {
                    { "symbol", order.Symbol },
                    { "newOrderRespType", "RESULT" }
                };

            orderParameters.AddEnum("side", order.Side);
            orderParameters.AddEnum("type", order.Type);
            var clientOrderId = BinanceHelpers.ApplyBrokerId(order.NewClientOrderId, BinanceConstants.ClientOrderIdFutures, 36, RestApiOptions.AllowAppendingClientOrderId);
            orderParameters.AddOptional("quantity", order.Quantity?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptional("newClientOrderId", clientOrderId);
            orderParameters.AddOptional("price", order.Price?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalEnum("timeInForce", order.TimeInForce);
            orderParameters.AddOptionalEnum("positionSide", order.PositionSide);
            orderParameters.AddOptional("stopPrice", order.StopPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptional("activationPrice", order.ActivationPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptional("callbackRate", order.CallbackRate?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalEnum("workingType", order.WorkingType);
            orderParameters.AddOptional("reduceOnly", order.ReduceOnly?.ToString().ToLower());
            orderParameters.AddOptional("priceProtect", order.PriceProtect?.ToString().ToUpper());
            orderParameters.AddOptionalEnum("selfTradePreventionMode", order.SelfTradePreventionMode);
            orderParameters.AddOptionalEnum("priceMatch", order.PriceMatch);
            parameterOrders[i] = orderParameters;
            i++;
        }

        parameters.Add("batchOrders", JsonConvert.SerializeObject(parameterOrders));
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        var response = await RequestAsync<IEnumerable<BinanceFuturesOrderResult>>(GetUrl(dapi, v1, "batchOrders"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5);
        if (!response.Success) return response.As<IEnumerable<CallResult<BinanceFuturesOrder>>>(default);

        var result = new List<CallResult<BinanceFuturesOrder>>();
        foreach (var item in response.Data)
        {
            if (item.Code == 0)
            {
                result.Add(new CallResult<BinanceFuturesOrder>(item));
                InvokeOrderPlaced(item.Id);
            }
            else
            {
                result.Add(new CallResult<BinanceFuturesOrder>(new ServerError(item.Code, item.Message)));
            }
        }

        return response.As<IEnumerable<CallResult<BinanceFuturesOrder>>>(result);
    }

    // TODO: Modify Order (TRADE)
    // TODO: Modify Multiple Orders(TRADE)
    // TODO: Get Order Modify History (USER_DATA)

    public async Task<RestCallResult<BinanceFuturesOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceFuturesOrder>(GetUrl(dapi, v1, "order"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
        if (result) InvokeOrderCanceled(result.Data.Id);

        return result;
    }

    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesOrder>>>> CancelMultipleOrdersAsync(string symbol, IEnumerable<long>? orderIdList = null, IEnumerable<string>? origClientOrderIdList = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderIdList == null && origClientOrderIdList == null)
            throw new ArgumentException("Either orderIdList or origClientOrderIdList must be sent");

        if (orderIdList?.Count() > 10)
            throw new ArgumentException("orderIdList cannot contain more than 10 items");

        if (origClientOrderIdList?.Count() > 10)
            throw new ArgumentException("origClientOrderIdList cannot contain more than 10 items");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };

        if (orderIdList != null)
            parameters.AddOptional("orderIdList", $"[{string.Join(",", orderIdList)}]");

        if (origClientOrderIdList != null)
            parameters.AddOptional("origClientOrderIdList", $"[{string.Join(",", origClientOrderIdList.Select(id => $"\"{id}\""))}]");

        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        var response = await RequestAsync<IEnumerable<BinanceFuturesOrderResult>>(GetUrl(dapi, v1, "batchOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);

        if (!response.Success)
            return response.As<IEnumerable<CallResult<BinanceFuturesOrder>>>(default);

        var result = new List<CallResult<BinanceFuturesOrder>>();
        foreach (var item in response.Data)
        {
            if (item.Code == 0)
            {
                result.Add(new CallResult<BinanceFuturesOrder>(item));
                InvokeOrderCanceled(item.Id);
            }
            else
            {
                result.Add(new CallResult<BinanceFuturesOrder>(new ServerError(item.Code, item.Message)));
            }
        }

        return response.As<IEnumerable<CallResult<BinanceFuturesOrder>>>(result);
    }

    public Task<RestCallResult<BinanceResult>> CancelAllOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceResult>(GetUrl(dapi, v1, "allOpenOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceFuturesCountDownResult>> CancelAllOrdersAfterTimeoutAsync(string symbol, TimeSpan countDownTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "countdownTime", (int)countDownTime.TotalMilliseconds }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesCountDownResult>(GetUrl(dapi, v1, "countdownCancelAll"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>(GetUrl(dapi, v1, "order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOrdersAsync(string? symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var weight = symbol == null ? 40 : 20;
        return RequestAsync<IEnumerable<BinanceFuturesOrder>>(GetUrl(dapi, v1, "allOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOpenOrdersAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));
        parameters.AddOptional("symbol", symbol);

        var weight = symbol == null ? 40 : 1;
        return RequestAsync<IEnumerable<BinanceFuturesOrder>>(GetUrl(dapi, v1, "openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<BinanceFuturesOrder>> GetOpenOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>(GetUrl(dapi, v1, "openOrder"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetForcedOrdersAsync(string? symbol = null, AutoCloseType? closeType = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptionalEnum("autoCloseType", closeType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        var weight = symbol == null ? 50 : 20;
        return RequestAsync<IEnumerable<BinanceFuturesOrder>>(GetUrl(dapi, v1, "forceOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesCoinUserTrade>>> GetUserTradesAsync(string? symbol = null, string? pair = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, long? orderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("pair", pair);
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        var weight = symbol == null ? 40 : 20;
        return RequestAsync<IEnumerable<BinanceFuturesCoinUserTrade>>(GetUrl(dapi, v1, "userTrades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesCoinPositionDetails>>> GetPositionInformationAsync(string? marginAsset = null, string? pair = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();

        parameters.AddOptional("marginAsset", marginAsset);
        parameters.AddOptional("pair", pair);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceFuturesCoinPositionDetails>>(GetUrl(dapi, v1, "positionRisk"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceResult>> ModifyPositionModeAsync(bool dualPositionSide, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "dualSidePosition", dualPositionSide.ToString().ToLower() }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceResult>(GetUrl(dapi, v1, "positionSide/dual"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceResult>> ChangeMarginTypeAsync(string symbol, BinanceFuturesMarginType marginType, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddEnum("marginType", marginType);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceResult>(GetUrl(dapi, v1, "marginType"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceFuturesInitialLeverageChangeResult>> ChangeInitialLeverageAsync(string symbol, int leverage, int? receiveWindow = null, CancellationToken ct = default)
    {
        leverage.ValidateIntBetween(nameof(leverage), 1, 125);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "leverage", leverage }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesInitialLeverageChangeResult>(GetUrl(dapi, v1, "leverage"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceFuturesQuantileEstimation>>(GetUrl(dapi, v1, "adlQuantile"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceFuturesPositionMarginResult>> ModifyPositionMarginAsync(string symbol, decimal quantity, BinanceFuturesMarginChangeDirectionType type, BinancePositionSide? positionSide = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
        };
        parameters.AddEnum("type", type);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesPositionMarginResult>(GetUrl(dapi, v1, "positionMargin"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>> GetMarginChangeHistoryAsync(string symbol, BinanceFuturesMarginChangeDirectionType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return RequestAsync<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>(GetUrl(dapi, v1, "positionMargin/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}