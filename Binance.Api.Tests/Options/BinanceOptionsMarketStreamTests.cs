using Binance.Api.Options;
using Binance.Api.Shared;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsMarketStreamTests
{
    [Fact]
    public void MarketTopics_UseCurrentNamesDatesAndIntervals()
    {
        Assert.Equal("!index@arr", BinanceOptionsSocketClient.IndexPriceStreamTopic);
        Assert.Equal("!optionSymbol", BinanceOptionsSocketClient.NewSymbolStreamTopic);
        Assert.Equal(
            ["btcusdt@openInterest@251230"],
            BinanceOptionsSocketClient.OpenInterestStreamTopics([("BTCUSDT", new DateTime(2025, 12, 30))]));
        Assert.Equal(
            ["btcusdt@optionMarkPrice", "ethusdt@optionMarkPrice"],
            BinanceOptionsSocketClient.MarkPriceStreamTopics(["BTCUSDT", "ETHUSDT"]));
        Assert.Equal(
            ["btc-251230-100000-c@kline_1m", "btc-251230-100000-c@kline_1w"],
            BinanceOptionsSocketClient.KlineStreamTopics(
                ["BTC-251230-100000-C"],
                [BinanceKlineInterval.OneMinute, BinanceKlineInterval.OneWeek]));
    }

    [Fact]
    public void MarketTopics_RejectUnpublishedOrMissingInputs()
    {
        Assert.Throws<ArgumentNullException>(() => BinanceOptionsSocketClient.OpenInterestStreamTopics(null!));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.OpenInterestStreamTopics([]));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.OpenInterestStreamTopics([(" ", DateTime.UtcNow)]));
        Assert.Throws<ArgumentNullException>(() => BinanceOptionsSocketClient.MarkPriceStreamTopics(null!));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.MarkPriceStreamTopics([]));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.MarkPriceStreamTopics([" "]));
        Assert.Throws<ArgumentNullException>(() => BinanceOptionsSocketClient.KlineStreamTopics(null!, [BinanceKlineInterval.OneMinute]));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.KlineStreamTopics([], [BinanceKlineInterval.OneMinute]));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.KlineStreamTopics([" "], [BinanceKlineInterval.OneMinute]));
        Assert.Throws<ArgumentNullException>(() => BinanceOptionsSocketClient.KlineStreamTopics(["BTCUSDT"], null!));
        Assert.Throws<ArgumentException>(() => BinanceOptionsSocketClient.KlineStreamTopics(["BTCUSDT"], []));
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceOptionsSocketClient.KlineStreamTopics(["BTCUSDT"], [BinanceKlineInterval.OneSecond]));
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceOptionsSocketClient.KlineStreamTopics(["BTCUSDT"], [BinanceKlineInterval.EightHours]));
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceOptionsSocketClient.KlineStreamTopics(["BTCUSDT"], [BinanceKlineInterval.OneMonth]));
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceOptionsSocketClient.KlineStreamTopics(["BTCUSDT"], [(BinanceKlineInterval)12345]));
    }

    [Fact]
    public void IndexPriceSurfaceAndArrayPayload_MatchAllMarketContract()
    {
        var method = Assert.Single(
            typeof(IBinanceOptionsSocketClientStreamMarketData).GetMethods(),
            candidate => candidate.Name == nameof(IBinanceOptionsSocketClientStreamMarketData.SubscribeToIndexPricesAsync));
        Assert.Equal(2, method.GetParameters().Length);
        Assert.Equal("onMessage", method.GetParameters()[0].Name);

        var updates = JsonConvert.DeserializeObject<List<BinanceOptionsStreamIndexPrice>>(
            """[{"e":"indexPrice","E":1763092572229,"s":"ETHUSDT","p":"3224.51976744"}]""")!;
        var update = Assert.Single(updates);
        Assert.Equal("indexPrice", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_763_092_572_229).UtcDateTime, update.EventTime);
        Assert.Equal("ETHUSDT", update.Symbol);
        Assert.Equal(3_224.51976744m, update.IndexPrice);
    }

    [Fact]
    public void KlinePayload_MapsCurrentNestedSchemaAndInt64Count()
    {
        var wrapper = JsonConvert.DeserializeObject<BinanceOptionsStreamKlineWrapper>(
            """{"e":"kline","E":1763092572229,"s":"BTC-251230-100000-C","k":{"t":1763092500000,"T":1763092559999,"s":"BTC-251230-100000-C","i":"1m","f":9223372036854775805,"L":9223372036854775806,"o":"1000.1","c":"1001.2","h":"1002.3","l":"999.4","v":"5.5","n":9223372036854775807,"x":false,"q":"5500.6","V":"2.7","Q":"2700.8"}}""")!;

        Assert.Equal("BTC-251230-100000-C", wrapper.Symbol);
        var update = BinanceOptionsSocketClient.StandardKline(wrapper);
        Assert.Equal("kline", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_763_092_572_229).UtcDateTime, update.EventTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_763_092_500_000).UtcDateTime, update.OpenTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_763_092_559_999).UtcDateTime, update.CloseTime);
        Assert.Equal("BTC-251230-100000-C", update.Symbol);
        Assert.Equal(BinanceKlineInterval.OneMinute, update.Interval);
        Assert.Equal(long.MaxValue - 2, update.FirstTrade);
        Assert.Equal(long.MaxValue - 1, update.LastTrade);
        Assert.Equal(1_000.1m, update.OpenPrice);
        Assert.Equal(1_001.2m, update.ClosePrice);
        Assert.Equal(1_002.3m, update.HighPrice);
        Assert.Equal(999.4m, update.LowPrice);
        Assert.Equal(5.5m, update.ContractVolume);
        Assert.Equal(5_500.6m, update.QuoteVolume);
        Assert.Equal(2.7m, update.TakerBuyContractVolume);
        Assert.Equal(2_700.8m, update.TakerBuyQuoteVolume);
        Assert.Equal(long.MaxValue, update.TradeCount);
        Assert.False(update.Final);
        Assert.Null(typeof(BinanceOptionsStreamKline).GetProperty("Volume"));
        Assert.Null(typeof(BinanceOptionsStreamKline).GetProperty("TakerBuyBaseVolume"));
    }

    [Fact]
    public void MarkPricePayload_PreservesAllPublishedRiskAndBookFields()
    {
        var updates = JsonConvert.DeserializeObject<List<BinanceOptionsStreamMarkPrice>>(
            """[{"s":"BTC-251120-126000-C","mp":"770.543","E":1762867543321,"e":"markPrice","i":"104334.60217391","P":"0.000","bo":"0.000","ao":"900.000","bq":"0.0000","aq":"0.2000","b":"-1.0","a":"0.98161161","hl":"924.652","ll":"616.435","vo":"0.9408058","rf":"0.0","d":"0.11111964","t":"-164.26702615","g":"0.00001245","v":"30.63855919"}]""")!;

        var update = Assert.Single(updates);
        Assert.Equal("markPrice", update.Event);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_762_867_543_321).UtcDateTime, update.EventTime);
        Assert.Equal("BTC-251120-126000-C", update.Symbol);
        Assert.Equal(770.543m, update.MarkPrice);
        Assert.Equal(104_334.60217391m, update.IndexPrice);
        Assert.Equal(0m, update.EstimatedSettlePrice);
        Assert.Equal(0m, update.BestBidPrice);
        Assert.Equal(900m, update.BestAskPrice);
        Assert.Equal(0m, update.BestBidQuantity);
        Assert.Equal(0.2m, update.BestAskQuantity);
        Assert.Equal(-1m, update.BidIV);
        Assert.Equal(0.98161161m, update.AskIV);
        Assert.Equal(924.652m, update.HighPriceLimit);
        Assert.Equal(616.435m, update.LowPriceLimit);
        Assert.Equal(0.9408058m, update.MarkIV);
        Assert.Equal(0m, update.RiskFreeInterest);
        Assert.Equal(0.11111964m, update.Delta);
        Assert.Equal(-164.26702615m, update.Theta);
        Assert.Equal(0.00001245m, update.Gamma);
        Assert.Equal(30.63855919m, update.Vega);
    }

    [Fact]
    public void NewSymbolAndOpenInterestPayloads_MapCurrentFields()
    {
        var symbol = JsonConvert.DeserializeObject<BinanceOptionsStreamSymbol>(
            """{"e":"optionSymbol","E":1669356423908,"s":"BTC-250926-140000-C","ps":"BTCUSDT","qa":"USDT","d":"CALL","sp":"21000","dt":4133404800000,"u":9223372036854775807,"ot":1569398400000,"cs":"TRADING"}""")!;

        Assert.Equal("optionSymbol", symbol.Event);
        Assert.Equal("BTC-250926-140000-C", symbol.Symbol);
        Assert.Equal("BTCUSDT", symbol.Underlying);
        Assert.Equal("USDT", symbol.QuoteAsset);
        Assert.Equal(BinanceOptionsSide.Call, symbol.Side);
        Assert.Equal(21_000m, symbol.StrikePrice);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(4_133_404_800_000).UtcDateTime, symbol.DeliveryTime);
        Assert.Equal(long.MaxValue, symbol.Unit);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_569_398_400_000).UtcDateTime, symbol.OnboardTime);
        Assert.Equal("TRADING", symbol.Status);
        Assert.Null(typeof(BinanceOptionsStreamSymbol).GetProperty("MinimumTradeVolume"));
        Assert.Null(typeof(BinanceOptionsStreamSymbol).GetProperty("TradeTime"));

        var updates = JsonConvert.DeserializeObject<List<BinanceOptionsStreamOpenInterest>>(
            """[{"e":"openInterest","E":1668759300045,"s":"ETH-221125-2700-C","o":"1580.87","h":"1912992.178168204"}]""")!;
        var openInterest = Assert.Single(updates);
        Assert.Equal("openInterest", openInterest.Event);
        Assert.Equal("ETH-221125-2700-C", openInterest.Symbol);
        Assert.Equal(1_580.87m, openInterest.OpenInterestInContracts);
        Assert.Equal(1_912_992.178168204m, openInterest.OpenInterestInUSDT);
    }
}
