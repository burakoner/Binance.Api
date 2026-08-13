namespace Binance.FIX.Api;

/// <summary>
/// Immutable public projection of the complete current Binance Spot FIX Reject surface.
/// </summary>
public sealed class BinanceFixReject
{
    internal BinanceFixReject()
    {
    }

    /// <summary>Gets the optional sequence number of the rejected message.</summary>
    public long? ReferencedSequenceNumber { get; internal init; }

    /// <summary>Gets the optional tag that directly caused the rejection.</summary>
    public long? ReferencedTagId { get; internal init; }

    /// <summary>Gets the optional MsgType of the rejected message.</summary>
    public string? ReferencedMessageType { get; internal init; }

    /// <summary>Gets the optional session-level rejection reason.</summary>
    public BinanceFixSessionRejectReason? SessionRejectReason { get; internal init; }

    /// <summary>Gets the optional Binance API error code.</summary>
    public long? ErrorCode { get; internal init; }

    /// <summary>Gets the optional human-readable error text.</summary>
    public string? ErrorText { get; internal init; }
}
