namespace Binance.Api.Convert;

/// <summary>
/// Interface for the Binance Convert Market Data Rest API client.
/// </summary>
public interface IBinanceConvertRestClientMarketData
{
    /// <summary>
    /// Query for all convertible token pairs and the tokens’ respective upper/lower limits
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-convert/api/rest-api/market-data#list-all-convert-pairs" /></para>
    /// </summary>
    /// <param name="fromAsset">Asset the user spends. Either fromAsset or toAsset must be provided.</param>
    /// <param name="toAsset">Asset the user receives. Either fromAsset or toAsset must be provided.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceConvertPair>>> GetPairsAsync(string? fromAsset = null, string? toAsset = null, CancellationToken ct = default);

    /// <summary>
    /// Query for supported asset’s precision information
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-convert/api/rest-api/market-data#query-order-quantity-precision-per-asset" /></para>
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds. The value cannot be greater than 60000.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceConvertAsset>>> GetAssetsAsync(int? receiveWindow = null, CancellationToken ct = default);
}
