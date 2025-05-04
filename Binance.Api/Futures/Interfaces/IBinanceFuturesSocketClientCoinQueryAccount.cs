namespace Binance.Api.Futures;

/// <summary>
/// Binance Coin Futures Account Web Socket Query API
/// </summary>
public interface IBinanceFuturesSocketClientCoinQueryAccount
{
    /// <summary>
    /// Gets account balances
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/websocket-api" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The account information</returns>
    Task<CallResult<IEnumerable<BinanceFuturesCoinAccountBalance>>> GetBalancesAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get account information, including position and balances
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/websocket-api/Account-Information" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    Task<CallResult<BinanceFuturesCoinAccountInfo>> GetAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default);
}