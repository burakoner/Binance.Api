using ApiSharp;
using Binance.Net.Objects.Models.Futures;
using System.ComponentModel;

namespace Binance.Api.Futures;

internal partial class BinanceRestApiClientFuturesUsd
{
    #region New Order

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceUsdFuturesOrder>> PlaceOrderAsync(
        string symbol,
        Enums.OrderSide side,
        FuturesOrderType type,
        decimal? quantity,
        decimal? price = null,
        Enums.PositionSide? positionSide = null,
        TimeInForce? timeInForce = null,
        bool? reduceOnly = null,
        string? newClientOrderId = null,
        decimal? stopPrice = null,
        decimal? activationPrice = null,
        decimal? callbackRate = null,
        WorkingType? workingType = null,
        bool? closePosition = null,
        OrderResponseType? orderResponseType = null,
        bool? priceProtect = null,
        PriceMatch? priceMatch = null,
        SelfTradePreventionMode? selfTradePreventionMode = null,
        DateTime? goodTillDate = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (closePosition == true && positionSide != null)
        {
            if (positionSide == Enums.PositionSide.Short && side == Enums.OrderSide.Sell)
                throw new ArgumentException("Can't close short position with order side sell");
            if (positionSide == Enums.PositionSide.Long && side == Enums.OrderSide.Buy)
                throw new ArgumentException("Can't close long position with order side buy");
        }

        if (orderResponseType == OrderResponseType.Full)
            throw new ArgumentException("OrderResponseType.Full is not supported in Futures");

        var rulesCheck = await _baseClient.CheckTradeRules(symbol, quantity, null, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            _logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new WebCallResult<BinanceUsdFuturesOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;

        var clientOrderId = LibraryHelpers.ApplyBrokerId(newClientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection()
        {
            { "symbol", symbol }
        };
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptionalParameter("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("newClientOrderId", clientOrderId);
        parameters.AddOptionalParameter("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptionalParameter("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("activationPrice", activationPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("callbackRate", callbackRate?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("workingType", workingType);
        parameters.AddOptionalParameter("reduceOnly", reduceOnly?.ToString().ToLower());
        parameters.AddOptionalParameter("closePosition", closePosition?.ToString().ToLower());
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptionalParameter("priceProtect", priceProtect?.ToString().ToUpper());
        parameters.AddOptionalEnum("priceMatch", priceMatch);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptionalMilliseconds("goodTillDate", goodTillDate);

        var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/order", BinanceExchange.RateLimiter.FuturesRest, 0, true);
        var result = await _baseClient.SendAsync<BinanceUsdFuturesOrder>(request, parameters, ct).ConfigureAwait(false);
        if (result)
        {
            _baseClient.InvokeOrderPlaced(new OrderId
            {
                SourceObject = result.Data,
                Id = result.Data.Id.ToString(CultureInfo.InvariantCulture)
            });
        }
        return result;
    }

    #endregion

    #region Multiple New Orders

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<CallResult<BinanceUsdFuturesOrder>>>> PlaceMultipleOrdersAsync(
        IEnumerable<BinanceFuturesBatchOrder> orders,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (_baseClient.ApiOptions.TradeRulesBehaviour != TradeRulesBehaviour.None)
        {
            foreach (var order in orders)
            {
                var rulesCheck = await _baseClient.CheckTradeRules(order.Symbol, order.Quantity, null, order.Price, order.StopPrice, order.Type, ct).ConfigureAwait(false);
                if (!rulesCheck.Passed)
                {
                    _logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
                    return new WebCallResult<IEnumerable<CallResult<BinanceUsdFuturesOrder>>>(new ArgumentError(rulesCheck.ErrorMessage!));
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
            var clientOrderId = LibraryHelpers.ApplyBrokerId(order.NewClientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

            var orderParameters = new ParameterCollection()
            {
                { "symbol", order.Symbol },
                { "newOrderRespType", "RESULT" }
            };
            orderParameters.AddEnum("side", order.Side);
            orderParameters.AddEnum("type", order.Type);
            orderParameters.AddOptionalParameter("quantity", order.Quantity?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("newClientOrderId", clientOrderId);
            orderParameters.AddOptionalEnum("timeInForce", order.TimeInForce);
            orderParameters.AddOptionalEnum("positionSide", order.PositionSide);
            orderParameters.AddOptionalParameter("price", order.Price?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("stopPrice", order.StopPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("activationPrice", order.ActivationPrice?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("callbackRate", order.CallbackRate?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalEnum("workingType", order.WorkingType);
            orderParameters.AddOptionalParameter("reduceOnly", order.ReduceOnly?.ToString().ToLower());
            orderParameters.AddOptionalParameter("priceProtect", order.PriceProtect?.ToString().ToUpper());
            orderParameters.AddOptionalEnum("priceMatch", order.PriceMatch);
            orderParameters.AddOptionalEnum("selfTradePreventionMode", order.SelfTradePreventionMode);
            parameterOrders.Add(orderParameters);
            i++;
        }

        parameters.Add("batchOrders", JsonSerializer.Serialize(parameterOrders));
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/batchOrders", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        var response = await _baseClient.SendAsync<IEnumerable<BinanceUsdFuturesMultipleOrderPlaceResult>>(request, parameters, ct).ConfigureAwait(false);
        if (!response.Success)
            return response.As<IEnumerable<CallResult<BinanceUsdFuturesOrder>>>(default);

        var result = new List<CallResult<BinanceUsdFuturesOrder>>();
        foreach (var item in response.Data)
        {
            result.Add(item.Code != 0
                ? new CallResult<BinanceUsdFuturesOrder>(new ServerError(item.Code, item.Message))
                : new CallResult<BinanceUsdFuturesOrder>(item));
        }

        return response.As<IEnumerable<CallResult<BinanceUsdFuturesOrder>>>(result);
    }

    #endregion



    #region Edit Order

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceUsdFuturesOrder>> EditOrderAsync(string symbol, OrderSide side, decimal quantity, decimal? price = null, PriceMatch? priceMatch = null, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        if (origClientOrderId != null)
            origClientOrderId = LibraryHelpers.ApplyBrokerId(origClientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "side", EnumConverter.GetString(side) },
            { "quantity", quantity.ToString(CultureInfo.InvariantCulture) },
        };
        parameters.AddOptionalParameter("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("priceMatch", priceMatch);
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Put, "fapi/v1/order", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceUsdFuturesOrder>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Edit Multiple Orders

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<CallResult<BinanceUsdFuturesOrder>>>> EditMultipleOrdersAsync(
        IEnumerable<BinanceFuturesBatchEditOrder> orders,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        var parameterOrders = new List<Dictionary<string, object>>();
        int i = 0;
        foreach (var order in orders)
        {
            var clientOrderId = order.ClientOrderId;
            if (clientOrderId != null)
                clientOrderId = LibraryHelpers.ApplyBrokerId(clientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

            var orderParameters = new ParameterCollection()
            {
                { "symbol", order.Symbol },
                { "quantity", order.Quantity.ToString(CultureInfo.InvariantCulture) },
            };
            orderParameters.AddOptionalParameter("price", order.Price?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalEnum("priceMatch", order.PriceMatch);
            orderParameters.AddEnum("side", order.Side);
            orderParameters.AddOptionalParameter("orderId", order.OrderId?.ToString(CultureInfo.InvariantCulture));
            orderParameters.AddOptionalParameter("origClientOrderId", clientOrderId);
            parameterOrders.Add(orderParameters);
            i++;
        }

        parameters.Add("batchOrders", JsonSerializer.Serialize(parameterOrders));
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Put, "fapi/v1/batchOrders", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        var response = await _baseClient.SendAsync<IEnumerable<BinanceUsdFuturesMultipleOrderPlaceResult>>(request, parameters, ct).ConfigureAwait(false);
        if (!response.Success)
            return response.As<IEnumerable<CallResult<BinanceUsdFuturesOrder>>>(default);

        var result = new List<CallResult<BinanceUsdFuturesOrder>>();
        foreach (var item in response.Data)
        {
            result.Add(item.Code != 0
                ? new CallResult<BinanceUsdFuturesOrder>(new ServerError(item.Code, item.Message))
                : new CallResult<BinanceUsdFuturesOrder>(item));
        }

        return response.As<IEnumerable<CallResult<BinanceUsdFuturesOrder>>>(result);
    }

    #endregion


    #region Query Order Edit History

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesOrderEditHistory>>> GetOrderEditHistoryAsync(string symbol, long? orderId = null, string? clientOrderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (clientOrderId != null)
            clientOrderId = LibraryHelpers.ApplyBrokerId(clientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", clientOrderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/orderAmendment", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesOrderEditHistory>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Cancel Order

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceUsdFuturesOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        if (origClientOrderId != null)
            origClientOrderId = LibraryHelpers.ApplyBrokerId(origClientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Delete, "fapi/v1/order", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        var result = await _baseClient.SendAsync<BinanceUsdFuturesOrder>(request, parameters, ct).ConfigureAwait(false);
        if (result)
        {
            _baseClient.InvokeOrderCanceled(new OrderId
            {
                SourceObject = result.Data,
                Id = result.Data.Id.ToString(CultureInfo.InvariantCulture)
            });
        }
        return result;
    }

    #endregion


    #region Cancel Multiple Orders

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<CallResult<BinanceUsdFuturesOrder>>>> CancelMultipleOrdersAsync(string symbol, List<long>? orderIdList = null, List<string>? origClientOrderIdList = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderIdList == null && origClientOrderIdList == null)
            throw new ArgumentException("Either orderIdList or origClientOrderIdList must be sent");

        if (orderIdList?.Count > 10)
            throw new ArgumentException("orderIdList cannot contain more than 10 items");

        if (origClientOrderIdList?.Count > 10)
            throw new ArgumentException("origClientOrderIdList cannot contain more than 10 items");

        var convertClientOrderIdList = origClientOrderIdList?.Select(x => LibraryHelpers.ApplyBrokerId(x, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId));

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };

        if (orderIdList != null)
            parameters.AddOptionalParameter("orderIdList", $"[{string.Join(",", orderIdList)}]");

        if (origClientOrderIdList != null)
            parameters.AddOptionalParameter("origClientOrderIdList", $"[{string.Join(",", convertClientOrderIdList.Select(id => $"\"{id}\""))}]");

        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Delete, "fapi/v1/batchOrders", BinanceExchange.RateLimiter.FuturesRest, 10, true);
        var response = await _baseClient.SendAsync<IEnumerable<BinanceUsdFuturesMultipleOrderCancelResult>>(request, parameters, ct).ConfigureAwait(false);

        if (!response.Success)
            return response.As<IEnumerable<CallResult<BinanceUsdFuturesOrder>>>(default);

        var result = new List<CallResult<BinanceUsdFuturesOrder>>();
        foreach (var item in response.Data)
        {
            result.Add(item.Code != 0
                ? new CallResult<BinanceUsdFuturesOrder>(new ServerError(item.Code, item.Message))
                : new CallResult<BinanceUsdFuturesOrder>(item));
        }

        return response.As<IEnumerable<CallResult<BinanceUsdFuturesOrder>>>(result);
    }

    #endregion


    #region Cancel All Open Orders

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesCancelAllOrders>> CancelAllOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Delete, "fapi/v1/allOpenOrders", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceFuturesCancelAllOrders>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Auto-Cancel All Open Orders

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesCountDownResult>> CancelAllOrdersAfterTimeoutAsync(string symbol, TimeSpan countDownTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "countdownTime", (int)countDownTime.TotalMilliseconds }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/countdownCancelAll", BinanceExchange.RateLimiter.FuturesRest, 10, true);
        return await _baseClient.SendAsync<BinanceFuturesCountDownResult>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion


    #region Query Order

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceUsdFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        if (origClientOrderId != null)
            origClientOrderId = LibraryHelpers.ApplyBrokerId(origClientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/order", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceUsdFuturesOrder>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion


    #region All Orders

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceUsdFuturesOrder>>> GetOrdersAsync(string? symbol = null, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/allOrders", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceUsdFuturesOrder>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Current All Open Orders

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceUsdFuturesOrder>>> GetOpenOrdersAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptionalParameter("symbol", symbol);

        var weight = symbol == null ? 40 : 1;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/openOrders", BinanceExchange.RateLimiter.FuturesRest, weight, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceUsdFuturesOrder>>(request, parameters, ct, weight).ConfigureAwait(false);
    }

    #endregion


    #region Query Current Open Order

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceUsdFuturesOrder>> GetOpenOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        if (origClientOrderId != null)
            origClientOrderId = LibraryHelpers.ApplyBrokerId(origClientOrderId, BinanceExchange.ClientOrderIdFutures, 36, _baseClient.ClientOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/openOrder", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceUsdFuturesOrder>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region User's Force Orders

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceUsdFuturesOrder>>> GetForcedOrdersAsync(string? symbol = null, AutoCloseType? closeType = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalEnum("autoCloseType", closeType);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        var weight = symbol == null ? 50 : 20;
        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/forceOrders", BinanceExchange.RateLimiter.FuturesRest, weight, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceUsdFuturesOrder>>(request, parameters, ct, weight).ConfigureAwait(false);
    }

    #endregion

    #region Account Trade List

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesUsdtTrade>>> GetUserTradesAsync(string symbol, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, long? orderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/userTrades", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesUsdtTrade>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Change Margin Type

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesChangeMarginTypeResult>> ChangeMarginTypeAsync(string symbol, FuturesMarginType marginType, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
        };
        parameters.AddEnum("marginType", marginType);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/marginType", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceFuturesChangeMarginTypeResult>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion


    #region Change Position Mode

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceResult>> ModifyPositionModeAsync(bool dualPositionSide, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "dualSidePosition", dualPositionSide.ToString().ToLower() }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/positionSide/dual", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceResult>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Change Initial Leverage

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesInitialLeverageChangeResult>> ChangeInitialLeverageAsync(string symbol, int leverage, int? receiveWindow = null, CancellationToken ct = default)
    {
        leverage.ValidateIntBetween(nameof(leverage), 1, 125);
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "leverage", leverage }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/leverage", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceFuturesInitialLeverageChangeResult>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceResult>> SetMultiAssetsModeAsync(bool enabled, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "multiAssetsMargin", enabled.ToString() }
        };
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/multiAssetsMargin", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceResult>(request, parameters, ct).ConfigureAwait(false);
    }

    #region Modify Isolated Position Margin

    /// <inheritdoc />
    public async Task<RestCallResult<BinanceFuturesPositionMarginResult>> ModifyPositionMarginAsync(string symbol, decimal quantity, FuturesMarginChangeDirectionType type, PositionSide? positionSide = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
        };
        parameters.AddEnum("type", type);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Post, "fapi/v1/positionMargin", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<BinanceFuturesPositionMarginResult>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Position Information

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinancePositionDetailsUsdt>>> GetPositionInformationAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v2/positionRisk", BinanceExchange.RateLimiter.FuturesRest, 10, true);
        return await _baseClient.SendAsync<IEnumerable<BinancePositionDetailsUsdt>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion

    #region Get Positions

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinancePositionV3>>> GetPositionsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        var request = _definitions.GetOrCreate(HttpMethod.Get, "/fapi/v3/positionRisk", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        var result = await _baseClient.SendAsync<IEnumerable<BinancePositionV3>>(request, parameters, ct).ConfigureAwait(false);
        return result;
    }

    #endregion

    #region Position ADL Quantile Estimations

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesQuantileEstimation>>> GetPositionAdlQuantileEstimationAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        if (symbol == null)
        {
            var request1 = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/adlQuantile", BinanceExchange.RateLimiter.FuturesRest, 5, true);
            return await _baseClient.SendAsync<IEnumerable<BinanceFuturesQuantileEstimation>>(request1, parameters, ct).ConfigureAwait(false);
        }

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/adlQuantile", BinanceExchange.RateLimiter.FuturesRest, 5, true);
        var result = await _baseClient.SendAsync<BinanceFuturesQuantileEstimation>(request, parameters, ct).ConfigureAwait(false);
        if (!result)
            return result.As<IEnumerable<BinanceFuturesQuantileEstimation>>(null);

        return result.As<IEnumerable<BinanceFuturesQuantileEstimation>>(new[] { result.Data });
    }

    #endregion


    #region Get Postion Margin Change History

    /// <inheritdoc />
    public async Task<RestCallResult<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>> GetMarginChangeHistoryAsync(string symbol, FuturesMarginChangeDirectionType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/positionMargin/history", BinanceExchange.RateLimiter.FuturesRest, 1, true);
        return await _baseClient.SendAsync<IEnumerable<BinanceFuturesMarginChangeHistoryResult>>(request, parameters, ct).ConfigureAwait(false);
    }

    #endregion


    // TODO: Test Order(TRADE)



















}