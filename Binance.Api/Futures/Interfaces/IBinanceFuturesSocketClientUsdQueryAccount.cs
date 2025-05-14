namespace Binance.Api.Futures;

/// <summary>
/// Binance USD Futures Account Web Socket Query API
/// </summary>
public interface IBinanceFuturesSocketClientUsdQueryAccount
{
    /// <summary>
    /// Gets account balances
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/account/websocket-api" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The account information</returns>
    Task<CallResult<IEnumerable<BinanceFuturesUsdAccountBalance>>> GetBalancesAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get account information, including position and balances
    /// <para><a href="https://developers.binance.com/docs/derivatives/usds-margined-futures/account/websocket-api/Account-Information-V2" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    Task<CallResult<BinanceFuturesAccountInfoV3>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default);
}