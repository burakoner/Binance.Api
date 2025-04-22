namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot REST API Client
/// </summary>
public interface IBinanceSpotRestApiClient : 
    IBinanceSpotRestApiClientAccount,
    IBinanceSpotRestApiClientGeneral,
    IBinanceSpotRestApiClientMarketData,
    IBinanceSpotRestApiClientTrading,
    IBinanceSpotRestApiClientUserDataStream;