namespace Binance.Api.Spot;

internal partial class BinanceSpotSocketClient
{
    public async Task<CallResult<BinanceSpotOrder>> PlaceOrderAsync(
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
        long? strategyType = null,
        BinanceSpotPegPriceType? pegPriceType = null,
        int? pegOffsetValue = null,
        BinanceSpotPegOffsetType? pegOffsetType = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.StrategyType(strategyType);
        BinanceSpotTradeValidation.Peg(type, pegPriceType, pegOffsetValue, pegOffsetType);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));
        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        // Check trade rules
        var rulesCheck = await CheckTradingRulesAsync(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            _logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new CallResult<BinanceSpotOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;

        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, SocketOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity);
        parameters.AddOptional("quoteOrderQty", quoteQuantity);
        parameters.AddOptional("price", price);
        parameters.AddOptional("stopPrice", stopPrice);
        parameters.AddOptional("icebergQty", icebergQuantity);
        parameters.AddOptional("newClientOrderId", clientOrderId);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptionalEnum("pegPriceType", pegPriceType);
        parameters.AddOptional("pegOffsetValue", pegOffsetValue);
        parameters.AddOptionalEnum("pegOffsetType", pegOffsetType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return await RequestAsync<BinanceSpotOrder>("ws-api/v3", $"order.place", parameters, true, true, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<BinanceSpotOrderTest>> PlaceTestOrderAsync(
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
        long? strategyType = null,
        BinanceSpotPegPriceType? pegPriceType = null,
        int? pegOffsetValue = null,
        BinanceSpotPegOffsetType? pegOffsetType = null,
        decimal? receiveWindow = null,
        bool? computeFeeRates = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.StrategyType(strategyType);
        BinanceSpotTradeValidation.Peg(type, pegPriceType, pegOffsetValue, pegOffsetType);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));
        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradingRulesAsync(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new CallResult<BinanceSpotOrderTest>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;
        stopPrice = rulesCheck.StopPrice;
        quoteQuantity = rulesCheck.QuoteQuantity;

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity);
        parameters.AddOptional("quoteOrderQty", quoteQuantity);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("price", price);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptional("stopPrice", stopPrice);
        parameters.AddOptional("icebergQty", icebergQuantity);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptionalEnum("pegPriceType", pegPriceType);
        parameters.AddOptional("pegOffsetValue", pegOffsetValue);
        parameters.AddOptionalEnum("pegOffsetType", pegOffsetType);
        parameters.AddOptional("computeCommissionRates", computeFeeRates);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return await RequestAsync<BinanceSpotOrderTest>("ws-api/v3", $"order.test", parameters, true, true, weight: computeFeeRates == true ? 20 : 1, ct: ct).ConfigureAwait(false);
    }

    public Task<CallResult<BinanceSpotOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<BinanceSpotOrder>("ws-api/v3", $"order.status", parameters, true, true, weight: 4, ct: ct);
    }

    public Task<CallResult<BinanceSpotOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, BinanceSpotOrderCancelRestriction? cancelRestriction = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.OrderIdentifiers(orderId, origClientOrderId);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));

        var parameters = CreateCancelOrderParameters(symbol, orderId, origClientOrderId, newClientOrderId, cancelRestriction, _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSpotOrder>("ws-api/v3", $"order.cancel", parameters, true, true, ct: ct);
    }

    internal static ParameterCollection CreateCancelOrderParameters(
        string symbol,
        long? orderId,
        string? originalClientOrderId,
        string? newClientOrderId,
        BinanceSpotOrderCancelRestriction? cancelRestriction,
        decimal? receiveWindow)
    {
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", originalClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptionalEnum("cancelRestrictions", cancelRestriction);
        parameters.AddOptional("recvWindow", receiveWindow);
        return parameters;
    }

    public async Task<CallResult<BinanceSpotReplaceOrderResult>> ReplaceOrderAsync(
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
        long? strategyType = null,
        BinanceSpotOrderRateLimitExceededMode? orderRateLimitExceededMode = null,
        BinanceSpotPegPriceType? pegPriceType = null,
        int? pegOffsetValue = null,
        BinanceSpotPegOffsetType? pegOffsetType = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.OrderIdentifiers(cancelOrderId, cancelClientOrderId);
        BinanceSpotTradeValidation.StrategyType(strategyType);
        BinanceSpotTradeValidation.Peg(type, pegPriceType, pegOffsetValue, pegOffsetType);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));

        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradingRulesAsync(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new CallResult<BinanceSpotReplaceOrderResult>(new ArgumentError(rulesCheck.ErrorMessage!));
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
        parameters.AddOptionalEnum("orderRateLimitExceededMode", orderRateLimitExceededMode);
        parameters.AddOptionalEnum("pegPriceType", pegPriceType);
        parameters.AddOptional("pegOffsetValue", pegOffsetValue);
        parameters.AddOptionalEnum("pegOffsetType", pegOffsetType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return await RequestAsync<BinanceSpotReplaceOrderResult>("ws-api/v3", $"order.cancelReplace", parameters, true, true, ct: ct).ConfigureAwait(false);
    }

    public Task<CallResult<BinanceSpotOrderAmendResult>> AmendOrderAsync(
        string symbol,
        decimal newQuantity,
        long? orderId = null,
        string? originalClientOrderId = null,
        string? newClientOrderId = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.Amend(newQuantity, orderId, originalClientOrderId);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));

        var parameters = new ParameterCollection
        {
            { "symbol", symbol },
            { "newQty", newQuantity }
        };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", originalClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSpotOrderAmendResult>("ws-api/v3", "order.amend.keepPriority", parameters, true, true, weight: 4, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotOrder>>> GetOpenOrdersAsync(string? symbol = null, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        if (symbol != null)
            symbol.ValidateBinanceSymbol();
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", BinanceSpotAccountValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow)));

        return RequestAsync<List<BinanceSpotOrder>>("ws-api/v3", $"openOrders.status", parameters, true, true, weight: symbol == null ? 80 : 6, ct: ct);
    }

    public Task<CallResult<List<BinanceSpotOrder>>> CancelOrdersAsync(string symbol, decimal? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceSpotOrder>>("ws-api/v3", $"openOrders.cancelAll", parameters, true, true, ct: ct);
    }

    public async Task<CallResult<List<BinanceSpotOrder>>> PlaceSorOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceSpotOrderType type,
        decimal quantity,
        decimal? price = null,
        string? newClientOrderId = null,
        BinanceTimeInForce? timeInForce = null,
        BinanceOrderResponseType? orderResponseType = null,
        decimal? icebergQuantity = null,
        long? strategyId = null,
        long? strategyType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.SmartOrderRouting(type, quantity, strategyType);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));

        var rulesCheck = await CheckTradingRulesAsync(symbol, quantity, null, price, null, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new CallResult<List<BinanceSpotOrder>>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity!.Value;
        price = rulesCheck.Price;
        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, SocketOptions.AllowAppendingClientOrderId);
        var parameters = CreateSorOrderParameters(symbol, side, type, quantity, price, clientOrderId, timeInForce, orderResponseType, icebergQuantity, strategyId, strategyType, selfTradePreventionMode, receiveWindow, null);
        return await RequestAsync<List<BinanceSpotOrder>>("ws-api/v3", "sor.order.place", parameters, true, true, ct: ct).ConfigureAwait(false);
    }

    public async Task<CallResult<BinanceSpotOrderTest>> PlaceSorTestOrderAsync(
        string symbol,
        BinanceOrderSide side,
        BinanceSpotOrderType type,
        decimal quantity,
        decimal? price = null,
        string? newClientOrderId = null,
        BinanceTimeInForce? timeInForce = null,
        BinanceOrderResponseType? orderResponseType = null,
        decimal? icebergQuantity = null,
        long? strategyId = null,
        long? strategyType = null,
        BinanceSelfTradePreventionMode? selfTradePreventionMode = null,
        decimal? receiveWindow = null,
        bool? computeFeeRates = null,
        CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.SmartOrderRouting(type, quantity, strategyType);
        BinanceSpotTradeValidation.ReceiveWindow(_.ReceiveWindow(receiveWindow));

        var rulesCheck = await CheckTradingRulesAsync(symbol, quantity, null, price, null, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new CallResult<BinanceSpotOrderTest>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity!.Value;
        price = rulesCheck.Price;
        var parameters = CreateSorOrderParameters(symbol, side, type, quantity, price, newClientOrderId, timeInForce, orderResponseType, icebergQuantity, strategyId, strategyType, selfTradePreventionMode, receiveWindow, computeFeeRates);
        return await RequestAsync<BinanceSpotOrderTest>("ws-api/v3", "sor.order.test", parameters, true, true, weight: computeFeeRates == true ? 20 : 1, ct: ct).ConfigureAwait(false);
    }

    private ParameterCollection CreateSorOrderParameters(
        string symbol,
        BinanceOrderSide side,
        BinanceSpotOrderType type,
        decimal quantity,
        decimal? price,
        string? newClientOrderId,
        BinanceTimeInForce? timeInForce,
        BinanceOrderResponseType? orderResponseType,
        decimal? icebergQuantity,
        long? strategyId,
        long? strategyType,
        BinanceSelfTradePreventionMode? selfTradePreventionMode,
        decimal? receiveWindow,
        bool? computeFeeRates)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddParameter("quantity", quantity);
        parameters.AddOptional("price", price);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptional("icebergQty", icebergQuantity);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));
        parameters.AddOptional("computeCommissionRates", computeFeeRates);
        return parameters;
    }
}
