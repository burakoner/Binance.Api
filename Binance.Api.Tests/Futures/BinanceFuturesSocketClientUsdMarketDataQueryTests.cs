using ApiSharp.Models;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesSocketClientUsdMarketDataQueryTests
{
    [Fact]
    public void OrderBookLimitsUseCurrentDiscreteWeightsAndDefault()
    {
        Assert.Equal(10, BinanceFuturesSocketClientUsd.DepthRequestWeight(null));
        Assert.Equal(2, BinanceFuturesSocketClientUsd.DepthRequestWeight(5));
        Assert.Equal(2, BinanceFuturesSocketClientUsd.DepthRequestWeight(10));
        Assert.Equal(2, BinanceFuturesSocketClientUsd.DepthRequestWeight(20));
        Assert.Equal(2, BinanceFuturesSocketClientUsd.DepthRequestWeight(50));
        Assert.Equal(5, BinanceFuturesSocketClientUsd.DepthRequestWeight(100));
        Assert.Equal(10, BinanceFuturesSocketClientUsd.DepthRequestWeight(500));
        Assert.Equal(20, BinanceFuturesSocketClientUsd.DepthRequestWeight(1000));

        foreach (var invalidLimit in new[] { 0, 1, 51, 99, 101, 499, 501, 999, 1001 })
            Assert.Throws<ArgumentOutOfRangeException>(() => BinanceFuturesSocketClientUsd.DepthRequestWeight(invalidLimit));
    }

    [Fact]
    public async Task RequiredSymbolsAndInvalidDepthLimitsAreRejectedBeforeTransport()
    {
        var client = Client();

        await Assert.ThrowsAsync<ArgumentException>(() => client.GetOrderBookAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetPriceAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.GetBookPriceAsync(" "));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.GetOrderBookAsync("BTCUSDT", 200));
    }

    [Fact]
    public void BookTickerPublicSurfaceUsesWebSocketSpecificResponseModel()
    {
        var methods = typeof(IBinanceFuturesSocketClientUsdQueryMarketData).GetMethods();
        var single = Assert.Single(methods, method => method.Name == nameof(IBinanceFuturesSocketClientUsdQueryMarketData.GetBookPriceAsync));
        var all = Assert.Single(methods, method => method.Name == nameof(IBinanceFuturesSocketClientUsdQueryMarketData.GetBookPricesAsync));

        Assert.Equal(typeof(Task<CallResult<BinanceFuturesWebSocketBookTicker>>), single.ReturnType);
        Assert.Equal(typeof(Task<CallResult<List<BinanceFuturesWebSocketBookTicker>>>), all.ReturnType);
    }

    [Fact]
    public void CurrentMarketDataResponsesPreserveInt64IdsPricesAndTimes()
    {
        var client = Client();
        var orderBook = client.Deserializer<BinanceResultWithRateLimits<BinanceFuturesOrderBook>>(JToken.Parse(
            """
            {
              "id":"depth",
              "status":200,
              "result":{
                "lastUpdateId":9007199254740993,
                "E":1589436922972,
                "T":1589436922959,
                "bids":[["4.00000000","431.00000000"]],
                "asks":[["4.00000200","12.00000000"]]
              },
              "rateLimits":[]
            }
            """));
        var price = client.Deserializer<BinanceResultWithRateLimits<BinanceFuturesPrice>>(JToken.Parse(
            """
            {
              "id":"price",
              "status":200,
              "result":{"symbol":"BTCUSDT","price":"6000.01","time":1589437530011},
              "rateLimits":[]
            }
            """));
        var bookTicker = client.Deserializer<BinanceResultWithRateLimits<BinanceFuturesWebSocketBookTicker>>(JToken.Parse(
            """
            {
              "id":"book",
              "status":200,
              "result":{
                "lastUpdateId":9007199254740995,
                "symbol":"BTCUSDT",
                "bidPrice":"4.00000000",
                "bidQty":"431.00000000",
                "askPrice":"4.00000200",
                "askQty":"9.00000000",
                "time":1589437530011
              },
              "rateLimits":[]
            }
            """));

        Assert.True(orderBook.Success);
        Assert.Equal(9_007_199_254_740_993L, orderBook.Data.Result.LastUpdateId);
        Assert.Equal(431m, Assert.Single(orderBook.Data.Result.Bids).Quantity);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_589_436_922_959).UtcDateTime, orderBook.Data.Result.TransactionTime);

        Assert.True(price.Success);
        Assert.Equal("BTCUSDT", price.Data.Result.Symbol);
        Assert.Equal(6000.01m, price.Data.Result.Price);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_589_437_530_011).UtcDateTime, price.Data.Result.Time);

        Assert.True(bookTicker.Success);
        Assert.Equal(9_007_199_254_740_995L, bookTicker.Data.Result.LastUpdateId);
        Assert.Equal(431m, bookTicker.Data.Result.BestBidQuantity);
        Assert.Equal(9m, bookTicker.Data.Result.BestAskQuantity);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_589_437_530_011).UtcDateTime, bookTicker.Data.Result.Time);
    }

    private static BinanceFuturesSocketClientUsd Client()
        => Assert.IsType<BinanceFuturesSocketClientUsd>(new BinanceSocketApiClient().UsdFutures);
}
