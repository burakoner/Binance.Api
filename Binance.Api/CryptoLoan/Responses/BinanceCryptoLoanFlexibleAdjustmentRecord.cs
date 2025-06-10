namespace Binance.Api.CryptoLoan;

/// <summary>
/// Adjustment History
/// </summary>
public record BinanceCryptoLoanFlexibleAdjustmentRecord
{
    /// <summary>
    /// The loaning asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// The collateral asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// Direction of the adjustment
    /// </summary>
    [JsonProperty("direction")]
    public BinanceCryptoLoanAdjustmentDirection Direction { get; set; }

    /// <summary>
    /// CollateralQuantity
    /// </summary>
    [JsonProperty("collateralAmount")]
    public decimal CollateralQuantity { get; set; }

    /// <summary>
    /// Previous LTV
    /// </summary>
    [JsonProperty("preLTV")]
    public decimal PreviousLTV { get; set; }

    /// <summary>
    /// After LTV
    /// </summary>
    [JsonProperty("afterLTV")]
    public decimal AfterLTV { get; set; }

    /// <summary>
    /// Adjustment Time
    /// </summary>
    [JsonProperty("adjustTime")]
    public DateTime Time { get; set; }
}
