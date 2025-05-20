namespace Binance.Api.Staking;

/// <summary>
/// Interface for the ETH Staking Rest API Client.
/// </summary>
public interface IBinanceStakingRestClientEth:
    IBinanceStakingRestClientEthAccount,
    IBinanceStakingRestClientEthHistory,
    IBinanceStakingRestClientEthStaking;