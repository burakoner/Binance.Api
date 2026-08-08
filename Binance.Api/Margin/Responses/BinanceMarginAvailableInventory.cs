namespace Binance.Api.Margin;

/// <summary>
/// Margin available inventory data
/// </summary>
public record BinanceMarginAvailableInventory
{
    /// <summary>
    /// Assets
    /// </summary>
    public Dictionary<string, string> Assets { get; set; } = [];

    /// <summary>
    /// Raw update timestamp reported by Binance. The current schema does not define its unit.
    /// </summary>
    public long UpdateTime { get; set; }
}
