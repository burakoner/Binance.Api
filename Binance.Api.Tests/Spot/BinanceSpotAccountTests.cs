using ApiSharp.Authentication;
using ApiSharp.Models;
using ApiSharp.Security;
using ApiSharp.Throttling;
using Binance.Api.Shared;
using Binance.Api.Spot;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Spot;

public class BinanceSpotAccountTests
{
    [Fact]
    public async Task NewAccountQueries_UseCurrentRoutesAndParameters()
    {
        var handler = new RecordingHttpMessageHandler("""{"orderListId":27,"orders":[]}""");
        using var client = CreateClient(handler);

        var result = await client.Spot.GetOrderListAsync(orderListId: 27, receiveWindow: 6000.346m);

        Assert.True(result.Success);
        Assert.Equal("/api/v3/orderList", handler.RequestUri!.AbsolutePath);
        var query = Uri.UnescapeDataString(handler.RequestUri.Query);
        Assert.Contains("orderListId=27", query);
        Assert.Contains("recvWindow=6000.346", query);

        Assert.Equal("/api/v3/allOrderList", await RequestPathAsync("[]", client => client.Spot.GetOrderListsAsync(limit: 1000)));
        Assert.Equal("/api/v3/openOrderList", await RequestPathAsync("[]", client => client.Spot.GetOpenOrderListsAsync()));
        Assert.Equal("/api/v3/myAllocations", await RequestPathAsync("[]", client => client.Spot.GetAllocationsAsync("BTCUSDT")));
        Assert.Equal("/api/v3/account/commission", await RequestPathAsync("{}", client => client.Spot.GetCommissionRatesAsync("BTCUSDT")));
        Assert.Equal("/api/v3/order/amendments", await RequestPathAsync("[]", client => client.Spot.GetOrderAmendmentsAsync("BTCUSDT", 9)));
        Assert.Equal("/api/v3/myFilters", await RequestPathAsync("{}", client => client.Spot.GetAccountFiltersAsync("BTCUSDT")));
    }

    [Fact]
    public async Task ExistingAccountQueries_UseCurrentWeightsAndPreventedMatchParameter()
    {
        var limiter = new RecordingRateLimiter();
        var handler = new RecordingHttpMessageHandler("[]");
        using var client = CreateClient(handler, limiter);

        Assert.True((await client.Spot.GetRateLimitsAsync()).Success);
        Assert.True((await client.Spot.GetOpenOrdersAsync("BTCUSDT")).Success);
        Assert.True((await client.Spot.GetOpenOrdersAsync()).Success);
        Assert.True((await client.Spot.GetOrdersAsync("BTCUSDT")).Success);
        Assert.True((await client.Spot.GetPreventedTradesAsync("BTCUSDT", orderId: 7, fromPreventedMatchId: 3, limit: 25)).Success);

        Assert.Contains(limiter.Requests, item => item.Endpoint == "/api/v3/rateLimit/order" && item.Weight == 40);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/api/v3/openOrders" && item.Weight == 6);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/api/v3/openOrders" && item.Weight == 80);
        Assert.Contains(limiter.Requests, item => item.Endpoint == "/api/v3/allOrders" && item.Weight == 20);
        var preventedQuery = Uri.UnescapeDataString(handler.RequestUri!.Query);
        Assert.Contains("limit=25", preventedQuery);
        Assert.DoesNotContain("size=", preventedQuery);
    }

    [Fact]
    public void AccountModels_MapCurrentOfficialPayloads()
    {
        var commission = JsonConvert.DeserializeObject<BinanceSpotCommissionRates>("""
            {
              "symbol":"BTCUSDT",
              "standardCommission":{"maker":"0.00000010","taker":"0.00000020","buyer":"0.00000030","seller":"0.00000040"},
              "specialCommission":{"maker":"0.01","taker":"0.02","buyer":"0.03","seller":"0.04"},
              "taxCommission":{"maker":"0.00000112","taker":"0.00000114","buyer":"0.00000118","seller":"0.00000116"},
              "discount":{"enabledForAccount":true,"enabledForSymbol":true,"discountAsset":"BNB","discount":"0.75"}
            }
            """)!;
        Assert.Equal(0.00000010m, commission.StandardCommission.Maker);
        Assert.True(commission.Discount.EnabledForAccount);

        var allocation = JsonConvert.DeserializeObject<BinanceSpotAllocation>("""
            {"symbol":"BTCUSDT","allocationId":4,"allocationType":"SOR","orderId":9,"orderListId":-1,"price":"1","qty":"5","quoteQty":"5","commission":"0.1","commissionAsset":"BTC","time":1687506878118,"isBuyer":true,"isMaker":false,"isAllocator":false}
            """)!;
        Assert.Equal(4, allocation.AllocationId);
        Assert.Equal(5m, allocation.Quantity);

        var amendment = JsonConvert.DeserializeObject<BinanceSpotOrderAmendment>("""
            {"symbol":"BTCUSDT","orderId":9,"executionId":22,"origClientOrderId":"old","newClientOrderId":"new","origQty":"5","newQty":"4","time":1741669661670}
            """)!;
        Assert.Equal(22, amendment.ExecutionId);
        Assert.Equal(4m, amendment.NewQuantity);

        var filters = JsonConvert.DeserializeObject<BinanceSpotAccountFilters>("""
            {
              "exchangeFilters":[{"filterType":"EXCHANGE_MAX_NUM_ORDERS","maxNumOrders":1000}],
              "symbolFilters":[
                {"filterType":"PRICE_FILTER","priceExponent":8,"minPrice":"0.00000001","maxPrice":"100000","tickSize":"0.00000001"},
                {"filterType":"T_PLUS_SELL","endTime":1741669661670}
              ],
              "assetFilters":[{"filterType":"MAX_ASSET","qtyExponent":8,"asset":"JPY","limit":"1000000"}]
            }
            """)!;
        Assert.Equal(1000, Assert.Single(filters.ExchangeFilters).MaxNumOrders);
        Assert.Equal(8, Assert.IsType<BinanceSymbolPriceFilter>(filters.SymbolFilters[0]).PriceExponent);
        Assert.Equal(DateTimeKind.Utc, Assert.IsType<BinanceSymbolTPlusSellFilter>(filters.SymbolFilters[1]).EndTime.Kind);
        var assetFilter = Assert.Single(filters.AssetFilters);
        Assert.Equal(8, assetFilter.QuantityExponent);
        Assert.Equal(1_000_000m, assetFilter.Limit);

        var order = JsonConvert.DeserializeObject<BinanceSpotOrder>("""
            {"symbol":"BTCUSDT","orderId":9,"origClientOrderId":"x-CUSTOM-old","clientOrderId":"new","isWorking":true,"preventedMatchId":2,"preventedQuantity":"1.2","strategyId":7,"strategyType":1000000,"trailingDelta":10,"trailingTime":1741669661670,"usedSor":true,"workingFloor":"SOR","pegPriceType":"PRIMARY_PEG","pegOffsetType":"PRICE_LEVEL","pegOffsetValue":2,"peggedPrice":"50000","expiryReason":"EXECUTION_RULE_PRICE_RANGE_EXCEEDED"}
            """)!;
        Assert.True(order.IsWorking);
        Assert.Equal("x-CUSTOM-old", order.RequestOriginalClientOrderId);
        order.OriginalClientOrderId = "x-QQCDRXG2-old";
        Assert.Equal("old", order.RequestOriginalClientOrderId);
        Assert.True(order.UsedSmartOrderRouting);
        Assert.Equal(BinanceSpotOrderExpiryReason.ExecutionRulePriceRangeExceeded, order.ExpiryReason);
    }

    [Fact]
    public async Task AccountValidation_RejectsUndocumentedCombinationsAndRanges()
    {
        Assert.Equal("5000.123", BinanceSpotAccountValidation.ReceiveWindow(5000.123m));
        Assert.Equal("5000.1", BinanceSpotAccountValidation.ReceiveWindow(5000.1000m));
        Assert.Throws<ArgumentOutOfRangeException>(() => BinanceSpotAccountValidation.ReceiveWindow(60_000.001m));
        Assert.Throws<ArgumentException>(() => BinanceSpotAccountValidation.ReceiveWindow(5_000.0001m));

        using var client = CreateClient(new RecordingHttpMessageHandler("[]"));
        var start = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        await Assert.ThrowsAsync<ArgumentException>(() => client.Spot.GetUserTradesAsync("BTCUSDT", startTime: start, fromId: 1));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Spot.GetOrderListsAsync(fromId: 1, startTime: start));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Spot.GetAllocationsAsync("BTCUSDT", startTime: start, orderId: 1));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Spot.GetPreventedTradesAsync("BTCUSDT"));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Spot.GetOrdersAsync("BTCUSDT", startTime: start, endTime: start.AddHours(25)));
    }

    private static async Task<string> RequestPathAsync<T>(string response, Func<BinanceRestApiClient, Task<RestCallResult<T>>> request) where T : class
    {
        var handler = new RecordingHttpMessageHandler(response);
        using var client = CreateClient(handler);
        Assert.True((await request(client)).Success);
        return handler.RequestUri!.AbsolutePath;
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
