namespace Binance.Api.Link;

/// <summary>
/// Interface for the Binance Exchange Link - Account Rest API client.
/// </summary>
public interface IBinanceLinkRestClientExchangeLinkAccount
{
    /// <summary>
    /// Create a Sub Account
    /// <para>This request will generate a sub account under your brokerage master account</para>
    /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Created sub-account id</returns>
    Task<RestCallResult<BinanceLinkSubAccountCreateResult>> CreateSubAccountAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Sub Account
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="page">Page (default 1)</param>
    /// <param name="size">Size (default 500)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Sub accounts</returns>
    Task<RestCallResult<List<BinanceLinkSubAccount>>> GetSubAccountsAsync(string? subAccountId = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Enable Futures for Sub Account
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Enable Futures result</returns>
    Task<RestCallResult<BinanceLinkEnableFuturesResult>> EnableFuturesAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Enable Margin for Sub Account
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Enable Margin result</returns>
    Task<RestCallResult<BinanceLinkEnableMarginResult>> EnableMarginAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Enable Leverage Token for Sub Account
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Enable Leverage Token result</returns>
    Task<RestCallResult<BinanceLinkEnableLeverageTokenResult>> EnableLeverageTokenAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Create Api Key for Sub Account
    /// <para>This request will generate a api key for a sub account</para>
    /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
    /// <para>Sub account should be enable margin before its api-key's marginTrade being enabled</para>
    /// <para>Sub account should be enable futures before its api-key's futuresTrade being enabled</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="isSpotTradingEnabled">Is spot trading enabled</param>
    /// <param name="isMarginTradingEnabled">Is margin trading enabled</param>
    /// <param name="isFuturesTradingEnabled">Is futures trading enabled</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Api key result</returns>
    Task<RestCallResult<BinanceLinkApiKeyCreateResult>> CreateApiKeyAsync(string subAccountId, bool isSpotTradingEnabled, bool? isMarginTradingEnabled = null, bool? isFuturesTradingEnabled = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Change Sub Account Api Permission
    /// <para>This request will change the api permission for a sub account</para>
    /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
    /// <para>Sub account should be enable margin before its api-key's marginTrade being enabled</para>
    /// <para>Sub account should be enable futures before its api-key's futuresTrade being enabled</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="apiKey">Api key</param>
    /// <param name="isSpotTradingEnabled">Is spot trading enabled</param>
    /// <param name="isMarginTradingEnabled">Is margin trading enabled</param>
    /// <param name="isFuturesTradingEnabled">Is futures trading enabled</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Api key result</returns>
    Task<RestCallResult<BinanceLinkSubAccountApiKey>> SetApiKeyPermissionAsync(string subAccountId, string apiKey, bool isSpotTradingEnabled, bool isMarginTradingEnabled, bool isFuturesTradingEnabled, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Enable Universal Transfer Permission For Sub Account Api Key

    /// <summary>
    /// Enable or Disable IP Restriction for Sub Account Api Key
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="apiKey">Api key</param>
    /// <param name="status">IP Restriction Status</param>
    /// <param name="ipAddress">IP address</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Restriction result</returns>
    Task<RestCallResult<BinanceLinkIpRestrictionV2>> SetApiKeyIpRestrictionAsync(string subAccountId, string apiKey, BinanceLinkApiKeyIpRestriction status, string? ipAddress = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Delete IP Restriction for Sub Account Api Key
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="apiKey">Api key</param>
    /// <param name="ipAddress">IP address</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Restriction result</returns>
    Task<RestCallResult<BinanceLinkIpRestrictionBase>> DeleteApiKeyIpRestrictionAsync(string subAccountId, string apiKey, string ipAddress, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Delete Sub Account Api Key
    /// <para>This request will delete a api key for a sub account</para>
    /// <para>You need to enable "trade" option for the api key which requests this endpoint</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="apiKey"></param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    Task<RestCallResult<bool>> DeleteApiKeyAsync(string subAccountId, string apiKey, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get IP Restriction for Sub Account Api Key
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="apiKey">Api key</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Restriction result</returns>
    Task<RestCallResult<BinanceLinkIpRestriction>> GetApiKeyIpRestrictionAsync(string subAccountId, string apiKey, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Sub Account Api Key
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="apiKey">Api key</param>
    /// <param name="page">Page (default 1)</param>
    /// <param name="size">Size (default 500, max 500)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Api key result</returns>
    Task<RestCallResult<BinanceLinkSubAccountApiKey>> GetApiKeyAsync(string subAccountId, string? apiKey = null, int? page = null, int? size = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Broker Account Information
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Broker information</returns>
    Task<RestCallResult<BinanceLinkAccountInfo>> GetBrokerAccountAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Enable Or Disable BNB Burn for Sub Account SPOT and MARGIN
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="spotBnbBurn">"true" or "false", spot and margin whether use BNB to pay for transaction fees or not</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Result</returns>
    Task<RestCallResult<BinanceLinkChangeBnbBurnSpotAndMarginResult>> SetBnbBurnForSpotAndMarginAsync(string subAccountId, bool spotBnbBurn, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Enable Or Disable BNB Burn for Sub Account Margin Interest
    /// <para>Sub account must be enabled margin before using this switch</para>
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="interestBnbBurn">"true" or "false", margin loan whether uses BNB to pay for margin interest or not</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Result</returns>
    Task<RestCallResult<BinanceLinkChangeBnbBurnMarginInterestResult>> SetBnbBurnForMarginInterestAsync(string subAccountId, bool interestBnbBurn, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get BNB Burn Status for Sub Account
    /// </summary>
    /// <param name="subAccountId">Sub account id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Status</returns>
    Task<RestCallResult<BinanceLinkBnbBurnStatus>> GetBnbBurnStatusAsync(string subAccountId, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Delete Sub Account
}