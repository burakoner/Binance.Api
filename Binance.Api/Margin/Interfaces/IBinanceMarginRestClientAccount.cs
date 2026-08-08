using Binance.Api.Wallet;

namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Account Methods
/// </summary>
public interface IBinanceMarginRestClientAccount
{
    /// <summary>
    /// Adjust cross margin max leverage
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/account#adjust-cross-margin-max-leverage" /></para>
    /// </summary>
    /// <param name="maxLeverage">Maximum leverage. Supported request values are 3, 5, and 10.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceCrossMarginLeverageResult>> AdjustMaximumLeverageAsync(long maxLeverage, CancellationToken ct = default);

    /// <summary>
    /// Disables an Isolated Margin account for a symbol. A symbol can be disabled only once every 24 hours.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/account#disable-isolated-margin-account" /></para>
    /// </summary>
    /// <param name="symbol">Symbol whose Isolated Margin account should be disabled, for example <c>ETHUSDT</c>.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceIsolatedMarginCreateAccountResult>> DisableIsolatedMarginAccountAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Enables a previously disabled Isolated Margin account for a symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/account#enable-isolated-margin-account" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to enable, for example <c>ETHUSDT</c>.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceIsolatedMarginCreateAccountResult>> EnableIsolatedMarginAccountAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the status of the BNB burn switch for spot trading and margin interest
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/account#get-bnb-burn-status" /></para>
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceWalletBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get personal margin level information for your account
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/account#get-summary-of-margin-account" /></para>
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Margin Level Information</returns>
    Task<RestCallResult<BinanceMarginLevel>> GetMarginLevelInformationAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Cross or Isolated Margin capital flow.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/account#query-cross-isolated-margin-capital-flow" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset.</param>
    /// <param name="symbol">Isolated Margin symbol. Required when querying isolated data.</param>
    /// <param name="type">Filter by flow type.</param>
    /// <param name="startTime">Start of the requested period. Only data from the last 90 days is available.</param>
    /// <param name="endTime">End of the requested period. An explicit range cannot exceed seven days.</param>
    /// <param name="fromId">Return records with an id greater than this value.</param>
    /// <param name="limit">Maximum records to return. The documented maximum is 1000 and the server default is 500.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds. The maximum is 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginCapitalFlow>>> GetMarginCapitalFlowAsync(
        string? asset = null,
        string? symbol = null,
        BinanceMarginCapitalFlowType? type = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        long? fromId = null,
        long? limit = null,
        int? receiveWindow = null,
        CancellationToken ct = default);

    /// <summary>
    /// Query margin account details
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/account#query-cross-margin-account-details" /></para>
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The margin account information</returns>
    Task<RestCallResult<BinanceMarginAccount>> GetMarginAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets Cross Margin fee data for a coin or the complete collection.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/account#query-cross-margin-fee-data" /></para>
    /// </summary>
    /// <param name="asset">Filter by coin, for example <c>ETH</c>.</param>
    /// <param name="vipLevel">VIP level. The user's current level is used when omitted.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceCrossMarginFee>>> GetCrossMarginFeeDataAsync(string? asset = null, long? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get max number of enabled isolated margin accounts
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/account#query-enabled-isolated-margin-account-limit" /></para>
    /// </summary>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceIsolatedMarginAccountLimit>> GetEnabledIsolatedMarginAccountLimitAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Isolated margin account info
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/account#query-isolated-margin-account-info" /></para>
    /// </summary>
    /// <param name="symbols">Optional set of one to five symbols. All isolated accounts are returned when omitted.</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceIsolatedMarginAccount>> GetIsolatedMarginAccountAsync(IEnumerable<string>? symbols = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get isolated margin fee data collection with any vip level or user's current specific data as https://www.binance.com/en/margin-fee
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/account#query-isolated-margin-fee-data" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example <c>ETHUSDT</c>.</param>
    /// <param name="vipLevel">User's current specific margin data will be returned if vipLevel is omitted</param>
    /// <param name="receiveWindow">Request validity window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceIsolatedMarginFee>>> GetIsolatedMarginFeeDataAsync(string? symbol = null, long? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default);
}
