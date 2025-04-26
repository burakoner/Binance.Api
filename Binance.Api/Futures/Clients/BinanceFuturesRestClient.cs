namespace Binance.Api.Futures;

internal class BinanceFuturesRestClient : IBinanceFuturesRestClient
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
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;

    // Interface Properties
    public IBinanceFuturesRestClientCoin Coin { get; }
    public IBinanceFuturesRestClientUsd USD { get; }

    // Constructor
    internal BinanceFuturesRestClient(BinanceRestApiClient root)
    {
        _ = root;
        Coin = new BinanceFuturesRestClientCoin(this);
        USD = new BinanceFuturesRestClientUsd(this);
    }
}