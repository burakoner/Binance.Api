namespace Binance.Api.Futures;

internal partial class BinanceRestApiClientFuturesUsd(BinanceRestApiClientFutures parent) : IBinanceRestApiClientFuturesUsd
{
    // Api
    private const string v1 = "1";
    private const string v2 = "2";
    private const string v3 = "3";
    private const string api = "api";
    private const string fapi = "fapi";
    private const string sapi = "sapi";

    // Parent
    private BinanceRestApiClientFutures _ { get; } = parent;

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions Options => _.Options;
    internal DateTime? LastExchangeInfoUpdate { get; private set; }
    internal BinanceFuturesUsdtExchangeInfo? ExchangeInfo { get; private set; }

    // Request
    internal Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => _._.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    // GetUrl
    internal Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.UsdFuturesRestClientAddress.AppendPath(api);
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    // CheckTradeRules
    internal async Task<BinanceTradeRuleResult> CheckTradeRulesAsync(string symbol, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, BinanceFuturesOrderType type, CancellationToken ct)
    {
        var outputQuantity = quantity;
        var outputQuoteQuantity = quoteQuantity;
        var outputPrice = price;
        var outputStopPrice = stopPrice;

        if (Options.UsdtFuturesOptions.TradeRulesBehavior ==  BinanceTradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, quoteQuantity, outputPrice, outputStopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > Options.UsdtFuturesOptions.TradeRulesUpdateInterval.TotalMinutes)
            await GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        var symbolData = ExchangeInfo.Symbols.SingleOrDefault(s => string.Equals(s.Name, symbol, StringComparison.CurrentCultureIgnoreCase));
        if (symbolData == null)
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Symbol {symbol} not found");

        if (!symbolData.OrderTypes.Contains(type))
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: {type} order type not allowed for {symbol}");

        if (symbolData.LotSizeFilter != null || symbolData.MarketLotSizeFilter != null && type == BinanceFuturesOrderType.Market)
        {
            var minQty = symbolData.LotSizeFilter?.MinQuantity;
            var maxQty = symbolData.LotSizeFilter?.MaxQuantity;
            var stepSize = symbolData.LotSizeFilter?.StepSize;
            if (type == BinanceFuturesOrderType.Market && symbolData.MarketLotSizeFilter != null)
            {
                minQty = symbolData.MarketLotSizeFilter.MinQuantity;
                if (symbolData.MarketLotSizeFilter.MaxQuantity != 0)
                    maxQty = symbolData.MarketLotSizeFilter.MaxQuantity;

                if (symbolData.MarketLotSizeFilter.StepSize != 0)
                    stepSize = symbolData.MarketLotSizeFilter.StepSize;
            }

            if (minQty.HasValue && quantity.HasValue)
            {
                outputQuantity = BinanceHelpers.ClampQuantity(minQty.Value, maxQty!.Value, stepSize!.Value, quantity.Value);
                if (outputQuantity != quantity.Value)
                {
                    if (Options.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                    {
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: LotSize filter failed. Original quantity: {quantity}, Closest allowed: {outputQuantity}");
                    }

                    Logger.Log(LogLevel.Information, $"Quantity clamped from {quantity} to {outputQuantity}");
                }
            }
        }

        if (symbolData.MinNotionalFilter != null && outputQuoteQuantity != null)
        {
            if (quoteQuantity < symbolData.MinNotionalFilter.MinNotional)
            {
                if (Options.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                    return BinanceTradeRuleResult.CreateFailed(
                        $"Trade rules check failed: MinNotional filter failed. Order value: {quoteQuantity}, minimal order value: {symbolData.MinNotionalFilter.MinNotional}");

                outputQuoteQuantity = symbolData.MinNotionalFilter.MinNotional;
                Logger.Log(LogLevel.Information, $"QuoteQuantity adjusted from {quoteQuantity} to {outputQuoteQuantity} based on min notional filter");
            }
        }

        if (price == null)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, null, outputStopPrice);

        if (symbolData.PriceFilter != null)
        {
            if (symbolData.PriceFilter.MaxPrice != 0 && symbolData.PriceFilter.MinPrice != 0)
            {
                outputPrice = BinanceHelpers.ClampPrice(symbolData.PriceFilter.MinPrice, symbolData.PriceFilter.MaxPrice, price.Value);
                if (outputPrice != price)
                {
                    if (Options.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter max/min failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Logger.Log(LogLevel.Information, $"price clamped from {price} to {outputPrice}");
                }

                if (stopPrice != null)
                {
                    outputStopPrice = BinanceHelpers.ClampPrice(symbolData.PriceFilter.MinPrice,
                        symbolData.PriceFilter.MaxPrice, stopPrice.Value);
                    if (outputStopPrice != stopPrice)
                    {
                        if (Options.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter max/min failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");

                        Logger.Log(LogLevel.Information,
                            $"Stop price clamped from {stopPrice} to {outputStopPrice} based on price filter");
                    }
                }
            }

            if (symbolData.PriceFilter.TickSize != 0)
            {
                var beforePrice = outputPrice;
                outputPrice = BinanceHelpers.FloorPrice(symbolData.PriceFilter.TickSize, price.Value);
                if (outputPrice != beforePrice)
                {
                    if (Options.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter tick failed. Original price: {price}, Closest allowed: {outputPrice}");

                    Logger.Log(LogLevel.Information, $"price rounded from {beforePrice} to {outputPrice}");
                }

                if (stopPrice != null)
                {
                    var beforeStopPrice = outputStopPrice;
                    outputStopPrice = BinanceHelpers.FloorPrice(symbolData.PriceFilter.TickSize, stopPrice.Value);
                    if (outputStopPrice != beforeStopPrice)
                    {
                        if (Options.UsdtFuturesOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter tick failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");

                        Logger.Log(LogLevel.Information,
                            $"Stop price floored from {beforeStopPrice} to {outputStopPrice} based on price filter");
                    }
                }
            }
        }

        return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);
    }

}