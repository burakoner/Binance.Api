namespace Binance.Api.Futures;

/// <summary>
/// Binance Coin Futures User Data Stream API
/// </summary>
public interface IBinanceFuturesSocketClientCoinStreamUserData
{
    /// <summary>
    /// Subscribes to the account update stream. Prior to using this, the <see cref="IBinanceFuturesRestClientCoinUserDataStream.StartUserStreamAsync(CancellationToken)">restClient.CoinFuturesApi.Account.StartUserStreamAsync</see> method should be called to start the stream and obtaining a listen key.
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/user-data-streams" /></para>
    /// </summary>
    /// <param name="listenKey">Listen key retrieved by the <see cref="IBinanceFuturesRestClientCoinUserDataStream.StartUserStreamAsync(CancellationToken)">restClient.CoinFuturesApi.Account.StartUserStreamAsync</see> method</param>
    /// <param name="onLeverageUpdated">The event handler for leverage changed update</param>
    /// <param name="onMarginUpdated">The event handler for whenever a margin has changed</param>
    /// <param name="onAccountUpdated">The event handler for whenever an account update is received</param>
    /// <param name="onOrderUpdated">The event handler for whenever an order status update is received</param>
    /// <param name="onListenKeyExpired">Responds when the listen key for the stream has expired. Initiate a new instance of the stream here</param>
    /// <param name="onStrategyUpdated">The event handler for whenever a strategy update is received</param>
    /// <param name="onGridUpdated">The event handler for whenever a grid update is received</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUserDataUpdatesAsync(
       string listenKey,
       Action<WebSocketDataEvent<BinanceFuturesStreamAccountUpdate>>? onAccountUpdated = null,
       Action<WebSocketDataEvent<BinanceFuturesStreamConfigUpdate>>? onLeverageUpdated = null,
       Action<WebSocketDataEvent<BinanceFuturesStreamMarginUpdate>>? onMarginUpdated = null,
       Action<WebSocketDataEvent<BinanceFuturesStreamOrderUpdate>>? onOrderUpdated = null,

       Action<WebSocketDataEvent<BinanceFuturesStreamStrategyUpdate>>? onStrategyUpdated = null,
       Action<WebSocketDataEvent<BinanceFuturesStreamGridUpdate>>? onGridUpdated = null,
       Action<WebSocketDataEvent<BinanceFuturesStreamUpdate>>? onListenKeyExpired = null,

       CancellationToken ct = default);
}