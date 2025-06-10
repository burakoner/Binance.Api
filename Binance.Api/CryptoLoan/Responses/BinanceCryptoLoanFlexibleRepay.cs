namespace Binance.Api.CryptoLoan;

/// <summary>
/// Repayment info
/// </summary>
public record BinanceCryptoLoanFlexibleRepay
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
    /// Remaining Debt
    /// </summary>
    public decimal RemainingDebt { get; set; }

    /// <summary>
    /// Remaining Collateral
    /// </summary>
    public decimal RemainingCollateral { get; set; }

    /// <summary>
    /// Full Repayment
    /// </summary>
    public bool FullRepayment { get; set; }

    /// <summary>
    /// Current LTV
    /// </summary>
    public decimal CurrentLTV { get; set; }

    /// <summary>
    /// Repay Status
    /// </summary>
    [JsonProperty("repayStatus")]
    public BinanceCryptoLoanFlexibleRepayStatus Status { get; set; }
}
