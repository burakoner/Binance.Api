namespace Binance.Api.Futures;

/// <summary>
/// Interface for the Binance Coin Futures Account endpoints
/// </summary>
public interface IBinanceFuturesRestClientCoinAccount
{
    /// <summary>.
    /// Gets account balances
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/Futures-Account-Balance" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The account information</returns>
    Task<RestCallResult<List<BinanceFuturesCoinAccountBalance>>> GetBalancesAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets account commission rates
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/User-Commission-Rate" /></para>
    /// </summary>
    /// <param name="symbol">Symbol, for example `BTCUSD_PERP`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>User commission rate information</returns>
    Task<RestCallResult<BinanceFuturesAccountUserCommissionRate>> GetUserCommissionRateAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets account information, including balances
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/Account-Information" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The account information</returns>
    Task<RestCallResult<BinanceFuturesCoinAccountInfo>> GetAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the default Notional and Leverage Brackets for a pair.
    /// <para><b>Warning:</b> Binance does not recommend this v1 operation because a pair can contain symbols with different brackets. Prefer <see cref="GetBracketsAsync" /> with a specific symbol.</para>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/account#notional-bracket-for-pair" /></para>
    /// </summary>
    /// <param name="pair">The pair to get the default brackets for, for example `BTCUSD`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. Maximum 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Pair-default Notional and Leverage Brackets</returns>
    Task<RestCallResult<List<BinanceFuturesPairBracket>>> GetPairBracketsAsync(string? pair = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the Notional and Leverage Brackets for a symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/account#notional-bracket-for-symbol" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the brackets for, for example `BTCUSD_PERP`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. Maximum 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Symbol-specific Notional and Leverage Brackets</returns>
    Task<RestCallResult<List<BinanceFuturesSymbolBracket>>> GetBracketsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get user's position mode (Hedge Mode or One-way Mode ) on EVERY symbol
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/Get-Current-Position-Mode" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether the request was successful</returns>
    Task<RestCallResult<BinanceFuturesPositionMode>> GetPositionModeAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the income history for the futures account
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/Get-Income-History" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get income history from, for example `BTCUSD_PERP`</param>
    /// <param name="incomeType">The income type filter to apply to the request</param>
    /// <param name="startTime">Time to start getting income history from</param>
    /// <param name="endTime">Time to stop getting income history from</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The income history for the futures account</returns>
    Task<RestCallResult<List<BinanceFuturesIncomeHistory>>> GetIncomeHistoryAsync(string? symbol = null, string? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get download id for downloading transaction history
    /// <para>Binance limits the requested range to one year, these history-download requests to eight per month across the website and REST API, and this endpoint to two calls per minute; a third call in the same minute triggers an IP ban.</para>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/account#get-download-id-for-futures-transaction-history" /></para>
    /// </summary>
    /// <param name="startTime">Start time of the data to download</param>
    /// <param name="endTime">End time of the data to download</param>
    /// <param name="receiveWindow">The receive window for which this request is active. Maximum 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTransactionHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the download link for transaction history by download id
    /// <para>The returned download link expires after seven days.</para>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/account#get-futures-transaction-history-download-link-by-id" /></para>
    /// </summary>
    /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForTransactionHistoryAsync" /></param>
    /// <param name="receiveWindow">The receive window for which this request is active. Maximum 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTransactionHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get download id for downloading order history
    /// <para>Binance limits the requested range to one year, these history-download requests to eight per month across the website and REST API, and this endpoint to two calls per minute; a third call in the same minute triggers an IP ban.</para>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/account#get-download-id-for-futures-order-history" /></para>
    /// </summary>
    /// <param name="startTime">Start time of the data to download</param>
    /// <param name="endTime">End time of the data to download</param>
    /// <param name="receiveWindow">The receive window for which this request is active. Maximum 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForOrderHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the download link for order history by download id
    /// <para>The returned download link expires after seven days.</para>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/account#get-futures-order-history-download-link-by-id" /></para>
    /// </summary>
    /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForOrderHistoryAsync" /></param>
    /// <param name="receiveWindow">The receive window for which this request is active. Maximum 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForOrderHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get download id for downloading trade history
    /// <para>Binance limits the requested range to one year, these history-download requests to eight per month across the website and REST API, and this endpoint to two calls per minute; a third call in the same minute triggers an IP ban.</para>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/account#get-download-id-for-futures-trade-history" /></para>
    /// </summary>
    /// <param name="startTime">Start time of the data to download</param>
    /// <param name="endTime">End time of the data to download</param>
    /// <param name="receiveWindow">The receive window for which this request is active. Maximum 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTradeHistoryAsync(DateTime startTime, DateTime endTime, long? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the download link for trade history by download id
    /// <para>The returned download link expires after seven days.</para>
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-coin-m-futures/api/rest-api/account#get-futures-trade-download-link-by-id" /></para>
    /// </summary>
    /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForTradeHistoryAsync" /></param>
    /// <param name="receiveWindow">The receive window for which this request is active. Maximum 60000 milliseconds</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTradeHistoryAsync(string downloadId, long? receiveWindow = null, CancellationToken ct = default);
}
