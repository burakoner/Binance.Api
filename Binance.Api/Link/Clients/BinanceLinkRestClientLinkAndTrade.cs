namespace Binance.Api.Link;

internal partial class BinanceLinkRestClientLinkAndTrade : IBinanceLinkRestClientLinkAndTrade
{
    // Parent
    internal BinanceRestApiClient __ { get; }
    internal BinanceLinkRestClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;

    // Clients
    public IBinanceLinkRestClientLinkAndTradeFastApi FastApi { get; }
    public IBinanceLinkRestClientLinkAndTradeFutures Futures { get; }
    public IBinanceLinkRestClientLinkAndTradeSpot Spot { get; }
    public IBinanceLinkRestClientLinkAndTradePortfolioMargin PortfolioMargin { get; }

    // Constructor
    internal BinanceLinkRestClientLinkAndTrade(BinanceLinkRestClient parent)
    {
        _ = parent;
        __ = parent._;

        FastApi = new BinanceLinkRestClientLinkAndTradeFastApi(this);
        Futures = new BinanceLinkRestClientLinkAndTradeFutures(this);
        PortfolioMargin = new BinanceLinkRestClientLinkAndTradePortfolioMargin(this);
        Spot = new BinanceLinkRestClientLinkAndTradeSpot(this);
    }
}