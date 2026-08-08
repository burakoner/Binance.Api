namespace Binance.Api.Margin;

/// <summary>Risk-based liquidation ratio for one Margin asset.</summary>
public record BinanceMarginRiskBasedLiquidationRatio
{
    /// <summary>Margin asset.</summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>Risk-based liquidation ratio.</summary>
    public decimal RiskBasedLiquidationRatio { get; set; }
}
