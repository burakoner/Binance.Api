namespace Binance.Api.Spot;

/// <summary>
/// Method used to calculate a Spot reference price.
/// </summary>
public enum BinanceSpotReferencePriceCalculationType : byte
{
    /// <summary>
    /// Arithmetic mean calculated by the matching engine.
    /// </summary>
    [Map("ARITHMETIC_MEAN")]
    ArithmeticMean = 1,

    /// <summary>
    /// Calculation performed outside the matching engine.
    /// </summary>
    [Map("EXTERNAL")]
    External
}
