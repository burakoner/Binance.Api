namespace Binance.Api.SimpleEarn;

/// <summary>
/// Account source
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceSimpleEarnSourceAccount : byte
{
    /// <summary>
    /// All
    /// </summary>
    [Map("ALL")]
    All = 0,

    /// <summary>
    /// Spot
    /// </summary>
    [Map("SPOT")]
    Spot = 1,

    /// <summary>
    /// Fund
    /// </summary>
    [Map("FUND")]
    Fund = 2,
}
