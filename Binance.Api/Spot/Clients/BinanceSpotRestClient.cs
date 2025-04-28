namespace Binance.Api.Spot;

internal partial class BinanceSpotRestClient(BinanceRestApiClient root) : IBinanceSpotRestClient
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Parent
    internal BinanceRestApiClient _ { get; } = root;

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;
    internal DateTime? LastExchangeInfoUpdate { get; private set; }
    internal BinanceExchangeInfo? ExchangeInfo { get; private set; }

    internal Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => _.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal Uri GetUrl(string api, string version, string endpoint)
    {
        var url = BinanceAddress.Default.SpotRestApiAddress.AppendPath(api);
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }

    internal async Task<BinanceTradeRuleResult> CheckTradeRulesAsync(string symbol, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, BinanceSpotOrderType? type, CancellationToken ct)
    {
        if (RestOptions.SpotOptions.TradeRulesBehavior == BinanceTradeRulesBehavior.None)
            return BinanceTradeRuleResult.CreatePassed(quantity, quoteQuantity, price, stopPrice);

        if (ExchangeInfo == null || LastExchangeInfoUpdate == null || (DateTime.UtcNow - LastExchangeInfoUpdate.Value).TotalMinutes > RestOptions.SpotOptions.TradeRulesUpdateInterval.TotalMinutes)
            await GetExchangeInfoAsync(ct).ConfigureAwait(false);

        if (ExchangeInfo == null)
            return BinanceTradeRuleResult.CreateFailed("Unable to retrieve trading rules, validation failed");

        return ValidateTradeRules(Logger, RestOptions.SpotOptions.TradeRulesBehavior, ExchangeInfo, symbol, quantity, quoteQuantity, price, stopPrice, type);
    }

    internal static BinanceTradeRuleResult ValidateTradeRules(ILogger? logger, BinanceTradeRulesBehavior tradeRulesBehavior, BinanceExchangeInfo exchangeInfo, string symbol, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice, BinanceSpotOrderType? type)
    {
        var outputQuantity = quantity;
        var outputQuoteQuantity = quoteQuantity;
        var outputPrice = price;
        var outputStopPrice = stopPrice;

        var symbolData = exchangeInfo.Symbols.SingleOrDefault(s => string.Equals(s.Symbol, symbol, StringComparison.CurrentCultureIgnoreCase));
        if (symbolData == null)
            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Symbol {symbol} not found");

        if (type != null)
        {
            if (!symbolData.OrderTypes.Contains(type.Value))
            {
                return BinanceTradeRuleResult.CreateFailed(
                    $"Trade rules check failed: {type} order type not allowed for {symbol}");
            }
        }

        if (symbolData.LotSizeFilter != null || symbolData.MarketLotSizeFilter != null && type == BinanceSpotOrderType.Market)
        {
            var minQty = symbolData.LotSizeFilter?.MinQuantity;
            var maxQty = symbolData.LotSizeFilter?.MaxQuantity;
            var stepSize = symbolData.LotSizeFilter?.StepSize;
            if (type == BinanceSpotOrderType.Market && symbolData.MarketLotSizeFilter != null)
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
                    if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                    {
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: LotSize filter failed. Original quantity: {quantity}, Closest allowed: {outputQuantity}");
                    }

                    logger?.Log(LogLevel.Information, $"Quantity clamped from {quantity} to {outputQuantity} based on lot size filter");
                }
            }
        }

        if (symbolData.MinNotionalFilter != null && outputQuoteQuantity != null)
        {
            if (quoteQuantity < symbolData.MinNotionalFilter.MinNotional)
            {
                if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                {
                    return BinanceTradeRuleResult.CreateFailed(
                        $"Trade rules check failed: MinNotional filter failed. Order value: {quoteQuantity}, minimal order value: {symbolData.MinNotionalFilter.MinNotional}");
                }

                outputQuoteQuantity = symbolData.MinNotionalFilter.MinNotional;
                logger?.Log(LogLevel.Information, $"QuoteQuantity adjusted from {quoteQuantity} to {outputQuoteQuantity} based on min notional filter");
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
                    if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter max/min failed. Original price: {price}, Closest allowed: {outputPrice}");

                    logger?.Log(LogLevel.Information, $"price clamped from {price} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    outputStopPrice = BinanceHelpers.ClampPrice(symbolData.PriceFilter.MinPrice,
                        symbolData.PriceFilter.MaxPrice, stopPrice.Value);
                    if (outputStopPrice != stopPrice)
                    {
                        if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        {
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter max/min failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");
                        }

                        logger?.Log(LogLevel.Information,
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
                    if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter tick failed. Original price: {price}, Closest allowed: {outputPrice}");

                    logger?.Log(LogLevel.Information, $"price floored from {beforePrice} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    var beforeStopPrice = outputStopPrice;
                    outputStopPrice = BinanceHelpers.FloorPrice(symbolData.PriceFilter.TickSize, stopPrice.Value);
                    if (outputStopPrice != beforeStopPrice)
                    {
                        if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        {
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter tick failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");
                        }

                        logger?.Log(LogLevel.Information,
                            $"Stop price floored from {beforeStopPrice} to {outputStopPrice} based on price filter");
                    }
                }
            }
        }

        if (symbolData.MinNotionalFilter == null || quantity == null || outputPrice == null)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);

        var currentQuantity = outputQuantity ?? quantity.Value;
        var notional = currentQuantity * outputPrice.Value;
        if (notional < symbolData.MinNotionalFilter.MinNotional)
        {
            if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
            {
                return BinanceTradeRuleResult.CreateFailed(
                    $"Trade rules check failed: MinNotional filter failed. Order quantity: {notional}, minimal order quantity: {symbolData.MinNotionalFilter.MinNotional}");
            }

            if (symbolData.LotSizeFilter == null)
                return BinanceTradeRuleResult.CreateFailed("Trade rules check failed: MinNotional filter failed. Unable to auto comply because LotSizeFilter not present");

            var minQuantity = symbolData.MinNotionalFilter.MinNotional / outputPrice.Value;
            var stepSize = symbolData.LotSizeFilter!.StepSize;
            outputQuantity = BinanceHelpers.Floor(minQuantity + (stepSize - minQuantity % stepSize));
            logger?.Log(LogLevel.Information, $"Quantity clamped from {currentQuantity} to {outputQuantity} based on min notional filter");
        }

        return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);
    }
}