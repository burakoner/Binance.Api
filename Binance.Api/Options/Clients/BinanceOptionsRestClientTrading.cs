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
        decimal? quantity = null,
        decimal? price = null,
        BinanceTimeInForce? timeInForce = null,
        string? clientOrderId = null,
        bool? reduceOnly = null,
        bool? postOnly = null,
        bool? isMmp = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var rulesCheck = await CheckTradingRulesAsync(symbol, type, quantity, null, price, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceOptionsOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        // stopPrice = rulesCheck.StopPrice;
        // quoteQuantity = rulesCheck.QuoteQuantity;
        clientOrderId = BinanceHelpers.ApplyBrokerId(clientOrderId, BinanceConstants.ClientOrderIdSpot, 36, RestOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddParameter("newOrderRespType", "RESULT");
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity?.ToString(BinanceConstants.CI));
        parameters.AddOptional("price", price?.ToString(BinanceConstants.CI));
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptional("clientOrderId", clientOrderId);
        parameters.AddOptional("reduceOnly", reduceOnly);
        parameters.AddOptional("postOnly", postOnly);
        parameters.AddOptional("isMmp", isMmp);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceOptionsOrder>(GetUrl(eapi, v1, "order"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (result) InvokeOrderPlaced(result.Data.Id);

        return result;
    }

    public async Task<RestCallResult<List<BinanceOptionsOrder>>> PlaceOrdersAsync(IEnumerable<BinanceOptionsBatchOrderRequest> orders, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orders.Count() <= 0 || orders.Count() > 5)
            throw new ArgumentException("Order list should be at least 1 and max 5 orders");

        if (RestOptions.CoinFuturesOptions.TradeRulesBehavior != BinanceTradeRulesBehavior.None)
        {
            foreach (var order in orders)
            {
                var rulesCheck = await CheckTradingRulesAsync(order.Symbol, order.Type, order.Quantity, null, order.Price, null, ct).ConfigureAwait(false);
                if (!rulesCheck.Passed)
                {
                    Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
                    return new RestCallResult<List<BinanceOptionsOrder>>(new ArgumentError(rulesCheck.ErrorMessage!));
                }

                order.Quantity = rulesCheck.Quantity;
                order.Price = rulesCheck.Price;
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
            orderParameters.AddOptional("quantity", order.Quantity?.ToString(BinanceConstants.CI));
            orderParameters.AddOptional("price", order.Price?.ToString(BinanceConstants.CI));
            orderParameters.AddOptionalEnum("timeInForce", order.TimeInForce);
            var clientOrderId = BinanceHelpers.ApplyBrokerId(order.ClientOrderId, BinanceConstants.ClientOrderIdFutures, 36, RestOptions.AllowAppendingClientOrderId);
            orderParameters.AddOptional("clientOrderId", clientOrderId);
            orderParameters.AddOptional("reduceOnly", order.ReduceOnly?.ToString().ToLower());
            orderParameters.AddOptional("postOnly", order.PostOnly?.ToString().ToLower());
            orderParameters.AddOptional("isMmp", order.MMP?.ToString().ToLower());
            parameterOrders[i] = orderParameters;
            i++;
        }

        parameters.Add("orders", JsonConvert.SerializeObject(parameterOrders));
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var response = await RequestAsync<List<BinanceOptionsOrder>>(GetUrl(eapi, v1, "batchOrders"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 5);
        if (!response.Success) return response.As<List<BinanceOptionsOrder>>([]);

        foreach (var item in response.Data)
        {
            if (item.Id > 0) InvokeOrderPlaced(item.Id);
        }

        return response;
    }

    public async Task<RestCallResult<BinanceOptionsOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? clientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (!orderId.HasValue && string.IsNullOrEmpty(clientOrderId))
            throw new ArgumentException("Either orderId or clientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("clientOrderId", clientOrderId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceOptionsOrder>(GetUrl(eapi, v1, "order"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (result) InvokeOrderCanceled(result.Data.Id);
        return result;
    }

    public async Task<RestCallResult<List<BinanceOptionsOrder>>> CancelOrdersAsync(string symbol, IEnumerable<long>? orderIdList = null, IEnumerable<string>? origClientOrderIdList = null, int? receiveWindow = null, CancellationToken ct = default)
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
            parameters.AddOptional("orderIds", $"[{string.Join(",", orderIdList)}]");

        if (origClientOrderIdList != null)
            parameters.AddOptional("clientOrderIds", $"[{string.Join(",", origClientOrderIdList.Select(id => $"\"{id}\""))}]");

        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var response = await RequestAsync<List<BinanceOptionsOrder>>(GetUrl(eapi, v1, "batchOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 1);
        if (!response.Success) return response.As<List<BinanceOptionsOrder>>([]);

        foreach (var item in response.Data)
        {
            if (item.Id > 0) InvokeOrderCanceled(item.Id);
        }

        return response;
    }

    public async Task<RestCallResult<long>> CancelOrdersByUnderlyingAsync(string underlying, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("underlying", underlying);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result =await RequestAsync<BinanceResponse<long>>(GetUrl(eapi, v1, "allOpenOrdersByUnderlying"), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        if(!result.Success) return result.As<long>(0);

        return result.As(result.Data.Data);
    }

    public async Task<RestCallResult<bool>> CancelOrdersBySymbolAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceResponse>(GetUrl(eapi, v1, "allOpenOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        return result.As(result.Success);
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

}