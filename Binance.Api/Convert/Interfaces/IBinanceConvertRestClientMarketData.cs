namespace Binance.Api.Convert;

/// <summary>
/// Interface for the Binance Convert Market Data Rest API client.
/// </summary>
public interface IBinanceConvertRestClientMarketData
{
    /// <summary>
    /// Query for all convertible token pairs and the tokens’ respective upper/lower limits
    /// </summary>
    /// <param name="fromAsset">From Asset</param>
    /// <param name="toAsset">To Asset</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<IEnumerable<BinanceConvertPair>>> GetPairsAsync(string? fromAsset = null, string? toAsset = null, CancellationToken ct = default);

    /// <summary>
    /// Query for supported asset’s precision information
    /// </summary>
    /// <param name="receiveWindow">The value cannot be greater than 60000</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<IEnumerable<BinanceConvertAsset>>> GetAssetsAsync(int? receiveWindow = null, CancellationToken ct = default);
}