namespace Binance.Api.Spot;

/// <summary>
/// Current Spot symbol trading status.
/// </summary>
public enum BinanceSpotSymbolStatus : byte
{
    /// <summary>
    /// Trading is active.
    /// </summary>
    [Map("TRADING")]
    Trading = 1,

    /// <summary>
    /// Trading is halted.
    /// </summary>
    [Map("HALT")]
    Halt,

    /// <summary>
    /// Trading is on break.
    /// </summary>
    [Map("BREAK")]
    Break
}
