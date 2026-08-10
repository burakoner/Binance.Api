namespace Binance.Api.Futures;

/// <summary>
/// Binance USD-M futures User Data Stream endpoints
/// </summary>
public interface IBinanceFuturesRestClientUsdUserDataStream
{
    /// <summary>
    /// Start a user stream. The resulting listen key can be used to subscribe to the user stream using the socket client. The stream will close after 60 minutes unless <see cref="KeepAliveUserStreamAsync">KeepAliveUserStreamAsync</see> is called.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/user-data-streams#start-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Keep alive the user stream. This should be called every 30 minutes to prevent the user stream being stopped
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/user-data-streams#keepalive-user-data-stream" /></para>
    /// </summary>
    /// <param name="listenKey">The listen key to keep alive</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default);

    /// <summary>
    /// Stop the user stream, no updates will be send anymore
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/user-data-streams#close-user-data-stream" /></para>
    /// </summary>
    /// <param name="listenKey">The listen key to stop</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> StopUserStreamAsync(string listenKey, CancellationToken ct = default);
}
