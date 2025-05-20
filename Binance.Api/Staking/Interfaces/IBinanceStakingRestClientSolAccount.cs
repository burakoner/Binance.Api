namespace Binance.Api.Staking;

/// <summary>
/// Interface for the SOL Staking -> Account Rest API Client.
/// </summary>
public interface IBinanceStakingRestClientSolAccount
{
    /// <summary>
    /// Get SOL staking account info
    /// <para><a href="https://developers.binance.com/docs/staking/sol-staking/account" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSolStakingAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get SOL staking quotas
    /// <para><a href="https://developers.binance.com/docs/staking/sol-staking/account/Get-SOL-staking-quota-details" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSolStakingQuota>> GetQuotaAsync(int? receiveWindow = null, CancellationToken ct = default);
}