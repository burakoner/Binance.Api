namespace Binance.Api.SimpleEarn;

/// <summary>
/// Binance Simple Earn Redeem Option
/// </summary>
public enum BinanceSimpleEarnRedeemOption : byte
{
    /// <summary>
    /// Spot
    /// </summary>
    [Map("SPOT")]
    Spot = 1,

    /// <summary>
    /// Flexible
    /// </summary>
    [Map("FLEXIBLE")]
    Flexible = 2,
}
