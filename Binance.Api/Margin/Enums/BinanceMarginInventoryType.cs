namespace Binance.Api.Margin;

/// <summary>
/// Type of margin inventory.
/// </summary>
public enum BinanceMarginInventoryType : byte
{
    /// <summary>
    /// Represents a regular margin account.
    /// </summary>
    [Map("MARGIN")]
    Margin = 1,

    /// <summary>
    /// Represents an isolated margin account.
    /// </summary>
    [Map("ISOLATED")]
    Isolated = 2
}
