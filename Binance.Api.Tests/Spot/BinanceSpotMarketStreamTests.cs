using Binance.Api.Shared;
using Binance.Api.Spot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Spot;

public class BinanceSpotMarketStreamTests
{
    [Fact]
    public void StreamTopics_UseCurrentOfficialNamesAndStrictValues()
    {
        Assert.Equal(
            ["btcusdt@referencePrice", "ethusdt@referencePrice"],
            BinanceSpotMarketDataValidation.SymbolStreamTopics(["BTCUSDT", "ETHUSDT"], "@referencePrice"));
        Assert.Equal(
            ["btcusdt@blockTrade"],
            BinanceSpotMarketDataValidation.SymbolStreamTopics(["BTCUSDT"], "@blockTrade"));
        Assert.Equal(
            ["btcusdt@avgPrice"],
            BinanceSpotMarketDataValidation.SymbolStreamTopics(["BTCUSDT"], "@avgPrice"));
        Assert.Equal(
            ["btcusdt@kline_1m", "btcusdt@kline_4h"],
            BinanceSpotMarketDataValidation.KlineStreamTopics(
                ["BTCUSDT"],
                [BinanceKlineInterval.OneMinute, BinanceKlineInterval.FourHours],
                utc8: false));
        Assert.Equal(
            ["btcusdt@kline_1d@+08:00"],
            BinanceSpotMarketDataValidation.KlineStreamTopics(
                ["BTCUSDT"],
                [BinanceKlineInterval.OneDay],
                utc8: true));

        Assert.Equal("1h", BinanceSpotMarketDataValidation.RollingStreamWindow(TimeSpan.FromHours(1)));
        Assert.Equal("4h", BinanceSpotMarketDataValidation.RollingStreamWindow(TimeSpan.FromHours(4)));
        Assert.Equal("1d", BinanceSpotMarketDataValidation.RollingStreamWindow(TimeSpan.FromDays(1)));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            BinanceSpotMarketDataValidation.RollingStreamWindow(TimeSpan.FromHours(2)));
        Assert.Throws<ArgumentException>(() =>
            BinanceSpotMarketDataValidation.SymbolStreamTopics([], "@trade"));
        Assert.Throws<ArgumentException>(() =>
            BinanceSpotMarketDataValidation.KlineStreamTopics(["BTCUSDT"], [], utc8: false));
        Assert.Throws<ArgumentException>(() =>
            BinanceSpotMarketDataValidation.KlineStreamTopics(
                Enumerable.Repeat("BTCUSDT", 513),
                [BinanceKlineInterval.OneMinute, BinanceKlineInterval.OneHour],
                utc8: false));
    }

    [Fact]
    public void NewStreamModels_MapCurrentOfficialPayloads()
    {
        var referencePrice = JsonConvert.DeserializeObject<BinanceSpotStreamReferencePrice>(
            """{"e":"referencePrice","s":"BAZUSD","r":"1.00","t":1770313263917}""")!;
        Assert.Equal("referencePrice", referencePrice.Event);
        Assert.Equal("BAZUSD", referencePrice.Symbol);
        Assert.Equal(1m, referencePrice.ReferencePrice);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_770_313_263_917).UtcDateTime, referencePrice.Timestamp);

        var blockTrade = JsonConvert.DeserializeObject<BinanceSpotStreamBlockTrade>(
            """{"e":"blockTrade","E":1772506983582,"s":"BNBBTC","t":582,"p":"0.052","q":"5838","T":1772506983321,"m":true}""")!;
        Assert.Equal(582, blockTrade.TradeId);
        Assert.Equal(0.052m, blockTrade.Price);
        Assert.Equal(5_838m, blockTrade.Quantity);
        Assert.True(blockTrade.BuyerIsMaker);

        var averagePrice = JsonConvert.DeserializeObject<BinanceSpotStreamAveragePrice>(
            """{"e":"avgPrice","E":1693907033000,"s":"BTCUSDT","i":"5m","w":"25776.86","T":1693907032213}""")!;
        Assert.Equal("5m", averagePrice.Interval);
        Assert.Equal(25_776.86m, averagePrice.AveragePrice);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_693_907_032_213).UtcDateTime, averagePrice.LastTradeTime);

        var trade = JsonConvert.DeserializeObject<BinanceSpotStreamTrade>(
            """{"e":"trade","E":1672515782136,"s":"BNBBTC","t":12345,"p":"0.001","q":"100","T":1672515782136,"m":true,"M":true}""")!;
        Assert.Equal(12_345, trade.Id);
        Assert.True(trade.Ignore);

        Assert.Null(typeof(BinanceSpotStreamTrade).GetProperty("BuyerOrderId"));
        Assert.Null(typeof(BinanceSpotStreamTrade).GetProperty("SellerOrderId"));
        Assert.Null(typeof(BinanceSpotStreamTrade).GetProperty("IsBestMatch"));
    }

    [Fact]
    public void ServerShutdown_RoutesRawCombinedAndWebSocketApiEnvelopes()
    {
        var root = new BinanceSocketApiClient();
        var client = Assert.IsType<BinanceSpotSocketClient>(root.Spot);
        var received = new List<BinanceSpotServerShutdown>();
        root.Spot.ServerShutdown += data => received.Add(data.Data);
        var timestamp = new DateTime(2026, 8, 8, 20, 0, 0, DateTimeKind.Utc);

        client.HandleServerShutdown(
            JToken.Parse("""{"e":"serverShutdown","E":1770123456789}"""),
            timestamp);
        client.HandleServerShutdown(
            JToken.Parse("""{"stream":"!serverShutdown","data":{"e":"serverShutdown","E":1770123456790}}"""),
            timestamp);
        client.HandleServerShutdown(
            JToken.Parse("""{"event":{"e":"serverShutdown","E":1770123456791}}"""),
            timestamp);
        client.HandleServerShutdown(JToken.Parse("""{"e":"trade"}"""), timestamp);
        client.HandleServerShutdown(JToken.Parse("""{"event":"invalid"}"""), timestamp);

        Assert.Equal(3, received.Count);
        Assert.All(received, update => Assert.Equal("serverShutdown", update.Event));
        Assert.Equal(
            DateTimeOffset.FromUnixTimeMilliseconds(1_770_123_456_791).UtcDateTime,
            received[2].EventTime);
        Assert.Null(BinanceSpotSocketClient.GetServerShutdownPayload(JArray.Parse("[]")));
    }

    [Fact]
    public void TestnetSpotStreamAddress_UsesCurrentStreamHost()
    {
        Assert.Equal("wss://stream.testnet.binance.vision", BinanceAddress.TestNet.SpotSocketApiStreamAddress);

        var root = new BinanceSocketApiClient(new BinanceSocketApiClientOptions
        {
            SpotOptions = new BinanceSocketApiClientSpotOptions
            {
                UseMicrosecondStreamTimestamps = true
            }
        });
        var client = Assert.IsType<BinanceSpotSocketClient>(root.Spot);
        Assert.Equal(
            "wss://stream.binance.com:9443/stream?timeUnit=MICROSECOND",
            client.GetMarketStreamAddress());
    }
}
