namespace Binance.Api.Margin;

/// <summary>Assets currently restricted by Margin risk controls.</summary>
public record BinanceMarginRestrictedAssets
{
    /// <summary>Assets for which opening a long position is restricted.</summary>
    [JsonProperty("openLongRestrictedAsset")]
    public List<string> OpenLongRestrictedAssets { get; set; } = [];

    /// <summary>Assets that exceeded maximum collateral limits.</summary>
    [JsonProperty("maxCollateralExceededAsset")]
    public List<string> MaxCollateralExceededAssets { get; set; } = [];
}
