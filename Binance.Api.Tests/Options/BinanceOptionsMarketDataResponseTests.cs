using ApiSharp.Authentication;
using Binance.Api.Options;
using Binance.Api.Shared;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsMarketDataResponseTests
{
    private const string Symbol = "BTC-251226-90000-C";

    [Fact]
    public async Task ExchangeInfo_DeserializesExactContractAssetSymbolFilterAndInt64Models()
    {
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "timezone":"UTC",
              "serverTime":1762843368098,
              "optionContracts":[{"baseAsset":"BTC","quoteAsset":"USDT","underlying":"BTCUSDT","settleAsset":"USDT"}],
              "optionAssets":[{"name":"USDT"}],
              "optionSymbols":[{
                "expiryDate":1766707200000,
                "filters":[{"filterType":"PRICE_FILTER","minPrice":"0.02","maxPrice":"80000.01","tickSize":"0.01","minQty":"0.01","maxQty":"100","stepSize":"0.01"}],
                "symbol":"BTC-251226-90000-C",
                "side":"CALL",
                "strikePrice":"90000",
                "underlying":"BTCUSDT",
                "unit":3000000000,
                "liquidationFeeRate":"0.0019",
                "minQty":"0.01",
                "maxQty":"100",
                "initialMargin":"0.15",
                "maintenanceMargin":"0.075",
                "minInitialMargin":"0.1",
                "minMaintenanceMargin":"0.05",
                "priceScale":3000000000,
                "quantityScale":3000000001,
                "quoteAsset":"USDT",
                "contractType":"CRYPTO_OPTIONS",
                "underlyingType":"CRYPTO",
                "nakedSell":false,
                "status":"TRADING"
              }],
              "rateLimits":[{"rateLimitType":"REQUEST_WEIGHT","interval":"MINUTE","intervalNum":3000000000,"limit":3000000001}]
            }
            """);
        using var client = CreateClient(handler);

        var result = await client.Options.GetExchangeInfoAsync();

        Assert.True(result.Success);
        var data = result.Data;
        Assert.Equal("UTC", data.TimeZone);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1762843368098).UtcDateTime, data.ServerTime);

        var contract = Assert.Single(data.Contracts);
        Assert.Equal("BTC", contract.BaseAsset);
        Assert.Equal("USDT", contract.QuoteAsset);
        Assert.Equal("BTCUSDT", contract.Underlying);
        Assert.Equal("USDT", contract.SettleAsset);
        Assert.Equal("USDT", Assert.Single(data.Assets).Name);

        var symbol = Assert.Single(data.Symbols);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1766707200000).UtcDateTime, symbol.ExpiryDate);
        Assert.Equal(Symbol, symbol.Symbol);
        Assert.Equal(BinanceOptionsSide.Call, symbol.Side);
        Assert.Equal(90_000m, symbol.StrikePrice);
        Assert.Equal("BTCUSDT", symbol.Underlying);
        Assert.Equal(3_000_000_000L, symbol.Unit);
        Assert.Equal(0.0019m, symbol.LiquidationFeeRate);
        Assert.Equal(0.01m, symbol.MinimumQuantity);
        Assert.Equal(100m, symbol.MaximumQuantity);
        Assert.Equal(0.15m, symbol.InitialMargin);
        Assert.Equal(0.075m, symbol.MaintenanceMargin);
        Assert.Equal(0.1m, symbol.MinimumInitialMargin);
        Assert.Equal(0.05m, symbol.MinimumMaintenanceMargin);
        Assert.Equal(3_000_000_000L, symbol.PriceScale);
        Assert.Equal(3_000_000_001L, symbol.QuantityScale);
        Assert.Equal("USDT", symbol.QuoteAsset);
        Assert.Equal("CRYPTO_OPTIONS", symbol.ContractType);
        Assert.Equal("CRYPTO", symbol.UnderlyingType);
        Assert.False(symbol.NakedSell);
        Assert.Equal("TRADING", symbol.Status);

        var filter = Assert.Single(symbol.Filters);
        Assert.Equal("PRICE_FILTER", filter.FilterType);
        Assert.Equal(0.02m, filter.MinPrice);
        Assert.Equal(80_000.01m, filter.MaxPrice);
        Assert.Equal(0.01m, filter.TickSize);
        Assert.Equal(0.01m, filter.MinQuantity);
        Assert.Equal(100m, filter.MaxQuantity);
        Assert.Equal(0.01m, filter.StepSize);
        var rateLimit = Assert.Single(data.RateLimits);
        Assert.Equal("REQUEST_WEIGHT", rateLimit.RateLimitType);
        Assert.Equal("MINUTE", rateLimit.Interval);
        Assert.Equal(3_000_000_000L, rateLimit.IntervalNum);
        Assert.Equal(3_000_000_001L, rateLimit.Limit);
        Assert.Null(typeof(BinanceOptionsSymbol).GetProperty("MakerFeeRate"));
        Assert.Null(typeof(BinanceOptionsSymbol).GetProperty("TakerFeeRate"));
    }

    [Fact]
    public async Task ExerciseAndIndex_DeserializePublishedDecimalAndTimestampFields()
    {
        var exerciseHandler = new RecordingHttpMessageHandler(
            """[{"symbol":"BTC-220121-60000-P","strikePrice":"60000","realStrikePrice":"38844.69652571","expiryDate":1642752000000,"strikeResult":"REALISTIC_VALUE_STRICKEN"}]""");
        using (var exerciseClient = CreateClient(exerciseHandler))
        {
            var result = await exerciseClient.Options.GetPublicExerciseRecordsAsync();

            Assert.True(result.Success);
            var exercise = Assert.Single(result.Data);
            Assert.Equal("BTC-220121-60000-P", exercise.Symbol);
            Assert.Equal(60_000m, exercise.StrikePrice);
            Assert.Equal(38_844.69652571m, exercise.RealStrikePrice);
            Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1642752000000).UtcDateTime, exercise.ExpiryDate);
            Assert.Equal("REALISTIC_VALUE_STRICKEN", exercise.StrikeResult);
        }

        var indexHandler = new RecordingHttpMessageHandler("""{"time":1656647305000,"indexPrice":"105917.75"}""");
        using var indexClient = CreateClient(indexHandler);
        var indexResult = await indexClient.Options.GetIndexPriceAsync("BTCUSDT");

        Assert.True(indexResult.Success);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1656647305000).UtcDateTime, indexResult.Data.Time);
        Assert.Equal(105_917.75m, indexResult.Data.IndexPrice);
    }

    [Fact]
    public async Task Klines_DeserializeThePublishedTwelveItemTuple()
    {
        var handler = new RecordingHttpMessageHandler(
            """[[1762779600000,"1300.000","1400.000","1200.000","1350.000","0.1000",1762780499999,"130.0000000",123,"0.0800","104.0000000","0"]]""");
        using var client = CreateClient(handler);

        var result = await client.Options.GetKlinesAsync(Symbol, BinanceKlineInterval.OneMinute);

        Assert.True(result.Success);
        var kline = Assert.Single(result.Data);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1762779600000).UtcDateTime, kline.OpenTime);
        Assert.Equal(1300m, kline.OpenPrice);
        Assert.Equal(1400m, kline.HighPrice);
        Assert.Equal(1200m, kline.LowPrice);
        Assert.Equal(1350m, kline.ClosePrice);
        Assert.Equal(0.1m, kline.Volume);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1762780499999).UtcDateTime, kline.CloseTime);
        Assert.Equal(130m, kline.QuoteVolume);
        Assert.Equal(123, kline.TradeCount);
        Assert.Equal(0.08m, kline.TakerBuyBaseVolume);
        Assert.Equal(104m, kline.TakerBuyQuoteVolume);
        Assert.Equal("0", kline.IgnoredValue);
        Assert.Null(typeof(BinanceOptionsKline).GetProperty("Interval"));
    }

    [Fact]
    public async Task OpenInterestAndMarkPrice_DeserializeAllPublishedFields()
    {
        var openInterestHandler = new RecordingHttpMessageHandler(
            """[{"symbol":"ETH-221119-1175-P","sumOpenInterest":"4.01","sumOpenInterestUsd":"4880.2985615624","timestamp":"1668754020000"}]""");
        using (var openInterestClient = CreateClient(openInterestHandler))
        {
            var result = await openInterestClient.Options.GetOpenInterestAsync("ETHUSDT", new DateTime(2022, 11, 19));

            Assert.True(result.Success);
            var interest = Assert.Single(result.Data);
            Assert.Equal("ETH-221119-1175-P", interest.Symbol);
            Assert.Equal(4.01m, interest.SumOpenInterest);
            Assert.Equal(4880.2985615624m, interest.SumOpenInterestUsd);
            Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1668754020000).UtcDateTime, interest.Timestamp);
        }

        var markHandler = new RecordingHttpMessageHandler(
            """[{"symbol":"BTC-200730-9000-C","markPrice":"1343.2883","bidIV":"1.40000077","askIV":"1.50000153","markIV":"1.45000000","delta":"0.55937056","theta":"3739.82509871","gamma":"0.00010969","vega":"978.58874732","highPriceLimit":"1618.241","lowPriceLimit":"1068.3356","riskFreeInterest":"0.1"}]""");
        using var markClient = CreateClient(markHandler);
        var markResult = await markClient.Options.GetMarkPriceAsync();

        Assert.True(markResult.Success);
        var mark = Assert.Single(markResult.Data);
        Assert.Equal("BTC-200730-9000-C", mark.Symbol);
        Assert.Equal(1343.2883m, mark.MarkPrice);
        Assert.Equal(1.40000077m, mark.BidIV);
        Assert.Equal(1.50000153m, mark.AskIV);
        Assert.Equal(1.45m, mark.MarkIV);
        Assert.Equal(0.55937056m, mark.Delta);
        Assert.Equal(3739.82509871m, mark.Theta);
        Assert.Equal(0.00010969m, mark.Gamma);
        Assert.Equal(978.58874732m, mark.Vega);
        Assert.Equal(1618.241m, mark.HighPriceLimit);
        Assert.Equal(1068.3356m, mark.LowPriceLimit);
        Assert.Equal(0.1m, mark.RiskFreeInterest);
    }

    [Fact]
    public async Task OrderBook_DeserializesLastUpdateIdTimeAndEndpointSpecificLevels()
    {
        var handler = new RecordingHttpMessageHandler(
            """{"bids":[["1000.000","0.1000"]],"asks":[["1900.000","0.2000"]],"T":1762780909676,"lastUpdateId":3000000000}""");
        using var client = CreateClient(handler);

        var result = await client.Options.GetOrderBookAsync(Symbol);

        Assert.True(result.Success);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1762780909676).UtcDateTime, result.Data.TransactionTime);
        Assert.Equal(3_000_000_000L, result.Data.LastUpdateId);
        Assert.Equal(1000m, Assert.Single(result.Data.Bids).Price);
        Assert.Equal(0.1m, Assert.Single(result.Data.Bids).Quantity);
        Assert.Equal(1900m, Assert.Single(result.Data.Asks).Price);
        Assert.Equal(0.2m, Assert.Single(result.Data.Asks).Quantity);
        Assert.Equal(typeof(List<BinanceOptionsOrderBookEntry>), result.Data.Bids.GetType());
        Assert.Null(typeof(BinanceOptionsOrderBook).GetProperty("Symbol"));
        Assert.Null(typeof(BinanceOptionsOrderBook).GetProperty("UpdateId"));
    }

    [Fact]
    public async Task RecentTrades_PreserveRecordAndTradeIdentifiersForBothEndpoints()
    {
        const long recordId = 2_323_857_420_768_529_000L;
        var tradesHandler = new RecordingHttpMessageHandler(
            $$"""[{"id":{{recordId}},"tradeId":3000000000,"symbol":"{{Symbol}}","price":"1300","qty":"0.1","quoteQty":"130","side":-1,"time":1762780453623}]""");
        using (var tradesClient = CreateClient(tradesHandler))
        {
            var result = await tradesClient.Options.GetRecentTradesAsync(Symbol);

            Assert.True(result.Success);
            var trade = Assert.Single(result.Data);
            Assert.Equal(recordId, trade.Id);
            Assert.Equal(3_000_000_000L, trade.TradeId);
            Assert.Equal(Symbol, trade.Symbol);
            Assert.Equal(1300m, trade.Price);
            Assert.Equal(0.1m, trade.BaseQuantity);
            Assert.Equal(130m, trade.QuoteQuantity);
            Assert.Equal(BinanceOptionsTradeSide.Sell, trade.Side);
            Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1762780453623).UtcDateTime, trade.Time);
        }

        var blockHandler = new RecordingHttpMessageHandler(
            """[{"id":1125899906901081100,"tradeId":3000000001,"symbol":"ETH-250725-1200-P","price":"342.40","qty":"-2167.20","quoteQty":"-4.90","side":1,"time":1733950676483}]""");
        using var blockClient = CreateClient(blockHandler);
        var blockResult = await blockClient.Options.GetRecentBlockTradesAsync();

        Assert.True(blockResult.Success);
        var block = Assert.Single(blockResult.Data);
        Assert.Equal(1_125_899_906_901_081_100L, block.Id);
        Assert.Equal(3_000_000_001L, block.TradeId);
        Assert.Equal("ETH-250725-1200-P", block.Symbol);
        Assert.Equal(342.40m, block.Price);
        Assert.Equal(-2167.20m, block.BaseQuantity);
        Assert.Equal(-4.90m, block.QuoteQuantity);
        Assert.Equal(BinanceOptionsTradeSide.Buy, block.Side);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1733950676483).UtcDateTime, block.Time);
    }

    [Fact]
    public async Task Ticker_DeserializesTheCompletePublishedPriceAndInt64Surface()
    {
        var handler = new RecordingHttpMessageHandler(
            """[{"symbol":"BTC-200730-9000-C","priceChange":"-16.2038","priceChangePercent":"-0.0162","lastPrice":"1000","lastQty":"1000","open":"1016.2038","high":"1016.2038","low":"0","volume":"5","amount":"1","bidPrice":"999.34","askPrice":"1000.23","openTime":1592317127349,"closeTime":1592380593516,"firstTradeId":3000000000,"tradeCount":3000000001,"strikePrice":"9000","exercisePrice":"3000.3356"}]""");
        using var client = CreateClient(handler);

        var result = await client.Options.GetTickersAsync("BTC-200730-9000-C");

        Assert.True(result.Success);
        var ticker = result.Data;
        Assert.Equal("BTC-200730-9000-C", ticker.Symbol);
        Assert.Equal(-16.2038m, ticker.PriceChange);
        Assert.Equal(-0.0162m, ticker.PriceChangePercent);
        Assert.Equal(1000m, ticker.LastPrice);
        Assert.Equal(1000m, ticker.LastQuantity);
        Assert.Equal(1016.2038m, ticker.Open);
        Assert.Equal(1016.2038m, ticker.High);
        Assert.Equal(0m, ticker.Low);
        Assert.Equal(5m, ticker.Volume);
        Assert.Equal(1m, ticker.QuoteVolume);
        Assert.Equal(999.34m, ticker.BestBidPrice);
        Assert.Equal(1000.23m, ticker.BestAskPrice);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1592317127349).UtcDateTime, ticker.OpenTime);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1592380593516).UtcDateTime, ticker.CloseTime);
        Assert.Equal(3_000_000_000L, ticker.FirstTradeId);
        Assert.Equal(3_000_000_001L, ticker.TradeCount);
        Assert.Equal(9000m, ticker.StrikePrice);
        Assert.Equal(3000.3356m, ticker.ExercisePrice);
        Assert.Null(typeof(BinanceOptionsTicker).GetProperty("BestAskQuantity"));
    }

    private static BinanceRestApiClient CreateClient(RecordingHttpMessageHandler handler)
        => new(new BinanceRestApiClientOptions(new ApiCredentials("api-key", "api-secret"))
        {
            AutoTimestamp = false,
            HttpClient = new HttpClient(handler),
            RateLimiterEnabled = false
        });
}
