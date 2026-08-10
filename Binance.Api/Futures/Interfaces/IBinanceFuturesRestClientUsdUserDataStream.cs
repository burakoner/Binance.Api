namespace Binance.Api.Futures;

/// <summary>
/// Binance USD-M futures User Data Stream endpoints
/// </summary>
public interface IBinanceFuturesRestClientUsdUserDataStream
{
    /// <summary>
    /// Start a user stream. The resulting listen key can be used to subscribe to the user stream using the socket client. The stream will close after 60 minutes unless <see cref="KeepAliveUserStreamAsync">KeepAliveUserStreamAsync</see> is called. If the account already has an active listen key, it is returned and extended for another 60 minutes.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/user-data-streams#start-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The active listen key</returns>
    Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Keep alive the user stream. Binance recommends sending a keepalive about every 60 minutes to prevent the stream from timing out.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/user-data-streams#keepalive-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The listen key that was kept alive</returns>
    Task<RestCallResult<string>> KeepAliveUserStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Stop the user stream; no further updates will be sent.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/user-data-streams#close-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether the request completed successfully</returns>
    Task<RestCallResult<bool>> StopUserStreamAsync(CancellationToken ct = default);
}
