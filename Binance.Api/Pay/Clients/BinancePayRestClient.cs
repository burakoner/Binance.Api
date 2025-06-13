namespace Binance.Api.Pay;

internal partial class BinancePayRestClient: IBinancePayRestClient
{
    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.ApiOptions;

    // Interface Properties
    public IBinancePayRestClientHistory History { get; }
    public IBinancePayRestClientMerchant Merchant { get; }

    // Constructor
    internal BinancePayRestClient(BinanceRestApiClient root)
    {
        _ = root;
        History = new BinancePayRestClientHistory(this);
        Merchant = new BinancePayRestClientMerchant(this);
    }
}