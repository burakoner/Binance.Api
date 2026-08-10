using Binance.Api.Futures;
using Binance.Api.Shared;
using Newtonsoft.Json.Linq;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdTradeTests
{
    [Theory]
    [InlineData(BinanceFuturesOrderType.Stop)]
    [InlineData(BinanceFuturesOrderType.StopMarket)]
    [InlineData(BinanceFuturesOrderType.TakeProfit)]
    [InlineData(BinanceFuturesOrderType.TakeProfitMarket)]
    [InlineData(BinanceFuturesOrderType.TrailingStopMarket)]
    [InlineData(BinanceFuturesOrderType.Liquidation)]
    [InlineData((BinanceFuturesOrderType)byte.MaxValue)]
    public async Task PlaceOrderAsync_RejectsNonNormalTypesBeforeTransport(BinanceFuturesOrderType type)
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.PlaceOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            type,
            quantity: 1));

        Assert.Equal(0, handler.RequestCount);
    }

    [Theory]
    [InlineData(BinanceFuturesOrderType.Limit, "LIMIT")]
    [InlineData(BinanceFuturesOrderType.Market, "MARKET")]
    public async Task PlaceOrderAsync_AllowsCurrentNormalTypes(BinanceFuturesOrderType type, string wireType)
    {
        var handler = new RecordingHttpMessageHandler("{\"orderId\":1}");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        var result = await client.UsdFutures.PlaceOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            type,
            quantity: 1,
            price: type == BinanceFuturesOrderType.Limit ? 50_000 : null,
            timeInForce: type == BinanceFuturesOrderType.Limit ? BinanceTimeInForce.GoodTillCanceled : null);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal("/fapi/v1/order", handler.RequestUri!.AbsolutePath);
        Assert.Contains($"type={wireType}", Uri.UnescapeDataString(handler.Body!));
    }

    [Fact]
    public void PlaceOrderAsync_PublicContractDoesNotExposeConditionalParameters()
    {
        var parameterNames = typeof(IBinanceFuturesRestClientUsdTrade)
            .GetMethod(nameof(IBinanceFuturesRestClientUsdTrade.PlaceOrderAsync))!
            .GetParameters()
            .Select(parameter => parameter.Name)
            .ToList();

        Assert.DoesNotContain("stopPrice", parameterNames);
        Assert.DoesNotContain("activationPrice", parameterNames);
        Assert.DoesNotContain("callbackRate", parameterNames);
        Assert.DoesNotContain("workingType", parameterNames);
        Assert.DoesNotContain("closePosition", parameterNames);
        Assert.DoesNotContain("priceProtect", parameterNames);
    }

    [Theory]
    [InlineData(BinanceFuturesOrderType.Stop)]
    [InlineData(BinanceFuturesOrderType.StopMarket)]
    [InlineData(BinanceFuturesOrderType.TakeProfit)]
    [InlineData(BinanceFuturesOrderType.TakeProfitMarket)]
    [InlineData(BinanceFuturesOrderType.TrailingStopMarket)]
    [InlineData(BinanceFuturesOrderType.Liquidation)]
    [InlineData((BinanceFuturesOrderType)byte.MaxValue)]
    public async Task PlaceOrdersAsync_RejectsNonNormalTypesBeforeTransport(BinanceFuturesOrderType type)
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.PlaceOrdersAsync(
            [CreateBatchOrder(BinanceFuturesOrderType.Limit), CreateBatchOrder(type)]));

        Assert.Equal(0, handler.RequestCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public async Task PlaceOrdersAsync_RejectsInvalidBatchSizeBeforeTransport(int count)
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);
        var orders = Enumerable.Range(0, count).Select(_ => CreateBatchOrder(BinanceFuturesOrderType.Market));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.PlaceOrdersAsync(orders));

        Assert.Equal(0, handler.RequestCount);
    }

    [Fact]
    public void BatchOrderRequest_PublicContractDoesNotExposeConditionalParameters()
    {
        var propertyNames = typeof(BinanceFuturesBatchOrderRequest)
            .GetProperties()
            .Select(property => property.Name)
            .ToList();

        Assert.DoesNotContain("StopPrice", propertyNames);
        Assert.DoesNotContain("ActivationPrice", propertyNames);
        Assert.DoesNotContain("CallbackRate", propertyNames);
        Assert.DoesNotContain("WorkingType", propertyNames);
        Assert.DoesNotContain("PriceProtect", propertyNames);
    }

    [Fact]
    public async Task PlaceOrdersAsync_AllowsOneToFiveCurrentNormalOrders()
    {
        var handler = new RecordingHttpMessageHandler("[]");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        var result = await client.UsdFutures.PlaceOrdersAsync(
            [CreateBatchOrder(BinanceFuturesOrderType.Limit), CreateBatchOrder(BinanceFuturesOrderType.Market)]);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal("/fapi/v1/batchOrders", handler.RequestUri!.AbsolutePath);
        var body = ParseBody(handler);
        var batch = JArray.Parse(body["batchOrders"]);
        Assert.Equal("LIMIT", batch[0]!["type"]!.Value<string>());
        Assert.Equal("MARKET", batch[1]!["type"]!.Value<string>());
    }

    [Fact]
    public async Task PlaceTestOrderAsync_RetainsDocumentedConditionalTypeContract()
    {
        var handler = new RecordingHttpMessageHandler("{\"orderId\":1}");
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);

        var result = await client.UsdFutures.PlaceTestOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            BinanceFuturesOrderType.Stop,
            quantity: 1,
            price: 50_000,
            stopPrice: 49_000,
            timeInForce: BinanceTimeInForce.GoodTillCanceled);

        Assert.True(result.Success);
        Assert.Equal("/fapi/v1/order/test", handler.RequestUri!.AbsolutePath);
        var body = ParseBody(handler);
        Assert.Equal("STOP", body["type"]);
        Assert.Equal("49000", body["stopPrice"]);
    }

    [Fact]
    public async Task GetMarginChangeHistoryAsync_UsesV1QueryContractAndDeserializesResponse()
    {
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "symbol": "BTCUSDT",
                "type": 1,
                "deltaType": "USER_ADJUST",
                "amount": "23.36332311",
                "asset": "USDT",
                "time": 1578047897183,
                "positionSide": "BOTH"
              }
            ]
            """);
        using var httpClient = new HttpClient(handler);
        using var client = CreateClient(httpClient);
        var startTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var endTime = startTime.AddDays(30);

        var result = await client.UsdFutures.GetMarginChangeHistoryAsync(
            "BTCUSDT",
            BinanceFuturesMarginChangeDirectionType.Add,
            startTime,
            endTime,
            limit: 25,
            receiveWindow: 5_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/positionMargin/history", handler.RequestUri!.AbsolutePath);
        Assert.Contains("symbol=BTCUSDT", handler.RequestUri.Query);
        Assert.Contains("type=1", handler.RequestUri.Query);
        Assert.Contains("startTime=1767225600000", handler.RequestUri.Query);
        Assert.Contains("endTime=1769817600000", handler.RequestUri.Query);
        Assert.Contains("limit=25", handler.RequestUri.Query);
        Assert.Contains("recvWindow=5000", handler.RequestUri.Query);
        Assert.Null(handler.Body);

        var item = Assert.Single(result.Data);
        Assert.Equal("BTCUSDT", item.Symbol);
        Assert.Equal(BinanceFuturesMarginChangeDirectionType.Add, item.Type);
        Assert.Equal("USER_ADJUST", item.DeltaType);
        Assert.Equal(23.36332311m, item.Quantity);
        Assert.Equal("USDT", item.Asset);
        Assert.Equal(BinancePositionSide.Both, item.PositionSide);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(31)]
    public async Task GetMarginChangeHistoryAsync_RejectsInvalidTimeRange(int endDayOffset)
    {
        using var httpClient = new HttpClient(new RecordingHttpMessageHandler("[]"));
        using var client = CreateClient(httpClient);
        var startTime = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.UsdFutures.GetMarginChangeHistoryAsync(
            "BTCUSDT",
            startTime: startTime,
            endTime: startTime.AddDays(endDayOffset)));
    }

    [Fact]
    public async Task GetPositionAdlQuantileEstimationAsync_SendsQueryAndReadsArrayForSymbol()
    {
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "symbol": "ETHUSDT",
                "adlQuantile": {
                  "LONG": 3,
                  "SHORT": 2,
                  "HEDGE": 0,
                  "BOTH": 1
                }
              }
            ]
            """);
        using var httpClient = new HttpClient(handler);
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });

        var result = await client.UsdFutures.GetPositionAdlQuantileEstimationAsync(
            "ETHUSDT",
            receiveWindow: 5_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/fapi/v1/adlQuantile", handler.RequestUri!.AbsolutePath);
        Assert.Contains("symbol=ETHUSDT", handler.RequestUri.Query);
        Assert.Contains("recvWindow=5000", handler.RequestUri.Query);
        Assert.Contains("timestamp=", handler.RequestUri.Query);
        Assert.Contains("signature=", handler.RequestUri.Query);
        Assert.Null(handler.Body);
        var estimation = Assert.Single(result.Data);
        Assert.Equal("ETHUSDT", estimation.Symbol);
        Assert.Equal(3, estimation.AdlQuantile!.Long);
        Assert.Equal(2, estimation.AdlQuantile.Short);
        Assert.Equal(0, estimation.AdlQuantile.Hedge);
        Assert.Equal(1, estimation.AdlQuantile.Both);
    }

    private static BinanceRestApiClient CreateClient(HttpClient httpClient)
        => new(new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });

    private static BinanceFuturesBatchOrderRequest CreateBatchOrder(BinanceFuturesOrderType type)
        => new()
        {
            Symbol = "BTCUSDT",
            Side = BinanceOrderSide.Buy,
            Type = type,
            Quantity = 1,
            Price = type == BinanceFuturesOrderType.Limit ? 50_000 : null,
            TimeInForce = type == BinanceFuturesOrderType.Limit ? BinanceTimeInForce.GoodTillCanceled : null
        };

    private static Dictionary<string, string> ParseBody(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.Body!).Split('&').Select(value => value.Split('=', 2)).ToDictionary(value => value[0], value => value[1]);
}
