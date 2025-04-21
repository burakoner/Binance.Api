namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot REST API Client User Data Stream Methods
/// </summary>
public interface IBinanceSpotRestApiClientUserDataStream
{
    [Obsolete("[!IMPORTANT] " +
        "These requests have been deprecated, which means we will remove them in the future. Please subscribe to the User Data Stream through the WebSocket API instead. " +
        "https://developers.binance.com/docs/binance-spot-api-docs/rest-api/user-data-stream-endpoints-deprecated")]
    Task<RestCallResult<string>> StartUserStreamAsync(CancellationToken ct = default);

    [Obsolete("[!IMPORTANT] " +
        "These requests have been deprecated, which means we will remove them in the future. Please subscribe to the User Data Stream through the WebSocket API instead. " +
        "https://developers.binance.com/docs/binance-spot-api-docs/rest-api/user-data-stream-endpoints-deprecated")]
    Task<RestCallResult<bool>> KeepAliveUserStreamAsync(string listenKey, CancellationToken ct = default);

    [Obsolete("[!IMPORTANT] " +
        "These requests have been deprecated, which means we will remove them in the future. Please subscribe to the User Data Stream through the WebSocket API instead. " +
        "https://developers.binance.com/docs/binance-spot-api-docs/rest-api/user-data-stream-endpoints-deprecated")]
    Task<RestCallResult<bool>> StopUserStreamAsync(string listenKey, CancellationToken ct = default);
}