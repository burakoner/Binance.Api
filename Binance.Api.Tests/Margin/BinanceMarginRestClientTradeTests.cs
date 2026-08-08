using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Margin;
using Binance.Api.Shared;
using Binance.Api.Spot;
using Microsoft.Extensions.Logging;

namespace Binance.Api.Tests.Margin;

public class BinanceMarginRestClientTradeTests
{
    [Fact]
    public async Task CancelMarginOrderAsync_SendsDocumentedDeleteRequest()
    {
        var handler = new RecordingHttpMessageHandler(
            """
            {
              "symbol": "BTCUSDT",
              "orderId": 28,
              "origClientOrderId": "original-order",
              "clientOrderId": "cancel-order",
              "status": "CANCELED",
              "timeInForce": "GTC",
              "type": "LIMIT",
              "side": "SELL",
              "isIsolated": false
            }
            """);
        using var httpClient = new HttpClient(handler);
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });

        var result = await client.Margin.CancelMarginOrderAsync(
            "BTCUSDT",
            orderId: 28,
            newClientOrderId: "cancel-order",
            isIsolated: false,
            receiveWindow: 5_000);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Delete, handler.Method);
        Assert.Equal("/sapi/v1/margin/order", handler.RequestUri!.AbsolutePath);
        Assert.Null(handler.Body);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("signature=", query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("orderId=28", query);
        Assert.Contains("newClientOrderId=cancel-order", query);
        Assert.Contains("isIsolated=FALSE", query);
        Assert.Contains("recvWindow=5000", query);
        Assert.Contains("timestamp=", query);
    }

    [Fact]
    public async Task CancelAllAndOco_SendDocumentedDeleteQueries()
    {
        var cancelAllHandler = new RecordingHttpMessageHandler("""
            [
              {"symbol":"BTCUSDT","orderId":11,"status":"CANCELED","type":"LIMIT","side":"BUY","isIsolated":true},
              {
                "symbol":"BTCUSDT","orderListId":12,"contingencyType":"OCO","listStatusType":"ALL_DONE",
                "listOrderStatus":"ALL_DONE","listClientOrderId":"list","transactionTime":1710000000000,
                "orders":[{"symbol":"BTCUSDT","orderId":13,"clientOrderId":"leg"}],
                "orderReports":[{"symbol":"BTCUSDT","orderId":13,"status":"CANCELED","type":"STOP_LOSS","side":"BUY"}]
              }
            ]
            """);
        using (var client = CreateClient(cancelAllHandler))
        {
            var result = await client.Margin.CancelAllMarginOrdersAsync("BTCUSDT", true, 5_000);

            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal("OCO", result.Data[1].ContingencyType);
            Assert.Equal(BinanceListStatusType.Done, result.Data[1].ListStatusType);
            Assert.Equal(13, Assert.Single(result.Data[1].Orders).OrderId);
            Assert.Equal(BinanceOrderStatus.Canceled, Assert.Single(result.Data[1].OrderReports).Status);
            Assert.Equal(HttpMethod.Delete, cancelAllHandler.Method);
            Assert.Equal("/sapi/v1/margin/openOrders", cancelAllHandler.RequestUri!.AbsolutePath);
            Assert.Null(cancelAllHandler.Body);
            var query = Uri.UnescapeDataString(cancelAllHandler.RequestUri.Query);
            Assert.Contains("symbol=BTCUSDT", query);
            Assert.Contains("isIsolated=TRUE", query);
            Assert.Contains("recvWindow=5000", query);
        }

        var cancelOcoHandler = new RecordingHttpMessageHandler("""{"orderListId":7,"orders":[],"orderReports":[]}""");
        using (var client = CreateClient(cancelOcoHandler))
        {
            var result = await client.Margin.CancelMarginOcoOrderAsync(
                "BTCUSDT",
                isIsolated: false,
                orderListId: 7,
                newClientOrderId: "cancel-list");

            Assert.True(result.Success);
            Assert.Equal(HttpMethod.Delete, cancelOcoHandler.Method);
            Assert.Equal("/sapi/v1/margin/orderList", cancelOcoHandler.RequestUri!.AbsolutePath);
            Assert.Null(cancelOcoHandler.Body);
            var query = Uri.UnescapeDataString(cancelOcoHandler.RequestUri.Query);
            Assert.Contains("orderListId=7", query);
            Assert.Contains("newClientOrderId=cancel-list", query);
            Assert.Contains("isIsolated=FALSE", query);
        }
    }

    [Fact]
    public async Task PlaceMarginOrder_SendsCurrentFieldsAndDynamicWeight()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""{"orderId":1,"isIsolated":true,"marginBuyBorrowAmount":"2.5","marginBuyBorrowAsset":"USDT"}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Margin.PlaceMarginOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Buy,
            BinanceSpotOrderType.StopLoss,
            quantity: 0.5m,
            stopPrice: 50_000m,
            sideEffectType: BinanceMarginSideEffectType.AutoBorrowRepay,
            isIsolated: true,
            selfTradePreventionMode: BinanceSelfTradePreventionMode.ExpireBoth,
            trailingDelta: 100,
            autoRepayAtCancel: false,
            receiveWindow: 5_000);

        Assert.True(result.Success);
        Assert.True(result.Data.IsIsolated);
        Assert.Equal(2.5m, result.Data.MarginBuyBorrowQuantity);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("isIsolated=TRUE", body);
        Assert.Contains("sideEffectType=AUTO_BORROW_REPAY", body);
        Assert.Contains("selfTradePreventionMode=EXPIRE_BOTH", body);
        Assert.Contains("trailingDelta=100", body);
        Assert.Contains("autoRepayAtCancel=false", body, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/order" && item.Weight == 1_500);
    }

    [Fact]
    public async Task PlaceMarginOco_UsesDynamicWeightAndCurrentBooleanEncoding()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""{"orderListId":2,"isIsolated":false,"orders":[],"orderReports":[]}""");
        using var client = CreateClient(handler, limiter);

        var result = await client.Margin.PlaceMarginOCOOrderAsync(
            "BTCUSDT",
            BinanceOrderSide.Sell,
            price: 60_000m,
            stopPrice: 50_000m,
            quantity: 0.1m,
            stopLimitPrice: 49_900m,
            stopLimitTimeInForce: BinanceTimeInForce.GoodTillCanceled,
            sideEffectType: BinanceMarginSideEffectType.MarginBuy,
            isIsolated: false);

        Assert.True(result.Success);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("isIsolated=FALSE", body);
        Assert.Contains("sideEffectType=MARGIN_BUY", body);
        Assert.Contains("stopLimitPrice=49900", body);
        Assert.Contains("stopLimitTimeInForce=GTC", body);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/order/oco" && item.Weight == 1_500);
    }

    [Fact]
    public async Task PlaceMarginOto_SendsCompleteCurrentContract()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""
            {
              "orderListId":3,"contingencyType":"OTO","isIsolated":true,
              "orders":[{"symbol":"BTCUSDT","orderId":31,"clientOrderId":"working"}],
              "orderReports":[{"symbol":"BTCUSDT","orderId":31,"type":"LIMIT","side":"SELL"}]
            }
            """);
        using var client = CreateClient(handler, limiter);
        var request = new BinanceMarginOtoOrderListRequest(
            "BTCUSDT",
            BinanceSpotOrderType.Limit,
            BinanceOrderSide.Sell,
            60_000m,
            0.2m,
            0.05m,
            BinanceSpotOrderType.StopLoss,
            BinanceOrderSide.Buy,
            0.2m)
        {
            IsIsolated = true,
            SideEffectType = BinanceMarginSideEffectType.MarginBuy,
            AutoRepayAtCancel = false,
            WorkingTimeInForce = BinanceTimeInForce.GoodTillCanceled,
            WorkingClientOrderId = "working",
            PendingClientOrderId = "pending",
            PendingPrice = 55_000m,
            PendingStopPrice = 54_000m,
            PendingTrailingDelta = 100m,
            OrderResponseType = BinanceOrderResponseType.Full,
            SelfTradePreventionMode = BinanceSelfTradePreventionMode.None
        };

        var result = await client.Margin.PlaceMarginOtoOrderAsync(request);

        Assert.True(result.Success);
        Assert.Equal(3, result.Data.Id);
        Assert.True(result.Data.IsIsolated);
        Assert.Equal("/sapi/v1/margin/order/oto", handler.RequestUri!.AbsolutePath);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("workingIcebergQty=0.05", body);
        Assert.Contains("pendingType=STOP_LOSS", body);
        Assert.Contains("pendingPrice=55000", body);
        Assert.Contains("pendingTrailingDelta=100", body);
        Assert.DoesNotContain("recvWindow", body);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/order/oto" && item.Weight == 1_500);
    }

    [Fact]
    public async Task PlaceMarginOtoco_SendsOptionalBelowLegAndBaseWeight()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""{"orderListId":4,"contingencyType":"OTOCO","orders":[],"orderReports":[]}""");
        using var client = CreateClient(handler, limiter);
        var request = new BinanceMarginOtocoOrderListRequest(
            "BTCUSDT",
            BinanceSpotOrderType.LimitMaker,
            BinanceOrderSide.Sell,
            60_000m,
            0.2m,
            BinanceOrderSide.Buy,
            0.2m,
            BinanceSpotOrderType.StopLoss)
        {
            SideEffectType = BinanceMarginSideEffectType.NoSideEffect,
            PendingAbovePrice = 55_000m,
            PendingAboveTrailingDelta = 100m,
            PendingBelowType = BinanceSpotOrderType.LimitMaker,
            PendingBelowPrice = 50_000m,
            PendingBelowClientOrderId = "below"
        };

        var result = await client.Margin.PlaceMarginOtocoOrderAsync(request);

        Assert.True(result.Success);
        Assert.Equal("/sapi/v1/margin/order/otoco", handler.RequestUri!.AbsolutePath);
        var body = Uri.UnescapeDataString(handler.Body!);
        Assert.Contains("pendingAboveType=STOP_LOSS", body);
        Assert.Contains("pendingAbovePrice=55000", body);
        Assert.Contains("pendingAboveTrailingDelta=100", body);
        Assert.Contains("pendingBelowType=LIMIT_MAKER", body);
        Assert.Contains("pendingBelowPrice=50000", body);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/order/otoco" && item.Weight == 6);
    }

    [Fact]
    public async Task OrderLists_RejectTrailingDeltaWithoutTheNewlyRequiredPrice()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));
        var oto = new BinanceMarginOtoOrderListRequest(
            "BTCUSDT",
            BinanceSpotOrderType.Limit,
            BinanceOrderSide.Sell,
            60_000m,
            0.2m,
            0.05m,
            BinanceSpotOrderType.StopLoss,
            BinanceOrderSide.Buy,
            0.2m)
        {
            WorkingTimeInForce = BinanceTimeInForce.GoodTillCanceled,
            PendingTrailingDelta = 100m
        };
        var otoco = new BinanceMarginOtocoOrderListRequest(
            "BTCUSDT",
            BinanceSpotOrderType.LimitMaker,
            BinanceOrderSide.Sell,
            60_000m,
            0.2m,
            BinanceOrderSide.Buy,
            0.2m,
            BinanceSpotOrderType.StopLoss)
        {
            PendingAboveTrailingDelta = 100m
        };

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.PlaceMarginOtoOrderAsync(oto));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.PlaceMarginOtocoOrderAsync(otoco));
    }

    [Fact]
    public async Task OrderLists_RejectIcebergQuantitiesWithoutGtc()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("{}"));
        var oto = new BinanceMarginOtoOrderListRequest(
            "BTCUSDT",
            BinanceSpotOrderType.Limit,
            BinanceOrderSide.Sell,
            60_000m,
            0.2m,
            0.05m,
            BinanceSpotOrderType.Limit,
            BinanceOrderSide.Buy,
            0.2m)
        {
            WorkingTimeInForce = BinanceTimeInForce.ImmediateOrCancel,
            PendingPrice = 55_000m,
            PendingTimeInForce = BinanceTimeInForce.GoodTillCanceled
        };
        var otoco = new BinanceMarginOtocoOrderListRequest(
            "BTCUSDT",
            BinanceSpotOrderType.LimitMaker,
            BinanceOrderSide.Sell,
            60_000m,
            0.2m,
            BinanceOrderSide.Buy,
            0.2m,
            BinanceSpotOrderType.StopLoss)
        {
            WorkingIcebergQuantity = 0.05m,
            PendingAboveStopPrice = 55_000m
        };
        var otoPending = new BinanceMarginOtoOrderListRequest(
            "BTCUSDT",
            BinanceSpotOrderType.Limit,
            BinanceOrderSide.Sell,
            60_000m,
            0.2m,
            0.05m,
            BinanceSpotOrderType.Limit,
            BinanceOrderSide.Buy,
            0.2m)
        {
            WorkingTimeInForce = BinanceTimeInForce.GoodTillCanceled,
            PendingPrice = 55_000m,
            PendingIcebergQuantity = 0.05m,
            PendingTimeInForce = BinanceTimeInForce.ImmediateOrCancel
        };
        var otocoPending = new BinanceMarginOtocoOrderListRequest(
            "BTCUSDT",
            BinanceSpotOrderType.LimitMaker,
            BinanceOrderSide.Sell,
            60_000m,
            0.2m,
            BinanceOrderSide.Buy,
            0.2m,
            BinanceSpotOrderType.StopLossLimit)
        {
            PendingAbovePrice = 55_000m,
            PendingAboveStopPrice = 54_000m,
            PendingAboveIcebergQuantity = 0.05m,
            PendingAboveTimeInForce = BinanceTimeInForce.FillOrKill
        };

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.PlaceMarginOtoOrderAsync(oto));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.PlaceMarginOtocoOrderAsync(otoco));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.PlaceMarginOtoOrderAsync(otoPending));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.PlaceMarginOtocoOrderAsync(otocoPending));
    }

    [Fact]
    public async Task ForcedLiquidationHistory_SendsCurrentContractAndReadsInt64Total()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""
            {
              "rows":[{
                "avgPrice":"60000","executedQty":"0.2","orderId":42,"price":"60010","qty":"0.2",
                "side":"SELL","symbol":"BTCUSDT","timeInForce":"GTC","updatedTime":1710000000000,"isIsolated":true
              }],
              "total":3000000000
            }
            """);
        using var client = CreateClient(handler, limiter);
        var start = DateTime.UnixEpoch.AddMilliseconds(1_710_000_000_000);
        var end = start.AddHours(1);

        var result = await client.Margin.GetMarginForcedLiquidationHistoryAsync(
            start,
            end,
            "BTCUSDT",
            current: 3_000_000_000,
            size: 100,
            receiveWindow: 5_000);

        Assert.True(result.Success);
        Assert.Equal(3_000_000_000, result.Data.Total);
        Assert.True(Assert.Single(result.Data.Rows).IsIsolated);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("isolatedSymbol=BTCUSDT", query);
        Assert.Contains("current=3000000000", query);
        Assert.Contains("size=100", query);
        Assert.DoesNotContain("page=", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/forceLiquidationRec" && item.Weight == 1);
    }

    [Fact]
    public async Task SmallLiabilityHistory_AlwaysSendsRequiredPagination()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""
            {"rows":[{"asset":"ETH","amount":"0.1","targetAsset":"BNB","targetAmount":"0.01","bizType":"EXCHANGE","timestamp":1710000000000}],"total":3000000001}
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.Margin.GetSmallLiabilityExchangeHistoryAsync();

        Assert.True(result.Success);
        Assert.Equal(3_000_000_001, result.Data.Total);
        Assert.Equal("BNB", Assert.Single(result.Data.Rows).TargetAsset);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("current=1", query);
        Assert.Contains("size=10", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/exchange-small-liability-history" && item.Weight == 100);
    }

    [Fact]
    public async Task MarginOrderCountUsage_SendsScopeAndReadsInt64Counters()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("""
            [{"rateLimitType":"ORDERS","interval":"SECOND","intervalNum":3000000000,"limit":3000000001,"count":3000000002}]
            """);
        using var client = CreateClient(handler, limiter);

        var result = await client.Margin.GetMarginOrderCountUsageAsync(true, "BTCUSDT", 5_000);

        Assert.True(result.Success);
        var usage = Assert.Single(result.Data);
        Assert.Equal(BinanceRateLimitType.Orders, usage.Type);
        Assert.Equal(BinanceRateLimitInterval.Second, usage.Interval);
        Assert.Equal(3_000_000_002, usage.Count);
        var query = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("isIsolated=TRUE", query);
        Assert.Contains("symbol=BTCUSDT", query);
        Assert.Contains("recvWindow=5000", query);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/rateLimit/order" && item.Weight == 20);
    }

    [Fact]
    public async Task ReadOnlyOrderQueries_SendDocumentedBooleanAndUnchangedClientId()
    {
        var orderHandler = new RecordingHttpMessageHandler("""{"symbol":"BTCUSDT","orderId":7,"isIsolated":true}""");
        using (var client = CreateClient(orderHandler))
        {
            var result = await client.Margin.GetMarginOrderAsync("BTCUSDT", origClientOrderId: "caller-id", isIsolated: true);

            Assert.True(result.Success);
            var query = Uri.UnescapeDataString(orderHandler.RequestUri!.Query);
            Assert.Contains("origClientOrderId=caller-id", query);
            Assert.DoesNotContain("x-", query, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("isIsolated=TRUE", query);
        }

        var ocoHandler = new RecordingHttpMessageHandler("""{"orderListId":8,"isIsolated":true,"orders":[]}""");
        using (var client = CreateClient(ocoHandler))
        {
            var result = await client.Margin.GetMarginOcoOrderAsync("BTCUSDT", true, origClientOrderId: "list-id");

            Assert.True(result.Success);
            var query = Uri.UnescapeDataString(ocoHandler.RequestUri!.Query);
            Assert.Contains("origClientOrderId=list-id", query);
            Assert.Contains("isIsolated=TRUE", query);
        }
    }

    [Fact]
    public async Task ReadOnlyMarginTradeCollections_SendCurrentPathsScopesAndWeights()
    {
        var limiter = new RecordingRateLimiter();
        var start = DateTime.UnixEpoch.AddMilliseconds(1_710_000_000_000);

        var assetsHandler = new RecordingHttpMessageHandler("[]");
        using (var client = CreateClient(assetsHandler, limiter))
        {
            Assert.True((await client.Margin.GetSmallLiabilityExchangeAssetsAsync(5_000)).Success);
            Assert.Equal("/sapi/v1/margin/exchange-small-liability", assetsHandler.RequestUri!.AbsolutePath);
            Assert.Contains("recvWindow=5000", Uri.UnescapeDataString(assetsHandler.RequestUri.Query));
        }

        var allOcoHandler = new RecordingHttpMessageHandler("[]");
        using (var client = CreateClient(allOcoHandler, limiter))
        {
            Assert.True((await client.Margin.GetMarginOcoOrdersAsync("BTCUSDT", true, limit: 1_000)).Success);
            var query = Uri.UnescapeDataString(allOcoHandler.RequestUri!.Query);
            Assert.Equal("/sapi/v1/margin/allOrderList", allOcoHandler.RequestUri.AbsolutePath);
            Assert.Contains("isIsolated=TRUE", query);
            Assert.Contains("limit=1000", query);
        }

        var allOrdersHandler = new RecordingHttpMessageHandler("[]");
        using (var client = CreateClient(allOrdersHandler, limiter))
        {
            Assert.True((await client.Margin.GetMarginOrdersAsync("BTCUSDT", startTime: start, endTime: start.AddHours(23), limit: 500, isIsolated: false)).Success);
            var query = Uri.UnescapeDataString(allOrdersHandler.RequestUri!.Query);
            Assert.Equal("/sapi/v1/margin/allOrders", allOrdersHandler.RequestUri.AbsolutePath);
            Assert.Contains("isIsolated=FALSE", query);
            Assert.Contains("limit=500", query);
        }

        var openOcoHandler = new RecordingHttpMessageHandler("[]");
        using (var client = CreateClient(openOcoHandler, limiter))
        {
            Assert.True((await client.Margin.GetMarginOpenOcoOrdersAsync("BTCUSDT", true)).Success);
            Assert.Equal("/sapi/v1/margin/openOrderList", openOcoHandler.RequestUri!.AbsolutePath);
            Assert.Contains("isIsolated=TRUE", Uri.UnescapeDataString(openOcoHandler.RequestUri.Query));
        }

        var openOrdersHandler = new RecordingHttpMessageHandler("[]");
        using (var client = CreateClient(openOrdersHandler, limiter))
        {
            Assert.True((await client.Margin.GetOpenMarginOrdersAsync("BTCUSDT", false)).Success);
            Assert.Equal("/sapi/v1/margin/openOrders", openOrdersHandler.RequestUri!.AbsolutePath);
            Assert.Contains("isIsolated=FALSE", Uri.UnescapeDataString(openOrdersHandler.RequestUri.Query));
        }

        var tradesHandler = new RecordingHttpMessageHandler("[]");
        using (var client = CreateClient(tradesHandler, limiter))
        {
            Assert.True((await client.Margin.GetMarginUserTradesAsync("BTCUSDT", startTime: start, endTime: start.AddHours(23), limit: 1_000, isIsolated: true)).Success);
            var query = Uri.UnescapeDataString(tradesHandler.RequestUri!.Query);
            Assert.Equal("/sapi/v1/margin/myTrades", tradesHandler.RequestUri.AbsolutePath);
            Assert.Contains("isIsolated=TRUE", query);
            Assert.Contains("limit=1000", query);
        }

        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/exchange-small-liability" && item.Weight == 100);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/allOrderList" && item.Weight == 200);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/allOrders" && item.Weight == 200);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/openOrderList" && item.Weight == 10);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/openOrders" && item.Weight == 10);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/sapi/v1/margin/myTrades" && item.Weight == 10);
    }

    [Fact]
    public async Task ReadOnlyMarginTradeQueries_RejectUndocumentedOrInvalidShapes()
    {
        using var client = CreateClient(new RecordingHttpMessageHandler("[]"));
        var start = DateTime.UtcNow.AddDays(-2);

        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginOrdersAsync("BTCUSDT", startTime: start, endTime: start.AddHours(24)));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginUserTradesAsync("BTCUSDT", startTime: start, endTime: start.AddHours(25)));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginOcoOrdersAsync(symbol: "BTCUSDT", isIsolated: false));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginOpenOcoOrdersAsync(isIsolated: true));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetOpenMarginOrdersAsync(isIsolated: true));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Margin.GetMarginOrderCountUsageAsync(isIsolated: true));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Margin.GetSmallLiabilityExchangeAssetsAsync(60_001));
    }

    private static BinanceRestApiClient CreateClient(RecordingHttpMessageHandler handler, IRateLimiter? limiter = null)
    {
        var options = new BinanceRestApiClientOptions(new ApiCredentials("api-key", "api-secret"))
        {
            AutoTimestamp = false,
            HttpClient = new HttpClient(handler),
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
            Requests.Add((endpoint, requestWeight));
            return Task.FromResult(new CallResult<int>(0));
        }
    }
}
