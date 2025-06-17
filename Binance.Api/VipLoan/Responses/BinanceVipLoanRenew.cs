namespace Binance.Api.VipLoan;

/// <summary>
/// VIP Loan Renew
/// </summary>
public record BinanceVipLoanRenew
{
    /// <summary>
    /// Loan Account ID
    /// </summary>
    public long LoanAccountId { get; set; }

    /// <summary>
    /// Loan Asset
    /// </summary>
    [JsonProperty("loanCoin")]
    public string LoanAsset { get; set; } = string.Empty;

    /// <summary>
    /// Loan Quantity
    /// </summary>
    [JsonProperty("loanAmount")]
    public decimal LoanQuantity { get; set; }

    /// <summary>
    /// Collateral Account ID
    /// </summary>
    [JsonProperty("collateralAccountId")]
    public string CollateralAccountId { get; set; } = string.Empty;

    /// <summary>
    /// Collateral Asset
    /// </summary>
    [JsonProperty("collateralCoin")]
    public long CollateralAsset { get; set; }

    /// <summary>
    /// Loan Term in days
    /// </summary>
    public int? LoanTerm { get; set; }
}
