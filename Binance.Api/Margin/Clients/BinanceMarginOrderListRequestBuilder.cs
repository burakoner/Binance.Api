using Binance.Api.Spot;

namespace Binance.Api.Margin;

internal static class BinanceMarginOrderListRequestBuilder
{
    public static ParameterCollection Oto(BinanceMarginOtoOrderListRequest request, Func<string?, string?> applyClientOrderId)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        ValidateCommon(request);
        ValidatePositive(request.WorkingIcebergQuantity, nameof(request.WorkingIcebergQuantity));
        ValidateIcebergTimeInForce(request.WorkingIcebergQuantity, request.WorkingTimeInForce, "working");
        ValidatePositive(request.PendingQuantity, nameof(request.PendingQuantity));
        ValidateOptionalPositive(request.PendingPrice, nameof(request.PendingPrice));
        ValidateOptionalPositive(request.PendingStopPrice, nameof(request.PendingStopPrice));
        ValidateOptionalPositive(request.PendingTrailingDelta, nameof(request.PendingTrailingDelta));
        ValidateOptionalPositive(request.PendingIcebergQuantity, nameof(request.PendingIcebergQuantity));
        ValidateTimeInForce(request.PendingTimeInForce, nameof(request.PendingTimeInForce));
        ValidateIcebergTimeInForce(request.PendingIcebergQuantity, request.PendingTimeInForce, "pending");
        ValidatePendingOrder(
            request.PendingType,
            request.PendingPrice,
            request.PendingStopPrice,
            request.PendingTrailingDelta,
            request.PendingTimeInForce,
            "pending");

        var parameters = CreateCommon(request, applyClientOrderId);
        AddDecimal(parameters, "workingIcebergQty", request.WorkingIcebergQuantity);
        parameters.AddEnum("pendingType", request.PendingType);
        parameters.AddEnum("pendingSide", request.PendingSide);
        AddDecimal(parameters, "pendingQuantity", request.PendingQuantity);
        parameters.AddOptional("pendingClientOrderId", applyClientOrderId(request.PendingClientOrderId));
        AddOptionalDecimal(parameters, "pendingPrice", request.PendingPrice);
        AddOptionalDecimal(parameters, "pendingStopPrice", request.PendingStopPrice);
        AddOptionalDecimal(parameters, "pendingTrailingDelta", request.PendingTrailingDelta);
        AddOptionalDecimal(parameters, "pendingIcebergQty", request.PendingIcebergQuantity);
        parameters.AddOptionalEnum("pendingTimeInForce", request.PendingTimeInForce);
        return parameters;
    }

    public static ParameterCollection Otoco(BinanceMarginOtocoOrderListRequest request, Func<string?, string?> applyClientOrderId)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        ValidateCommon(request);
        ValidateOptionalPositive(request.WorkingIcebergQuantity, nameof(request.WorkingIcebergQuantity));
        ValidateIcebergTimeInForce(request.WorkingIcebergQuantity, request.WorkingTimeInForce, "working");
        ValidatePositive(request.PendingQuantity, nameof(request.PendingQuantity));
        ValidateOcoType(request.PendingAboveType, nameof(request.PendingAboveType));
        ValidateOcoLeg(
            request.PendingAboveType,
            request.PendingAbovePrice,
            request.PendingAboveStopPrice,
            request.PendingAboveTrailingDelta,
            request.PendingAboveIcebergQuantity,
            request.PendingAboveTimeInForce,
            "pendingAbove");

        if (request.PendingBelowType.HasValue)
        {
            ValidateOcoType(request.PendingBelowType.Value, nameof(request.PendingBelowType));
            ValidateOcoLeg(
                request.PendingBelowType.Value,
                request.PendingBelowPrice,
                request.PendingBelowStopPrice,
                request.PendingBelowTrailingDelta,
                request.PendingBelowIcebergQuantity,
                request.PendingBelowTimeInForce,
                "pendingBelow");
        }
        else if (request.PendingBelowClientOrderId != null
            || request.PendingBelowPrice.HasValue
            || request.PendingBelowStopPrice.HasValue
            || request.PendingBelowTrailingDelta.HasValue
            || request.PendingBelowIcebergQuantity.HasValue
            || request.PendingBelowTimeInForce.HasValue)
        {
            throw new ArgumentException("PendingBelowType is required when any pending-below field is provided.", nameof(request));
        }

        var parameters = CreateCommon(request, applyClientOrderId);
        AddOptionalDecimal(parameters, "workingIcebergQty", request.WorkingIcebergQuantity);
        parameters.AddEnum("pendingSide", request.PendingSide);
        AddDecimal(parameters, "pendingQuantity", request.PendingQuantity);
        AddOcoLeg(
            parameters,
            "pendingAbove",
            request.PendingAboveType,
            request.PendingAboveClientOrderId,
            request.PendingAbovePrice,
            request.PendingAboveStopPrice,
            request.PendingAboveTrailingDelta,
            request.PendingAboveIcebergQuantity,
            request.PendingAboveTimeInForce,
            applyClientOrderId);

        if (request.PendingBelowType.HasValue)
        {
            AddOcoLeg(
                parameters,
                "pendingBelow",
                request.PendingBelowType.Value,
                request.PendingBelowClientOrderId,
                request.PendingBelowPrice,
                request.PendingBelowStopPrice,
                request.PendingBelowTrailingDelta,
                request.PendingBelowIcebergQuantity,
                request.PendingBelowTimeInForce,
                applyClientOrderId);
        }

        return parameters;
    }

    private static ParameterCollection CreateCommon(BinanceMarginOrderListRequest request, Func<string?, string?> applyClientOrderId)
    {
        var parameters = new ParameterCollection
        {
            { "symbol", request.Symbol }
        };
        parameters.AddEnum("workingType", request.WorkingType);
        parameters.AddEnum("workingSide", request.WorkingSide);
        AddDecimal(parameters, "workingPrice", request.WorkingPrice);
        AddDecimal(parameters, "workingQuantity", request.WorkingQuantity);
        parameters.AddOptional("isIsolated", FormatBoolean(request.IsIsolated));
        parameters.AddOptionalEnum("sideEffectType", request.SideEffectType);
        parameters.AddOptional("autoRepayAtCancel", request.AutoRepayAtCancel);
        parameters.AddOptional("listClientOrderId", request.ListClientOrderId);
        parameters.AddOptionalEnum("newOrderRespType", request.OrderResponseType);
        parameters.AddOptionalEnum("selfTradePreventionMode", request.SelfTradePreventionMode);
        parameters.AddOptional("workingClientOrderId", applyClientOrderId(request.WorkingClientOrderId));
        parameters.AddOptionalEnum("workingTimeInForce", request.WorkingTimeInForce);
        return parameters;
    }

    private static void AddOcoLeg(
        ParameterCollection parameters,
        string prefix,
        BinanceSpotOrderType type,
        string? clientOrderId,
        decimal? price,
        decimal? stopPrice,
        decimal? trailingDelta,
        decimal? icebergQuantity,
        BinanceTimeInForce? timeInForce,
        Func<string?, string?> applyClientOrderId)
    {
        parameters.AddEnum(prefix + "Type", type);
        parameters.AddOptional(prefix + "ClientOrderId", applyClientOrderId(clientOrderId));
        AddOptionalDecimal(parameters, prefix + "Price", price);
        AddOptionalDecimal(parameters, prefix + "StopPrice", stopPrice);
        AddOptionalDecimal(parameters, prefix + "TrailingDelta", trailingDelta);
        AddOptionalDecimal(parameters, prefix + "IcebergQty", icebergQuantity);
        parameters.AddOptionalEnum(prefix + "TimeInForce", timeInForce);
    }

    private static void ValidateCommon(BinanceMarginOrderListRequest request)
    {
        request.Symbol.ValidateBinanceSymbol();
        if (request.WorkingType is not BinanceSpotOrderType.Limit and not BinanceSpotOrderType.LimitMaker)
            throw new ArgumentException("A working order must be LIMIT or LIMIT_MAKER.", nameof(request));
        if (request.SideEffectType is not null
            and not BinanceMarginSideEffectType.NoSideEffect
            and not BinanceMarginSideEffectType.MarginBuy)
        {
            throw new ArgumentException("Margin OTO and OTOCO accept only NO_SIDE_EFFECT or MARGIN_BUY.", nameof(request));
        }

        ValidatePositive(request.WorkingPrice, nameof(request.WorkingPrice));
        ValidatePositive(request.WorkingQuantity, nameof(request.WorkingQuantity));
        ValidateTimeInForce(request.WorkingTimeInForce, nameof(request.WorkingTimeInForce));
        if (request.WorkingType == BinanceSpotOrderType.Limit && !request.WorkingTimeInForce.HasValue)
            throw new ArgumentException("WorkingTimeInForce is required for a LIMIT working order.", nameof(request));
    }

    private static void ValidatePendingOrder(
        BinanceSpotOrderType type,
        decimal? price,
        decimal? stopPrice,
        decimal? trailingDelta,
        BinanceTimeInForce? timeInForce,
        string prefix)
    {
        if (type == BinanceSpotOrderType.Limit && (!price.HasValue || !timeInForce.HasValue))
            throw new ArgumentException($"{prefix}Price and {prefix}TimeInForce are required for a LIMIT order.");
        if (type is BinanceSpotOrderType.StopLoss or BinanceSpotOrderType.TakeProfit
            && !stopPrice.HasValue && !trailingDelta.HasValue)
        {
            throw new ArgumentException($"{prefix}StopPrice and/or {prefix}TrailingDelta is required for a {type} order.");
        }
        if (type is BinanceSpotOrderType.StopLossLimit or BinanceSpotOrderType.TakeProfitLimit)
        {
            if (!price.HasValue || !timeInForce.HasValue || !stopPrice.HasValue && !trailingDelta.HasValue)
                throw new ArgumentException($"{prefix}Price, {prefix}TimeInForce, and {prefix}StopPrice and/or {prefix}TrailingDelta are required for a {type} order.");
        }
        if (trailingDelta.HasValue && !price.HasValue)
            throw new ArgumentException($"{prefix}Price is required when {prefix}TrailingDelta is provided.");
    }

    private static void ValidateOcoLeg(
        BinanceSpotOrderType type,
        decimal? price,
        decimal? stopPrice,
        decimal? trailingDelta,
        decimal? icebergQuantity,
        BinanceTimeInForce? timeInForce,
        string prefix)
    {
        ValidateOptionalPositive(price, prefix + "Price");
        ValidateOptionalPositive(stopPrice, prefix + "StopPrice");
        ValidateOptionalPositive(trailingDelta, prefix + "TrailingDelta");
        ValidateOptionalPositive(icebergQuantity, prefix + "IcebergQuantity");
        ValidateTimeInForce(timeInForce, prefix + "TimeInForce");
        ValidateIcebergTimeInForce(icebergQuantity, timeInForce, prefix);

        if (type == BinanceSpotOrderType.LimitMaker && !price.HasValue)
            throw new ArgumentException($"{prefix}Price is required for a LIMIT_MAKER order.");
        if (type == BinanceSpotOrderType.StopLoss && !stopPrice.HasValue && !trailingDelta.HasValue)
            throw new ArgumentException($"{prefix}StopPrice and/or {prefix}TrailingDelta is required for a STOP_LOSS order.");
        if (type == BinanceSpotOrderType.StopLossLimit
            && (!price.HasValue || !timeInForce.HasValue || !stopPrice.HasValue && !trailingDelta.HasValue))
        {
            throw new ArgumentException($"{prefix}Price, {prefix}TimeInForce, and {prefix}StopPrice and/or {prefix}TrailingDelta are required for a STOP_LOSS_LIMIT order.");
        }
        if (trailingDelta.HasValue && !price.HasValue)
            throw new ArgumentException($"{prefix}Price is required when {prefix}TrailingDelta is provided.");
    }

    private static void ValidateOcoType(BinanceSpotOrderType type, string parameterName)
    {
        if (type is not BinanceSpotOrderType.LimitMaker
            and not BinanceSpotOrderType.StopLoss
            and not BinanceSpotOrderType.StopLossLimit)
        {
            throw new ArgumentException("An OTOCO pending leg must be LIMIT_MAKER, STOP_LOSS, or STOP_LOSS_LIMIT.", parameterName);
        }
    }

    private static void ValidateTimeInForce(BinanceTimeInForce? value, string parameterName)
    {
        if (value.HasValue
            && value is not BinanceTimeInForce.GoodTillCanceled
                and not BinanceTimeInForce.ImmediateOrCancel
                and not BinanceTimeInForce.FillOrKill)
        {
            throw new ArgumentException("Time in force must be GTC, IOC, or FOK.", parameterName);
        }
    }

    private static void ValidatePositive(decimal value, string parameterName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(parameterName, "Value must be greater than zero.");
    }

    private static void ValidateOptionalPositive(decimal? value, string parameterName)
    {
        if (value is <= 0)
            throw new ArgumentOutOfRangeException(parameterName, "Value must be greater than zero when provided.");
    }

    private static void ValidateIcebergTimeInForce(decimal? icebergQuantity, BinanceTimeInForce? timeInForce, string prefix)
    {
        if (icebergQuantity.HasValue && timeInForce != BinanceTimeInForce.GoodTillCanceled)
            throw new ArgumentException($"{prefix}TimeInForce must be GTC when {prefix}IcebergQty is provided.");
    }

    internal static int RequestWeight(BinanceMarginSideEffectType? sideEffectType)
        => sideEffectType is BinanceMarginSideEffectType.MarginBuy or BinanceMarginSideEffectType.AutoBorrowRepay ? 1_500 : 6;

    internal static string? FormatBoolean(bool? value)
        => value.HasValue ? value.Value ? "TRUE" : "FALSE" : null;

    private static void AddDecimal(ParameterCollection parameters, string name, decimal value)
        => parameters.AddParameter(name, value.ToString(BinanceConstants.CI));

    private static void AddOptionalDecimal(ParameterCollection parameters, string name, decimal? value)
        => parameters.AddOptional(name, value?.ToString(BinanceConstants.CI));
}
