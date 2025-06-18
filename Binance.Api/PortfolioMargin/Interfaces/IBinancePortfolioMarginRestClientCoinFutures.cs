namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Interface for the Binance Portfolio Margin -> Coin-M Futures REST API Client
/// </summary>
public interface IBinancePortfolioMarginRestClientCoinFutures :
    IBinancePortfolioMarginRestClientCoinFuturesAccount,
    IBinancePortfolioMarginRestClientCoinFuturesTrading,
    IBinancePortfolioMarginRestClientCoinFuturesUserDataStream;