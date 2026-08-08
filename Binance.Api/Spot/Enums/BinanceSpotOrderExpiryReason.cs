namespace Binance.Api.Spot;

/// <summary>
/// Reason an order expired.
/// </summary>
public enum BinanceSpotOrderExpiryReason : byte
{
    /// <summary>No expiry reason.</summary>
    [Map("NONE")]
    None = 0,

    /// <summary>The order was rejected.</summary>
    [Map("REJECTED")]
    Rejected,

    /// <summary>The exchange canceled the order.</summary>
    [Map("EXCHANGE_CANCELED")]
    ExchangeCanceled,

    /// <summary>An OCO order triggered.</summary>
    [Map("OCO_TRIGGER")]
    OcoTrigger,

    /// <summary>The first phase of an OTO expired.</summary>
    [Map("OTO_PHASE_ONE_EXPIRED")]
    OtoPhaseOneExpired,

    /// <summary>The unfilled IOC quantity expired.</summary>
    [Map("UNFILLED_IOC_QUANTITY_EXPIRED")]
    UnfilledIocQuantityExpired,

    /// <summary>The unfilled FOK order expired.</summary>
    [Map("UNFILLED_FOK_ORDER_EXPIRED")]
    UnfilledFokOrderExpired,

    /// <summary>Available liquidity was insufficient.</summary>
    [Map("INSUFFICIENT_LIQUIDITY")]
    InsufficientLiquidity,

    /// <summary>The execution-rule price range was exceeded.</summary>
    [Map("EXECUTION_RULE_PRICE_RANGE_EXCEEDED")]
    ExecutionRulePriceRangeExceeded
}
