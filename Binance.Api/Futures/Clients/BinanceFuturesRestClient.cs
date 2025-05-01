namespace Binance.Api.Futures;

internal class BinanceFuturesRestClient : IBinanceFuturesRestClient
{
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