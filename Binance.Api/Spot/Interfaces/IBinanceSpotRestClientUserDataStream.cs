namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot REST API Client User Data Stream Methods
/// </summary>
public interface IBinanceSpotRestClientUserDataStream
{
    /// <summary>
    /// Starts a user stream by requesting a listen key. This listen key can be used in subsequent requests to SubscribeToUserDataUpdates. The stream will close after 60 minutes unless a keep alive is send.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/user-data-stream#create-a-listenkey-user_stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Listen key</returns>
    [Obsolete("[!IMPORTANT] " +
        "These requests have been deprecated, which means we will remove them in the future. Please subscribe to the User Data Stream through the WebSocket API instead. " +
        "https://developers.binance.com/docs/binance-spot-api-docs/rest-api/user-data-stream-endpoints-deprecated")]
    Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Sends a keep alive for the current user stream listen key to keep the stream from closing. Stream auto closes after 60 minutes if no keep alive is send. 30 minute interval for keep alive is recommended.
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/user-data-stream#pingkeep-alive-a-listenkey-user_stream" /></para>
    /// </summary>
    /// <param name="listenKey">The listen key to keep alive</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    [Obsolete("[!IMPORTANT] " +
        "These requests have been deprecated, which means we will remove them in the future. Please subscribe to the User Data Stream through the WebSocket API instead. " +
        "https://developers.binance.com/docs/binance-spot-api-docs/rest-api/user-data-stream-endpoints-deprecated")]
    Task<RestCallResult<bool>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default);

    /// <summary>
    /// Stops the current user stream
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/user-data-stream#close-a-listenkey-user_stream" /></para>
    /// </summary>
    /// <param name="listenKey">The listen key to keep alive</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    [Obsolete("[!IMPORTANT] " +
        "These requests have been deprecated, which means we will remove them in the future. Please subscribe to the User Data Stream through the WebSocket API instead. " +
        "https://developers.binance.com/docs/binance-spot-api-docs/rest-api/user-data-stream-endpoints-deprecated")]
    Task<RestCallResult<bool>> StopUserStreamAsync(string listenKey, CancellationToken ct = default);
}