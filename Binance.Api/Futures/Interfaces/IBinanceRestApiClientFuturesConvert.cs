namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Futures USD Convert endpoints
/// </summary>
public interface IBinanceRestApiClientFuturesConvert
{
    /// <summary>
    /// Get list of convert symbols
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/convert" /></para>
    /// </summary>
    /// <param name="fromAsset">From asset</param>
    /// <param name="toAsset">To asset</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<IEnumerable<BinanceFuturesConvertSymbol>>> GetConvertSymbolsAsync(string? fromAsset = null, string? toAsset = null, CancellationToken ct = default);
}