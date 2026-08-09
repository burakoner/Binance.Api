using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Futures;
using Binance.Api.Shared;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdAlgoPlacementTests
{
    [Fact]
    public async Task PlaceAlgoOrderAsync_UsesSignedFormBodyAndDeserializesCompleteResponse()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "algoId": 2147676000001,
              "clientAlgoId": "client.algo-1",
              "algoType": "CONDITIONAL",
              "orderType": "STOP",
              "symbol": "BTCUSDT",
              "side": "SELL",
              "positionSide": "BOTH",
              "timeInForce": "GTD",
              "quantity": "1.25",
              "algoStatus": "NEW",
              "triggerPrice": "50000.5",
              "price": "0",
              "icebergQuantity": "null",
              "selfTradePreventionMode": "EXPIRE_MAKER",
              "workingType": "MARK_PRICE",
              "priceMatch": "OPPONENT_5",
              "closePosition": false,
              "priceProtect": false,
              "reduceOnly": false,
              "activatePrice": "",
              "callbackRate": "",
              "createTime": 1750485492076,
              "updateTime": 1750485492077,
              "triggerTime": 0,
              "goodTillDate": 1750489092000
            }
            """);
        using var client = CreateClient(handler, limiter);
        var goodTillDate = DateTimeOffset.FromUnixTimeMilliseconds(
            DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeMilliseconds()).UtcDateTime;

        var result = await client.UsdFutures.PlaceAlgoOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Sell,
            BinanceFuturesAlgoOrderType.Stop,
            positionSide: BinancePositionSide.Both,
            timeInForce: BinanceTimeInForce.GoodTillDate,
            quantity: 1.25m,
            triggerPrice: 50_000.5m,
            workingType: BinanceFuturesWorkingType.Mark,
            priceMatch: BinanceFuturesPriceMatch.Opponent5,
            reduceOnly: false,
            clientAlgoId: "client.algo-1",
            orderResponseType: BinanceOrderResponseType.Result,
            selfTradePreventionMode: BinanceSelfTradePreventionMode.ExpireMaker,
            goodTillDate: goodTillDate,
            receiveWindow: 60_001);

        Assert.True(result.Success);
        Assert.Equal(2_147_676_000_001L, result.Data!.AlgoId);
        Assert.Equal("client.algo-1", result.Data.ClientAlgoId);
        Assert.Equal("CONDITIONAL", result.Data.AlgoType);
        Assert.Equal("STOP", result.Data.OrderType);
        Assert.Equal("BTCUSDT", result.Data.Symbol);
        Assert.Equal("SELL", result.Data.Side);
        Assert.Equal("BOTH", result.Data.PositionSide);
        Assert.Equal("GTD", result.Data.TimeInForce);
        Assert.Equal(1.25m, result.Data.Quantity);
        Assert.Equal("NEW", result.Data.Status);
        Assert.Equal(50_000.5m, result.Data.TriggerPrice);
        Assert.Equal(0m, result.Data.Price);
        Assert.Equal("null", result.Data.IcebergQuantity);
        Assert.Equal("EXPIRE_MAKER", result.Data.SelfTradePreventionMode);
        Assert.Equal("MARK_PRICE", result.Data.WorkingType);
        Assert.Equal("OPPONENT_5", result.Data.PriceMatch);
        Assert.False(result.Data.ClosePosition);
        Assert.False(result.Data.PriceProtect);
        Assert.False(result.Data.ReduceOnly);
        Assert.Equal(string.Empty, result.Data.ActivatePrice);
        Assert.Equal(string.Empty, result.Data.CallbackRate);
        Assert.Equal(1_750_485_492_076L, result.Data.CreateTime);
        Assert.Equal(1_750_485_492_077L, result.Data.UpdateTime);
        Assert.Equal(0L, result.Data.TriggerTime);
        Assert.Equal(1_750_489_092_000L, result.Data.GoodTillDate);

        AssertSignedPost(handler, limiter, "/fapi/v1/algoOrder", 0);
        var body = ParseBody(handler);
        Assert.Equal("CONDITIONAL", body["algoType"]);
        Assert.Equal("BTCUSDT", body["symbol"]);
        Assert.Equal("SELL", body["side"]);
        Assert.Equal("STOP", body["type"]);
        Assert.Equal("BOTH", body["positionSide"]);
        Assert.Equal("GTD", body["timeInForce"]);
        Assert.Equal("1.25", body["quantity"]);
        Assert.Equal("50000.5", body["triggerPrice"]);
        Assert.Equal("MARK_PRICE", body["workingType"]);
        Assert.Equal("OPPONENT_5", body["priceMatch"]);
        Assert.Equal("false", body["reduceOnly"]);
        Assert.Equal("client.algo-1", body["clientAlgoId"]);
        Assert.Equal("RESULT", body["newOrderRespType"]);
        Assert.Equal("EXPIRE_MAKER", body["selfTradePreventionMode"]);
        Assert.Equal(new DateTimeOffset(goodTillDate).ToUnixTimeMilliseconds().ToString(), body["goodTillDate"]);
        Assert.Equal("60001", body["recvWindow"]);
        Assert.False(body.ContainsKey("price"));
    }

    [Fact]
    public async Task PlaceAlgoOrderAsync_SerializesTrailingParametersAndConfiguredReceiveWindow()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler, defaultReceiveWindow: TimeSpan.FromMilliseconds(90_000));

        var result = await client.UsdFutures.PlaceAlgoOrderAsync(
            "ETHUSDT",
            BinanceOrderSide.Sell,
            BinanceFuturesAlgoOrderType.TrailingStopMarket,
            quantity: 2.5m,
            workingType: BinanceFuturesWorkingType.Contract,
            activatePrice: 3_500.25m,
            callbackRate: 0.1m);

        Assert.True(result.Success);
        var body = ParseBody(handler);
        Assert.Equal("TRAILING_STOP_MARKET", body["type"]);
        Assert.Equal("2.5", body["quantity"]);
        Assert.Equal("CONTRACT_PRICE", body["workingType"]);
        Assert.Equal("3500.25", body["activatePrice"]);
        Assert.Equal("0.1", body["callbackRate"]);
        Assert.Equal("90000", body["recvWindow"]);
        Assert.False(body.ContainsKey("clientAlgoId"));
    }

    [Fact]
    public async Task PlaceAlgoOrderAsync_SerializesCloseAllMarketOrder()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);

        var result = await client.UsdFutures.PlaceAlgoOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            BinanceFuturesAlgoOrderType.StopMarket,
            positionSide: BinancePositionSide.Short,
            triggerPrice: 40_000m,
            closePosition: true,
            priceProtect: true);

        Assert.True(result.Success);
        var body = ParseBody(handler);
        Assert.Equal("STOP_MARKET", body["type"]);
        Assert.Equal("SHORT", body["positionSide"]);
        Assert.Equal("true", body["closePosition"]);
        Assert.Equal("true", body["priceProtect"]);
        Assert.False(body.ContainsKey("quantity"));
        Assert.False(body.ContainsKey("reduceOnly"));
    }

    [Fact]
    public async Task PlaceAlgoOrderAsync_SerializesCurrentRpiTimeInForce()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);

        var result = await client.UsdFutures.PlaceAlgoOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            BinanceFuturesAlgoOrderType.TakeProfit,
            timeInForce: BinanceTimeInForce.RetailPriceImprovement,
            price: 61_000m,
            triggerPrice: 60_000m);

        Assert.True(result.Success);
        var body = ParseBody(handler);
        Assert.Equal("RPI", body["timeInForce"]);
        Assert.Equal("61000", body["price"]);
    }

    [Fact]
    public async Task PlaceAlgoOrderAsync_RejectsDocumentedInvalidCombinationsBeforeTransport()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);
        Task Call(
            BinanceFuturesAlgoOrderType type = BinanceFuturesAlgoOrderType.Stop,
            BinanceOrderSide side = BinanceOrderSide.Buy,
            BinancePositionSide? positionSide = null,
            BinanceTimeInForce? timeInForce = null,
            decimal? quantity = null,
            decimal? price = null,
            BinanceFuturesPriceMatch? priceMatch = null,
            bool? closePosition = null,
            bool? priceProtect = null,
            bool? reduceOnly = null,
            decimal? activatePrice = null,
            decimal? callbackRate = null,
            string? clientAlgoId = null,
            BinanceOrderResponseType? responseType = null,
            BinanceSelfTradePreventionMode? stp = null,
            DateTime? goodTillDate = null)
            => client.UsdFutures.PlaceAlgoOrderAsync("BTCUSDT", side, type, positionSide, timeInForce, quantity, price, priceMatch: priceMatch, closePosition: closePosition, priceProtect: priceProtect, reduceOnly: reduceOnly, activatePrice: activatePrice, callbackRate: callbackRate, clientAlgoId: clientAlgoId, orderResponseType: responseType, selfTradePreventionMode: stp, goodTillDate: goodTillDate);

        await Assert.ThrowsAsync<ArgumentException>(() => client.UsdFutures.PlaceAlgoOrderAsync(" ", BinanceOrderSide.Buy, BinanceFuturesAlgoOrderType.Stop));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.PlaceAlgoOrderAsync("BTCUSDT", (BinanceOrderSide)0, BinanceFuturesAlgoOrderType.Stop));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Call((BinanceFuturesAlgoOrderType)0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Call(positionSide: BinancePositionSide.Hedge));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Call(timeInForce: BinanceTimeInForce.GoodTillExpiredOrCanceled));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Call(responseType: BinanceOrderResponseType.Full));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Call(priceMatch: BinanceFuturesPriceMatch.None));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Call(stp: BinanceSelfTradePreventionMode.Decrement));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(closePosition: false));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(priceProtect: false));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(BinanceFuturesAlgoOrderType.StopMarket, quantity: 1, closePosition: true));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(BinanceFuturesAlgoOrderType.StopMarket, closePosition: true, reduceOnly: false));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(positionSide: BinancePositionSide.Long, reduceOnly: false));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(BinanceFuturesAlgoOrderType.StopMarket, positionSide: BinancePositionSide.Long, closePosition: true));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(BinanceFuturesAlgoOrderType.StopMarket, BinanceOrderSide.Sell, BinancePositionSide.Short, closePosition: true));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(BinanceFuturesAlgoOrderType.TrailingStopMarket, priceMatch: BinanceFuturesPriceMatch.Opponent));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(price: 1, priceMatch: BinanceFuturesPriceMatch.Opponent));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(activatePrice: 1));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Call(BinanceFuturesAlgoOrderType.TrailingStopMarket, callbackRate: 0.09m));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Call(BinanceFuturesAlgoOrderType.TrailingStopMarket, callbackRate: 10.01m));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(clientAlgoId: string.Empty));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(clientAlgoId: "invalid?"));
        await Assert.ThrowsAsync<ArgumentException>(() => Call(timeInForce: BinanceTimeInForce.GoodTillDate));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Call(timeInForce: BinanceTimeInForce.GoodTillDate, goodTillDate: DateTime.UtcNow.AddSeconds(600)));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Call(timeInForce: BinanceTimeInForce.GoodTillDate, goodTillDate: DateTimeOffset.FromUnixTimeMilliseconds(253_402_300_799_000L).UtcDateTime));
        Assert.Null(handler.RequestUri);
    }

    private static Dictionary<string, string> ParseBody(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.Body!).Split('&').Select(value => value.Split('=', 2)).ToDictionary(value => value[0], value => value[1]);

    private static void AssertSignedPost(RecordingHttpMessageHandler handler, RecordingRateLimiter limiter, string path, int weight)
    {
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal(path, handler.RequestUri!.AbsolutePath);
        Assert.Equal("application/x-www-form-urlencoded", handler.ContentType);
        Assert.True(handler.Headers.TryGetValue("X-MBX-APIKEY", out var values));
        Assert.Equal("api-key", Assert.Single(values!));
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("signature=", query);
        Assert.DoesNotContain("timestamp=", query);
        Assert.DoesNotContain("symbol=", query);
        Assert.Contains("timestamp=", handler.Body);
        Assert.DoesNotContain("signature=", handler.Body);
        Assert.Contains(limiter.Requests, item => item.Endpoint == path && item.Weight == weight && item.Signed);
    }

    private static BinanceRestApiClient CreateClient(RecordingHttpMessageHandler handler, IRateLimiter? limiter = null, TimeSpan? defaultReceiveWindow = null)
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

        public Task<CallResult<int>> LimitRequestAsync(ILogger logger, string endpoint, HttpMethod method, bool signed, SensitiveString? apiKey, RateLimitingBehavior limitBehaviour, int requestWeight, CancellationToken ct)
        {
            Requests.Add((endpoint, requestWeight, signed));
            return Task.FromResult(new CallResult<int>(0));
        }
    }
}
