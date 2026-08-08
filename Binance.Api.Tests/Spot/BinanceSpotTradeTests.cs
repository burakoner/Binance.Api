using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Shared;
using Binance.Api.Spot;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Spot;

public class BinanceSpotTradeTests
{
    [Fact]
    public async Task NewOrder_SendsCurrentPegAndReceiveWindowParameters()
    {
        var handler = new RecordingHttpMessageHandler("""{"orderId":1}""");
        using var client = CreateClient(handler);

        var result = await client.Spot.PlaceOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            BinanceSpotOrderType.Limit,
            quantity: 0.5m,
            timeInForce: BinanceTimeInForce.GoodTillCanceled,
            strategyType: 1_000_000,
            pegPriceType: BinanceSpotPegPriceType.Primary,
            pegOffsetValue: 5,
            pegOffsetType: BinanceSpotPegOffsetType.PriceLevel,
            receiveWindow: 6_000.346m);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Post, handler.Method);
        Assert.Equal("/api/v3/order", handler.RequestUri!.AbsolutePath);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("pegPriceType=PRIMARY_PEG", body);
        Assert.Contains("pegOffsetValue=5", body);
        Assert.Contains("pegOffsetType=PRICE_LEVEL", body);
        Assert.Contains("recvWindow=6000.346", body);
    }

    [Fact]
    public async Task CancelAndReplace_SendCurrentRestrictionsAndRateLimitBehavior()
    {
        var cancelHandler = new RecordingHttpMessageHandler("""{"orderId":1}""");
        using (var client = CreateClient(cancelHandler))
        {
            var cancel = await client.Spot.CancelOrderAsync(
                "BTCUSDT",
                orderId: 1,
                origClientOrderId: "original",
                cancelRestriction: BinanceSpotOrderCancelRestriction.OnlyNew,
                receiveWindow: 5_000.125m);

            Assert.True(cancel.Success);
            var body = Uri.UnescapeDataString(cancelHandler.Body!);
            Assert.Contains("orderId=1", body);
            Assert.Contains("origClientOrderId=original", body);
            Assert.Contains("cancelRestrictions=ONLY_NEW", body);
            Assert.Contains("recvWindow=5000.125", body);
        }

        var replaceHandler = new RecordingHttpMessageHandler("""
            {
              "cancelResult":"SUCCESS",
              "newOrderResult":"SUCCESS",
              "cancelResponse":{"orderId":1},
              "newOrderResponse":{"orderId":2}
            }
            """);
        using (var client = CreateClient(replaceHandler))
        {
            var replace = await client.Spot.ReplaceOrderAsync(
                "BTCUSDT",
                BinanceOrderSide.Buy,
                BinanceSpotOrderType.Limit,
                BinanceSpotOrderCancelReplaceMode.StopOnFailure,
                cancelOrderId: 1,
                cancelClientOrderId: "original",
                quantity: 0.5m,
                timeInForce: BinanceTimeInForce.GoodTillCanceled,
                strategyType: 1_000_000,
                orderRateLimitExceededMode: BinanceSpotOrderRateLimitExceededMode.CancelOnly,
                pegPriceType: BinanceSpotPegPriceType.Market,
                pegOffsetValue: 1,
                pegOffsetType: BinanceSpotPegOffsetType.PriceLevel);

            Assert.True(replace.Success);
            var body = Uri.UnescapeDataString(replaceHandler.Body!);
            Assert.Contains("cancelOrderId=1", body);
            Assert.Contains("cancelOrigClientOrderId=original", body);
            Assert.Contains("orderRateLimitExceededMode=CANCEL_ONLY", body);
            Assert.Contains("pegPriceType=MARKET_PEG", body);
        }
    }

    [Fact]
    public async Task AmendAndSor_UseCurrentRoutesParametersAndWeights()
    {
        var amendLimiter = new RecordingRateLimiter();
        var amendHandler = new RecordingHttpMessageHandler("""
            {
              "transactTime":1741669661670,
              "executionId":22,
              "amendedOrder":{"symbol":"BTCUSDT","orderId":9,"qty":"4.5","cumulativeQuoteQty":"10"}
            }
            """);
        using (var client = CreateClient(amendHandler, amendLimiter))
        {
            var amend = await client.Spot.AmendOrderAsync(
                "BTCUSDT",
                4.5m,
                orderId: 9,
                originalClientOrderId: "original",
                newClientOrderId: "amended",
                receiveWindow: 6_000.346m);

            Assert.True(amend.Success);
            Assert.Equal(22, amend.Data.ExecutionId);
            Assert.Equal(4.5m, amend.Data.AmendedOrder.Quantity);
            Assert.Equal(HttpMethod.Put, amendHandler.Method);
            Assert.Equal("/api/v3/order/amend/keepPriority", amendHandler.RequestUri!.AbsolutePath);
            Assert.Contains("newQty=4.5", Uri.UnescapeDataString(amendHandler.Body!));
            Assert.Contains(amendLimiter.Requests, item => item.Endpoint == "/api/v3/order/amend/keepPriority" && item.Weight == 4);
        }

        var sorHandler = new RecordingHttpMessageHandler("""
            {
              "symbol":"BTCUSDT","orderId":10,"usedSor":true,"workingFloor":"SOR",
              "fills":[{"matchType":"ONE_PARTY_TRADE_REPORT","price":"10","qty":"2","commission":"0.1","commissionAsset":"BTC","tradeId":3,"allocId":4}]
            }
            """);
        using (var client = CreateClient(sorHandler))
        {
            var sor = await client.Spot.PlaceSorOrderAsync(
                "BTCUSDT",
                BinanceOrderSide.Buy,
                BinanceSpotOrderType.Market,
                2m,
                strategyType: 1_000_000,
                receiveWindow: 5_000.25m);

            Assert.True(sor.Success);
            Assert.Equal("/api/v3/sor/order", sorHandler.RequestUri!.AbsolutePath);
            Assert.True(sor.Data.UsedSmartOrderRouting);
            Assert.Equal(4, Assert.Single(sor.Data.Fills).AllocationId);
        }
    }

    [Fact]
    public async Task TestOrders_UseCommissionParameterAndConditionalWeight()
    {
        var standardLimiter = new RecordingRateLimiter();
        var standardHandler = new RecordingHttpMessageHandler("""{"specialCommissionForOrder":{"maker":"0.05","taker":"0.06"}}""");
        using (var client = CreateClient(standardHandler, standardLimiter))
        {
            var result = await client.Spot.PlaceTestOrderAsync(
                "BTCUSDT",
                BinanceOrderSide.Buy,
                BinanceSpotOrderType.Market,
                quantity: 1,
                computeFeeRates: true);

            Assert.True(result.Success);
            Assert.Equal(0.05m, result.Data.SpecialFeeForOrder!.Maker);
            Assert.Contains("computeCommissionRates=true", Uri.UnescapeDataString(standardHandler.Body!));
            Assert.Contains(standardLimiter.Requests, item => item.Endpoint == "/api/v3/order/test" && item.Weight == 20);
        }

        var sorLimiter = new RecordingRateLimiter();
        var sorHandler = new RecordingHttpMessageHandler("""{"standardCommissionForOrder":{"maker":"0.01","taker":"0.02"}}""");
        using (var client = CreateClient(sorHandler, sorLimiter))
        {
            var result = await client.Spot.PlaceSorTestOrderAsync(
                "BTCUSDT",
                BinanceOrderSide.Buy,
                BinanceSpotOrderType.Market,
                1,
                computeFeeRates: true);

            Assert.True(result.Success);
            Assert.Equal("/api/v3/sor/order/test", sorHandler.RequestUri!.AbsolutePath);
            Assert.Contains("computeCommissionRates=true", Uri.UnescapeDataString(sorHandler.Body!));
            Assert.Contains(sorLimiter.Requests, item => item.Endpoint == "/api/v3/sor/order/test" && item.Weight == 20);
        }
    }

    [Fact]
    public void TradeModelsAndValidation_MapCurrentContracts()
    {
        var fees = JsonConvert.DeserializeObject<BinanceSpotOrderTest>("""
            {
              "standardCommissionForOrder":{"maker":"0.1","taker":"0.2"},
              "specialCommissionForOrder":{"maker":"0.3","taker":"0.4"},
              "taxCommissionForOrder":{"maker":"0.5","taker":"0.6"}
            }
            """)!;
        Assert.Equal(0.3m, fees.SpecialFeeForOrder!.Maker);

        BinanceSpotTradeValidation.Peg(BinanceSpotOrderType.Limit, BinanceSpotPegPriceType.Primary, 100, BinanceSpotPegOffsetType.PriceLevel);
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceSpotTradeValidation.Peg(BinanceSpotOrderType.Limit, BinanceSpotPegPriceType.Primary, 101, BinanceSpotPegOffsetType.PriceLevel));
        Assert.Throws<ArgumentException>(() => BinanceSpotTradeValidation.Peg(BinanceSpotOrderType.Limit, BinanceSpotPegPriceType.Primary, 1, null));
        Assert.Throws<ArgumentException>(() => BinanceSpotTradeValidation.Peg(BinanceSpotOrderType.Market, BinanceSpotPegPriceType.Primary, null, null));
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceSpotTradeValidation.StrategyType(999_999));
        Assert.Throws<ArgumentException>(() => BinanceSpotTradeValidation.OrderIdentifiers(null, null));
        Assert.Throws<ArgumentException>(() => BinanceSpotTradeValidation.SmartOrderRouting(BinanceSpotOrderType.StopLoss, 1, null));

        var socketCancel = BinanceSpotSocketClient.CreateCancelOrderParameters(
            "BTCUSDT",
            1,
            "original",
            "replacement",
            BinanceSpotOrderCancelRestriction.OnlyPartiallyFilled,
            5_000.125m);
        Assert.Equal("ONLY_PARTIALLY_FILLED", socketCancel["cancelRestrictions"]);
        Assert.Equal(5_000.125m, socketCancel["recvWindow"]);

        var root = new BinanceSocketApiClient();
        var socketClient = Assert.IsType<BinanceSpotSocketClient>(root.Spot);
        var sorResponse = socketClient.Deserializer<BinanceResultWithRateLimits<List<BinanceSpotOrder>>>(
            Newtonsoft.Json.Linq.JToken.Parse("""
                {
                  "id":"request-id",
                  "status":200,
                  "result":[{"symbol":"BTCUSDT","orderId":2,"usedSor":true,"workingFloor":"SOR"}],
                  "rateLimits":[]
                }
                """));
        Assert.True(sorResponse.Success);
        Assert.True(Assert.Single(sorResponse.Data.Result).UsedSmartOrderRouting);
    }

    private static BinanceRestApiClient CreateClient(RecordingHttpMessageHandler handler, IRateLimiter? limiter = null)
    {
        var options = new BinanceRestApiClientOptions(new ApiCredentials("api-key", "secret"))
        {
            HttpClient = new HttpClient(handler),
            AutoTimestamp = false,
            RateLimiterEnabled = limiter != null
        };
#pragma warning disable CS0612
        options.RateLimiters = limiter == null ? [] : [limiter];
#pragma warning restore CS0612
        return new BinanceRestApiClient(options);
    }

    private sealed class RecordingRateLimiter : IRateLimiter
    {
        public List<(string Endpoint, int Weight)> Requests { get; } = [];

        public Task<CallResult<int>> LimitRequestAsync(ILogger logger, string endpoint, HttpMethod method, bool signed, SensitiveString? apiKey, RateLimitingBehavior limitBehaviour, int requestWeight, CancellationToken ct)
        {
            Requests.Add((endpoint, requestWeight));
            return Task.FromResult(new CallResult<int>(0));
        }
    }
}
