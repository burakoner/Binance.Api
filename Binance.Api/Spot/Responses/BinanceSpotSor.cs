namespace Binance.Api.Spot;

/// <summary>
/// Smart Order Routing configuration returned by exchange information.
/// </summary>
public record BinanceSpotSor
{
    /// <summary>
    /// Shared base asset.
    /// </summary>
    public string BaseAsset { get; set; } = string.Empty;

    /// <summary>
    /// Symbols in the SOR group.
    /// </summary>
    public List<string> Symbols { get; set; } = [];
}
