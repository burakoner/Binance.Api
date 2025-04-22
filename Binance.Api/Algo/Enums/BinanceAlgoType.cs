namespace Binance.Api.Algo;

/// <summary>
/// Binance Algo Type
/// </summary>
public enum BinanceAlgoType : byte
{
    /// <summary>
    /// Volume Participation
    /// </summary>
    [Map("VP")]
    VP = 1,

    /// <summary>
    /// Time-Weighted Average Price
    /// </summary>
    [Map("TWAP")]
    TWAP = 2,
}
