namespace Binance.Api.Futures;

/// <summary>
/// USDⓈ-M Futures WebSocket API connection authentication methods.
/// </summary>
public interface IBinanceFuturesSocketClientUsdQueryAuthentication
{
    /// <summary>
    /// Opens a WebSocket API connection and authenticates it with the configured Ed25519 API key.
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The authenticated, connection-scoped session</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/products/derivatives-trading-usds-futures/websocket-api-general-info#log-in-with-api-key-signed" /></remarks>
    Task<CallResult<BinanceFuturesUsdWebSocketSession>> LogonAsync(long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Authenticates an existing WebSocket API session with the currently configured Ed25519 API key.
    /// Calling this again changes the API key authenticated on that connection.
    /// </summary>
    /// <param name="session">Connection-scoped session to authenticate</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The updated session status</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/products/derivatives-trading-usds-futures/websocket-api-general-info#log-in-with-api-key-signed" /></remarks>
    Task<CallResult<BinanceFuturesUsdWebSocketSessionStatus>> LogonAsync(BinanceFuturesUsdWebSocketSession session, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Queries the authentication status and current API key of a WebSocket API session.
    /// </summary>
    /// <param name="session">Connection-scoped session to query</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The current session status</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/products/derivatives-trading-usds-futures/websocket-api-general-info#query-session-status" /></remarks>
    Task<CallResult<BinanceFuturesUsdWebSocketSessionStatus>> GetSessionStatusAsync(BinanceFuturesUsdWebSocketSession session, CancellationToken ct = default);

    /// <summary>
    /// Forgets the API key authenticated on a WebSocket API session without closing the connection.
    /// </summary>
    /// <param name="session">Connection-scoped session to log out</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The logged-out session status</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/products/derivatives-trading-usds-futures/websocket-api-general-info#log-out-of-the-session" /></remarks>
    Task<CallResult<BinanceFuturesUsdWebSocketSessionStatus>> LogoutAsync(BinanceFuturesUsdWebSocketSession session, CancellationToken ct = default);
}
