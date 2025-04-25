namespace Binance.Api.Wallet;

/// <summary>
/// Interface for the Binance Wallet REST API Client Capital Methods
/// </summary>
public interface IBinanceWalletRestClientCapital
{
    /// <summary>
    /// Gets information of assets for a user
    /// <para><a href="https://developers.binance.com/docs/wallet/capital" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Assets info</returns>
    Task<RestCallResult<IEnumerable<BinanceUserAsset>>> GetUserAssetsAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Withdraw assets from Binance to an address
    /// <para><a href="https://developers.binance.com/docs/wallet/capital/withdraw" /></para>
    /// </summary>
    /// <param name="asset">The asset to withdraw, for example `ETH`</param>
    /// <param name="address">The address to send the funds to</param>
    /// <param name="addressTag">Secondary address identifier for assets like XRP,XMR etc.</param>
    /// <param name="withdrawOrderId">Custom client order id</param>
    /// <param name="transactionFeeFlag">When making internal transfer, true for returning the fee to the destination account; false for returning the fee back to the departure account. Default false.</param>
    /// <param name="quantity">The quantity to withdraw</param>
    /// <param name="network">The network to use</param>
    /// <param name="walletType">The wallet type for withdraw</param>
    /// <param name="name">Description of the address</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Withdrawal confirmation</returns>
    Task<RestCallResult<BinanceWithdrawalPlaced>> WithdrawAsync(string asset, string address, decimal quantity, string? withdrawOrderId = null, string? network = null, string? addressTag = null, string? name = null, bool? transactionFeeFlag = null, BinanceWalletType? walletType = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the withdrawal history
    /// <para><a href="https://developers.binance.com/docs/wallet/capital/withdraw-history" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="withdrawOrderId">Filter by withdraw order id</param>
    /// <param name="status">Filter by status</param>
    /// <param name="startTime">Filter start time from</param>
    /// <param name="endTime">Filter end time till</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <param name="limit">Add limit. Default: 1000, Max: 1000</param>
    /// <param name="offset">Add offset</param>
    /// <returns>List of withdrawals</returns>
    Task<RestCallResult<IEnumerable<BinanceWithdrawal>>> GetWithdrawalHistoryAsync(string? asset = null, string? withdrawOrderId = null, BinanceWithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, int? limit = null, int? offset = null, CancellationToken ct = default);

    /// <summary>
    /// Get list of withdrawal addresses
    /// <para><a href="https://developers.binance.com/docs/wallet/capital/fetch-withdraw-address" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<IEnumerable<BinanceWithdrawalAddress>>> GetWithdrawalAddressesAsync(int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Fetch withdraw quota (USER_DATA)

    /// <summary>
    /// Gets the deposit history
    /// <para><a href="https://developers.binance.com/docs/wallet/capital/deposite-history" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="status">Filter by status</param>
    /// <param name="limit">Amount of results</param>
    /// <param name="offset">Offset the results</param>
    /// <param name="startTime">Filter start time from</param>
    /// <param name="endTime">Filter end time till</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="includeSource">Include source address to response</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of deposits</returns>
    Task<RestCallResult<IEnumerable<BinanceDeposit>>> GetDepositHistoryAsync(string? asset = null, BinanceDepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, int? receiveWindow = null, bool includeSource = false, CancellationToken ct = default);

    /// <summary>
    /// Gets the deposit address for an asset
    /// <para><a href="https://developers.binance.com/docs/wallet/capital/deposite-address" /></para>
    /// </summary>
    /// <param name="asset">Asset to get address for, for example `ETH`</param>
    /// <param name="network">Network</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Deposit address</returns>
    Task<RestCallResult<BinanceDepositAddress>> GetDepositAddressAsync(string asset, string? network = null, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Fetch deposit address list with network(USER_DATA)
    // TODO: One click arrival deposit apply (for expired address deposit) (USER_DATA)
}