namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options REST API Client User Data Stream Methods
/// </summary>
public interface IBinanceOptionsRestClientUserDataStream
{
    /// <summary>
    /// Start a user stream. The resulting listen key can be used to subscribe to the user stream using the socket client. The stream will close after 60 minutes unless <see cref="KeepAliveUserStreamAsync">KeepAliveUserStreamAsync</see> is called. If the account already has an active listen key, it is returned and extended for another 60 minutes.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/user-data-streams#start-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The active listen key and its expiration timestamp in milliseconds</returns>
    Task<RestCallResult<BinanceOptionsListenKey>> StartUserStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Keep alive the active user stream before its 60-minute timeout.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/user-data-streams#keepalive-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether the request completed successfully</returns>
    Task<RestCallResult<bool>> KeepAliveUserStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Stop the active user stream; no further updates will be sent.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/user-data-streams#close-user-data-stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether the request completed successfully</returns>
    Task<RestCallResult<bool>> StopUserStreamAsync(CancellationToken ct = default);
}
