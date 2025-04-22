namespace Binance.Api.Margin;

/// <summary>
/// Binance isolated margin tier data
/// </summary>
public record BinanceIsolatedMarginTier
{
    /// <summary>
    /// Symbol
    /// </summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>
    /// Tier
    /// </summary>
    public int Tier { get; set; }

    /// <summary>
    /// Effective multiple
    /// </summary>
    public decimal? EffectiveMultiple { get; set; } = null;

    /// <summary>
    /// Initial risk ratio
    /// </summary>
    public decimal? InitialRiskRatio { get; set; } = null;

    /// <summary>
    /// Liquidation risk ratio
    /// </summary>
    public decimal? LiquidationRiskRatio { get; set; } = null;

    /// <summary>
    /// Base asset max borrowable
    /// </summary>
    public decimal? BaseAssetMaxBorrowable { get; set; } = null;

    /// <summary>
    /// Quote asset max borrowable
    /// </summary>
    public decimal? QuoteAssetMaxBorrowable { get; set; } = null;
}
