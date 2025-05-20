namespace Binance.Api.Staking;

/// <summary>
/// Interface for the ETH Staking -> Account Rest API Client.
/// </summary>
public interface IBinanceStakingRestClientEthAccount
{
    /// <summary>
    /// Get eth staking account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#eth-staking-account-user_data" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
   Task<RestCallResult<BinanceEthStakingAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get ETH staking quotas
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-current-eth-staking-quota-user_data" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceEthStakingQuota>> GetQuotaAsync(int? receiveWindow = null, CancellationToken ct = default);
}