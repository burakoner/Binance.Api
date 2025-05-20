namespace Binance.Api.SimpleEarn;

/// <summary>
/// Simple earn subscription status
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceSimpleEarnSubscriptionStatus : byte
{
    /// <summary>
    /// Purchasing
    /// </summary>
    [Map("PURCHASING")]
    Purchasing = 1,

    /// <summary>
    /// Success
    /// </summary>
    [Map("SUCCESS")]
    Success = 2,

    /// <summary>
    /// Failed
    /// </summary>
    [Map("FAILED")]
    Failed = 3
}
