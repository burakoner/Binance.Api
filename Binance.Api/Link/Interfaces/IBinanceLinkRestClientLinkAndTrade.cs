namespace Binance.Api.Link;

/// <summary>
/// Interface for the Binance Link and Trade Rest API client.
/// </summary>
public interface IBinanceLinkRestClientLinkAndTrade
{
    /// <summary>
    /// Interface for the Binance Link and Trade -> Fast Api Rest API client.
    /// </summary>
    IBinanceLinkRestClientLinkAndTradeFastApi FastApi { get; }

    /// <summary>
    /// Interface for the Binance Link and Trade -> Futures Rest API client.
    /// </summary>
    IBinanceLinkRestClientLinkAndTradeFutures Futures { get; }

    /// <summary>
    /// Interface for the Binance Link and Trade -> Portfolio Margin Rest API client.
    /// </summary>
    IBinanceLinkRestClientLinkAndTradePortfolioMargin PortfolioMargin { get; }

    /// <summary>
    /// Interface for the Binance Link and Trade -> Spot Rest API client.
    /// </summary>
    IBinanceLinkRestClientLinkAndTradeSpot Spot { get; }
}