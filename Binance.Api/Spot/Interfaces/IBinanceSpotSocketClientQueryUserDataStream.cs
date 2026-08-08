namespace Binance.Api.Spot;

/// <summary>
/// Interface for Binance Spot WebSocket API user data stream methods.
/// </summary>
public interface IBinanceSpotSocketClientQueryUserDataStream
{
    /// <summary>
    /// Subscribes to the current account's user data stream through a signed WebSocket API request.
    /// </summary>
    /// <param name="onOrderUpdated">Handler for order execution updates</param>
    /// <param name="onOrderListUpdated">Handler for order-list updates</param>
    /// <param name="onAccountUpdated">Handler for account position updates</param>
    /// <param name="onBalanceUpdated">Handler for balance updates</param>
    /// <param name="onBalanceLockUpdated">Handler for external balance lock updates</param>
    /// <param name="onUserDataStreamTerminated">Handler invoked when the stream is terminated</param>
    /// <param name="receiveWindow">Request validity window in milliseconds; maximum 60000 with up to three decimal places</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>The confirmed subscription, including Binance's server subscription identifier</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/user-data-stream#user-data-stream-subscribe-signature" /></remarks>
    Task<CallResult<BinanceSpotUserDataStreamSubscription>> SubscribeToUserDataStreamAsync(
        Action<WebSocketDataEvent<BinanceSpotStreamOrderUpdate>>? onOrderUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamOrderListUpdate>>? onOrderListUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamPositionsUpdate>>? onAccountUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamBalanceUpdate>>? onBalanceUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamExternalLockUpdate>>? onBalanceLockUpdated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamUpdate>>? onUserDataStreamTerminated = null,
        decimal? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Unsubscribes one user data stream on its WebSocket API connection.
    /// </summary>
    /// <param name="subscription">The subscription to close</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether Binance confirmed the unsubscribe request</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/user-data-stream#user-data-stream-unsubscribe" /></remarks>
    Task<CallResult<bool>> UnsubscribeFromUserDataStreamAsync(BinanceSpotUserDataStreamSubscription subscription, CancellationToken ct = default);

    /// <summary>
    /// Unsubscribes all user data streams on the WebSocket API connection containing the supplied subscription.
    /// </summary>
    /// <param name="sessionSubscription">A subscription identifying the WebSocket API connection</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether Binance confirmed the unsubscribe request</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/ws-api/user-data-stream#user-data-stream-unsubscribe" /></remarks>
    Task<CallResult<bool>> UnsubscribeAllUserDataStreamsAsync(BinanceSpotUserDataStreamSubscription sessionSubscription, CancellationToken ct = default);
}
