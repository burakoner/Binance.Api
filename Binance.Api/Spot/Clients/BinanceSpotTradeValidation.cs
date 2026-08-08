namespace Binance.Api.Spot;

internal static class BinanceSpotTradeValidation
{
    public static void ReceiveWindow(decimal? receiveWindow)
        => BinanceSpotAccountValidation.ReceiveWindow(receiveWindow);

    public static void StrategyType(long? strategyType)
    {
        if (strategyType is < 1_000_000)
            throw new ArgumentOutOfRangeException(nameof(strategyType), "Strategy type must be at least 1000000.");
    }

    public static void Peg(BinanceSpotOrderType orderType, BinanceSpotPegPriceType? pegPriceType, int? pegOffsetValue, BinanceSpotPegOffsetType? pegOffsetType)
    {
        if (pegOffsetValue is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(pegOffsetValue), "Peg offset value must be between 0 and 100 price levels.");
        if (pegOffsetValue.HasValue != pegOffsetType.HasValue)
            throw new ArgumentException("pegOffsetValue and pegOffsetType must be provided together.");
        if (!pegPriceType.HasValue && (pegOffsetValue.HasValue || pegOffsetType.HasValue))
            throw new ArgumentException("pegPriceType is required when a peg offset is provided.");
        if (pegPriceType.HasValue && orderType is not BinanceSpotOrderType.Limit
            and not BinanceSpotOrderType.LimitMaker
            and not BinanceSpotOrderType.StopLossLimit
            and not BinanceSpotOrderType.TakeProfitLimit)
            throw new ArgumentException("Pegged pricing is supported only for LIMIT, LIMIT_MAKER, STOP_LOSS_LIMIT, and TAKE_PROFIT_LIMIT orders.", nameof(orderType));
    }

    public static void OrderIdentifiers(long? orderId, string? originalClientOrderId)
    {
        if (!orderId.HasValue && string.IsNullOrEmpty(originalClientOrderId))
            throw new ArgumentException("Either orderId or originalClientOrderId must be sent.");
    }

    public static void Amend(decimal newQuantity, long? orderId, string? originalClientOrderId)
    {
        if (newQuantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(newQuantity), "New quantity must be greater than zero.");
        OrderIdentifiers(orderId, originalClientOrderId);
    }

    public static void SmartOrderRouting(BinanceSpotOrderType type, decimal quantity, long? strategyType)
    {
        if (type is not BinanceSpotOrderType.Limit and not BinanceSpotOrderType.Market)
            throw new ArgumentException("Smart Order Routing supports only LIMIT and MARKET orders.", nameof(type));
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        StrategyType(strategyType);
    }
}
