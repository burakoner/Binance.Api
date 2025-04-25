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
    Task<RestCallResult<IEnumerable<BinanceCoinFuturesAccountBalance>>> GetBalancesAsync(int? receiveWindow = null, CancellationToken ct = default);

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
    /// Gets Notional and Leverage Brackets.
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/Notional-Bracket-for-Pair" /></para>
    /// </summary>
    /// <param name="symbolOrPair">The symbol or pair to get the data for, for example `BTCUSD_PERP`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Notional and Leverage Brackets</returns>
    Task<RestCallResult<IEnumerable<BinanceFuturesSymbolBracket>>> GetBracketsAsync(string? symbolOrPair = null, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Notional Bracket for Pair(USER_DATA)

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
    Task<RestCallResult<IEnumerable<BinanceFuturesIncomeHistory>>> GetIncomeHistoryAsync(string? symbol = null, string? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get download id for downloading transaction history
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/Get-Download-Id-For-Futures-Transaction-History" /></para>
    /// </summary>
    /// <param name="startTime">Start time of the data to download</param>
    /// <param name="endTime">End time of the data to download</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTransactionHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the download link for transaction history by download id
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/Get-Futures-Transaction-History-Download-Link-by-Id" /></para>
    /// </summary>
    /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForTransactionHistoryAsync" /></param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTransactionHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get download id for downloading order history
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/Get-Download-Id-For-Futures-Order-History" /></para>
    /// </summary>
    /// <param name="startTime">Start time of the data to download</param>
    /// <param name="endTime">End time of the data to download</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForOrderHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the download link for order history by download id
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/Get-Futures-Order-History-Download-Link-by-Id" /></para>
    /// </summary>
    /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForOrderHistoryAsync" /></param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForOrderHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get download id for downloading trade history
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/Get-Download-Id-For-Futures-Trade-History" /></para>
    /// </summary>
    /// <param name="startTime">Start time of the data to download</param>
    /// <param name="endTime">End time of the data to download</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTradeHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the download link for order history by download id
    /// <para><a href="https://developers.binance.com/docs/derivatives/coin-margined-futures/account/rest-api/Get-Futures-Trade-Download-Link-by-Id" /></para>
    /// </summary>
    /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForTradeHistoryAsync" /></param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTradeHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default);
}
