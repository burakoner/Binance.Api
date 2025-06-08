namespace Binance.Api.CryptoLoan;

/// <summary>
/// Collateral asset info
/// </summary>
public record BinanceCryptoLoanCollateralAsset
{
    /// <summary>
    /// Collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// Initial ltv
    /// </summary>
    [JsonProperty("initialLTV")]
    public decimal InitialLtv { get; set; }

    /// <summary>
    /// Margin call ltv
    /// </summary>
    [JsonProperty("marginCallLTV")]
    public decimal MarginCallLtv { get; set; }

    /// <summary>
    /// Liquidation ltv
    /// </summary>
    [JsonProperty("liquidationLTV")]
    public decimal LiquidationLtv { get; set; }

    /// <summary>
    /// Max limit
    /// </summary>
    [JsonProperty("maxLimit")]
    public decimal MaxLimit { get; set; }

    /// <summary>
    /// Vip level
    /// </summary>
    [JsonProperty("vipLevel")]
    public int VipLevel { get; set; }
}
