namespace Binance.Api.Margin;

/// <summary>
/// Binance Margin listen-token user data stream methods.
/// </summary>
public interface IBinanceMarginRestClientUserDataStream
{
    /// <summary>
    /// Creates a listen token for a cross or isolated Margin user data stream.
    /// Calling this method again before expiry creates the replacement token used to extend the WebSocket subscription.
    /// </summary>
    /// <param name="symbol">Isolated Margin symbol; required when <paramref name="isIsolated"/> is true</param>
    /// <param name="isIsolated">True for an isolated Margin stream; omitted or false for cross Margin</param>
    /// <param name="validity">Validity in milliseconds; default and maximum are 24 hours</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The listen token and its expiration time</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/products/margin-trading/listen-token-data-stream#http-request--post-sapiv1userlistentoken" /></remarks>
    Task<RestCallResult<BinanceMarginListenToken>> CreateUserDataStreamAsync(
        string? symbol = null,
        bool? isIsolated = null,
        long? validity = null,
        CancellationToken ct = default);
}
