using Binance.Api.Models.RestApi.Futures;

namespace Binance.Api.Clients.RestApi.UsdtFutures;

public class BinanceRestApiUsdtFuturesTradingClient
{
    // Api
    private const string api = "fapi";
    private const string signedVersion = "1";

    // Trade
    private const string newOrderEndpoint = "order";
    private const string multipleNewOrdersEndpoint = "batchOrders";
    private const string queryOrderEndpoint = "order";
    private const string cancelOrderEndpoint = "order";
    private const string cancelAllOrdersEndpoint = "allOpenOrders";
    private const string cancelMultipleOrdersEndpoint = "batchOrders";
    private const string countDownCancelAllEndpoint = "countdownCancelAll";
    private const string openOrderEndpoint = "openOrder";
    private const string openOrdersEndpoint = "openOrders";
    private const string allOrdersEndpoint = "allOrders";
    private const string myFuturesTradesEndpoint = "userTrades";
    private const string forceOrdersEndpoint = "forceOrders";

    // Internal References
    internal BinanceRestApiUsdtFuturesClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    RestParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiUsdtFuturesTradingClient(BinanceRestApiUsdtFuturesClient main)
    {
        MainClient = main;
    }

    #region New Order
    public async Task<RestCallResult<BinanceFuturesPlacedOrder>> PlaceOrderAsync(
        string symbol,
        OrderSide side,
        FuturesOrderType type,
        decimal? quantity,
        decimal? price = null,
        PositionSide? positionSide = null,
        TimeInForce? timeInForce = null,
        bool? reduceOnly = null,
        string newClientOrderId = null,
        decimal? stopPrice = null,
        decimal? activationPrice = null,
        decimal? callbackRate = null,
        WorkingType? workingType = null,
        bool? closePosition = null,
        OrderResponseType? orderResponseType = null,
        bool? priceProtect = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (closePosition == true && positionSide != null)
        {
            if (positionSide == PositionSide.Short && side == OrderSide.Sell)
                throw new ArgumentException("Can't close short position with order side sell");
            if (positionSide == PositionSide.Long && side == OrderSide.Buy)
                throw new ArgumentException("Can't close long position with order side buy");
        }

        if (orderResponseType == OrderResponseType.Full)
            throw new ArgumentException("OrderResponseType.Full is not supported in Futures");

        var rulesCheck = await MainClient.CheckTradeRules(symbol, quantity, null, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            MainClient.Log.Write(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceFuturesPlacedOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;

        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol },
            { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
            { "type", JsonConvert.SerializeObject(type, new FuturesOrderTypeConverter(false)) }
        };
        parameters.AddOptionalParameter("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("newClientOrderId", newClientOrderId);
        parameters.AddOptionalParameter("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("timeInForce", timeInForce == null ? null : JsonConvert.SerializeObject(timeInForce, new TimeInForceConverter(false)));
        parameters.AddOptionalParameter("positionSide", positionSide == null ? null : JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)));
        parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("activationPrice", activationPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("callbackRate", callbackRate?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("workingType", workingType == null ? null : JsonConvert.SerializeObject(workingType, new WorkingTypeConverter(false)));
        parameters.AddOptionalParameter("reduceOnly", reduceOnly?.ToString().ToLower());
        parameters.AddOptionalParameter("closePosition", closePosition?.ToString().ToLower());
        parameters.AddOptionalParameter("newOrderRespType", orderResponseType == null ? null : JsonConvert.SerializeObject(orderResponseType, new OrderResponseTypeConverter(false)));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("priceProtect", priceProtect?.ToString().ToUpper());

        var result = await SendRequestInternal<BinanceFuturesPlacedOrder>(GetUrl(newOrderEndpoint, api, "1"), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        if (result)
        {
            MainClient.InvokeOrderPlaced(new OrderId
            {
                SourceObject = result.Data,
                Id = result.Data.Id.ToString(CultureInfo.InvariantCulture)
            });
        }
        return result;
    }
    #endregion

    #region Place Multiple Orders
    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesPlacedOrder>>>> PlaceMultipleOrdersAsync(BinanceFuturesBatchOrder[] orders, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orders.Length <= 0 || orders.Length > 5)
            throw new ArgumentException("Order list should be at least 1 and max 5 orders");

        if (Options.UsdtFuturesOptions.TradeRulesBehavior != TradeRulesBehavior.None)
        {
            foreach (var order in orders)
            {
                var rulesCheck = await MainClient.CheckTradeRules(order.Symbol, order.Quantity, null, order.Price, order.StopPrice, order.Type, ct).ConfigureAwait(false);
                if (!rulesCheck.Passed)
                {
                    MainClient.Log.Write(LogLevel.Warning, rulesCheck.ErrorMessage!);
                    return new RestCallResult<IEnumerable<CallResult<BinanceFuturesPlacedOrder>>>(new ArgumentError(rulesCheck.ErrorMessage!));
                }

                order.Quantity = rulesCheck.Quantity;
                order.Price = rulesCheck.Price;
                order.StopPrice = rulesCheck.StopPrice;
            }
        }

        var parameters = new Dictionary<string, object>();
        var parameterOrders = new Dictionary<string, object>[orders.Length];
        int i = 0;
        foreach (var order in orders)
        {
            var orderParameters = new Dictionary<string, object>()
            {
                { "symbol", order.Symbol },
                { "side", JsonConvert.SerializeObject(order.Side, new OrderSideConverter(false)) },
                { "type", JsonConvert.SerializeObject(order.Type, new FuturesOrderTypeConverter(false)) },
                { "newOrderRespType", "RESULT" }
            };

            orderParameters.AddOptionalParameter("quantity", order.Quantity?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("newClientOrderId", order.NewClientOrderId);
            orderParameters.AddOptionalParameter("price", order.Price?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("timeInForce", order.TimeInForce == null ? null : JsonConvert.SerializeObject(order.TimeInForce, new TimeInForceConverter(false)));
            orderParameters.AddOptionalParameter("positionSide", order.PositionSide == null ? null : JsonConvert.SerializeObject(order.PositionSide, new PositionSideConverter(false)));
            orderParameters.AddOptionalParameter("stopPrice", order.StopPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("activationPrice", order.ActivationPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("callbackRate", order.CallbackRate?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("workingType", order.WorkingType == null ? null : JsonConvert.SerializeObject(order.WorkingType, new WorkingTypeConverter(false)));
            orderParameters.AddOptionalParameter("reduceOnly", order.ReduceOnly?.ToString().ToLower());
            orderParameters.AddOptionalParameter("priceProtect", order.PriceProtect?.ToString().ToUpper());
            parameterOrders[i] = orderParameters;
            i++;
        }

        parameters.Add("batchOrders", JsonConvert.SerializeObject(parameterOrders));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var response = await SendRequestInternal<IEnumerable<BinanceFuturesMultipleOrderPlaceResult>>(GetUrl(multipleNewOrdersEndpoint, api, "1"), HttpMethod.Post, ct, parameters, true, weight: 5).ConfigureAwait(false);
        if (!response.Success)
            return response.As<IEnumerable<CallResult<BinanceFuturesPlacedOrder>>>(default);

        var result = new List<CallResult<BinanceFuturesPlacedOrder>>();
        foreach (var item in response.Data)
        {
            result.Add(item.Code != 0
                ? new CallResult<BinanceFuturesPlacedOrder>(new ServerError(item.Code, item.Message))
                : new CallResult<BinanceFuturesPlacedOrder>(item));
        }

        return response.As<IEnumerable<CallResult<BinanceFuturesPlacedOrder>>>(result);
    }
    #endregion

    #region Query Order
    public async Task<RestCallResult<BinanceFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesOrder>(GetUrl(queryOrderEndpoint, api, "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Cancel Order
    public async Task<RestCallResult<BinanceFuturesCancelOrder>> CancelOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceFuturesCancelOrder>(GetUrl(cancelOrderEndpoint, api, "1"), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);

        if (result)
        {
            MainClient.InvokeOrderCanceled(new OrderId
            {
                SourceObject = result.Data,
                Id = result.Data.Id.ToString(CultureInfo.InvariantCulture)
            });
        }
        return result;
    }
    #endregion

    #region Cancel All Open Orders
    public async Task<RestCallResult<BinanceFuturesCancelAllOrders>> CancelAllOrdersAsync(string symbol, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesCancelAllOrders>(GetUrl(cancelAllOrdersEndpoint, api, "1"), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Cancel Multiple Orders
    public async Task<RestCallResult<IEnumerable<CallResult<BinanceFuturesCancelOrder>>>> CancelMultipleOrdersAsync(string symbol, List<long> orderIdList = null, List<string> origClientOrderIdList = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderIdList == null && origClientOrderIdList == null)
            throw new ArgumentException("Either orderIdList or origClientOrderIdList must be sent");

        if (orderIdList?.Count > 10)
            throw new ArgumentException("orderIdList cannot contain more than 10 items");

        if (origClientOrderIdList?.Count > 10)
            throw new ArgumentException("origClientOrderIdList cannot contain more than 10 items");

        var parameters = new Dictionary<string, object>
        {
            { "symbol", symbol }
        };

        if (orderIdList != null)
            parameters.AddOptionalParameter("orderIdList", $"[{string.Join(",", orderIdList)}]");

        if (origClientOrderIdList != null)
            parameters.AddOptionalParameter("origClientOrderIdList", $"[{string.Join(",", origClientOrderIdList.Select(id => $"\"{id}\""))}]");

        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var response = await SendRequestInternal<IEnumerable<BinanceFuturesMultipleOrderCancelResult>>(GetUrl(cancelMultipleOrdersEndpoint, api, "1"), HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);

        if (!response.Success)
            return response.As<IEnumerable<CallResult<BinanceFuturesCancelOrder>>>(default);

        var result = new List<CallResult<BinanceFuturesCancelOrder>>();
        foreach (var item in response.Data)
        {
            result.Add(item.Code != 0
                ? new CallResult<BinanceFuturesCancelOrder>(new ServerError(item.Code, item.Message))
                : new CallResult<BinanceFuturesCancelOrder>(item));
        }

        return response.As<IEnumerable<CallResult<BinanceFuturesCancelOrder>>>(result);
    }
    #endregion

    #region Auto-Cancel All Open Orders
    public async Task<RestCallResult<BinanceFuturesCountDownResult>> CancelAllOrdersAfterTimeoutAsync(string symbol, TimeSpan countDownTime, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "countdownTime", (int)countDownTime.TotalMilliseconds }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesCountDownResult>(GetUrl(countDownCancelAllEndpoint, api, "1"), HttpMethod.Post, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Current Open Order
    public async Task<RestCallResult<BinanceFuturesOrder>> GetOpenOrderAsync(string symbol, long? orderId = null, string origClientOrderId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceFuturesOrder>(GetUrl(openOrderEndpoint, api, "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    #endregion

    #region Current All Open Orders
    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOpenOrdersAsync(string symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);

        return await SendRequestInternal<IEnumerable<BinanceFuturesOrder>>(GetUrl(openOrdersEndpoint, api, "1"), HttpMethod.Get, ct, parameters, true, weight: symbol == null ? 40 : 1).ConfigureAwait(false);
    }
    #endregion

    #region All Orders
    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
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

        return await SendRequestInternal<IEnumerable<BinanceFuturesOrder>>(GetUrl(allOrdersEndpoint, api, "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
    }
    #endregion

    #region Account Trade List
    public async Task<RestCallResult<IEnumerable<BinanceFuturesUsdtTrade>>> GetUserTradesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceFuturesUsdtTrade>>(GetUrl(myFuturesTradesEndpoint, api, "1"), HttpMethod.Get, ct, parameters, true, weight: 5).ConfigureAwait(false);
    }
    #endregion

    #region User's Force Orders
    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrder>>> GetForcedOrdersAsync(string symbol = null, AutoCloseType? closeType = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("autoCloseType", closeType.HasValue ? JsonConvert.SerializeObject(closeType, new AutoCloseTypeConverter(false)) : null);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());

        return await SendRequestInternal<IEnumerable<BinanceFuturesOrder>>(GetUrl(forceOrdersEndpoint, api, "1"), HttpMethod.Get, ct, parameters, true, weight: symbol == null ? 50 : 20).ConfigureAwait(false);
    }
    #endregion
}