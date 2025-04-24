namespace Binance.Api.Futures;

internal class BinanceRestApiClientFutures : IBinanceRestApiClientFutures
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions Options => _.RestOptions;

    // Interface Properties
    public IBinanceRestApiClientFuturesCoin Coin { get; }
    public IBinanceRestApiClientFuturesUsd USD { get; }

    // Constructor
    internal BinanceRestApiClientFutures(BinanceRestApiClient root)
    {
        _ = root;
        Coin = new BinanceRestApiClientFuturesCoin(this);
        USD = new BinanceRestApiClientFuturesUsd(this);
    }
}