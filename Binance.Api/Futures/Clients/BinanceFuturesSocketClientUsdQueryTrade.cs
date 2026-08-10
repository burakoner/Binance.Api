namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    internal const string PlaceOrderPath = "ws-fapi/v1";
    internal const string PlaceOrderMethod = "order.place";
    internal const int PlaceOrderIpWeight = 0;
    internal const string PlaceAlgoOrderPath = "ws-fapi/v1";
    internal const string PlaceAlgoOrderMethod = "algoOrder.place";
    internal const int PlaceAlgoOrderIpWeight = 0;
    internal const string CancelAlgoOrderPath = "ws-fapi/v1";
    internal const string CancelAlgoOrderMethod = "algoOrder.cancel";
    internal const int CancelAlgoOrderIpWeight = 1;
    internal const string PositionQueryPath = "ws-fapi/v1";
    internal const string GetPositionsV1Method = "account.position";
    internal const string GetPositionsV2Method = "v2/account.position";
    internal const int PositionQueryIpWeight = 5;
    internal const string ModifyOrderPath = "ws-fapi/v1";
    internal const string ModifyOrderMethod = "order.modify";
    internal const int ModifyOrderIpWeight = 0;

    internal Task<BinanceTradeRuleResult> CheckTradingRulesAsync(
        string symbol,
        BinanceFuturesOrderType type,
        decimal? quantity,
        decimal? quoteQuantity,
        decimal? price,
        decimal? stopPrice,
        CancellationToken ct)
    {
        var options = SocketOptions.UsdtFuturesOptions;
        return ((BinanceFuturesRestClientUsd)__.RestApiClient.UsdFutures).CheckTradingRulesAsync(
            symbol,
            type,
            quantity,
            quoteQuantity,
            price,
            stopPrice,
            options.TradeRulesBehavior,
            options.TradeRulesUpdateInterval,
            ct);
    }

    public async Task<CallResult<BinanceFuturesOrder>> PlaceOrderAsync(
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
        DateTime? goodTillDate = null,
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidatePlaceOrderParameters(
            symbol,
            side,
            type,
            quantity,
            price,
            newClientOrderId,
            positionSide,
            timeInForce,
            orderResponseType,
            selfTradePreventionMode,
            priceMatch,
            reduceOnly,
            goodTillDate);

        var rulesCheck = await CheckTradingRulesAsync(symbol, type, quantity, null, price, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceFuturesOrder>(new ArgumentError(rulesCheck.ErrorMessage!));
        }

        quantity = rulesCheck.Quantity;
        price = rulesCheck.Price;

        var clientOrderId = BinanceHelpers.ApplyBrokerId(newClientOrderId, BinanceConstants.ClientOrderIdFutures, 36, SocketOptions.AllowAppendingClientOrderId);
        var parameters = CreatePlaceOrderParameters(
            symbol,
            side,
            type,
            quantity,
            price,
            clientOrderId,
            positionSide,
            timeInForce,
            orderResponseType,
            selfTradePreventionMode,
            priceMatch,
            reduceOnly,
            goodTillDate,
            __.ReceiveWindow(receiveWindow));

        return await RequestAsync<BinanceFuturesOrder>(PlaceOrderPath, PlaceOrderMethod, parameters, true, true, weight: PlaceOrderIpWeight, ct: ct).ConfigureAwait(false);
    }

    internal static ParameterCollection CreatePlaceOrderParameters(
        string symbol,
        BinanceOrderSide side,
        BinanceFuturesOrderType type,
        decimal? quantity,
        decimal? price,
        string? newClientOrderId,
        BinancePositionSide? positionSide,
        BinanceTimeInForce? timeInForce,
        BinanceOrderResponseType? orderResponseType,
        BinanceSelfTradePreventionMode? selfTradePreventionMode,
        BinanceFuturesPriceMatch? priceMatch,
        bool? reduceOnly,
        DateTime? goodTillDate,
        long? receiveWindow)
    {
        ValidatePlaceOrderParameters(
            symbol,
            side,
            type,
            quantity,
            price,
            newClientOrderId,
            positionSide,
            timeInForce,
            orderResponseType,
            selfTradePreventionMode,
            priceMatch,
            reduceOnly,
            goodTillDate);

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptional("quantity", quantity);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        parameters.AddOptional("price", price);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptional("reduceOnly", reduceOnly?.ToString().ToLowerInvariant());
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptionalEnum("priceMatch", priceMatch);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptionalMilliseconds("goodTillDate", goodTillDate);
        parameters.AddOptional("recvWindow", receiveWindow);

        return parameters;
    }

    internal static void ValidatePlaceOrderParameters(
        string symbol,
        BinanceOrderSide side,
        BinanceFuturesOrderType type,
        decimal? quantity,
        decimal? price,
        string? newClientOrderId,
        BinancePositionSide? positionSide,
        BinanceTimeInForce? timeInForce,
        BinanceOrderResponseType? orderResponseType,
        BinanceSelfTradePreventionMode? selfTradePreventionMode,
        BinanceFuturesPriceMatch? priceMatch,
        bool? reduceOnly,
        DateTime? goodTillDate)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol cannot be empty", nameof(symbol));
        if (side is not BinanceOrderSide.Buy and not BinanceOrderSide.Sell)
            throw new ArgumentOutOfRangeException(nameof(side), side, "Unsupported order side");
        if (type is not BinanceFuturesOrderType.Limit and not BinanceFuturesOrderType.Market)
            throw new ArgumentOutOfRangeException(nameof(type), type, "USD-M order.place supports only Limit and Market orders; use PlaceAlgoOrderAsync for conditional orders");
        if (!quantity.HasValue || quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "quantity must be greater than zero");
        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), price, "price must be greater than zero when provided");
        if (newClientOrderId is not null && !System.Text.RegularExpressions.Regex.IsMatch(newClientOrderId, @"^[\.A-Z\:/a-z0-9_-]{1,36}$", System.Text.RegularExpressions.RegexOptions.CultureInvariant))
            throw new ArgumentException("newClientOrderId does not match the documented format", nameof(newClientOrderId));
        if (positionSide.HasValue && positionSide is not BinancePositionSide.Both and not BinancePositionSide.Long and not BinancePositionSide.Short)
            throw new ArgumentOutOfRangeException(nameof(positionSide), positionSide, "Unsupported position side");
        if (timeInForce.HasValue && timeInForce is not BinanceTimeInForce.GoodTillCanceled
            and not BinanceTimeInForce.ImmediateOrCancel
            and not BinanceTimeInForce.FillOrKill
            and not BinanceTimeInForce.GoodTillCrossing
            and not BinanceTimeInForce.GoodTillDate
            and not BinanceTimeInForce.RetailPriceImprovement)
            throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, "Unsupported time in force");
        if (orderResponseType.HasValue && orderResponseType is not BinanceOrderResponseType.Acknowledge and not BinanceOrderResponseType.Result)
            throw new ArgumentOutOfRangeException(nameof(orderResponseType), orderResponseType, "Only ACK and RESULT response types are supported");
        if (priceMatch.HasValue && (!Enum.IsDefined(typeof(BinanceFuturesPriceMatch), priceMatch.Value) || priceMatch == BinanceFuturesPriceMatch.None))
            throw new ArgumentOutOfRangeException(nameof(priceMatch), priceMatch, "Unsupported placement priceMatch value");
        if (selfTradePreventionMode.HasValue && selfTradePreventionMode is not BinanceSelfTradePreventionMode.None
            and not BinanceSelfTradePreventionMode.ExpireTaker
            and not BinanceSelfTradePreventionMode.ExpireMaker
            and not BinanceSelfTradePreventionMode.ExpireBoth)
            throw new ArgumentOutOfRangeException(nameof(selfTradePreventionMode), selfTradePreventionMode, "Unsupported self-trade prevention mode");
        if (reduceOnly.HasValue && positionSide is BinancePositionSide.Long or BinancePositionSide.Short)
            throw new ArgumentException("reduceOnly cannot be sent in Hedge Mode", nameof(reduceOnly));

        if (type == BinanceFuturesOrderType.Limit)
        {
            if (!timeInForce.HasValue)
                throw new ArgumentException("timeInForce is required for Limit orders", nameof(timeInForce));
            if (price.HasValue == priceMatch.HasValue)
                throw new ArgumentException("Exactly one of price or priceMatch must be sent for Limit orders");
        }
        else if (price.HasValue || priceMatch.HasValue || timeInForce.HasValue || goodTillDate.HasValue)
        {
            throw new ArgumentException("Market orders cannot include price, priceMatch, timeInForce, or goodTillDate");
        }

        if (timeInForce == BinanceTimeInForce.GoodTillDate && !goodTillDate.HasValue)
            throw new ArgumentException("goodTillDate is required when timeInForce is GTD", nameof(goodTillDate));
        if (goodTillDate.HasValue && timeInForce != BinanceTimeInForce.GoodTillDate)
            throw new ArgumentException("goodTillDate is only available when timeInForce is GTD", nameof(goodTillDate));
        if (goodTillDate.HasValue)
        {
            var goodTillDateMilliseconds = new DateTimeOffset(goodTillDate.Value.ToUniversalTime()).ToUnixTimeMilliseconds();
            if (goodTillDateMilliseconds <= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 600_000)
                throw new ArgumentOutOfRangeException(nameof(goodTillDate), goodTillDate, "goodTillDate must be more than 600 seconds in the future");
            if (goodTillDateMilliseconds >= 253_402_300_799_000L)
                throw new ArgumentOutOfRangeException(nameof(goodTillDate), goodTillDate, "goodTillDate exceeds the documented maximum");
        }
    }

    public Task<CallResult<BinanceFuturesAlgoOrderPlacementResult>> PlaceAlgoOrderAsync(
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
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        var parameters = CreatePlaceAlgoOrderParameters(
            symbol,
            side,
            type,
            positionSide,
            timeInForce,
            quantity,
            price,
            triggerPrice,
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
            goodTillDate,
            __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesAlgoOrderPlacementResult>(PlaceAlgoOrderPath, PlaceAlgoOrderMethod, parameters, true, true, weight: PlaceAlgoOrderIpWeight, ct: ct);
    }

    internal static ParameterCollection CreatePlaceAlgoOrderParameters(
        string symbol,
        BinanceOrderSide side,
        BinanceFuturesAlgoOrderType type,
        BinancePositionSide? positionSide,
        BinanceTimeInForce? timeInForce,
        decimal? quantity,
        decimal? price,
        decimal? triggerPrice,
        BinanceFuturesWorkingType? workingType,
        BinanceFuturesPriceMatch? priceMatch,
        bool? closePosition,
        bool? priceProtect,
        bool? reduceOnly,
        decimal? activatePrice,
        decimal? callbackRate,
        string? clientAlgoId,
        BinanceOrderResponseType? orderResponseType,
        BinanceSelfTradePreventionMode? selfTradePreventionMode,
        DateTime? goodTillDate,
        long? receiveWindow)
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

        if (timeInForce is BinanceTimeInForce.GoodTillCrossing or BinanceTimeInForce.RetailPriceImprovement)
            throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, "USD-M WebSocket Algo orders support IOC, GTC, FOK, and the separately documented GTD contract");

        var parameters = new ParameterCollection
        {
            { "algoType", "CONDITIONAL" },
            { "symbol", symbol }
        };
        parameters.AddEnum("side", side);
        parameters.AddEnum("type", type);
        parameters.AddOptionalEnum("positionSide", positionSide);
        parameters.AddOptionalEnum("timeInForce", timeInForce);
        parameters.AddOptional("quantity", quantity);
        parameters.AddOptional("price", price);
        parameters.AddOptional("triggerPrice", triggerPrice);
        parameters.AddOptionalEnum("workingType", workingType);
        parameters.AddOptionalEnum("priceMatch", priceMatch);
        parameters.AddOptional("closePosition", closePosition?.ToString().ToLowerInvariant());
        parameters.AddOptional("priceProtect", priceProtect?.ToString().ToLowerInvariant());
        parameters.AddOptional("reduceOnly", reduceOnly?.ToString().ToLowerInvariant());
        parameters.AddOptional("activatePrice", activatePrice);
        parameters.AddOptional("callbackRate", callbackRate);
        parameters.AddOptional("clientAlgoId", clientAlgoId);
        parameters.AddOptionalEnum("newOrderRespType", orderResponseType);
        parameters.AddOptionalEnum("selfTradePreventionMode", selfTradePreventionMode);
        parameters.AddOptionalMilliseconds("goodTillDate", goodTillDate);
        parameters.AddOptional("recvWindow", receiveWindow);

        return parameters;
    }

    public Task<CallResult<BinanceFuturesAlgoOrderCancellationResult>> CancelAlgoOrderAsync(long? algoId = null, string? clientAlgoId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = CreateCancelAlgoOrderParameters(algoId, clientAlgoId, __.ReceiveWindow(receiveWindow));
        return RequestAsync<BinanceFuturesAlgoOrderCancellationResult>(CancelAlgoOrderPath, CancelAlgoOrderMethod, parameters, true, true, weight: CancelAlgoOrderIpWeight, ct: ct);
    }

    internal static ParameterCollection CreateCancelAlgoOrderParameters(long? algoId, string? clientAlgoId, long? receiveWindow)
    {
        BinanceFuturesAlgoOrderValidation.ValidateCancellation(algoId, clientAlgoId);

        var parameters = new ParameterCollection();
        parameters.AddOptional("algoId", algoId);
        parameters.AddOptional("clientAlgoId", clientAlgoId);
        parameters.AddOptional("recvWindow", receiveWindow);

        return parameters;
    }

    public Task<CallResult<BinanceFuturesOrder>> ModifyOrderAsync(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        decimal price,
        long? orderId = null,
        string? origClientOrderId = null,
        BinanceFuturesPriceMatch? priceMatch = null,
        long? modifyId = null,
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        var parameters = CreateModifyOrderParameters(
            symbol,
            side,
            quantity,
            price,
            orderId,
            origClientOrderId,
            priceMatch,
            modifyId,
            __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>(ModifyOrderPath, ModifyOrderMethod, parameters, true, true, weight: ModifyOrderIpWeight, ct: ct);
    }

    internal static ParameterCollection CreateModifyOrderParameters(
        string symbol,
        BinanceOrderSide side,
        decimal quantity,
        decimal price,
        long? orderId,
        string? origClientOrderId,
        BinanceFuturesPriceMatch? priceMatch,
        long? modifyId,
        long? receiveWindow)
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
            { "quantity", quantity },
            { "price", price }
        };
        parameters.AddEnum("side", side);
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("modifyId", modifyId);
        parameters.AddOptional("recvWindow", receiveWindow);

        return parameters;
    }

    public Task<CallResult<BinanceFuturesOrder>> CancelOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(origClientOrderId))
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>("ws-fapi/v1", $"order.cancel", parameters, true, true, weight: 1, ct: ct);
    }

    public Task<CallResult<BinanceFuturesOrder>> GetOrderAsync(string symbol, long? orderId = null, string? origClientOrderId = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (orderId == null && origClientOrderId == null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("orderId", orderId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceFuturesOrder>("ws-fapi/v1", $"order.status", parameters, true, true, weight: 1, ct: ct);
    }

    public Task<CallResult<List<BinanceFuturesUsdtPosition>>> GetPositionsV1Async(string? symbol = null, long? receiveWindow = null, CancellationToken ct = default)
        => RequestAsync<List<BinanceFuturesUsdtPosition>>(
            PositionQueryPath,
            GetPositionsV1Method,
            CreatePositionQueryParameters(symbol, __.ReceiveWindow(receiveWindow)),
            true,
            true,
            weight: PositionQueryIpWeight,
            ct: ct);

    public Task<CallResult<List<BinanceFuturesPositionV3>>> GetPositionsAsync(string? symbol = null, long? receiveWindow = null, CancellationToken ct = default)
        => RequestAsync<List<BinanceFuturesPositionV3>>(
            PositionQueryPath,
            GetPositionsV2Method,
            CreatePositionQueryParameters(symbol, __.ReceiveWindow(receiveWindow)),
            true,
            true,
            weight: PositionQueryIpWeight,
            ct: ct);

    internal static ParameterCollection CreatePositionQueryParameters(string? symbol, long? receiveWindow)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", receiveWindow);

        return parameters;
    }

}
