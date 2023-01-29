using Binance.Api.Models.RestApi.Account;

namespace Binance.Api.Clients.RestApi.Spot;

public class BinanceRestApiSpotTradingClient
{
    // Api
    protected const string api = "api";
    protected const string publicVersion = "3";
    protected const string signedVersion = "3";

    // Spot Trading
    private const string newTestOrderEndpoint = "order/test";
    private const string newOrderEndpoint = "order";
    private const string cancelOrderEndpoint = "order";
    private const string cancelAllOpenOrderEndpoint = "openOrders";
    private const string queryOrderEndpoint = "order";
    private const string cancelReplaceOrderEndpoint = "order/cancelReplace";
    private const string openOrdersEndpoint = "openOrders";
    private const string allOrdersEndpoint = "allOrders";

    // OCO Orders
    private const string newOcoOrderEndpoint = "order/oco";
    private const string cancelOcoOrderEndpoint = "orderList";
    private const string getOcoOrderEndpoint = "orderList";
    private const string getAllOcoOrderEndpoint = "allOrderList";
    private const string getOpenOcoOrderEndpoint = "openOrderList";

    // Account
    private const string accountInfoEndpoint = "account";
    private const string myTradesEndpoint = "myTrades";
    private const string orderRateLimitEndpoint = "rateLimit/order";
    // TODO: Query Prevented Matches (USER_DATA)

    /// <summary>
    /// Event triggered when an order is placed via this client. Only available for Spot orders
    /// </summary>
    public event Action<OrderId> OnOrderPlaced;

    /// <summary>
    /// Event triggered when an order is canceled via this client. 
    /// Note that this does not trigger when using CancelAllOrdersAsync. Only available for Spot orders
    /// </summary>
    public event Action<OrderId> OnOrderCanceled;

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiSpotClient SpotClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => SpotClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await SpotClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiSpotTradingClient(BinanceRestApiClient root, BinanceRestApiSpotClient spot)
    {
        RootClient = root;
        SpotClient = spot;
    }

    internal void InvokeOrderPlaced(OrderId id)
        => OnOrderPlaced?.Invoke(id);

    internal void InvokeOrderCanceled(OrderId id)
        => OnOrderCanceled?.Invoke(id);

    #region Test New Order 
    public async Task<RestCallResult<BinancePlacedOrder>> PlaceTestOrderAsync(string symbol,
        OrderSide side,
        SpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        string newClientOrderId = null,
        decimal? price = null,
        TimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQty = null,
        OrderResponseType? orderResponseType = null,
        int? trailingDelta = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        return await SpotClient.PlaceOrderInternal(GetUrl(newTestOrderEndpoint, api, signedVersion),
            symbol,
            side,
            type,
            quantity,
            quoteQuantity,
            newClientOrderId,
            price,
            timeInForce,
            stopPrice,
            icebergQty,
            null,
            null,
            orderResponseType,
            trailingDelta,
            receiveWindow,
            1,
            ct).ConfigureAwait(false);
    }
    #endregion

    #region New Order
    public async Task<RestCallResult<BinancePlacedOrder>> PlaceOrderAsync(string symbol,
        OrderSide side,
        SpotOrderType type,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        string newClientOrderId = null,
        decimal? price = null,
        TimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQty = null,
        OrderResponseType? orderResponseType = null,
        int? trailingDelta = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var result = await SpotClient.PlaceOrderInternal(GetUrl(newOrderEndpoint, api, signedVersion),
            symbol,
            side,
            type,
            quantity,
            quoteQuantity,
            newClientOrderId,
            price,
            timeInForce,
            stopPrice,
            icebergQty,
            null,
            null,
            orderResponseType,
            trailingDelta,
            receiveWindow,
            1,
            ct).ConfigureAwait(false);
        if (result)
            InvokeOrderPlaced(new OrderId() { SourceObject = result.Data, Id = result.Data.Id.ToString(CultureInfo.InvariantCulture) });
        return result;
    }
    #endregion

    #region Cancel Order
    public async Task<RestCallResult<BinanceOrderBase>> CancelOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, string newClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceOrderBase>(GetUrl(cancelOrderEndpoint, api, signedVersion), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
        if (result)
            InvokeOrderCanceled(new OrderId() { SourceObject = result.Data, Id = result.Data.Id.ToString(CultureInfo.InvariantCulture) });
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderBase>>(GetUrl(cancelAllOpenOrderEndpoint, api, signedVersion), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Cancel an Existing Order and Send a New Order
    public async Task<RestCallResult<BinanceReplaceOrderResult>> ReplaceOrderAsync(string symbol,
        OrderSide side,
        SpotOrderType type,
        CancelReplaceMode cancelReplaceMode,
        long? cancelOrderId = null,
        string cancelClientOrderId = null,
        string newCancelClientOrderId = null,
        string newClientOrderId = null,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        TimeInForce? timeInForce = null,
        decimal? stopPrice = null,
        decimal? icebergQty = null,
        OrderResponseType? orderResponseType = null,
        int? trailingDelta = null,
        int? strategyId = null,
        int? strategyType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        if (cancelOrderId == null && cancelClientOrderId == null || cancelOrderId != null && cancelClientOrderId != null)
            throw new ArgumentException("1 of either should be specified, cancelOrderId or cancelClientOrderId");

        if (quoteQuantity != null && type != SpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await SpotClient.CheckTradeRules(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            SpotClient.Log.Write(LogLevel.Warning, rulesCheck.ErrorMessage!);
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
                { "cancelReplaceMode", EnumConverter.GetString(cancelReplaceMode) }
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceReplaceOrderResult>(GetUrl(cancelReplaceOrderEndpoint, api, signedVersion), HttpMethod.Post, ct, parameters, true, weight: 1).ConfigureAwait(false);
        if (!result && result.OriginalData != null)
        {
            // Attempt to parse the error
            var jsonData = result.OriginalData.ToJToken(SpotClient.Log);
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
            InvokeOrderPlaced(new OrderId() { SourceObject = result.Data, Id = result.Data.NewOrderResponse!.Id.ToString(CultureInfo.InvariantCulture) });
        return result;
    }
    #endregion

    #region Current Open Orders
    public async Task<RestCallResult<IEnumerable<BinanceOrder>>> GetOpenOrdersAsync(string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol?.ValidateBinanceSymbol();

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);

        return await SendRequestInternal<IEnumerable<BinanceOrder>>(GetUrl(openOrdersEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true, weight: symbol == null ? 40 : 3).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceOrder>(GetUrl(queryOrderEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true, weight: 2).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrder>>(GetUrl(allOrdersEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region New OCO
    public async Task<RestCallResult<BinanceOrderOcoList>> PlaceOcoOrderAsync(string symbol,
        OrderSide side,
        decimal quantity,
        decimal price,
        decimal stopPrice,
        decimal? stopLimitPrice = null,
        string listClientOrderId = null,
        string limitClientOrderId = null,
        string stopClientOrderId = null,
        decimal? limitIcebergQuantity = null,
        decimal? stopIcebergQuantity = null,
        TimeInForce? stopLimitTimeInForce = null,
        int? trailingDelta = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var rulesCheck = await SpotClient.CheckTradeRules(symbol, quantity, null, price, stopPrice, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            SpotClient.Log.Write(LogLevel.Warning, rulesCheck.ErrorMessage!);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceOrderOcoList>(GetUrl(newOcoOrderEndpoint, api, signedVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceOrderOcoList>(GetUrl(cancelOcoOrderEndpoint, api, signedVersion), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceOrderOcoList>(GetUrl(getOcoOrderEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true, weight: 2).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderOcoList>>(GetUrl(getAllOcoOrderEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Open OCO
    public async Task<RestCallResult<IEnumerable<BinanceOrderOcoList>>> GetOpenOcoOrdersAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderOcoList>>(GetUrl(getOpenOcoOrderEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true, weight: 3).ConfigureAwait(false);
    }
    #endregion

    #region Account Information
    public async Task<RestCallResult<BinanceAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceAccountInfo>(GetUrl(accountInfoEndpoint, api, "3"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
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
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceTrade>>(GetUrl(myTradesEndpoint, api, signedVersion), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Current Order Count Usage
    public async Task<RestCallResult<IEnumerable<BinanceOrderRateLimit>>> GetOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceOrderRateLimit>>(GetUrl(orderRateLimitEndpoint, api, "3"), HttpMethod.Get, ct, parameters, true, weight: 20).ConfigureAwait(false);
    }
    #endregion

}