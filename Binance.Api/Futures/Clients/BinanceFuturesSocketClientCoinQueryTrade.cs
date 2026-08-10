namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientCoin
{
    internal const string PlaceOrderPath = "ws-dapi/v1";
    internal const string PlaceOrderMethod = "order.place";
    internal const int PlaceOrderIpWeight = 0;
    internal const string ModifyOrderPath = "ws-dapi/v1";
    internal const string ModifyOrderMethod = "order.modify";
    internal const int ModifyOrderIpWeight = 1;
    internal const string CancelOrderPath = "ws-dapi/v1";
    internal const string CancelOrderMethod = "order.cancel";
    internal const int CancelOrderIpWeight = 1;
    internal const string GetOrderPath = "ws-dapi/v1";
    internal const string GetOrderMethod = "order.status";
    internal const int GetOrderIpWeight = 1;
    internal const string PositionQueryPath = "ws-dapi/v1";
    internal const string GetPositionsMethod = "account.position";
    internal const int PositionQueryIpWeight = 5;

    internal Task<BinanceTradeRuleResult> CheckTradingRulesAsync(
        string symbol,
        BinanceFuturesOrderType type,
        decimal? quantity,
        decimal? quoteQuantity,
        decimal? price,
        decimal? stopPrice,
        CancellationToken ct)
    {
        var options = SocketOptions.CoinFuturesOptions;
        return ((BinanceFuturesRestClientCoin)__.RestApiClient.CoinFutures).CheckTradingRulesAsync(
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

    public async Task<CallResult<BinanceFuturesCoinSocketOrderAcknowledgement>> PlaceOrderAsync(
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
        long? receiveWindow = null,
        CancellationToken ct = default)
    {
        var normalizedReceiveWindow = __.ReceiveWindow(receiveWindow);
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
            normalizedReceiveWindow);

        var rulesCheck = await CheckTradingRulesAsync(symbol, type, quantity, null, price, null, ct).ConfigureAwait(false);
        if (!rulesCheck.Passed)
        {
            Logger.Log(LogLevel.Warning, rulesCheck.ErrorMessage!);
            return new RestCallResult<BinanceFuturesCoinSocketOrderAcknowledgement>(new ArgumentError(rulesCheck.ErrorMessage!));
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
            normalizedReceiveWindow);

        return await RequestAsync<BinanceFuturesCoinSocketOrderAcknowledgement>(PlaceOrderPath, PlaceOrderMethod, parameters, true, true, weight: PlaceOrderIpWeight, ct: ct).ConfigureAwait(false);
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
            receiveWindow);

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
        long? receiveWindow)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol cannot be empty", nameof(symbol));
        if (side is not BinanceOrderSide.Buy and not BinanceOrderSide.Sell)
            throw new ArgumentOutOfRangeException(nameof(side), side, "Unsupported order side");
        if (type is not BinanceFuturesOrderType.Limit and not BinanceFuturesOrderType.Market)
            throw new ArgumentOutOfRangeException(nameof(type), type, "COIN-M order.place supports only Limit and Market orders; use the REST Algo Order API for conditional orders");
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
            and not BinanceTimeInForce.GoodTillCrossing)
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
        if (receiveWindow > 60_000)
            throw new ArgumentOutOfRangeException(nameof(receiveWindow), receiveWindow, "receiveWindow cannot exceed 60000 milliseconds");

        if (type == BinanceFuturesOrderType.Limit)
        {
            if (!timeInForce.HasValue)
                throw new ArgumentException("timeInForce is required for Limit orders", nameof(timeInForce));
            if (price.HasValue == priceMatch.HasValue)
                throw new ArgumentException("Exactly one of price or priceMatch must be sent for Limit orders");
        }
        else if (price.HasValue || priceMatch.HasValue || timeInForce.HasValue)
        {
            throw new ArgumentException("Market orders cannot include price, priceMatch, or timeInForce");
        }
    }

    public Task<CallResult<BinanceFuturesCoinSocketOrderAcknowledgement>> ModifyOrderAsync(
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

        return RequestAsync<BinanceFuturesCoinSocketOrderAcknowledgement>(ModifyOrderPath, ModifyOrderMethod, parameters, true, true, weight: ModifyOrderIpWeight, ct: ct);
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
        if (receiveWindow > 60_000)
            throw new ArgumentOutOfRangeException(nameof(receiveWindow), receiveWindow, "receiveWindow cannot exceed 60000 milliseconds");

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

    public Task<CallResult<BinanceFuturesCoinSocketOrderAcknowledgement>> CancelOrderAsync(
        string symbol,
        long? orderId = null,
        string? origClientOrderId = null,
        long? receiveWindow = null,
        CancellationToken ct = default)
        => RequestAsync<BinanceFuturesCoinSocketOrderAcknowledgement>(
            CancelOrderPath,
            CancelOrderMethod,
            CreateOrderIdentityParameters(symbol, orderId, origClientOrderId, __.ReceiveWindow(receiveWindow)),
            true,
            true,
            weight: CancelOrderIpWeight,
            ct: ct);

    public Task<CallResult<BinanceFuturesCoinSocketOrder>> GetOrderAsync(
        string symbol,
        long? orderId = null,
        string? origClientOrderId = null,
        long? receiveWindow = null,
        CancellationToken ct = default)
        => RequestAsync<BinanceFuturesCoinSocketOrder>(
            GetOrderPath,
            GetOrderMethod,
            CreateOrderIdentityParameters(symbol, orderId, origClientOrderId, __.ReceiveWindow(receiveWindow)),
            true,
            true,
            weight: GetOrderIpWeight,
            ct: ct);

    internal static ParameterCollection CreateOrderIdentityParameters(
        string symbol,
        long? orderId,
        string? origClientOrderId,
        long? receiveWindow)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Symbol is required", nameof(symbol));
        if (origClientOrderId is not null && string.IsNullOrWhiteSpace(origClientOrderId))
            throw new ArgumentException("Original client order id cannot be empty", nameof(origClientOrderId));
        if (!orderId.HasValue && origClientOrderId is null)
            throw new ArgumentException("Either orderId or origClientOrderId must be sent");

        var parameters = new ParameterCollection
        {
            { "symbol", symbol }
        };
        parameters.AddOptional("orderId", orderId);
        parameters.AddOptional("origClientOrderId", origClientOrderId);
        parameters.AddOptional("recvWindow", ValidateQueryReceiveWindow(receiveWindow));

        return parameters;
    }

    public Task<CallResult<List<BinanceFuturesCoinPosition>>> GetPositionsAsync(
        long? receiveWindow = null,
        string? marginAsset = null,
        string? pair = null,
        CancellationToken ct = default)
        => RequestAsync<List<BinanceFuturesCoinPosition>>(
            PositionQueryPath,
            GetPositionsMethod,
            CreatePositionQueryParameters(marginAsset, pair, __.ReceiveWindow(receiveWindow)),
            true,
            true,
            weight: PositionQueryIpWeight,
            ct: ct);

    internal static ParameterCollection CreatePositionQueryParameters(
        string? marginAsset,
        string? pair,
        long? receiveWindow)
    {
        if (marginAsset is not null && string.IsNullOrWhiteSpace(marginAsset))
            throw new ArgumentException("Margin asset cannot be empty", nameof(marginAsset));
        if (pair is not null && string.IsNullOrWhiteSpace(pair))
            throw new ArgumentException("Pair cannot be empty", nameof(pair));

        var parameters = new ParameterCollection();
        parameters.AddOptional("marginAsset", marginAsset);
        parameters.AddOptional("pair", pair);
        parameters.AddOptional("recvWindow", ValidateQueryReceiveWindow(receiveWindow));

        return parameters;
    }
}
