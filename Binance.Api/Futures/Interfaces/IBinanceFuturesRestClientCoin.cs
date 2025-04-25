namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Futures Coin Rest API client.
/// </summary>
public interface IBinanceFuturesRestClientCoin:
    IBinanceFuturesRestClientCoinAccount,
    IBinanceFuturesRestClientCoinMarketData,
    IBinanceFuturesRestClientCoinPortfolioMargin,
    IBinanceFuturesRestClientCoinTrade,
    IBinanceFuturesRestClientCoinUserDataStream;
