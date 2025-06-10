namespace Binance.Api.Algo;

internal class BinanceAlgoRestClient: IBinanceAlgoRestClient
{
    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;

    // Interface Properties
    public IBinanceAlgoRestClientSpot Spot { get; }
    public IBinanceAlgoRestClientFutures Futures { get; }

    // Constructor
    internal BinanceAlgoRestClient(BinanceRestApiClient root)
    {
        _ = root;
        Spot = new BinanceAlgoRestClientSpot(this);
        Futures = new BinanceAlgoRestClientFutures(this);
    }
}