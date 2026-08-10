using Binance.Api.Futures;
using Binance.Api.Options;
using Binance.Api.Shared;

namespace Binance.Api.Tests.Futures;

public class BinanceTradeRuleIsolationTests
{
    private const string FuturesExchangeInfo = """
        {
          "symbols": [
            {
              "symbol": "SYMBOL",
              "orderTypes": ["LIMIT"],
              "filters": [
                { "filterType": "LOT_SIZE", "minQty": "1", "maxQty": "10", "stepSize": "1" }
              ]
            }
          ]
        }
        """;

    private const string OptionsExchangeInfo = """
        {
          "optionSymbols": [
            {
              "symbol": "BTC-260925-50000-C",
              "filters": [
                { "filterType": "LOT_SIZE", "minQty": "1", "maxQty": "10", "stepSize": "1" }
              ]
            }
          ]
        }
        """;

    [Fact]
    public async Task RestUsdRules_UseUsdSettingsOnly()
    {
        var handler = new RecordingHttpMessageHandler(FuturesExchangeInfo.Replace("SYMBOL", "BTCUSDT"));
        using var client = CreateRestClient(handler, options =>
        {
            options.UsdtFuturesOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.ThrowError;
            options.UsdtFuturesOptions.TradeRulesUpdateInterval = TimeSpan.Zero;
            options.CoinFuturesOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.AutoComply;
            options.CoinFuturesOptions.TradeRulesUpdateInterval = TimeSpan.FromDays(1);
            options.SpotOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.AutoComply;
        });

        var result = await ((BinanceFuturesRestClientUsd)client.UsdFutures).CheckTradingRulesAsync(
            "BTCUSDT", BinanceFuturesOrderType.Limit, 1.5m, null, null, null, default);
        await ((BinanceFuturesRestClientUsd)client.UsdFutures).CheckTradingRulesAsync(
            "BTCUSDT", BinanceFuturesOrderType.Limit, 1.5m, null, null, null, default);

        Assert.False(result.Passed);
        Assert.Contains("LotSize filter failed", result.ErrorMessage);
        Assert.Equal(2, handler.RequestCount);
        Assert.Contains("/fapi/v1/exchangeInfo", handler.RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task RestCoinRules_UseCoinSettingsOnly()
    {
        var handler = new RecordingHttpMessageHandler(FuturesExchangeInfo.Replace("SYMBOL", "BTCUSD_PERP"));
        using var client = CreateRestClient(handler, options =>
        {
            options.CoinFuturesOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.ThrowError;
            options.SpotOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.AutoComply;
        });

        var result = await ((BinanceFuturesRestClientCoin)client.CoinFutures).CheckTradingRulesAsync(
            "BTCUSD_PERP", BinanceFuturesOrderType.Limit, 1.5m, null, null, null, default);

        Assert.False(result.Passed);
        Assert.Contains("LotSize filter failed", result.ErrorMessage);
        Assert.Contains("/dapi/v1/exchangeInfo", handler.RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task RestOptionsRules_UseEuropeanOptionsSettingsOnly()
    {
        var handler = new RecordingHttpMessageHandler(OptionsExchangeInfo);
        using var client = CreateRestClient(handler, options =>
        {
            options.EuropeanOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.ThrowError;
            options.EuropeanOptions.TradeRulesUpdateInterval = TimeSpan.Zero;
            options.SpotOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.AutoComply;
            options.SpotOptions.TradeRulesUpdateInterval = TimeSpan.FromDays(1);
            options.CoinFuturesOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.None;
        });

        var result = await ((BinanceOptionsRestClient)client.Options).CheckTradingRulesAsync(
            "BTC-260925-50000-C", BinanceOptionsOrderType.Limit, 1.5m, null, null, null, default);
        await ((BinanceOptionsRestClient)client.Options).CheckTradingRulesAsync(
            "BTC-260925-50000-C", BinanceOptionsOrderType.Limit, 1.5m, null, null, null, default);

        Assert.False(result.Passed);
        Assert.Contains("LotSize filter failed", result.ErrorMessage);
        Assert.Equal(2, handler.RequestCount);
        Assert.Contains("/eapi/v1/exchangeInfo", handler.RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task SocketFuturesRules_UseTheirOwnProductSettingsAndRestClient()
    {
        var usdOptions = new BinanceSocketApiClientOptions();
        usdOptions.UsdtFuturesOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.None;
        usdOptions.CoinFuturesOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.ThrowError;
        var usdClient = new BinanceSocketApiClient(usdOptions);

        var coinOptions = new BinanceSocketApiClientOptions();
        coinOptions.CoinFuturesOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.None;
        coinOptions.UsdtFuturesOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.ThrowError;
        var coinClient = new BinanceSocketApiClient(coinOptions);

        var usdResult = await ((BinanceFuturesSocketClientUsd)usdClient.UsdFutures).CheckTradingRulesAsync(
            "BTCUSDT", BinanceFuturesOrderType.Limit, 1.5m, null, null, null, default);
        var coinResult = await ((BinanceFuturesSocketClientCoin)coinClient.CoinFutures).CheckTradingRulesAsync(
            "BTCUSD_PERP", BinanceFuturesOrderType.Limit, 1.5m, null, null, null, default);

        Assert.True(usdResult.Passed);
        Assert.Equal(1.5m, usdResult.Quantity);
        Assert.True(coinResult.Passed);
        Assert.Equal(1.5m, coinResult.Quantity);
    }

    [Fact]
    public async Task OptionsBatchRules_UseEuropeanOptionsGate()
    {
        var handler = new RecordingHttpMessageHandler(OptionsExchangeInfo);
        using var client = CreateRestClient(handler, options =>
        {
            options.EuropeanOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.ThrowError;
            options.CoinFuturesOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.None;
            options.SpotOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.None;
        });

        var result = await client.Options.PlaceOrdersAsync(
        [
            new BinanceOptionsBatchOrderRequest
            {
                Symbol = "BTC-260925-50000-C",
                Side = BinanceOrderSide.Buy,
                Type = BinanceOptionsOrderType.Limit,
                Quantity = 1.5m
            }
        ]);

        Assert.False(result.Success);
        Assert.Equal("/eapi/v1/exchangeInfo", handler.RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task DisabledProductRules_DoNotInheritAnotherProductsEnabledSettings()
    {
        var usdHandler = new RecordingHttpMessageHandler("{}");
        using var usdClient = CreateRestClient(usdHandler, options =>
        {
            options.UsdtFuturesOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.None;
            options.CoinFuturesOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.ThrowError;
        });

        var usdResult = await ((BinanceFuturesRestClientUsd)usdClient.UsdFutures).CheckTradingRulesAsync(
            "BTCUSDT", BinanceFuturesOrderType.Limit, 1.5m, null, null, null, default);

        var optionsHandler = new RecordingHttpMessageHandler("{}");
        using var optionsClient = CreateRestClient(optionsHandler, options =>
        {
            options.EuropeanOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.None;
            options.SpotOptions.TradeRulesBehavior = BinanceTradeRulesBehavior.ThrowError;
        });

        var optionsResult = await ((BinanceOptionsRestClient)optionsClient.Options).CheckTradingRulesAsync(
            "BTC-260925-50000-C", BinanceOptionsOrderType.Limit, 1.5m, null, null, null, default);

        Assert.True(usdResult.Passed);
        Assert.Null(usdHandler.RequestUri);
        Assert.True(optionsResult.Passed);
        Assert.Null(optionsHandler.RequestUri);
    }

    private static BinanceRestApiClient CreateRestClient(
        RecordingHttpMessageHandler handler,
        Action<BinanceRestApiClientOptions> configure)
    {
        var options = new BinanceRestApiClientOptions("api-key", "api-secret")
        {
            AutoTimestamp = false,
            HttpClient = new HttpClient(handler),
            RateLimiterEnabled = false
        };
        configure(options);
        return new BinanceRestApiClient(options);
    }
}
