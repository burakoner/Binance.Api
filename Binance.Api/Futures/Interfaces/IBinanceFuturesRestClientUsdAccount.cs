namespace Binance.Api.Futures;

/// <summary>
/// Binance USD-M futures account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
/// </summary>
public interface IBinanceFuturesRestClientUsdAccount
{
    /// <summary>
    /// Gets account balances using the v3 contract
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#futures-account-balance-v3" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The account information</returns>
    Task<RestCallResult<List<BinanceFuturesUsdAccountBalance>>> GetBalancesAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets account balances using the v2 contract
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#futures-account-balance-v2" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The account information</returns>
    Task<RestCallResult<List<BinanceFuturesUsdAccountBalance>>> GetBalancesV2Async(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get current account information. User in single-asset/ multi-assets mode will see different value, see comments in response section for detail.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#account-information-v3" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesAccountInfo>> GetAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get account information, including position and balances
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#account-information-v2" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    [Obsolete("Please use GetAccountInfoAsync (v3) instead")]
    Task<RestCallResult<BinanceFuturesAccountInfoV2>> GetAccountInfoV2Async(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets account commission rates
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#user-commission-rate" /></para>
    /// </summary>
    /// <param name="symbol">Symbol, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>User commission rate information</returns>
    Task<RestCallResult<BinanceFuturesAccountUserCommissionRate>> GetUserCommissionRateAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get user account configuration
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#futures-account-configuration" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<BinanceFuturesAccountConfiguration>> GetAccountConfigurationAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get user symbol configuration
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#symbol-configuration" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<List<BinanceFuturesSymbolConfiguration>>> GetSymbolConfigurationAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the order rate limits
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#query-user-rate-limit" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceRateLimit>>> GetRateLimitsAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets Notional and Leverage Brackets.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#notional-and-leverage-brackets" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get the data for, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Notional and Leverage Brackets</returns>
    Task<RestCallResult<List<BinanceFuturesSymbolBracket>>> GetBracketsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get user's Multi-Assets mode (Multi-Assets Mode or Single-Asset Mode) on Every symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#get-current-multi-assets-mode" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Multi asset mode</returns>
    Task<RestCallResult<BinanceFuturesMultiAssetMode>> GetMultiAssetsModeAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get user's position mode (Hedge Mode or One-way Mode ) on EVERY symbol
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#get-current-position-mode" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Whether the request was successful</returns>
    Task<RestCallResult<BinanceFuturesPositionMode>> GetPositionModeAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the income history for the futures account
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#get-income-history" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get income history from, for example `ETHUSDT`</param>
    /// <param name="incomeType">The income type filter to apply to the request</param>
    /// <param name="startTime">Time to start getting income history from</param>
    /// <param name="endTime">Time to stop getting income history from</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="page">Page number</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The income history for the futures account</returns>
    Task<RestCallResult<List<BinanceFuturesIncomeHistory>>> GetIncomeHistoryAsync(string? symbol = null, BinanceFuturesIncomeType? incomeType = null, DateTime? startTime = null, DateTime? endTime = null, long? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the current status of the trading rules for the account
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#futures-trading-quantitative-rules-indicators" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of trading rules status per symbol</returns>
    Task<RestCallResult<BinanceFuturesTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get download id for downloading transaction history
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#get-download-id-for-futures-transaction-history" /></para>
    /// </summary>
    /// <param name="startTime">Start time of the data to download</param>
    /// <param name="endTime">End time of the data to download</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTransactionHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the download link for transaction history by download id
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#get-futures-transaction-history-download-link-by-id" /></para>
    /// </summary>
    /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForTransactionHistoryAsync" /></param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTransactionHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get download id for downloading order history
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#get-download-id-for-futures-order-history" /></para>
    /// </summary>
    /// <param name="startTime">Start time of the data to download</param>
    /// <param name="endTime">End time of the data to download</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForOrderHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the download link for order history by download id
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#get-futures-order-history-download-link-by-id" /></para>
    /// </summary>
    /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForOrderHistoryAsync" /></param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForOrderHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get download id for downloading trade history
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#get-download-id-for-futures-trade-history" /></para>
    /// </summary>
    /// <param name="startTime">Start time of the data to download</param>
    /// <param name="endTime">End time of the data to download</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadIdInfo>> GetDownloadIdForTradeHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get the download link for order history by download id
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#get-futures-trade-download-link-by-id" /></para>
    /// </summary>
    /// <param name="downloadId">The download id as requested by <see cref="GetDownloadIdForTradeHistoryAsync" /></param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceFuturesDownloadLink>> GetDownloadLinkForTradeHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Set BNB burn for fee discount status
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#toggle-bnb-burn-on-futures-trade" /></para>
    /// </summary>
    /// <param name="feeBurn">Fee burn status</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> SetBnbBurnStatusAsync(bool feeBurn, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get BNB burn for fee discount status
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-derivatives-trading-usd-s-m-futures/api/rest-api/account#get-bnb-burn-status" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default);
}
