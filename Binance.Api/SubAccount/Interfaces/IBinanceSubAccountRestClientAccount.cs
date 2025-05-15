namespace Binance.Api.SubAccount;

/// <summary>
/// Interface for the Binance Sub-Account Account Rest API client.
/// </summary>
public interface IBinanceSubAccountRestClientAccount
{
    /// <summary>
    /// Create a virtual sub account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#create-a-virtual-sub-account-for-master-account" /></para>
    /// </summary>
    /// <param name="subAccountString">String based with which a subaccount email will be generated. Should not contain special characters</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSubAccountEmail>> CreateVirtualSubAccountAsync(string subAccountString, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets a list of sub accounts associated with this master account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#query-sub-account-list-for-master-account" /></para>
    /// </summary>
    /// <param name="email">Filter the list by email</param>
    /// <param name="page">The page of the results</param>
    /// <param name="limit">The max amount of results to return</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="isFreeze">Is frozen</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of sub accounts</returns>
    Task<RestCallResult<List<BinanceSubAccount>>> GetSubAccountsAsync(string? email = null, int? page = null, int? limit = null, int? receiveWindow = null, bool? isFreeze = null, CancellationToken ct = default);

    /// <summary>
    /// Enables futures for a sub account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#enable-futures-for-sub-account-for-master-account" /></para>
    /// </summary>
    /// <param name="email">The sub account email to enable futures for</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Futures status</returns>
    Task<RestCallResult<BinanceSubAccountFuturesEnabled>> EnableFuturesAsync(string email, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Enables margin for a sub account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#enable-margin-for-sub-account-for-master-account" /></para>
    /// </summary>
    /// <param name="email">The email of the account to enable margin for</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Margin enable status</returns>
    Task<RestCallResult<BinanceSubAccountMarginEnabled>> EnableMarginAsync(string email, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Enables options for a sub account
    /// <para><a href="https://developers.binance.com/docs/sub_account/account-management/Enable-Options-for-Sub-account" /></para>
    /// </summary>
    /// <param name="email">The email of the account to enable margin for</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Options enable status</returns>
    Task<RestCallResult<BinanceSubAccountOptionsEnabled>> EnableOptionsAsync(string email, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Enable or disable blvt
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#enable-leverage-token-for-sub-account-for-master-account" /></para>
    /// </summary>
    /// <param name="email">Email of the sub account</param>
    /// <param name="enable">Enable or disable (only true for now)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSubAccountBlvt>> EnableBlvtAsync(string email, bool enable, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Sub-account's Status on Margin/Futures(For Master Account)
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-sub-account-39-s-status-on-margin-futures-for-master-account" /></para>
    /// </summary>
    /// <param name="email">Filter the list by email</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of sub accounts status</returns>
    Task<RestCallResult<List<BinanceSubAccountStatus>>> GetSubAccountStatusAsync(string? email = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets futures position risk for a sub account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-futures-position-risk-of-sub-account-for-master-account" /></para>
    /// </summary>
    /// <param name="email">Email of the sub account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Position risk</returns>
    Task<RestCallResult<List<BinanceSubAccountFuturesPositionRisk>>> GetFuturesPositionRiskAsync(string email, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets futures position risk for a sub account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-futures-position-risk-of-sub-account-v2-for-master-account" /></para>
    /// </summary>
    /// <param name="email">Email of the sub account</param>
    /// <param name="futuresType">The account type to get future details for</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Position risk</returns>
    Task<RestCallResult<BinanceSubAccountFuturesPositionRiskV2>> GetFuturesPositionRiskAsync(BinanceFuturesType futuresType, string email, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Query Sub-account Transaction statistics (For Master Account).
    /// <para><a href="https://developers.binance.com/docs/sub_account/account-management/Query-Sub-account-Transaction-Statistics" /></para>
    /// </summary>
    /// <param name="email">Email of the sub account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSubAccountTransactionStatistics>> GetTransactionStatisticsAsync(string email, int? receiveWindow = null, CancellationToken ct = default);
}