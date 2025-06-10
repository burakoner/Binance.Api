namespace Binance.Api.Broker;

internal partial class BinanceBrokerRestClientLinkAndTrade : IBinanceBrokerRestClientLinkAndTrade
{
    // Parent
    internal BinanceRestApiClient __ { get; }
    internal BinanceBrokerRestClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;

    // Clients
    public IBinanceBrokerRestClientLinkAndTradeFastApi FastApi { get; }
    public IBinanceBrokerRestClientLinkAndTradeFutures Futures { get; }
    public IBinanceBrokerRestClientLinkAndTradeSpot Spot { get; }
    public IBinanceBrokerRestClientLinkAndTradePortfolioMargin PortfolioMargin { get; }

    // Constructor
    internal BinanceBrokerRestClientLinkAndTrade(BinanceBrokerRestClient parent)
    {
        _ = parent;
        __ = parent._;

        FastApi = new BinanceBrokerRestClientLinkAndTradeFastApi(this);
        Futures = new BinanceBrokerRestClientLinkAndTradeFutures(this);
        PortfolioMargin = new BinanceBrokerRestClientLinkAndTradePortfolioMargin(this);
        Spot = new BinanceBrokerRestClientLinkAndTradeSpot(this);
    }
}