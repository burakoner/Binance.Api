using System;

namespace Binance.FIX.Api;

/// <summary>
/// Immutable, validated public request for one Binance Spot FIX OrderAmendKeepPriorityRequest.
/// </summary>
public sealed class BinanceFixOrderAmendRequest
{
    /// <summary>
    /// Creates an amendment that reduces the original order quantity while preserving priority.
    /// </summary>
    /// <param name="clientOrderId">Caller-owned ID for the amended order.</param>
    /// <param name="symbol">Printable-ASCII Spot symbol.</param>
    /// <param name="newQuantity">Positive new total order quantity. Binance requires it to be smaller than the current quantity.</param>
    /// <param name="orderId">Optional exchange-assigned signed 64-bit order ID.</param>
    /// <param name="originalClientOrderId">Optional current client order ID.</param>
    public BinanceFixOrderAmendRequest(
        string clientOrderId,
        string symbol,
        decimal newQuantity,
        long? orderId = null,
        string? originalClientOrderId = null)
    {
        ValidateClientId(clientOrderId, nameof(clientOrderId));
        ValidatePrintableAscii(symbol, nameof(symbol));
        ValidateOptionalClientId(originalClientOrderId, nameof(originalClientOrderId));

        if (newQuantity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(newQuantity),
                newQuantity,
                "New quantity must be positive.");
        }

        if (orderId is null && originalClientOrderId is null)
        {
            throw new ArgumentException(
                "An order amendment requires OrderId, OriginalClientOrderId, or both.",
                nameof(orderId));
        }

        ClientOrderId = clientOrderId;
        Symbol = symbol;
        NewQuantity = newQuantity;
        OrderId = orderId;
        OriginalClientOrderId = originalClientOrderId;
    }

    /// <summary>Gets the caller-owned FIX tag 11 value for the amended order.</summary>
    public string ClientOrderId { get; }

    /// <summary>Gets the FIX tag 55 symbol.</summary>
    public string Symbol { get; }

    /// <summary>Gets the new total order quantity.</summary>
    public decimal NewQuantity { get; }

    /// <summary>Gets the optional exchange-assigned order ID.</summary>
    public long? OrderId { get; }

    /// <summary>Gets the optional current client order ID.</summary>
    public string? OriginalClientOrderId { get; }

    internal bool MatchesCoreResponse(string? clientOrderId, string symbol, decimal quantity)
        => string.Equals(ClientOrderId, clientOrderId, StringComparison.Ordinal)
            && string.Equals(Symbol, symbol, StringComparison.Ordinal)
            && NewQuantity == quantity;

    internal bool MatchesEchoedTarget(long? orderId, string? originalClientOrderId)
    {
        if (orderId is not null && orderId != OrderId)
        {
            return false;
        }

        return originalClientOrderId is null
            || string.Equals(
                OriginalClientOrderId,
                originalClientOrderId,
                StringComparison.Ordinal);
    }

    internal bool MatchesReportedTarget(long? orderId, string? originalClientOrderId)
    {
        if (OrderId is not null && orderId is not null && OrderId != orderId)
        {
            return false;
        }

        return OriginalClientOrderId is null
            || originalClientOrderId is null
            || string.Equals(
                OriginalClientOrderId,
                originalClientOrderId,
                StringComparison.Ordinal);
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
                throw new ArgumentException(
                    "Value must contain only printable ASCII characters.",
                    parameterName);
            }
        }
    }
}
