namespace Binance.Api.Shared;

/// <summary>
/// Position side
/// </summary>
public enum BinancePositionSide : byte
{
    /// <summary>
    /// Hedge
    /// </summary>
    [Map("HEDGE")]
    Hedge = 0,

    /// <summary>
    /// Short
    /// </summary>
    [Map("SHORT")]
    Short = 1,

    /// <summary>
    /// Long
    /// </summary>
    [Map("LONG")]
    Long = 2,

    /// <summary>
    /// Both for One-way mode when placing an order
    /// </summary>
    [Map("BOTH")]
    Both = 3
}
