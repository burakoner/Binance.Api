namespace Binance.Api.Staking;

/// <summary>
/// Interface for the SOL Staking -> Account Rest API Client.
/// </summary>
public interface IBinanceStakingRestClientSolStaking
{
    /// <summary>
    /// Subscribe to SOL staking
    /// <para><a href="https://developers.binance.com/docs/staking/sol-staking/staking" /></para>
    /// </summary>
    /// <param name="quantity">Amount</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSolStakingStake>> StakeAsync(decimal quantity, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Redeem from SOL staking
    /// <para><a href="https://developers.binance.com/docs/staking/sol-staking/staking/Redeem-SOL" /></para>
    /// </summary>
    /// <param name="quantity">Amount</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSolStakingRedeem>> RedeemAsync(decimal quantity, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Claim Boost APR Airdrop rewards
    /// <para><a href="https://developers.binance.com/docs/staking/sol-staking/staking/Claim-Boost-rewards" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSolStakingClaim>> ClaimAsync(int? receiveWindow = null, CancellationToken ct = default);
}