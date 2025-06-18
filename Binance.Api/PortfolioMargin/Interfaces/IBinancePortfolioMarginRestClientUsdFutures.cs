namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Interface for the Binance Portfolio Margin -> USD-M Futures REST API Client
/// </summary>
public interface IBinancePortfolioMarginRestClientUsdFutures :
    IBinancePortfolioMarginRestClientCoinFuturesAccount,
    IBinancePortfolioMarginRestClientCoinFuturesTrading,
    IBinancePortfolioMarginRestClientCoinFuturesUserDataStream;