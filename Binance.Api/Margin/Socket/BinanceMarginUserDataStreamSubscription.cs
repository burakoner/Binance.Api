namespace Binance.Api.Margin;

/// <summary>
/// A Margin listen-token user data stream subscription.
/// </summary>
public sealed class BinanceMarginUserDataStreamSubscription
{
    private readonly BinanceMarginUserDataStreamRequest request;

    /// <summary>
    /// Server-assigned subscription identifier. This can change after extension or reconnection.
    /// </summary>
    public int SubscriptionId => request.SubscriptionId
        ?? throw new InvalidOperationException("The Margin user data stream subscription has not been confirmed by Binance.");

    /// <summary>
    /// Raw expiration-time value returned by the WebSocket API.
    /// The current official endpoint page does not define its unit and shows a value with microsecond-scale precision.
    /// </summary>
    public long ExpirationTime => request.ExpirationTime
        ?? throw new InvalidOperationException("The Margin user data stream subscription expiration time was not returned by Binance.");

    /// <summary>
    /// Underlying socket subscription used to observe connection lifecycle events.
    /// </summary>
    public WebSocketUpdateSubscription SocketSubscription { get; }

    internal BinanceMarginUserDataStreamSubscription(BinanceMarginUserDataStreamRequest request, WebSocketUpdateSubscription socketSubscription)
    {
        this.request = request;
        SocketSubscription = socketSubscription;
    }

    internal BinanceMarginUserDataStreamRequest GetRequest() => request;
}
