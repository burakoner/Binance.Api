namespace Binance.Api.Futures;

/// <summary>
/// Binance Coin Futures Account Web Socket Query API
/// </summary>
public interface IBinanceFuturesSocketClientCoinQueryAccount
{
    /// <summary>
    /// Gets account balances
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-api/account#futures-account-balance" /></para>
    /// </summary>
    /// <param name="receiveWindow">The int64 receive window in milliseconds; cannot exceed 60000</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The account information</returns>
    Task<CallResult<List<BinanceFuturesCoinAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get account information, including position and balances
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/ws-api/account#account-information" /></para>
    /// </summary>
    /// <param name="receiveWindow">The int64 receive window in milliseconds; cannot exceed 60000</param>
    /// <param name="ct">Cancellation token</param>
    Task<CallResult<BinanceFuturesCoinAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default);
}
