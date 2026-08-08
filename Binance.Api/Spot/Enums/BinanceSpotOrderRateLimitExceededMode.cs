namespace Binance.Api.Spot;

/// <summary>
/// Behavior when a cancel-replace request encounters the unfilled-order rate limit.
/// </summary>
public enum BinanceSpotOrderRateLimitExceededMode : byte
{
    /// <summary>Do not attempt the cancellation after the limit has been exceeded.</summary>
    [Map("DO_NOTHING")]
    DoNothing = 1,

    /// <summary>Always attempt the cancellation, but do not place the replacement order.</summary>
    [Map("CANCEL_ONLY")]
    CancelOnly
}
