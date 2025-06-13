namespace Binance.Api.Link;

/// <summary>
/// Interface for the Binance Link Rest API client.
/// </summary>
public interface IBinanceLinkRestClient
{
    /// <summary>
    /// Interface for the Binance Exchange Link Rest API client.
    /// </summary>
    IBinanceLinkRestClientExchangeLink ExchangeLink { get; }

    /// <summary>
    /// Interface for the Binance Link and Trade Rest API client.
    /// </summary>
    IBinanceLinkRestClientLinkAndTrade LinkAndTrade { get; }
}