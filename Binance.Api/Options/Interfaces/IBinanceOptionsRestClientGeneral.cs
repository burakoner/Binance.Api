namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options REST API Client General Methods
/// </summary>
public interface IBinanceOptionsRestClientGeneral
{
    /// <summary>
    /// Test connectivity to the Rest API.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#test-connectivity" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default);

    /// <summary>
    /// Test connectivity to the Rest API and get the current server time.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#check-server-time" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default);

    /// <summary>
    /// Current exchange trading rules and symbol information
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-options/api/rest-api/market-data#exchange-information" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default);
}
