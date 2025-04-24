using Binance.Api.Wallet;

namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Account Methods
/// </summary>
public interface IBinanceMarginRestApiClientAccount
{
    /// <summary>
    /// Adjust cross margin max leverage
    /// <para><a href="https://developers.binance.com/docs/margin_trading/account" /></para>
    /// </summary>
    /// <param name="maxLeverage">Max leverage, can only adjust 3 or 5</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceCrossMarginLeverageResult>> CrossMarginAdjustMaxLeverageAsync(int maxLeverage, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Disabled an isolated margin account info
    /// <para><a href="https://developers.binance.com/docs/margin_trading/account/Disable-Isolated-Margin-Account" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to enable isolated margin account for, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<CreateIsolatedMarginAccountResult>> DisableIsolatedMarginAccountAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Enable an isolated margin account
    /// <para><a href="https://developers.binance.com/docs/margin_trading/account/Enable-Isolated-Margin-Account" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to enable isolated margin account for, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<CreateIsolatedMarginAccountResult>> EnableIsolatedMarginAccountAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the status of the BNB burn switch for spot trading and margin interest
    /// <para><a href="https://developers.binance.com/docs/margin_trading/account/Get-BNB-Burn-Status" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceWalletBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get personal margin level information for your account
    /// <para><a href="https://developers.binance.com/docs/margin_trading/account/Get-Summary-Of-Margin-Account" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Margin Level Information</returns>
    Task<RestCallResult<BinanceMarginLevel>> GetMarginLevelInformationAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query margin account details
    /// <para><a href="https://developers.binance.com/docs/margin_trading/account/Query-Cross-Margin-Account-Details" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The margin account information</returns>
    Task<RestCallResult<BinanceMarginAccount>> GetMarginAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get cross margin interest data
    /// <para><a href="https://developers.binance.com/docs/margin_trading/account/Query-Cross-Margin-Fee-Data" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="vipLevel">Vip level</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<IEnumerable<BinanceInterestMarginData>>> GetInterestMarginDataAsync(string? asset = null, string? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get max number of enabled isolated margin accounts
    /// <para><a href="https://developers.binance.com/docs/margin_trading/account/Query-Enabled-Isolated-Margin-Account-Limit" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<IsolatedMarginAccountLimit>> GetEnabledIsolatedMarginAccountLimitAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Isolated margin account info
    /// <para><a href="https://developers.binance.com/docs/margin_trading/account/Query-Isolated-Margin-Account-Info" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceIsolatedMarginAccount>> GetIsolatedMarginAccountAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get isolated margin fee data collection with any vip level or user's current specific data as https://www.binance.com/en/margin-fee
    /// <para><a href="https://developers.binance.com/docs/margin_trading/account/Query-Isolated-Margin-Fee-Data" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="vipLevel">User's current specific margin data will be returned if vipLevel is omitted</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<IEnumerable<BinanceIsolatedMarginFee>>> GetIsolatedMarginFeeDataAsync(string? symbol = null, int? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default);
    
    // TODO: Query Cross Isolated Margin Capital Flow (USER_DATA)
}
