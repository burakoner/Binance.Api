namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Futures Rest API client.
/// </summary>
public interface IBinanceFuturesRestClient
{
    /// <summary>
    /// Interface for the Binance Coin-M Futures Rest API client.
    /// </summary>
    IBinanceFuturesRestClientCoin Coin { get; }

    /// <summary>
    /// Interface for the Binance USDⓈ-M Futures  Rest API client.
    /// </summary>
    IBinanceFuturesRestClientUsd USD { get; }

    /// <summary>
    /// Interface for the Binance Futures Data Rest API client.
    /// </summary>
    IBinanceFuturesRestClientData Data { get; }
}