namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Interface for the Binance Portfolio Margin -> Margin Account REST API Client
/// </summary>
public interface IBinancePortfolioMarginRestClientCrossMargin :
    IBinancePortfolioMarginRestClientCrossMarginAccount,
    IBinancePortfolioMarginRestClientCrossMarginTrading,
    IBinancePortfolioMarginRestClientCrossMarginUserDataStream;