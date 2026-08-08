namespace Binance.Api.Spot;

/// <summary>
/// A connection-scoped Spot WebSocket API session.
/// </summary>
/// <remarks>
/// Binance authentication belongs to the underlying WebSocket connection. Closing or losing that
/// connection invalidates the session.
/// </remarks>
public sealed class BinanceSpotWebSocketSession
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
    /// Most recently returned session status.
    /// </summary>
    public BinanceSpotWebSocketSessionStatus LastStatus { get; internal set; }

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

    internal WebSocketConnection Connection => connection;

    internal BinanceSpotWebSocketSession(WebSocketConnection connection, BinanceSpotWebSocketSessionStatus status)
    {
        this.connection = connection;
        LastStatus = status;
    }

    /// <summary>
    /// Closes the underlying WebSocket connection and invalidates the session.
    /// </summary>
    public Task CloseAsync() => connection.CloseAsync();
}
