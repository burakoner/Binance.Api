namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Futures USD Portfolio Margin endpoints
/// </summary>
public interface IBinanceFuturesRestClientUsdPortfolioMargin
{
    /// <summary>
    /// Get Classic Portfolio Margin current account information.
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesPortfolioMarginAccount>> GetPortfolioMarginAccountInfoAsync(string asset, int? receiveWindow = null, CancellationToken ct = default);
}