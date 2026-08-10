namespace Binance.Api.Futures;

internal partial class BinanceFuturesRestClientCoin
{
    public event Action<long>? OnOrderPlaced;
    public event Action<long>? OnOrderCanceled;

    internal void InvokeOrderPlaced(long id) => OnOrderPlaced?.Invoke(id);
    internal void InvokeOrderCanceled(long id) => OnOrderCanceled?.Invoke(id);

    public async Task<RestCallResult<BinanceFuturesCoinRestOrderAcknowledgement>> PlaceOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceFuturesOrderType type,
        decimal? quantity,
        decimal? price = null,
        string? newClientOrderId = null,
        BinancePositionSide? positionSide = null,
        BinanceTimeInForce? timeInForce = null,
        BinanceOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        BinanceFuturesPriceMatch? priceMatch = null,
        bool? reduceOnly = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateNormalOrderType(type);

        if (orderResponseType == BinanceOrderResponseType.Full)
            throw new ArgumentException("OrderResponseType.Full is not supported in Futures");

        var rulesCheck = await CheckTradingRulesAsync(symbol, type, quantity, null, price, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceFuturesCoinRestOrderAcknowledgement>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;

        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdFutures, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity?.ToString(BinanceConstants.CI));
        parameters.AddOptional("newClientOrderId", clientOrderId);
        parameters.AddOptional("price", price?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("reduceOnly", reduceOnly?.ToString().ToLower());
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));
        parameters.AddOptionalEnum("priceMatch", priceMatch);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);

        var result = await RequestAsync<BinanceFuturesCoinRestOrderAcknowledgement>(GetUrl(dapi, v1, "order"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
        if (result) InvokeOrderPlaced(result.Data.Id);
        return result;
    }

    public async Task<RestCallResult<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>> PlaceOrdersAsync(IEnumerable<BinanceFuturesBatchOrderRequest> orders, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orders == null)
            throw new ArgumentNullException(nameof(orders));

        var orderList = orders.ToList();
        if (orderList.Count is < 1 or > 5)
            throw new ArgumentOutOfRangeException(nameof(orders), orderList.Count, "Between one and five orders must be provided");

        foreach (var order in orderList)
        {
            if (order == null)
                throw new ArgumentException("Orders cannot contain null items", nameof(orders));

            ValidateNormalOrderType(order.Type);
        }

        if (RestOptions.CoinFuturesOptions.TradeRulesBehavior != BinanceTradeRulesBehavior.None)
        {
            foreach (var order in orderList)
            {
                var rulesCheck = await CheckTradingRulesAsync(order.Symbol, order.Type, order.Quantity, null, order.Price, null, ct).ConfigureAwait(false);
                if (!rulesCheck.Passed)
                {
                    Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
                    return new RestCallResult<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>(new ArgumentError(rulesCheck.ErrorMessage!));
                }

                order.Quantity = rulesCheck.Quantity;
                order.Price = rulesCheck.Price;
            }
        }

        var parameters = new ParameterCollection();
        var parameterOrders = new List<Dictionary<string, object>>();
        foreach (var order in orderList)
        {
            var clientOrderId = BinanceHelpers.ApplyBrokerId(order.NewClientOrderId, BinanceConstants.ClientOrderIdFutures, 36, RestOptions.AllowAppendingClientOrderId);

            var orderParameters = new ParameterCollection()
            {
                { "symbol", order.Symbol },
                { "newOrderRespType", "RESULT" }
            };
            orderParameters.AddEnum("side", order.Side);
            orderParameters.AddEnum("type", order.Type);
            orderParameters.AddOptional("quantity", order.Quantity?.ToString(BinanceConstants.CI));
            orderParameters.AddOptional("newClientOrderId", clientOrderId);
            orderParameters.AddOptionalEnum("timeInForce", order.TimeInForce);
            orderParameters.AddOptionalEnum("positionSide", order.PositionSide);
            orderParameters.AddOptional("price", order.Price?.ToString(BinanceConstants.CI));
            orderParameters.AddOptional("reduceOnly", order.ReduceOnly?.ToString().ToLower());
            orderParameters.AddOptionalEnum("priceMatch", order.PriceMatch);
            orderParameters.AddOptionalEnum("selfTradePreventionMode", order.SelfTradePreventionMode);
            parameterOrders.Add(orderParameters);
        }

        parameters.Add("batchOrders", JsonConvert.SerializeObject(parameterOrders));
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var response = await RequestAsync<List<BinanceFuturesCoinRestOrderAcknowledgementResult>>(GetUrl(dapi, v1, "batchOrders"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5);
        if (!response.Success) return response.As<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>([]);

        var result = new List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>();
        foreach (var item in response.Data)
        {
            if (item.Code == 0)
            {
                result.Add(new CallResult<BinanceFuturesCoinRestOrderAcknowledgement>(item));
                InvokeOrderPlaced(item.Id);
            }
            else
            {
                result.Add(new CallResult<BinanceFuturesCoinRestOrderAcknowledgement>(new ServerError(item.Code, item.Message)));
            }
        }

        return response.As<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>(result);
    }

    private static void ValidateNormalOrderType(BinanceFuturesOrderType type)
    {
        if (type is not BinanceFuturesOrderType.Limit and not BinanceFuturesOrderType.Market)
            throw new ArgumentOutOfRangeException(nameof(type), type, "COIN-M normal order endpoints support only Limit and Market orders; use PlaceAlgoOrderAsync for conditional orders");
    }

    public Task<RestCallResult<BinanceFuturesCoinRestOrderAcknowledgement>> ModifyOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        decimal price,
        long? orderId = null,
        string? origClientOrderId = null,
        BinanceFuturesPriceMatch? priceMatch = null,
        long? modifyId = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Symbol is required", nameof(symbol));
        if (side != BinanceOrderSide.Buy && side != BinanceOrderSide.Sell)
            throw new ArgumentOutOfRangeException(nameof(side), side, "Side must be Buy or Sell");
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Quantity must be greater than zero");
        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), price, "Price must be greater than zero");
        if (origClientOrderId != null && string.IsNullOrWhiteSpace(origClientOrderId))
            throw new ArgumentException("Original client order id cannot be empty", nameof(origClientOrderId));
        if (!orderId.HasValue && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");
        if (priceMatch.HasValue)
            throw new ArgumentException("Binance currently requires price for Modify Order and also prohibits sending priceMatch with price", nameof(priceMatch));

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "quantity", quantity.ToString(BinanceConstants.CI) },
            { "price", price.ToString(BinanceConstants.CI) }
        };
        parameters.AddEnum("side", side);
        parameters.AddOptional("orderId", orderId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("modifyId", modifyId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesCoinRestOrderAcknowledgement>(GetUrl(dapi, v1, "order"), HttpMethod.Put, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>> ModifyOrdersAsync(IEnumerable<BinanceFuturesBatchModifyRequest> orders, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orders == null)
            throw new ArgumentNullException(nameof(orders));

        var orderList = orders.ToList();
        if (orderList.Count is < 1 or > 5)
            throw new ArgumentException("Order list must contain between 1 and 5 orders", nameof(orders));

        var parameters = new ParameterCollection();
        var parameterOrders = new List<Dictionary<string, object>>();
        for (var index = 0; index < orderList.Count; index++)
        {
            var order = orderList[index];
            if (order == null)
                throw new ArgumentException($"Order at index {index} cannot be null", nameof(orders));
            if (string.IsNullOrWhiteSpace(order.Symbol))
                throw new ArgumentException($"Symbol is required for order at index {index}", nameof(orders));
            if (order.Side != BinanceOrderSide.Buy && order.Side != BinanceOrderSide.Sell)
                throw new ArgumentOutOfRangeException(nameof(orders), order.Side, $"Side must be Buy or Sell for order at index {index}");
            if (order.Quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(orders), order.Quantity, $"Quantity must be greater than zero for order at index {index}");
            if (order.Price <= 0)
                throw new ArgumentOutOfRangeException(nameof(orders), order.Price, $"Price must be greater than zero for order at index {index}");
            if (order.OriginalClientOrderId != null && string.IsNullOrWhiteSpace(order.OriginalClientOrderId))
                throw new ArgumentException($"Original client order id cannot be empty for order at index {index}", nameof(orders));
            if (!order.OrderId.HasValue && order.OriginalClientOrderId == null)
                throw new ArgumentException($"Either OrderId or OriginalClientOrderId must be sent for order at index {index}", nameof(orders));
            if (order.PriceMatch.HasValue)
                throw new ArgumentException($"The current Binance COIN-M batch Modify Order contract does not provide a usable priceMatch combination for order at index {index}", nameof(orders));

            var orderParameters = new ParameterCollection()
            {
                { "symbol", order.Symbol },
                { "quantity", order.Quantity },
                { "price", order.Price }
            };
            orderParameters.AddEnum("side", order.Side);
            orderParameters.AddOptional("orderId", order.OrderId);
            orderParameters.AddOptional("origClientOrderId", order.OriginalClientOrderId);
            orderParameters.AddOptional("modifyId", order.ModifyId);
            parameterOrders.Add(orderParameters);
        }

        parameters.Add("batchOrders", JsonConvert.SerializeObject(parameterOrders));
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var response = await RequestAsync<List<BinanceFuturesCoinRestOrderAcknowledgementResult>>(GetUrl(dapi, v1, "batchOrders"), HttpMethod.Put, ct, true, bodyParameters: parameters, requestWeight: 5).ConfigureAwait(false);
        if (!response.Success) return response.As<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>([]);

        var result = new List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>();
        foreach (var item in response.Data)
        {
            result.Add(item.Code != 0
                ? new CallResult<BinanceFuturesCoinRestOrderAcknowledgement>(new ServerError(item.Code, item.Message))
                : new CallResult<BinanceFuturesCoinRestOrderAcknowledgement>(item));
        }

        return response.As<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>(result);
    }

    public Task<RestCallResult<List<BinanceFuturesOrderModifyHistory>>> GetOrderModifyHistoryAsync(string symbol, long? orderId = null, string? origClientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol is required", nameof(symbol));
        if (origClientOrderId is not null && string.IsNullOrWhiteSpace(origClientOrderId))
            throw new ArgumentException("origClientOrderId cannot be empty when provided", nameof(origClientOrderId));
        if (!orderId.HasValue && origClientOrderId is null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");
        if (limit > 100)
            throw new ArgumentOutOfRangeException(nameof(limit), limit, "limit cannot exceed 100");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));

        return RequestAsync<List<BinanceFuturesOrderModifyHistory>>(GetUrl(dapi, v1, "orderAmendment"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<BinanceFuturesCoinRestOrderAcknowledgement>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceFuturesCoinRestOrderAcknowledgement>(GetUrl(dapi, v1, "order"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
        if (result) InvokeOrderCanceled(result.Data.Id);

        return result;
    }

    public async Task<RestCallResult<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>> CancelOrdersAsync(string symbol, IEnumerable<long>? orderIdList = null, IEnumerable<string>? origClientOrderIdList = null, int? receiveWindow = null, CancellationToken ct = default)
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

        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var response = await RequestAsync<List<BinanceFuturesCoinRestOrderAcknowledgementResult>>(GetUrl(dapi, v1, "batchOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);

        if (!response.Success)
            return response.As<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>(default!);

        var result = new List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>();
        foreach (var item in response.Data)
        {
            if (item.Code == 0)
            {
                result.Add(new CallResult<BinanceFuturesCoinRestOrderAcknowledgement>(item));
                InvokeOrderCanceled(item.Id);
            }
            else
            {
                result.Add(new CallResult<BinanceFuturesCoinRestOrderAcknowledgement>(new ServerError(item.Code, item.Message)));
            }
        }

        return response.As<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>(result);
    }

    public async Task<RestCallResult<bool>> CancelAllOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceResponse>(GetUrl(dapi, v1, "allOpenOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public Task<RestCallResult<BinanceFuturesCountDownResult>> CancelAllOrdersAfterTimeoutAsync(string symbol, TimeSpan countDownTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "countdownTime", (int)countDownTime.TotalMilliseconds }
        };
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesCountDownResult>(GetUrl(dapi, v1, "countdownCancelAll"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceFuturesAlgoOrderPlacementResult>> PlaceAlgoOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceFuturesAlgoOrderType type,
        BinancePositionSide? positionSide = null,
        BinanceTimeInForce? timeInForce = null,
        decimal? quantity = null,
        decimal? price = null,
        decimal? triggerPrice = null,
        BinanceFuturesWorkingType? workingType = null,
        BinanceFuturesPriceMatch? priceMatch = null,
        bool? closePosition = null,
        bool? priceProtect = null,
        bool? reduceOnly = null,
        decimal? activatePrice = null,
        decimal? callbackRate = null,
        string? clientAlgoId = null,
        BinanceOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        DateTime? goodTillDate = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        BinanceFuturesAlgoOrderValidation.ValidatePlacement(
            symbol,
            side,
            type,
            positionSide,
            timeInForce,
            quantity,
            price,
            workingType,
            priceMatch,
            closePosition,
            priceProtect,
            reduceOnly,
            activatePrice,
            callbackRate,
            clientAlgoId,
            orderResponseType,
            selfTradePreventionMode,
            goodTillDate);

        var parameters = new ParameterCollection
        {
            { "algoType", "CONDITIONAL" },
            { "symbol", symbol }
        };
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptional("quantity", quantity?.ToString(BinanceConstants.CI));
        parameters.AddOptional("price", price?.ToString(BinanceConstants.CI));
        parameters.AddOptional("triggerPrice", triggerPrice?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("workingType", workingType);
        parameters.AddOptionalEnum("priceMatch", priceMatch);
        parameters.AddOptional("closePosition", closePosition?.ToString().ToLowerInvariant());
        parameters.AddOptional("priceProtect", priceProtect?.ToString().ToLowerInvariant());
        parameters.AddOptional("reduceOnly", reduceOnly?.ToString().ToLowerInvariant());
        parameters.AddOptional("activatePrice", activatePrice?.ToString(BinanceConstants.CI));
        parameters.AddOptional("callbackRate", callbackRate?.ToString(BinanceConstants.CI));
        parameters.AddOptional("clientAlgoId", clientAlgoId);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptionalMilliseconds("goodTillDate", goodTillDate);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesAlgoOrderPlacementResult>(GetUrl(dapi, v1, "algoOrder"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 0);
    }

    public Task<RestCallResult<BinanceFuturesAlgoOrderCancellationResult>> CancelAlgoOrderAsync(long? algoId = null, string? clientAlgoId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        BinanceFuturesAlgoOrderValidation.ValidateCancellation(algoId, clientAlgoId);

        var parameters = new ParameterCollection();
        parameters.AddOptional("algoId", algoId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("clientAlgoId", clientAlgoId);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesAlgoOrderCancellationResult>(GetUrl(dapi, v1, "algoOrder"), HttpMethod.Delete, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesAlgoOrderListItem>>> GetOpenAlgoOrdersAsync(string? symbol = null, string? algoType = null, long? algoId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (symbol is not null && string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol cannot be empty when provided", nameof(symbol));
        if (algoType is not null && string.IsNullOrWhiteSpace(algoType))
            throw new ArgumentException("algoType cannot be empty when provided", nameof(algoType));

        var parameters = new ParameterCollection();
        parameters.AddOptional("algoType", algoType);
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("algoId", algoId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var weight = symbol is null ? 40 : 1;
        return RequestAsync<List<BinanceFuturesAlgoOrderListItem>>(GetUrl(dapi, v1, "openAlgoOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<BinanceFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>(GetUrl(dapi, v1, "order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesOrder>>> GetOrdersAsync(string? symbol = null, string? pair = null, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (symbol is not null && string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol cannot be empty when provided", nameof(symbol));
        if (pair is not null && string.IsNullOrWhiteSpace(pair))
            throw new ArgumentException("pair cannot be empty when provided", nameof(pair));
        if ((symbol is null) == (pair is null))
            throw new ArgumentException("Exactly one of symbol or pair must be provided");
        if (pair is not null && orderId.HasValue)
            throw new ArgumentException("orderId can only be combined with symbol", nameof(orderId));
        limit?.ValidateIntBetween(nameof(limit), 1, 100);
        if (startTime.HasValue && endTime.HasValue)
        {
            if (endTime.Value <= startTime.Value)
                throw new ArgumentOutOfRangeException(nameof(endTime), "endTime must be later than startTime");
            if (endTime.Value - startTime.Value >= TimeSpan.FromDays(7))
                throw new ArgumentOutOfRangeException(nameof(endTime), "The query time period must be less than 7 days");
        }

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("pair", pair);
        parameters.AddOptional("orderId", orderId?.ToString(BinanceConstants.CI));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));

        return RequestAsync<List<BinanceFuturesOrder>>(GetUrl(dapi, v1, "allOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceFuturesOrder>>> GetOpenOrdersAsync(string? symbol = null, string? pair = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (symbol is not null && string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol cannot be empty when provided", nameof(symbol));
        if (pair is not null && string.IsNullOrWhiteSpace(pair))
            throw new ArgumentException("pair cannot be empty when provided", nameof(pair));

        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("pair", pair);

        var weight = symbol == null ? 40 : 1;
        return RequestAsync<List<BinanceFuturesOrder>>(GetUrl(dapi, v1, "openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<BinanceFuturesOrder>> GetOpenOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>(GetUrl(dapi, v1, "openOrder"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesOrder>>> GetForcedOrdersAsync(string? symbol = null, BinanceFuturesAutoCloseType? autoCloseType = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, int? limit = null, CancellationToken ct = default)
    {
        if (symbol is not null && string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol cannot be empty when provided", nameof(symbol));
        if (limit > 100)
            throw new ArgumentOutOfRangeException(nameof(limit), limit, "limit cannot exceed 100");

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptionalEnum("autoCloseType", autoCloseType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var weight = symbol == null ? 50 : 20;
        return RequestAsync<List<BinanceFuturesOrder>>(GetUrl(dapi, v1, "forceOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinUserTrade>>> GetUserTradesAsync(string? symbol = null, string? pair = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, string? orderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (symbol is not null && string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol cannot be empty when provided", nameof(symbol));
        if (pair is not null && string.IsNullOrWhiteSpace(pair))
            throw new ArgumentException("pair cannot be empty when provided", nameof(pair));
        if ((symbol is null) == (pair is null))
            throw new ArgumentException("Exactly one of symbol or pair must be provided");
        if (orderId is not null && string.IsNullOrWhiteSpace(orderId))
            throw new ArgumentException("orderId cannot be empty when provided", nameof(orderId));
        if (pair is not null && fromId.HasValue)
            throw new ArgumentException("fromId cannot be combined with pair", nameof(fromId));
        if (pair is not null && orderId is not null)
            throw new ArgumentException("orderId can only be combined with symbol", nameof(orderId));
        if (fromId.HasValue && (startTime.HasValue || endTime.HasValue))
            throw new ArgumentException("fromId cannot be combined with startTime or endTime", nameof(fromId));
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);
        if (startTime.HasValue && endTime.HasValue && endTime.Value - startTime.Value > TimeSpan.FromDays(7))
            throw new ArgumentOutOfRangeException(nameof(endTime), "The query time period cannot exceed 7 days");

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("pair", pair);
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("fromId", fromId?.ToString(BinanceConstants.CI));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceFuturesCoinUserTrade>>(GetUrl(dapi, v1, "userTrades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<List<BinanceFuturesCoinPositionRisk>>> GetPositionsAsync(string? marginAsset = null, string? pair = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();

        parameters.AddOptional("marginAsset", marginAsset);
        parameters.AddOptional("pair", pair);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceFuturesCoinPositionRisk>>(GetUrl(dapi, v1, "positionRisk"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<bool>> SetPositionModeAsync(bool dualPositionSide, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "dualSidePosition", dualPositionSide.ToString().ToLower() }
        };
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceResponse>(GetUrl(dapi, v1, "positionSide/dual"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public async Task<RestCallResult<bool>> SetMarginTypeAsync(string symbol, BinanceFuturesMarginType marginType, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddEnum("marginType", marginType);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceResponse>(GetUrl(dapi, v1, "marginType"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public Task<RestCallResult<BinanceFuturesInitialLeverageChangeResult>> SetInitialLeverageAsync(string symbol, int leverage, int? receiveWindow = null, CancellationToken ct = default)
    {
        leverage.ValidateIntBetween(nameof(leverage), 1, 125);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "leverage", leverage }
        };
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesInitialLeverageChangeResult>(GetUrl(dapi, v1, "leverage"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceFuturesQuantileEstimation>>(GetUrl(dapi, v1, "adlQuantile"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceFuturesPositionMarginResult>> SetPositionMarginAsync(string symbol, decimal quantity, BinanceFuturesMarginChangeDirectionType type, BinancePositionSide? positionSide = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "amount", quantity.ToString(BinanceConstants.CI) },
        };
        parameters.AddEnum("type", type);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesPositionMarginResult>(GetUrl(dapi, v1, "positionMargin"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceFuturesMarginChangeHistoryResult>>> GetMarginChangeHistoryAsync(string symbol, BinanceFuturesMarginChangeDirectionType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));

        return RequestAsync<List<BinanceFuturesMarginChangeHistoryResult>>(GetUrl(dapi, v1, "positionMargin/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}
