using System;

namespace Binance.FIX.Api;

/// <summary>
/// Immutable, validated public request for one Binance Spot FIX
/// OrderCancelRequestAndNewOrderSingle message.
/// </summary>
public sealed class BinanceFixCancelReplaceRequest
{
    /// <summary>
    /// Creates a two-phase request that first cancels one order and then conditionally submits a new order.
    /// </summary>
    /// <param name="newOrder">The complete validated new-order phase.</param>
    /// <param name="mode">Behavior when the cancellation phase fails.</param>
    /// <param name="orderId">Optional exchange-assigned signed 64-bit ID of the order to cancel.</param>
    /// <param name="originalClientOrderId">Optional client order ID of the order to cancel.</param>
    /// <param name="cancelClientOrderId">Optional caller-owned ID for the cancellation phase.</param>
    /// <param name="cancelRestriction">Optional state restriction applied to the cancellation phase.</param>
    /// <param name="orderRateLimitExceededMode">Optional behavior when the unfilled-order rate limit is exceeded.</param>
    public BinanceFixCancelReplaceRequest(
        BinanceFixNewOrderRequest newOrder,
        BinanceFixCancelReplaceMode mode,
        long? orderId = null,
        string? originalClientOrderId = null,
        string? cancelClientOrderId = null,
        BinanceFixCancelRestriction? cancelRestriction = null,
        BinanceFixOrderRateLimitExceededMode? orderRateLimitExceededMode = null)
    {
        ArgumentNullException.ThrowIfNull(newOrder);
        ValidateEnum(mode, nameof(mode));
        ValidateOptionalEnum(cancelRestriction, nameof(cancelRestriction));
        ValidateOptionalEnum(orderRateLimitExceededMode, nameof(orderRateLimitExceededMode));
        ValidateOptionalClientId(originalClientOrderId, nameof(originalClientOrderId));
        ValidateOptionalClientId(cancelClientOrderId, nameof(cancelClientOrderId));

        if (orderId is null && originalClientOrderId is null)
        {
            throw new ArgumentException(
                "Cancel-and-new requires OrderId, OriginalClientOrderId, or both.",
                nameof(orderId));
        }

        if (newOrder.SmartOrderRouting is not null)
        {
            throw new ArgumentException(
                "The current OrderCancelRequestAndNewOrderSingle surface does not publish SOR.",
                nameof(newOrder));
        }

        NewOrder = newOrder;
        Mode = mode;
        OrderId = orderId;
        OriginalClientOrderId = originalClientOrderId;
        CancelClientOrderId = cancelClientOrderId;
        CancelRestriction = cancelRestriction;
        OrderRateLimitExceededMode = orderRateLimitExceededMode;
    }

    /// <summary>Gets the validated new-order phase.</summary>
    public BinanceFixNewOrderRequest NewOrder { get; }

    /// <summary>Gets the behavior when the cancellation phase fails.</summary>
    public BinanceFixCancelReplaceMode Mode { get; }

    /// <summary>Gets the optional exchange-assigned ID of the order to cancel.</summary>
    public long? OrderId { get; }

    /// <summary>Gets the optional client order ID of the order to cancel.</summary>
    public string? OriginalClientOrderId { get; }

    /// <summary>Gets the optional caller-owned ID for the cancellation phase.</summary>
    public string? CancelClientOrderId { get; }

    /// <summary>Gets the optional cancellation state restriction.</summary>
    public BinanceFixCancelRestriction? CancelRestriction { get; }

    /// <summary>Gets the optional unfilled-order-rate-limit behavior.</summary>
    public BinanceFixOrderRateLimitExceededMode? OrderRateLimitExceededMode { get; }

    private static void ValidateOptionalClientId(string? value, string parameterName)
    {
        if (value is null)
        {
            return;
        }

        if (value.Length is < 1 or > 36)
        {
            throw InvalidClientId(parameterName);
        }

        foreach (var character in value)
        {
            if ((character is >= 'a' and <= 'z')
                || (character is >= 'A' and <= 'Z')
                || (character is >= '0' and <= '9')
                || character is '-' or '_')
            {
                continue;
            }

            throw InvalidClientId(parameterName);
        }
    }

    private static ArgumentException InvalidClientId(string parameterName)
        => new(
            "Client ID must contain 1-36 ASCII letters, digits, hyphens, or underscores.",
            parameterName);

    private static void ValidateEnum<TEnum>(TEnum value, string parameterName)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(value))
        {
            throw new ArgumentOutOfRangeException(parameterName, value, "Unsupported value.");
        }
    }

    private static void ValidateOptionalEnum<TEnum>(TEnum? value, string parameterName)
        where TEnum : struct, Enum
    {
        if (value is not null)
        {
            ValidateEnum(value.Value, parameterName);
        }
    }
}
