namespace Binance.Api.Futures;

/// <summary>
/// Binance USD Futures General Web Socket Query API
/// </summary>
public interface IBinanceFuturesSocketClientUsdQueryGeneral
{
    /// <summary>
    /// Pings the Binance Futures API
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if successful ping, false if no response</returns>
    Task<CallResult<TimeSpan>> PingAsync(CancellationToken ct = default);

    /// <summary>
    /// Requests the server for the local time. This function also determines the offset between server and local time and uses this for subsequent API calls
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Server time</returns>
    Task<CallResult<DateTime>> GetTimeAsync(CancellationToken ct = default);
}