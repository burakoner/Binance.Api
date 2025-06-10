namespace Binance.Api.CryptoLoan;

/// <summary>
/// Adjustment info
/// </summary>
public record BinanceCryptoLoanFlexibleAdjustment
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
    /// Remaining Debt
    /// </summary>
    [JsonProperty("adjustmentAmount")]
    public decimal AdjustmentQuantity { get; set; }

    /// <summary>
    /// Current LTV
    /// </summary>
    public decimal CurrentLTV { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonProperty("status")]
    public BinanceCryptoLoanFlexibleAdjustmentStatus Status { get; set; }
}
