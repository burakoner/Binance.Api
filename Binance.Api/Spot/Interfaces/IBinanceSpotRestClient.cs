namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot REST API Client
/// </summary>
public interface IBinanceSpotRestClient : 
    IBinanceSpotRestClientAccount,
    IBinanceSpotRestClientGeneral,
    IBinanceSpotRestClientMarketData,
    IBinanceSpotRestClientTrading,
    IBinanceSpotRestClientUserDataStream;