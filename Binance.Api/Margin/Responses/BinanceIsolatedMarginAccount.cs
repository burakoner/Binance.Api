namespace Binance.Api.Margin;

/// <summary>
/// Isolated margin account info
/// </summary>
public record BinanceIsolatedMarginAccount
{
    /// <summary>
    /// Account assets
    /// </summary>
    public List<BinanceIsolatedMarginAccountSymbol> Assets { get; set; } = [];

    /// <summary>
    /// Total btc asset
    /// </summary>
    public decimal TotalAssetOfBtc { get; set; }

    /// <summary>
    /// Total liability
    /// </summary>
    public decimal TotalLiabilityOfBtc { get; set; }

    /// <summary>
    /// Total net asset
    /// </summary>
    public decimal TotalNetAssetOfBtc { get; set; }
}
