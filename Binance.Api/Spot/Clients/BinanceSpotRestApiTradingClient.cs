namespace Binance.Api.Spot;

public class BinanceSpotRestApiTradingClient(BinanceSpotRestApiClient parent)
{
    // Api
    private const string api = "api";
    private const string v1 = "1";
    private const string v3 = "3";

    // Parent Clients
    internal BinanceRestApiClient _ { get; } = parent._;
    internal BinanceSpotRestApiClient __ { get; } = parent;
    internal ILogger? Logger { get; } = parent.Logger;
    internal BinanceRestApiClientOptions ClientOptions { get; } = parent.ClientOptions;

    public async Task<RestCallResult<BinancePlacedOrder>> PlaceOrderAsync(
        string symbol,
        BinanceSpotOrderSide side,
        BinanceSpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        string? newClientOrderId = null,
        BinanceSpotTimeInForce? timeInForce = null,
        BinanceSpotOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        int? trailingDelta = null,
        int? strategyId = null,
        int? strategyType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var result = await __.PlaceOrderInternal(
            __.GetUrl(api, v3, "order"),
            symbol,
            side,
            type,
            quantity,
            quoteQuantity,
            price,
            stopPrice,
            icebergQuantity,
            newClientOrderId,
            timeInForce,
            orderResponseType,
            selfTradePreventionMode,
            trailingDelta,
            strategyId,
            strategyType,
            receiveWindow,
            null,
            null,
            null,
            1,
            ct).ConfigureAwait(false);
        if (result) __.InvokeOrderPlaced(result.Data.Id);
        return result;
    }

    public async Task<RestCallResult<BinanceSpotTestOrderFee>> PlaceTestOrderAsync(
        string symbol,
        BinanceSpotOrderSide side,
        BinanceSpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        decimal? stopPrice = null,
        decimal? icebergQuantity = null,
        string? newClientOrderId = null,
        BinanceSpotTimeInForce? timeInForce = null,
        BinanceSpotOrderResponseType? orderResponseType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        int? trailingDelta = null,
        int? strategyId = null,
        int? strategyType = null,
        int? receiveWindow = null,
        bool? computeFeeRates = null,
        CancellationToken ct = default)
    {
        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await __.CheckTradeRules(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger?.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceSpotTestOrderFee>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("quoteOrderQty", quoteQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptional("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("icebergQty", icebergQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptional("computeCommissionRates", computeFeeRates?.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptionalString("recvWindow", receiveWindow ?? Convert.ToInt32(__.ClientOptions.ReceiveWindow.TotalMilliseconds));

        var weight = computeFeeRates == true ? 20 : 1;
        return await __.SendRequestInternal<BinanceSpotTestOrderFee>(__.GetUrl(api, v3, "order/test"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: weight).ConfigureAwait(false);
    }

    #region Cancel Order
    public async Task<RestCallResult<BinanceOrderBase>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string newClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await __.SendRequestInternal<BinanceOrderBase>(__.GetUrl(api, v3, "order"), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        if (result)
            __.InvokeOrderCanceled(result.Data.Id);
        return result;
    }
    #endregion

    #region Cancel All Open Orders on a Symbol
    public async Task<RestCallResult<IEnumerable<BinanceOrderBase>>> CancelAllOrdersAsync(string symbol, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<IEnumerable<BinanceOrderBase>>(__.GetUrl(api, v3, "openOrders"), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Cancel an Existing Order and Send a New Order
    public async Task<RestCallResult<BinanceReplaceOrderResult>> ReplaceOrderAsync(string symbol,
        BinanceSpotOrderSide side,
        BinanceSpotOrderType type,
        CancelReplaceMode cancelReplaceMode,
        long? cancelOrderId = null,
        string? cancelClientOrderId = null,
        string? newCancelClientOrderId = null,
        string? newClientOrderId = null,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        BinanceSpotTimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQty = null,
        BinanceSpotOrderResponseType? orderResponseType = null,
        int? trailingDelta = null,
        int? strategyId = null,
        int? strategyType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        if (cancelOrderId == null && cancelClientOrderId == null || cancelOrderId != null && cancelClientOrderId != null)
            throw new ArgumentException("1 of either should be specified, cancelOrderId or cancelClientOrderId");

        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await __.CheckTradeRules(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceReplaceOrderResult>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "type", JsonConvert.SerializeObject(type, new SpotOrderTypeConverter(false)) },
                { "cancelReplaceMode", MapConverter.GetString(cancelReplaceMode) }
            };
        parameters.AddOptionalParameter("cancelNewClientOrderId", newCancelClientOrderId);
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("cancelOrderId", cancelOrderId);
        parameters.AddOptionalParameter("strategyId", strategyId);
        parameters.AddOptionalParameter("strategyType", strategyType);
        parameters.AddOptionalParameter("cancelOrigClientOrderId", cancelClientOrderId);
        parameters.AddOptionalParameter("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("quoteOrderQty", quoteQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("timeInForce", timeInForce == null ? null : JsonConvert.SerializeObject(timeInForce, new TimeInForceConverter(false)));
        parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("icebergQty", icebergQty?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("newOrderRespType", orderResponseType == null ? null : JsonConvert.SerializeObject(orderResponseType, new OrderResponseTypeConverter(false)));
        parameters.AddOptionalParameter("trailingDelta", trailingDelta);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await __.SendRequestInternal<BinanceReplaceOrderResult>(__.GetUrl(api, v3, "order/cancelReplace"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1).ConfigureAwait(false);
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
                    return result.AsError<BinanceReplaceOrderResult>(new ServerError(error!.Value<int>("code"), error.Value<string>("msg")!));
            }
        }

        if (result && result.Data.NewOrderResult == OrderOperationResult.Success)
            __.InvokeOrderPlaced(result.Data.NewOrderResponse!.Id);
        return result;
    }
    #endregion

    #region Current Open Orders
    public async Task<RestCallResult<IEnumerable<BinanceOrder>>> GetOpenOrdersAsync(string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol?.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);

        return await __.SendRequestInternal<IEnumerable<BinanceOrder>>(__.GetUrl(api, v3, "openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 40 : 3).ConfigureAwait(false);
    }
    #endregion

    #region Query Order
    public async Task<RestCallResult<BinanceOrder>> GetOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<BinanceOrder>(__.GetUrl(api, v3, "order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
    }
    #endregion

    #region All Orders 
    public async Task<RestCallResult<IEnumerable<BinanceOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<IEnumerable<BinanceOrder>>(__.GetUrl(api, v3, "allOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region New OCO
    public async Task<RestCallResult<BinanceOrderOcoList>> PlaceOcoOrderAsync(string symbol,
        BinanceSpotOrderSide side,
        decimal quantity,
        decimal price,
        decimal stopPrice,
        decimal? stopLimitPrice = null,
        string listClientOrderId = null,
        string limitClientOrderId = null,
        string stopClientOrderId = null,
        decimal? limitIcebergQuantity = null,
        decimal? stopIcebergQuantity = null,
        BinanceSpotTimeInForce? stopLimitTimeInForce = null,
        int? trailingDelta = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var rulesCheck = await __.CheckTradeRules(symbol, quantity, null, price, stopPrice, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceOrderOcoList>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity!.Value;
        price = rulesCheck.Price!.Value;
        stopPrice = rulesCheck.StopPrice!.Value;

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
                { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
                { "price", price.ToString(CultureInfo.InvariantCulture) },
                { "stopPrice", stopPrice.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("stopLimitPrice", stopLimitPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("listClientOrderId", listClientOrderId);
        parameters.AddOptionalParameter("limitClientOrderId", limitClientOrderId);
        parameters.AddOptionalParameter("stopClientOrderId", stopClientOrderId);
        parameters.AddOptionalParameter("limitIcebergQty", limitIcebergQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("stopIcebergQty", stopIcebergQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("stopLimitTimeInForce", stopLimitTimeInForce == null ? null : JsonConvert.SerializeObject(stopLimitTimeInForce, new TimeInForceConverter(false)));
        parameters.AddOptionalParameter("trailingDelta", trailingDelta);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<BinanceOrderOcoList>(__.GetUrl(api, v3, "order/oco"), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Cancel OCO 
    public async Task<RestCallResult<BinanceOrderOcoList>> CancelOcoOrderAsync(string symbol, long? orderListId = null, string listClientOrderId = null, string newClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        if (!orderListId.HasValue && string.IsNullOrEmpty(listClientOrderId))
            throw new ArgumentException("Either orderListId or listClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderListId", orderListId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("listClientOrderId", listClientOrderId);
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<BinanceOrderOcoList>(__.GetUrl(api, v3, "orderList"), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Query OCO
    public async Task<RestCallResult<BinanceOrderOcoList>> GetOcoOrderAsync(long? orderListId = null, string origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderListId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderListId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("orderListId", orderListId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<BinanceOrderOcoList>(__.GetUrl(api, v3, "orderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 2).ConfigureAwait(false);
    }
    #endregion

    #region Query All OCO
    public async Task<RestCallResult<IEnumerable<BinanceOrderOcoList>>> GetOcoOrdersAsync(long? fromId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (fromId != null && (startTime != null || endTime != null))
            throw new ArgumentException("Start/end time can only be provided without fromId parameter");

        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<IEnumerable<BinanceOrderOcoList>>(__.GetUrl(api, v3, "allOrderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Open OCO
    public async Task<RestCallResult<IEnumerable<BinanceOrderOcoList>>> GetOpenOcoOrdersAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<IEnumerable<BinanceOrderOcoList>>(__.GetUrl(api, v3, "openOrderList"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3).ConfigureAwait(false);
    }
    #endregion

    #region Account Information
    public async Task<RestCallResult<BinanceAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<BinanceAccountInfo>(__.GetUrl(api, v3, "account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Account Trade List
    public async Task<RestCallResult<IEnumerable<BinanceTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<IEnumerable<BinanceTrade>>(__.GetUrl(api, v3, "myTrades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Current Order Count Usage
    public async Task<RestCallResult<IEnumerable<BinanceOrderRateLimit>>> GetOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<IEnumerable<BinanceOrderRateLimit>>(__.GetUrl(api, v3, "rateLimit/order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
    }
    #endregion

    // TODO: Query Prevented Matches (USER_DATA)

}