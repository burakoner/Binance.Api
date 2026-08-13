using System;

namespace Binance.FIX.Api;

/// <summary>
/// Immutable, validated public request for one Binance Spot FIX OrderCancelRequest message.
/// </summary>
public sealed class BinanceFixOrderCancelRequest
{
    private BinanceFixOrderCancelRequest(
        string clientOrderId,
        string symbol,
        BinanceFixOrderCancelTarget target,
        long? orderId,
        string? originalClientOrderId,
        string? listId,
        string? originalClientListId,
        BinanceFixCancelRestriction? cancelRestriction)
    {
        ClientOrderId = clientOrderId;
        Symbol = symbol;
        Target = target;
        OrderId = orderId;
        OriginalClientOrderId = originalClientOrderId;
        ListId = listId;
        OriginalClientListId = originalClientListId;
        CancelRestriction = cancelRestriction;
    }

    /// <summary>
    /// Creates a cancellation targeting one order. If that order belongs to a list, Binance cancels the whole list.
    /// </summary>
    /// <param name="clientOrderId">Caller-owned ID for this cancel request.</param>
    /// <param name="symbol">Printable-ASCII Spot symbol.</param>
    /// <param name="orderId">Optional exchange-assigned signed 64-bit order ID.</param>
    /// <param name="originalClientOrderId">Optional client order ID of the order to cancel.</param>
    /// <param name="cancelRestriction">Optional order-state restriction.</param>
    /// <returns>A validated order-target cancellation.</returns>
    public static BinanceFixOrderCancelRequest ForOrder(
        string clientOrderId,
        string symbol,
        long? orderId = null,
        string? originalClientOrderId = null,
        BinanceFixCancelRestriction? cancelRestriction = null)
    {
        ValidateCommon(clientOrderId, symbol, cancelRestriction);
        ValidateOptionalClientId(originalClientOrderId, nameof(originalClientOrderId));
        if (orderId is null && originalClientOrderId is null)
        {
            throw new ArgumentException(
                "An order cancellation requires OrderId, OriginalClientOrderId, or both.",
                nameof(orderId));
        }

        return new BinanceFixOrderCancelRequest(
            clientOrderId,
            symbol,
            BinanceFixOrderCancelTarget.Order,
            orderId,
            originalClientOrderId,
            listId: null,
            originalClientListId: null,
            cancelRestriction);
    }

    /// <summary>
    /// Creates a cancellation targeting one order list.
    /// </summary>
    /// <param name="clientOrderId">Caller-owned ID for this cancel request.</param>
    /// <param name="symbol">Printable-ASCII Spot symbol.</param>
    /// <param name="listId">Optional opaque exchange-assigned order-list ID.</param>
    /// <param name="originalClientListId">Optional client list ID of the order list to cancel.</param>
    /// <param name="cancelRestriction">Optional order-state restriction.</param>
    /// <returns>A validated order-list-target cancellation.</returns>
    public static BinanceFixOrderCancelRequest ForOrderList(
        string clientOrderId,
        string symbol,
        string? listId = null,
        string? originalClientListId = null,
        BinanceFixCancelRestriction? cancelRestriction = null)
    {
        ValidateCommon(clientOrderId, symbol, cancelRestriction);
        ValidateOptionalPrintableAscii(listId, nameof(listId));
        ValidateOptionalClientId(originalClientListId, nameof(originalClientListId));
        if (listId is null && originalClientListId is null)
        {
            throw new ArgumentException(
                "An order-list cancellation requires ListId, OriginalClientListId, or both.",
                nameof(listId));
        }

        return new BinanceFixOrderCancelRequest(
            clientOrderId,
            symbol,
            BinanceFixOrderCancelTarget.OrderList,
            orderId: null,
            originalClientOrderId: null,
            listId,
            originalClientListId,
            cancelRestriction);
    }

    /// <summary>Gets the caller-owned FIX tag 11 value for this cancellation.</summary>
    public string ClientOrderId { get; }

    /// <summary>Gets the FIX tag 55 symbol.</summary>
    public string Symbol { get; }

    /// <summary>Gets whether this request targets an order or an order list.</summary>
    public BinanceFixOrderCancelTarget Target { get; }

    /// <summary>Gets the optional exchange-assigned order ID.</summary>
    public long? OrderId { get; }

    /// <summary>Gets the optional client order ID of the target order.</summary>
    public string? OriginalClientOrderId { get; }

    /// <summary>Gets the optional opaque exchange-assigned order-list ID.</summary>
    public string? ListId { get; }

    /// <summary>Gets the optional client list ID of the target order list.</summary>
    public string? OriginalClientListId { get; }

    /// <summary>Gets the optional order-state restriction.</summary>
    public BinanceFixCancelRestriction? CancelRestriction { get; }

    internal bool MatchesCoreResponse(string? clientOrderId, string symbol)
        => string.Equals(ClientOrderId, clientOrderId, StringComparison.Ordinal)
            && string.Equals(Symbol, symbol, StringComparison.Ordinal);

    private static void ValidateCommon(
        string clientOrderId,
        string symbol,
        BinanceFixCancelRestriction? cancelRestriction)
    {
        ValidateClientId(clientOrderId, nameof(clientOrderId));
        ValidatePrintableAscii(symbol, nameof(symbol));
        if (cancelRestriction is not null && !Enum.IsDefined(cancelRestriction.Value))
        {
            throw new ArgumentOutOfRangeException(
                nameof(cancelRestriction),
                cancelRestriction,
                "Unsupported cancel restriction.");
        }
    }

    private static void ValidateOptionalClientId(string? value, string parameterName)
    {
        if (value is not null)
        {
            ValidateClientId(value, parameterName);
        }
    }

    private static void ValidateClientId(string value, string parameterName)
    {
        if (value is not { Length: >= 1 and <= 36 })
        {
            throw new ArgumentException(
                "Client ID must contain 1-36 ASCII letters, digits, hyphens, or underscores.",
                parameterName);
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

            throw new ArgumentException(
                "Client ID must contain 1-36 ASCII letters, digits, hyphens, or underscores.",
                parameterName);
        }
    }

    private static void ValidateOptionalPrintableAscii(string? value, string parameterName)
    {
        if (value is not null)
        {
            ValidatePrintableAscii(value, parameterName);
        }
    }

    private static void ValidatePrintableAscii(string value, string parameterName)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Value must not be empty.", parameterName);
        }

        foreach (var character in value)
        {
            if (character is < ' ' or > '~')
            {
                throw new ArgumentException("Value must contain only printable ASCII characters.", parameterName);
            }
        }
    }
}
