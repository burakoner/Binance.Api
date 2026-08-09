using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUserTradesTests
{
    [Fact]
    public async Task UsdUserTrades_UseCurrentSignedContractAndCompleteResponse()
    {
        var startTime = DateTimeOffset.FromUnixTimeMilliseconds(1623319461670).UtcDateTime;
        var endTime = startTime.AddDays(7);
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [{
              "buyer": false,
              "commission": "0.07819010",
              "commissionAsset": "USDT",
              "id": 9223372036854775806,
              "maker": false,
              "orderId": 9223372036854775805,
              "price": "7819.01",
              "qty": "0.002",
              "quoteQty": "15.63802",
              "baseQty": "0.002",
              "marginAsset": "USDT",
              "realizedPnl": "-0.91539999",
              "side": "SELL",
              "positionSide": "SHORT",
              "symbol": "BTCUSDT",
              "pair": "BTCUSDT",
              "time": 1569514978020
            }]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetUserTradesAsync(
            "BTCUSDT",
            startTime,
            endTime,
            1000,
            orderId: 9223372036854775805,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/fapi/v1/userTrades", 5);
        var query = DecodedQuery(handler);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains($"startTime={new DateTimeOffset(startTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains($"endTime={new DateTimeOffset(endTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains("limit=1000", query);
        Assert.Contains("orderId=9223372036854775805", query);
        Assert.Contains("recvWindow=60000", query);
        Assert.DoesNotContain("fromId=", query);

        var trade = Assert.Single(result.Data);
        Assert.False(trade.Buyer);
        Assert.Equal(0.07819010m, trade.Fee);
        Assert.Equal("USDT", trade.FeeAsset);
        Assert.Equal(9223372036854775806, trade.Id);
        Assert.False(trade.Maker);
        Assert.Equal(9223372036854775805, trade.OrderId);
        Assert.Equal(7819.01m, trade.Price);
        Assert.Equal(0.002m, trade.Quantity);
        Assert.Equal(15.63802m, trade.QuoteQuantity);
        Assert.Equal(0.002m, trade.BaseQuantity);
        Assert.Equal("USDT", trade.MarginAsset);
        Assert.Equal(-0.91539999m, trade.RealizedPnl);
        Assert.Equal(BinanceOrderSide.Sell, trade.Side);
        Assert.Equal(BinancePositionSide.Short, trade.PositionSide);
        Assert.Equal("BTCUSDT", trade.Symbol);
        Assert.Equal("BTCUSDT", trade.Pair);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1569514978020).UtcDateTime, trade.Timestamp);
    }

    [Fact]
    public async Task CoinPairUserTrades_UseCurrentFlatWeightAndCompleteResponse()
    {
        var startTime = DateTimeOffset.FromUnixTimeMilliseconds(1623319461670).UtcDateTime;
        var endTime = startTime.AddDays(7);
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [{
              "symbol": "BTCUSD_200626",
              "id": 9223372036854775806,
              "orderId": 9223372036854775805,
              "pair": "BTCUSD",
              "side": "SELL",
              "price": "8800",
              "qty": "1",
              "realizedPnl": "0",
              "marginAsset": "BTC",
              "baseQty": "0.01136364",
              "quoteQty": "100",
              "commission": "0.00000454",
              "commissionAsset": "BTC",
              "time": 1590743483586,
              "positionSide": "BOTH",
              "buyer": false,
              "maker": false
            }]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetUserTradesAsync(
            pair: "BTCUSD",
            startTime: startTime,
            endTime: endTime,
            limit: 1000,
            receiveWindow: 60_000);

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/dapi/v1/userTrades", 5);
        var query = DecodedQuery(handler);
        Assert.Contains("pair=BTCUSD", query);
        Assert.DoesNotContain("symbol=", query);
        Assert.DoesNotContain("fromId=", query);
        Assert.DoesNotContain("orderId=", query);
        Assert.Contains($"startTime={new DateTimeOffset(startTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains($"endTime={new DateTimeOffset(endTime).ToUnixTimeMilliseconds()}", query);
        Assert.Contains("limit=1000", query);
        Assert.Contains("recvWindow=60000", query);

        var trade = Assert.Single(result.Data);
        Assert.Equal("BTCUSD_200626", trade.Symbol);
        Assert.Equal(9223372036854775806, trade.Id);
        Assert.Equal(9223372036854775805, trade.OrderId);
        Assert.Equal("BTCUSD", trade.Pair);
        Assert.Equal(BinanceOrderSide.Sell, trade.Side);
        Assert.Equal(8800m, trade.Price);
        Assert.Equal(1m, trade.Quantity);
        Assert.Equal(0m, trade.RealizedPnl);
        Assert.Equal("BTC", trade.MarginAsset);
        Assert.Equal(0.01136364m, trade.BaseQuantity);
        Assert.Equal(100m, trade.QuoteQuantity);
        Assert.Equal(0.00000454m, trade.Fee);
        Assert.Equal("BTC", trade.FeeAsset);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1590743483586).UtcDateTime, trade.Timestamp);
        Assert.Equal(BinancePositionSide.Both, trade.PositionSide);
        Assert.False(trade.Buyer);
        Assert.False(trade.Maker);
    }

    [Fact]
    public async Task CoinSymbolUserTrades_PreserveStringOrderId()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);

        var result = await client.CoinFutures.GetUserTradesAsync(
            symbol: "BTCUSD_PERP",
            orderId: "9223372036854775808");

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/dapi/v1/userTrades", 5);
        var query = DecodedQuery(handler);
        Assert.Contains("symbol=BTCUSD_PERP", query);
        Assert.Contains("orderId=9223372036854775808", query);
        Assert.DoesNotContain("pair=", query);
    }

    [Fact]
    public async Task UserTrades_RejectMissingOrAmbiguousScope()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetUserTradesAsync(null!));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetUserTradesAsync(""));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetUserTradesAsync(" "));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetUserTradesAsync());
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetUserTradesAsync(symbol: ""));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetUserTradesAsync(pair: " "));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetUserTradesAsync("BTCUSD_PERP", "BTCUSD"));
        Assert.Null(handler.RequestUri);
    }

    [Fact]
    public async Task UserTrades_RejectInvalidIdentifierCombinations()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);
        var time = DateTime.UtcNow;

        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetUserTradesAsync("BTCUSDT", startTime: time, fromId: 1));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetUserTradesAsync("BTCUSDT", endTime: time, fromId: 1));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetUserTradesAsync(symbol: "BTCUSD_PERP", startTime: time, fromId: 1));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetUserTradesAsync(symbol: "BTCUSD_PERP", endTime: time, fromId: 1));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetUserTradesAsync(pair: "BTCUSD", fromId: 1));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetUserTradesAsync(pair: "BTCUSD", orderId: "1"));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetUserTradesAsync(symbol: "BTCUSD_PERP", orderId: " "));
        Assert.Null(handler.RequestUri);
    }

    [Fact]
    public async Task UserTrades_RejectTimeRangeLongerThanSevenDays()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);
        var startTime = DateTime.UtcNow.AddDays(-8);
        var endTime = startTime.AddDays(7).AddMilliseconds(1);

        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.UsdFutures.GetUserTradesAsync("BTCUSDT", startTime, endTime));
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            client.CoinFutures.GetUserTradesAsync(symbol: "BTCUSD_PERP", startTime: startTime, endTime: endTime));
        Assert.Null(handler.RequestUri);
    }

    [Fact]
    public async Task UserTrades_RejectLimitOutsideCurrentRange()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler);

        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetUserTradesAsync("BTCUSDT", limit: 0));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.UsdFutures.GetUserTradesAsync("BTCUSDT", limit: 1001));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetUserTradesAsync(symbol: "BTCUSD_PERP", limit: 0));
        await Assert.ThrowsAnyAsync<ArgumentException>(() => client.CoinFutures.GetUserTradesAsync(symbol: "BTCUSD_PERP", limit: 1001));
        Assert.Null(handler.RequestUri);
    }

    [Fact]
    public async Task UserTrades_RejectReceiveWindowAboveCurrentMaximum()
    {
        var explicitHandler = new RecordingHttpMessageHandler("[]");
        using (var explicitClient = CreateClient(explicitHandler))
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.UsdFutures.GetUserTradesAsync("BTCUSDT", receiveWindow: 60_001));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.CoinFutures.GetUserTradesAsync(symbol: "BTCUSD_PERP", receiveWindow: 60_001));
        }
        Assert.Null(explicitHandler.RequestUri);

        var configuredHandler = new RecordingHttpMessageHandler("[]");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.GetUserTradesAsync("BTCUSDT"));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.CoinFutures.GetUserTradesAsync(symbol: "BTCUSD_PERP"));
        Assert.Null(configuredHandler.RequestUri);
    }

    private static string DecodedQuery(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.RequestUri!.Query);

    private static void AssertSignedGet(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        string path,
        int weight)
    {
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        Assert.Null(handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values));
        Assert.Contains("timestamp=", handler.RequestUri.Query);
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.Contains(limiter.Requests, request =>
            request.Endpoint == path && request.Weight == weight && request.Signed);
    }

    private static BinanceRestApiClient CreateClient(
        RecordingHttpMessageHandler handler,
        IRateLimiter? limiter = null,
        TimeSpan? defaultReceiveWindow = null)
    {
        var options = new BinanceRestApiClientOptions(new ApiCredentials("api-key", "api-secret"))
        {
            AutoTimestamp = false,
            HttpClient = new HttpClient(handler),
            RateLimiterEnabled = limiter != null,
            ReceiveWindow = defaultReceiveWindow
        };
#pragma warning disable CS0612
        options.RateLimiters = limiter == null ? [] : [limiter];
#pragma warning restore CS0612
        return new BinanceRestApiClient(options);
    }

    private sealed class RecordingRateLimiter : IRateLimiter
    {
        internal List<(string Endpoint, int Weight, bool Signed)> Requests { get; } = [];

        public Task<CallResult<int>> LimitRequestAsync(
            ILogger logger,
            string endpoint,
            HttpMethod method,
            bool signed,
            SensitiveString? apiKey,
            RateLimitingBehavior limitBehaviour,
            int requestWeight,
            CancellationToken ct)
        {
            Requests.Add((endpoint, requestWeight, signed));
            return Task.FromResult(new CallResult<int>(0));
        }
    }
}
