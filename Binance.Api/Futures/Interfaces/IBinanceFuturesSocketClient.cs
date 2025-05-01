namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Futures Socket API client.
/// </summary>
public interface IBinanceFuturesSocketClient
{
    /// <summary>
    /// Interface for the Binance Coin-M Futures Socket API client.
    /// </summary>
    IBinanceFuturesSocketClientCoin Coin { get; }

    /// <summary>
    /// Interface for the Binance USDⓈ-M Futures Socket API client.
    /// </summary>
    IBinanceFuturesSocketClientUsd USD { get; }

    // TODO: Futures Data
}