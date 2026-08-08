namespace Binance.Api.Spot;

/// <summary>
/// The reason the order was rejected
/// </summary>
public enum BinanceSpotOrderRejectReason : byte
{
    /// <summary>
    /// Not rejected
    /// </summary>
    [Map("NONE")]
    None = 1,

    /// <summary>
    /// Insufficient balance
    /// </summary>
    [Map("INSUFFICIENT_BALANCES")]
    InsufficientBalances,

    /// <summary>
    /// Stop price would trigger immediately
    /// </summary>
    [Map("STOP_PRICE_WOULD_TRIGGER_IMMEDIATELY")]
    StopPriceWouldTriggerImmediately,

    /// <summary>
    /// Order would match immediately
    /// </summary>
    [Map("WOULD_MATCH_IMMEDIATELY")]
    WouldMatchImmediately,

    /// <summary>
    /// Invalid relationship between order-list prices
    /// </summary>
    [Map("OCO_BAD_PRICES")]
    OcoBadPrices
}
