namespace Binance.Api.PortfolioMargin;

/// <summary>
/// Interface for the Binance Portfolio Margin REST API Client
/// </summary>
public interface IBinancePortfolioMarginRestClient :
    IBinancePortfolioMarginRestClientAccount
{
    /// <summary>
    /// Interface for the Binance Portfolio Margin -> Cross Margin REST API Client
    /// </summary>
    IBinancePortfolioMarginRestClientCrossMargin CrossMargin { get; }

    /// <summary>
    /// Interface for the Binance Portfolio Margin -> Coin-M Futures REST API Client
    /// </summary>
    IBinancePortfolioMarginRestClientCoinFutures CoinFutures { get; }

    /// <summary>
    /// Interface for the Binance Portfolio Margin -> USD-M Futures REST API Client
    /// </summary>
    IBinancePortfolioMarginRestClientUsdFutures UsdFutures { get; }

    /// <summary>
    /// Pings the Binance API
    /// <para><a href="https://developers.binance.com/docs/derivatives/portfolio-margin/market-data" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if successful ping, false if no response</returns>
    Task<RestCallResult<TimeSpan>> PingAsync(CancellationToken ct = default);

    /// <summary>
    /// Requests the server for the local time
    /// <para><a href="https://developers.binance.com/docs/derivatives/portfolio-margin/market-data" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Server time</returns>
    Task<RestCallResult<DateTime>> GetTimeAsync(CancellationToken ct = default);
}