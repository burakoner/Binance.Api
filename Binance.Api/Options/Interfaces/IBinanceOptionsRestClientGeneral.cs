namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options REST API Client General Methods
/// </summary>
public interface IBinanceOptionsRestClientGeneral
{
    /// <summary>
    /// Test connectivity to the Rest API.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/market-data/Test-Connectivity" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default);

    /// <summary>
    /// Test connectivity to the Rest API and get the current server time.
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/market-data" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default);

    /// <summary>
    /// Current exchange trading rules and symbol information
    /// <para><a href="https://developers.binance.com/docs/derivatives/option/market-data/Exchange-Information" /></para>
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceOptionsExchangeInfo>> GetExchangeInfoAsync(CancellationToken ct = default);
}