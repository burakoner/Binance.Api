namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientUsd
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
        DateTime? goodTillDate = null,
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
            { "symbol", symbol }
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
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptional("priceProtect", priceProtect?.ToString().ToUpper());
        parameters.AddOptionalEnum("priceMatch", priceMatch);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptionalMilliseconds("goodTillDate", goodTillDate);

        var result = await RequestAsync<BinanceFuturesOrder>(GetUrl(fapi, v1, "order"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0).ConfigureAwait(false);
        if (result) InvokeOrderPlaced(result.Data.Id);

        return result;
    }

    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesOrder>>>> PlaceMultipleOrdersAsync(IEnumerable<BinanceFuturesBatchOrderRequest> orders, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (RestApiOptions.UsdtFuturesOptions.TradeRulesBehavior != BinanceTradeRulesBehavior.None)
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
        var parameterOrders = new List<Dictionary<string, object>>();
        int i = 0;
        foreach (var order in orders)
        {
            var clientOrderId = BinanceHelpers.ApplyBrokerId(order.NewClientOrderId, BinanceConstants.ClientOrderIdFutures, 36, RestApiOptions.AllowAppendingClientOrderId);

            var orderParameters = new ParameterCollection()
            {
                { "symbol", order.Symbol },
                { "newOrderRespType", "RESULT" }
            };
            orderParameters.AddEnum("side", order.Side);
            orderParameters.AddEnum("type", order.Type);
            orderParameters.AddOptional("quantity", order.Quantity?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptional("newClientOrderId", clientOrderId);
            orderParameters.AddOptionalEnum("timeInForce", order.TimeInForce);
            orderParameters.AddOptionalEnum("positionSide", order.PositionSide);
            orderParameters.AddOptional("price", order.Price?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptional("stopPrice", order.StopPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptional("activationPrice", order.ActivationPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptional("callbackRate", order.CallbackRate?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalEnum("workingType", order.WorkingType);
            orderParameters.AddOptional("reduceOnly", order.ReduceOnly?.ToString().ToLower());
            orderParameters.AddOptional("priceProtect", order.PriceProtect?.ToString().ToUpper());
            orderParameters.AddOptionalEnum("priceMatch", order.PriceMatch);
            orderParameters.AddOptionalEnum("selfTradePreventionMode", order.SelfTradePreventionMode);
            parameterOrders.Add(orderParameters);
            i++;
        }

        parameters.Add("batchOrders", JsonConvert.SerializeObject(parameterOrders));
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var response = await RequestAsync<IEnumerable<BinanceFuturesOrderResult>>(GetUrl(fapi, v1, "batchOrders"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
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

    public Task<RestCallResult<BinanceFuturesOrder>> EditOrderAsync(string symbol, BinanceOrderSide side, decimal quantity, decimal? price = null, BinanceFuturesPriceMatch? priceMatch = null, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "side", MapConverter.GetString(side) },
            { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
        };
        parameters.AddOptional("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("priceMatch", priceMatch);
        parameters.AddOptional("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>(GetUrl(fapi, v1, "order"), HttpMethod.Put, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesOrder>>>> EditMultipleOrdersAsync(IEnumerable<BinanceFuturesBatchEditOrderRequest> orders, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        var parameterOrders = new List<Dictionary<string, object>>();
        int i = 0;
        foreach (var order in orders)
        {
            var orderParameters = new ParameterCollection()
            {
                { "symbol", order.Symbol },
                { "quantity", order.Quantity.ToString(CultureInfo.InvariantCulture) },
            };
            orderParameters.AddEnum("side", order.Side);
            orderParameters.AddOptional("price", order.Price?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalEnum("priceMatch", order.PriceMatch);
            orderParameters.AddOptional("orderId", order.OrderId?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptional("origClientOrderId", order.ClientOrderId);
            parameterOrders.Add(orderParameters);
            i++;
        }

        parameters.Add("batchOrders", JsonConvert.SerializeObject(parameterOrders));
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var response = await RequestAsync<IEnumerable<BinanceFuturesOrderResult>>(GetUrl(fapi, v1, "batchOrders"), HttpMethod.Put, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!response.Success) return response.As<IEnumerable<CallResult<BinanceFuturesOrder>>>(default);

        var result = new List<CallResult<BinanceFuturesOrder>>();
        foreach (var item in response.Data)
        {
            result.Add(item.Code != 0
                ? new CallResult<BinanceFuturesOrder>(new ServerError(item.Code, item.Message))
                : new CallResult<BinanceFuturesOrder>(item));
        }

        return response.As<IEnumerable<CallResult<BinanceFuturesOrder>>>(result);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesOrderEditHistory>>> GetOrderEditHistoryAsync(string symbol, long? orderId = null, string? clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("origClientOrderId", clientOrderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return RequestAsync<IEnumerable<BinanceFuturesOrderEditHistory>>(GetUrl(fapi, v1, "orderAmendment"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

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
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceFuturesOrder>(GetUrl(fapi, v1, "order"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (result) InvokeOrderCanceled(result.Data.Id);

        return result;
    }

    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesOrder>>>> CancelMultipleOrdersAsync(string symbol, List<long>? orderIdList = null, List<string>? origClientOrderIdList = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderIdList == null && origClientOrderIdList == null)
            throw new ArgumentException("Either orderIdList or origClientOrderIdList must be sent");

        if (orderIdList?.Count > 10)
            throw new ArgumentException("orderIdList cannot contain more than 10 items");

        if (origClientOrderIdList?.Count > 10)
            throw new ArgumentException("origClientOrderIdList cannot contain more than 10 items");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };

        if (orderIdList != null)
            parameters.AddOptional("orderIdList", $"[{string.Join(",", orderIdList)}]");

        if (origClientOrderIdList != null)
            parameters.AddOptional("origClientOrderIdList", $"[{string.Join(",", origClientOrderIdList.Select(id => $"\"{id}\""))}]");

        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var response = await RequestAsync<IEnumerable<BinanceFuturesOrderResult>>(GetUrl(fapi, v1, "batchOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 10).ConfigureAwait(false);
        if (!response.Success) return response.As<IEnumerable<CallResult<BinanceFuturesOrder>>>(default);

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
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceResult>(GetUrl(fapi, v1, "allOpenOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceFuturesCountDownResult>> CancelAllOrdersAfterTimeoutAsync(string symbol, TimeSpan countDownTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "countdownTime", (int)countDownTime.TotalMilliseconds }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesCountDownResult>(GetUrl(fapi, v1, "countdownCancelAll"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
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
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>(GetUrl(fapi, v1, "order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOrdersAsync(string? symbol = null, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return RequestAsync<IEnumerable<BinanceFuturesOrder>>(GetUrl(fapi, v1, "allOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOpenOrdersAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptional("symbol", symbol);

        var weight = symbol == null ? 40 : 1;
        return RequestAsync<IEnumerable<BinanceFuturesOrder>>(GetUrl(fapi, v1, "openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
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
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>(GetUrl(fapi, v1, "openOrder"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetForcedOrdersAsync(string? symbol = null, AutoCloseType? closeType = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptionalEnum("autoCloseType", closeType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        var weight = symbol == null ? 50 : 20;
        return RequestAsync<IEnumerable<BinanceFuturesOrder>>(GetUrl(fapi, v1, "forceOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesUsdUserTrade>>> GetUserTradesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, long? orderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceFuturesUsdUserTrade>>(GetUrl(fapi, v1, "userTrades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceResult>> SetMarginTypeAsync(string symbol, BinanceFuturesMarginType marginType, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddEnum("marginType", marginType);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceResult>(GetUrl(fapi, v1, "marginType"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceResult>> SetPositionModeAsync(bool dualPositionSide, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "dualSidePosition", dualPositionSide.ToString().ToLower() }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceResult>(GetUrl(fapi, v1, "positionSide/dual"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceFuturesInitialLeverageChangeResult>> SetInitialLeverageAsync(string symbol, int leverage, int? receiveWindow = null, CancellationToken ct = default)
    {
        leverage.ValidateIntBetween(nameof(leverage), 1, 125);
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "leverage", leverage }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesInitialLeverageChangeResult>(GetUrl(fapi, v1, "leverage"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceResult>> SetMultiAssetsModeAsync(bool enabled, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "multiAssetsMargin", enabled.ToString() }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceResult>(GetUrl(fapi, v1, "multiAssetsMargin"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceFuturesPositionMarginResult>> SetPositionMarginAsync(string symbol, decimal quantity, BinanceFuturesMarginChangeDirectionType type, BinancePositionSide? positionSide = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
        };
        parameters.AddEnum("type", type);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesPositionMarginResult>(GetUrl(fapi, v1, "positionMargin"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesUsdtPositionDetails>>> GetPositionInformationAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceFuturesUsdtPositionDetails>>(GetUrl(fapi, v2, "positionRisk"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<IEnumerable<BinanceFuturesPositionV3>>> GetPositionsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        return RequestAsync<IEnumerable<BinanceFuturesPositionV3>>(GetUrl(fapi, v3, "positionRisk"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public async Task<RestCallResult<IEnumerable<BinanceFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        if (symbol == null)
        {
            return await RequestAsync<IEnumerable<BinanceFuturesQuantileEstimation>>(GetUrl(fapi, v1, "adlQuantile"), HttpMethod.Get, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        }

        var result = await RequestAsync<BinanceFuturesQuantileEstimation>(GetUrl(fapi, v1, "adlQuantile"), HttpMethod.Get, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!result) return result.As<IEnumerable<BinanceFuturesQuantileEstimation>>(null);

        return result.As<IEnumerable<BinanceFuturesQuantileEstimation>>([result.Data]);
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
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return RequestAsync<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>(GetUrl(fapi, v3, "positionMargin/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    // TODO: Test Order(TRADE)
}
