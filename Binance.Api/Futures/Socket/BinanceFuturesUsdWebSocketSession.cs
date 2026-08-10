namespace Binance.Api.Futures;

/// <summary>
/// A connection-scoped USDⓈ-M Futures WebSocket API session.
/// </summary>
/// <remarks>
/// Binance authentication belongs to the underlying WebSocket connection. Closing or losing that
/// connection invalidates the session until the client successfully authenticates the restored connection.
/// </remarks>
public sealed class BinanceFuturesUsdWebSocketSession
{
    private readonly WebSocketConnection connection;

    /// <summary>
    /// Identifier of the underlying WebSocket connection.
    /// </summary>
    public int ConnectionId => connection.Id;

    /// <summary>
    /// Whether the underlying WebSocket connection is currently open.
    /// </summary>
    public bool Connected => connection.Connected;

    /// <summary>
    /// Whether an API key is currently authenticated on the connection.
    /// </summary>
    public bool Authenticated => Connected && LastStatus.ApiKey != null;

    /// <summary>
    /// Most recently returned session status.
    /// </summary>
    public BinanceFuturesUsdWebSocketSessionStatus LastStatus { get; internal set; }

    /// <summary>
    /// Raised when Binance revokes the authenticated API key on this connection.
    /// </summary>
    public event Action<BinanceFuturesUsdWebSocketSessionRevocation>? AuthenticationRevoked;

    /// <summary>
    /// Raised when the underlying connection is lost.
    /// </summary>
    public event Action ConnectionLost
    {
        add => connection.ConnectionLost += value;
        remove => connection.ConnectionLost -= value;
    }

    /// <summary>
    /// Raised when the underlying connection is closed.
    /// </summary>
    public event Action ConnectionClosed
    {
        add => connection.ConnectionClosed += value;
        remove => connection.ConnectionClosed -= value;
    }

    /// <summary>
    /// Raised after the underlying connection is restored. While session authentication remains active,
    /// restoration is reported only after re-authentication succeeds. After logout or API-key revocation,
    /// only the connection is restored.
    /// </summary>
    public event Action<TimeSpan> ConnectionRestored
    {
        add => connection.ConnectionRestored += value;
        remove => connection.ConnectionRestored -= value;
    }

    internal WebSocketConnection Connection => connection;
    internal WebSocketSubscription LifecycleSubscription { get; }
    internal bool AuthenticationRequired => LifecycleSubscription.Authenticated;
    internal long? ReceiveWindow { get; set; }

    internal BinanceFuturesUsdWebSocketSession(
        WebSocketConnection connection,
        WebSocketSubscription lifecycleSubscription,
        BinanceFuturesUsdWebSocketSessionStatus status,
        long? receiveWindow)
    {
        this.connection = connection;
        LifecycleSubscription = lifecycleSubscription;
        LastStatus = status;
        ReceiveWindow = receiveWindow;
    }

    internal void MarkAuthenticationRevoked(BinanceFuturesUsdWebSocketSessionRevocation revocation)
    {
        MarkAuthenticationPending();
        LifecycleSubscription.Authenticated = false;
        AuthenticationRevoked?.Invoke(revocation);
    }

    internal void MarkAuthenticationPending()
    {
        LastStatus.ApiKey = null;
        LastStatus.AuthorizedSince = null;
    }

    /// <summary>
    /// Closes the underlying WebSocket connection and invalidates the session.
    /// </summary>
    public Task CloseAsync() => connection.CloseAsync();
}
