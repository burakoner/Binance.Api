namespace Binance.Api.PortfolioMargin;

internal partial class BinancePortfolioMarginSocketClientCoinFutures(BinancePortfolioMarginSocketClient parent) : IBinancePortfolioMarginSocketClientCoinFutures
{
    // Parent
    private BinanceSocketApiClient __ { get; } = parent._;
    private BinancePortfolioMarginSocketClient _ { get; } = parent;

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceSocketApiClientOptions SocketOptions => _.SocketOptions;
}