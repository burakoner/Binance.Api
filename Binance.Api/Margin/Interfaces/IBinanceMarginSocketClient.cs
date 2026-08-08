namespace Binance.Api.Margin;

/// <summary>
/// Binance Margin WebSocket API client.
/// </summary>
public interface IBinanceMarginSocketClient
{
    /// <summary>
    /// Subscribes to a Margin user data stream using a REST-issued listen token.
    /// </summary>
    /// <param name="listenToken">Token returned by <see cref="IBinanceMarginRestClientUserDataStream.CreateUserDataStreamAsync"/></param>
    /// <param name="onOrderUpdated">Handler for order execution updates</param>
    /// <param name="onOrderListUpdated">Handler for order-list updates</param>
    /// <param name="onAccountUpdated">Handler for account position updates</param>
    /// <param name="onBalanceUpdated">Handler for balance updates</param>
    /// <param name="onUserDataStreamTerminated">Handler invoked when the token subscription expires</param>
    /// <param name="ct">Cancellation token for closing the subscription</param>
    /// <returns>The confirmed Margin subscription</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/products/margin-trading/listen-token-data-stream#websocket-api-method--userdatastreamsubscribelistentoken" /></remarks>
    Task<CallResult<BinanceMarginUserDataStreamSubscription>> SubscribeToUserDataStreamAsync(
        string listenToken,
        Action<WebSocketDataEvent<BinanceMarginStreamOrderUpdate>>? onOrderUpdated = null,
        Action<WebSocketDataEvent<BinanceMarginStreamOrderListUpdate>>? onOrderListUpdated = null,
        Action<WebSocketDataEvent<BinanceMarginStreamPositionsUpdate>>? onAccountUpdated = null,
        Action<WebSocketDataEvent<BinanceMarginStreamBalanceUpdate>>? onBalanceUpdated = null,
        Action<WebSocketDataEvent<BinanceMarginStreamUpdate>>? onUserDataStreamTerminated = null,
        CancellationToken ct = default);

    /// <summary>
    /// Extends an active subscription with a newly issued replacement listen token.
    /// </summary>
    /// <param name="subscription">Existing subscription on the target WebSocket connection</param>
    /// <param name="listenToken">New token returned by the REST create operation</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The updated subscription</returns>
    Task<CallResult<BinanceMarginUserDataStreamSubscription>> ExtendUserDataStreamAsync(
        BinanceMarginUserDataStreamSubscription subscription,
        string listenToken,
        CancellationToken ct = default);

    /// <summary>
    /// Unsubscribes one Margin user data stream.
    /// </summary>
    /// <param name="subscription">Subscription to terminate</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether Binance confirmed the unsubscribe request</returns>
    Task<CallResult<bool>> UnsubscribeFromUserDataStreamAsync(BinanceMarginUserDataStreamSubscription subscription, CancellationToken ct = default);

    /// <summary>
    /// Unsubscribes all Margin user data streams on the connection containing the supplied subscription.
    /// </summary>
    /// <param name="sessionSubscription">Any Margin user data subscription on the target connection</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether Binance confirmed the unsubscribe request</returns>
    Task<CallResult<bool>> UnsubscribeAllUserDataStreamsAsync(BinanceMarginUserDataStreamSubscription sessionSubscription, CancellationToken ct = default);
}
