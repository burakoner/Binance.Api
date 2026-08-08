namespace Binance.Api.Spot;

internal static class BinanceSpotOrderListRequestBuilder
{
    public static ParameterCollection Cancel(string symbol, long? orderListId, string? listClientOrderId, string? newClientOrderId, decimal? receiveWindow, bool stringifyDecimals)
    {
        symbol.ValidateBinanceSymbol();
        if (!orderListId.HasValue && string.IsNullOrEmpty(listClientOrderId))
            throw new ArgumentException("Either orderListId or listClientOrderId must be sent.");

        BinanceSpotTradeValidation.ReceiveWindow(receiveWindow);
        var parameters = new ParameterCollection { { "symbol", symbol } };
        parameters.AddOptional("orderListId", orderListId);
        parameters.AddOptional("listClientOrderId", listClientOrderId);
        parameters.AddOptional("newClientOrderId", newClientOrderId);
        AddOptionalDecimal(parameters, "recvWindow", receiveWindow, stringifyDecimals);
        return parameters;
    }

    public static ParameterCollection Oco(BinanceSpotOcoOrderListRequest request, Func<string?, string?> applyClientOrderId, decimal? receiveWindow, bool stringifyDecimals)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        ValidateCommon(request, receiveWindow);
        ValidateQuantity(request.Quantity, nameof(request.Quantity));
        ValidateOcoPair(request.AboveOrder, request.BelowOrder, true);

        var parameters = CreateCommon(request);
        parameters.AddEnum("side", request.Side);
        AddDecimal(parameters, "quantity", request.Quantity, stringifyDecimals);
        AddOcoLeg(parameters, "above", request.AboveOrder, applyClientOrderId, stringifyDecimals);
        AddOcoLeg(parameters, "below", request.BelowOrder, applyClientOrderId, stringifyDecimals);
        AddCommonOptions(parameters, request, receiveWindow, stringifyDecimals);
        return parameters;
    }

    public static ParameterCollection Opo(BinanceSpotOpoOrderListRequest request, Func<string?, string?> applyClientOrderId, decimal? receiveWindow, bool stringifyDecimals)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        ValidateCommon(request, receiveWindow);
        ValidateWorking(request.WorkingOrder);
        ValidatePending(request.PendingOrder);

        var parameters = CreateCommon(request);
        AddWorking(parameters, request.WorkingOrder, applyClientOrderId, stringifyDecimals);
        AddPending(parameters, request.PendingOrder, applyClientOrderId, stringifyDecimals);
        AddCommonOptions(parameters, request, receiveWindow, stringifyDecimals);
        return parameters;
    }

    public static ParameterCollection Opoco(BinanceSpotOpocoOrderListRequest request, Func<string?, string?> applyClientOrderId, decimal? receiveWindow, bool stringifyDecimals)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        ValidateCommon(request, receiveWindow);
        ValidateWorking(request.WorkingOrder);
        ValidateOcoLeg(request.PendingAboveOrder, false);
        if (request.PendingBelowOrder != null)
            ValidateOcoPair(request.PendingAboveOrder, request.PendingBelowOrder, true);

        var parameters = CreateCommon(request);
        AddWorking(parameters, request.WorkingOrder, applyClientOrderId, stringifyDecimals);
        parameters.AddEnum("pendingSide", request.PendingSide);
        AddOcoLeg(parameters, "pendingAbove", request.PendingAboveOrder, applyClientOrderId, stringifyDecimals);
        if (request.PendingBelowOrder != null)
            AddOcoLeg(parameters, "pendingBelow", request.PendingBelowOrder, applyClientOrderId, stringifyDecimals);
        AddCommonOptions(parameters, request, receiveWindow, stringifyDecimals);
        return parameters;
    }

    public static ParameterCollection Oto(BinanceSpotOtoOrderListRequest request, Func<string?, string?> applyClientOrderId, decimal? receiveWindow, bool stringifyDecimals)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        ValidateCommon(request, receiveWindow);
        ValidateWorking(request.WorkingOrder);
        ValidatePending(request.PendingOrder);
        ValidateQuantity(request.PendingQuantity, nameof(request.PendingQuantity));

        var parameters = CreateCommon(request);
        AddWorking(parameters, request.WorkingOrder, applyClientOrderId, stringifyDecimals);
        AddPending(parameters, request.PendingOrder, applyClientOrderId, stringifyDecimals);
        AddDecimal(parameters, "pendingQuantity", request.PendingQuantity, stringifyDecimals);
        AddCommonOptions(parameters, request, receiveWindow, stringifyDecimals);
        return parameters;
    }

    public static ParameterCollection Otoco(BinanceSpotOtocoOrderListRequest request, Func<string?, string?> applyClientOrderId, decimal? receiveWindow, bool stringifyDecimals)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        ValidateCommon(request, receiveWindow);
        ValidateWorking(request.WorkingOrder);
        ValidateQuantity(request.PendingQuantity, nameof(request.PendingQuantity));
        ValidateOcoLeg(request.PendingAboveOrder, false);
        if (request.PendingBelowOrder != null)
            ValidateOcoPair(request.PendingAboveOrder, request.PendingBelowOrder, true);

        var parameters = CreateCommon(request);
        AddWorking(parameters, request.WorkingOrder, applyClientOrderId, stringifyDecimals);
        parameters.AddEnum("pendingSide", request.PendingSide);
        AddDecimal(parameters, "pendingQuantity", request.PendingQuantity, stringifyDecimals);
        AddOcoLeg(parameters, "pendingAbove", request.PendingAboveOrder, applyClientOrderId, stringifyDecimals);
        if (request.PendingBelowOrder != null)
            AddOcoLeg(parameters, "pendingBelow", request.PendingBelowOrder, applyClientOrderId, stringifyDecimals);
        AddCommonOptions(parameters, request, receiveWindow, stringifyDecimals);
        return parameters;
    }

    private static ParameterCollection CreateCommon(BinanceSpotOrderListRequest request)
        => new() { { "symbol", request.Symbol } };

    private static void AddCommonOptions(ParameterCollection parameters, BinanceSpotOrderListRequest request, decimal? receiveWindow, bool stringifyDecimals)
    {
        parameters.AddOptional("listClientOrderId", request.ListClientOrderId);
        parameters.AddOptionalEnum("newOrderRespType", request.OrderResponseType);
        parameters.AddOptionalEnum("selfTradePreventionMode", request.SelfTradePreventionMode);
        AddOptionalDecimal(parameters, "recvWindow", receiveWindow, stringifyDecimals);
    }

    private static void AddWorking(ParameterCollection parameters, BinanceSpotOrderListWorkingOrderRequest order, Func<string?, string?> applyClientOrderId, bool stringifyDecimals)
    {
        parameters.AddEnum("workingType", order.Type);
        parameters.AddEnum("workingSide", order.Side);
        AddDecimal(parameters, "workingPrice", order.Price, stringifyDecimals);
        AddDecimal(parameters, "workingQuantity", order.Quantity, stringifyDecimals);
        parameters.AddOptional("workingClientOrderId", applyClientOrderId(order.ClientOrderId));
        AddOptionalDecimal(parameters, "workingIcebergQty", order.IcebergQuantity, stringifyDecimals);
        parameters.AddOptionalEnum("workingTimeInForce", order.TimeInForce);
        parameters.AddOptional("workingStrategyId", order.StrategyId);
        parameters.AddOptional("workingStrategyType", order.StrategyType);
        parameters.AddOptionalEnum("workingPegPriceType", order.PegPriceType);
        parameters.AddOptionalEnum("workingPegOffsetType", order.PegOffsetType);
        parameters.AddOptional("workingPegOffsetValue", order.PegOffsetValue);
    }

    private static void AddPending(ParameterCollection parameters, BinanceSpotOrderListPendingOrderRequest order, Func<string?, string?> applyClientOrderId, bool stringifyDecimals)
    {
        parameters.AddEnum("pendingType", order.Type);
        parameters.AddEnum("pendingSide", order.Side);
        parameters.AddOptional("pendingClientOrderId", applyClientOrderId(order.ClientOrderId));
        AddOptionalDecimal(parameters, "pendingPrice", order.Price, stringifyDecimals);
        AddOptionalDecimal(parameters, "pendingStopPrice", order.StopPrice, stringifyDecimals);
        AddOptionalDecimal(parameters, "pendingTrailingDelta", order.TrailingDelta, stringifyDecimals);
        AddOptionalDecimal(parameters, "pendingIcebergQty", order.IcebergQuantity, stringifyDecimals);
        parameters.AddOptionalEnum("pendingTimeInForce", order.TimeInForce);
        parameters.AddOptional("pendingStrategyId", order.StrategyId);
        parameters.AddOptional("pendingStrategyType", order.StrategyType);
        parameters.AddOptionalEnum("pendingPegPriceType", order.PegPriceType);
        parameters.AddOptionalEnum("pendingPegOffsetType", order.PegOffsetType);
        parameters.AddOptional("pendingPegOffsetValue", order.PegOffsetValue);
    }

    private static void AddOcoLeg(ParameterCollection parameters, string prefix, BinanceSpotOrderListOcoLegRequest order, Func<string?, string?> applyClientOrderId, bool stringifyDecimals)
    {
        parameters.AddEnum(prefix + "Type", order.Type);
        parameters.AddOptional(prefix + "ClientOrderId", applyClientOrderId(order.ClientOrderId));
        AddOptionalDecimal(parameters, prefix + "IcebergQty", order.IcebergQuantity, stringifyDecimals);
        AddOptionalDecimal(parameters, prefix + "Price", order.Price, stringifyDecimals);
        AddOptionalDecimal(parameters, prefix + "StopPrice", order.StopPrice, stringifyDecimals);
        AddOptionalDecimal(parameters, prefix + "TrailingDelta", order.TrailingDelta, stringifyDecimals);
        parameters.AddOptionalEnum(prefix + "TimeInForce", order.TimeInForce);
        parameters.AddOptional(prefix + "StrategyId", order.StrategyId);
        parameters.AddOptional(prefix + "StrategyType", order.StrategyType);
        parameters.AddOptionalEnum(prefix + "PegPriceType", order.PegPriceType);
        parameters.AddOptionalEnum(prefix + "PegOffsetType", order.PegOffsetType);
        parameters.AddOptional(prefix + "PegOffsetValue", order.PegOffsetValue);
    }

    private static void ValidateCommon(BinanceSpotOrderListRequest request, decimal? receiveWindow)
    {
        request.Symbol.ValidateBinanceSymbol();
        BinanceSpotTradeValidation.ReceiveWindow(receiveWindow);
    }

    private static void ValidateWorking(BinanceSpotOrderListWorkingOrderRequest order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));
        if (order.Type is not BinanceSpotOrderType.Limit and not BinanceSpotOrderType.LimitMaker)
            throw new ArgumentException("A working order must be LIMIT or LIMIT_MAKER.", nameof(order));
        ValidateQuantity(order.Price, nameof(order.Price));
        ValidateQuantity(order.Quantity, nameof(order.Quantity));
        ValidateOptionalQuantity(order.IcebergQuantity, nameof(order.IcebergQuantity));
        BinanceSpotTradeValidation.StrategyType(order.StrategyType);
        BinanceSpotTradeValidation.Peg(order.Type, order.PegPriceType, order.PegOffsetValue, order.PegOffsetType);
    }

    private static void ValidatePending(BinanceSpotOrderListPendingOrderRequest order)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));
        ValidateOptionalQuantity(order.Price, nameof(order.Price));
        ValidateOptionalQuantity(order.StopPrice, nameof(order.StopPrice));
        ValidateOptionalQuantity(order.TrailingDelta, nameof(order.TrailingDelta));
        ValidateOptionalQuantity(order.IcebergQuantity, nameof(order.IcebergQuantity));
        BinanceSpotTradeValidation.StrategyType(order.StrategyType);
        BinanceSpotTradeValidation.Peg(order.Type, order.PegPriceType, order.PegOffsetValue, order.PegOffsetType);
    }

    private static void ValidateOcoPair(BinanceSpotOrderListOcoLegRequest above, BinanceSpotOrderListOcoLegRequest below, bool validateBelowType)
    {
        ValidateOcoLeg(above, false);
        ValidateOcoLeg(below, validateBelowType);

        var aboveIsStop = IsStopOrder(above.Type);
        var belowIsStop = IsStopOrder(below.Type);
        if (aboveIsStop == belowIsStop)
            throw new ArgumentException("An OCO pair must contain one STOP_LOSS/STOP_LOSS_LIMIT order and one LIMIT_MAKER/TAKE_PROFIT/TAKE_PROFIT_LIMIT order.");
    }

    private static void ValidateOcoLeg(BinanceSpotOrderListOcoLegRequest order, bool isBelow)
    {
        if (order == null)
            throw new ArgumentNullException(nameof(order));
        if (!IsOcoType(order.Type) || isBelow && order.Type == BinanceSpotOrderType.LimitMaker)
            throw new ArgumentException(isBelow
                ? "A below OCO order must be STOP_LOSS, STOP_LOSS_LIMIT, TAKE_PROFIT, or TAKE_PROFIT_LIMIT."
                : "An OCO order must be STOP_LOSS, STOP_LOSS_LIMIT, LIMIT_MAKER, TAKE_PROFIT, or TAKE_PROFIT_LIMIT.", nameof(order));

        ValidateOptionalQuantity(order.IcebergQuantity, nameof(order.IcebergQuantity));
        ValidateOptionalQuantity(order.Price, nameof(order.Price));
        ValidateOptionalQuantity(order.StopPrice, nameof(order.StopPrice));
        ValidateOptionalQuantity(order.TrailingDelta, nameof(order.TrailingDelta));
        BinanceSpotTradeValidation.StrategyType(order.StrategyType);
        BinanceSpotTradeValidation.Peg(order.Type, order.PegPriceType, order.PegOffsetValue, order.PegOffsetType);
    }

    private static bool IsOcoType(BinanceSpotOrderType type)
        => type is BinanceSpotOrderType.StopLoss
            or BinanceSpotOrderType.StopLossLimit
            or BinanceSpotOrderType.LimitMaker
            or BinanceSpotOrderType.TakeProfit
            or BinanceSpotOrderType.TakeProfitLimit;

    private static bool IsStopOrder(BinanceSpotOrderType type)
        => type is BinanceSpotOrderType.StopLoss or BinanceSpotOrderType.StopLossLimit;

    private static void ValidateQuantity(decimal value, string parameterName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(parameterName, "Value must be greater than zero.");
    }

    private static void ValidateOptionalQuantity(decimal? value, string parameterName)
    {
        if (value is <= 0)
            throw new ArgumentOutOfRangeException(parameterName, "Value must be greater than zero when provided.");
    }

    private static void AddDecimal(ParameterCollection parameters, string name, decimal value, bool stringify)
        => parameters.AddParameter(name, stringify ? value.ToString(BinanceConstants.CI) : value);

    private static void AddOptionalDecimal(ParameterCollection parameters, string name, decimal? value, bool stringify)
        => parameters.AddOptional(name, value.HasValue && stringify ? value.Value.ToString(BinanceConstants.CI) : value);
}
