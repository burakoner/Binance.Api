namespace Binance.Api.Models.RestApi.Margin;

/// <summary>
/// Isolated margin symbol info
/// </summary>
public record BinanceIsolatedMarginSymbol
{
    /// <summary>
    /// Symbol name
    /// </summary>
    public string Symbol { get; set; } = "";
    /// <summary>
    /// Base asset
    /// </summary>
    public string Base { get; set; } = "";
    /// <summary>
    /// Quote asset
    /// </summary>
    public string Quote { get; set; } = "";
    /// <summary>
    /// Margin trade
    /// </summary>
    public bool IsMarginTrade { get; set; }
    /// <summary>
    /// Is buy allowed
    /// </summary>
    public bool IsBuyAllowed { get; set; }
    /// <summary>
    /// Is sell allowed
    /// </summary>
    public bool IsSellAllowed { get; set; }
}
