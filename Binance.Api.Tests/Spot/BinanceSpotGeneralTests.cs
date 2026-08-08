using ApiSharp.Converters;
using Binance.Api.Shared;
using Binance.Api.Spot;
using Newtonsoft.Json;

namespace Binance.Api.Tests.Spot;

public class BinanceSpotGeneralTests
{
    [Fact]
    public async Task ExecutionRules_UsesCurrentRouteParametersAndResponseSchema()
    {
        const string response = """
            {
              "symbolRules": [{
                "symbol": "BTCUSDT",
                "rules": [{
                  "ruleType": "PRICE_RANGE",
                  "bidLimitMultUp": "1.0001",
                  "bidLimitMultDown": "0.9999",
                  "askLimitMultUp": "1.0002",
                  "askLimitMultDown": "0.9998"
                }]
              }]
            }
            """;
        var handler = new RecordingHttpMessageHandler(response);
        using var httpClient = new HttpClient(handler);
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions
        {
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });

        var result = await client.Spot.GetExecutionRulesAsync(["BTCUSDT", "ETHUSDT"]);

        Assert.True(result.Success);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal("/api/v3/executionRules", handler.RequestUri!.AbsolutePath);
        Assert.Equal("?symbols=[\"BTCUSDT\",\"ETHUSDT\"]", Uri.UnescapeDataString(handler.RequestUri.Query));
        var symbolRules = Assert.Single(result.Data.SymbolRules);
        Assert.Equal("BTCUSDT", symbolRules.Symbol);
        var rule = Assert.Single(symbolRules.Rules);
        Assert.Equal("PRICE_RANGE", rule.RuleType);
        Assert.Equal(1.0001m, rule.BidLimitMultUp);
        Assert.Equal(0.9998m, rule.AskLimitMultDown);
    }

    [Fact]
    public async Task ExecutionRules_StatusOverload_UsesOnlyCurrentSpotStatus()
    {
        var handler = new RecordingHttpMessageHandler("""{"symbolRules":[]}""");
        using var httpClient = new HttpClient(handler);
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions
        {
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });

        var result = await client.Spot.GetExecutionRulesAsync(BinanceSpotSymbolStatusFilter.Halt);

        Assert.True(result.Success);
        Assert.Equal("?symbolStatus=HALT", handler.RequestUri!.Query);
        Assert.Equal(
            ["TRADING", "HALT", "BREAK"],
            Enum.GetValues<BinanceSpotSymbolStatusFilter>().Select(value => MapConverter.GetString(value)!).ToArray());
    }

    [Fact]
    public async Task ExchangeInfo_UsesMappedPermissionsAndRejectsForbiddenCombinations()
    {
        var handler = new RecordingHttpMessageHandler("""{"symbols":[]}""");
        using var httpClient = new HttpClient(handler);
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions
        {
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });

        var result = await client.Spot.GetExchangeInfoAsync(
            [],
            permissions: [BinancePermissionType.TradeGroup004]);

        Assert.True(result.Success);
        Assert.Equal("?permissions=TRD_GRP_004", handler.RequestUri!.Query);
        await Assert.ThrowsAsync<ArgumentException>(() => client.Spot.GetExchangeInfoAsync(
            ["BTCUSDT"],
            status: BinanceSpotSymbolStatusFilter.Trading));
        await Assert.ThrowsAsync<ArgumentException>(() => client.Spot.GetExchangeInfoAsync(
            ["BTCUSDT"],
            permissions: [BinancePermissionType.Spot]));
    }

    [Fact]
    public async Task ExchangeInfo_MapsCurrentPrecisionFeaturesEnumsAndFilters()
    {
        const string response = """
            {
              "timezone":"UTC",
              "serverTime":1770000000000,
              "rateLimits":[],
              "exchangeFilters":[],
              "symbols":[{
                "symbol":"BTCUSDT",
                "status":"CANCEL_ONLY",
                "baseAsset":"BTC",
                "baseAssetPrecision":8,
                "quoteAsset":"USDT",
                "quotePrecision":7,
                "quoteAssetPrecision":10,
                "baseCommissionPrecision":8,
                "quoteCommissionPrecision":9,
                "orderTypes":["LIMIT"],
                "icebergAllowed":true,
                "ocoAllowed":true,
                "otoAllowed":true,
                "opoAllowed":true,
                "quoteOrderQtyMarketAllowed":true,
                "allowTrailingStop":true,
                "cancelReplaceAllowed":true,
                "amendAllowed":true,
                "pegInstructionsAllowed":true,
                "isSpotTradingAllowed":true,
                "isMarginTradingAllowed":true,
                "permissions":[],
                "permissionSets":[["SPOT"]],
                "defaultSelfTradePreventionMode":"DECREMENT",
                "allowedSelfTradePreventionModes":["DECREMENT","TRANSFER"],
                "filters":[
                  {"filterType":"MAX_NUM_ICEBERG_ORDERS","maxNumIcebergOrders":5},
                  {"filterType":"MAX_NUM_ORDER_AMENDS","maxNumOrderAmends":10},
                  {"filterType":"MAX_NUM_ORDER_LISTS","maxNumOrderLists":20}
                ]
              }],
              "sors":[{"baseAsset":"BTC","symbols":["BTCUSDT","BTCUSDC"]}]
            }
            """;
        var handler = new RecordingHttpMessageHandler(response);
        using var httpClient = new HttpClient(handler);
        using var client = new BinanceRestApiClient(new BinanceRestApiClientOptions
        {
            HttpClient = httpClient,
            RateLimiterEnabled = false
        });

        var result = await client.Spot.GetExchangeInfoAsync();

        Assert.True(result.Success);
        var symbol = Assert.Single(result.Data.Symbols);
        Assert.Equal(BinanceSpotSymbolStatus.CancelOnly, symbol.Status);
        Assert.Equal(7, symbol.QuotePrecision);
        Assert.Equal(10, symbol.QuoteAssetPrecision);
        Assert.True(symbol.OPOAllowed);
        Assert.True(symbol.AmendAllowed);
        Assert.True(symbol.PegInstructionsAllowed);
        Assert.Equal(BinanceSelfTradePreventionMode.Decrement, symbol.DefaultSelfTradePreventionMode);
        Assert.Equal(
            [BinanceSelfTradePreventionMode.Decrement, BinanceSelfTradePreventionMode.Transfer],
            symbol.AllowedSelfTradePreventionModes);
        Assert.Equal(5, symbol.MaxNumberOfIcebergOrdersFilter!.MaxNumIcebergOrders);
        Assert.Equal(10, symbol.MaxOrderAmendsFilter!.MaxNumOrderAmends);
        Assert.Equal(20, symbol.MaxOrderListsFilter!.MaxNumOrderLists);
        var sor = Assert.Single(result.Data.Sors);
        Assert.Equal("BTC", sor.BaseAsset);
        Assert.Equal(["BTCUSDT", "BTCUSDC"], sor.Symbols);
    }

    [Fact]
    public void SymbolFilterSerialization_UsesCurrentPropertyNames()
    {
        var filter = new BinanceSymbolNotionalFilter
        {
            FilterType = BinanceSymbolFilterType.Notional,
            MinNotional = 10,
            MaxNotional = 1000,
            ApplyMinToMarketOrders = true,
            ApplyMaxToMarketOrders = false,
            AveragePriceMinutes = 5
        };

        var json = JsonConvert.SerializeObject(filter);

        Assert.Contains("\"applyMinToMarket\":true", json);
        Assert.Contains("\"applyMaxToMarket\":false", json);
        Assert.DoesNotContain("applyMinToMarketOrders", json);
        Assert.DoesNotContain("applyMaxToMarketOrders", json);
    }
}
