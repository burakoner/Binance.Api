namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Futures USD Rest API client.
/// </summary>
public interface IBinanceRestApiClientFuturesUsd :
    IBinanceRestApiClientFuturesUsdAccount,
    IBinanceRestApiClientFuturesUsdConvert,
    IBinanceRestApiClientFuturesUsdMarketData,
    IBinanceRestApiClientFuturesUsdPortfolioMargin,
    IBinanceRestApiClientFuturesUsdTrade,
    IBinanceRestApiClientFuturesUserDataStream;
