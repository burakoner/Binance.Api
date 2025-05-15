namespace Binance.Api.Wallet;

/// <summary>
/// Interface for the Binance Wallet REST API Client Account Methods
/// </summary>
public interface IBinanceWalletRestClientAccount
{
    /// <summary>
    /// Get account VIP level and margin/futures enabled status
    /// <para><a href="https://developers.binance.com/docs/wallet/account" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceWalletVipLevelAndStatus>> GetAccountVipLevelAndStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get a daily account snapshot (balances)
    /// <para><a href="https://developers.binance.com/docs/wallet/account/daily-account-snapshoot" /></para>
    /// </summary>
    /// <param name="startTime">The start time</param>
    /// <param name="endTime">The end time</param>
    /// <param name="limit">The amount of days to retrieve</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceWalletSpotAccountSnapshot>>> GetDailySpotAccountSnapshotAsync(DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get a daily account snapshot (balances)
    /// <para><a href="https://developers.binance.com/docs/wallet/account/daily-account-snapshoot" /></para>
    /// </summary>
    /// <param name="startTime">The start time</param>
    /// <param name="endTime">The end time</param>
    /// <param name="limit">The amount of days to retrieve</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceWalletMarginAccountSnapshot>>> GetDailyMarginAccountSnapshotAsync(DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get a daily account snapshot (balances)
    /// <para><a href="https://developers.binance.com/docs/wallet/account/daily-account-snapshoot" /></para>
    /// </summary>
    /// <param name="startTime">The start time</param>
    /// <param name="endTime">The end time</param>
    /// <param name="limit">The amount of days to retrieve</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceWalletFuturesAccountSnapshot>>> GetDailyFutureAccountSnapshotAsync(DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// This request will disable fastwithdraw switch under your account.
    /// You need to enable "trade" option for the api key which requests this endpoint.
    /// <para><a href="https://developers.binance.com/docs/wallet/account/disable-fast-withdraw-switch" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> DisableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// This request will enable fastwithdraw switch under your account.
    /// You need to enable "trade" option for the api key which requests this endpoint.
    ///
    /// When Fast Withdraw Switch is on, transferring funds to a Binance account will be done instantly.
    /// There is no on-chain transaction, no transaction ID and no withdrawal fee.
    /// <para><a href="https://developers.binance.com/docs/wallet/account/enable-fast-withdraw-switch" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<bool>> EnableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the status of the account associated with the api key/secret
    /// <para><a href="https://developers.binance.com/docs/wallet/account/account-status" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Account status</returns>
    Task<RestCallResult<BinanceWalletAccountStatus>> GetAccountStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the trading status for the current account
    /// <para><a href="https://developers.binance.com/docs/wallet/account/account-api-trading-status" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The trading status of the account</returns>
    Task<RestCallResult<BinanceWalletTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get permission info for the current API key
    /// <para><a href="https://developers.binance.com/docs/wallet/account/api-key-permission" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Permission info</returns>
    Task<RestCallResult<BinanceWalletApiKeyPermissions>> GetAPIKeyPermissionsAsync(int? receiveWindow = null, CancellationToken ct = default);
}