namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClient : IBinanceFuturesSocketClient
{
    // Parent
    internal BinanceSocketApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceSocketApiClientOptions SocketOptions => _.SocketOptions;

    // Interface Properties
    public IBinanceFuturesSocketClientCoin Coin { get; }
    public IBinanceFuturesSocketClientUsd USD { get; }

    // Constructor
    internal BinanceFuturesSocketClient(BinanceSocketApiClient root)
    {
        _ = root;
        Coin = new BinanceFuturesSocketClientCoin(this);
        USD = new BinanceFuturesSocketClientUsd(this);
    }
}