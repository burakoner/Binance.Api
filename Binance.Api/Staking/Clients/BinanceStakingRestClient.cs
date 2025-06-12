namespace Binance.Api.Staking;

internal class BinanceStakingRestClient : IBinanceStakingRestClient
{
    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.ApiOptions;

    // Interface Properties
    public IBinanceStakingRestClientEth ETH { get; }
    public IBinanceStakingRestClientSol SOL { get; }

    // Constructor
    internal BinanceStakingRestClient(BinanceRestApiClient root)
    {
        _ = root;
        ETH = new BinanceStakingRestClientEth(this);
        SOL = new BinanceStakingRestClientSol(this);
    }
}