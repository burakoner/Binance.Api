namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options Web Socket API Client User Data Stream Methods
/// </summary>
public interface IBinanceOptionsSocketClientStreamUserDataStream
{
    /// <summary>
    /// Subscribes to the account update stream.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/user-data-streams" /></para>
    /// </summary>
    /// <param name="listenKey">Listen Key</param>
    /// <param name="onAccountUpdated"> On Account Update Event Handler</param>
    /// <param name="onOrderUpdated"> On Order Update Event Handler</param>
    /// <param name="onRiskLevelUpdated"> On Risk Level Change Event Handler</param>
    /// <param name="ct">Cancellation token for closing this subscription</param>
    /// <returns></returns>
    Task<CallResult<WebSocketUpdateSubscription>> SubscribeToUserDataStreamAsync(
       string listenKey,
       Action<WebSocketDataEvent<BinanceOptionsStreamAccount>>? onAccountUpdated = null,
       Action<WebSocketDataEvent<BinanceOptionsStreamOrder>>? onOrderUpdated = null,
       Action<WebSocketDataEvent<BinanceOptionsStreamRiskLevel>>? onRiskLevelUpdated = null,
       CancellationToken ct = default);
}