namespace Binance.Api.Spot;

/// <summary>
/// Cancel replace mode
/// </summary>
public enum BinanceSpotOrderCancelReplaceMode : byte
{
    /// <summary>
    /// If the cancel request fails, the new order placement will not be attempted.
    /// </summary>
    [Map("STOP_ON_FAILURE")]
    StopOnFailure = 1,

    /// <summary>
    /// New order placement will be attempted even if cancel request fails.
    /// </summary>
    [Map("ALLOW_FAILURE")]
    AllowFailure
}
