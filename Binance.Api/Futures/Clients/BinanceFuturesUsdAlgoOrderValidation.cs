namespace Binance.Api.Futures;

internal static class BinanceFuturesUsdAlgoOrderValidation
{
    internal static void ValidatePlacement(
        string symbol,
        BinanceOrderSide side,
        BinanceFuturesAlgoOrderType type,
        BinancePositionSide? positionSide,
        BinanceTimeInForce? timeInForce,
        decimal? quantity,
        decimal? price,
        BinanceFuturesWorkingType? workingType,
        BinanceFuturesPriceMatch? priceMatch,
        bool? closePosition,
        bool? priceProtect,
        bool? reduceOnly,
        decimal? activatePrice,
        decimal? callbackRate,
        string? clientAlgoId,
        BinanceOrderResponseType? orderResponseType,
        BinanceSelfTradePreventionMode? selfTradePreventionMode,
        DateTime? goodTillDate)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("symbol cannot be empty", nameof(symbol));
        if (!Enum.IsDefined(typeof(BinanceOrderSide), side))
            throw new ArgumentOutOfRangeException(nameof(side), side, "Unsupported order side");
        if (!Enum.IsDefined(typeof(BinanceFuturesAlgoOrderType), type))
            throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported native conditional Algo order type");
        if (positionSide.HasValue && !Enum.IsDefined(typeof(BinancePositionSide), positionSide.Value))
            throw new ArgumentOutOfRangeException(nameof(positionSide), positionSide, "Unsupported position side");
        if (positionSide == BinancePositionSide.Hedge)
            throw new ArgumentOutOfRangeException(nameof(positionSide), positionSide, "HEDGE is not a valid positionSide value");
        if (timeInForce.HasValue && !Enum.IsDefined(typeof(BinanceTimeInForce), timeInForce.Value))
            throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, "Unsupported time in force");
        if (timeInForce == BinanceTimeInForce.GoodTillExpiredOrCanceled)
            throw new ArgumentOutOfRangeException(nameof(timeInForce), timeInForce, "GTE_GTC is not supported for native Algo orders");
        if (workingType.HasValue && !Enum.IsDefined(typeof(BinanceFuturesWorkingType), workingType.Value))
            throw new ArgumentOutOfRangeException(nameof(workingType), workingType, "Unsupported working type");
        if (orderResponseType.HasValue && !Enum.IsDefined(typeof(BinanceOrderResponseType), orderResponseType.Value))
            throw new ArgumentOutOfRangeException(nameof(orderResponseType), orderResponseType, "Unsupported response type");
        if (orderResponseType == BinanceOrderResponseType.Full)
            throw new ArgumentOutOfRangeException(nameof(orderResponseType), orderResponseType, "FULL is not supported for native Algo orders");
        if (priceMatch.HasValue && !Enum.IsDefined(typeof(BinanceFuturesPriceMatch), priceMatch.Value))
            throw new ArgumentOutOfRangeException(nameof(priceMatch), priceMatch, "Unsupported price matching mode");
        if (priceMatch == BinanceFuturesPriceMatch.None)
            throw new ArgumentOutOfRangeException(nameof(priceMatch), priceMatch, "NONE is not a valid placement priceMatch value");
        if (selfTradePreventionMode.HasValue && !Enum.IsDefined(typeof(BinanceSelfTradePreventionMode), selfTradePreventionMode.Value))
            throw new ArgumentOutOfRangeException(nameof(selfTradePreventionMode), selfTradePreventionMode, "Unsupported self-trade prevention mode");
        if (selfTradePreventionMode is BinanceSelfTradePreventionMode.Decrement or BinanceSelfTradePreventionMode.Transfer)
            throw new ArgumentOutOfRangeException(nameof(selfTradePreventionMode), selfTradePreventionMode, "Unsupported self-trade prevention mode");

        var isStopLimit = type is BinanceFuturesAlgoOrderType.Stop or BinanceFuturesAlgoOrderType.TakeProfit;
        var isStopMarket = type is BinanceFuturesAlgoOrderType.StopMarket or BinanceFuturesAlgoOrderType.TakeProfitMarket;
        var isTrailing = type == BinanceFuturesAlgoOrderType.TrailingStopMarket;
        if (closePosition.HasValue && !isStopMarket)
            throw new ArgumentException("closePosition is only available for STOP_MARKET or TAKE_PROFIT_MARKET", nameof(closePosition));
        if (priceProtect.HasValue && !isStopMarket)
            throw new ArgumentException("priceProtect is only available for STOP_MARKET or TAKE_PROFIT_MARKET", nameof(priceProtect));
        if (closePosition == true && quantity.HasValue)
            throw new ArgumentException("quantity cannot be sent with closePosition=true", nameof(quantity));
        if (closePosition == true && reduceOnly.HasValue)
            throw new ArgumentException("reduceOnly cannot be sent with closePosition=true", nameof(reduceOnly));
        if (reduceOnly.HasValue && positionSide is BinancePositionSide.Long or BinancePositionSide.Short)
            throw new ArgumentException("reduceOnly cannot be sent in Hedge Mode", nameof(reduceOnly));
        if (closePosition == true && positionSide == BinancePositionSide.Long && side == BinanceOrderSide.Buy)
            throw new ArgumentException("A BUY close-all order cannot use LONG positionSide", nameof(side));
        if (closePosition == true && positionSide == BinancePositionSide.Short && side == BinanceOrderSide.Sell)
            throw new ArgumentException("A SELL close-all order cannot use SHORT positionSide", nameof(side));
        if (priceMatch.HasValue && !isStopLimit)
            throw new ArgumentException("priceMatch is only available for STOP or TAKE_PROFIT", nameof(priceMatch));
        if (priceMatch.HasValue && price.HasValue)
            throw new ArgumentException("price and priceMatch cannot be sent together");
        if ((activatePrice.HasValue || callbackRate.HasValue) && !isTrailing)
            throw new ArgumentException("activatePrice and callbackRate are only available for TRAILING_STOP_MARKET");
        if (callbackRate is < 0.1m or > 10m)
            throw new ArgumentOutOfRangeException(nameof(callbackRate), callbackRate, "callbackRate must be between 0.1 and 10");
        if (clientAlgoId is not null && !System.Text.RegularExpressions.Regex.IsMatch(clientAlgoId, @"^[\.A-Z\:/a-z0-9_-]{1,36}$", System.Text.RegularExpressions.RegexOptions.CultureInvariant))
            throw new ArgumentException("clientAlgoId does not match the documented format", nameof(clientAlgoId));
        if (timeInForce == BinanceTimeInForce.GoodTillDate && !goodTillDate.HasValue)
            throw new ArgumentException("goodTillDate is required when timeInForce is GTD", nameof(goodTillDate));
        if (goodTillDate.HasValue)
        {
            var goodTillDateMilliseconds = new DateTimeOffset(goodTillDate.Value.ToUniversalTime()).ToUnixTimeMilliseconds();
            if (goodTillDateMilliseconds <= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 600_000)
                throw new ArgumentOutOfRangeException(nameof(goodTillDate), goodTillDate, "goodTillDate must be more than 600 seconds in the future");
            if (goodTillDateMilliseconds >= 253_402_300_799_000L)
                throw new ArgumentOutOfRangeException(nameof(goodTillDate), goodTillDate, "goodTillDate exceeds the documented maximum");
        }
    }

    internal static void ValidateCancellation(long? algoId, string? clientAlgoId)
    {
        if (clientAlgoId is not null && string.IsNullOrWhiteSpace(clientAlgoId))
            throw new ArgumentException("clientAlgoId cannot be empty when provided", nameof(clientAlgoId));
        if (algoId is null && clientAlgoId is null)
            throw new ArgumentException("Either algoId or clientAlgoId must be sent");
    }
}
