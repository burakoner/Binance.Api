namespace Binance.Api.Wallet;

/// <summary>
/// Interface for the Binance Wallet REST API Client Other Methods
/// </summary>
public interface IBinanceWalletRestApiClientOthers
{
    /// <summary>
    /// Gets the status of the Binance platform
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#system-status-system" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The system status</returns>
    Task<RestCallResult<BinanceSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default);

    // TODO: Get symbols delist schedule for spot (MARKET_DATA)
}