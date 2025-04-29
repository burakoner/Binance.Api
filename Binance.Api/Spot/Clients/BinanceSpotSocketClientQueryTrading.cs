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
        int? strategyType = null,
        CancellationToken ct = default)
    {
        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        // Check trade rules
        var rulesCheck = await CheckTradeRulesAsync(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            _logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new CallResult<BinanceSpotOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdSpot, 36, SocketOptions.AllowAppendingClientOrderId);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("quoteOrderQty", quoteQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("price", price?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("stopPrice", stopPrice?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("icebergQty", icebergQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("newClientOrderId", clientOrderId);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("newOrderRespjsoType", orderResponseType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptional("trailingDelta", trailingDelta);
        parameters.AddOptional("strategyId", strategyId);
        parameters.AddOptional("strategyType", strategyType);

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
        int? strategyType = null,
        bool? computeFeeRates = null,
        CancellationToken ct = default)
    {
        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradeRulesAsync(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
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

        return await RequestAsync<BinanceSpotOrderTest>("ws-api/v3", $"order.test", parameters, true, true, ct: ct).ConfigureAwait(false);
    }

    public Task<CallResult<BinanceSpotOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);

        return RequestAsync<BinanceSpotOrder>("ws-api/v3", $"order.status", parameters, true, true, weight: 4, ct: ct);
    }

    public Task<CallResult<BinanceSpotOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, string? newClientOrderId = null, BinanceSpotOrderCancelRestriction? cancelRestriction = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);

        return RequestAsync<BinanceSpotOrder>("ws-api/v3", $"order.cancel", parameters, true, true, ct: ct);
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
        int? strategyType = null,
        CancellationToken ct = default)
    {
        if (cancelOrderId == null && cancelClientOrderId == null || cancelOrderId != null && cancelClientOrderId != null)
            throw new ArgumentException("1 of either should be specified, cancelOrderId or cancelClientOrderId");

        if (quoteQuantity != null && type != BinanceSpotOrderType.Market)
            throw new ArgumentException("quoteQuantity is only valid for market orders");

        if (quantity == null && quoteQuantity == null || quantity != null && quoteQuantity != null)
            throw new ArgumentException("1 of either should be specified, quantity or quoteOrderQuantity");

        var rulesCheck = await CheckTradeRulesAsync(symbol, quantity, quoteQuantity, price, stopPrice, type, ct).ConfigureAwait(false);
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

        return await RequestAsync<BinanceSpotReplaceOrderResult>("ws-api/v3", $"order.cancelReplace", parameters, true, true, ct: ct).ConfigureAwait(false);
    }

    // TODO: Order Amend Keep Priority (TRADE)

    public Task<CallResult<IEnumerable<BinanceSpotOrder>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);

        return RequestAsync<IEnumerable<BinanceSpotOrder>>("ws-api/v3", $"openOrders.status", parameters, true, true, weight: symbol == null ? 80 : 6, ct: ct);
    }

    public Task<CallResult<IEnumerable<BinanceSpotOrder>>> CancelAllOrdersAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);

        return RequestAsync<IEnumerable<BinanceSpotOrder>>("ws-api/v3", $"openOrders.cancelAll", parameters, true, true, ct: ct);
    }

    // TODO: Place new order using SOR (TRADE)
}