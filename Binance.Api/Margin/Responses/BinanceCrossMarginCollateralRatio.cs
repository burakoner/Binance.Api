namespace Binance.Api.Margin;

/// <summary>
/// Cross margin collateral info
/// </summary>
public record BinanceCrossMarginCollateralRatio
{
    /// <summary>
    /// Collaterals
    /// </summary>
    public List<BinanceCrossMarginCollateral> Collaterals { get; set; } = [];

    /// <summary>
    /// Asset names
    /// </summary>
    public List<string> AssetNames { get; set; } = [];
}

/// <summary>
/// Collateral info
/// </summary>
public record BinanceCrossMarginCollateral
{
    /// <summary>
    /// Min usd value
    /// </summary>
    public decimal MinUsdValue { get; set; }

    /// <summary>
    /// Max usd value
    /// </summary>
    public decimal? MaxUsdValue { get; set; }

    /// <summary>
    /// Discount rate
    /// </summary>
    public decimal DiscountRate { get; set; }
}
