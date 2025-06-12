namespace Binance.Api.SimpleEarn;

internal class BinanceSimpleEarnRestClient : IBinanceSimpleEarnRestClient
{    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.ApiOptions;

    // Interface Properties
    public IBinanceSimpleEarnRestClientFlexible Flexible { get; }
    public IBinanceSimpleEarnRestClientLocked Locked { get; }

    // Constructor
    internal BinanceSimpleEarnRestClient(BinanceRestApiClient root)
    {
        _ = root;
        Flexible = new BinanceSimpleEarnRestClientFlexible(this);
        Locked = new BinanceSimpleEarnRestClientLocked(this);
    }
}