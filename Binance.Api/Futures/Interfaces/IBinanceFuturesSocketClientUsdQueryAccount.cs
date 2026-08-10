namespace Binance.Api.Futures;

/// <summary>
/// Binance USDⓈ-M Futures WebSocket API Account endpoints
/// </summary>
public interface IBinanceFuturesSocketClientUsdQueryAccount
{
    /// <summary>
    /// Gets account balances using the current v1 response contract
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/account#futures-account-balance" /></para>
    /// </summary>
    /// <remarks>Binance announced future deprecation without a removal date in 2024; the endpoint remains in the current catalog. Prefer v2 unless the v1 contract is required.</remarks>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The v1 account balances</returns>
    Task<CallResult<List<BinanceFuturesUsdAccountBalance>>> GetBalancesV1Async(long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets account balances using the current v2 response contract
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/account#futures-account-balance-v2" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The v2 account balances</returns>
    Task<CallResult<List<BinanceFuturesUsdAccountBalance>>> GetBalancesAsync(long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets v1 account information, including all market positions and balances
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/account#account-information" /></para>
    /// </summary>
    /// <remarks>Binance announced future deprecation without a removal date in 2024; the endpoint remains in the current catalog. Prefer v2 unless the full v1 contract is required.</remarks>
    /// <param name="receiveWindow">The receive window for which this request is active</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The v1 WebSocket account information, which shares Binance's REST-v2 response shape</returns>
    Task<CallResult<BinanceFuturesAccountInfoV2>> GetAccountV1Async(long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets v2 account information, including active positions and balances
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/ws-api/account#account-information-v2" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The v2 account information</returns>
    Task<CallResult<BinanceFuturesAccountInfo>> GetAccountAsync(long? receiveWindow = null, CancellationToken ct = default);
}
