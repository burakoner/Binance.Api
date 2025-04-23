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
    internal ILogger Logger => Logger;
    internal BinanceRestApiClientOptions Options => Options;

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

    Futures algo'ları hem usd ye hem de coin e ekle. alias olarak
}