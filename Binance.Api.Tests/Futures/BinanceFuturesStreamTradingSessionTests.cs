using ApiSharp.WebSocket;
using Binance.Api.Futures;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesStreamTradingSessionTests
{
    [Fact]
    public void Topic_UsesCurrentMarketStreamName()
    {
        Assert.Equal("tradingSession", BinanceFuturesSocketClientUsd.TradingSessionStreamTopic);
    }

    [Theory]
    [InlineData("EquityUpdate", "PRE_MARKET")]
    [InlineData("EquityUpdate", "REGULAR")]
    [InlineData("EquityUpdate", "AFTER_MARKET")]
    [InlineData("EquityUpdate", "OVERNIGHT")]
    [InlineData("EquityUpdate", "NO_TRADING")]
    [InlineData("CommodityUpdate", "REGULAR")]
    [InlineData("CommodityUpdate", "NO_TRADING")]
    [InlineData("KR_EquityUpdate", "REGULAR")]
    [InlineData("KR_EquityUpdate", "NO_TRADING")]
    [InlineData("HK_EquityUpdate", "REGULAR")]
    [InlineData("HK_EquityUpdate", "NO_TRADING")]
    public void Payload_MapsEveryCurrentEventAndSessionType(string eventType, string sessionType)
    {
        var payload = $$"""
            {
              "stream": "tradingSession",
              "data": {
                "e": "{{eventType}}",
                "E": 1765244143062,
                "t": 1765242000000,
                "T": 1765270800000,
                "S": "{{sessionType}}"
              }
            }
            """;

        var envelope = JsonConvert.DeserializeObject<BinanceFuturesStreamCombinedStream<BinanceFuturesStreamTradingSessionUpdate>>(payload);

        Assert.NotNull(envelope);
        Assert.Equal("tradingSession", envelope.Stream);
        Assert.Equal(eventType, envelope.Data.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_765_244_143_062).UtcDateTime, envelope.Data.EventTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_765_242_000_000).UtcDateTime, envelope.Data.SessionStartTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_765_270_800_000).UtcDateTime, envelope.Data.SessionEndTime);
        Assert.Equal(sessionType, envelope.Data.SessionType);
    }

    [Fact]
    public void PublicSurface_ExposesTypedUsdOnlySubscription()
    {
        var method = Assert.Single(
            typeof(IBinanceFuturesSocketClientUsdStreamMarketData).GetMethods(),
            candidate => candidate.Name == nameof(IBinanceFuturesSocketClientUsdStreamMarketData.SubscribeToTradingSessionsAsync));

        var callback = method.GetParameters()[0].ParameterType;
        Assert.Equal(
            typeof(Action<WebSocketDataEvent<BinanceFuturesStreamTradingSessionUpdate>>),
            callback);
        Assert.DoesNotContain(
            typeof(IBinanceFuturesSocketClientCoinStreamMarketData).GetMethods(),
            candidate => candidate.Name == method.Name);
    }
}
