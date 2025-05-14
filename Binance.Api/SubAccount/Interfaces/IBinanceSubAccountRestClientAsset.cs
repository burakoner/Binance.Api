namespace Binance.Api.SubAccount;

/// <summary>
/// Interface for the Binance Sub-Account Asset Rest API client.
/// </summary>
public interface IBinanceSubAccountRestClientAsset
{
    /// <summary>
    /// Transfers from or to a futures sub account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#futures-transfer-for-sub-account-for-master-account" /></para>
    /// </summary>
    /// <param name="email">Email of the sub account</param>
    /// <param name="asset">The asset to transfer</param>
    /// <param name="quantity">The quantity to transfer</param>
    /// <param name="type">The type of the transfer</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The result of the transfer</returns>
    Task<RestCallResult<BinanceSubAccountTransactionId>> FuturesTransferAsync(string email, string asset, decimal quantity, BinanceSubAccountFuturesTransferType type, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets futures details for a sub account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-detail-on-sub-account-39-s-futures-account-for-master-account" /></para>
    /// </summary>
    /// <param name="email">The email of the account to get future details for</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Futures details</returns>
    Task<RestCallResult<BinanceSubAccountFuturesDetails>> GetFuturesDetailsAsync(string email, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets futures details for a sub account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-detail-on-sub-account-39-s-futures-account-v2-for-master-account" /></para>
    /// </summary>
    /// <param name="email">The email of the account to get future details for</param>
    /// <param name="futuresType">The account type to get future details for</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Futures details</returns>
    Task<RestCallResult<BinanceSubAccountFuturesDetailsV2>> GetFuturesDetailsAsync(BinanceSubAccountFuturesType futuresType, string email, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets margin details for a sub account
    /// <para><a href="https://developers.binance.com/docs/sub_account/asset-management/Get-Detail-on-Sub-accounts-Margin-Account" /></para>
    /// </summary>
    /// <param name="email">The email of the account to get margin details for</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Margin details</returns>
    Task<RestCallResult<BinanceSubAccountMarginDetails>> GetMarginDetailsAsync(string email, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the deposit address for an asset to a sub account
    /// <para><a href="https://developers.binance.com/docs/sub_account/asset-management/Get-Sub-account-Deposit-Address" /></para>
    /// </summary>
    /// <param name="email">The email of the account to deposit to</param>
    /// <param name="asset">The asset of the deposit</param>
    /// <param name="network">The coin network</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The deposit address</returns>
    Task<RestCallResult<BinanceSubAccountDepositAddress>> GetDepositAddressAsync(string email, string asset, string? network = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the deposit history for a sub account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-sub-account-deposit-history-for-master-account" /></para>
    /// </summary>
    /// <param name="email">The email of the account to get history for</param>
    /// <param name="asset">Filter for an asset</param>
    /// <param name="startTime">Only return deposits placed later this</param>
    /// <param name="endTime">Only return deposits placed before this</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="offset">Offset results by this</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The deposit history</returns>
    Task<RestCallResult<IEnumerable<BinanceSubAccountDeposit>>> GetDepositHistoryAsync(string email, string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? offset = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets futures summary for sub accounts
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-summary-of-sub-account-39-s-futures-account-for-master-account" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Futures summary</returns>
    Task<RestCallResult<BinanceSubAccountFuturesSummary>> GetFuturesSummaryAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets futures summary for sub accounts V2
    /// <para><a href="https://developers.binance.com/docs/sub_account/asset-management/Get-Summary-of-Sub-accounts-Futures-Account-V2" /></para>
    /// </summary>
    /// <param name="futuresType">Futures Type</param>
    /// <param name="page">Page</param>
    /// <param name="limit">Limit. Default:10, Max:20</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSubAccountFuturesSummary>> GetFuturesSummaryAsync(BinanceSubAccountFuturesType futuresType, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets margin summary for sub accounts
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-summary-of-sub-account-39-s-margin-account-for-master-account" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Margin summary</returns>
    Task<RestCallResult<BinanceSubAccountMarginSummary>> GetMarginSummaryAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Transfers from or to a margin sub account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#margin-transfer-for-sub-account-for-master-account" /></para>
    /// </summary>
    /// <param name="email">Email of the sub account</param>
    /// <param name="asset">The asset to transfer</param>
    /// <param name="quantity">The quantity to transfer</param>
    /// <param name="type">The type of the transfer</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The result of the transfer</returns>
    Task<RestCallResult<BinanceSubAccountTransactionId>> MarginTransferAsync(string email, string asset, decimal quantity, BinanceSubAccountMarginTransferType type, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets list of balances for a sub account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#query-sub-account-assets-for-master-account" /></para>
    /// </summary>
    /// <param name="email">For which account to get the assets</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of balances</returns>
    Task<RestCallResult<IEnumerable<BinanceSubAccountBalance>>> GetBalancesAsync(string email, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get internal asset transfers for a sub account (for master account)
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#query-sub-account-futures-asset-transfer-history-for-master-account" /></para>
    /// </summary>
    /// <param name="email">Email of the sub account</param>
    /// <param name="futuresType">Futures account type</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">The page of the results</param>
    /// <param name="limit">The max amount of results to return (Default 50, max 500)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSubAccountFuturesTransferHistory>> GetFuturesTransferHistoryAsync(string email, BinanceSubAccountFuturesType futuresType, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the transfer history of a sub account (from the master account) 
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#query-sub-account-spot-asset-transfer-history-for-master-account" /></para>
    /// </summary>
    /// <param name="fromEmail">Filter the history by from email</param>
    /// <param name="toEmail">Filter the history by to email</param>
    /// <param name="startTime">Filter the history by startTime</param>
    /// <param name="endTime">Filter the history by endTime</param>
    /// <param name="page">The page of the results</param>
    /// <param name="limit">The max amount of results to return</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of transfers</returns>
    Task<RestCallResult<IEnumerable<BinanceSubAccountSpotTransfer>>> GetSpotTransferHistoryAsync(string? fromEmail = null, string? toEmail = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get BTC valued asset summary of subaccounts.
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#query-sub-account-spot-assets-summary-for-master-account" /></para>
    /// </summary>
    /// <param name="email">Email of the sub account</param>
    /// <param name="page">The page</param>
    /// <param name="limit">The page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Btc asset values</returns>
    Task<RestCallResult<BinanceSubAccountSpotSummary>> GetSpotSummaryAsync(string? email = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets a list of universal transfers
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#query-universal-transfer-history-for-master-account" /></para>
    /// </summary>
    /// <param name="fromEmail">Filter the list by from email (fromEmail and toEmail cannot be present at same time)</param>
    /// <param name="toEmail">Filter the list by to email (fromEmail and toEmail cannot be present at same time)</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">The page of the results</param>
    /// <param name="limit">The max amount of results to return (Default 500, max 500)</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of universal transfers</returns>
    Task<RestCallResult<IEnumerable<BinanceSubAccountUniversalTransfer>>> GetUniversalTransferHistoryAsync(string? fromEmail = null, string? toEmail = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Transfer futures asset (for master account)
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#sub-account-futures-asset-transfer-for-master-account" /></para>
    /// </summary>
    /// <param name="fromEmail">From email</param>
    /// <param name="toEmail">To email</param>
    /// <param name="futuresType">Futures account</param>
    /// <param name="asset">Asset</param>
    /// <param name="quantity">Quantity</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSubAccountTransactionId>> FuturesAssetTransferAsync(string fromEmail, string toEmail, BinanceSubAccountFuturesType futuresType, string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the transfer history of a sub account (from the sub account)
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#sub-account-transfer-history-for-sub-account" /></para>
    /// </summary>
    /// <param name="asset">The asset</param>
    /// <param name="type">Filter by type of transfer</param>
    /// <param name="startTime">Only return transfers later than this</param>
    /// <param name="endTime">Only return transfers before this</param>
    /// <param name="limit">Max number of results</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Transfer history</returns>
    Task<RestCallResult<IEnumerable<BinanceSubAccountTransferSubAccount>>> GetTransferHistoryAsync(string? asset = null, BinanceSubAccountTransferType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Transfers to master account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#transfer-to-master-for-sub-account" /></para>
    /// </summary>
    /// <param name="asset">The asset to transfer</param>
    /// <param name="quantity">The quantity to transfer</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The result of the transfer</returns>
    Task<RestCallResult<BinanceSubAccountTransactionId>> TransferSubAccountToMasterAsync(string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Transfers to another sub account of the same master
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#transfer-to-sub-account-of-same-master-for-sub-account" /></para>
    /// </summary>
    /// <param name="email">Email of the sub account</param>
    /// <param name="asset">The asset to transfer</param>
    /// <param name="quantity">The quantity to transfer</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The result of the transfer</returns>
    Task<RestCallResult<BinanceSubAccountTransactionId>> TransferSubAccountToSubAccountAsync(string email, string asset, decimal quantity, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Transfers an asset form/to a sub account. If fromEmail or toEmail is not send it is interpreted as from/to the master account. Transfer between futures accounts is not supported
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#sub-account-futures-asset-transfer-for-master-account" /></para>
    /// </summary>
    /// <param name="fromEmail">From which account to transfer</param>
    /// <param name="fromAccountType">Account type to transfer from</param>
    /// <param name="toEmail">To which account to transfer</param>
    /// <param name="toAccountType">Account type to transfer to</param>
    /// <param name="asset">The asset to transfer</param>
    /// <param name="symbol">The symbol to transfer, only for isolated margin</param>
    /// <param name="quantity">The quantity to transfer</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The result of the transfer</returns>
    Task<RestCallResult<BinanceSubAccountTransactionId>> UniversalTransferAsync(BinanceSubAccountTransferAccountType fromAccountType, BinanceSubAccountTransferAccountType toAccountType, string asset, decimal quantity, string? fromEmail = null, string? toEmail = null, string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Move Position for Sub-account (For Master Account) (USER_DATA)
    // TODO: Get Move Position History for Sub-account (For Master Account) (USER_DATA)
}