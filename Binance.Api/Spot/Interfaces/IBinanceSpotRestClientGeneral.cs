namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot REST API Client General Methods
/// </summary>
public interface IBinanceSpotRestClientGeneral
{
    /// <summary>
    /// Pings the Binance API
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#test-connectivity" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if successful ping, false if no response</returns>
    Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default);

    /// <summary>
    /// Requests the server for the local time
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#check-server-time" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Server time</returns>
    Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get data for, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="status">Filter by symbol status, Trading, Halt or Break</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinanceSpotSymbolStatus status, CancellationToken ct = default);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="permission">Permission Type</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(BinancePermissionType permission, CancellationToken ct = default);

    /// <summary>
    /// Gets information about the exchange including rate limits and symbol list
    /// <para><a href="https://developers.binance.com/docs/binance-spot-api-docs/rest-api/general-endpoints#exchange-information" /></para>
    /// </summary>
    /// <param name="symbols">Symbols to get data for, for example `ETHUSDT`</param>
    /// <param name="status">Filter by symbol status, Trading, Halt or Break</param>
    /// <param name="permissions">Permission Types</param>
    /// <param name="showPermissionSets">Whether or not permission sets should be returned</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Exchange Info</returns>
    Task<RestCallResult<BinanceSpotExchangeInfo>> GetExchangeInfoAsync(IEnumerable<string> symbols, BinanceSpotSymbolStatus? status = null, IEnumerable<BinancePermissionType>? permissions = null, bool? showPermissionSets = null, CancellationToken ct = default);

    /// <summary>
    /// Gets execution rules for all Spot symbols.
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Execution rules grouped by symbol</returns>
    /// <remarks><a href="https://developers.binance.com/en/docs/catalog/core-trading-spot-trading/api/rest-api/general#execution-rules" /></remarks>
    Task<RestCallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets execution rules for one Spot symbol.
    /// </summary>
    /// <param name="symbol">Symbol to query</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Execution rules grouped by symbol</returns>
    Task<RestCallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets execution rules for multiple Spot symbols.
    /// </summary>
    /// <param name="symbols">Symbols to query</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Execution rules grouped by symbol</returns>
    Task<RestCallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(IEnumerable<string> symbols, CancellationToken ct = default);

    /// <summary>
    /// Gets execution rules for all symbols with the supplied status.
    /// </summary>
    /// <param name="status">Current Spot symbol status</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Execution rules grouped by symbol</returns>
    Task<RestCallResult<BinanceSpotExecutionRules>> GetExecutionRulesAsync(BinanceSpotSymbolStatus status, CancellationToken ct = default);
}
