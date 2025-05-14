namespace Binance.Api.CopyTrading;

internal class BinanceCopyTradingRestClient : IBinanceCopyTradingRestClient
{
    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;

    // Interface Properties
    public IBinanceCopyTradingRestClientFutures Futures { get; }

    // Constructor
    internal BinanceCopyTradingRestClient(BinanceRestApiClient root)
    {
        _ = root;
        Futures = new BinanceCopyTradingRestClientFutures(this);
    }
}