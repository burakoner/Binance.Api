using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesRestClientUsdAlgoQueryTests
{
    [Fact]
    public async Task GetAlgoOrderAsync_UsesCurrentSignedContractAndDeserializesQueryShape()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "algoId": 2146760,
              "clientAlgoId": "6B2I9XVcJpCjqPAJ4YoFX7",
              "algoType": "CONDITIONAL",
              "orderType": "TAKE_PROFIT",
              "symbol": "BNBUSDT",
              "side": "SELL",
              "positionSide": "BOTH",
              "timeInForce": "GTC",
              "quantity": "0.01",
              "algoStatus": "CANCELED",
              "actualOrderId": "22542179",
              "actualPrice": "749.50000",
              "actualType": "LIMIT",
              "actualQty": "0.01",
              "triggerPrice": "750.000",
              "price": "750.000",
              "icebergQuantity": "null",
              "tpOrderType": "",
              "selfTradePreventionMode": "EXPIRE_MAKER",
              "workingType": "CONTRACT_PRICE",
              "priceMatch": "NONE",
              "closePosition": false,
              "priceProtect": true,
              "reduceOnly": false,
              "createTime": 1750485492076,
              "updateTime": 1750514545091,
              "triggerTime": 1750514500000,
              "goodTillDate": 0
            }
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetAlgoOrderAsync(2146760, receiveWindow: 5_000);

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/fapi/v1/algoOrder", 1);
        var query = DecodedQuery(handler);
        Assert.Contains("algoId=2146760", query);
        Assert.DoesNotContain("clientAlgoId=", query);
        Assert.Contains("recvWindow=5000", query);

        Assert.Equal(2146760, result.Data.AlgoId);
        Assert.Equal("6B2I9XVcJpCjqPAJ4YoFX7", result.Data.ClientAlgoId);
        Assert.Equal("CONDITIONAL", result.Data.AlgoType);
        Assert.Equal("TAKE_PROFIT", result.Data.OrderType);
        Assert.Equal("BNBUSDT", result.Data.Symbol);
        Assert.Equal("SELL", result.Data.Side);
        Assert.Equal("BOTH", result.Data.PositionSide);
        Assert.Equal("GTC", result.Data.TimeInForce);
        Assert.Equal(0.01m, result.Data.Quantity);
        Assert.Equal("CANCELED", result.Data.Status);
        Assert.Equal("22542179", result.Data.ActualOrderId);
        Assert.Equal(749.5m, result.Data.ActualPrice);
        Assert.Equal("LIMIT", result.Data.ActualOrderType);
        Assert.Equal(0.01m, result.Data.ActualQuantity);
        Assert.Equal(750m, result.Data.TriggerPrice);
        Assert.Equal(750m, result.Data.Price);
        Assert.Equal("null", result.Data.IcebergQuantity);
        Assert.Equal(string.Empty, result.Data.TakeProfitOrderType);
        Assert.Equal("EXPIRE_MAKER", result.Data.SelfTradePreventionMode);
        Assert.Equal("CONTRACT_PRICE", result.Data.WorkingType);
        Assert.Equal("NONE", result.Data.PriceMatch);
        Assert.False(result.Data.ClosePosition);
        Assert.True(result.Data.PriceProtect);
        Assert.False(result.Data.ReduceOnly);
        Assert.Equal(1750485492076, result.Data.CreateTime);
        Assert.Equal(1750514545091, result.Data.UpdateTime);
        Assert.Equal(1750514500000, result.Data.TriggerTime);
        Assert.Equal(0, result.Data.GoodTillDate);
    }

    [Fact]
    public async Task GetAlgoOrderAsync_UsesClientAlgoIdAlternative()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""{"algoId":2146760,"clientAlgoId":"client-1"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetAlgoOrderAsync(clientAlgoId: "client-1");

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/fapi/v1/algoOrder", 1);
        var query = DecodedQuery(handler);
        Assert.Contains("clientAlgoId=client-1", query);
        Assert.DoesNotContain("algoId=", query);
    }

    [Fact]
    public async Task GetAlgoOrderAsync_RequiresUsableIdentifier()
    {
        var handler = new RecordingHttpMessageHandler("{}");
        using var client = CreateClient(handler);

        await Assert.ThrowsAsync<ArgumentException>(() => client.UsdFutures.GetAlgoOrderAsync());
        await Assert.ThrowsAsync<ArgumentException>(() => client.UsdFutures.GetAlgoOrderAsync(clientAlgoId: " "));
        Assert.Null(handler.RequestUri);
    }

    [Fact]
    public async Task GetOpenAlgoOrdersAsync_UsesSymbolWeightAndDeserializesOpenShape()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler(
            """
            [
              {
                "algoId": 2148627,
                "clientAlgoId": "MRumok0dkhrP4kCm12AHaB",
                "algoType": "CONDITIONAL",
                "orderType": "TAKE_PROFIT",
                "symbol": "BNBUSDT",
                "side": "SELL",
                "positionSide": "BOTH",
                "timeInForce": "GTC",
                "quantity": "0.01",
                "algoStatus": "NEW",
                "actualOrderId": "",
                "actualPrice": "0.00000",
                "triggerPrice": "750.000",
                "price": "750.000",
                "icebergQuantity": "null",
                "tpTriggerPrice": "760.000",
                "tpPrice": "759.500",
                "slTriggerPrice": "740.000",
                "slPrice": "739.500",
                "tpOrderType": "",
                "selfTradePreventionMode": "EXPIRE_MAKER",
                "workingType": "CONTRACT_PRICE",
                "priceMatch": "NONE",
                "closePosition": false,
                "priceProtect": false,
                "reduceOnly": false,
                "createTime": 1750514941540,
                "updateTime": 1750514941540,
                "triggerTime": 0,
                "goodTillDate": 0
              }
            ]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetOpenAlgoOrdersAsync(
            "BNBUSDT",
            "CONDITIONAL",
            2148627,
            60_000);

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/fapi/v1/openAlgoOrders", 1);
        var query = DecodedQuery(handler);
        Assert.Contains("symbol=BNBUSDT", query);
        Assert.Contains("algoType=CONDITIONAL", query);
        Assert.Contains("algoId=2148627", query);
        Assert.Contains("recvWindow=60000", query);

        var order = Assert.Single(result.Data);
        Assert.Equal(2148627, order.AlgoId);
        Assert.Equal("MRumok0dkhrP4kCm12AHaB", order.ClientAlgoId);
        Assert.Equal("CONDITIONAL", order.AlgoType);
        Assert.Equal("TAKE_PROFIT", order.OrderType);
        Assert.Equal("BNBUSDT", order.Symbol);
        Assert.Equal("SELL", order.Side);
        Assert.Equal("BOTH", order.PositionSide);
        Assert.Equal("GTC", order.TimeInForce);
        Assert.Equal(0.01m, order.Quantity);
        Assert.Equal("NEW", order.Status);
        Assert.Equal(string.Empty, order.ActualOrderId);
        Assert.Equal(0m, order.ActualPrice);
        Assert.Equal(750m, order.TriggerPrice);
        Assert.Equal(750m, order.Price);
        Assert.Equal("null", order.IcebergQuantity);
        Assert.Equal(760m, order.TakeProfitTriggerPrice);
        Assert.Equal(759.5m, order.TakeProfitPrice);
        Assert.Equal(740m, order.StopLossTriggerPrice);
        Assert.Equal(739.5m, order.StopLossPrice);
        Assert.Equal(string.Empty, order.TakeProfitOrderType);
        Assert.Equal("EXPIRE_MAKER", order.SelfTradePreventionMode);
        Assert.Equal("CONTRACT_PRICE", order.WorkingType);
        Assert.Equal("NONE", order.PriceMatch);
        Assert.False(order.ClosePosition);
        Assert.False(order.PriceProtect);
        Assert.False(order.ReduceOnly);
        Assert.Equal(1750514941540, order.CreateTime);
        Assert.Equal(1750514941540, order.UpdateTime);
        Assert.Equal(0, order.TriggerTime);
        Assert.Equal(0, order.GoodTillDate);
    }

    [Fact]
    public async Task GetOpenAlgoOrdersAsync_UsesAllSymbolsWeightWhenSymbolIsOmitted()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);

        var result = await client.UsdFutures.GetOpenAlgoOrdersAsync();

        Assert.True(result.Success);
        AssertSignedGet(handler, limiter, "/fapi/v1/openAlgoOrders", 40);
        var query = DecodedQuery(handler);
        Assert.DoesNotContain("symbol=", query);
        Assert.DoesNotContain("algoType=", query);
        Assert.DoesNotContain("algoId=", query);
    }

    [Fact]
    public async Task GetOpenAlgoOrdersAsync_RejectsInvalidFiltersAndReceiveWindow()
    {
        var explicitHandler = new RecordingHttpMessageHandler("[]");
        using (var explicitClient = CreateClient(explicitHandler))
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                explicitClient.UsdFutures.GetOpenAlgoOrdersAsync(symbol: " "));
            await Assert.ThrowsAsync<ArgumentException>(() =>
                explicitClient.UsdFutures.GetOpenAlgoOrdersAsync(algoType: " "));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                explicitClient.UsdFutures.GetOpenAlgoOrdersAsync(receiveWindow: 60_001));
        }
        Assert.Null(explicitHandler.RequestUri);

        var configuredHandler = new RecordingHttpMessageHandler("[]");
        using var configuredClient = CreateClient(
            configuredHandler,
            defaultReceiveWindow: TimeSpan.FromMilliseconds(60_001));

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            configuredClient.UsdFutures.GetOpenAlgoOrdersAsync());
        Assert.Null(configuredHandler.RequestUri);
    }

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
        Assert.Equal("api-key", Assert.Single(values!));
        var query = DecodedQuery(handler);
        Assert.Contains("timestamp=", query);
        Assert.Contains("signature=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == path && item.Weight == weight && item.Signed);
    }

    private static string DecodedQuery(RecordingHttpMessageHandler handler)
        => Uri.UnescapeDataString(handler.RequestUri!.Query);

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
