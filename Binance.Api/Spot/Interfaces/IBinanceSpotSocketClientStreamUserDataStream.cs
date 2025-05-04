namespace Binance.Api.Spot;

/// <summary>
/// Binance Spot Accountsubscriptions
/// </summary>
public interface IBinanceSpotSocketClientStreamUserDataStream
{
    /// <summary>
    /// Subscribes to the account update stream. Prior to using this, the <see cref="IBinanceSpotRestClientUserDataStream.StartUserStreamAsync(CancellationToken)">StartUserStreamAsync</see> method should be called to start the stream and obtaining a listen key.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/user-data-stream#web-socket-payloads" /></para>
    /// </summary>
    /// <param name="listenKey">Listen key retrieved by the <see cref="IBinanceSpotRestClientUserDataStream.StartUserStreamAsync(CancellationToken)">StartUserStreamAsync</see> method</param>
    /// <param name="onOrderUpdateMessage">The event handler for whenever an order status update is received</param>
    /// <param name="onOcoOrderUpdateMessage">The event handler for whenever an oco order status update is received</param>
    /// <param name="onAccountPositionMessage">The event handler for whenever an account position update is received. Account position updates are a list of changed funds</param>
    /// <param name="onAccountBalanceUpdate">The event handler for whenever a deposit or withdrawal has been processed and the account balance has changed</param>
    /// <param name="onListenKeyExpired">The event handler for when the listen key has expired. No events will be send anymore after this</param>
    /// <param name="onUserDataStreamTerminated">The event handler for when the User Data Stream is stopped. For example, after you call <see cref="IBinanceSpotRestClientUserDataStream.StopUserStreamAsync(string, CancellationToken)" /></param>
    /// <param name="onBalanceLockUpdate">The event handler for when the part of your spot wallet balance is locked/unlocked by an external system, for example when used as margin collateral. </param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUserDataStreamAsync(
        string listenKey,
        Action<WebSocketDataEvent<BinanceSpotStreamOrderUpdate>>? onOrderUpdateMessage = null,
        Action<WebSocketDataEvent<BinanceSpotStreamOrderListUpdate>>? onOcoOrderUpdateMessage = null,
        Action<WebSocketDataEvent<BinanceSpotStreamPositionsUpdate>>? onAccountPositionMessage = null,
        Action<WebSocketDataEvent<BinanceSpotStreamBalanceUpdate>>? onAccountBalanceUpdate = null,
        Action<WebSocketDataEvent<BinanceSpotStreamBalanceUpdate>>? onBalanceLockUpdate = null,
        Action<WebSocketDataEvent<BinanceSpotStreamUpdate>>? onUserDataStreamTerminated = null,
        Action<WebSocketDataEvent<BinanceSpotStreamUpdate>>? onListenKeyExpired = null,
        CancellationToken ct = default);
}