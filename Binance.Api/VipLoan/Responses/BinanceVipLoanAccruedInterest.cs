namespace Binance.Api.VipLoan;

/// <summary>
/// VIP Loan Accrued Interest
/// </summary>
public record BinanceVipLoanAccruedInterest
{
    /// <summary>
    /// Loan Asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Principal Quantity
    /// </summary>
    [JsonProperty("principalAmount")]
    public decimal PrincipalQuantity { get; set; }

    /// <summary>
    /// Interest Quantity
    /// </summary>
    [JsonProperty("interestAmount")]
    public decimal InterestQuantity { get; set; }

    /// <summary>
    /// Annual Interest Rate
    /// </summary>
    [JsonProperty("annualInterestRate")]
    public decimal AnnualInterestRate { get; set; }

    /// <summary>
    /// Accrual Time
    /// </summary>
    [JsonProperty("accrualTime")]
    public DateTime AccrualTime { get; set; }

    /// <summary>
    /// Order ID
    /// </summary>
    public long OrderId { get; set; }
}
