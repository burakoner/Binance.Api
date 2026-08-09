namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Futures USD Portfolio Margin endpoints
/// </summary>
public interface IBinanceFuturesRestClientUsdPortfolioMargin
{
    /// <summary>
    /// Get Classic Portfolio Margin current account information.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/portfolio-margin-endpoints#classic-portfolio-margin-account-information" /></para>
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="receiveWindow">The receive window for which this request is active. Maximum 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The Classic Portfolio Margin account information for the asset</returns>
    Task<RestCallResult<BinanceFuturesPortfolioMarginAccount>> GetPortfolioMarginAccountInfoAsync(string asset, int? receiveWindow = null, CancellationToken ct = default);
}
