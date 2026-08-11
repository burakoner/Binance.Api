using System.Text.Json;
using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Options;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Options;

public class BinanceOptionsBlockTradeContractTests
{
    private const string MatchingKey = "7d085e6e-a229-2335-ab9d-6a581febcd25";
    private const string Symbol = "BNB-241101-700-C";
    private const string FullOrderResponse =
        """{"blockTradeSettlementKey":"settlement-key","expireTime":1730170445600,"liquidity":"TAKER","status":"RECEIVED","createTime":1730170445500,"legs":[{"symbol":"BNB-241101-700-C","side":"BUY","quantity":"1.25","price":"2.5"}]}""";

    [Fact]
    public async Task PlaceBlockOrder_UsesCurrentSingleLegBodyAndResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """{"blockTradeSettlementKey":"settlement-key","expireTime":1730170445600,"liquidity":"TAKER","status":"RECEIVED","legs":[{"symbol":"BNB-241101-700-C","side":"SELL","quantity":"1.25","price":"2.5"}]}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.MarketMaker.PlaceBlockOrderAsync(
            BinanceOptionsLiquidity.Taker,
            [new BinanceOptionsMarketMakerBlockOrderRequestLeg(Symbol, BinanceOrderSide.Sell, BinanceOptionsOrderType.Limit, 1.25m) { Price = 2.5m }],
            60_000);

        Assert.True(result.Success);
        Assert.Equal("settlement-key", result.Data.BlockTradeSettlementKey);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_730_170_445_600).UtcDateTime, result.Data.ExpireTime);
        Assert.Null(result.Data.CreateTime);
        Assert.Equal(BinanceOptionsLiquidity.Taker, result.Data.Liquidity);
        var responseLeg = Assert.Single(result.Data.Legs);
        Assert.Equal(BinanceOrderSide.Sell, responseLeg.Side);
        Assert.Equal(1.25m, responseLeg.Quantity);
        AssertSignedRequest(handler, limiter, HttpMethod.Post, "/eapi/v1/block/order/create", hasBody: true);

        var body = ParseBody(handler);
        Assert.Equal("TAKER", body["liquidity"]);
        Assert.Equal("60000", body["recvWindow"]);
        Assert.False(body.ContainsKey("orders"));
        using var legs = JsonDocument.Parse(body["legs"]);
        var leg = Assert.Single(legs.RootElement.EnumerateArray());
        Assert.Equal(Symbol, leg.GetProperty("symbol").GetString());
        Assert.Equal("SELL", leg.GetProperty("side").GetString());
        Assert.Equal("LIMIT", leg.GetProperty("type").GetString());
        Assert.Equal("1.25", leg.GetProperty("quantity").GetString());
        Assert.Equal("2.5", leg.GetProperty("price").GetString());
    }

    [Fact]
    public async Task CancelBlockOrder_UsesSignedQueryAndAcceptsEmptyResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("");
        using var client = CreateClient(handler, limiter);

        var result = await client.Options.MarketMaker.CancelBlockOrderAsync(MatchingKey, 60_000);

        Assert.True(result.Success);
        Assert.True(result.Data);
        AssertSignedRequest(handler, limiter, HttpMethod.Delete, "/eapi/v1/block/order/create", hasBody: false);
        var query = ParseQuery(handler.RequestUri!);
        Assert.Equal(MatchingKey, query["blockOrderMatchingKey"]);
        Assert.Equal("60000", query["recvWindow"]);
    }

    [Fact]
    public async Task ExtendAndAccept_UseCurrentSignedBodiesAndCompleteResponses()
    {
        var extendLimiter = new RecordingRateLimiter();
        var extendHandler = new RecordingHttpMessageHandler(FullOrderResponse);
        using (var client = CreateClient(extendHandler, extendLimiter))
        {
            var result = await client.Options.MarketMaker.ExtendBlockOrderAsync(MatchingKey, 60_000);

            Assert.True(result.Success);
            Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_730_170_445_500).UtcDateTime, result.Data.CreateTime);
            AssertSignedRequest(extendHandler, extendLimiter, HttpMethod.Put, "/eapi/v1/block/order/create", hasBody: true);
            AssertMatchingKeyBody(extendHandler);
        }

        var acceptLimiter = new RecordingRateLimiter();
        var acceptHandler = new RecordingHttpMessageHandler(FullOrderResponse);
        using var acceptClient = CreateClient(acceptHandler, acceptLimiter);

        var acceptResult = await acceptClient.Options.MarketMaker.AcceptBlockOrderAsync(MatchingKey, 60_000);

        Assert.True(acceptResult.Success);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_730_170_445_500).UtcDateTime, acceptResult.Data.CreateTime);
        AssertSignedRequest(acceptHandler, acceptLimiter, HttpMethod.Post, "/eapi/v1/block/order/execute", hasBody: true);
        AssertMatchingKeyBody(acceptHandler);
    }

    [Fact]
    public async Task QueryOrderAndDetails_UseDistinctCurrentContracts()
    {
        var startTime = DateTimeOffset.FromUnixTimeMilliseconds(1_730_000_000_000).UtcDateTime;
        var endTime = DateTimeOffset.FromUnixTimeMilliseconds(1_730_100_000_000).UtcDateTime;
        var ordersLimiter = new RecordingRateLimiter();
        var ordersHandler = new RecordingHttpMessageHandler($"[{FullOrderResponse}]");
        using (var client = CreateClient(ordersHandler, ordersLimiter))
        {
            var result = await client.Options.MarketMaker.GetBlockOrdersAsync(
                MatchingKey, "BTCUSDT", startTime, endTime, 60_000);

            Assert.True(result.Success);
            Assert.Single(result.Data);
            AssertSignedRequest(ordersHandler, ordersLimiter, HttpMethod.Get, "/eapi/v1/block/order/orders", hasBody: false);
            var query = ParseQuery(ordersHandler.RequestUri!);
            Assert.Equal(MatchingKey, query["blockOrderMatchingKey"]);
            Assert.Equal("BTCUSDT", query["underlying"]);
            Assert.Equal("1730000000000", query["startTime"]);
            Assert.Equal("1730100000000", query["endTime"]);
            Assert.Equal("60000", query["recvWindow"]);
        }

        var detailsLimiter = new RecordingRateLimiter();
        var detailsHandler = new RecordingHttpMessageHandler(FullOrderResponse);
        using var detailsClient = CreateClient(detailsHandler, detailsLimiter);

        var details = await detailsClient.Options.MarketMaker.GetBlockTradeDetailsAsync(MatchingKey, 60_000);

        Assert.True(details.Success);
        Assert.Equal(MatchingKey, ParseQuery(detailsHandler.RequestUri!)["blockOrderMatchingKey"]);
        AssertSignedRequest(detailsHandler, detailsLimiter, HttpMethod.Get, "/eapi/v1/block/order/execute", hasBody: false);
    }

    [Fact]
    public async Task InvalidInputsAndReceiveWindowsFailBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);
        var validLeg = new BinanceOptionsMarketMakerBlockOrderRequestLeg(Symbol, BinanceOrderSide.Buy, BinanceOptionsOrderType.Limit, 1);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.MarketMaker.PlaceBlockOrderAsync((BinanceOptionsLiquidity)0, [validLeg]));
        await Assert.ThrowsAsync<ArgumentNullException>(() => client.Options.MarketMaker.PlaceBlockOrderAsync(BinanceOptionsLiquidity.Maker, null!));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.PlaceBlockOrderAsync(BinanceOptionsLiquidity.Maker, []));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.PlaceBlockOrderAsync(BinanceOptionsLiquidity.Maker, [validLeg, validLeg]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.PlaceBlockOrderAsync(
            BinanceOptionsLiquidity.Maker,
            [new BinanceOptionsMarketMakerBlockOrderRequestLeg(" ", BinanceOrderSide.Buy, BinanceOptionsOrderType.Limit, 1)]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.MarketMaker.PlaceBlockOrderAsync(
            BinanceOptionsLiquidity.Maker,
            [new BinanceOptionsMarketMakerBlockOrderRequestLeg(Symbol, (BinanceOrderSide)0, BinanceOptionsOrderType.Limit, 1)]));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Options.MarketMaker.PlaceBlockOrderAsync(
            BinanceOptionsLiquidity.Maker,
            [new BinanceOptionsMarketMakerBlockOrderRequestLeg(Symbol, BinanceOrderSide.Buy, (BinanceOptionsOrderType)0, 1)]));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.CancelBlockOrderAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.ExtendBlockOrderAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.AcceptBlockOrderAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.GetBlockTradeDetailsAsync(" "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.GetBlockOrdersAsync(blockOrderMatchingKey: " "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.GetBlockOrdersAsync(underlying: " "));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Options.MarketMaker.GetBlockTradesAsync(" "));

        var receiveWindowCalls = new Func<Task>[]
        {
            () => client.Options.MarketMaker.PlaceBlockOrderAsync(BinanceOptionsLiquidity.Maker, [validLeg], 60_001),
            () => client.Options.MarketMaker.CancelBlockOrderAsync(MatchingKey, 60_001),
            () => client.Options.MarketMaker.ExtendBlockOrderAsync(MatchingKey, 60_001),
            () => client.Options.MarketMaker.GetBlockOrdersAsync(receiveWindow: 60_001),
            () => client.Options.MarketMaker.AcceptBlockOrderAsync(MatchingKey, 60_001),
            () => client.Options.MarketMaker.GetBlockTradeDetailsAsync(MatchingKey, 60_001),
            () => client.Options.MarketMaker.GetBlockTradesAsync(receiveWindow: 60_001)
        };
        foreach (var call in receiveWindowCalls)
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(call);

        var configuredHandler = new RecordingHttpMessageHandler("{}");
        using var configuredClient = CreateClient(configuredHandler, defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => configuredClient.Options.MarketMaker.GetBlockOrdersAsync());

        Assert.Equal(0, handler.RequestCount);
        Assert.Equal(0, configuredHandler.RequestCount);
    }

    [Fact]
    public void PublicContractsExposeCurrentTypesAndUnambiguousQueries()
    {
        var methods = typeof(IBinanceOptionsRestClientMarketMakerBlockTrade).GetMethods();
        Assert.All(methods, method =>
            Assert.Equal(typeof(long?), method.GetParameters().Single(parameter => parameter.Name == "receiveWindow").ParameterType));
        Assert.DoesNotContain(methods, method => method.Name == "GetBlockOrderAsync");
        Assert.Contains(methods, method => method.Name == nameof(IBinanceOptionsRestClientMarketMakerBlockTrade.GetBlockOrdersAsync));
        Assert.Contains(methods, method => method.Name == nameof(IBinanceOptionsRestClientMarketMakerBlockTrade.GetBlockTradeDetailsAsync));

        var place = methods.Single(method => method.Name == nameof(IBinanceOptionsRestClientMarketMakerBlockTrade.PlaceBlockOrderAsync));
        Assert.Equal(
            typeof(IEnumerable<BinanceOptionsMarketMakerBlockOrderRequestLeg>),
            place.GetParameters().Single(parameter => parameter.Name == "legs").ParameterType);
        Assert.Equal(typeof(DateTime?), typeof(BinanceOptionsMarketMakerBlockOrder).GetProperty(nameof(BinanceOptionsMarketMakerBlockOrder.CreateTime))!.PropertyType);
    }

    private static void AssertMatchingKeyBody(RecordingHttpMessageHandler handler)
    {
        var body = ParseBody(handler);
        Assert.Equal(MatchingKey, body["blockOrderMatchingKey"]);
        Assert.Equal("60000", body["recvWindow"]);
    }

    private static Dictionary<string, string> ParseBody(RecordingHttpMessageHandler handler)
        => ParseParameters(handler.Body!);

    private static Dictionary<string, string> ParseQuery(Uri uri)
        => ParseParameters(uri.Query.TrimStart('?'));

    private static Dictionary<string, string> ParseParameters(string parameters)
        => parameters.Split('&')
            .Select(value => value.Split('=', 2))
            .ToDictionary(value => Uri.UnescapeDataString(value[0]), value => Uri.UnescapeDataString(value[1]));

    private static void AssertSignedRequest(
        RecordingHttpMessageHandler handler,
        RecordingRateLimiter limiter,
        HttpMethod method,
        string path,
        bool hasBody)
    {
        Assert.Equal(method, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Equal(hasBody, handler.Body != null);
        Assert.Equal(hasBody ? "application/x-www-form-urlencoded" : null, handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == path && item.Weight == 5 && item.Signed);
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
