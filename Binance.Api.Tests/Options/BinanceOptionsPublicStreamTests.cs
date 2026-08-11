using Binance.Api.Options;
using Binance.Api.Shared;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsPublicStreamTests
{
    [Fact]
    public void PublicAndMarketStreams_UseCurrentSeparateRoutes()
    {
        Assert.Equal("wss://fstream.binance.com/public", BinanceAddress.Default.EuropeanOptionsPublicSocketApiStreamAddress);
        Assert.Equal("wss://fstream.binance.com/public/stream", BinanceOptionsSocketClient.PublicStreamAddress());
        Assert.Equal("wss://fstream.binance.com/market", BinanceAddress.Default.EuropeanOptionsMarketSocketApiStreamAddress);
        Assert.Equal("wss://fstream.binance.com/market/stream", BinanceOptionsSocketClient.MarketStreamAddress());
    }

    [Fact]
    public void PublicTopics_UseCurrentNamesAndStrictValues()
    {
        Assert.Equal(
            ["btcusdt@depth@100ms", "ethusdt@depth@100ms"],
            BinanceOptionsSocketClient.DiffDepthStreamTopics(["BTCUSDT", "ETHUSDT"], 100));
        Assert.Equal(
            ["btcusdt@bookTicker"],
            BinanceOptionsSocketClient.BookTickerStreamTopics(["BTCUSDT"]));
        Assert.Equal(
            ["btc-251230-100000-c@depth20@500ms"],
            BinanceOptionsSocketClient.PartialDepthStreamTopics(["BTC-251230-100000-C"], 20, 500));
        Assert.Equal(
            ["btcusdt@optionTicker"],
            BinanceOptionsSocketClient.TickerStreamTopics(["BTCUSDT"]));
        Assert.Equal(
            ["btcusdt@optionTicker251230"],
            BinanceOptionsSocketClient.TickerStreamTopics([("BTCUSDT", new DateTime(2025, 12, 30))]));
        Assert.Equal(
            ["btcusdt@optionTrade"],
            BinanceOptionsSocketClient.TradeStreamTopics(["BTCUSDT"]));

        Assert.Throws<ArgumentNullException>(() => BinanceOptionsSocketClient.DiffDepthStreamTopics(null!, 100));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.BookTickerStreamTopics([]));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.TradeStreamTopics([" "]));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.DiffDepthStreamTopics(["BTCUSDT"], 1_000));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.PartialDepthStreamTopics(["BTCUSDT"], 50, 100));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.TickerStreamTopics(Array.Empty<(string Symbol, DateTime ExpirationDate)>()));
    }

    [Fact]
    public void DepthPayload_MapsCurrentSequenceAndBookFields()
    {
        var update = JsonConvert.DeserializeObject<BinanceOptionsStreamOrderBook>(
            """{"e":"depthUpdate","E":1762914568643,"T":1762914568619,"s":"BTC-251230-100000-C","U":9223372036854775805,"u":9223372036854775807,"pu":9223372036854775804,"b":[["100.25","2.5"]],"a":[["100.5","1.25"]]}""")!;

        Assert.Equal("depthUpdate", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_762_914_568_643).UtcDateTime, update.EventTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_762_914_568_619).UtcDateTime, update.TransactionTime);
        Assert.Equal("BTC-251230-100000-C", update.Symbol);
        Assert.Equal(long.MaxValue - 2, update.FirstUpdateId);
        Assert.Equal(long.MaxValue, update.LastUpdateId);
        Assert.Equal(long.MaxValue - 3, update.PreviousLastUpdateId);
        Assert.Equal(100.25m, Assert.Single(update.Bids).Price);
        Assert.Equal(2.5m, Assert.Single(update.Bids).Quantity);
        Assert.Equal(100.5m, Assert.Single(update.Asks).Price);
        Assert.Equal(1.25m, Assert.Single(update.Asks).Quantity);
        Assert.Null(typeof(BinanceOptionsStreamOrderBook).GetProperty("UpdateId"));
    }

    [Fact]
    public void BookTickerPayload_MapsCurrentBestPricesAndTimestamp()
    {
        var update = JsonConvert.DeserializeObject<BinanceOptionsStreamBookTicker>(
            """{"e":"bookTicker","E":1762914568643,"u":9223372036854775807,"s":"BTC-251230-100000-C","b":"100.25","B":"2.5","a":"100.5","A":"1.25","T":1762914568619}""")!;

        Assert.Equal("bookTicker", update.Event);
        Assert.Equal(long.MaxValue, update.UpdateId);
        Assert.Equal("BTC-251230-100000-C", update.Symbol);
        Assert.Equal(100.25m, update.BestBidPrice);
        Assert.Equal(2.5m, update.BestBidQuantity);
        Assert.Equal(100.5m, update.BestAskPrice);
        Assert.Equal(1.25m, update.BestAskQuantity);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_762_914_568_619).UtcDateTime, update.TransactionTime);
    }

    [Fact]
    public void TickerPayload_MapsCurrentStatisticsAndRemovesRetiredGreeks()
    {
        var update = JsonConvert.DeserializeObject<BinanceOptionsStreamTicker>(
            """{"e":"24hrTicker","E":1762914568643,"s":"BTCUSDT","p":"1.25","P":"2.5","w":"100.25","c":"101.5","Q":"0.75","o":"99","h":"110","l":"95","v":"123.5","q":"12450.75","O":1762828168643,"C":1762914568643,"F":9223372036854775805,"L":9223372036854775807,"n":42}""")!;

        Assert.Equal("24hrTicker", update.Event);
        Assert.Equal("BTCUSDT", update.Symbol);
        Assert.Equal(1.25m, update.PriceChange);
        Assert.Equal(2.5m, update.PriceChangePercent);
        Assert.Equal(100.25m, update.WeightedAveragePrice);
        Assert.Equal(101.5m, update.LastPrice);
        Assert.Equal(0.75m, update.LastQuantity);
        Assert.Equal(99m, update.Open);
        Assert.Equal(110m, update.High);
        Assert.Equal(95m, update.Low);
        Assert.Equal(123.5m, update.ContractVolume);
        Assert.Equal(12_450.75m, update.QuoteAmount);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_762_828_168_643).UtcDateTime, update.StatisticsOpenTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_762_914_568_643).UtcDateTime, update.StatisticsCloseTime);
        Assert.Equal(long.MaxValue - 2, update.FirstTradeId);
        Assert.Equal(long.MaxValue, update.LastTradeId);
        Assert.Equal(42, update.TradeCount);
        Assert.Null(typeof(BinanceOptionsStreamTicker).GetProperty("Delta"));
        Assert.Null(typeof(BinanceOptionsStreamTicker).GetProperty("MarkPrice"));
        Assert.Null(typeof(BinanceOptionsStreamTicker).GetProperty("BestBidPrice"));
        Assert.Null(typeof(BinanceOptionsStreamTicker).GetProperty("Volume"));
    }

    [Fact]
    public void TradePayload_MapsCurrentSideTypeAndMakerFlag()
    {
        var update = JsonConvert.DeserializeObject<BinanceOptionsStreamTrade>(
            """{"e":"trade","E":1762914568643,"T":1762914568619,"s":"BTC-251230-100000-C","t":9223372036854775807,"p":"100.25","q":"2.5","X":"BLOCK","S":"BUY","m":true}""")!;

        Assert.Equal("trade", update.Event);
        Assert.Equal("BTC-251230-100000-C", update.Symbol);
        Assert.Equal(long.MaxValue, update.TradeId);
        Assert.Equal(100.25m, update.Price);
        Assert.Equal(2.5m, update.Quantity);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_762_914_568_619).UtcDateTime, update.Time);
        Assert.Equal(BinanceOrderSide.Buy, update.Side);
        Assert.Equal(BinanceOptionsTradeType.BlockTrading, update.Type);
        Assert.True(update.BuyerIsMarketMaker);
        Assert.Null(typeof(BinanceOptionsStreamTrade).GetProperty("BuyOrderId"));
        Assert.Null(typeof(BinanceOptionsStreamTrade).GetProperty("SellOrderId"));
    }
}
