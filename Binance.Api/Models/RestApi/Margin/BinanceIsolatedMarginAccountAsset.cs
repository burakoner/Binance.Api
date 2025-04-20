namespace Binance.Api.Models.RestApi.Margin;

/// <summary>
/// Isolated margin account asset
/// </summary>
public record BinanceIsolatedMarginAccountAsset
{
    /// <summary>
    /// Asset name
    /// </summary>
    public string Asset { get; set; } = "";
    /// <summary>
    /// If borrow is enabled
    /// </summary>
    public bool BorrowEnabled { get; set; }
    /// <summary>
    /// Borrowed
    /// </summary>
    public decimal Borrowed { get; set; }
    /// <summary>
    /// Free
    /// </summary>
    public decimal Free { get; set; }
    /// <summary>
    /// Interest
    /// </summary>
    public decimal Interest { get; set; }
    /// <summary>
    /// Locked
    /// </summary>
    public decimal Locked { get; set; }
    /// <summary>
    /// Net asset
    /// </summary>
    public decimal NetAsset { get; set; }
    /// <summary>
    /// Net asset in btc
    /// </summary>
    public decimal NetAssetOfBtc { get; set; }
    /// <summary>
    /// Is repay enabled
    /// </summary>
    public bool RepayEnabled { get; set; }
    /// <summary>
    /// Total asset
    /// </summary>
    public decimal TotalAsset { get; set; }
}
