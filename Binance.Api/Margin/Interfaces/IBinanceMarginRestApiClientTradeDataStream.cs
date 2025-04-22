namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Trade Data Stream Methods
/// </summary>
public interface IBinanceMarginRestApiClientTradeDataStream
{
    /// <summary>
    /// Starts a user stream by requesting a listen key. This listen key can be used in subsequent requests to BinanceSocketClient.SpotApi.Account..SubscribeToUserDataUpdates. The stream will close after 60 minutes unless a keep alive is send.
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade-data-stream/Start-Margin-User-Data-Stream" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Listen key</returns>
    Task<RestCallResult<string>> StartMarginUserStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Sends a keep alive for the current user stream listen key to keep the stream from closing. Stream auto closes after 60 minutes if no keep alive is send. 30 minute interval for keep alive is recommended.
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade-data-stream/Keepalive-Margin-User-Data-Stream" /></para>
    /// </summary>
    /// <param name="listenKey">The listen key to keep alive</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> KeepAliveMarginUserStreamAsync(string listenKey, CancellationToken ct = default);

    /// <summary>
    /// Stops the current user stream
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade-data-stream/Close-Margin-User-Data-Stream" /></para>
    /// </summary>
    /// <param name="listenKey">The listen key to keep alive</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> StopMarginUserStreamAsync(string listenKey, CancellationToken ct = default);

    /// <summary>
    /// Starts a user stream  for margin account by requesting a listen key. 
    /// This listen key can be used in subsequent requests to  BinanceSocketClient.SpotApi.Account.SubscribeToUserDataUpdates  
    /// The stream will close after 60 minutes unless a keep alive is send.
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade-data-stream/Start-Isolated-Margin-User-Data-Stream" /></para>
    /// </summary>
    /// <param name="symbol">The isolated symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Listen key</returns>
    Task<RestCallResult<string>> StartIsolatedMarginUserStreamAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Sends a keep alive for the current user stream for margin account listen key to keep the stream from closing. 
    /// Stream auto closes after 60 minutes if no keep alive is send. 30 minute interval for keep alive is recommended.
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade-data-stream/Keepalive-Isolated-Margin-User-Data-Stream" /></para>
    /// </summary>
    /// <param name="symbol">The isolated symbol, for example `ETHUSDT`</param>
    /// <param name="listenKey">The listen key to keep alive</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> KeepAliveIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default);

    /// <summary>
    /// Close the user stream for margin account
    /// <para><a href="https://developers.binance.com/docs/margin_trading/trade-data-stream/Close-Isolated-Margin-User-Data-Stream" /></para>
    /// </summary>
    /// <param name="symbol">The isolated symbol, for example `ETHUSDT`</param>
    /// <param name="listenKey">The listen key to keep alive</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> CloseIsolatedMarginUserStreamAsync(string symbol, string listenKey, CancellationToken ct = default);
}