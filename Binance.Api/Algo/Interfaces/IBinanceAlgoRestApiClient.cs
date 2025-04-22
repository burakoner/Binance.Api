namespace Binance.Api.Algo;

/// <summary>
/// Interface for the Binance Algo REST API Client
/// </summary>
public interface IBinanceAlgoRestApiClient
{
    /// <summary>
    /// Futures Algo REST API Client
    /// </summary>
    IBinanceAlgoRestApiClientFutures Futures { get; }

    /// <summary>
    /// Spot Algo REST API Client
    /// </summary>
    IBinanceAlgoRestApiClientSpot Spot { get; }
}
