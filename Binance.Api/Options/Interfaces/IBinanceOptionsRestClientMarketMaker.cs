namespace Binance.Api.Options;

/// <summary>
/// Interface for the Binance Options Market Maker REST API Client
/// </summary>
public interface IBinanceOptionsRestClientMarketMaker :
    IBinanceOptionsRestClientMarketMakerAccount,
    IBinanceOptionsRestClientMarketMakerBlockTrade;