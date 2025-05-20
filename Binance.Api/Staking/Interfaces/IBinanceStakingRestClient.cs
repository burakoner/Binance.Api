namespace Binance.Api.Staking;

/// <summary>
/// Interface for the Staking Rest API Client.
/// </summary>
public interface IBinanceStakingRestClient
{
    /// <summary>
    /// Interface for the ETH Staking Rest API Client.
    /// </summary>
    IBinanceStakingRestClientEth ETH { get; }

    /// <summary>
    /// Interface for the SOL Staking Rest API Client.
    /// </summary>
    IBinanceStakingRestClientSol SOL { get; }
}