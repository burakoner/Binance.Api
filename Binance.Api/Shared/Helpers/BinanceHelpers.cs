using Binance.Api.Futures;
using Binance.Api.Options;
using Binance.Api.Spot;

namespace Binance.Api.Shared;

/// <summary>
/// Helper methods for the Binance API
/// </summary>
public static class BinanceHelpers
{
    /// <summary>
    /// Get the used weight from the response headers
    /// </summary>
    /// <param name="headers"></param>
    /// <returns></returns>
    public static int? UsedWeight(this IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
    {
        if (headers == null)
            return null;

        var headerValues = headers.SingleOrDefault(s => s.Key.StartsWith("X-MBX-USED-WEIGHT-", StringComparison.InvariantCultureIgnoreCase)).Value;
        if (headerValues != null && int.TryParse(headerValues.First(), out var value))
            return value;
        return null;
    }

    /// <summary>
    /// Get the used weight from the response headers
    /// </summary>
    /// <param name="headers"></param>
    /// <returns></returns>
    public static int? UsedOrderCount(this IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
    {
        if (headers == null)
            return null;

        var headerValues = headers.SingleOrDefault(s => s.Key.StartsWith("X-MBX-ORDER-COUNT-", StringComparison.InvariantCultureIgnoreCase)).Value;
        if (headerValues != null && int.TryParse(headerValues.First(), out var value))
            return value;
        return null;
    }

    /// <summary>
    /// Clamp a quantity between a min and max quantity and floor to the closest step
    /// </summary>
    /// <param name="minQuantity"></param>
    /// <param name="maxQuantity"></param>
    /// <param name="stepSize"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public static decimal ClampQuantity(decimal minQuantity, decimal maxQuantity, decimal stepSize, decimal quantity)
    {
        quantity = Math.Min(maxQuantity, quantity);
        quantity = Math.Max(minQuantity, quantity);
        if (stepSize == 0)
            return quantity;
        quantity -= quantity % stepSize;
        quantity = Floor(quantity);
        return quantity;
    }

    /// <summary>
    /// Clamp a price between a min and max price
    /// </summary>
    /// <param name="minPrice"></param>
    /// <param name="maxPrice"></param>
    /// <param name="price"></param>
    /// <returns></returns>
    public static decimal ClampPrice(decimal minPrice, decimal maxPrice, decimal price)
    {
        price = Math.Min(maxPrice, price);
        price = Math.Max(minPrice, price);
        return price;
    }

    /// <summary>
    /// Floor a price to the closest tick
    /// </summary>
    /// <param name="tickSize"></param>
    /// <param name="price"></param>
    /// <returns></returns>
    public static decimal FloorPrice(decimal tickSize, decimal price)
    {
        price -= price % tickSize;
        price = Floor(price);
        return price;
    }

    /// <summary>
    /// Floor
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static decimal Floor(decimal number)
    {
        return Math.Floor(number * 100000000) / 100000000;
    }

    /// <summary>
    /// Client order id separator
    /// </summary>
    public const string ClientOrderIdSeparator = "-";

    /// <summary>
    /// Apply broker id to a client order id
    /// </summary>
    /// <param name="clientOrderId"></param>
    /// <param name="brokerId"></param>
    /// <param name="maxLength"></param>
    /// <param name="allowValueAdjustment"></param>
    /// <returns></returns>
    public static string ApplyBrokerId(string? clientOrderId, string brokerId, int maxLength, bool allowValueAdjustment)
    {
        var reservedLength = brokerId.Length + ClientOrderIdSeparator.Length;

        if ((clientOrderId?.Length + reservedLength) > maxLength)
            return clientOrderId!;

        if (!string.IsNullOrEmpty(clientOrderId))
        {
            if (allowValueAdjustment && !clientOrderId!.StartsWith(brokerId + ClientOrderIdSeparator))
                clientOrderId = brokerId + ClientOrderIdSeparator + clientOrderId;

            return clientOrderId!;
        }
        else
        {
            // if (string.IsNullOrEmpty(clientOrderId) || !clientOrderId!.StartsWith(brokerId + ClientOrderIdSeparator))
                clientOrderId = ExchangeHelpers.AppendRandomString(brokerId + ClientOrderIdSeparator, maxLength);
        }

        return clientOrderId;
    }

    internal static BinanceTradeRuleResult ValidateSpotTradingRules(ILogger? logger, BinanceTradeRulesBehavior tradeRulesBehavior, BinanceSpotSymbol symbolInfo, BinanceSpotOrderType? orderType, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice)
    {
        var outputQuantity = quantity;
        var outputQuoteQuantity = quoteQuantity;
        var outputPrice = price;
        var outputStopPrice = stopPrice;

        if (orderType != null)
        {
            if (!symbolInfo.OrderTypes.Contains(orderType.Value))
            {
                return BinanceTradeRuleResult.CreateFailed(
                    $"Trade rules check failed: {orderType} order type not allowed for {symbolInfo.Symbol}");
            }
        }

        if (symbolInfo.LotSizeFilter != null || symbolInfo.MarketLotSizeFilter != null && orderType == BinanceSpotOrderType.Market)
        {
            var minQty = symbolInfo.LotSizeFilter?.MinQuantity;
            var maxQty = symbolInfo.LotSizeFilter?.MaxQuantity;
            var stepSize = symbolInfo.LotSizeFilter?.StepSize;
            if (orderType == BinanceSpotOrderType.Market && symbolInfo.MarketLotSizeFilter != null)
            {
                minQty = symbolInfo.MarketLotSizeFilter.MinQuantity;
                if (symbolInfo.MarketLotSizeFilter.MaxQuantity != 0)
                    maxQty = symbolInfo.MarketLotSizeFilter.MaxQuantity;

                if (symbolInfo.MarketLotSizeFilter.StepSize != 0)
                    stepSize = symbolInfo.MarketLotSizeFilter.StepSize;
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

        if (symbolInfo.MinNotionalFilter != null && outputQuoteQuantity != null)
        {
            if (quoteQuantity < symbolInfo.MinNotionalFilter.MinNotional)
            {
                if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                {
                    return BinanceTradeRuleResult.CreateFailed(
                        $"Trade rules check failed: MinNotional filter failed. Order value: {quoteQuantity}, minimal order value: {symbolInfo.MinNotionalFilter.MinNotional}");
                }

                outputQuoteQuantity = symbolInfo.MinNotionalFilter.MinNotional;
                logger?.Log(LogLevel.Information, $"QuoteQuantity adjusted from {quoteQuantity} to {outputQuoteQuantity} based on min notional filter");
            }
        }

        if (price == null)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, null, outputStopPrice);

        if (symbolInfo.PriceFilter != null)
        {
            if (symbolInfo.PriceFilter.MaxPrice != 0 && symbolInfo.PriceFilter.MinPrice != 0)
            {
                outputPrice = BinanceHelpers.ClampPrice(symbolInfo.PriceFilter.MinPrice, symbolInfo.PriceFilter.MaxPrice, price.Value);
                if (outputPrice != price)
                {
                    if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter max/min failed. Original price: {price}, Closest allowed: {outputPrice}");

                    logger?.Log(LogLevel.Information, $"price clamped from {price} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    outputStopPrice = BinanceHelpers.ClampPrice(symbolInfo.PriceFilter.MinPrice,
                        symbolInfo.PriceFilter.MaxPrice, stopPrice.Value);
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

            if (symbolInfo.PriceFilter.TickSize != 0)
            {
                var beforePrice = outputPrice;
                outputPrice = BinanceHelpers.FloorPrice(symbolInfo.PriceFilter.TickSize, price.Value);
                if (outputPrice != beforePrice)
                {
                    if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter tick failed. Original price: {price}, Closest allowed: {outputPrice}");

                    logger?.Log(LogLevel.Information, $"price floored from {beforePrice} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    var beforeStopPrice = outputStopPrice;
                    outputStopPrice = BinanceHelpers.FloorPrice(symbolInfo.PriceFilter.TickSize, stopPrice.Value);
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

        if (symbolInfo.MinNotionalFilter == null || quantity == null || outputPrice == null)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);

        var currentQuantity = outputQuantity ?? quantity.Value;
        var notional = currentQuantity * outputPrice.Value;
        if (notional < symbolInfo.MinNotionalFilter.MinNotional)
        {
            if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
            {
                return BinanceTradeRuleResult.CreateFailed(
                    $"Trade rules check failed: MinNotional filter failed. Order quantity: {notional}, minimal order quantity: {symbolInfo.MinNotionalFilter.MinNotional}");
            }

            if (symbolInfo.LotSizeFilter == null)
                return BinanceTradeRuleResult.CreateFailed("Trade rules check failed: MinNotional filter failed. Unable to auto comply because LotSizeFilter not present");

            var minQuantity = symbolInfo.MinNotionalFilter.MinNotional / outputPrice.Value;
            var stepSize = symbolInfo.LotSizeFilter!.StepSize;
            outputQuantity = BinanceHelpers.Floor(minQuantity + (stepSize - minQuantity % stepSize));
            logger?.Log(LogLevel.Information, $"Quantity clamped from {currentQuantity} to {outputQuantity} based on min notional filter");
        }

        return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);
    }

    internal static BinanceTradeRuleResult ValidateFuturesTradingRules(ILogger? logger, BinanceTradeRulesBehavior tradeRulesBehavior, BinanceFuturesSymbol symbolInfo, BinanceFuturesOrderType? orderType, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice)
    {
        var outputQuantity = quantity;
        var outputQuoteQuantity = quoteQuantity;
        var outputPrice = price;
        var outputStopPrice = stopPrice;

        if (orderType != null)
        {
            if (!symbolInfo.OrderTypes.Contains(orderType.Value))
            {
                return BinanceTradeRuleResult.CreateFailed(
                    $"Trade rules check failed: {orderType} order type not allowed for {symbolInfo.Symbol}");
            }
        }

        if (symbolInfo.LotSizeFilter != null || symbolInfo.MarketLotSizeFilter != null && orderType == BinanceFuturesOrderType.Market)
        {
            var minQty = symbolInfo.LotSizeFilter?.MinQuantity;
            var maxQty = symbolInfo.LotSizeFilter?.MaxQuantity;
            var stepSize = symbolInfo.LotSizeFilter?.StepSize;
            if (orderType == BinanceFuturesOrderType.Market && symbolInfo.MarketLotSizeFilter != null)
            {
                minQty = symbolInfo.MarketLotSizeFilter.MinQuantity;
                if (symbolInfo.MarketLotSizeFilter.MaxQuantity != 0)
                    maxQty = symbolInfo.MarketLotSizeFilter.MaxQuantity;

                if (symbolInfo.MarketLotSizeFilter.StepSize != 0)
                    stepSize = symbolInfo.MarketLotSizeFilter.StepSize;
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

                    logger?.Log(LogLevel.Information, $"Quantity clamped from {quantity} to {outputQuantity}");
                }
            }
        }

        if (symbolInfo.MinNotionalFilter != null && outputQuoteQuantity != null)
        {
            if (quoteQuantity < symbolInfo.MinNotionalFilter.MinNotional)
            {
                if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                    return BinanceTradeRuleResult.CreateFailed(
                        $"Trade rules check failed: MinNotional filter failed. Order value: {quoteQuantity}, minimal order value: {symbolInfo.MinNotionalFilter.MinNotional}");

                outputQuoteQuantity = symbolInfo.MinNotionalFilter.MinNotional;
                logger?.Log(LogLevel.Information, $"QuoteQuantity adjusted from {quoteQuantity} to {outputQuoteQuantity} based on min notional filter");
            }
        }

        if (price == null)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, null, outputStopPrice);

        if (symbolInfo.PriceFilter != null)
        {
            if (symbolInfo.PriceFilter.MaxPrice != 0 && symbolInfo.PriceFilter.MinPrice != 0)
            {
                outputPrice = BinanceHelpers.ClampPrice(symbolInfo.PriceFilter.MinPrice, symbolInfo.PriceFilter.MaxPrice, price.Value);
                if (outputPrice != price)
                {
                    if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter max/min failed. Original price: {price}, Closest allowed: {outputPrice}");

                    logger?.Log(LogLevel.Information, $"price clamped from {price} to {outputPrice}");
                }

                if (stopPrice != null)
                {
                    outputStopPrice = BinanceHelpers.ClampPrice(symbolInfo.PriceFilter.MinPrice,
                        symbolInfo.PriceFilter.MaxPrice, stopPrice.Value);
                    if (outputStopPrice != stopPrice)
                    {
                        if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                            return BinanceTradeRuleResult.CreateFailed(
                                $"Trade rules check failed: Stop price filter max/min failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");

                        logger?.Log(LogLevel.Information, $"Stop price clamped from {stopPrice} to {outputStopPrice} based on price filter");
                    }
                }
            }

            if (symbolInfo.PriceFilter.TickSize != 0)
            {
                var beforePrice = outputPrice;
                outputPrice = BinanceHelpers.FloorPrice(symbolInfo.PriceFilter.TickSize, price.Value);
                if (outputPrice != beforePrice)
                {
                    if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter tick failed. Original price: {price}, Closest allowed: {outputPrice}");

                    logger?.Log(LogLevel.Information, $"price rounded from {beforePrice} to {outputPrice}");
                }

                if (stopPrice != null)
                {
                    var beforeStopPrice = outputStopPrice;
                    outputStopPrice = BinanceHelpers.FloorPrice(symbolInfo.PriceFilter.TickSize, stopPrice.Value);
                    if (outputStopPrice != beforeStopPrice)
                    {
                        if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                            return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Stop price filter tick failed. Original stop price: {stopPrice}, Closest allowed: {outputStopPrice}");

                        logger?.Log(LogLevel.Information,
                            $"Stop price floored from {beforeStopPrice} to {outputStopPrice} based on price filter");
                    }
                }
            }
        }

        return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);
    }

    internal static BinanceTradeRuleResult ValidateOptionsTradingRules(ILogger? logger, BinanceTradeRulesBehavior tradeRulesBehavior, BinanceOptionsSymbol symbolInfo, BinanceOptionsOrderType? orderType, decimal? quantity, decimal? quoteQuantity, decimal? price, decimal? stopPrice)
    {
        var outputQuantity = quantity;
        var outputQuoteQuantity = quoteQuantity;
        var outputPrice = price;
        var outputStopPrice = stopPrice;

        if (symbolInfo.LotSizeFilter != null)
        {
            var minQty = symbolInfo.LotSizeFilter?.MinQuantity;
            var maxQty = symbolInfo.LotSizeFilter?.MaxQuantity;
            var stepSize = symbolInfo.LotSizeFilter?.StepSize;

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

        if (price == null)
            return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, null, outputStopPrice);

        if (symbolInfo.PriceFilter != null)
        {
            if (symbolInfo.PriceFilter.MaxPrice != 0 && symbolInfo.PriceFilter.MinPrice != 0)
            {
                outputPrice = BinanceHelpers.ClampPrice(symbolInfo.PriceFilter.MinPrice, symbolInfo.PriceFilter.MaxPrice, price.Value);
                if (outputPrice != price)
                {
                    if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter max/min failed. Original price: {price}, Closest allowed: {outputPrice}");

                    logger?.Log(LogLevel.Information, $"price clamped from {price} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    outputStopPrice = BinanceHelpers.ClampPrice(symbolInfo.PriceFilter.MinPrice,
                        symbolInfo.PriceFilter.MaxPrice, stopPrice.Value);
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

            if (symbolInfo.PriceFilter.TickSize != 0)
            {
                var beforePrice = outputPrice;
                outputPrice = BinanceHelpers.FloorPrice(symbolInfo.PriceFilter.TickSize, price.Value);
                if (outputPrice != beforePrice)
                {
                    if (tradeRulesBehavior == BinanceTradeRulesBehavior.ThrowError)
                        return BinanceTradeRuleResult.CreateFailed($"Trade rules check failed: Price filter tick failed. Original price: {price}, Closest allowed: {outputPrice}");

                    logger?.Log(LogLevel.Information, $"price floored from {beforePrice} to {outputPrice} based on price filter");
                }

                if (stopPrice != null)
                {
                    var beforeStopPrice = outputStopPrice;
                    outputStopPrice = BinanceHelpers.FloorPrice(symbolInfo.PriceFilter.TickSize, stopPrice.Value);
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

        return BinanceTradeRuleResult.CreatePassed(outputQuantity, outputQuoteQuantity, outputPrice, outputStopPrice);
    }

}