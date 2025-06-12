namespace Binance.Api.Options;

/// <summary>
/// Options Side
/// </summary>
public enum BinanceOptionsSide : byte
{
    /// <summary>
    /// CALL Long
    /// </summary>
    [Map("CALL")]
    Call = 1,

    /// <summary>
    /// PUT Short
    /// </summary>
    [Map("PUT")]
    Put = 2,
}
