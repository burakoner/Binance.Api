﻿namespace Binance.Api.PortfolioMargin;

internal partial class BinancePortfolioMarginRestClientCrossMargin
{
    public Task<RestCallResult<BinancePortfolioMarginCrossOrderPlaced>> PlaceOrderAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginTransactionId>> BorrowAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginTransactionId>> RepayAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginCrossOco>> PlaceOcoOrderAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginCrossOrderCanceled>> CancelOrderAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginCrossOco>> CancelOcoOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<List<BinancePortfolioMarginCrossOrderCanceled>>> CancelOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinanceRowsResult<BinancePortfolioMarginCrossForceOrder>>> GetForceOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginCrossOrderQuery>> GetOrderAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<List<BinancePortfolioMarginCrossOrderQuery>>> GetOpenOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<List<BinancePortfolioMarginCrossOrderQuery>>> GetOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginCrossOco>> GetOcoOrderAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<List<BinancePortfolioMarginCrossOco>>> GetOcoOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<List<BinancePortfolioMarginCrossOco>>> GetOpenOcoOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<List<BinancePortfolioMarginCrossTrade>>> GetTradesAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginCrossRepay>> RepayDeptAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    /*
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
        int? strategyType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
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
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

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
        int? strategyType = null,
        int? receiveWindow = null,
        bool? computeFeeRates = null,
        CancellationToken ct = default)
    {
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
        parameters.AddOptional("computeCommissionRates", computeFeeRates?.ToString(BinanceConstants.CI).ToLowerInvariant());
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var weight = computeFeeRates == true ? 20 : 1;
        return await RequestAsync<BinanceSpotOrderTest>(GetUrl(api, v3, "order/test"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: weight).ConfigureAwait(false);
    }

    public Task<RestCallResult<BinanceSpotOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSpotOrder>(GetUrl(api, v3, "order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 4);
    }

    public async Task<RestCallResult<BinanceSpotOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, BinanceSpotOrderCancelRestriction? cancelRestriction = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptionalEnum("cancelRestrictions", cancelRestriction);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceSpotOrder>(GetUrl(api, v3, "order"), HttpMethod.Delete, ct, true, bodyParameters: parameters).ConfigureAwait(false);
        if (result) InvokeOrderCanceled(result.Data.Id);
        return result;
    }
    
    public async Task<RestCallResult<List<BinanceSpotOrder>>> CancelOrdersAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

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
        int? strategyType = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (cancelOrderId == null && cancelClientOrderId == null || cancelOrderId != null && cancelClientOrderId != null)
            throw new ArgumentException("1 of either should be specified, cancelOrderId or cancelClientOrderId");

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
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

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

    public async Task<RestCallResult<List<BinanceSpotOrder>>> GetOpenOrdersAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return await RequestAsync<List<BinanceSpotOrder>>(GetUrl(api, v3, "openOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: symbol == null ? 40 : 3).ConfigureAwait(false);
    }

    public Task<RestCallResult<List<BinanceSpotOrder>>> GetOrdersAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("startTime", startTime?.ConvertToMilliseconds());
        parameters.AddOptional("endTime", endTime?.ConvertToMilliseconds());
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceSpotOrder>>(GetUrl(api, v3, "allOrders"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }
    */
}