namespace Binance.Api.Spot;

/// <summary>
/// A Spot WebSocket API user data stream subscription.
/// </summary>
public sealed class BinanceSpotUserDataStreamSubscription
{
    private readonly BinanceSpotUserDataStreamRequest request;

    /// <summary>
    /// Server-assigned subscription identifier. This value is refreshed after a reconnect.
    /// </summary>
    public long SubscriptionId => request.SubscriptionId
        ?? throw new InvalidOperationException("The user data stream subscription has not been confirmed by Binance.");

    /// <summary>
    /// Whether the subscription uses the API key authenticated on its WebSocket session.
    /// </summary>
    public bool UsesSessionAuthentication => request.UsesSessionAuthentication;

    /// <summary>
    /// Underlying socket subscription, used to observe connection lifecycle events.
    /// </summary>
    public WebSocketUpdateSubscription SocketSubscription { get; }

    internal BinanceSpotUserDataStreamSubscription(BinanceSpotUserDataStreamRequest request, WebSocketUpdateSubscription socketSubscription)
    {
        this.request = request;
        SocketSubscription = socketSubscription;
    }
}
