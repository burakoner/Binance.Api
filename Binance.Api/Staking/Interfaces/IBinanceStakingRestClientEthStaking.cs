namespace Binance.Api.Staking;

/// <summary>
/// Interface for the ETH Staking -> Staking Rest API Client.
/// </summary>
public interface IBinanceStakingRestClientEthStaking
{
    /// <summary>
    /// Subscribe to ETH staking
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#subscribe-eth-staking-trade" /></para>
    /// </summary>
    /// <param name="quantity">Amount</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceEthStakingStake>> StakeAsync(decimal quantity, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Redeem from ETH staking
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#redeem-eth-trade" /></para>
    /// </summary>
    /// <param name="quantity">Amount</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceEthStakingRedeem>> RedeemAsync(decimal quantity, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Wrap Beth
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#wrap-beth-trade" /></para>
    /// </summary>
    /// <param name="quantity">Quantity to wrap</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceEthStakingWrap>> WrapAsync(decimal quantity, int? receiveWindow = null, CancellationToken ct = default);
}