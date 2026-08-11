namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options Web Socket API Client User Data Stream Methods
/// </summary>
public interface IBinanceOptionsSocketClientStreamUserDataStream
{
    /// <summary>
    /// Subscribes to the Options user data stream.
    /// <para><a href="https://developers.binance.com/en/docs/products/derivatives-trading-options/user-data-streams" /></para>
    /// </summary>
    /// <param name="listenKey">Listen Key</param>
    /// <param name="onAccountUpdated"> On Account Update Event Handler</param>
    /// <param name="onOrderUpdated"> On Order Update Event Handler</param>
    /// <param name="onRiskLevelUpdated"> On Risk Level Change Event Handler</param>
    /// <param name="onBalancePositionUpdated">On Balance and Position Update Event Handler</param>
    /// <param name="onGreekUpdated">On Greek Update Event Handler</param>
    /// <param name="onListenKeyExpired">On Listen Key Expired Event Handler</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUserDataStreamAsync(
       string listenKey,
       Action<WebSocketDataEvent<BinanceOptionsStreamAccount>>? onAccountUpdated = null,
       Action<WebSocketDataEvent<BinanceOptionsStreamOrder>>? onOrderUpdated = null,
       Action<WebSocketDataEvent<BinanceOptionsStreamRiskLevel>>? onRiskLevelUpdated = null,
       Action<WebSocketDataEvent<BinanceOptionsStreamBalancePosition>>? onBalancePositionUpdated = null,
       Action<WebSocketDataEvent<BinanceOptionsStreamGreek>>? onGreekUpdated = null,
       Action<WebSocketDataEvent<BinanceOptionsStreamListenKeyExpired>>? onListenKeyExpired = null,
       CancellationToken ct = default);
}
