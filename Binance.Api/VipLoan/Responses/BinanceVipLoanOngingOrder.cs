namespace Binance.Api.VipLoan;

/// <summary>
/// VIP Loan Order
/// </summary>
public record BinanceVipLoanOngingOrder
{
    /// <summary>
    /// Order ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Loan Asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Total Debt
    /// </summary>
    [JsonProperty("totalDebt")]
    public decimal TotalDebt { get; set; }

    /// <summary>
    /// Residual Interest
    /// </summary>
    [JsonProperty("residualInterest")]
    public decimal ResidualInterest { get; set; }

    /// <summary>
    /// Collateral Account ID
    /// </summary>
    [JsonProperty("collateralAccountId")]
    public string CollateralAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Collateral Asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public string CollateralAsset { get; set; } = string.Empty;

    /// <summary>
    /// Total Collateral Value After Haircut
    /// </summary>
    [JsonProperty("totalCollateralValueAfterHaircut")]
    public decimal TotalCollateralValueAfterHaircut { get; set; }

    /// <summary>
    /// Locked Collateral Value
    /// </summary>
    [JsonProperty("lockedCollateralValue")]
    public decimal LockedCollateralValue { get; set; }

    /// <summary>
    /// Current LTV (Loan-to-Value)
    /// </summary>
    [JsonProperty("currentLTV")]
    public decimal CurrentLTV { get; set; }

    /// <summary>
    /// Expiration Time
    /// </summary>
    [JsonProperty("expirationTime")]
    public DateTime ExpirationTime { get; set; }

    /// <summary>
    /// Loan Date
    /// </summary>
    [JsonProperty("loanDate")]
    public DateTime LoanDate { get; set; }

    /// <summary>
    /// Loan Term
    /// </summary>
    [JsonProperty("loanTerm")]
    public string LoanTerm { get; set; } = string.Empty;
}
