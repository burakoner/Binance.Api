namespace Binance.Api.Broker;

/// <summary>
/// Interface for the Binance Link and Trade Rest API client.
/// </summary>
public interface IBinanceBrokerRestClientLinkAndTrade
{
    /// <summary>
    /// Interface for the Binance Link and Trade -> Fast Api Rest API client.
    /// </summary>
    IBinanceBrokerRestClientLinkAndTradeFastApi FastApi { get; }

    /// <summary>
    /// Interface for the Binance Link and Trade -> Futures Rest API client.
    /// </summary>
    IBinanceBrokerRestClientLinkAndTradeFutures Futures { get; }

    /// <summary>
    /// Interface for the Binance Link and Trade -> Portfolio Margin Rest API client.
    /// </summary>
    IBinanceBrokerRestClientLinkAndTradePortfolioMargin PortfolioMargin { get; }

    /// <summary>
    /// Interface for the Binance Link and Trade -> Spot Rest API client.
    /// </summary>
    IBinanceBrokerRestClientLinkAndTradeSpot Spot { get; }
}