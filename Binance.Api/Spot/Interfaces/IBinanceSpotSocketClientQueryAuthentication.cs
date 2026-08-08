namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot Web Socket API Client Authentication Methods
/// </summary>
public interface IBinanceSpotSocketClientQueryAuthentication
{
    /// <summary>
    /// Opens a WebSocket API connection and authenticates it with the configured Ed25519 API key.
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The authenticated, connection-scoped session</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/auth#session-logon" /></remarks>
    Task<CallResult<BinanceSpotWebSocketSession>> LogonAsync(decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Authenticates an existing WebSocket API session with the currently configured Ed25519 API key.
    /// Calling this again changes the API key authenticated on that connection.
    /// </summary>
    /// <param name="session">Connection-scoped session to authenticate</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The updated session status</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/auth#session-logon" /></remarks>
    Task<CallResult<BinanceSpotWebSocketSessionStatus>> LogonAsync(BinanceSpotWebSocketSession session, decimal? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Queries the authentication and user-data-stream status of a WebSocket API session.
    /// </summary>
    /// <param name="session">Connection-scoped session to query</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The current session status</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/auth#session-status" /></remarks>
    Task<CallResult<BinanceSpotWebSocketSessionStatus>> GetSessionStatusAsync(BinanceSpotWebSocketSession session, CancellationToken ct = default);

    /// <summary>
    /// Forgets the API key authenticated on a WebSocket API session without closing the connection.
    /// </summary>
    /// <param name="session">Connection-scoped session to log out</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The logged-out session status</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/auth#session-logout" /></remarks>
    Task<CallResult<BinanceSpotWebSocketSessionStatus>> LogoutAsync(BinanceSpotWebSocketSession session, CancellationToken ct = default);
}
