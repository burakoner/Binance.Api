namespace Binance.Api.Link;

internal class BinanceLinkRestClient : IBinanceLinkRestClient
{
    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.ApiOptions;

    // Clients
    public IBinanceLinkRestClientExchangeLink ExchangeLink { get; }
    public IBinanceLinkRestClientLinkAndTrade LinkAndTrade { get; }

    // Constructor
    internal BinanceLinkRestClient(BinanceRestApiClient root)
    {
        _ = root;
        ExchangeLink = new BinanceLinkRestClientExchangeLink(this);
        LinkAndTrade = new BinanceLinkRestClientLinkAndTrade(this);
    }
}