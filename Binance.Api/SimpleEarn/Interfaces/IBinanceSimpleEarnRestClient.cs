namespace Binance.Api.SimpleEarn;

/// <summary>
/// Interface for the Binance Simple Earn Rest API client.
/// </summary>
public interface IBinanceSimpleEarnRestClient
{
    /// <summary>
    /// Interface for the Binance Flexible Simple Earn Rest API client.
    /// </summary>
    IBinanceSimpleEarnRestClientFlexible Flexible { get; }

    /// <summary>
    /// Interface for the Binance Locked Simple Earn Rest API client.
    /// </summary>
    IBinanceSimpleEarnRestClientLocked Locked { get; }
}