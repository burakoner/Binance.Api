namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options REST API Client
/// </summary>
public interface IBinanceOptionsRestClient : 
    IBinanceOptionsRestClientAccount,
    IBinanceOptionsRestClientGeneral,
    IBinanceOptionsRestClientMarketData,
    IBinanceOptionsRestClientTrading,
    IBinanceOptionsRestClientUserDataStream
{
    /// <summary>
    /// Binance Options Market Maker REST API Client
    /// </summary>
    IBinanceOptionsRestClientMarketMaker MarketMaker { get; }
}