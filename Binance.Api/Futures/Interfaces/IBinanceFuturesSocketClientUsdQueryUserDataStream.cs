namespace Binance.Api.Futures;

/// <summary>
/// Binance USDⓈ-M Futures User Data Stream WebSocket API
/// </summary>
public interface IBinanceFuturesSocketClientUsdQueryUserDataStream
{
    /// <summary>
    /// Starts or extends the user data stream associated with the configured API key
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/user-data-streams#start-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The listen key for the user data stream</returns>
    Task<CallResult<string>> StartUserDataStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Extends the user data stream associated with the configured API key for another 60 minutes
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/user-data-streams#keepalive-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The listen key that was kept alive</returns>
    Task<CallResult<string>> KeepAliveUserDataStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Stops the user data stream associated with the configured API key
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/user-data-streams#close-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether the user data stream was stopped successfully</returns>
    Task<CallResult<bool>> StopUserDataStreamAsync(CancellationToken ct = default);
}
