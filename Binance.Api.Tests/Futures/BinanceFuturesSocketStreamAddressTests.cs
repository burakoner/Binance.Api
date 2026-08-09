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

    [Fact]
    public void UsdUserDataStream_UsesCurrentPrivateChannel()
    {
        Assert.Equal("wss://fstream.binance.com/private/stream", BinanceFuturesSocketClientUsd.PrivateStreamAddress);
    }

    [Fact]
    public void UsdUserDataStream_PreservesAndValidatesListenKeyTopic()
    {
        Assert.Equal(["Case-Sensitive-Listen-Key"],
            BinanceFuturesSocketClientUsd.UserDataStreamTopics("Case-Sensitive-Listen-Key"));

        Assert.Throws<ArgumentException>(() => BinanceFuturesSocketClientUsd.UserDataStreamTopics(null!));
        Assert.Throws<ArgumentException>(() => BinanceFuturesSocketClientUsd.UserDataStreamTopics(string.Empty));
        Assert.Throws<ArgumentException>(() => BinanceFuturesSocketClientUsd.UserDataStreamTopics(" "));
    }
}
