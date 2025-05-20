namespace Binance.Api.Staking;

/// <summary>
/// Interface for the SOL Staking Rest API Client.
/// </summary>
public interface IBinanceStakingRestClientSol :
    IBinanceStakingRestClientSolAccount,
    IBinanceStakingRestClientSolHistory,
    IBinanceStakingRestClientSolStaking;