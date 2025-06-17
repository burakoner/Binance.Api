namespace Binance.Api.VipLoan;

/// <summary>
/// VIP Loan Repay
/// </summary>
public record BinanceVipLoanRepay
{
    /// <summary>
    /// Loan Asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Repay Quantity
    /// </summary>
    [JsonProperty("repayAmount")]
    public decimal RepayQuantity { get; set; }

    /// <summary>
    /// Remaining Principal
    /// </summary>
    [JsonProperty("remainingPrincipal")]
    public decimal RemainingPrincipal { get; set; }

    /// <summary>
    /// Remaining Interest
    /// </summary>
    [JsonProperty("remainingInterest")]
    public decimal RemainingInterest { get; set; }

    /// <summary>
    /// Collateral Asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public long CollateralAsset { get; set; }

    /// <summary>
    /// Current LTV
    /// </summary>
    [JsonProperty("currentLTV")]
    public decimal CurrentLTV { get; set; }

    /// <summary>
    /// Repay Status
    /// </summary>
    [JsonProperty("repayStatus")]
    public BinanceVipLoanStatus RepayStatus { get; set; }
}
