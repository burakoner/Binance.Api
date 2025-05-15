namespace Binance.Api.Broker;

internal class BinanceBrokerRestClient : IBinanceBrokerRestClient
{
    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;

    // Interface Properties
    public IBinanceBrokerRestClientExchangeLink ExchangeLink { get; }
    public IBinanceBrokerRestClientLinkAndTrade LinkAndTrade { get; }

    // Constructor
    internal BinanceBrokerRestClient(BinanceRestApiClient root)
    {
        _ = root;
        ExchangeLink = new BinanceBrokerRestClientExchangeLink(this);
        LinkAndTrade = new BinanceBrokerRestClientLinkAndTrade(this);
    }
}