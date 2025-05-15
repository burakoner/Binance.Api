namespace Binance.Api.Broker;

/// <summary>
/// Interface for the Binance Link and Trade Rest API client.
/// </summary>
public interface IBinanceBrokerRestClientLinkAndTrade :
    IBinanceBrokerRestClientLinkAndTradeFastApi,
    IBinanceBrokerRestClientLinkAndTradeFutures,
    IBinanceBrokerRestClientLinkAndTradeSpot;