namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot Web Socket API Client General Methods
/// </summary>
public interface IBinanceSpotSocketClientQueryGeneral
{
    /// <summary>
    /// Ping to test connection
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/general-requests#test-connectivity" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<TimeSpan>> PingAsync(CancellationToken ct = default);

    /// <summary>
    /// Get the server time
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/general-requests#check-server-time" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<DateTime>> GetTimeAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/general-requests#exchange-information" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/general-requests#exchange-information" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// </summary>
    /// <param name="status">Status</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinanceSymbolStatus status, CancellationToken ct = default);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/general-requests#exchange-information" /></para>
    /// </summary>
    /// <param name="permission">Permission</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinancePermissionType permission, CancellationToken ct = default);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/web-socket-api/general-requests#exchange-information" /></para>
    /// </summary>
    /// <param name="symbols">Filter by symbols, for example `ETHUSDT`</param>
    /// <param name="status">Status</param>
    /// <param name="permissions">Permissions</param>
    /// <param name="showPermissionSets">Show Permission Sets</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<CallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(IEnumerable<string> symbols, BinanceSymbolStatus? status = null, IEnumerable<BinancePermissionType>? permissions = null, bool? showPermissionSets = null, CancellationToken ct = default);
}