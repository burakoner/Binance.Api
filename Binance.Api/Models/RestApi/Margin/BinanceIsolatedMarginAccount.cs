namespace Binance.ApiClient.Models.RestApi.Margin;

/// <summary>
/// Isolated margin account info
/// </summary>
public class BinanceIsolatedMarginAccount
{
    /// <summary>
    /// Account assets
    /// </summary>
    public IEnumerable<BinanceIsolatedMarginAccountSymbol> Assets { get; set; } = Array.Empty<BinanceIsolatedMarginAccountSymbol>();
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
