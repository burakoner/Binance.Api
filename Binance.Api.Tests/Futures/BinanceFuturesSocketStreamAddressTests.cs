using Binance.Api.Futures;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesSocketStreamAddressTests
{
    [Fact]
    public void UsdDocumentedStreams_UseCurrentMarketAndPublicChannels()
    {
        Assert.Equal("wss://fstream.binance.com/market/stream", BinanceFuturesSocketClientUsd.MarketStreamAddress);
        Assert.Equal("wss://fstream.binance.com/public/stream", BinanceFuturesSocketClientUsd.PublicStreamAddress);
    }
}
