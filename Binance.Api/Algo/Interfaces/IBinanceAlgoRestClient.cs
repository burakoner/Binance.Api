namespace Binance.Api.Algo;

/// <summary>
/// Interface for the Binance Algo REST API Client
/// </summary>
public interface IBinanceAlgoRestClient
{
    /// <summary>
    /// Futures Algo REST API Client
    /// </summary>
    IBinanceAlgoRestClientFutures Futures { get; }

    /// <summary>
    /// Spot Algo REST API Client
    /// </summary>
    IBinanceAlgoRestClientSpot Spot { get; }
}
