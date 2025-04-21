namespace Binance.Api.Spot;

/// <summary>
/// Restrictions for order cancelation
/// </summary>
public enum BinanceSpotOrderCancelRestriction : byte
{
    /// <summary>
    /// Cancel will succeed if the order status is New
    /// </summary>
    [Map("ONLY_NEW")]
    OnlyNew = 1,

    /// <summary>
    /// Cancel will succeed if order status is PartiallyFilled
    /// </summary>
    [Map("ONLY_PARTIALLY_FILLED")]
    OnlyPartiallyFilled
}
