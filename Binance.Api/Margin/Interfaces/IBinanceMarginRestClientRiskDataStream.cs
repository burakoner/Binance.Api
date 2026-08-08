namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Risk Data Stream Methods
/// </summary>
public interface IBinanceMarginRestClientRiskDataStream
{
    /// <summary>
    /// Starts or refreshes the active Cross Margin risk data stream.
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The listen key used to connect to the risk stream</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/user-data-stream#start-user-data-stream" /></remarks>
    Task<RestCallResult<string>> StartRiskDataStreamAsync(CancellationToken ct = default);

    /// <summary>
    /// Extends a Cross Margin risk data stream listen key for another 60 minutes.
    /// </summary>
    /// <param name="listenKey">Listen key to extend</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether Binance accepted the keepalive</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/user-data-stream#keepalive-user-data-stream" /></remarks>
    Task<RestCallResult<bool>> KeepAliveRiskDataStreamAsync(string listenKey, CancellationToken ct = default);

    /// <summary>
    /// Closes the active Cross Margin risk data stream.
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether Binance accepted the close request</returns>
    /// <remarks>The current formal endpoint schema and generated connector define no request parameters.
    /// <a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/user-data-stream#close-user-data-stream" /></remarks>
    Task<RestCallResult<bool>> CloseRiskDataStreamAsync(CancellationToken ct = default);
}
