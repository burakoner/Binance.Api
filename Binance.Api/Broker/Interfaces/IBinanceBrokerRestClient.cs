namespace Binance.Api.Broker;

/// <summary>
/// Interface for the Binance Broker Rest API client.
/// </summary>
public interface IBinanceBrokerRestClient
{
    /// <summary>
    /// Interface for the Binance Exchange Link Rest API client.
    /// </summary>
    IBinanceBrokerRestClientExchangeLink ExchangeLink { get; }

    /// <summary>
    /// Interface for the Binance Link and Trade Rest API client.
    /// </summary>
    IBinanceBrokerRestClientLinkAndTrade LinkAndTrade { get; }
}