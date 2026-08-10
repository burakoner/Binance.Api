namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Coin Futures User Data Stream endpoints
/// </summary>
public interface IBinanceFuturesRestClientCoinUserDataStream
{
    /// <summary>
    /// Starts a user stream. If a stream is already active for the configured API key, its listen key is returned and its validity is extended for 60 minutes. The stream will close after 60 minutes unless <see cref="KeepAliveUserStreamAsync">KeepAliveUserStreamAsync</see> is called.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/user-data-streams#start-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Extends the user stream associated with the configured API key for another 60 minutes
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/user-data-streams#keepalive-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The listen key that was kept alive</returns>
    Task<RestCallResult<string>> KeepAliveUserStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Stops the user stream associated with the configured API key
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/user-data-streams#close-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether the user stream was stopped successfully</returns>
    Task<RestCallResult<bool>> StopUserStreamAsync(CancellationToken ct = default);
}
