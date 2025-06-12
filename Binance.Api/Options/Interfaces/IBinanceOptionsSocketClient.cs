namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options Web Socket API Client
/// </summary>
public interface IBinanceOptionsSocketClient :
    IBinanceOptionsSocketClientStreamMarketData,
    IBinanceOptionsSocketClientStreamUserDataStream
{
    /// <summary>
    /// Unsubscribes from a stream. This will close the socket connection and unsubscribe from the stream.
    /// </summary>
    /// <param name="subscription">WebSocket Update Subscription</param>
    /// <param name="force">Force Close Connection</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task UnsubscribeAsync(WebSocketUpdateSubscription subscription, bool force = false, CancellationToken ct = default);

    /// <summary>
    /// Unsubscribes from a stream. This will close the socket connection and unsubscribe from the stream.
    /// </summary>
    /// <param name="subscriptionId">WebSocket Update Subscription Id</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task UnsubscribeAsync(int subscriptionId, CancellationToken ct = default);

    /// <summary>
    /// Unsubscribes from all streams. This will close the socket connection and unsubscribe from all streams.
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task UnsubscribeAllAsync(CancellationToken ct = default);
}

