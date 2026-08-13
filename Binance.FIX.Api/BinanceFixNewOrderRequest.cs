using System;

namespace Binance.FIX.Api;

/// <summary>
/// Immutable, validated public request for one Binance Spot FIX NewOrderSingle message.
/// </summary>
public sealed class BinanceFixNewOrderRequest
{
    /// <summary>
    /// Creates a request from the complete current Binance Spot FIX NewOrderSingle field surface.
    /// </summary>
    /// <param name="clientOrderId">Caller-owned client order ID matching <c>^[a-zA-Z0-9-_]{1,36}$</c>. It is never generated or reused by this wrapper.</param>
    /// <param name="symbol">Non-empty Spot symbol representable as valid UTF-8 without control characters.</param>
    /// <param name="side">Order side.</param>
    /// <param name="orderType">Binance order type.</param>
    /// <param name="orderQuantity">Base-asset quantity.</param>
    /// <param name="cashOrderQuantity">Quote-asset quantity for a market order.</param>
    /// <param name="price">Limit price.</param>
    /// <param name="timeInForce">Time-in-force value.</param>
    /// <param name="icebergQuantity">Visible quantity for an iceberg order.</param>
    /// <param name="triggerPrice">Activation price for a contingent order.</param>
    /// <param name="triggerTrailingDeltaBips">Trailing delta in basis points for a contingent order.</param>
    /// <param name="targetStrategy">Strategy type; Binance requires a value of at least 1,000,000.</param>
    /// <param name="strategyId">Caller-owned signed 64-bit strategy ID.</param>
    /// <param name="selfTradePreventionMode">Optional self-trade-prevention mode.</param>
    /// <param name="pegPriceType">Reference side of the book for a pegged order.</param>
    /// <param name="pegOffsetValue">Optional price-tier offset for a pegged order.</param>
    /// <param name="smartOrderRouting">Optional Smart Order Routing flag.</param>
    /// <exception cref="ArgumentException">A string or field combination violates the published contract.</exception>
    /// <exception cref="ArgumentOutOfRangeException">A numeric or enum value violates the published contract.</exception>
    public BinanceFixNewOrderRequest(
        string clientOrderId,
        string symbol,
        BinanceFixOrderSide side,
        BinanceFixOrderType orderType,
        decimal? orderQuantity = null,
        decimal? cashOrderQuantity = null,
        decimal? price = null,
        BinanceFixTimeInForce? timeInForce = null,
        decimal? icebergQuantity = null,
        decimal? triggerPrice = null,
        long? triggerTrailingDeltaBips = null,
        long? targetStrategy = null,
        long? strategyId = null,
        BinanceFixSelfTradePreventionMode? selfTradePreventionMode = null,
        BinanceFixPegPriceType? pegPriceType = null,
        decimal? pegOffsetValue = null,
        bool? smartOrderRouting = null)
    {
        ValidateClientOrderId(clientOrderId);
        ValidateUtf8Symbol(symbol, nameof(symbol));
        ValidateEnum(side, nameof(side));
        ValidateEnum(orderType, nameof(orderType));
        ValidateNullableEnum(timeInForce, nameof(timeInForce));
        ValidateNullableEnum(selfTradePreventionMode, nameof(selfTradePreventionMode));
        ValidateNullableEnum(pegPriceType, nameof(pegPriceType));
        ValidatePositive(orderQuantity, nameof(orderQuantity));
        ValidatePositive(cashOrderQuantity, nameof(cashOrderQuantity));
        ValidatePositive(price, nameof(price));
        ValidatePositive(icebergQuantity, nameof(icebergQuantity));
        ValidatePositive(triggerPrice, nameof(triggerPrice));

        if (triggerTrailingDeltaBips is <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(triggerTrailingDeltaBips),
                triggerTrailingDeltaBips,
                "Trigger trailing delta must be positive.");
        }

        if (targetStrategy is < 1_000_000)
        {
            throw new ArgumentOutOfRangeException(
                nameof(targetStrategy),
                targetStrategy,
                "Target strategy must be at least 1,000,000.");
        }

        ValidateOrderCombination(
            orderType,
            orderQuantity,
            cashOrderQuantity,
            price,
            timeInForce,
            icebergQuantity,
            triggerPrice,
            triggerTrailingDeltaBips,
            pegPriceType,
            pegOffsetValue);

        ClientOrderId = clientOrderId;
        Symbol = symbol;
        Side = side;
        OrderType = orderType;
        OrderQuantity = orderQuantity;
        CashOrderQuantity = cashOrderQuantity;
        Price = price;
        TimeInForce = timeInForce;
        IcebergQuantity = icebergQuantity;
        TriggerPrice = triggerPrice;
        TriggerTrailingDeltaBips = triggerTrailingDeltaBips;
        TargetStrategy = targetStrategy;
        StrategyId = strategyId;
        SelfTradePreventionMode = selfTradePreventionMode;
        PegPriceType = pegPriceType;
        PegOffsetValue = pegOffsetValue;
        SmartOrderRouting = smartOrderRouting;
    }

    /// <summary>Gets the caller-owned FIX tag 11 value.</summary>
    public string ClientOrderId { get; }

    /// <summary>Gets the FIX tag 55 symbol.</summary>
    public string Symbol { get; }

    /// <summary>Gets the order side.</summary>
    public BinanceFixOrderSide Side { get; }

    /// <summary>Gets the Binance order type.</summary>
    public BinanceFixOrderType OrderType { get; }

    /// <summary>Gets the optional base-asset quantity.</summary>
    public decimal? OrderQuantity { get; }

    /// <summary>Gets the optional quote-asset market-order quantity.</summary>
    public decimal? CashOrderQuantity { get; }

    /// <summary>Gets the optional limit price.</summary>
    public decimal? Price { get; }

    /// <summary>Gets the optional time-in-force value.</summary>
    public BinanceFixTimeInForce? TimeInForce { get; }

    /// <summary>Gets the optional visible iceberg quantity.</summary>
    public decimal? IcebergQuantity { get; }

    /// <summary>Gets the optional contingent-order trigger price.</summary>
    public decimal? TriggerPrice { get; }

    /// <summary>Gets the optional trailing delta in basis points.</summary>
    public long? TriggerTrailingDeltaBips { get; }

    /// <summary>Gets the optional strategy type.</summary>
    public long? TargetStrategy { get; }

    /// <summary>Gets the optional caller-owned strategy ID.</summary>
    public long? StrategyId { get; }

    /// <summary>Gets the optional self-trade-prevention mode.</summary>
    public BinanceFixSelfTradePreventionMode? SelfTradePreventionMode { get; }

    /// <summary>Gets the optional pegged-price reference.</summary>
    public BinanceFixPegPriceType? PegPriceType { get; }

    /// <summary>Gets the optional pegged price-tier offset.</summary>
    public decimal? PegOffsetValue { get; }

    /// <summary>Gets the optional Smart Order Routing flag.</summary>
    public bool? SmartOrderRouting { get; }

    private static void ValidateOrderCombination(
        BinanceFixOrderType orderType,
        decimal? orderQuantity,
        decimal? cashOrderQuantity,
        decimal? price,
        BinanceFixTimeInForce? timeInForce,
        decimal? icebergQuantity,
        decimal? triggerPrice,
        long? triggerTrailingDeltaBips,
        BinanceFixPegPriceType? pegPriceType,
        decimal? pegOffsetValue)
    {
        var isContingent = orderType is BinanceFixOrderType.StopLoss
            or BinanceFixOrderType.StopLossLimit
            or BinanceFixOrderType.TakeProfit
            or BinanceFixOrderType.TakeProfitLimit;
        var isLimit = orderType is BinanceFixOrderType.Limit
            or BinanceFixOrderType.LimitMaker
            or BinanceFixOrderType.StopLossLimit
            or BinanceFixOrderType.TakeProfitLimit;

        if (orderType is BinanceFixOrderType.Market)
        {
            if (orderQuantity.HasValue == cashOrderQuantity.HasValue)
            {
                throw new ArgumentException(
                    "A market order requires exactly one of OrderQuantity or CashOrderQuantity.",
                    nameof(orderQuantity));
            }
        }
        else
        {
            Require(orderQuantity, nameof(orderQuantity), "This order type requires OrderQuantity.");
            Reject(cashOrderQuantity, nameof(cashOrderQuantity), "CashOrderQuantity is supported only for market orders.");
        }

        if (isLimit)
        {
            Require(price, nameof(price), "This order type requires Price.");
            if (orderType is not BinanceFixOrderType.LimitMaker)
            {
                Require(timeInForce, nameof(timeInForce), "This order type requires TimeInForce.");
            }
            else if (timeInForce is not null and not BinanceFixTimeInForce.GoodTillCanceled)
            {
                throw new ArgumentException(
                    "A limit-maker order supports only GoodTillCanceled time in force.",
                    nameof(timeInForce));
            }
        }
        else if (orderType is not BinanceFixOrderType.Pegged)
        {
            Reject(price, nameof(price), "Price is not supported for this order type.");
            Reject(timeInForce, nameof(timeInForce), "TimeInForce is not supported for this order type.");
        }

        if (isContingent)
        {
            if (triggerPrice is null && triggerTrailingDeltaBips is null)
            {
                throw new ArgumentException(
                    "A contingent order requires TriggerPrice, TriggerTrailingDeltaBips, or both.",
                    nameof(triggerPrice));
            }
        }
        else
        {
            Reject(triggerPrice, nameof(triggerPrice), "TriggerPrice is supported only for contingent orders.");
            Reject(
                triggerTrailingDeltaBips,
                nameof(triggerTrailingDeltaBips),
                "TriggerTrailingDeltaBips is supported only for contingent orders.");
        }

        if (icebergQuantity is not null)
        {
            if (orderQuantity is null)
            {
                throw new ArgumentException("An iceberg order requires OrderQuantity.", nameof(icebergQuantity));
            }

            if (timeInForce is not BinanceFixTimeInForce.GoodTillCanceled)
            {
                throw new ArgumentException(
                    "An iceberg order requires GoodTillCanceled time in force.",
                    nameof(timeInForce));
            }
        }

        if (orderType is BinanceFixOrderType.Pegged)
        {
            Require(pegPriceType, nameof(pegPriceType), "A pegged order requires PegPriceType.");
        }
        else
        {
            Reject(pegPriceType, nameof(pegPriceType), "PegPriceType is supported only for pegged orders.");
            Reject(pegOffsetValue, nameof(pegOffsetValue), "PegOffsetValue is supported only for pegged orders.");
        }

        if (pegOffsetValue is not null && pegPriceType is null)
        {
            throw new ArgumentException("PegOffsetValue requires PegPriceType.", nameof(pegOffsetValue));
        }
    }

    private static void ValidateClientOrderId(string clientOrderId)
    {
        if (clientOrderId is not { Length: >= 1 and <= 36 })
        {
            throw new ArgumentException(
                "ClientOrderId must contain 1-36 ASCII letters, digits, hyphens, or underscores.",
                nameof(clientOrderId));
        }

        foreach (var character in clientOrderId)
        {
            if ((character is >= 'a' and <= 'z')
                || (character is >= 'A' and <= 'Z')
                || (character is >= '0' and <= '9')
                || character is '-' or '_')
            {
                continue;
            }

            throw new ArgumentException(
                "ClientOrderId must contain 1-36 ASCII letters, digits, hyphens, or underscores.",
                nameof(clientOrderId));
        }
    }

    private static void ValidateUtf8Symbol(string value, string parameterName)
    {
        if (!BinanceFixUtf8FieldValidator.IsValidNonEmptyValue(value))
        {
            throw new ArgumentException(
                "Symbol must be non-empty valid Unicode without control characters.",
                parameterName);
        }
    }

    private static void ValidatePositive(decimal? value, string parameterName)
    {
        if (value is <= 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, value, "Value must be positive.");
        }
    }

    private static void ValidateEnum<TEnum>(TEnum value, string parameterName)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(value))
        {
            throw new ArgumentOutOfRangeException(parameterName, value, "Unsupported value.");
        }
    }

    private static void ValidateNullableEnum<TEnum>(TEnum? value, string parameterName)
        where TEnum : struct, Enum
    {
        if (value is not null)
        {
            ValidateEnum(value.Value, parameterName);
        }
    }

    private static void Require<T>(T? value, string parameterName, string message)
        where T : struct
    {
        if (value is null)
        {
            throw new ArgumentException(message, parameterName);
        }
    }

    private static void Reject<T>(T? value, string parameterName, string message)
        where T : struct
    {
        if (value is not null)
        {
            throw new ArgumentException(message, parameterName);
        }
    }
}
