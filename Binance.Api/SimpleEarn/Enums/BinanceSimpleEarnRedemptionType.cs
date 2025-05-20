namespace Binance.Api.SimpleEarn;

/// <summary>
/// Simple earn redemption type
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceSimpleEarnRedemptionType : byte
{
    /// <summary>
    /// Redeem to spot account
    /// </summary>
    [Map("MATURE")]
    ToSpot = 1,

    /// <summary>
    /// Redeem to flexible product
    /// </summary>
    [Map("NEW_TRANSFERRED")]
    ToFlexibleProduct = 2,

    /// <summary>
    /// Early redemption
    /// </summary>
    [Map("AHEAD")]
    EarlyRedemption = 3,
}
