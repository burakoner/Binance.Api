namespace Binance.Api.Spot;

/// <summary>
/// Symbol statuses accepted by Spot request filters.
/// </summary>
public enum BinanceSpotSymbolStatusFilter : byte
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
