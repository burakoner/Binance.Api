namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Interface for the Binance Portfolio Margin Web Socket API Client
/// </summary>
public interface IBinancePortfolioMarginSocketClient
{
    /// <summary>
    /// Interface for the Binance Portfolio Margin -> Cross Margin Web Socket API Client
    /// </summary>
    IBinancePortfolioMarginSocketClientCrossMargin CrossMargin { get; }

    /// <summary>
    /// Interface for the Binance Portfolio Margin -> Coin-M Futures Web Socket API Client
    /// </summary>
    IBinancePortfolioMarginSocketClientCoinFutures CoinFutures { get; }

    /// <summary>
    /// Interface for the Binance Portfolio Margin -> USD-M Futures Web Socket API Client
    /// </summary>
    IBinancePortfolioMarginSocketClientUsdFutures UsdFutures { get; }

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

